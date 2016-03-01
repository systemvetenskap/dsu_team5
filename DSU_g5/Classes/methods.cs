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
        public void getBookedMember()
        {
            try
            {
                NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["Halslaget"].ConnectionString);
                string sql;
                sql = "SELECT * FROM games g INNER JOIN member m ON g.member_id = m.id_member INNER JOIN game_date gd ON g.date_id = gd.dates_id INNER JOIN game_starts gs ON g.time_id = gs.time_id WHERE game_date = 2016-03-05";
                
            }
            catch (NpgsqlException ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
    



