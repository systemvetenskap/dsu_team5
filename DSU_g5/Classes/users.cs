using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DSU_g5
{
    public class users
    {
        public int idUser { get; set; }
        public string userName { get; set; }
        public string userPassword { get; set; }
        public int fkIdMember { get; set; }

        public override string ToString()
        {
            return idUser.ToString() + " " + userName;
        }
    }
}