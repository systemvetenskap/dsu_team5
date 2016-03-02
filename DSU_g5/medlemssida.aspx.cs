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
        protected void Page_Load(object sender, EventArgs e)
        {

            ddlGender.Items.Add("Male");
            ddlGender.Items.Add("Female");

            // Under utveckling
            // populate(1021);
            List<member_category> memberCategoryList = new List<member_category>();
            memberCategoryList = methods.getMemberCategoryList();
            ddlCategory.DataSource = memberCategoryList;
            ddlCategory.DataBind();

        }

        protected void btAdd_Click(object sender, EventArgs e)
        {
            member newMember = new member();
            newMember.firstName = tbFirstName.Text;
            newMember.lastName = tbLastName.Text;
            newMember.address = tbAddress.Text;
            newMember.postalCode = tbPostalCode.Text;
            newMember.city = tbCity.Text;
            newMember.mail = tbMail.Text;
            newMember.gender = ddlGender.Text;
            newMember.hcp = Convert.ToDouble(tbHcp.Text);
            newMember.golfId = tbGolfId.Text;
            newMember.category = ddlCategory.Text;

            users newUser = new users();
            newUser.userName = tbUserName.Text;
            newUser.userPassword = tbUserPassword.Text;

            methods.addMember(newMember, newUser);
        }

        protected void btRemove_Click(object sender, EventArgs e)
        {
            member newMember = new member();
            newMember.memberId = Convert.ToInt32(tbIdMember.Text);

            users newUser = new users();
            newUser.idUser = Convert.ToInt32(tbIdUser.Text);

            methods.removeMember(newMember, newUser);
        }

        public void populate(int id_member)
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
                newMember.categoryId = Convert.ToInt32(ddlCategoryId.Text);
                ddlCategory.Text = newMember.category;
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

