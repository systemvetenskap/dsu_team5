using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Npgsql;
using System.Configuration;
using System.Diagnostics;

namespace DSU_g5
{

    public static class methods
    {
        public static List<member> getBookedMember(DateTime selectedDate)
        {
            NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
            List<member> bookingmembers = new List<member>();
            member m;
            game_starts gs;

            string sql = "";
            try
            {
                sql = "SELECT first_name, last_name, gender, g.member_id, hcp, times " +
                        "FROM member m " +
                        "INNER JOIN games g on g.member_id = m.id_member "+
                        "INNER JOIN game_dates gd ON g.date_id = gd.dates_id "+
                        "INNER JOIN game_starts gs ON g.time_id = gs.time_id "+
                        "WHERE gd.dates = '" + selectedDate + "'";
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

                    bookingmembers.Add(m);

                }
                
            }
            catch (NpgsqlException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            
            return bookingmembers;
        }
    }
}
    



