using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DSU_g5
{
    public partial class anmalantavling : System.Web.UI.Page
    {
        member selectedMember;
        tournament selectedTournament;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {

                //ddlAllTournaments.Items.Insert(0, "Välj tävling");
                List<tournament> tourList = new List<tournament>();
                tourList = methods.getTourList();


                ddlAllTournaments.DataValueField = "id_tournament";
                ddlAllTournaments.DataTextField = "tour_name";
                ddlAllTournaments.DataSource = tourList;
                ddlAllTournaments.DataBind();

                //ddlAllTournaments.SelectedIndex = 0;
                //ddlAllTournaments.SelectedValue = "";
                //ddlAllTournaments.ClearSelection();

                lbMembersTournament.DataValueField = "mID";
                lbMembersTournament.DataTextField = "namn";
                lbMembersTournament.DataSource = methods.showAllMembersForBooking();
                lbMembersTournament.DataBind();

            }
        }

        #region SELECTED INDEX CHANGED
        protected void ddlAllTournaments_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            ListItem li = ddl.SelectedItem;

            tournament tour = new tournament();
            tour.id_tournament = Convert.ToInt32(ddl.SelectedItem.Value);
            //int tournamentID = Convert.ToInt32(ddl.SelectedItem.Value);

            hfTourId.Value = tour.id_tournament.ToString();

            selectedTournament = methods.GetTournament(tour.id_tournament);
            
            lblTournamentInfo.Text = selectedTournament.id_tournament + " " + selectedTournament.tour_name;

            infoAboutTourTBs(tour.id_tournament);

            //Gridview
            gvTourInfo.DataSource = methods.GetInfoAboutTour(tour.id_tournament);
            gvTourInfo.DataBind();

        }

        protected void lbMembersTournament_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox lb = (ListBox)sender;
            ListItem li = lb.SelectedItem;

            member mem = new member();
            mem.memberId = Convert.ToInt32(li.Value);
            //int memberId = Convert.ToInt32(li.Value);

            hfMemberId.Value = mem.memberId.ToString();

            selectedMember = methods.getMember(mem.memberId);

            lblMemberInfo.Text = selectedMember.memberId + " " + selectedMember.firstName + " " + selectedMember.gender + " " + selectedMember.city;
        }

        #endregion




        #region KNAPPAR
        protected void btnRegMemberOnTour_Click(object sender, EventArgs e)
        {
            string message = null;

            if(hfTourId.Value != "")
            {
                if (hfMemberId.Value != "")
                {
                    // KÖR PÅ.
                    int tourId = Convert.ToInt32(hfTourId.Value);
                    int memId = Convert.ToInt32(hfMemberId.Value);
                    int result = 0;

                    methods.RegMemberOnTour(tourId, memId, result, out message);

                    if(message != null)
                    {
                        Response.Write("<script>alert('" + message + "')</script>");
                    }
                }

                else
                {
                    Response.Write("<script>alert('" + "Välj en medlem i listan" + "')</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('" + "Välj en tävling i listan." + "')</script>");
            }
        }

        #endregion


        private void infoAboutTourTBs(int tourId)
        {
            tbTourName.Text = selectedTournament.tour_name;
            tbTourInfo.Text = selectedTournament.tour_info;
            tbTourDate.Text = selectedTournament.tour_date.ToShortDateString();
            tbRegStart.Text = selectedTournament.registration_start.ToShortDateString();
            tbRegEnd.Text = selectedTournament.registration_end.ToShortDateString();
            tbTourStart.Text = selectedTournament.tour_start_time.ToShortTimeString();
            tbTourEnd.Text = selectedTournament.tour_end_time.ToShortTimeString();
            tbContactPerson.Text = Convert.ToString(methods.ContactPersonName(tourId)); //Lägger till en 0a, vet inte varför.
            tbHole.Text = selectedTournament.hole.ToString();
        }

        protected void gvTourInfo_DataBound(object sender, EventArgs e)
        {

        }
    }
}