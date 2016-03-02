using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Npgsql;
using NpgsqlTypes;
using System.Configuration;
using System.Diagnostics;

namespace DSU_g5
{

    public static class methods
    {
        public static List<member> getBookedMember(DateTime selectedDate)
        {

            //ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["Halslaget"];
            //NpgsqlConnection conn = new NpgsqlConnection(settings.ConnectionString);

            //NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);



            //NpgsqlConnection conn = new NpgsqlConnection("Server=webblabb.miun.se;Port=5432;Database=dsu_g5;User Id=dsu_g5;Password=dsu_g5;SSL=true");

            //List<member> bookingmembers = new List<member>();








            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
            List<member> bookingmembers = new List<member>();
            member m;
            game_starts gs;
            game_dates gd;

            string sql = "";
            try
            {
                sql = "SELECT first_name, last_name, gender, g.member_id, hcp, times, dates " +
                        "FROM member_new m " +
                        "INNER JOIN games g on g.member_id = m.id_member " +
                        "INNER JOIN game_dates gd ON g.date_id = gd.dates_id " +
                        "INNER JOIN game_starts gs ON g.time_id = gs.time_id " +
                        "WHERE gd.dates = '2016-03-05' " +
                        "GROUP BY m.first_name, m.last_name, m.gender, g. member_id, m.hcp, gs.times, dates";
                conn.Open();

                NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
                NpgsqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    m = new member();
                    m.memberId = int.Parse(dr["member_id"].ToString());
                    m.firstName = dr["first_name"].ToString();
                    m.lastName = dr["last_name"].ToString();
                    m.gender = dr["gender"].ToString();
                    m.hcp = double.Parse(dr["hcp"].ToString());

                    gs = new game_starts();
                    gs.times = Convert.ToDateTime(dr["times"].ToString());

                    gd = new game_dates();
                    gd.dates = DateTime.Parse(dr["dates"].ToString());


                    bookingmembers.Add(m);

                }

            }
            catch (NpgsqlException ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return bookingmembers;
        }

        public static void addMember(member newMember)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
            NpgsqlTransaction tran = null;

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = conn;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                command.Connection = conn;
                command.Transaction = tran;
                string plsql = string.Empty;
                plsql = plsql + "INSERT INTO member_new (first_name, last_name, address, postal_code,  city, mail, gender, hcp, golf_id, fk_category_id, member_category)";
                plsql = plsql + " VALUES (:newFirstName, :newLastName, :newAddress, :newPostalCode, :newCity, :newMail, :newGender, :newHcp, :newGolfId, :newFkCategoryId, :newMemberCategori)";
                plsql = plsql + " RETURNING id_member";

                command.Parameters.Add(new NpgsqlParameter("newFirstName", NpgsqlDbType.Varchar));
                command.Parameters["newFirstName"].Value = newMember.firstName;
                command.Parameters.Add(new NpgsqlParameter("newLastName", NpgsqlDbType.Varchar));
                command.Parameters["newLastName"].Value = newMember.lastName;
                command.Parameters.Add(new NpgsqlParameter("newAddress", NpgsqlDbType.Varchar));
                command.Parameters["newAddress"].Value = newMember.address;
                command.Parameters.Add(new NpgsqlParameter("newPostalCode", NpgsqlDbType.Varchar));
                command.Parameters["newPostalCode"].Value = newMember.postalCode;
                command.Parameters.Add(new NpgsqlParameter("newCity", NpgsqlDbType.Varchar));
                command.Parameters["newCity"].Value = newMember.city;
                command.Parameters.Add(new NpgsqlParameter("newMail", NpgsqlDbType.Varchar));
                command.Parameters["newMail"].Value = newMember.mail;
                command.Parameters.Add(new NpgsqlParameter("newGender", NpgsqlDbType.Varchar));
                command.Parameters["newGender"].Value = newMember.gender;
                command.Parameters.Add(new NpgsqlParameter("newHcp", NpgsqlDbType.Double));
                command.Parameters["newHcp"].Value = newMember.hcp;
                command.Parameters.Add(new NpgsqlParameter("newGolfId", NpgsqlDbType.Varchar));
                command.Parameters["newGolfId"].Value = newMember.golfId;

                command.Parameters.Add(new NpgsqlParameter("newFkCategoryId", NpgsqlDbType.Integer));
                command.Parameters["newFkCategoryId"].Value = newMember.categoryId;

                // Ej obligatorisk
                command.Parameters.Add(new NpgsqlParameter("newMemberCategori", NpgsqlDbType.Varchar));
                command.Parameters["newMemberCategori"].Value = newMember.category;

                command.CommandText = plsql;
                int id_member = Convert.ToInt32(command.ExecuteScalar());
                tran.Commit();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                tran.Rollback();
            }
            finally
            {
                conn.Close();
            }

        }



        //public static void addNews(string newsinfo)
        //{
        //    NpgsqlConnection conn1 = new NpgsqlConnection("Server=webblabb.miun.se;Port=5432;Database=pgmvaru_g4;User Id=pgmvaru_g4;Password=trapets;ssl=true");

        //    try
        //    {
        //        conn1.Open();


        //        NpgsqlCommand command1 = new NpgsqlCommand(@"INSERT INTO news(newsInfo) VALUES (:newNewsInfo)", conn);


        //        command1.Parameters.Add(new NpgsqlParameter("newNewsInfo", DbType.varchar));
        //        command1.Parameters[0].Value = newsInfo;





        public static void addNews(news newNews)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
            NpgsqlTransaction trans = null;

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = conn;

            try
            {
                string sql = string.Empty;
                conn.Open();
                trans = conn.BeginTransaction();
                command.Connection = conn;
                command.Transaction = trans;
                sql = "INSERT INTO news (news_info) VALUES(:newNewsInfo) RETURNING news_id";

                command.Parameters.Add(new NpgsqlParameter("newNewsInfo", NpgsqlDbType.Varchar));
                command.Parameters["newNewsInfo"].Value = newNews.newsInfo;

                command.CommandText = sql;
                int newsID = Convert.ToInt32(command.ExecuteScalar());
                trans.Commit();
                int numberOfAffectedRows = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                trans.Rollback();
            }
            finally
            {
                conn.Close();
            }
        }
        public static void updateNews(news newNews)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
            NpgsqlTransaction trans = null;

            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = conn;
            try
            {
                string sql = string.Empty;
                conn.Open();
                trans = conn.BeginTransaction();
                command.Connection = conn;
                command.Transaction = trans;
                sql = "UPDATE news SET news_info = :newNewsInfo WHERE news_id = :newNewsId RETURNING news_id";

                command.Parameters.Add(new NpgsqlParameter("newNewsInfo", NpgsqlDbType.Varchar));
                command.Parameters["newNewsInfo"].Value = newNews.newsInfo;
                command.Parameters.Add(new NpgsqlParameter("newNewsId", NpgsqlDbType.Integer));
                command.Parameters["newNewsId"].Value = newNews.newsId; 

                command.CommandText = sql;
                int news_id = Convert.ToInt32(command.ExecuteScalar());
                trans.Commit();
                int numberOfAffectedRows = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                trans.Rollback();
            }
            finally
            {
                conn.Close();
            }
        }
    }
}



    



