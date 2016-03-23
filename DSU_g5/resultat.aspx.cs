using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Security;

namespace DSU_g5
{
    public partial class resultat : System.Web.UI.Page
    {
        public int g_tournamentId = 0;
        public int g_memberId = 0;
        public string g_gender = "Male";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                lblstate.Visible = false;

                // Steg 1. Hämtar listan med samtliga tillgänliga tävlingar och lägger det i lblTournamentList
                List<tournament> tourList = new List<tournament>();
                tourList = methods.getTourList();

                // använder en extra listan för att lägga nyckel i "Value" och visa enbart information om id't och tour_name    
                List<ListItem> nytourList = new List<ListItem>();
                foreach (tournament li in tourList)
                {
                    nytourList.Add(new ListItem(li.id_tournament.ToString() + " " + li.tour_name, li.id_tournament.ToString()));
                }
                lblTournamentList.DataTextField = "Text";
                lblTournamentList.DataValueField = "Value";
                lblTournamentList.DataSource = nytourList;
                lblTournamentList.DataBind();

                // Steg 2. Hämtar deltagare
                if (lblTournamentList.Items.Count > -1)
                {
                    if (methods.g_tournamentId > 0)
                    {
                        lblTournamentList.SelectedIndex = methods.g_tournamentIndex;
                        // lblTournamentList.SelectedItem.Value = methods.g_tournamentId.ToString();
                        lblTournamentList.SelectedItem.Selected = true;
                    }
                    else
                    {
                        lblTournamentList.SelectedIndex = 0;
                        lblTournamentList.SelectedItem.Selected = true;
                    }
                    if (lblTournamentList.Items.Count > -1)
                    {
                        g_tournamentId = Convert.ToInt32(lblTournamentList.SelectedItem.Value);
                        getParticipantList(g_tournamentId, g_gender);
                    }
                }

                // Steg 3. Hämtar deltagar info
                if (lblParticipantList.Items.Count > -1)
                {
                    if (methods.g_memberId > 0)
                    {
                        lblParticipantList.SelectedIndex = methods.g_participantIndex;
                        // lblParticipantList.SelectedItem.Value = methods.g_memberId.ToString();
                        lblParticipantList.SelectedItem.Selected = true;
                    }
                    else
                    {
                        lblParticipantList.SelectedIndex = 0;
                        lblParticipantList.SelectedItem.Selected = true;
                    }

                    if ((lblTournamentList.Items.Count > -1) && (lblParticipantList.Items.Count > -1))
                    {
                        g_memberId = Convert.ToInt32(lblParticipantList.SelectedItem.Value);
                        getParticipantInfo(g_tournamentId, g_memberId);
                    }
                }
            }
        }

        protected void lblTournamentList_SelectedIndexChanged(object sender, EventArgs e)
        {
            g_tournamentId = Convert.ToInt32(lblTournamentList.SelectedItem.Value);
            g_memberId = 0;

            // rensar listan
            lblParticipantList.DataSource = string.Empty;
            lblParticipantList.DataBind();

            // rensar griden 
            gvParticipantResults.DataSource = string.Empty;
            gvParticipantResults.DataBind();

            getParticipantList(g_tournamentId, g_gender);
        }

        protected void lblParticipantList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // hämtar in värdena i globaler
            g_tournamentId = Convert.ToInt32(lblTournamentList.SelectedItem.Value);
            g_memberId = Convert.ToInt32(lblParticipantList.SelectedItem.Value);

            // rensar griden 
            gvParticipantResults.DataSource = string.Empty;
            gvParticipantResults.DataBind();

            // hämtar uppgifter för aktuell deltagare
            getParticipantInfo(g_tournamentId, g_memberId);
        }

        public void getParticipantList(int tournamentId, string gender)
        {
            // om tävligen finns, hämta alla deltagare för respektive tävling  
            if (tournamentId > 0)
            {
                // andra listan
                List<member> participantList = new List<member>();

                // för respektive tävling och kön
                participantList = methods.getParticipantList(tournamentId, gender);
                List<ListItem> nyparticipantList = new List<ListItem>();
                foreach (member li in participantList)
                {
                    nyparticipantList.Add(new ListItem(li.memberId.ToString() + " " + li.firstName + " " + li.lastName, li.memberId.ToString()));
                }
                lblParticipantList.DataTextField = "Text";
                lblParticipantList.DataValueField = "Value";
                lblParticipantList.DataSource = nyparticipantList;
                lblParticipantList.DataBind();
            }
        }

        public void getParticipantInfo(int tournamentId, int memberId)
        {
            if (tournamentId > 0 && memberId > 0)
            {
                // resulatet existerar redan för respektive tävling och medlem, gå till update lägge
                if (methods.checkResultExist(tournamentId, memberId) == true)
                {
                    // rensar griden 
                    gvParticipantResults.DataSource = string.Empty;
                    gvParticipantResults.DataBind();

                    // hämta data för uppdatering 
                    List<results> resultsList = new List<results>();
                    gvParticipantResults.CssClass = "resultstable";
                    resultsList = methods.getExistsResults(tournamentId, memberId);
                    gvParticipantResults.DataSource = resultsList;
                    gvParticipantResults.DataBind();

                    lblstate.Text = "2";
                    btSave.Text = "Uppdatera slag";
                    lbUserMessage.Text = "";

                }
                // resulatet finns inte, hämtar default värdena  
                else
                {
                    gvParticipantResults.DataSource = string.Empty;
                    gvParticipantResults.DataBind();

                    // resultat grid, default  
                    List<results> resultsList = new List<results>();
                    gvParticipantResults.CssClass = "resultstable";
                    resultsList = methods.getDefaultResults(tournamentId, memberId);
                    gvParticipantResults.DataSource = resultsList;
                    gvParticipantResults.DataBind();

                    lblstate.Text = "1";
                    btSave.Text = "Lägg till slag";
                    lbUserMessage.Text = "";
                }

                GridView gridview = (GridView)gvParticipantResults;
                gridview.HeaderRow.Cells[0].Text = "TävlingId";  // tourId
                gridview.HeaderRow.Cells[1].Text = "MedlemId";  // memberId

                gridview.HeaderRow.Cells[2].Text = "Hål";  // courseId
                gridview.HeaderRow.Cells[3].Text = "Par";  // pair
                gridview.HeaderRow.Cells[4].Text = "HCP/ind";  // hcp (hålsvårighet) 
                gridview.HeaderRow.Cells[5].Text = "Slag";  // Tries
                gridview.HeaderRow.Cells[6].Text = "Erhåll/slag";  // gameHCP
                gridview.HeaderRow.Cells[7].Text = "Netto";  // netto

                gridview.HeaderRow.Cells[0].Visible = false;  // tourId
                gridview.HeaderRow.Cells[1].Visible = false;  // memberId

                foreach (GridViewRow row in gridview.Rows)
                {
                    row.Cells[0].Visible = false;  // tourId
                    row.Cells[1].Visible = false;  // memberId                                         
                }
            }
        }

        protected void btSave_Click(object sender, EventArgs e)
        {
            // globala variabler
            methods.g_tournamentIndex = lblTournamentList.SelectedIndex;
            methods.g_participantIndex = lblParticipantList.SelectedIndex;

            // globala variabler, används på scorekort 
            methods.g_tournamentId = Convert.ToInt32(lblTournamentList.SelectedItem.Value);
            methods.g_memberId = Convert.ToInt32(lblParticipantList.SelectedItem.Value);

            // hämtar in värdena i globaler
            int accessId = Convert.ToInt32(Session["IdAccess"]);
            FormsAuthentication.RedirectFromLoginPage(accessId.ToString(), false);
            Response.Redirect("scorekort.aspx");
        }

        protected void btRemove_Click(object sender, EventArgs e)
        {
            List<results> resultsList = new List<results>();
            // lägger till tävling och deltagarnummer i griden
            GridView gridview = (GridView)gvParticipantResults;
            foreach (GridViewRow row in gridview.Rows)
            {
                results newResult = new results();
                newResult.tourId = Convert.ToInt32(row.Cells[0].Text);
                newResult.memberId = Convert.ToInt32(row.Cells[1].Text);
                newResult.courseId = Convert.ToInt32(row.Cells[2].Text);
                newResult.pair = Convert.ToInt32(row.Cells[3].Text);
                newResult.hcp = Convert.ToInt32(row.Cells[4].Text);
                newResult.tries = Convert.ToInt32(row.Cells[5].Text);
                newResult.gamehcp = Convert.ToInt32(row.Cells[6].Text);
                newResult.netto = Convert.ToInt32(row.Cells[7].Text);
                resultsList.Add(newResult);
            }
            // anrop till db för att ta bort datat
            if (methods.removeResult(resultsList) == true)
            {
                lblstate.Text = "1";
                btSave.Text = "Lägg till slag";
                lbUserMessage.Text = "Resultat borttagen";
            }
        }
    }
}