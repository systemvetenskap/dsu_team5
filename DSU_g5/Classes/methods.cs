using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Npgsql;
using NpgsqlTypes;
using System.Configuration;
using System.Diagnostics;
using System.Data;
using System.Net.Mail;


namespace DSU_g5
{

    public static class methods
    {
        #region BOKNING OCH AVBOKNING - MEDLEM

        public static void bookingByMember(DateTime date, int timeId, int playerId, int bookedById)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);


            string sqlMemberAlreadyBooked;



            double hcp = 0;
            double hcpNew = 0;
            int antal = 0;
            int dateId = 0;
            int userCount = 0;

            string sqlfirst;
            string sqlmembercount;
            string sqlHcpCount;
            string sqlHcpCountNew;

            try
            {
                sqlfirst = "SELECT dates_id FROM game_dates WHERE dates = '" + date + "'";
                conn.Open();
                NpgsqlCommand cmdfirst = new NpgsqlCommand(sqlfirst, conn);
                NpgsqlDataReader drfirst = cmdfirst.ExecuteReader();
                while (drfirst.Read())
                {
                    dateId = int.Parse(drfirst["dates_id"].ToString());
                }
                conn.Close();



                try
                {
                    sqlMemberAlreadyBooked = "SELECT * FROM game_member gm " +
                                             "INNER JOIN game g " +
                                             "ON gm.game_id = g.game_id " +
                                             "WHERE date_id = '" + dateId + "' AND time_id = '" + timeId + "' AND member_id = '" + playerId + "'";
                    conn.Open();
                    NpgsqlCommand cmdAlreadyBooked = new NpgsqlCommand(sqlMemberAlreadyBooked, conn);
                    NpgsqlDataReader drAlreadyBooked = cmdAlreadyBooked.ExecuteReader();


                    //userCount = Convert.ToInt32(cmdAlreadyBooked.ExecuteScalar());

                    if (drAlreadyBooked.HasRows)
                    {
                        //Medlem finns redan.
                        HttpContext.Current.Response.Write("Medlem finns redan inbokad på detta datum och tid.");
                    }

                    else
                    {
                        //Medlem finns inte. Kör på!

                        sqlmembercount = "SELECT COUNT (member_id) as antal FROM game_member gm INNER JOIN game g ON g.game_id = gm.game_id WHERE g.date_id = '" + dateId + "' AND time_id = '" + timeId + "'";
                        //conn.Open();
                        NpgsqlCommand cmdsecond = new NpgsqlCommand(sqlmembercount, conn);
                        NpgsqlDataReader drsecond = cmdsecond.ExecuteReader();
                        while (drsecond.Read())
                        {
                            antal = int.Parse(drsecond["antal"].ToString());
                        }
                        conn.Close();


                        sqlHcpCount = "SELECT SUM (hcp) as hcp FROM member_new m INNER JOIN game_member gm ON m.id_member = gm.member_id INNER JOIN game g ON g.game_id = gm.game_id WHERE g.date_id = '" + dateId + "' AND g.time_id = '" + timeId + "'";
                        conn.Open();
                        NpgsqlCommand cmdthird = new NpgsqlCommand(sqlHcpCount, conn);
                        NpgsqlDataReader drthird = cmdthird.ExecuteReader();
                        
                        //if(drthird.IsDBNull(Convert.ToInt32("hcp")))
                        //if(drthird.Has)
                        //{
                        //    Debug.WriteLine("Nullvärde!");
                        //    hcp = 0;
                        //}


                        //if(drthird["hcp"] != DBNull.Value)
                        //{
                        //    hcp = double.Parse(drthird["hcp"].ToString());
                        //}
                        //else
                        //{
                        //    hcp = 0;
                        //}

                        while (drthird.Read())
                        {
                            try
                            {
                                hcp = double.Parse(drthird["hcp"].ToString());
                            }
                            catch
                            {
                                hcp = 0;
                            }
                        }

                        //if(drthird.Read())
                        //{
                        //    hcp = double.Parse(drthird["hcp"].ToString());
                        //}
                        //else
                        //{
                        //    hcp = 0;
                        //}


                        conn.Close();


                        sqlHcpCountNew = "SELECT hcp as handicap FROM member_new WHERE id_member = '" + playerId + "';";
                        conn.Open();
                        NpgsqlCommand cmdforth = new NpgsqlCommand(sqlHcpCountNew, conn);
                        NpgsqlDataReader drforth = cmdforth.ExecuteReader();
                        while (drforth.Read())
                        {
                            hcpNew = double.Parse(drforth["handicap"].ToString());
                        }
                        conn.Close();



                        if (hcp + hcpNew <= 100 && antal < 4)
                        {
                            string sqlInsToGame; //SQLsträng för att skapa rad i game-tabellen.
                            string sqlInsToGM;  //SQLsträng för att skapa rad i game_member-tabellen.
                            int dateID = 0; //DateID som får värde efter att datumet kollats mot tabellen.


                            try
                            {
                                string sqlGetDateId = "SELECT dates_id FROM game_dates WHERE dates = '" + date + "'";

                                conn.Open();
                                NpgsqlCommand cmd = new NpgsqlCommand(sqlGetDateId, conn);
                                NpgsqlDataReader dr = cmd.ExecuteReader();

                                if (dr.Read())
                                {
                                    dateID = int.Parse(dr["dates_id"].ToString());
                                }

                                else
                                {
                                    Debug.WriteLine("Finns ej detta datum i databasen");
                                    return;
                                }
                                dr.Close();


                                //Går att byta ut date nedan. Då bör en date-klass skapas som får det värdet istället.
                                sqlInsToGame = "INSERT INTO game (date_id, time_id) VALUES (@da, @t) RETURNING game_id";

                                sqlInsToGM = "INSERT INTO game_member (game_id, member_id, booked_by) VALUES (@gId, @mId, @bb)";


                                NpgsqlCommand cmdInsToGame = new NpgsqlCommand(sqlInsToGame, conn);
                                cmdInsToGame.Parameters.AddWithValue("da", dateID);
                                cmdInsToGame.Parameters.AddWithValue("t", timeId);

                                int gameID = Convert.ToInt32(cmdInsToGame.ExecuteScalar()); // Returnerar game_id som används i nästa query.
                                conn.Close(); //kanske stäng



                                NpgsqlCommand cmdInsToGameMem = new NpgsqlCommand(sqlInsToGM, conn);
                                cmdInsToGameMem.Parameters.AddWithValue("gId", gameID);
                                cmdInsToGameMem.Parameters.AddWithValue("mId", playerId);
                                cmdInsToGameMem.Parameters.AddWithValue("bb", bookedById);

                                conn.Open();
                                cmdInsToGameMem.ExecuteNonQuery();
                                conn.Close();

                            }

                            catch (NpgsqlException ex)
                            {
                                Debug.WriteLine(ex.Message);
                                conn.Close();
                            }

                            finally
                            {
                                conn.Close();
                            }
                        }

                        else
                        {
                            HttpContext.Current.Response.Write("Antal deltagare eller för högt handicap");

                        }

                    }
                }

                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    conn.Close();
                }

                finally
                {
                    conn.Close();
                }
            }


            catch (NpgsqlException ex)
            {
                Debug.WriteLine(ex.Message);
                conn.Close();
            }

            finally
            {
                conn.Close();
            }

        }



        #endregion











        #region BOKNING OCH AVBOKNING AV MEDLEMMAR - ADMIN


        public static void bookMember(DateTime date, int timeId, int chosenMid)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);


            string sqlMemberAlreadyBooked;



            double hcp = 0;
            double hcpNew = 0;
            int antal = 0;
            int dateId = 0;
            int userCount = 0;

            string sqlfirst;
            string sqlmembercount;
            string sqlHcpCount;
            string sqlHcpCountNew;

            try
            {
                sqlfirst = "SELECT dates_id FROM game_dates WHERE dates = '" + date + "'";
                conn.Open();
                NpgsqlCommand cmdfirst = new NpgsqlCommand(sqlfirst, conn);
                NpgsqlDataReader drfirst = cmdfirst.ExecuteReader();
                while (drfirst.Read())
                {
                    dateId = int.Parse(drfirst["dates_id"].ToString());
                }
                conn.Close();



                try
                {
                    sqlMemberAlreadyBooked = "SELECT * FROM game_member gm " +
                                             "INNER JOIN game g " +
                                             "ON gm.game_id = g.game_id " +
                                             "WHERE date_id = '" + dateId + "' AND time_id = '" + timeId + "' AND member_id = '" + chosenMid + "'";
                    conn.Open();
                    NpgsqlCommand cmdAlreadyBooked = new NpgsqlCommand(sqlMemberAlreadyBooked, conn);
                    NpgsqlDataReader drAlreadyBooked = cmdAlreadyBooked.ExecuteReader();


                    //userCount = Convert.ToInt32(cmdAlreadyBooked.ExecuteScalar());

                    if (drAlreadyBooked.HasRows)
                    {
                        //Medlem finns redan.
                        HttpContext.Current.Response.Write("Medlem finns redan inbokad på detta datum och tid.");
                    }

                    else
                    {
                        //Medlem finns inte. Kör på!

                        sqlmembercount = "SELECT COUNT (member_id) as antal FROM game_member gm INNER JOIN game g ON g.game_id = gm.game_id WHERE g.date_id = '" + dateId + "' AND time_id = '" + timeId + "'";
                        //conn.Open();
                        NpgsqlCommand cmdsecond = new NpgsqlCommand(sqlmembercount, conn);
                        NpgsqlDataReader drsecond = cmdsecond.ExecuteReader();
                        while (drsecond.Read())
                        {
                            antal = int.Parse(drsecond["antal"].ToString());
                        }
                        conn.Close();


                        sqlHcpCount = "SELECT SUM (hcp) as hcp FROM member_new m INNER JOIN game_member gm ON m.id_member = gm.member_id INNER JOIN game g ON g.game_id = gm.game_id WHERE g.date_id = '" + dateId + "' AND g.time_id = '" + timeId + "'";
                        conn.Open();
                        NpgsqlCommand cmdthird = new NpgsqlCommand(sqlHcpCount, conn);
                        NpgsqlDataReader drthird = cmdthird.ExecuteReader();
                        while (drthird.Read())
                        {
                            hcp = double.Parse(drthird["hcp"].ToString());
                        }
                        conn.Close();


                        sqlHcpCountNew = "SELECT hcp as handicap FROM member_new WHERE id_member = '" + chosenMid + "';";
                        conn.Open();
                        NpgsqlCommand cmdforth = new NpgsqlCommand(sqlHcpCountNew, conn);
                        NpgsqlDataReader drforth = cmdforth.ExecuteReader();
                        while (drforth.Read())
                        {
                            hcpNew = double.Parse(drforth["handicap"].ToString());
                        }
                        conn.Close();



                        if (hcp + hcpNew <= 100 && antal < 4)
                        {
                            string sqlInsToGame; //SQLsträng för att skapa rad i game-tabellen.
                            string sqlInsToGM;  //SQLsträng för att skapa rad i game_member-tabellen.
                            int dateID = 0; //DateID som får värde efter att datumet kollats mot tabellen.


                            try
                            {
                                string sqlGetDateId = "SELECT dates_id FROM game_dates WHERE dates = '" + date + "'";

                                conn.Open();
                                NpgsqlCommand cmd = new NpgsqlCommand(sqlGetDateId, conn);
                                NpgsqlDataReader dr = cmd.ExecuteReader();

                                if (dr.Read())
                                {
                                    dateID = int.Parse(dr["dates_id"].ToString());
                                }

                                else
                                {
                                    Debug.WriteLine("Finns ej detta datum i databasen");
                                    return;
                                }
                                dr.Close();


                                //Går att byta ut date nedan. Då bör en date-klass skapas som får det värdet istället.
                                sqlInsToGame = "INSERT INTO game (date_id, time_id) VALUES (@da, @t) RETURNING game_id";

                                sqlInsToGM = "INSERT INTO game_member (game_id, member_id) VALUES (@gId, @mId)";


                                NpgsqlCommand cmdInsToGame = new NpgsqlCommand(sqlInsToGame, conn);
                                cmdInsToGame.Parameters.AddWithValue("da", dateID);
                                cmdInsToGame.Parameters.AddWithValue("t", timeId);

                                int gameID = Convert.ToInt32(cmdInsToGame.ExecuteScalar()); // Returnerar game_id som används i nästa query.
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
                                conn.Close();
                            }

                            finally
                            {
                                conn.Close();
                            }
                        }

                        else
                        {
                            HttpContext.Current.Response.Write("Antal deltagare eller för högt handicap");

                        }

                    }
                }

                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    conn.Close();
                }

                finally
                {
                    conn.Close();
                }
            }


            catch (NpgsqlException ex)
            {
                Debug.WriteLine(ex.Message);
                conn.Close();
            }

            finally
            {
                conn.Close();
            }

        }


        public static void unBookMember(DateTime date, int timeId, int chosenMemberId)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
            
            int dateId = 0;
            int gameId = 0;

            List<game> gameList = new List<game>();


            string sqlGetDateId = "SELECT dates_id FROM game_dates WHERE dates = '" + date + "'";


            try
            {
                conn.Open();

                NpgsqlCommand cmdGetDateId = new NpgsqlCommand(sqlGetDateId, conn);
                NpgsqlDataReader dr = cmdGetDateId.ExecuteReader();

                if (dr.Read())
                {
                    dateId = int.Parse(dr["dates_id"].ToString());
            }
                else
                {
                    Debug.WriteLine("Finns ej detta datum_id i databasen");
                    return;
                }
                dr.Close();
                conn.Close();


                conn.Open();
                string sqlGetGameId = "SELECT game_id FROM game WHERE date_id = '" + dateId + "' AND time_id = '" + timeId + "'";
                NpgsqlCommand cmdGetGameId = new NpgsqlCommand(sqlGetGameId, conn);
                NpgsqlDataReader dRead = cmdGetGameId.ExecuteReader();
                
                if (dRead.HasRows)
                {
                    while (dRead.Read())
                    {
                        game g = new game();
                        g.game_id = int.Parse(dRead["game_id"].ToString());
                        gameList.Add(g);

                        //gameId = int.Parse(dRead["game_id"].ToString()); - FUNKAR OM DET ÄR 1 SPELARE PÅ EN BOKNING.
                    }
                }
                else
                {
                    Debug.WriteLine("Finns ej detta game_id i databasen");
                    return;
                }
                dRead.Close();


                //Tar bort rad från game_member.
                foreach (game game in gameList)
                {
                    string sqlDelFromGM = "DELETE FROM game_member WHERE game_id = '" + game.game_id + "' AND member_id = '" + chosenMemberId + "'";
                NpgsqlCommand cmdDelGM = new NpgsqlCommand(sqlDelFromGM, conn);
                cmdDelGM.ExecuteNonQuery();
                }



                //Tar bort om det finns ett game_id UTAN en spelare.
                string sqlDelNonUsedGameID = "DELETE FROM game g WHERE g.game_id NOT IN (SELECT gm.game_id FROM game_member gm)";
                NpgsqlCommand cmdDelGameID = new NpgsqlCommand(sqlDelNonUsedGameID, conn);
                cmdDelGameID.ExecuteNonQuery();

                conn.Close();

            }

            catch (NpgsqlException ex)
            {
                Debug.WriteLine(ex.Message);
                conn.Close();
        }

            finally
            {
                conn.Close();
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
                        "WHERE gd.dates = '" + selectedDate + "' " +
                        "GROUP BY m.first_name, m.last_name, m.gender, gm.member_id, m.hcp, gs.times, dates";
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
            finally
            {
                conn.Close();
            }

            return bookingmembers;
                }
                
        //Admin får se alla medlemmar i en lista. Möjliggör för att lägga in personer på bokning.
        public static DataTable showAllMembersForBooking()
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);

            string sql;

            DataTable dt = new DataTable();
           
            try
            {
                sql = "SELECT (first_name ||  ' ' ||  last_name) AS namn, id_member AS mID FROM member_new"; //first_name och last_name blir en egen kolumn som heter 'name'.

                conn.Open();

                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, conn);
                da.Fill(dt); //Fyller dataAdatpter med dataTable.
            }
            catch (NpgsqlException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            
            return dt;
        }

        //Returnerar en datatable med medlemmar inbokade på en viss tid
        public static DataTable showAllMembersForBookingByDateAndTime(DateTime datum, int timeID)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);

            string sql;
            string date = datum.ToShortDateString();
            DataTable dt = new DataTable();

            try
            {
                sql = "SELECT (first_name ||  ' ' ||  last_name) AS namn, id_member AS mID " +
                      "FROM member_new, game_member, game, game_dates, game_starts " +
                      "WHERE member_new.id_member = game_member.member_id " +
                      "AND game_member.game_id = game.game_id " +
                      "AND game.time_id = game_starts.time_id " +
                      "AND game.date_id = game_dates.dates_id " +
                      "AND game_starts.time_id = " + timeID + " " +
                      "AND game_dates.dates = '" + date + "';";

                conn.Open();

                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, conn);
                da.Fill(dt); //Fyller dataAdatpter med dataTable.
            }
            catch (NpgsqlException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return dt;
        }

        #endregion

        #region MEDLEMSSIDA
        
        public static bool addMember(member newMember, users newUser)
        {
            bool succesfull = false;
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

                plsql = plsql + "INSERT INTO member_new(";
                plsql = plsql + "first_name, last_name, address, postal_code, city, ";
                plsql = plsql + "mail, gender, hcp, golf_id, member_category, fk_category_id, ";
                plsql = plsql + "access_id, payment, access_category) ";
                plsql = plsql + "VALUES (:newFirstName, :newLastName, :newAddress, :newPostalCode, :newCity, ";
                plsql = plsql + ":newMail, :newGender, :newHcp, :newGolfId, :newMemberCategory, :newFkCategoryId, ";
                plsql = plsql + ":newAccessId, :newPayment, :newAccessCategory) ";
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
                command.Parameters.Add(new NpgsqlParameter("newMemberCategory", NpgsqlDbType.Varchar));
                command.Parameters["newMemberCategory"].Value = newMember.category;
                command.Parameters.Add(new NpgsqlParameter("newAccessId", NpgsqlDbType.Integer));
                command.Parameters["newAccessId"].Value = newMember.accessId;
                command.Parameters.Add(new NpgsqlParameter("newAccessCategory", NpgsqlDbType.Varchar));
                command.Parameters["newAccessCategory"].Value = newMember.accessCategory;
                command.Parameters.Add(new NpgsqlParameter("newPayment", NpgsqlDbType.Boolean));
                command.Parameters["newPayment"].Value = newMember.payment;
                
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
                succesfull = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                succesfull = false;
                tran.Rollback();
            }
            finally
            {
                conn.Close();
            }
            return succesfull;
        }

        public static bool modifyMember(member newMember, users newUser)
        {
            bool succesfull = false;
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

                plsql = plsql + "UPDATE member_new ";
                plsql = plsql + " SET first_name = :newFirstName, ";
                plsql = plsql + " last_name = :newLastName, "; 
                plsql = plsql + " address = :newAddress, ";
                plsql = plsql + " postal_code = :newPostalCode, ";
                plsql = plsql + " city =:newCity, ";
                plsql = plsql + " mail = :newMail, ";
                plsql = plsql + " gender = :newGender,";
                plsql = plsql + " hcp = :newHcp, ";
                plsql = plsql + " golf_id =:newGolfId ,";
                plsql = plsql + " member_category = :newMemberCategory, ";
                plsql = plsql + " fk_category_id = :newFkCategoryId, ";
                plsql = plsql + " access_id = :newAccessId,"; 
                plsql = plsql + " payment = :newPayment, ";
                plsql = plsql + " access_category = :newAccessCategory ";
                plsql = plsql + "WHERE id_member = :newIdMember"; 
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
                command.Parameters.Add(new NpgsqlParameter("newMemberCategory", NpgsqlDbType.Varchar));
                command.Parameters["newMemberCategory"].Value = newMember.category;
                command.Parameters.Add(new NpgsqlParameter("newAccessId", NpgsqlDbType.Integer));
                command.Parameters["newAccessId"].Value = newMember.accessId;
                command.Parameters.Add(new NpgsqlParameter("newAccessCategory", NpgsqlDbType.Varchar));
                command.Parameters["newAccessCategory"].Value = newMember.accessCategory;
                command.Parameters.Add(new NpgsqlParameter("newPayment", NpgsqlDbType.Boolean));
                command.Parameters["newPayment"].Value = newMember.payment;

                // key
                command.Parameters.Add(new NpgsqlParameter("newIdMember", NpgsqlDbType.Integer));
                command.Parameters["newIdMember"].Value = newMember.memberId;
                
                command.CommandText = plsql;
                int id_member = Convert.ToInt32(command.ExecuteScalar());

                // user
                plsql = string.Empty;
                newUser.fkIdMember = newMember.memberId;

                plsql = plsql + "UPDATE users ";
                plsql = plsql + " SET user_name = :newUserName,";
                plsql = plsql + "  user_password = :newUserPassword,";
                plsql = plsql + "  fk_id_member = :newFkIdMember";
                plsql = plsql + " WHERE id_user = :newIdUser";

                command.Parameters.Add(new NpgsqlParameter("newUserName", NpgsqlDbType.Varchar));
                command.Parameters["newUserName"].Value = newUser.userName;
                command.Parameters.Add(new NpgsqlParameter("newUserPassword", NpgsqlDbType.Varchar));
                command.Parameters["newUserPassword"].Value = newUser.userPassword;
                command.Parameters.Add(new NpgsqlParameter("newFkIdMember", NpgsqlDbType.Integer));
                command.Parameters["newFkIdMember"].Value = newUser.fkIdMember;

                command.Parameters.Add(new NpgsqlParameter("newIdUser", NpgsqlDbType.Integer));
                command.Parameters["newIdUser"].Value = newUser.idUser;

                command.CommandText = plsql;
                int id_user = Convert.ToInt32(command.ExecuteScalar());

                tran.Commit();
                succesfull = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                tran.Rollback();
                succesfull = false;
            }
            finally
            {
                conn.Close();
            }        
            return succesfull;
        }
        
        public static bool removeMember(member newMember, users newUser)
        {
            bool succesfull = false;
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
                succesfull = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                tran.Rollback();
                succesfull = false;
            }
            finally
            {
                conn.Close();
            }
            return succesfull;
        }
        
        public static member getMember(int idMember)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
            member newMember = new member();
            try
            {
                conn.Open();
                string plsql = string.Empty;
                
                plsql = plsql + "SELECT id_member, first_name, last_name, address, postal_code,";
                plsql = plsql + " city, mail, gender, hcp, golf_id, fk_category_id, member_category, access_id, access_category, payment ";
                plsql = plsql + " FROM member_new ";
                plsql = plsql + " WHERE id_member = :newIdMember;";

                NpgsqlCommand command = new NpgsqlCommand(@plsql, conn);
                command.Parameters.Add(new NpgsqlParameter("newIdMember", NpgsqlDbType.Integer));
                command.Parameters["newIdMember"].Value = idMember;
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
                    newMember.hcp = Convert.ToDouble((dr["hcp"]));
                    newMember.golfId = (string)(dr["golf_id"]);
                    newMember.categoryId = (int)(dr["fk_category_id"]);
                    newMember.category = (string)(dr["member_category"]);
                    newMember.accessId = (int)(dr["access_id"]);
                    newMember.accessCategory = (string)(dr["access_category"]);
                    newMember.payment = (Boolean)(dr["payment"]);
                }
            }
            finally
            {
                conn.Close();
            }
            return newMember;
        }

        public static users getUser(int IdMember)
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
                command.Parameters["newFkIdMember"].Value = IdMember;
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

        public static int getMemberAccesId(int IdMember)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
            int accesId = 0;
            try
            {
                conn.Open();
                string plsql = string.Empty;

                plsql = "SELECT access_id FROM member_new WHERE id_member = :newIdMember;";
                NpgsqlCommand command = new NpgsqlCommand(@plsql, conn);

                command.Parameters.Add(new NpgsqlParameter("newIdMember", NpgsqlDbType.Integer));
                command.Parameters["newIdMember"].Value = IdMember;

                NpgsqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    accesId = (int)(dr["access_id"]);
                }
            }
            finally
            {
                conn.Close();
            }
            return accesId;
        }

        public static List<access> getAccesCategoryList()
        {
            List<access> accesCategoryList = new List<access>();
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
            users newUser = new users();
            try
            {
                conn.Open();
                string plsql = string.Empty;
                plsql = "SELECT access_id, access_category FROM access;";
                NpgsqlCommand command = new NpgsqlCommand(@plsql, conn);
                NpgsqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    access nyAccesCategory = new access();
                    nyAccesCategory.accessId = (int)(dr["access_id"]);
                    nyAccesCategory.accessCategory = (string)(dr["access_category"]);
                    accesCategoryList.Add(nyAccesCategory);
                }
            }
            finally
            {
                conn.Close();
            }
            return accesCategoryList;
        }
        
        #endregion MEDLEMSSIDA

        #region GAMES

        public static List<games> getGamesByDate(DateTime selectedDate)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
            List<games> gameList = new List<games>();

            string date = selectedDate.ToString().Split(' ')[0];
            string sql = "";
            try
            {
                sql = "SELECT game_id, dates, times, game_starts.time_id " +
                      "FROM game, game_dates, game_starts " +
                      "WHERE game.date_id = game_dates.dates_id " +
                      "AND game.time_id = game_starts.time_id " +
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
                    g.gameId = int.Parse(dr["game_id"].ToString());
                    g.memberInGameList = getBookedMembersByGameId(g.gameId);

                    gameList.Add(g);
                }
            }
            catch (NpgsqlException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
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
                sql = "SELECT id_member, first_name, last_name, address, postal_code, city, mail, gender, hcp, golf_id, member_category " +
                      "FROM game, game_member, member_new " +
                      "WHERE game.game_id = game_member.game_id " +
                      "AND game_member.member_id = member_new.id_member " +
                      "AND game.game_id = " + gameId + ";";

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

            finally
            {
                conn.Close();
            }

            return memberList;
        }
        
        //Hämtar tiden tillhörande ett timeId
        public static DateTime getTimeByTimeId(int timeId)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
            DateTime time = new DateTime();

            string sql = "";
            try
            {
                sql = "SELECT times from game_starts WHERE time_id = " + timeId + ";";

                conn.Open();

                NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
                NpgsqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    time = DateTime.Parse(dr["times"].ToString());
                }
            }
            catch (NpgsqlException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return time;
        }

        public static void addSeason(DateTime startDate)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
            string sql;
            conn.Open();
            try
            {
                sql = "insert into game_dates(dates) VALUES ('" + startDate + "')";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
            catch
            {

            }
            finally
            {
                conn.Close();
            }
        }

        #endregion GAMES

        #region NEWS

        public static List<news> getNewsList()
        {
            List<news> newsList = new List<news>();
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);

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
                sql = "INSERT INTO news (news_info, news_name, news_date) VALUES(:newNewsInfo, :newNewsName, :newNewsDate) RETURNING news_id";

                command.Parameters.Add(new NpgsqlParameter("newNewsInfo", NpgsqlDbType.Varchar));
                command.Parameters["newNewsInfo"].Value = newNews.newsInfo;
                command.Parameters.Add(new NpgsqlParameter("newNewsName", NpgsqlDbType.Varchar));
                command.Parameters["newNewsName"].Value = newNews.newsName;
                command.Parameters.Add(new NpgsqlParameter("newNewsDate", NpgsqlDbType.Date));
                command.Parameters["newNewsDate"].Value = newNews.newsDate;

                command.CommandText = sql;
                int newsID = Convert.ToInt32(command.ExecuteScalar());
                trans.Commit();
              
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
                sql = "UPDATE news SET news_info = :newNewsInfo, news_name = :newNewsName, news_date = :newNewsDate WHERE news_id = :newNewsId RETURNING news_id";

                command.Parameters.Add(new NpgsqlParameter("newNewsInfo", NpgsqlDbType.Varchar));
                command.Parameters["newNewsInfo"].Value = newNews.newsInfo;
                command.Parameters.Add(new NpgsqlParameter("newNewsName", NpgsqlDbType.Varchar));
                command.Parameters["newNewsName"].Value = newNews.newsName;                
                command.Parameters.Add(new NpgsqlParameter("newNewsDate", NpgsqlDbType.Date));
                command.Parameters["newNewsDate"].Value = newNews.newsDate;                
                
                command.Parameters.Add(new NpgsqlParameter("newNewsId", NpgsqlDbType.Integer));
                command.Parameters["newNewsId"].Value = newNews.newsId;

                command.CommandText = sql;
                int news_id = Convert.ToInt32(command.ExecuteScalar());
               
                trans.Commit();
               
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

        public static void removeNews(news newNews)
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

                string sql = string.Empty;
                sql = "DELETE FROM news WHERE news_id = :newNewsId";
                command.Parameters.Add(new NpgsqlParameter("newNewsId", NpgsqlDbType.Integer));
                command.Parameters["newNewsId"].Value = newNews.newsId;
                command.CommandText = sql;
                int news_id = Convert.ToInt32(command.ExecuteScalar());
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

        public static news getNews(int news_id)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
            news newNews = new news();
            try
            {
                conn.Open();
                string sql = string.Empty;
                sql = "SELECT news_id, news_name, news_info FROM news WHERE news_id = :newNewsId;";
                NpgsqlCommand command = new NpgsqlCommand(@sql, conn);
                command.Parameters.Add(new NpgsqlParameter("newNewsId", NpgsqlDbType.Integer));
                command.Parameters["newNewsId"].Value = news_id;
                NpgsqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    newNews.newsId = (int)(dr["news_id"]);
                    newNews.newsName = (string)(dr["news_name"]);
                    newNews.newsInfo = (string)(dr["news_info"]);
                }
            }
            finally
            {
                conn.Close();
            }
            return newNews;
        }

        //Hämtar de 5 senaste nyheterna till en DataTable
        public static DataTable getLatestNews()
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
            DataTable dt = new DataTable();
            string sql;

            try
            {
                sql = "SELECT news_info, news_name, news_date FROM news " +
                      "ORDER BY news_date DESC, news_id DESC " +
                      "LIMIT 10;";
                conn.Open();

                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, conn);
                da.Fill(dt);
            }
            catch (NpgsqlException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return dt;
        }

        //Hämtar nyheter från en angiven månad till en DataTable
        public static DataTable getNewsByDates(string startDate, string endDate)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
            DataTable dt = new DataTable();
            string sql;

            try
            {
                sql = "SELECT news_info, news_name, news_date FROM news " +
                      "WHERE news_date BETWEEN '" + startDate + "' AND '" + endDate + "' " +
                      "ORDER BY news_date DESC, news_id DESC;";
                conn.Open();

                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, conn);
                da.Fill(dt);
            }
            catch (NpgsqlException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }

        public static void SkickaMail(string nyhetsbrev, string rubrik)
        {
            #region skicka mail till alla
            //NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
            //string firstname;
            //string lastname;
            //string mail2;
            //string sql = "";
            //try
            //{
            //    sql = "select first_name, last_name, mail from member_new";
            //    conn.Open();

            //    NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
            //    NpgsqlDataReader dr = cmd.ExecuteReader();

            //    while (dr.Read())
            //    {                   
            //        firstname = dr["first_name"].ToString();
            //        lastname = dr["last_name"].ToString();
            //        mail2 = dr["mail"].ToString();

            //        System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            //        mail.To.Add(mail2);
            //        mail.From = new MailAddress("halslaget@gmail.com", rubrik, System.Text.Encoding.UTF8);
            //        mail.Subject = "Nyhetsbrev Hålslaget";
            //        mail.SubjectEncoding = System.Text.Encoding.UTF8;
            //        mail.Body = nyhetsbrev;
            //        mail.BodyEncoding = System.Text.Encoding.UTF8;
            //        mail.IsBodyHtml = true;
            //        mail.Priority = MailPriority.High;
            //        SmtpClient client = new SmtpClient();
            //        client.Credentials = new System.Net.NetworkCredential("halslaget@gmail.com", "halslagetg5");
            //        client.Port = 587;
            //        client.Host = "smtp.gmail.com";
            //        client.EnableSsl = true;
            //        client.Send(mail);
            //    }
            //}

            //catch
            //{

            //}
            #endregion skicka mail till alla
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            mail.To.Add("halslaget@gmail.com");
            mail.From = new MailAddress("halslaget@gmail.com", rubrik, System.Text.Encoding.UTF8);
            mail.Subject = "Nyhetsbrev Hålslaget";
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = nyhetsbrev;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("halslaget@gmail.com", "halslagetg5");
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;

            try
            {
                client.Send(mail);
            }
            catch (Exception ex)
            {
                
            }
        }
           

        #endregion NEWS

        #region LOGGIN

        public static bool checkUserNameExist(string userName)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
            int amount = 0;
            try
            {
                conn.Open();
                string plsql = string.Empty;

                plsql = plsql + "SELECT 1 AS amount ";
                plsql = plsql + " FROM users ";
                plsql = plsql + " WHERE user_name = :newUserName;";
                NpgsqlCommand command = new NpgsqlCommand(@plsql, conn);

                command.Parameters.Add(new NpgsqlParameter("newUserName", NpgsqlDbType.Varchar));
                command.Parameters["newUserName"].Value = userName;

                NpgsqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    amount = (int)(dr["amount"]);
                }
            }
            finally
            {
                conn.Close();
            }
            if (amount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool checkUserPasswordExist(string userPassword)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
            int amount = 0;
            try
            {
                conn.Open();
                string plsql = string.Empty;

                plsql = plsql + "SELECT 1 AS amount ";
                plsql = plsql + " FROM users ";
                plsql = plsql + " WHERE user_password = :newUserPassword;";
                NpgsqlCommand command = new NpgsqlCommand(@plsql, conn);

                command.Parameters.Add(new NpgsqlParameter("newUserpassword", NpgsqlDbType.Varchar));
                command.Parameters["newUserPassword"].Value = userPassword;

                NpgsqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    amount = (int)(dr["amount"]);
                }
            }
            finally
            {
                conn.Close();
            }
            if (amount > 0)
            { 
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool checkUserExist(string userName, string userPassword)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
            int amount = 0;
            try
            {
                conn.Open();
                string plsql = string.Empty;

                plsql = plsql + "SELECT 1 AS amount ";
                plsql = plsql + " FROM users ";
                plsql = plsql + " WHERE user_name = :newUserName ";
                plsql = plsql + " AND user_password = :newUserpassword;";
                NpgsqlCommand command = new NpgsqlCommand(@plsql, conn);

                command.Parameters.Add(new NpgsqlParameter("newUserName", NpgsqlDbType.Varchar));
                command.Parameters["newUserName"].Value = userName;
                command.Parameters.Add(new NpgsqlParameter("newUserpassword", NpgsqlDbType.Varchar));
                command.Parameters["newUserpassword"].Value = userPassword;

                NpgsqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    amount = (int)(dr["amount"]);
                }
            }
            finally
            {
                conn.Close();
            }
            if (amount > 0)
            {
                return true;
        }
            else
            {
                return false;
            }
        }
        
        public static users getUserByName(string userName, string userPassword) 
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
            users newUser = new users();
            try
            {
                conn.Open();
                string plsql = string.Empty;

                plsql = plsql + "SELECT id_user, user_name, user_password, fk_id_member ";
                plsql = plsql + " FROM users ";
                plsql = plsql + " WHERE user_name = :newUserName ";
                plsql = plsql + " AND user_password = :newUserpassword;";
                NpgsqlCommand command = new NpgsqlCommand(@plsql, conn);

                command.Parameters.Add(new NpgsqlParameter("newUserName", NpgsqlDbType.Varchar));
                command.Parameters["newUserName"].Value = userName;
                command.Parameters.Add(new NpgsqlParameter("newUserpassword", NpgsqlDbType.Varchar));
                command.Parameters["newUserpassword"].Value = userPassword;
                
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

        #endregion LOGGIN

        #region MEDLEMSREGISTRERING
        
        public static List<member> getMemberList()
        {
            List<member> memberList = new List<member>();
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
            try
            {
                conn.Open();
                string plsql = string.Empty;

                plsql = plsql + "SELECT id_member, first_name, last_name, address, postal_code,";
                plsql = plsql + " city, mail, gender, hcp, golf_id, fk_category_id, member_category, access_id, access_category, payment ";
                plsql = plsql + " FROM member_new"; 

                NpgsqlCommand command = new NpgsqlCommand(@plsql, conn);
                command.Parameters.Add(new NpgsqlParameter("newIdMember", NpgsqlDbType.Integer));
                NpgsqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    member newMember = new member();
                    newMember.memberId = (int)(dr["id_member"]);
                    newMember.firstName = (string)(dr["first_name"]);
                    newMember.lastName = (string)(dr["last_name"]);
                    newMember.address = (string)(dr["address"]);
                    newMember.postalCode = (string)(dr["postal_code"]);
                    newMember.city = (string)(dr["city"]);
                    newMember.mail = (string)(dr["mail"]);
                    newMember.gender = (string)(dr["gender"]);
                    newMember.hcp = Convert.ToDouble((dr["hcp"]));
                    newMember.golfId = (string)(dr["golf_id"]);
                    newMember.categoryId = (int)(dr["fk_category_id"]);
                    newMember.category = (string)(dr["member_category"]);
                    newMember.accessId = (int)(dr["access_id"]);
                    newMember.accessCategory = (string)(dr["access_category"]);
                    newMember.payment = (Boolean)(dr["payment"]);
                    memberList.Add(newMember);
                }
            }
            finally
            {
                conn.Close();
            }
            return memberList;
        }
        #endregion 
    }
}
