using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DSU_g5
{
    public partial class medlemssida : System.Web.UI.Page
    {
        public users g_newUser = new users();

        protected void Page_Load(object sender, EventArgs e)
        {
            g_newUser.idUser = Convert.ToInt32(Session["idUser"]);
            g_newUser.fkIdMember = Convert.ToInt32(Session["IdMember"]);

            if (!Page.IsPostBack)
            {
                // section ett rundor
                DataTable dtr = methods.getGameMember(g_newUser.fkIdMember);
                gvGameMember.DataSource = dtr;
                gvGameMember.DataBind();

                // section två tävlingar
                DataTable dtt = methods.getMemberTournament(g_newUser.fkIdMember);
                gvMemberTournament.DataSource = dtt;
                gvMemberTournament.DataBind();

                // section tre för medlemsuppgifter
                btSave.Text = "Uppdatera";

                ddlGender.Items.Add("Male");
                ddlGender.Items.Add("Female");

                List<member_category> memberCategoryList = new List<member_category>();
                memberCategoryList = methods.getMemberCategoryList();
                List<ListItem> nyCategoryLista = new List<ListItem>();

                foreach (member_category me in memberCategoryList)
                {
                    nyCategoryLista.Add(new ListItem(me.category, me.categoryId.ToString()));
                }

                ddlCategory.DataTextField = "Text";
                ddlCategory.DataValueField = "Value";
                ddlCategory.DataSource = nyCategoryLista;
                ddlCategory.DataBind();

                List<access> accesCategoryList = new List<access>();
                accesCategoryList = methods.getAccesCategoryList();
                List<ListItem> nyAccesLista = new List<ListItem>();

                foreach (access ac in accesCategoryList)
                {
                    nyAccesLista.Add(new ListItem(ac.accessCategory, ac.accessId.ToString()));
                }

                ddlAccessCategory.DataTextField = "Text";
                ddlAccessCategory.DataValueField = "Value";
                ddlAccessCategory.DataSource = nyAccesLista;
                ddlAccessCategory.DataBind();

                populateMember(g_newUser.fkIdMember);
            }
            lbHcp.Visible = false;
            tbHcp.Visible = false;
            lbGolfId.Visible = false;
            lbGolfId.Visible = false;
            lbGolfId.Visible = false;
            tbGolfId.Visible = false;
            lbCategory.Visible = false;
            ddlCategory.Visible = false;
            lbAccessCategory.Visible = false;
            ddlAccessCategory.Visible = false;
            lbPayment.Visible = false;
            cbPayment.Visible = false;
            btRemove.Visible = false;
        }

        protected void btSave_Click(object sender, EventArgs e)
        {
            member newMember = new member();
            // kontroller
            if (tbFirstName.Text == string.Empty)
            {
                lbUserMessage.Text = lbFirstName.Text + " måste ha ett värde.";
                return;
            }
            if (tbUserName.Text == string.Empty)
            {
                lbUserMessage.Text = lbUserName.Text + " måste ha ett värde.";
                return;
            }
            if (methods.checkUserExist(Convert.ToInt32(tbIdUser.Text), tbUserName.Text, tbUserPassword.Text) == true)
            {
                lbUserMessage.Text = "Användarnamn existerar redan. Välj en annan användarnamn.";
                return;
            }

            newMember.memberId = Convert.ToInt32(tbIdMember.Text);
            newMember.firstName = tbFirstName.Text;
            newMember.lastName = tbLastName.Text;
            newMember.address = tbAddress.Text;
            newMember.postalCode = tbPostalCode.Text;
            newMember.city = tbCity.Text;
            newMember.mail = tbMail.Text;
            newMember.gender = ddlGender.Text;
            newMember.hcp = Convert.ToDouble(tbHcp.Text);
            newMember.golfId = tbGolfId.Text;

            DropDownList lbCategory = (DropDownList)ddlCategory;
            ListItem liCategory = lbCategory.SelectedItem;
            int categoryId = Convert.ToInt32(liCategory.Value);
            string category = liCategory.Text;
            newMember.categoryId = categoryId;
            newMember.category = category;

            DropDownList lbAcces = (DropDownList)ddlAccessCategory;
            ListItem liAcces = lbAcces.SelectedItem;
            int accessId = Convert.ToInt32(liAcces.Value);
            string accessCategory = liAcces.Text;
            newMember.accessId = accessId;
            newMember.accessCategory = accessCategory;
            newMember.payment = cbPayment.Checked;

            users newUser = new users();
            newUser.idUser = Convert.ToInt32(tbIdUser.Text);
            newUser.userName = tbUserName.Text;
            newUser.userPassword = tbUserPassword.Text;
            newUser.fkIdMember = newMember.memberId;

            if (methods.modifyMember(newMember, newUser) == true)
            {
                populateMember(newMember.memberId);
                //lbUserMessage.Text = "Uppdatering klar";
                Response.Write("<script>alert('Uppdatering klar')</script>");
            }
        }

        protected void btRemove_Click(object sender, EventArgs e)
        {
            member newMember = new member();
            users newUser = new users();

            newMember.memberId = g_newUser.fkIdMember;
            newUser.idUser = g_newUser.idUser;

            if (methods.removeMember(newMember, newUser) == true)
            {
                clearMember();
                lbUserMessage.Text = "Användare borttagen";
            }
        }

        public void populateMember(int id_member)
        {
            member newMember = new member();
            newMember = methods.getMember(id_member);
            if (newMember.memberId > 0)
            {
                tbIdMember.Text = newMember.memberId.ToString();
                tbFirstName.Text = newMember.firstName;
                tbLastName.Text = newMember.lastName;
                tbAddress.Text = newMember.address;
                tbPostalCode.Text = newMember.postalCode;
                tbCity.Text = newMember.city;
                tbMail.Text = newMember.mail;
                ddlGender.Text = newMember.gender;
                ddlGender.SelectedItem.Selected = true;
                tbHcp.Text = newMember.hcp.ToString();
                tbMail.Text = newMember.mail;
                tbGolfId.Text = newMember.golfId;

                ddlCategory.SelectedIndex = newMember.categoryId - 1;
                ddlCategory.SelectedItem.Selected = true;

                ddlAccessCategory.SelectedIndex = newMember.accessId - 1;
                ddlAccessCategory.SelectedItem.Selected = true;

                cbPayment.Checked = newMember.payment;
            }
            users newUser = new users();
            newUser = methods.getUser(newMember.memberId);
            if (newUser.idUser > 0)
            {
                tbIdUser.Text = newUser.idUser.ToString();
                tbUserName.Text = newUser.userName;
                tbUserPassword.Text = newUser.userPassword;
                tbFkIdMember.Text = newUser.fkIdMember.ToString();
            }
        }

        public void clearMember()
        {
            tbIdMember.Text = string.Empty;
            tbFirstName.Text = string.Empty;
            tbLastName.Text = string.Empty;
            tbAddress.Text = string.Empty;
            tbPostalCode.Text = string.Empty;
            tbCity.Text = string.Empty;
            tbMail.Text = string.Empty;
            tbHcp.Text = string.Empty;
            tbMail.Text = string.Empty;
            tbGolfId.Text = string.Empty;
            lbUserMessage.Text = string.Empty;
        }
    }
} 


