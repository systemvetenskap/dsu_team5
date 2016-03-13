using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DSU_g5
{
    public class results
    {
        public int tourId { get; set; }
        public int memberId { get; set; }
        public int courseId { get; set; }
        public int pair { get; set; }
        public int hcp { get; set; }
        public int tries { get; set; }
        public int gamehcp { get; set; }
        public int netto { get; set; }

        public override string ToString()
        {
            return memberId.ToString() + " " + tourId.ToString() + " " + courseId.ToString();
        }
    }
}