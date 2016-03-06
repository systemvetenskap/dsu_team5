using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace DSU_g5
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btLoggIn_Click(object sender, EventArgs e)
        {
            lbUserMessage.Text = string.Empty;
            if (tbUserName.Text == string.Empty)
            {
                lbUserMessage.Text = "Du måste ange Användarnamn";
                return;
            }
            if (tbUserPassword.Text == string.Empty)
            {
                lbUserMessage.Text = "Du måste ange Lösenord";
                return;
            }
            string userName = tbUserName.Text;
            string userPassword = tbUserPassword.Text;
            // check user name
            if (methods.checkUserNameExist(userName) == false)
            {
                lbUserMessage.Text = "Felaktig användarnamn.";
                return;
            }
            // check password
            if (methods.checkUserPasswordExist(userPassword) == false)
            {
                lbUserMessage.Text = "Felaktig lösenord.";
                return;
            }
            // call db and check if user exist
            if (methods.checkUserExist(userName, userPassword) == true)
            {
                users newUser = new users();
                
                // get all user info by name and password
                newUser = methods.getUserByName(userName, userPassword);
                Session["IdUser"] = newUser.idUser;
                Session["IdMember"] = newUser.fkIdMember;

                // check access1 - alt 1 - blir medlemssida, alt 2 - blir personal, alt 3 - admin
                int accessId = 0;
                accessId = methods.getMemberAccesId(newUser.fkIdMember);
                Session["IdAccess"] = accessId;

                if (accessId == 1)
                {
                    FormsAuthentication.RedirectFromLoginPage(accessId.ToString(), false);
                    Response.Redirect("medlemssida.aspx");
                }
                else if ((accessId == 2) || (accessId == 3))
                {
                    FormsAuthentication.RedirectFromLoginPage(accessId.ToString(), false);
                    Response.Redirect("admin.aspx");
                }
                else
                {
                    lbUserMessage.Text = "Användare saknar behörighet";                
                }
            }
            else
            {
                lbUserMessage.Text = "Fel användarnamn eller lösenord. Försök igen.";
            }
        }
    }
}