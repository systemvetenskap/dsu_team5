using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DSU_g5
{
    public class game_starts
    {
        public int timeId { get; set; }

        public DateTime times { get; set; }
        public override string ToString()
        {
            return timeId.ToString();
        }
    }
}