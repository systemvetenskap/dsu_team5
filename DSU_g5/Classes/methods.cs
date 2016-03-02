﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Npgsql;
using NpgsqlTypes;
using System.Configuration;
using System.Diagnostics;
using System.Data;

namespace DSU_g5
{

    public static class methods
    {
        public static void bookMember(DateTime date, int timeId, member chosenM)
        {
            string sqlInsToGame;
            string sqlInsToGM;

            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);

            try
            {
                sqlInsToGame = "INSERT INTO game (date_id, time_id) VALUES((SELECT dates_id FROM game_dates WHERE dates = '" + date + "'), '" + timeId + "') RETURNING id";


            }

            catch (Exception ex)
            {

            }
        }

        public static List<member> getBookedMember(DateTime selectedDate)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
            List<member> bookingmembers = new List<member>();
            member m;
            game_starts gs;
            game_dates gd;

            string sql = "";
            try
            {
                sql = "SELECT first_name, last_name, gender, gm.member_id, hcp, times, dates " +
                        "FROM member_new m " +
                        "INNER JOIN game_member gm ON gm.member_id = m.id_member " +
                        "INNER JOIN game g ON g.game_id = gm.game_id " +
                        "INNER JOIN game_dates gd ON gd.dates_id = g.date_id " +
                        "INNER JOIN game_starts gs ON gs.time_id = g.time_id " +
                        "WHERE gd.dates = '"+selectedDate+"' " +
                        "GROUP BY m.first_name, m.last_name, m.gender, gm.member_id, m.hcp, gs.times, dates";
                conn.Open();

                NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
                NpgsqlDataReader dr = cmd.ExecuteReader();

                while(dr.Read())
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

        
        
        //Admin får se alla medlemmar i en lista. Möjliggör för att lägga in personer på bokning.
        public static DataTable showAllMembersForBooking()
        {
            //GÖR DT i metoden.
            //NpgsqlDataAdapter istället för Command
            //använda value

            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);

            string sql;

            DataTable dt = new DataTable();
           
            try
            {
                sql = "SELECT (first_name ||  ' ' ||  last_name) AS namn, id_member AS mID  FROM member_new";

                conn.Open();

                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, conn);
                da.Fill(dt);
            }

            catch (NpgsqlException ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return dt;

            //List<member> membersForBookingList = new List<member>();
            //member m;
            //string sql;

            //try
            //{
            //    sql = "SELECT * FROM member_new ORDER BY id_member ASC";

            //    conn.Open();

            //    NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
            //    NpgsqlDataReader dr = cmd.ExecuteReader();

            //    while(dr.Read())
            //    {
            //        m = new member();
            //        m.memberId = int.Parse(dr["id_member"].ToString());
            //        m.firstName = dr["first_name"].ToString();
            //        m.lastName = dr["last_name"].ToString();
            //        m.hcp = double.Parse(dr["hcp"].ToString());
            //        m.gender = dr["gender"].ToString();

            //        membersForBookingList.Add(m);
            //    }

            //}

            //catch (NpgsqlException ex)
            //{
            //    Debug.WriteLine(ex.Message);
            //}

            //return membersForBookingList;
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
    }
}
    



