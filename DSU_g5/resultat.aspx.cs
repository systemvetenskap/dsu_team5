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
                // Steg 1. Hämtar listan med samtliga tillgänliga tävlingar och lägger det i lblTournamentList
                List<tournament> tourList = new List<tournament>();
                tourList = methods.getTourList();
                // använder en extra listan för att lägga nyckel i "Value" och presentera enbart information om id't och tour_name    
                List<ListItem> nytourList = new List<ListItem>();
                foreach (tournament li in tourList)
                {
                    nytourList.Add(new ListItem(li.id_tournament.ToString() + " " + li.tour_name, li.id_tournament.ToString()));
                }
                lblTournamentList.DataTextField = "Text";
                lblTournamentList.DataValueField = "Value";
                lblTournamentList.DataSource = nytourList;
                lblTournamentList.DataBind();

                // Steg 2. Kontrollerar om det finns rader i tävlingslistan för att hämta vald tävling
                if (lblTournamentList.Items.Count > 0)
                {
                    // om någon rad är vald
                    if (lblTournamentList.SelectedIndex > -1)
                    {
                        g_tournamentId = Convert.ToInt32(lblTournamentList.SelectedItem.Value);
                    }
                    else
                    {
                        lblTournamentList.SelectedIndex = 0;
                        lblTournamentList.SelectedItem.Selected = true;
                        g_tournamentId = Convert.ToInt32(lblTournamentList.SelectedItem.Value);
                    }
                }
                // om tävligen finns, hämta alla deltagare för respektive tävling in list boxen lblParticipantList
                getParticipantList(g_tournamentId, g_gender);

                // Steg 3. kontrollerar om det finns rader i deltagar listan för att hämta
                if (lblParticipantList.Items.Count > 0)
                {
                    // om en delagare är vald
                    if (lblParticipantList.SelectedIndex > -1)
                    {
                        g_memberId = Convert.ToInt32(lblParticipantList.SelectedItem.Value);
                    }
                    // om ingen deltagare vald hämta för deltagare 1
                    else
                    {
                        lblParticipantList.SelectedIndex = 0;
                        lblParticipantList.SelectedItem.Selected = true;
                        g_memberId = Convert.ToInt32(lblParticipantList.SelectedItem.Value);
                    }
                }
                // hämtar uppgifter om aktuell deltagare till griden. Default värdena används.
                getParticipantInfo(g_tournamentId, g_memberId);
            }
        }

        protected void lblTournamentList_SelectedIndexChanged(object sender, EventArgs e)
        {
            g_tournamentId = Convert.ToInt32(lblTournamentList.SelectedItem.Value);

            // rensar globalen
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
            Session["TournamentId"] = g_tournamentId;
            Session["AccMember"] = g_memberId;

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
                    // hämta data för uppdatering 
                    List<results> resultsList = new List<results>();
                    gvParticipantResults.CssClass = "resultstable";
                    resultsList = methods.getExistsResults(tournamentId, memberId);
                    gvParticipantResults.DataSource = resultsList;
                    gvParticipantResults.DataBind();
                    lblstate.Text = "2";
                    btSave.Text = "Uppdatera";
                    lbUserMessage.Text = "";
                }
                // resulatet finns inte, hämtar default värdena  
                else
                {
                    // resultat grid, default  
                    List<results> resultsList = new List<results>();
                    gvParticipantResults.CssClass = "resultstable";
                    resultsList = methods.getDefaultResults(tournamentId, memberId);
                    gvParticipantResults.DataSource = resultsList;
                    gvParticipantResults.DataBind();

                    lblstate.Text = "1";
                    btSave.Text = "Lägg till";
                    lbUserMessage.Text = "";
                }

                // lägger till tävling och deltagarnummer i griden
                GridView gridview = (GridView)gvParticipantResults;
                foreach (GridViewRow row in gridview.Rows)
                {
                    row.Cells[5].Attributes.Add("contenteditable", "true");
                    row.Cells[6].Attributes.Add("contenteditable", "true");
                    row.Cells[7].Attributes.Add("contenteditable", "true");
                }
            }
        }

        protected void btSave_Click(object sender, EventArgs e)
        {
            // hämtar in värdena i globaler
            int accessId = Convert.ToInt32(Session["IdAccess"]);
            // Session["TournamentId"] = g_tournamentId;
            // Session["AccMember"] = g_memberId;
            
            FormsAuthentication.RedirectFromLoginPage(accessId.ToString(), false);
            Response.Redirect("scorekort.aspx");

            /* 
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
            // anrop till db för att spara datat
            if (lblstate.Text == "2")
            {
                if (methods.modifyResult(resultsList) == true)
                {
                    lblstate.Text = "2";
                    btSave.Text = "Uppdatera";
                    lbUserMessage.Text = "Resultat uppdaterad";
                }
                else
                {
                    lblstate.Text = "2";
                    btSave.Text = "Uppdatera";
                    lbUserMessage.Text = "Ett fel har uppståt vid update.";
                }
            }
            else if (lblstate.Text == "1")
            {
                if (methods.addResult(resultsList) == true)
                {
                    lblstate.Text = "2";
                    btSave.Text = "Uppdatera";
                    lbUserMessage.Text = "Resultat sparad";
                }
                else
                {
                    lblstate.Text = "1";
                    btSave.Text = "Lägg till";
                    lbUserMessage.Text = "Ett fel har uppståt vid insert.";
                }
            }
            */ 
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
                btSave.Text = "Lägg till";
                lbUserMessage.Text = "Resultat borttagen";
            }
        }

        protected void btClear_Click(object sender, EventArgs e)
        {
            // rensar samtliga fälten 
            GridView gridview = (GridView)gvParticipantResults;
            foreach (GridViewRow row in gridview.Rows)
            {
                row.Cells[5].Text = "0";
            }
        }
    }
}