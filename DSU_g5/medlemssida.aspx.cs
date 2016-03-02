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

            ddlCategoryId.Items.Add("1");
            ddlCategoryId.Items.Add("2");
            ddlCategoryId.Items.Add("3");
            ddlCategoryId.Items.Add("4");

            ddlCategory.Items.Add("Junior 0 - 12 år");
            ddlCategory.Items.Add("Junior 13 - 21 år");
            ddlCategory.Items.Add("Senior");
            ddlCategory.Items.Add("Studerande");

            // Under utveckling
            //List<member_category> memberCategoryList = new List<member_category>();
            //memberCategoryList = getMemberCategory();
            //ddlCategory.DataSource = memberCategoryList;
            //ddlCategory.DataBind();

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
            newMember.categoryId = Convert.ToInt32(ddlCategoryId.Text);
            newMember.category = ddlCategory.Text;

            // Under utveckling
            //member_category newMemberCategory = new member_category();
            //newMemberCategory = (member_category)ddlCategory.SelectedValue;
            //newMember.categoryId = newMemberCategory.categoryId;
            //newMember.category = newMemberCategory.category;

            methods.addMember(newMember);
        }

        public List<member_category> getMemberCategory()
        {
            List<member_category> memberCategoryList = new List<member_category>();
            
            member_category nyMemberCategory1 = new member_category();            
            nyMemberCategory1.categoryId = 1;
            nyMemberCategory1.category = "Junior 0 - 12 år";
            nyMemberCategory1.cleaningFee = 0;
            memberCategoryList.Add(nyMemberCategory1);

            member_category nyMemberCategory2 = new member_category(); 
            nyMemberCategory2.categoryId = 2;
            nyMemberCategory2.category = "Junior 13 - 21 år";
            nyMemberCategory2.cleaningFee = 0;
            memberCategoryList.Add(nyMemberCategory2);

            member_category nyMemberCategory3 = new member_category(); 
            nyMemberCategory3.categoryId = 3;
            nyMemberCategory3.category = "Senior";
            nyMemberCategory3.cleaningFee = 0;
            memberCategoryList.Add(nyMemberCategory3);

            member_category nyMemberCategory4 = new member_category(); 
            nyMemberCategory4.categoryId = 4;
            nyMemberCategory4.category = "Studerande";
            nyMemberCategory4.cleaningFee = 0;
            memberCategoryList.Add(nyMemberCategory4);
            
            return memberCategoryList;
        }
    }
}