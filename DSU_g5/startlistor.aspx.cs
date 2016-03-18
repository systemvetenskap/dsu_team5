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

                ddlTournamentList.Text = "";
            }
        }

        protected void ddlTournamentList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList tournamentList = (DropDownList)sender;
            ListItem li = tournamentList.SelectedItem;
            tournament newTour = new tournament();
            id_tournament = Convert.ToInt32(li.Value);
            //List<member> randomMemberList = new List<member>();
            ////randomMemberList = methods.participantsByTourId(id_tournament);

            ////LsbParticipants.DataSource = randomMemberList;
            ////LsbParticipants.DataBind();


            //List<string> medlemStarttime = new List<string>();
            //medlemStarttime = methods.participantsByTourId(id_tournament);

            //LsbParticipants.DataSource = medlemStarttime;
            //LsbParticipants.DataBind();

            hfTourId.Value = li.Value;





            //startList.Text = methods.GetIDsFromMemberTournaments(id_tournament).ToString();

            //DropDownList ddlTourName = (DropDownList)sender;
            //ListItem li = ddlTourName.SelectedItem;
            //string value = li.Text;


            //newNews = methods.getNews(news_id);
            //textNews.InnerText = newNews.newsInfo;
        }

        protected void LsbParticipants_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnStartlist_Click(object sender, EventArgs e)
        {
            List<string> medlemStarttime = new List<string>();
            
            string mess = null;
            if (hfTourId.Value != "")
            {

                int numG = Convert.ToInt32(tbMemPerGroup.Text);

                List<member> randomMemberList = new List<member>();

                medlemStarttime = methods.participantsByTourId(Convert.ToInt32(hfTourId.Value), numG, out mess);


                //LsbParticipants.DataSource = medlemStarttime;
                //LsbParticipants.DataBind();
                DataTable dt = new DataTable();
                dt.Columns.Add("MemberID");
                dt.Columns.Add("Förnamn");
                dt.Columns.Add("Efternamn");
                dt.Columns.Add("Starttid");

                string[] splitted;

                foreach (string s in medlemStarttime)
                {

                    splitted = s.Split(' ');
                    dt.Rows.Add(splitted[0], splitted[1], splitted[2], splitted[3]);
                }


                gvRandom.DataSource = dt;
                gvRandom.DataBind();
            }
            else
            {
                Response.Write("<script>alert('" + "Du måste välja en tävling" + "')</script>");
            }


            
        }

        protected void gvRandom_SelectedIndexChanged(object sender, EventArgs e)
        {
            
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

    }
}