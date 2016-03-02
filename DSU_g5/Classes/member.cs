using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DSU_g5
{
    public class member
    {
        public int memberId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string address { get; set; }
        public string postalCode { get; set; }
        public string city { get; set; }
        public string mail { get; set; }
        public string gender { get; set; }
        public double hcp { get; set; }
        public string golfId { get; set; }
        public int categoryId { get; set; }
        public string category { get; set; }

        public override string ToString()
        {
            return firstName + " " + lastName;
        }

        public string id_member { get; set; }
    }
}