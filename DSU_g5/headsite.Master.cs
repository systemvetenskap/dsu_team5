﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Npgsql;
using System.Web.Security;

namespace DSU_g5
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        public users g_newUser = new users();
        int g_idAccess;

        protected void Page_Load(object sender, EventArgs e)
        {
            //g_newUser.idUser = Convert.ToInt32(Session["idUser"]);
            //g_newUser.fkIdMember = Convert.ToInt32(Session["IdMember"]);
            //g_idAccess = Convert.ToInt32(Session["IdAccess"]);
            //if (g_idAccess == 1 || g_idAccess == 2 || g_idAccess == 3)
            //{
            //    Loggin.Text = "Logga ut";
            //}
            //else
            //{
            //    Loggin.Text = "Logga in";
            //    FormsAuthentication.SignOut();
            //    Session.Abandon();                
            //    // FormsAuthentication.RedirectFromLoginPage(g_idAccess.ToString(), false);
            //    // Response.Redirect("login.aspx");
            //}
        }

        protected void Loggin_Click(object sender, EventArgs e)
        {
            if (Loggin.Text == "Logga ut")
            {
                Loggin.Text = "Logga in";
                FormsAuthentication.SignOut();
                Session.Abandon();
            }
            else
            {
                Loggin.Text = "Logga ut";
            }
        }
    }
}