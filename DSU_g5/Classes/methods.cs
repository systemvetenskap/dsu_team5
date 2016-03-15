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


        public static void bookingByMember(DateTime date, int timeId, int playerId, int bookedById, out string message)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
            message = null;

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
                        //HttpContext.Current.Response.Write("Medlem finns redan inbokad på detta datum och tid.");
                        message = "Medlem finns redan inbokad på detta datum och tid.";
                        //return message;
                        
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
                            try
                            {
                                hcp = double.Parse(drthird["hcp"].ToString());
                            }
                            catch
                            {
                                hcp = 0;
                            }
                        }

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
                                    message = "Finns ej detta datum i databasen";
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
                            //HttpContext.Current.Response.Write("Antal deltagare eller för högt handicap");
                            message = "Antal deltagare får max vara 4 och handicap får sammanlagt vara högst 100.";
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

        public static void unBookingByMem (int gameId, int memId)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);

            string sqlDelGIdAndMId;

            try
            {
                sqlDelGIdAndMId = "DELETE FROM game_member gm WHERE gm.game_id = '"+ gameId +"' AND gm.member_id = '" + memId + "'";
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(sqlDelGIdAndMId, conn);
                cmd.ExecuteNonQuery();

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

        public static void unBookMemWhithBookedByID (int gameId, int bookedBy)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);

            string sqlDelGIdAndMId;

            try
            {
                sqlDelGIdAndMId = "DELETE FROM game_member gm WHERE gm.game_id = '" + gameId + "' AND gm.booked_by = '" + bookedBy + "'";
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(sqlDelGIdAndMId, conn);
                cmd.ExecuteNonQuery();

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


        //En datatable som innehåller värdet av game_id. Datum och tid kan man sedan läsa ut från detta game_id från en SQL-fråga som sedan kan visas i olika labels tex.
        // /Andreas

        public static DataTable LoggedInMemberBookings(int memberId)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);

            string sqlGetMembersBookings;

            DataTable dt = new DataTable();

            try
            {
                sqlGetMembersBookings = "SELECT game.game_id AS gID, (dates || ' ' || times) AS timeAndDate "+
                                        "FROM game_member, game, game_dates, game_starts "+
                                        "WHERE game_member.game_id = game.game_id "+
                                        "AND game.date_id = game_dates.dates_id "+
                                        "AND game.time_id = game_starts.time_id "+
                                        "AND member_id = '"+ memberId +"' ORDER BY gID ASC;";
                
                conn.Open();
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sqlGetMembersBookings, conn);
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

        public static DataTable BookedByLoggedInMemId(int memberId)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);

            string sqlGetBBByMemId;

            DataTable dt = new DataTable();

            try
            {
                sqlGetBBByMemId = "SELECT gm.game_id AS gameId, (first_name ||  ' ' || last_name) AS namn " +
                                  "FROM game_member gm " +
                                  "INNER JOIN member_new m ON m.id_member = gm.member_id " +
                                  "WHERE booked_by = '" + memberId + "' " +
                                  "ORDER BY gameId ASC";

                conn.Open();
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sqlGetBBByMemId, conn);
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

        public static string GetInfoAboutGame(int gameId)
        {
            string infoAboutGame;
            string sqlGetInfoAboutGame;
            string sqlGetBookedBy;
            string datum;
            string tid;
            string namn;
            string bokningsansvarig = "";

            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
            
            //Objekt
            game_dates gd = new game_dates();
            game_starts gs = new game_starts();
            member m = new member();
            member BB = new member();


            try
            {
                sqlGetInfoAboutGame = "SELECT gd.dates AS dates, gs.times AS time, m.first_name, m.last_name " +
                                    "FROM game_member gm " + 
                                    "INNER JOIN game g ON g.game_id = gm.game_id " +
                                    "INNER JOIN game_dates gd ON gd.dates_id = g.date_id " +
                                    "INNER JOIN game_starts gs ON gs.time_id = g.time_id " +
                                    "INNER JOIN member_new m ON m.id_member = gm.member_id " +
                                    "WHERE gm.game_id = '" + gameId + "'";
                conn.Open();
                NpgsqlCommand cmdGetInfo = new NpgsqlCommand(sqlGetInfoAboutGame, conn);
                NpgsqlDataReader dr = cmdGetInfo.ExecuteReader();

                while(dr.Read())
                {
                    //gd = new game_dates();
                    gd.dates = Convert.ToDateTime(dr["dates"].ToString());

                    //gs = new game_starts();
                    gs.times = Convert.ToDateTime(dr["time"].ToString());

                    //m = new member();
                    m.firstName = dr["first_name"].ToString();
                    m.lastName = dr["last_name"].ToString();
                }
                conn.Close();


                sqlGetBookedBy = "SELECT (first_name || '' || last_name) AS bokningsansvarig " +
                                "FROM member_new " +
                                "INNER JOIN game_member gm ON gm.booked_by = id_member " +
                                "WHERE gm.game_id = '" + gameId + "'";

                //sqlGetBookedBy = "SELECT first_name, last_name " +
                //                 "FROM member_new " +
                //                 "INNER JOIN game_member gm ON gm.booked_by = id_member " +
                //                 "WHERE gm.game_id = '" + gameId + "'";

                conn.Open();
                NpgsqlCommand cmdGetBB = new NpgsqlCommand(sqlGetBookedBy, conn);
                NpgsqlDataReader drBB = cmdGetBB.ExecuteReader();

                while(drBB.Read())
                {
                    bokningsansvarig = drBB["bokningsansvarig"].ToString();
                    //BB.firstName = drBB["first_name"].ToString();
                    //BB.lastName = drBB["last_name"].ToString();
                }
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

            datum = gd.dates.ToShortDateString();
            tid = gs.times.ToShortTimeString();
            namn = m.firstName + " " + m.lastName;
            //bokningsansvarig = BB.firstName + " " + BB.lastName;

            infoAboutGame = datum + ", " + tid + ", " + namn + ". Bokningsansvarig: " + bokningsansvarig + ".";
            //infoAboutGame = datum + ", " + tid + ", " + namn + ". Bokningsansvarig: " + bokningsansvarig + ".";

            return infoAboutGame;
        }

        public static List<int> GetIDsFromMemberList()
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
            List<int> memberIdList = new List<int>();

            string sqlMemberIDs;

            try
            {
                sqlMemberIDs = "SELECT id_member FROM member_new ORDER BY id_member ASC";

                conn.Open();

                NpgsqlCommand cmd = new NpgsqlCommand(sqlMemberIDs, conn);
                NpgsqlDataReader dr = cmd.ExecuteReader();

                while(dr.Read())
                {
                    memberIdList.Add(Convert.ToInt32(dr["id_member"]));
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

            return memberIdList;
        }

        #endregion

        #region BOKNING OCH AVBOKNING - ADMIN


        public static void bookMember(DateTime date, int timeId, int chosenMid, out string message)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);

            message = null;

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
                        //HttpContext.Current.Response.Write("Medlem finns redan inbokad på detta datum och tid.");
                        message = "Medlem finns redan inbokad på detta datum och tid.";
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
                        while(drthird.Read())
                        {
                            
                            //hcp = Convert.ToInt32(drthird["hcp"].ToString());
                            if(double.TryParse(drthird["hcp"].ToString(), out hcp))
                            {

                        }
                            else
                        {
                                hcp = 0;
                        }
                            
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
                            //HttpContext.Current.Response.Write("Antal deltagare får max vara 4 och handicap får max vara 100");
                            message = "Antal deltagare får max vara 4 och handicap får max vara 100.";
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
                sql = "SELECT (first_name ||  ' ' ||  last_name) AS namn, id_member AS mID FROM member_new WHERE payment = true ORDER BY last_name"; //first_name och last_name blir en egen kolumn som heter 'name'.
                                                                                                        
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

                newMember.memberId = id_member;
                newUser.fkIdMember = newMember.memberId;

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
                newUser.idUser = id_user;

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
                    newMember.hcp = dr["hcp"] != DBNull.Value ? Convert.ToDouble((dr["hcp"])) : 0.00;                    
                    newMember.golfId = (string)(dr["golf_id"]);                    
                    newMember.categoryId = dr["fk_category_id"] != DBNull.Value ? (int)(dr["fk_category_id"]) : 0;
                    newMember.category = dr["member_category"] != DBNull.Value ? (string)(dr["member_category"]) : "";                    
                    newMember.accessId = dr["access_id"] != DBNull.Value ? (int)(dr["access_id"]) : 0;
                    newMember.category = dr["member_category"] != DBNull.Value ? (string)(dr["member_category"]) : "";
                    newMember.payment = dr["payment"] != DBNull.Value ? (Boolean)(dr["payment"]) : false;
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
        public static void removeSeason(DateTime startDate, DateTime endDate)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
            string sql;
            conn.Open();
            try
            {
                sql = "delete from game_dates where dates = '"+ startDate +"'";
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
        public static game_dates maxmindates()
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
            string sql = "";

            game_dates maxmin = new game_dates();

            DateTime year = DateTime.Now;
            try
            {
                sql = "select max(dates), min(dates) from game_dates where dates >= '" + year + "'";
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
                NpgsqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {

                    //maxmin.endDate = dr["max"].ToString();
                    //maxmin.startDate = dr["min"].ToString();               
                    conn.Close();
                }
            }
            catch
            {

            }

            return maxmin;
        }

        public static List<DateTime> getDates()
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
            string sql = "";
            DateTime year = DateTime.Now;
            List<DateTime> datesList = new List<DateTime>();
            try
            {
                conn.Open();
                sql = "select * from game_dates where dates >= '" + year + "'";
                NpgsqlCommand command = new NpgsqlCommand(sql, conn);
                NpgsqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    DateTime exDates = new DateTime();
                    exDates = Convert.ToDateTime(dr["dates"].ToString());
                    datesList.Add(exDates);
                }
            }
            catch
            {

            }
            return datesList;
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

                plsql = plsql + "SELECT DISTINCT 1 AS amount ";
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

        public static bool checkUserExist(int idUser, string userName, string userPassword)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
            int amount = 0;
            try
            {
                conn.Open();
                string plsql = string.Empty;

                plsql = plsql + "SELECT DISTINCT 1 AS amount ";
                plsql = plsql + " FROM users ";
                plsql = plsql + " WHERE user_name = :newUserName ";
                plsql = plsql + " AND user_password = :newUserpassword";
                plsql = plsql + " AND id_user <> :newIdUser;";
                NpgsqlCommand command = new NpgsqlCommand(@plsql, conn);

                command.Parameters.Add(new NpgsqlParameter("newUserName", NpgsqlDbType.Varchar));
                command.Parameters["newUserName"].Value = userName;
                command.Parameters.Add(new NpgsqlParameter("newUserpassword", NpgsqlDbType.Varchar));
                command.Parameters["newUserpassword"].Value = userPassword;
                command.Parameters.Add(new NpgsqlParameter("newIdUser", NpgsqlDbType.Integer));
                command.Parameters["newIdUser"].Value = idUser;

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
                plsql = plsql + " ORDER BY first_name, last_name";

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
                    newMember.hcp = dr["hcp"] != DBNull.Value ? Convert.ToDouble((dr["hcp"])) : 0.00;                                        
                    newMember.golfId = (string)(dr["golf_id"]);                    
                    newMember.categoryId = dr["fk_category_id"] != DBNull.Value ? (int)(dr["fk_category_id"]) : 0;
                    newMember.accessCategory = dr["member_category"] != DBNull.Value ? (string)(dr["member_category"]) : "";
                    newMember.accessId = dr["access_id"] != DBNull.Value ? (int)(dr["access_id"]) : 0;                    
                    newMember.accessCategory = dr["access_category"] != DBNull.Value ? (string)(dr["access_category"]) : "";
                    newMember.payment = dr["payment"] != DBNull.Value ? (Boolean)(dr["payment"]) : false;
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

        #region SKAPA TÄVLING

        //hämta datatable med tävlingsformer
        public static DataTable getGameForms()
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);

            string sql;
            DataTable dt = new DataTable();

            try
            {
                sql = "SELECT * FROM gameform";

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

        //lägg till tävling i databas och returnera dess id
        public static int insertTournament(tournament tour)
        {
            int id_tournament = 0;
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = conn;
            try
            {
                conn.Open();
                command.Connection = conn;
                string plsql;

                plsql = "INSERT INTO tournament (tour_name, tour_info, registration_start, registration_end, "+
                        "tour_start_time, tour_start_end, publ_date_startlists, contact_person, gameform, hole, tour_date) "+
                        "VALUES (:tour_name, :tour_info, :registration_start, :registration_end, "+
                        ":tour_start_time, :tour_start_end, :publ_date_startlists, :contact_person, :gameform, :hole, :tour_date) "+
                        "RETURNING id_tournament;";

                command.Parameters.Add(new NpgsqlParameter("tour_name", NpgsqlDbType.Varchar));
                command.Parameters["tour_name"].Value = tour.tour_name;
                command.Parameters.Add(new NpgsqlParameter("tour_info", NpgsqlDbType.Varchar));
                command.Parameters["tour_info"].Value = tour.tour_info;
                command.Parameters.Add(new NpgsqlParameter("registration_start", NpgsqlDbType.Date));
                command.Parameters["registration_start"].Value = tour.registration_start;
                command.Parameters.Add(new NpgsqlParameter("registration_end", NpgsqlDbType.Date));
                command.Parameters["registration_end"].Value = tour.registration_end;
                command.Parameters.Add(new NpgsqlParameter("tour_start_time", NpgsqlDbType.Time));
                command.Parameters["tour_start_time"].Value = tour.tour_start_time;
                command.Parameters.Add(new NpgsqlParameter("tour_start_end", NpgsqlDbType.Time));
                command.Parameters["tour_start_end"].Value = tour.tour_end_time;
                command.Parameters.Add(new NpgsqlParameter("publ_date_startlists", NpgsqlDbType.Date));
                command.Parameters["publ_date_startlists"].Value = tour.publ_date_startlists;
                command.Parameters.Add(new NpgsqlParameter("contact_person", NpgsqlDbType.Integer));
                command.Parameters["contact_person"].Value = tour.contact_person;
                command.Parameters.Add(new NpgsqlParameter("gameform", NpgsqlDbType.Integer));
                command.Parameters["gameform"].Value = tour.gameform;
                command.Parameters.Add(new NpgsqlParameter("hole", NpgsqlDbType.Integer));
                command.Parameters["hole"].Value = tour.hole;
                command.Parameters.Add(new NpgsqlParameter("tour_date", NpgsqlDbType.Date));
                command.Parameters["tour_date"].Value = tour.tour_date;

                command.CommandText = plsql;
                id_tournament = Convert.ToInt32(command.ExecuteScalar());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return id_tournament;
        }

        //hämta datatable med sponsorer
        public static DataTable getSponsors()
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);

            string sql;
            DataTable dt = new DataTable();

            try
            {
                sql = "SELECT sponsor_id, sponsor_name FROM sponsor";

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

        //lägg till sponsor i databas och returnera dess id
        public static int insertSponsor(sponsor sp)
        {
            int sponsor_id = 0;
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = conn;
            try
            {
                conn.Open();
                command.Connection = conn;
                string plsql;

                plsql = "INSERT INTO sponsor (sponsor_name, phone) " +
                        "VALUES (:sponsor_name, :phone) " +
                        "RETURNING sponsor_id;";

                command.Parameters.Add(new NpgsqlParameter("sponsor_name", NpgsqlDbType.Varchar));
                command.Parameters["sponsor_name"].Value = sp.sponsor_name;
                command.Parameters.Add(new NpgsqlParameter("phone", NpgsqlDbType.Varchar));
                command.Parameters["phone"].Value = sp.phone;

                command.CommandText = plsql;
                sponsor_id = Convert.ToInt32(command.ExecuteScalar());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return sponsor_id;
        }

        //lägg till koppling mellan tävling och sponsor
        public static void insertTour_sponsor(int tour_id, int sponsor_id)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = conn;
            try
            {
                conn.Open();
                command.Connection = conn;
                string plsql;

                plsql = "INSERT INTO tour_sponsor (tour_id, sponsor_id) " +
                        "VALUES (:tour_id, :sponsor_id);";

                command.Parameters.Add(new NpgsqlParameter("tour_id", NpgsqlDbType.Integer));
                command.Parameters["tour_id"].Value = tour_id;
                command.Parameters.Add(new NpgsqlParameter("sponsor_id", NpgsqlDbType.Integer));
                command.Parameters["sponsor_id"].Value = sponsor_id;

                command.CommandText = plsql;
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        #endregion

        #region TÄVLING
        public static List<tournament> getTourList()
        {
            List<tournament> tourList = new List<tournament>();
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);

            try
            {
                conn.Open();
                string sql = string.Empty;
                DateTime d;
                DateTime f;
                
                sql = "SELECT * from tournament";
                NpgsqlCommand command = new NpgsqlCommand(@sql, conn);
                NpgsqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    tournament newTournament = new tournament();
                    newTournament.id_tournament = (int)(dr["id_tournament"]);
                    newTournament.tour_name = (string)(dr["tour_name"]);
                    newTournament.tour_info = (string)(dr["tour_info"]);
                    newTournament.registration_start = (DateTime)(dr["registration_start"]);
                    newTournament.registration_end = (DateTime)(dr["registration_end"]);
                    newTournament.tour_start_time = DateTime.Parse((dr["tour_start_time"]).ToString());
                    newTournament.tour_end_time = DateTime.Parse((dr["tour_start_end"]).ToString());
                    newTournament.publ_date_startlists = (DateTime)(dr["publ_date_startlists"]);
                    newTournament.contact_person = (int)(dr["contact_person"]);
                    newTournament.gameform = (int)(dr["gameform"]);
                    newTournament.hole = (int)(dr["hole"]);
                    newTournament.tour_date = (DateTime)(dr["tour_date"]);

                    tourList.Add(newTournament);
                }
            }
            finally
            {
                conn.Close();
            }
            return tourList;
        }

        
        //public static DataTable getAllTournaments()
        //{
        //    NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);

        //    string sql;

        //    DataTable dt = new DataTable();

        //    try
        //    {
        //        sql = "SELECT (tour_name) AS namn, id_member AS mID FROM member_new WHERE payment = true ORDER BY last_name"; //first_name och last_name blir en egen kolumn som heter 'name'.

        //        conn.Open();

        //        NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, conn);
        //        da.Fill(dt); //Fyller dataAdatpter med dataTable.
        //    }
        //    catch (NpgsqlException ex)
        //    {
        //        Debug.WriteLine(ex.Message);
        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }

        //    return dt;
        //}


        public static tournament GetTournament(int tourId)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
            tournament t = new tournament();

            string sqlGetTournament;

            try
            {
                conn.Open();
                sqlGetTournament = "SELECT id_tournament, tour_name, tour_info, registration_start, registration_end, tour_start_time, tour_start_end, publ_date_startlists, contact_person, gameform, hole, tour_date " +
                                   "FROM tournament " +
                                   "WHERE id_tournament = '" + tourId + "' ";

                NpgsqlCommand cmdGetTour = new NpgsqlCommand(sqlGetTournament, conn);
                NpgsqlDataReader dr = cmdGetTour.ExecuteReader();

                while(dr.Read())
                {
                    t.id_tournament = Convert.ToInt32(dr["id_tournament"].ToString());
                    t.tour_name = dr["tour_name"].ToString();
                    t.tour_info = dr["tour_info"].ToString();
                    t.registration_start = Convert.ToDateTime(dr["registration_start"].ToString());
                    t.registration_end = Convert.ToDateTime(dr["registration_end"].ToString());
                    t.tour_start_time = Convert.ToDateTime(dr["tour_start_time"].ToString());
                    t.tour_end_time = Convert.ToDateTime(dr["tour_start_end"].ToString());
                    t.publ_date_startlists = Convert.ToDateTime(dr["publ_date_startlists"].ToString());
                    t.contact_person = Convert.ToInt32(dr["contact_person"].ToString());
                    t.gameform = Convert.ToInt32(dr["gameform"].ToString());
                    t.hole = Convert.ToInt32(dr["hole"].ToString());
                    t.tour_date = Convert.ToDateTime(dr["tour_date"].ToString());
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
            return t;
        }



        public static DataTable getLatestTour()
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
            DataTable dt = new DataTable();
            string sql;

            try
            {

                //sql = "SELECT id_tournament as tId ( tourname ||  ' ' ||  tour_info || ' ' || tour_date) AS tInfo FROM tournament ORDER BY tour_date DESC, id_tournament DESC LIMIT 10;";


                sql = "SELECT * FROM tournament " +
                      "ORDER BY tour_date DESC, id_tournament DESC " +
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
        public static DataTable getTourByDates(string startDate, string endDate)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
            DataTable dt = new DataTable();
            string sql;

            try
            {
                sql = "SELECT tour_info, tour_name, tour_date FROM tournament " +
                      "WHERE tour_date BETWEEN '" + startDate + "' AND '" + endDate + "' " +
                      "ORDER BY tour_date DESC, id_tournament DESC;";
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


        public static void RegMemberOnTour(int tourId, int memId, int resultat, out string message)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);

            message = null;
            string sqlAlreadyRegisters;


            try
            {
                conn.Open();
                sqlAlreadyRegisters = "SELECT * FROM member_tournament mt " +
                                      "INNER JOIN member_new mn " +
                                      "ON mt.member_id = mn.id_member " +
                                      "INNER JOIN tournament t " +
                                      "ON mt.tournament_id = t.id_tournament " +
                                      "WHERE member_id = '" + memId + "' AND tournament_id = '" + tourId + "'";
                NpgsqlCommand cmdExist = new NpgsqlCommand(sqlAlreadyRegisters, conn);
                NpgsqlDataReader dr = cmdExist.ExecuteReader();

                if(dr.HasRows)
                {
                    message = "Vald medlem finns redan inbokad på vald tävling";
                }
                else
                {
                    string sqlAddToMT = "INSERT INTO member_tournament (member_id, tournament_id, result) VALUES (@mId, @tId, @r)";

                    NpgsqlCommand cmdInsToMT = new NpgsqlCommand(sqlAddToMT, conn);
                    cmdInsToMT.Parameters.AddWithValue("mId", memId);
                    cmdInsToMT.Parameters.AddWithValue("tId", tourId);
                    cmdInsToMT.Parameters.AddWithValue("r", resultat);

                    //conn.Open();
                    cmdInsToMT.ExecuteNonQuery();
                    conn.Close();
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
        
        }

        //METOD FÖR ATT LADDA TILL EN DATATABLE SOM VISAS I GV!
        public static DataTable GetInfoAboutTour(int tourID)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Tävlingsnamn");
            dt.Columns.Add("Tävlingsinfo");
            dt.Columns.Add("Tävlingsdatum");
            dt.Columns.Add("Registreringsstart");
            dt.Columns.Add("Registreringsslut");
            dt.Columns.Add("Start");
            dt.Columns.Add("Slut");
            dt.Columns.Add("Kontaktperson");
            dt.Columns.Add("Antal hål");

            return dt;

        }


        public static string ContactPersonName(int tourID)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);

            member contactPerson = new member();
            string sqlGetName;

            try
            {

                sqlGetName = "SELECT first_name, last_name " +
                             "FROM member_new mn " +
                             "INNER JOIN tournament t " +
                             "ON mn.id_member = t.contact_person " +
                             "WHERE t.id_tournament = '" + tourID + "'";
                conn.Open();

                NpgsqlCommand cmd = new NpgsqlCommand(sqlGetName, conn);
                NpgsqlDataReader dr = cmd.ExecuteReader();

                while(dr.Read())
                {
                    contactPerson.firstName = dr["first_name"].ToString();
                    contactPerson.lastName = dr["last_name"].ToString();
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
            return contactPerson.firstName + " " + contactPerson.lastName;
        }



        #endregion

        #region startlist

        public static tournament getTournament(int id_tournament)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
            tournament newTour = new tournament();
            try
            {
                conn.Open();
                string sql = string.Empty;
                sql = "SELECT id_tournament, tour_name FROM tournament WHERE id_tournament = :newIdTournament;";
                NpgsqlCommand command = new NpgsqlCommand(@sql, conn);
                command.Parameters.Add(new NpgsqlParameter("newIdTournament", NpgsqlDbType.Integer));
                command.Parameters["newIdTournament"].Value = id_tournament;
                NpgsqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    newTour.id_tournament = (int)(dr["id_tournament"]);
                    newTour.tour_name = (string)(dr["tour_name"]);

                }
            }
            finally
            {
                conn.Close();
            }
            return newTour;
        }

       

        public static List<member> participantsByTourId(int id_tournament)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
            //member newMember = new member();
            List<member> memberList = new List<member>();
            string sql;
            try
            {
                sql = "SELECT id_member, first_name, last_name FROM member_new mn INNER JOIN member_tournament mt " +
                      "ON mn.id_member = mt.member_id INNER JOIN tournament t ON t.id_tournament = mt.tournament_id " +
                      "WHERE mt.tournament_id = '" + id_tournament + "'";

                conn.Open();

                NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
                NpgsqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    member newMember = new member();
                    newMember.memberId = int.Parse(dr["id_member"].ToString());
                    newMember.firstName = dr["first_name"].ToString();
                    newMember.lastName = dr["last_name"].ToString();
                    memberList.Add(newMember);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            finally
            {
                conn.Close();
            }






            //List<int> memberTournamentIdList = new List<int>();
            //foreach (member me in memberList)
            //{
            //    memberTournamentIdList.Add(me.memberId);

            //}
            //List <int> page = new List<int>();
            //Random rnd = new Random(); // <-- This line goes out of the loop        
            //for (int i = 0; i < memberTournamentIdList.Count; i++)
            //{
            //    int temp = 0;
            //    temp = rnd.Next(0, 2);
            //    page[i] = temp;
            //}

            //Random rnd = new Random();
            //int randomNumber = rnd.Next();
            //for (int i = 0; i < 3; i++)
            ////{
            //    i = memberTournamentIdList;
            //}
            //
            //return;

            //Random r = new Random();
            //int index = r.Next(memberTournamentIdList.Count);
            //string randomString = memberTournamentIdList[index];
            //memberTournamentIdList.Sort()

            //Random r = new Random();
            //int index = r.Next(memberList.Count);

            //string randomString = memberList[index].ToString();

            Random r = new Random();
            List<member> newmemberList = new List<member>();
            bool keepgoing = true;
            int max = memberList.Count;
            //for (int i = 0; i < max; i++) 
            while (keepgoing == true)
            {
                int index = r.Next(memberList.Count);
                member randomMember = (member)memberList[index];
                if (newmemberList.Contains(randomMember) == true)
                {

                }
                else
                {
                    newmemberList.Add(randomMember);
                }
                if (newmemberList.Count == memberList.Count)
                {
                    keepgoing = false;
                }
                else
                {
                    keepgoing = true;
                }
            }

            return newmemberList;


        }





        public static List<int> GetIDsFromMemberTournaments(int id_tournament)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
            List<int> memberTournamentIdList = new List<int>();
            //List<tour_member> tourMemberParticipant = new List<tour_member>();
            //tour_member newTourMember = new tour_member();
            string sql;

            try
            {
                conn.Open();
                sql = "SELECT member_id FROM member_tournament WHERE tournament_id = '" + id_tournament + "' ORDER BY member_id ASC";

                //sql ="SELECT member_tournament.member_id, member_tournament.tournament_id FROM public.member_tournament," + 
                //     "public.member_new, public.tournament WHERE member_tournament.member_id = member_new.id_member AND" +
                //     "tournament.id_tournament = '" + id_tournament + "'";


                NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
                NpgsqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {

                    //newTourMember.memberId = (int)(dr["member_id"]);
                    //newTourMember.tourId = Convert.ToInt32(dr["tournament_id"].ToString());
                    //memberTournamentIdList.Add(newTourMember.memberId);

                    //tourMemberParticipant.Add(newTourMember);
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

            return memberTournamentIdList;
            //return tourMemberParticipant;


            //Random rnd = new Random();
            //int randomNumber = rnd.Next();

            //for (int i = 0; i < 3; % i++)
            //{
            //   memberList = i;
            //}
            //return;
        }

        //public static Random()
        //{
        //    Random rnd = new Random();
        //    int randomNumber = rnd.Next();

        //    for (int i = 0; i < 3; i++)
        //    {
        //        i = Convert.ToInt32(memberTournamentIdList);
        //    }

        //    return; 

        //DISPLAY OUTPUT
        //txt_output.Text += randomNumber;

        //}


        #endregion








        #region RESULTAT

        public static List<member> getParticipantList(int tourId, string gender)
        { 
            List<member> memberList = new List<member>();
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
            try
            {
                conn.Open();
                string plsql = string.Empty;

                plsql = plsql + "SELECT id_member, first_name, last_name, address, postal_code,";
                plsql = plsql + "   city, mail, gender, hcp, golf_id, fk_category_id, member_category, ";
                plsql = plsql + "   access_id, access_category, payment ";
                plsql = plsql + " FROM member_tournament ";
                plsql = plsql + "     LEFT JOIN tournament AS tournament ON tournament.id_tournament = member_tournament.tournament_id";
                plsql = plsql + "     LEFT JOIN member_new AS member_new ON member_new.id_member = member_tournament.member_id";
                plsql = plsql + " WHERE member_tournament.tournament_id = :newTournamentId ";
                plsql = plsql + "     AND member_new.gender = :newGender";

                NpgsqlCommand command = new NpgsqlCommand(@plsql, conn);
                command.Parameters.Add(new NpgsqlParameter("newTournamentId", NpgsqlDbType.Integer));
                command.Parameters["newTournamentId"].Value = tourId;

                // gender hårdkodad än så länge då är inte klar vilken tabell den komer att finnas 
                command.Parameters.Add(new NpgsqlParameter("newGender", NpgsqlDbType.Varchar));
                command.Parameters["newGender"].Value = gender; 
                
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
                    newMember.hcp = dr["hcp"] != DBNull.Value ? Convert.ToDouble((dr["hcp"])) : 0.00;                                        
                    newMember.golfId = (string)(dr["golf_id"]);                    
                    newMember.categoryId = dr["fk_category_id"] != DBNull.Value ? (int)(dr["fk_category_id"]) : 0;
                    newMember.accessCategory = dr["member_category"] != DBNull.Value ? (string)(dr["member_category"]) : "";
                    newMember.accessId = dr["access_id"] != DBNull.Value ? (int)(dr["access_id"]) : 0;                    
                    newMember.accessCategory = dr["access_category"] != DBNull.Value ? (string)(dr["access_category"]) : "";
                    newMember.payment = dr["payment"] != DBNull.Value ? (Boolean)(dr["payment"]) : false;
                    memberList.Add(newMember);
                }
            }
            finally
            {
                conn.Close();
            }
            return memberList;
        }

        public static bool checkResultExist(int tourId, int memberId)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
            int amount = 0;
            try
            {
                conn.Open();
                string plsql = string.Empty;

                plsql = plsql + "SELECT DISTINCT 1 AS amount ";
                plsql = plsql + " FROM results ";
                plsql = plsql + " WHERE member_id = :newMemberId ";
                plsql = plsql + " AND tour_id = :newTourId;";

                NpgsqlCommand command = new NpgsqlCommand(@plsql, conn);

                command.Parameters.Add(new NpgsqlParameter("newMemberId", NpgsqlDbType.Integer));
                command.Parameters["newMemberId"].Value = memberId;
                command.Parameters.Add(new NpgsqlParameter("newTourId", NpgsqlDbType.Integer));
                command.Parameters["newTourId"].Value = tourId;

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

        public static List<results> getExistsResults(int tourId, int memberId)
        {
            List<results> resultsList = new List<results>();
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
            try
            {
                conn.Open();
                string plsql = string.Empty;

                plsql = plsql + "SELECT results.tour_id AS tourId, results.member_id AS memberId,";
                plsql = plsql + "      results.course_id AS courseId, pair, hcp, tries, gamehcp, netto";
                plsql = plsql + " FROM results ";
                plsql = plsql + "    LEFT JOIN course AS course ON course.course_id = results.course_id";
                plsql = plsql + " WHERE tour_id = :newTourId AND member_id = :newMemberId";
                plsql = plsql + " ORDER BY results.course_id";

                NpgsqlCommand command = new NpgsqlCommand(@plsql, conn);

                command.Parameters.Add(new NpgsqlParameter("newTourId", NpgsqlDbType.Integer));
                command.Parameters["newTourId"].Value = tourId;
                command.Parameters.Add(new NpgsqlParameter("newMemberId", NpgsqlDbType.Integer));
                command.Parameters["newMemberId"].Value = memberId;                

                NpgsqlDataReader dr = command.ExecuteReader();

                while (dr.Read())
                {
                    results newResults = new results();
                    newResults.tourId = dr["tourId"] != DBNull.Value ? (int)(dr["tourId"]) : 0;
                    newResults.memberId = dr["memberId"] != DBNull.Value ? (int)(dr["memberId"]) : 0;
                    newResults.courseId = dr["courseId"] != DBNull.Value ? (int)(dr["courseId"]) : 0;
                    newResults.pair = dr["pair"] != DBNull.Value ? (int)(dr["pair"]) : 0;
                    newResults.hcp = dr["hcp"] != DBNull.Value ? (int)(dr["hcp"]) : 0;
                    newResults.tries = dr["tries"] != DBNull.Value ? (int)(dr["tries"]) : 0;
                    newResults.gamehcp = dr["gamehcp"] != DBNull.Value ? (int)(dr["gamehcp"]) : 0;
                    newResults.netto = dr["netto"] != DBNull.Value ? (int)(dr["netto"]) : 0;
                    resultsList.Add(newResults);
                }
            }
            finally
            {
                conn.Close();
            }
            return resultsList;        
        }

        public static List<results> getDefaultResults(int tourId, int memberId)
        {
            List<results> resultsList = new List<results>();
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
            try
            {
                conn.Open();
                string plsql = string.Empty;

                plsql = plsql + "SELECT course.course_id AS courseId, pair, hcp, 0 AS tries, 0 AS gamehcp, 0 AS netto";
                plsql = plsql + " FROM course";
                plsql = plsql + " ORDER BY course.course_id";

                NpgsqlCommand command = new NpgsqlCommand(@plsql, conn);

                command.Parameters.Add(new NpgsqlParameter("newTourId", NpgsqlDbType.Integer));
                command.Parameters["newTourId"].Value = tourId;
                command.Parameters.Add(new NpgsqlParameter("newMemberId", NpgsqlDbType.Integer));
                command.Parameters["newMemberId"].Value = memberId;       

                NpgsqlDataReader dr = command.ExecuteReader();

                while (dr.Read())
                {
                    results newResults = new results();
                    // lägger till detta som default för respektive medlem 
                    newResults.tourId = tourId;
                    newResults.memberId = memberId;                    
                    newResults.courseId = dr["courseId"] != DBNull.Value ? (int)(dr["courseId"]) : 0;
                    newResults.pair = dr["pair"] != DBNull.Value ? (int)(dr["pair"]) : 0;
                    newResults.hcp = dr["hcp"] != DBNull.Value ? (int)(dr["hcp"]) : 0;
                    newResults.tries = dr["tries"] != DBNull.Value ? (int)(dr["tries"]) : 0;
                    newResults.gamehcp = dr["gamehcp"] != DBNull.Value ? (int)(dr["gamehcp"]) : 0;
                    newResults.netto = dr["netto"] != DBNull.Value ? (int)(dr["netto"]) : 0;
                    resultsList.Add(newResults);
                }
            }
            finally
            {
                conn.Close();
            }
            return resultsList;
        }
        
        public static bool addResult(List<results> resultsList)
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

                int memberId = 0;
                int tourId = 0;
                int summa_netto = 0;

                foreach (results li in resultsList)
                {
                    plsql = string.Empty;
                    plsql = plsql + "INSERT INTO results(";
                    plsql = plsql + "            member_id, tour_id, course_id, tries, gamehcp, netto)";
                    plsql = plsql + "    VALUES (:newMemberId, :newTourId, :newCourseId, :newTries, :newGamehcp, :newNetto)";
                    plsql = plsql + " RETURNING course_id";
                    
                    command.Parameters.Add(new NpgsqlParameter("newMemberId", NpgsqlDbType.Integer));
                    command.Parameters["newMemberId"].Value = li.memberId;
                    command.Parameters.Add(new NpgsqlParameter("newTourId", NpgsqlDbType.Integer));
                    command.Parameters["newTourId"].Value = li.tourId;
                    command.Parameters.Add(new NpgsqlParameter("newCourseId", NpgsqlDbType.Integer));
                    command.Parameters["newCourseId"].Value = li.courseId;
                    command.Parameters.Add(new NpgsqlParameter("newTries", NpgsqlDbType.Integer));
                    command.Parameters["newTries"].Value = li.tries;
                    command.Parameters.Add(new NpgsqlParameter("newGamehcp", NpgsqlDbType.Integer));
                    command.Parameters["newGamehcp"].Value = li.gamehcp;
                    command.Parameters.Add(new NpgsqlParameter("newNetto", NpgsqlDbType.Integer));
                    command.Parameters["newNetto"].Value = li.netto;

                    tourId = li.tourId;
                    memberId = li.memberId;
                    summa_netto = summa_netto + li.netto;

                    command.CommandText = plsql;
                    int course_id = Convert.ToInt32(command.ExecuteScalar());
                }
                
                // upddaterar member_tournament, kolumn resultat till suma(netto)
                plsql = string.Empty;
                plsql = plsql + "UPDATE member_tournament";
                plsql = plsql + "   SET result = :newNetto";
                plsql = plsql + " WHERE tournament_id = :newTourId AND member_id = :newMemberId;";

                command.Parameters.Add(new NpgsqlParameter("newMemberId", NpgsqlDbType.Integer));
                command.Parameters["newMemberId"].Value = memberId;                
                command.Parameters.Add(new NpgsqlParameter("newTourId", NpgsqlDbType.Integer));
                command.Parameters["newTourId"].Value = tourId;
                command.Parameters.Add(new NpgsqlParameter("newNetto", NpgsqlDbType.Integer));
                command.Parameters["newNetto"].Value = summa_netto;

                command.CommandText = plsql;
                // tar inte emot returen  
                Convert.ToInt32(command.ExecuteScalar());

                // lopp stop                
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

        public static bool modifyResult(List<results> resultsList)
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

                int memberId = 0;
                int tourId = 0;
                int summa_netto = 0;

                foreach (results li in resultsList)
                {
                    plsql = string.Empty;
                    plsql = plsql + "UPDATE results";
                    plsql = plsql + "   SET tries = :newTries, gamehcp = :newGamehcp, netto = :newNetto ";
                    plsql = plsql + " WHERE member_id = :newMemberId AND tour_id = :newTourId AND course_id = :newCourseId;";

                    command.Parameters.Add(new NpgsqlParameter("newMemberId", NpgsqlDbType.Integer));
                    command.Parameters["newMemberId"].Value = li.memberId;
                    command.Parameters.Add(new NpgsqlParameter("newTourId", NpgsqlDbType.Integer));
                    command.Parameters["newTourId"].Value = li.tourId;
                    command.Parameters.Add(new NpgsqlParameter("newCourseId", NpgsqlDbType.Integer));
                    command.Parameters["newCourseId"].Value = li.courseId;
                    command.Parameters.Add(new NpgsqlParameter("newTries", NpgsqlDbType.Integer));
                    command.Parameters["newTries"].Value = li.tries;
                    command.Parameters.Add(new NpgsqlParameter("newGamehcp", NpgsqlDbType.Integer));
                    command.Parameters["newGamehcp"].Value = li.gamehcp;
                    command.Parameters.Add(new NpgsqlParameter("newNetto", NpgsqlDbType.Integer));
                    command.Parameters["newNetto"].Value = li.netto;

                    tourId = li.tourId;
                    memberId = li.memberId;

                    summa_netto = summa_netto + li.netto;
                    command.CommandText = plsql;
                    int course_id = Convert.ToInt32(command.ExecuteScalar());
                }
                
                // uppdateraR resultat i member_tournament till ny summa då kortet har uppdaterats.
                plsql = string.Empty;
                plsql = plsql + "UPDATE member_tournament";
                plsql = plsql + "   SET result = :newNetto";
                plsql = plsql + " WHERE tournament_id = :newTourId AND member_id = :newMemberId;";
                
                command.Parameters.Add(new NpgsqlParameter("newTourId", NpgsqlDbType.Integer));
                command.Parameters["newTourId"].Value = tourId;
                command.Parameters.Add(new NpgsqlParameter("newMemberId", NpgsqlDbType.Integer));
                command.Parameters["newMemberId"].Value = memberId;            
                command.Parameters.Add(new NpgsqlParameter("newNetto", NpgsqlDbType.Integer));
                command.Parameters["newNetto"].Value = summa_netto;

                command.CommandText = plsql;
                // tar inte emot returen  
                Convert.ToInt32(command.ExecuteScalar());

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

        public static bool removeResult(List<results> resultsList)
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

                int memberId = 0;
                int tourId = 0;
                int summa_netto = 0;
                foreach (results li in resultsList)
                {
                    plsql = string.Empty;
                    plsql = plsql + "DELETE FROM results";
                    plsql = plsql + " WHERE tour_id = :newTourId AND member_id = :newMemberId AND course_id = :newCourseId;";

                    command.Parameters.Add(new NpgsqlParameter("newMemberId", NpgsqlDbType.Integer));
                    command.Parameters["newMemberId"].Value = li.memberId;
                    command.Parameters.Add(new NpgsqlParameter("newTourId", NpgsqlDbType.Integer));
                    command.Parameters["newTourId"].Value = li.tourId;
                    command.Parameters.Add(new NpgsqlParameter("newCourseId", NpgsqlDbType.Integer));
                    command.Parameters["newCourseId"].Value = li.courseId;

                    tourId = li.tourId;
                    memberId = li.memberId;

                    command.CommandText = plsql;
                    int course_id = Convert.ToInt32(command.ExecuteScalar());
                }
                
                // uppdaterat resultat i member_tournament till 0 då kortet tas bort.
                plsql = string.Empty;
                plsql = plsql + "UPDATE member_tournament";
                plsql = plsql + "   SET result = :newNetto";
                plsql = plsql + " WHERE tournament_id = :newTourId AND member_id = :newMemberId;";

                command.Parameters.Add(new NpgsqlParameter("newTourId", NpgsqlDbType.Integer));
                command.Parameters["newTourId"].Value = tourId;
                command.Parameters.Add(new NpgsqlParameter("newMemberId", NpgsqlDbType.Integer));
                command.Parameters["newMemberId"].Value = memberId;
                command.Parameters.Add(new NpgsqlParameter("newNetto", NpgsqlDbType.Integer));
                command.Parameters["newNetto"].Value = summa_netto;

                command.CommandText = plsql;
                // tar inte emot returen  
                Convert.ToInt32(command.ExecuteScalar());
                
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
        
        #endregion

        #region skickamail

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

        public static void skickaMailBokningMedlem(DateTime trimDateTime, int memberID)
        {

            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
            string firstname;
            string lastname;
            string mail2;
            string sql = "";
            try
            {
                sql = "select first_name, last_name, mail from member_new where id_member = " + memberID + "";
                conn.Open();

                NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
                NpgsqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    firstname = dr["first_name"].ToString();
                    lastname = dr["last_name"].ToString();
                    mail2 = dr["mail"].ToString();

                    System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                    mail.To.Add(mail2);
                    mail.From = new MailAddress("halslaget@gmail.com", "bokning", System.Text.Encoding.UTF8);
                    mail.Subject = "Bokning av spel på hålslaget";
                    mail.SubjectEncoding = System.Text.Encoding.UTF8;
                    mail.Body = "Hej " + firstname + " " + lastname + " Du har blivit bokad på datum " + trimDateTime;
                    mail.BodyEncoding = System.Text.Encoding.UTF8;
                    mail.IsBodyHtml = true;
                    mail.Priority = MailPriority.High;
                    SmtpClient client = new SmtpClient();
                    client.Credentials = new System.Net.NetworkCredential("halslaget@gmail.com", "halslagetg5");
                    client.Port = 587;
                    client.Host = "smtp.gmail.com";
                    client.EnableSsl = true;
                    client.Send(mail);
                }
            }
            catch (Exception ex)
            {

            }
        }

        #endregion
    }
}
