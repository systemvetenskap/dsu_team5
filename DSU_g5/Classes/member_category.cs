using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DSU_g5
{
    public class member_category
    {
        public int categoryId { get; set; }
        public string category { get; set; }
        public int cleaningFee { get; set; }
        
        public override string ToString()
        {
            return categoryId + " " + category;
        }

    }
}