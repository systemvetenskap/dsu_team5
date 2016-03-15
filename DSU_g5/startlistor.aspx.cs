using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DSU_g5
{
    public partial class startlistor : System.Web.UI.Page
    {
        private int id_tournament; 

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                populateTournamentList();

            }
        
        }

        protected void ddlTournamentList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList tournamentList = (DropDownList)sender;
            ListItem li = tournamentList.SelectedItem;
            tournament newTour = new tournament();
            id_tournament = Convert.ToInt32(li.Value);
            List<member> memberList = new List<member>();
            memberList = methods.participantsByTourId(id_tournament);

            LsbParticipants.DataSource = memberList;
            LsbParticipants.DataBind();


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