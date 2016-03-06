using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
        }

        protected void btAdd_Click(object sender, EventArgs e)
        {
            member newMember = new member();
            if (tbIdMember.Text != string.Empty)
            {
                newMember.memberId = Convert.ToInt32(tbIdMember.Text);
            }            
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
            if (tbIdUser.Text != string.Empty)
            {
                newUser.idUser = Convert.ToInt32(tbIdUser.Text);
            }
            newUser.userName = tbUserName.Text;
            newUser.userPassword = tbUserPassword.Text;
            if (methods.addMember(newMember, newUser) == true)
            {
                lbUserMessage.Text = "Användare registrerad";
            }
        }

        protected void btUpdate_Click(object sender, EventArgs e)
        {
            member newMember = new member();
            if (tbIdMember.Text != string.Empty)
            {
                newMember.memberId = Convert.ToInt32(tbIdMember.Text);
            }   
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
            if (tbIdUser.Text != string.Empty)
            {
                newUser.idUser = Convert.ToInt32(tbIdUser.Text);
            }
            newUser.userName = tbUserName.Text;
            newUser.userPassword = tbUserPassword.Text;

            if (methods.modifyMember(newMember, newUser) == true)
            {
                lbUserMessage.Text = "Uppdatering klar";
            }
        }

        protected void btRemove_Click(object sender, EventArgs e)
        {
            member newMember = new member();
            newMember.memberId = Convert.ToInt32(tbIdMember.Text);

            users newUser = new users();
            newUser.idUser = Convert.ToInt32(tbIdUser.Text);
            if (methods.removeMember(newMember, newUser) == true)
            {
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
                ddlGender.Text = newMember.gender.ToString();
                tbHcp.Text = newMember.hcp.ToString();
                tbMail.Text = newMember.mail;
                tbGolfId.Text = newMember.golfId;

                ddlCategory.SelectedItem.Value = newMember.categoryId.ToString();
                ddlCategory.SelectedItem.Text = newMember.category;

                ddlAccessCategory.SelectedItem.Value = newMember.accessId.ToString();
                ddlAccessCategory.SelectedItem.Text = newMember.accessCategory;

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

        protected void ddlGender_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DropDownList lb = (DropDownList)sender;
            //ListItem li = lb.SelectedItem;
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DropDownList lb = (DropDownList)sender;
            //ListItem li = lb.SelectedItem;
            //int id = Convert.ToInt32(li.Value);
            //string value = li.Text;
        }
    }
}

