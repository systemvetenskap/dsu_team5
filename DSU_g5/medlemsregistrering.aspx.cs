using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DSU_g5
{
    public partial class medlemsregistrering : System.Web.UI.Page
    {
        public users g_newUser = new users();

        protected void Page_Load(object sender, EventArgs e)
        {
            g_newUser.idUser = Convert.ToInt32(Session["idUser"]);
            g_newUser.fkIdMember = Convert.ToInt32(Session["IdMember"]);

            if (!Page.IsPostBack)
            {
                btSave.Text = "Lägg till";
                btRemove.Text = "Ta bort";
                btClear.Text = "Rensa";

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
                populateMemberList();
            }
        }

        protected void btRemove_Click(object sender, EventArgs e)
        {
            lbUserMessage.Text = string.Empty;
            if (tbIdMember.Text != string.Empty)
            {
                member newMember = new member();
                newMember.memberId = Convert.ToInt32(tbIdMember.Text);
                users newUser = new users();
                newUser.idUser = Convert.ToInt32(tbIdUser.Text);
                int currentMemberIndex = lblMembers.SelectedIndex;
                if (methods.removeMember(newMember, newUser) == true)
                {
                    populateMemberList();
                    clearMember();
                    if ((currentMemberIndex - 1) < 0)
                    {
                        lblMembers.SelectedIndex = 0;
                        lblMembers.SelectedItem.Selected = true;
                    }
                    else
                    {
                        lblMembers.SelectedIndex = currentMemberIndex - 1;
                        lblMembers.SelectedItem.Selected = true;
                    }
                    btSave.Text = "Lägg till";
                    lbUserMessage.Text = "Användare " + newMember.memberId.ToString() + " borttagen.";
                }
            }
            else
            {
                lbUserMessage.Text = "Ingen användare att ta bort.";
            }
        }

        protected void lblMembers_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idMember = Convert.ToInt32(lblMembers.SelectedItem.Value);
            clearMember();
            populateMember(idMember);
            btSave.Text = "Uppdatera";
        }

        protected void btClear_Click(object sender, EventArgs e)
        {
            clearMember();
            lblMembers.ClearSelection();
            btSave.Text = "Lägg till";
        }

        protected void btSave_Click(object sender, EventArgs e)
        {
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
            if (tbIdMember.Text != string.Empty)
            {
                if (methods.checkUserExist(Convert.ToInt32(tbIdUser.Text), tbUserName.Text, tbUserPassword.Text) == true)
                {
                    lbUserMessage.Text = "Användarnamn existerar redan. Välj en annan användarnamn.";
                    return;
                }
            }
            else
            {
                if (methods.checkUserExist(tbUserName.Text, tbUserPassword.Text) == true)
                {
                    lbUserMessage.Text = "Användarnamn existerar redan. Välj en annan användarnamn.";
                    return;
                }
            }
            
            if (tbIdMember.Text != string.Empty)
            {
                // update
                lbUserMessage.Text = string.Empty;

                member newMember = new member();
                newMember.memberId = Convert.ToInt32(tbIdMember.Text);
                newMember.firstName = tbFirstName.Text;
                newMember.lastName = tbLastName.Text;
                newMember.address = tbAddress.Text;
                newMember.postalCode = tbPostalCode.Text;
                newMember.city = tbCity.Text;
                newMember.mail = tbMail.Text;
                newMember.gender = ddlGender.Text;
                if (tbHcp.Text == string.Empty)
                {
                    newMember.hcp = 0.00; 
                }
                else
                {
                    newMember.hcp = Convert.ToDouble(tbHcp.Text);
                }
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
                
                int currentMemberIndex = lblMembers.SelectedIndex;
                
                if (methods.modifyMember(newMember, newUser) == true)
                {
                    populateMemberList();
                    lblMembers.SelectedIndex = currentMemberIndex;
                    if ((currentMemberIndex) <= 0)
                    {
                        lblMembers.SelectedIndex = 0;
                        lblMembers.SelectedItem.Selected = true;
                    }
                    else
                    {
                        lblMembers.SelectedIndex = currentMemberIndex;
                        lblMembers.SelectedItem.Selected = true;
                    }
                    tbIdMember.Text = newMember.memberId.ToString();
                    tbIdUser.Text = newUser.idUser.ToString();
                    tbFkIdMember.Text = newUser.fkIdMember.ToString();
                    btSave.Text = "Uppdatera";
                    lbUserMessage.Text = "Användare " + tbIdMember.Text + " uppdaterad. ";
                }
            }
            else
            {
                // insert
                lbUserMessage.Text = string.Empty;
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
                if (tbHcp.Text == string.Empty)
                {
                    newMember.hcp = 0.00;
                }
                else
                {
                    newMember.hcp = Convert.ToDouble(tbHcp.Text);
                }

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
                newUser.userName = tbUserName.Text;
                newUser.userPassword = tbUserPassword.Text;

                if (methods.addMember(newMember, newUser) == true)
                {
                    populateMemberList();
                    tbIdMember.Text = newMember.memberId.ToString();
                    tbIdUser.Text = newUser.idUser.ToString();
                    tbFkIdMember.Text = newUser.fkIdMember.ToString();
                    btSave.Text = "Uppdatera";
                    lbUserMessage.Text = "Användare " + tbIdMember.Text + " registrerad.";
                }
            }
        }
        
        protected void populateMemberList()
        {
            List<member> memberList = new List<member>();
            memberList = methods.getMemberList();
            List<ListItem> nyMemberList = new List<ListItem>();

            lblMembers.Items.Clear();
            foreach (member mb in memberList)
            {
                nyMemberList.Add(new ListItem(mb.firstName + " " + mb.lastName, mb.memberId.ToString()));
            }

            lblMembers.DataTextField = "Text";
            lblMembers.DataValueField = "Value";
            lblMembers.DataSource = nyMemberList;
            lblMembers.DataBind();
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
            tbIdUser.Text = string.Empty;
            tbUserName.Text = string.Empty;
            tbUserPassword.Text = string.Empty;
            tbFkIdMember.Text = string.Empty;
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
    }
}