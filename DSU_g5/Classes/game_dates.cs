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

        public string startDate { get; set; }
        public string endDate { get; set; }

        public override string ToString()
        {
            return startDate + " " + endDate;


        }
    }
}