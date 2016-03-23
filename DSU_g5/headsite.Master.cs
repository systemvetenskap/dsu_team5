using System;
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
            g_newUser.idUser = Convert.ToInt32(Session["idUser"]);
            g_newUser.fkIdMember = Convert.ToInt32(Session["IdMember"]);
            g_idAccess = Convert.ToInt32(Session["IdAccess"]);

            if (g_idAccess > 0)
            {
                Loggin.Visible = false;
                Loggout.Visible = true;
            }
            else
            {
                Loggin.Visible = true;
                Loggout.Visible = false;
            }

            if (!IsPostBack)
            {
                g_newUser.idUser = Convert.ToInt32(Session["idUser"]);
                g_newUser.fkIdMember = Convert.ToInt32(Session["IdMember"]);
                g_idAccess = Convert.ToInt32(Session["IdAccess"]);

                //sidor som inte är tillgängliga för vanliga användare
                List<string> memberDenied = new List<string> 
                {
                    "ASP.admin_aspx",
                    "ASP.medlemsregistrering_aspx",
                    "ASP.resultat_aspx",
                    "ASP.skapatavling_aspx",
                    "ASP.startlistor_aspx",
                    "ASP.scorekort_aspx"
                };

                //sidor som besökare kommer åt (publika sidor)
                List<string> visitorAllowed = new List<string> 
                {
                    "ASP.index_aspx",
                    "ASP.login_aspx",
                    "ASP.tavlingar_aspx",
                    "ASP.tavlingsresultat_aspx"
                };

                if (g_idAccess == 1) //medlem
                {
                    navAdmin.Visible = false;
                    if (memberDenied.Contains(Page.ToString()))
                    {
                        Response.Redirect("index.aspx");
                    }
                }
                else if (g_idAccess == 2 || g_idAccess == 3) //admin
                {

                }
                else //besökare
                {
                    navBokning.Visible = false;
                    navMedlemssida.Visible = false;
                    navAdmin.Visible = false;
                    if (!visitorAllowed.Contains(Page.ToString()))
                    {
                        Response.Redirect("login.aspx");
                    }
                }
            }
        }

        protected void Loggin_Click(object sender, EventArgs e)
        {
            Loggin.Visible = false;
            Loggout.Visible = true;
            FormsAuthentication.RedirectFromLoginPage(g_idAccess.ToString(), false);
            Response.Redirect("login.aspx");
        }

        protected void Loggout_Click(object sender, EventArgs e)
            {
            Loggin.Visible = true;
            Loggout.Visible = false;
                FormsAuthentication.SignOut();
                Session.Abandon();
            FormsAuthentication.RedirectFromLoginPage(g_idAccess.ToString(), false);
            Response.Redirect("index.aspx");
        }
    }
}