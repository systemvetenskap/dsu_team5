using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DSU_g5
{
    public partial class startlistor : System.Web.UI.Page
    {
        public int id_tournament; 

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                populateTournamentList();
                populateTournamentWithST();
                ddlTournamentList.Items.Insert(0, "Välj tävling");
                ddlTourWithStarttime.Items.Insert(0, "Välj tävling");
            }
        }

        protected void ddlTournamentList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList tournamentList = (DropDownList)sender;
            ListItem li = tournamentList.SelectedItem;
            //tournament newTour = new tournament();
            if (li.Value == "Välj tävling")
            {
                gvRandom.DataSource = null;
                gvRandom.DataBind();
            }
            else
            {
                id_tournament = Convert.ToInt32(li.Value);

                hfTourId.Value = li.Value;
            }
        }

        //protected void LsbParticipants_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //}

        protected void btnStartlist_Click(object sender, EventArgs e)
        {
            List<string> medlemStarttime = new List<string>();
            
            string mess = null;
            if (hfTourId.Value != "")
            {

                int numG = Convert.ToInt32(tbMemPerGroup.Text);

                List<member> randomMemberList = new List<member>();

                medlemStarttime = methods.participantsByTourId(Convert.ToInt32(hfTourId.Value), numG, out mess);

                if (medlemStarttime.Count >= 1)
                {//LsbParticipants.DataSource = medlemStarttime;
                    //LsbParticipants.DataBind();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("MemberID");
                    dt.Columns.Add("Namn");
                    //dt.Columns.Add("Förnamn");
                    //dt.Columns.Add("Efternamn");
                    dt.Columns.Add("Starttid");

                    string[] splitted;

                    foreach (string s in medlemStarttime)
                    {

                        splitted = s.Split(' ');
                        dt.Rows.Add(splitted[0], splitted[1] + " " + splitted[2], splitted[3]);
                    }


                    gvRandom.DataSource = dt;
                    gvRandom.DataBind();
                }
                else
                {
                    Response.Write("<script>alert('" + "Tävlingen har inga deltagare." + "')</script>");
                    gvRandom.DataSource = null;
                    gvRandom.DataBind();
                }

            }
            else
            {
                Response.Write("<script>alert('" + "Du måste välja en tävling" + "')</script>");
            }


            
        }


        public void populateTournamentList()
        {
            List<tournament> tourList = new List<tournament>();
            tourList = methods.getTourList();

            List<ListItem> newListTournaments = new List<ListItem>();
            foreach (tournament tour in tourList)
            {
                newListTournaments.Add(new ListItem(tour.tour_name, tour.id_tournament.ToString()));
            }

            ddlTournamentList.DataValueField = "id_tournament";
            ddlTournamentList.DataTextField = "tour_name";
            ddlTournamentList.DataSource = tourList;
            ddlTournamentList.DataBind();

        }

        public void populateTournamentWithST()
        {
            ddlTourWithStarttime.DataTextField = "tour_name";
            ddlTourWithStarttime.DataValueField = "id_tournament";
            ddlTourWithStarttime.DataSource = methods.toursWithST();
            ddlTourWithStarttime.DataBind();
        }

        protected void ddlTourWithStarttime_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlTourWithStarttime = (DropDownList)sender;
            ListItem li = ddlTourWithStarttime.SelectedItem;
            hfTourWithST.Value = li.Value;

            if (li.Value == "Välj tävling")
            {
                gvHasStartlist.DataSource = null;
                gvHasStartlist.DataBind();
            }
            else
            {
                int tourID = Convert.ToInt32(hfTourWithST.Value);

                gvHasStartlist.DataSource = methods.getMembersWithST(tourID);
                gvHasStartlist.DataBind();
            }
        }

    }
}