using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DSU_g5
{
    public class games
    {
        public int gameId { get; set; }
        
        public DateTime time {get; set;}
        public DateTime date { get; set; }

        public List<member> memberInGameList = new List<member>();
          
    }
}