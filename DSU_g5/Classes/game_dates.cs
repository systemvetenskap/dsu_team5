using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DSU_g5
{
    public class game_dates
    {
        public int dateId { get; set; }
        public DateTime dates { get; set; }

        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }

        public override string ToString()
        {
            return dates.ToString();


        }
    }
}