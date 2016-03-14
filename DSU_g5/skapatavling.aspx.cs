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

                //fyll lbFormerSponsors
                lbFormerSponsors.DataValueField = "sponsor_id";
                lbFormerSponsors.DataTextField = "sponsor_name";
                lbFormerSponsors.DataSource = methods.getSponsors();
                lbFormerSponsors.DataBind();               
            }

            lblMessage.Text = "";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (tbName.Text != "" &&
                    taInformation.Value != "" &&
                    calRegStart.SelectedDate != DateTime.MinValue &&
                    calRegEnd.SelectedDate != DateTime.MinValue &&
                    calPublishList.SelectedDate != DateTime.MinValue &&
                    lbContactPerson.SelectedIndex > -1 &&
                    calDate.SelectedDate != DateTime.MinValue)
                {
                    DateTime startTime = new DateTime();
                    DateTime endTime = new DateTime();

                    if (DateTime.TryParse(tbStartTime.Text, out startTime) && DateTime.TryParse(tbEndTime.Text, out endTime))
                    {
                        //skapa tävlingsobjekt
                        tournament tour = new tournament
                        {
                            tour_name = tbName.Text,
                            tour_info = taInformation.Value,
                            registration_start = calRegStart.SelectedDate,
                            registration_end = calRegEnd.SelectedDate,
                            tour_start_time = startTime,
                            tour_end_time = endTime,
                            publ_date_startlists = calPublishList.SelectedDate,
                            contact_person = Convert.ToInt32(lbContactPerson.SelectedItem.Value),
                            gameform = Convert.ToInt32(ddlGameForm.SelectedItem.Value),
                            hole = 18,
                            tour_date = calDate.SelectedDate
                        };

                        int newTourId = methods.insertTournament(tour);

                        //lägg till valda sponsorer
                        foreach (ListItem li in lbSponsors.Items)
                        {
                            methods.insertTour_sponsor(newTourId, Convert.ToInt32(li.Value));
                        }

                        clearFields();
                        lblMessage.Text = " Tävlingen är registrerad.";
                    }
                    else
                    {
                        lblMessage.Text = " Vänligen kontrollera att tävlingens tider är korrekt inskrivna.";
                    }
                }
                else
                {
                    lblMessage.Text = " Vänligen kontrollera att alla uppgifter stämmer.";
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = " Ett fel uppstod. Mer information:\n" + ex.Message.ToString();
            }
        }

        protected void btnNewSponsorAdd_Click(object sender, EventArgs e)
        {
            sponsor sp = new sponsor
            {
                sponsor_name = tbNewSponsorName.Text,
                phone = tbNewSponsorPhone.Text
            };

            methods.insertSponsor(sp);

            //fyll lbFormerSponsors
            lbFormerSponsors.DataValueField = "sponsor_id";
            lbFormerSponsors.DataTextField = "sponsor_name";
            lbFormerSponsors.DataSource = methods.getSponsors();
            lbFormerSponsors.DataBind();
        }

        protected void btnFormerSponsorsAdd_Click(object sender, EventArgs e)
        {
            try
            {
                ListItem sponsor = lbFormerSponsors.SelectedItem;
                lbSponsors.Items.Add(sponsor);
                lbSponsors.SelectedIndex = -1;
            }
            catch (Exception)
            {

            }
        }

        protected void btnSponsorsRemove_Click(object sender, EventArgs e)
        {
            try
            {
                lbSponsors.Items.Remove(lbSponsors.SelectedItem);
            }
            catch (Exception)
            {

            }
        }

        //metod för att rensa alla fält
        protected void clearFields()
        {
            tbName.Text = "";
            ddlGameForm.SelectedIndex = 0;
            ddlMemberCategory.SelectedIndex = 0;
            taInformation.Value = "";
            lbContactPerson.SelectedIndex = -1;
            tbSokContactPerson.Text = "";
            calDate.SelectedDate = DateTime.MinValue;
            tbStartTime.Text = ":";
            tbEndTime.Text = ":";
            calRegStart.SelectedDate = DateTime.MinValue;
            calRegEnd.SelectedDate = DateTime.MinValue;
            calPublishList.SelectedDate = DateTime.MinValue;
            lbSponsors.Items.Clear();
            tbNewSponsorName.Text = "";
            tbNewSponsorPhone.Text = "";
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            clearFields();
        }
    }
}