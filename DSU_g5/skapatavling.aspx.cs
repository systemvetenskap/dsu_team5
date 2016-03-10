using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DSU_g5
{
    public partial class skapatavling : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //fyll ddlGameForm
                ddlGameForm.DataValueField = "gameform_id";
                ddlGameForm.DataTextField = "gameform_name";
                ddlGameForm.DataSource = methods.getGameForms();
                ddlGameForm.DataBind();
                
                //fyll ddlMemberCategory
                List<member_category> memberCategoryList = new List<member_category>();
                memberCategoryList = methods.getMemberCategoryList();
                List<ListItem> nyCategoryLista = new List<ListItem>();

                foreach (member_category me in memberCategoryList)
                {
                    nyCategoryLista.Add(new ListItem(me.category, me.categoryId.ToString()));
                }

                ddlMemberCategory.DataTextField = "Text";
                ddlMemberCategory.DataValueField = "Value";
                ddlMemberCategory.DataSource = nyCategoryLista;
                ddlMemberCategory.DataBind();

                //fyll lbContactPerson
                List<member> memberList = new List<member>();
                memberList = methods.getMemberList();
                List<ListItem> nyMemberList = new List<ListItem>();

                foreach (member mb in memberList)
                {
                    nyMemberList.Add(new ListItem(mb.firstName + " " + mb.lastName, mb.memberId.ToString()));
                }

                lbContactPerson.DataTextField = "Text";
                lbContactPerson.DataValueField = "Value";
                lbContactPerson.DataSource = nyMemberList;
                lbContactPerson.DataBind();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            tournament tour = new tournament {
                tour_name = tbName.Text,
                tour_info = taInformation.Value,
                registration_start = calRegStart.SelectedDate,
                registration_end = calRegEnd.SelectedDate,
                tour_start_time = DateTime.Parse(tbStartTime.Text),
                tour_end_time = DateTime.Parse(tbEndTime.Text),
                publ_date_startlists = calPublishList.SelectedDate,
                contact_person = Convert.ToInt32(lbContactPerson.SelectedItem.Value),
                gameform = Convert.ToInt32(ddlGameForm.SelectedItem.Value),
                tour_date = calDate.SelectedDate
            };

            int newTourId = methods.insertTournament(tour);
        }
    }
}