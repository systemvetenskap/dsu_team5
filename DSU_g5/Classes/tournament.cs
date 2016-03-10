using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DSU_g5
{
    public class tournament
    {
        public int id_tournament { get; set; }
        public string tour_name { get; set; }
        public string tour_info { get; set; }
        public DateTime registration_start { get; set; }
        public DateTime registration_end { get; set; }
        public string tour_start_time { get; set; }
        public string tour_end_time { get; set; }
        public DateTime publ_date_startlists { get; set; }
        public int contact_person { get; set; }
        public int gameform { get; set; }
        public int hole { get; set; }
    }
}