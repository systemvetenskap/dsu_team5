﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DSU_g5
{
    public class news
    {
        public int newsId { get; set; }
        public string newsName { get; set; }
        public string newsInfo { get; set; }
        public DateTime newsDate { get; set; }

        public override string ToString()
        {
            return newsId + " " + newsName;


        }
    }
}