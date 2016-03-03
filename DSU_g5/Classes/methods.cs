using System;
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

        public static void bookMember(DateTime date, int timeId, int chosenMid)
        {
            string sqlInsToGame;
            string sqlInsToGM;


            int dateID = 0;

            try
            {
                string sqlGetDateId = "SELECT dates_id FROM game_dates WHERE dates = '"+ date +"'";
                
                NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(sqlGetDateId, conn);
                NpgsqlDataReader dr = cmd.ExecuteReader();

                if(dr.Read())
                {
                    dateID = int.Parse(dr["dates_id"].ToString());
                }

                else
                {
                    Debug.WriteLine("Finns ej detta datum i databasen");
                    return;
                }
                dr.Close();


                //int dID = dateID;

                //Går att byta ut date nedan. Då bör en date-klass skapas som får det värdet istället.
                sqlInsToGame = "INSERT INTO game (date_id, time_id) VALUES (@da, @t) RETURNING game_id";

                sqlInsToGM = "INSERT INTO game_member (game_id, member_id) VALUES (@gId, @mId)";

                NpgsqlCommand cmdInsToGame = new NpgsqlCommand(sqlInsToGame, conn);
                cmdInsToGame.Parameters.AddWithValue("da", dateID);
                cmdInsToGame.Parameters.AddWithValue("t", timeId);

                int gameID = Convert.ToInt32(cmdInsToGame.ExecuteScalar());
                conn.Close(); //kanske stäng



                NpgsqlCommand cmdInsToGameMem = new NpgsqlCommand(sqlInsToGM, conn);
                cmdInsToGameMem.Parameters.AddWithValue("gId", gameID);
                cmdInsToGameMem.Parameters.AddWithValue("mId", chosenMid);

                conn.Open();
                cmdInsToGameMem.ExecuteNonQuery();
                conn.Close();

            }

            catch (NpgsqlException ex)
            {
                Debug.WriteLine(ex.Message);
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
                sql = "SELECT (first_name ||  ' ' ||  last_name) AS namn, id_member AS mID FROM member_new";

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

        public static void addMember(member newMember, users newUser)
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

                // member
                // plsql = plsql + "INSERT INTO member_new (first_name, last_name, address, postal_code,  city, mail, gender, hcp, golf_id, fk_category_id, member_category)";
                // plsql = plsql + " VALUES (:newFirstName, :newLastName, :newAddress, :newPostalCode, :newCity, :newMail, :newGender, :newHcp, :newGolfId, :newFkCategoryId, :newMemberCategori)";
                // plsql = plsql + " RETURNING id_member";

                plsql = plsql + "INSERT INTO member_new (first_name, last_name, address, postal_code,  city, mail, gender, hcp, golf_id, member_category)";
                plsql = plsql + " VALUES (:newFirstName, :newLastName, :newAddress, :newPostalCode, :newCity, :newMail, :newGender, :newHcp, :newGolfId, :newMemberCategori)";
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
                
                command.Parameters.Add(new NpgsqlParameter("newMemberCategori", NpgsqlDbType.Varchar));
                command.Parameters["newMemberCategori"].Value = newMember.category;

                command.CommandText = plsql;
                int id_member = Convert.ToInt32(command.ExecuteScalar());

                // user
                newUser.fkIdMember = id_member;

                plsql = string.Empty;
                plsql = "INSERT INTO users (user_name, user_password, fk_id_member)";
                plsql = plsql + " VALUES (:newUserName, :newUserPassword, :newFkIdMember)";
                plsql = plsql + " RETURNING id_user";

                command.Parameters.Add(new NpgsqlParameter("newUserName", NpgsqlDbType.Varchar));
                command.Parameters["newUserName"].Value = newUser.userName;
                command.Parameters.Add(new NpgsqlParameter("newUserPassword", NpgsqlDbType.Varchar));
                command.Parameters["newUserPassword"].Value = newUser.userPassword;
                command.Parameters.Add(new NpgsqlParameter("newFkIdMember", NpgsqlDbType.Integer));
                command.Parameters["newFkIdMember"].Value = newUser.fkIdMember;

                command.CommandText = plsql;
                int id_user = Convert.ToInt32(command.ExecuteScalar());

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

        public static void removeMember(member newMember, users newUser)
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
                plsql = plsql + "DELETE FROM users WHERE id_user = :newUser.IdUser";
                command.Parameters.Add(new NpgsqlParameter("newUser.IdUser", NpgsqlDbType.Integer));
                command.Parameters["newUser.IdUser"].Value = newUser.idUser;
                command.CommandText = plsql;
                int id_user = Convert.ToInt32(command.ExecuteScalar());

                plsql = string.Empty;
                plsql = plsql + "DELETE FROM member_new WHERE id_member = :newMember.memberId";
                command.Parameters.Add(new NpgsqlParameter("newMember.memberId", NpgsqlDbType.Integer));
                command.Parameters["newMember.memberId"].Value = newMember.memberId;
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
        public static member getMember(int id_member)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
            member newMember = new member();
            try
            {
                conn.Open();
                string plsql = string.Empty;
                plsql = "SELECT id_member, first_name, last_name, address, postal_code, city, mail, gender, hcp, golf_id, fk_category_id, member_category FROM member_new WHERE id_member = :newIdMember;";
                NpgsqlCommand command = new NpgsqlCommand(@plsql, conn);
                command.Parameters.Add(new NpgsqlParameter("newIdMember", NpgsqlDbType.Integer));
                command.Parameters["newIdMember"].Value = id_member;
                NpgsqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    newMember.memberId = (int)(dr["id_member"]);
                    newMember.firstName = (string)(dr["first_name"]);
                    newMember.lastName = (string)(dr["last_name"]);
                    newMember.address = (string)(dr["address"]);
                    newMember.postalCode = (string)(dr["postal_code"]);
                    newMember.city = (string)(dr["city"]);
                    newMember.mail = (string)(dr["mail"]);
                    newMember.gender = (string)(dr["gender"]);
                    newMember.hcp = (double)(dr["hcp"]);
                    newMember.golfId = (string)(dr["golf_id"]);
                    // newMember.categoryId = (int)(dr["fk_category_id"]);
                    newMember.category = (string)(dr["member_category"]);
                }
            }
            finally
            {
                conn.Close();
            }
            return newMember;
        }

        public static users getUser(int fk_id_member)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
            users newUser = new users();
            try
            {
                conn.Open();
                string plsql = string.Empty;

                plsql = "SELECT id_user, user_name, user_password, fk_id_member FROM users WHERE fk_id_member = :newFkIdMember;";
                NpgsqlCommand command = new NpgsqlCommand(@plsql, conn);
                command.Parameters.Add(new NpgsqlParameter("newFkIdMember", NpgsqlDbType.Integer));
                command.Parameters["newFkIdMember"].Value = fk_id_member;
                NpgsqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    newUser.idUser = (int)(dr["id_user"]);
                    newUser.userName = (string)(dr["user_name"]);
                    newUser.userPassword = (string)(dr["user_password"]);
                    newUser.fkIdMember = (int)(dr["fk_id_member"]);
                }
            }
            finally
            {
                conn.Close();
            }
            return newUser;
        }

        public static List<member_category> getMemberCategoryList()
        {
            List<member_category> memberCategoryList = new List<member_category>();
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
            users newUser = new users();
            try
            {
                conn.Open();
                string plsql = string.Empty;
                plsql = "SELECT category_id, category, cleaningfee FROM member_category;";
                NpgsqlCommand command = new NpgsqlCommand(@plsql, conn);
                NpgsqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    member_category nyMemberCategory = new member_category();
                    nyMemberCategory.categoryId = (int)(dr["category_id"]);
                    nyMemberCategory.category = (string)(dr["category"]);
                    nyMemberCategory.cleaningFee = (int)(dr["cleaningfee"]);
                    memberCategoryList.Add(nyMemberCategory);
                }
            }
            finally
            {
                conn.Close();
            }
            return memberCategoryList;
        }

        public static List<games> getGamesByDate(DateTime selectedDate)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
            List<games> gameList = new List<games>();

            string date = selectedDate.ToString().Split(' ')[0];
            string sql = "";
            try
            {
                sql = "SELECT game_id, dates, times, game_starts.time_id "+
                      "FROM game, game_dates, game_starts "+
                      "WHERE game.date_id = game_dates.dates_id "+
                      "AND game.time_id = game_starts.time_id "+
                      "AND dates = '" + date + "';";

                conn.Open();

                NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
                NpgsqlDataReader dr = cmd.ExecuteReader();
               
                while (dr.Read())
                {
                    games g = new games();
                    g.date = DateTime.Parse(dr["dates"].ToString());
                    g.time = Convert.ToDateTime(dr["times"].ToString());
                    g.timeId = int.Parse(dr["time_id"].ToString());
                    int gameId = int.Parse(dr["game_id"].ToString());
                    g.memberInGameList = getBookedMembersByGameId(gameId);

                    gameList.Add(g);
                }
            }
            catch (NpgsqlException ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return gameList;
        }

        public static List<member> getBookedMembersByGameId(int gameId)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
            List<member> memberList = new List<member>();

            string sql = "";
            try
            {
                sql = "SELECT id_member, first_name, last_name, address, postal_code, city, mail, gender, hcp, golf_id, member_category "+
                      "FROM game, game_member, member_new "+
                      "WHERE game.game_id = game_member.game_id "+
                      "AND game_member.member_id = member_new.id_member "+
                      "AND game.game_id = "+ gameId +";";

                conn.Open();

                NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
                NpgsqlDataReader dr = cmd.ExecuteReader();
                
                while (dr.Read())
                {
                    member m = new member();
                    m.memberId = int.Parse(dr["id_member"].ToString());
                    m.firstName = dr["first_name"].ToString();
                    m.lastName = dr["last_name"].ToString();
                    m.gender = dr["gender"].ToString();
                    m.hcp = double.Parse(dr["hcp"].ToString());

                    memberList.Add(m);
                }
            }
            catch (NpgsqlException ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return memberList;
        }


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

        public static List<news> getNewsList()
        {
            List<news> newsList = new List<news>();
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
            //news newNews = new news();
            try
            {
                conn.Open();
                string sql = string.Empty;
                sql = "SELECT news_id,news_info,news_name FROM news;";
                NpgsqlCommand command = new NpgsqlCommand(@sql, conn);
                NpgsqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    news newNews = new news();
                    newNews.newsId = (int)(dr["news_id"]);
                    newNews.newsInfo = (string)(dr["news_info"]);
                    newNews.newsName = (string)(dr["news_name"]);
                    newsList.Add(newNews);
                }
            }
            finally
            {
                conn.Close();
            }
            return newsList;
        }


    }

}

    



