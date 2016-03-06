using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DSU_g5
{
    public class access
    {
        public int accessId { get; set; }
        public string accessCategory { get; set; }

        public override string ToString()
        {
            return accessId + " " + accessCategory;
        }
    }
}