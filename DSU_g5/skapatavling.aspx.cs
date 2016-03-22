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

                nyCategoryLista.Add(new ListItem("Välj kategori", "0"));

                foreach (member_category me in memberCategoryList)
                {
                    nyCategoryLista.Add(new ListItem(me.category, me.categoryId.ToString()));
                }

                ddlMemberCategory.DataTextField = "Text";
                ddlMemberCategory.DataValueField = "Value";
                ddlMemberCategory.DataSource = nyCategoryLista;
                ddlMemberCategory.DataBind();

                //fyll lbFormerSponsors
                lbFormerSponsors.DataValueField = "sponsor_id";
                lbFormerSponsors.DataTextField = "sponsor_name";
                lbFormerSponsors.DataSource = methods.getSponsors();
                lbFormerSponsors.DataBind();

            }

            //array till kontaktpersonssökningstextboxkontroll
            List<member> memberList = new List<member>();
            memberList = methods.getMemberList();

            foreach (member me in memberList)
            {
                ClientScript.RegisterArrayDeclaration("contactPersons",
                "{label: '" + me.firstName + " " + me.lastName + "', value: '" + me.memberId + "'}");
            }
            
            lblMessage.Text = "";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime date = new DateTime();
                DateTime regStart = new DateTime();
                DateTime regEnd = new DateTime();
                DateTime publishList = new DateTime();
                DateTime startTime = new DateTime();
                DateTime endTime = new DateTime();

                if (tbName.Text != "" &&
                    taInformation.Value != "" &&
                    ddlMemberCategory.SelectedValue != "0" &&
                    DateTime.TryParse(tbRegStartCal.Text, out regStart) &&
                    DateTime.TryParse(tbRegEndCal.Text, out regEnd) &&
                    DateTime.TryParse(tbPublishListCal.Text, out publishList) &&
                    hfContactPerson.Value != "" &&
                    DateTime.TryParse(tbDateCal.Text, out date) &&
                    DateTime.Parse(tbDateCal.Text) >= DateTime.Now &&
                    DateTime.TryParse(tbStartTime.Text, out startTime) &&
                    DateTime.TryParse(tbEndTime.Text, out endTime))
                {
                    
                    //skapa tävlingsobjekt
                    tournament tour = new tournament
                    {
                        tour_name = tbName.Text,
                        tour_info = taInformation.Value,
                        registration_start = regStart,
                        registration_end = regEnd,
                        tour_start_time = startTime,
                        tour_end_time = endTime,
                        publ_date_startlists = publishList,
                        contact_person = Convert.ToInt32(hfContactPerson.Value),
                        gameform = Convert.ToInt32(ddlGameForm.SelectedItem.Value),
                        hole = 18,
                        tour_date = date
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
                    string meddelande = "Registrering misslyckades. Vänligen kontrollera att följande uppgifter stämmer:\\n";
                    
                    if (tbName.Text == "")
                    {
                        meddelande += "\\nTävlingens namn";
                    }
                    if (taInformation.Value == "")
                    {
                        meddelande += "\\nInformation";
                    }
                    if (ddlMemberCategory.SelectedValue == "0")
                    {
                        meddelande += "\\nMedlemskategori";
                    }
                    if (!DateTime.TryParse(tbDateCal.Text, out date) || DateTime.Parse(tbDateCal.Text) < DateTime.Now)
                    {
                        meddelande += "\\nTävlingens datum";
                    }
                    if (!DateTime.TryParse(tbStartTime.Text, out startTime))
                    {
                        meddelande += "\\nStarttid";
                    }
                    if (!DateTime.TryParse(tbEndTime.Text, out endTime))
                    {
                        meddelande += "\\nSluttid";
                    }
                    if (hfContactPerson.Value == "")
                    {
                        meddelande += "\\nKontaktperson";
                    }
                    if (!DateTime.TryParse(tbRegStartCal.Text, out regStart))
                    {
                        meddelande += "\\nFörsta registreringsdatum";
                    }
                    if (!DateTime.TryParse(tbRegEndCal.Text, out regEnd))
                    {
                        meddelande += "\\nSista registreringsdatum";
                    }
                    if (!DateTime.TryParse(tbPublishListCal.Text, out publishList))
                    {
                        meddelande += "\\nStartlistor publiceras";
                    }

                    Response.Write("<script>alert('" + meddelande + "')</script>");
                    lblMessage.Text = " Registrering misslyckades.";
                }
            }
            catch (Exception ex)
            {
                string meddelande = " Ett fel uppstod. Mer information:\\n" + ex.Message.ToString();
                Response.Write("<script>alert('" + meddelande + "')</script>");
                lblMessage.Text = " Registrering misslyckades.";
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

            tbNewSponsorName.Text = "";
            tbNewSponsorPhone.Text = "";
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
            tbSokContactPerson.Text = "";
            hfContactPerson.Value = "";
            tbDateCal.Text = "";
            tbStartTime.Text = "";
            tbEndTime.Text = "";
            tbRegStartCal.Text = "";
            tbRegEndCal.Text = "";
            tbPublishListCal.Text = "";
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