using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace DSU_g5
{
    public partial class scorekort : System.Web.UI.Page
    {
        results newResult = new results();
        public int g_acc_member;
        public int g_tournament_id;
        public int tournamentIndex;
        public int participantIndex;

        protected void Page_Load(object sender, EventArgs e)
        {
            lbUserMessage.Text = "";
            g_tournament_id = Convert.ToInt32(Session["TournamentId"]);
            g_acc_member = Convert.ToInt32(Session["AccMember"]);

            tournamentIndex = Convert.ToInt16(Session["tournamentIndex"]);
            participantIndex = Convert.ToInt16(Session["participantIndex"]);

            if (g_tournament_id > 0 && g_acc_member > 0)
            {
                getTries();
            }
        }

        public void getTries()
        {
            // resulatet existerar redan för respektive tävling och medlem, gå till update lägge
            List<results> resultsList = new List<results>();
            if (methods.checkResultExist(g_tournament_id, g_acc_member) == true)
            {
                btnupdate.Text = "Uppdatera scorecard";
                resultsList = methods.getExistsResults(g_tournament_id, g_acc_member);
            }
            else
            {
                resultsList = methods.getDefaultResults(g_tournament_id, g_acc_member);
                btnupdate.Text = "Lägg till scorecard";
            }

            for (int i = 0; i < resultsList.Count; i++)
            {
                if (i == 0)
                    txb1.Text = resultsList[i].tries.ToString();
                if (i == 1)
                    txb2.Text = resultsList[i].tries.ToString();
                if (i == 2)
                    txb3.Text = resultsList[i].tries.ToString();
                if (i == 3)
                    txb4.Text = resultsList[i].tries.ToString();
                if (i == 4)
                    txb5.Text = resultsList[i].tries.ToString();
                if (i == 5)
                    txb6.Text = resultsList[i].tries.ToString();
                if (i == 6)
                    txb7.Text = resultsList[i].tries.ToString();
                if (i == 7)
                    txb8.Text = resultsList[i].tries.ToString();
                if (i == 8)
                    txb9.Text = resultsList[i].tries.ToString();
                if (i == 9)
                    txb10.Text = resultsList[i].tries.ToString();
                if (i == 10)
                    txb11.Text = resultsList[i].tries.ToString();
                if (i == 11)
                    txb12.Text = resultsList[i].tries.ToString();
                if (i == 12)
                    txb13.Text = resultsList[i].tries.ToString();
                if (i == 13)
                    txb14.Text = resultsList[i].tries.ToString();
                if (i == 14)
                    txb15.Text = resultsList[i].tries.ToString();
                if (i == 15)
                    txb16.Text = resultsList[i].tries.ToString();
                if (i == 16)
                    txb17.Text = resultsList[i].tries.ToString();
                if (i == 17)
                    txb18.Text = resultsList[i].tries.ToString();
            }
        }

        protected void btnupdate_Click(object sender, EventArgs e)
        {
            newResult.tourId = g_tournament_id;
            newResult.memberId = g_acc_member;

            if (newResult.tourId > 0 && newResult.memberId > 0)
            {
                if (checkField() == false)
                {
                    return;
                }
                // resulatet existerar redan för respektive tävling och medlem, gå till update lägge
                if (methods.checkResultExist(newResult.tourId, newResult.memberId) == true)
                {
                    // hämta data för uppdatering 
                    btnupdate.Text = "Uppdatera scorecard";
                    List<results> resultsList = new List<results>();
                    resultsList = methods.getExistsResults(newResult.tourId, newResult.memberId);
                    for (int i = 0; i < resultsList.Count; i++)
                    {
                        if (i == 0)
                        {
                            resultsList[i].tries = Convert.ToInt32(txb1.Text);
                            resultsList[i].netto = getNetto(resultsList[i].tries, resultsList[i].pair, resultsList[i].gamehcp);
                        }
                        else if (i == 1)
                        {
                            resultsList[i].tries = Convert.ToInt32(txb2.Text);
                            resultsList[i].netto = getNetto(resultsList[i].tries, resultsList[i].pair, resultsList[i].gamehcp);
                        }
                        else if (i == 2)
                        {
                            resultsList[i].tries = Convert.ToInt32(txb3.Text);
                            resultsList[i].netto = getNetto(resultsList[i].tries, resultsList[i].pair, resultsList[i].gamehcp);
                        }
                        else if (i == 3)
                        {
                            resultsList[i].tries = Convert.ToInt32(txb4.Text);
                            resultsList[i].netto = getNetto(resultsList[i].tries, resultsList[i].pair, resultsList[i].gamehcp);
                        }
                        else if (i == 4)
                        {
                            resultsList[i].tries = Convert.ToInt32(txb5.Text);
                            resultsList[i].netto = getNetto(resultsList[i].tries, resultsList[i].pair, resultsList[i].gamehcp);
                        }
                        else if (i == 5)
                        {
                            resultsList[i].tries = Convert.ToInt32(txb6.Text);
                            resultsList[i].netto = getNetto(resultsList[i].tries, resultsList[i].pair, resultsList[i].gamehcp);
                        }
                        else if (i == 6)
                        {
                            resultsList[i].tries = Convert.ToInt32(txb7.Text);
                            resultsList[i].netto = getNetto(resultsList[i].tries, resultsList[i].pair, resultsList[i].gamehcp);
                        }
                        else if (i == 7)
                        {
                            resultsList[i].tries = Convert.ToInt32(txb8.Text);
                            resultsList[i].netto = getNetto(resultsList[i].tries, resultsList[i].pair, resultsList[i].gamehcp);
                        }
                        else if (i == 8)
                        {
                            resultsList[i].tries = Convert.ToInt32(txb9.Text);
                            resultsList[i].netto = getNetto(resultsList[i].tries, resultsList[i].pair, resultsList[i].gamehcp);
                        }
                        else if (i == 9)
                        {
                            resultsList[i].tries = Convert.ToInt32(txb10.Text);
                            resultsList[i].netto = getNetto(resultsList[i].tries, resultsList[i].pair, resultsList[i].gamehcp);
                        }
                        else if (i == 10)
                        {
                            resultsList[i].tries = Convert.ToInt32(txb11.Text);
                            resultsList[i].netto = getNetto(resultsList[i].tries, resultsList[i].pair, resultsList[i].gamehcp);
                        }
                        else if (i == 11)
                        {
                            resultsList[i].tries = Convert.ToInt32(txb12.Text);
                            resultsList[i].netto = getNetto(resultsList[i].tries, resultsList[i].pair, resultsList[i].gamehcp);
                        }
                        else if (i == 12)
                        {
                            resultsList[i].tries = Convert.ToInt32(txb13.Text);
                            resultsList[i].netto = getNetto(resultsList[i].tries, resultsList[i].pair, resultsList[i].gamehcp);
                        }
                        else if (i == 13)
                        {
                            resultsList[i].tries = Convert.ToInt32(txb14.Text);
                            resultsList[i].netto = getNetto(resultsList[i].tries, resultsList[i].pair, resultsList[i].gamehcp);
                        }
                        else if (i == 14)
                        {
                            resultsList[i].tries = Convert.ToInt32(txb15.Text);
                            resultsList[i].netto = getNetto(resultsList[i].tries, resultsList[i].pair, resultsList[i].gamehcp);
                        }
                        else if (i == 15)
                        {
                            resultsList[i].tries = Convert.ToInt32(txb16.Text);
                            resultsList[i].netto = getNetto(resultsList[i].tries, resultsList[i].pair, resultsList[i].gamehcp);
                        }
                        else if (i == 16)
                        {
                            resultsList[i].tries = Convert.ToInt32(txb17.Text);
                            resultsList[i].netto = getNetto(resultsList[i].tries, resultsList[i].pair, resultsList[i].gamehcp);
                        }
                        else if (i == 17)
                        {
                            resultsList[i].tries = Convert.ToInt32(txb18.Text);
                            resultsList[i].netto = getNetto(resultsList[i].tries, resultsList[i].pair, resultsList[i].gamehcp);
                        }
                    }
                    if (methods.modifyResult(resultsList) == true)
                    {
                        getTries();
                        lbUserMessage.Text = "Uppdatering av slag klar";
                    }
                    else
                    {
                        lbUserMessage.Text = "Vänligen kontrollera så att alla fält är ifyllda";
                    }
                }
                else
                {
                    btnupdate.Text = "Lägg till slag";
                    List<results> resultsList = new List<results>();
                    resultsList = methods.getDefaultResults(newResult.tourId, newResult.memberId);
                    for (int i = 0; i < resultsList.Count; i++)
                    {
                        if (i == 0)
                        {
                            if (txb1.Text == string.Empty)
                            {
                                lbUserMessage.Text = "Fältet måste vara ifyllt";
                            }
                            else
                            {
                                resultsList[i].tries = Convert.ToInt32(txb1.Text);
                                resultsList[i].netto = getNetto(resultsList[i].tries, resultsList[i].pair, resultsList[i].gamehcp);
                            }
                        }
                        else if (i == 1)
                        {
                            resultsList[i].tries = Convert.ToInt32(txb2.Text);
                            resultsList[i].netto = getNetto(resultsList[i].tries, resultsList[i].pair, resultsList[i].gamehcp);
                        }
                        else if (i == 2)
                        {
                            resultsList[i].tries = Convert.ToInt32(txb3.Text);
                            resultsList[i].netto = getNetto(resultsList[i].tries, resultsList[i].pair, resultsList[i].gamehcp);
                        }
                        else if (i == 3)
                        {
                            resultsList[i].tries = Convert.ToInt32(txb4.Text);
                            resultsList[i].netto = getNetto(resultsList[i].tries, resultsList[i].pair, resultsList[i].gamehcp);
                        }
                        else if (i == 4)
                        {
                            resultsList[i].tries = Convert.ToInt32(txb5.Text);
                            resultsList[i].netto = getNetto(resultsList[i].tries, resultsList[i].pair, resultsList[i].gamehcp);
                        }
                        else if (i == 5)
                        {
                            resultsList[i].tries = Convert.ToInt32(txb6.Text);
                            resultsList[i].netto = getNetto(resultsList[i].tries, resultsList[i].pair, resultsList[i].gamehcp);
                        }
                        else if (i == 6)
                        {
                            resultsList[i].tries = Convert.ToInt32(txb7.Text);
                            resultsList[i].netto = getNetto(resultsList[i].tries, resultsList[i].pair, resultsList[i].gamehcp);
                        }
                        else if (i == 7)
                        {
                            resultsList[i].tries = Convert.ToInt32(txb8.Text);
                            resultsList[i].netto = getNetto(resultsList[i].tries, resultsList[i].pair, resultsList[i].gamehcp);
                        }
                        else if (i == 8)
                        {
                            resultsList[i].tries = Convert.ToInt32(txb9.Text);
                            resultsList[i].netto = getNetto(resultsList[i].tries, resultsList[i].pair, resultsList[i].gamehcp);
                        }
                        else if (i == 9)
                        {
                            resultsList[i].tries = Convert.ToInt32(txb10.Text);
                            resultsList[i].netto = getNetto(resultsList[i].tries, resultsList[i].pair, resultsList[i].gamehcp);
                        }
                        else if (i == 10)
                        {
                            resultsList[i].tries = Convert.ToInt32(txb11.Text);
                            resultsList[i].netto = getNetto(resultsList[i].tries, resultsList[i].pair, resultsList[i].gamehcp);
                        }
                        else if (i == 11)
                        {
                            resultsList[i].tries = Convert.ToInt32(txb12.Text);
                            resultsList[i].netto = getNetto(resultsList[i].tries, resultsList[i].pair, resultsList[i].gamehcp);
                        }
                        else if (i == 12)
                        {
                            resultsList[i].tries = Convert.ToInt32(txb13.Text);
                            resultsList[i].netto = getNetto(resultsList[i].tries, resultsList[i].pair, resultsList[i].gamehcp);
                        }
                        else if (i == 13)
                        {
                            resultsList[i].tries = Convert.ToInt32(txb14.Text);
                            resultsList[i].netto = getNetto(resultsList[i].tries, resultsList[i].pair, resultsList[i].gamehcp);
                        }
                        else if (i == 14)
                        {
                            resultsList[i].tries = Convert.ToInt32(txb15.Text);
                            resultsList[i].netto = getNetto(resultsList[i].tries, resultsList[i].pair, resultsList[i].gamehcp);
                        }
                        else if (i == 15)
                        {
                            resultsList[i].tries = Convert.ToInt32(txb16.Text);
                            resultsList[i].netto = getNetto(resultsList[i].tries, resultsList[i].pair, resultsList[i].gamehcp);
                        }
                        else if (i == 16)
                        {
                            resultsList[i].tries = Convert.ToInt32(txb17.Text);
                            resultsList[i].netto = getNetto(resultsList[i].tries, resultsList[i].pair, resultsList[i].gamehcp);
                        }
                        else if (i == 17)
                        {
                            resultsList[i].tries = Convert.ToInt32(txb18.Text);
                            resultsList[i].netto = getNetto(resultsList[i].tries, resultsList[i].pair, resultsList[i].gamehcp);
                        }
                    }
                    if (methods.addResult(resultsList) == true)
                    {
                        getTries();
                        btnupdate.Text = "Uppdatera scorecard";
                        lbUserMessage.Text = "Registrering av slag klar";
                    }
                    else
                    {
                        lbUserMessage.Text = "Vänligen kontrollera att alla fält är ifyllda korrekt";
                    }
                }
            }
        }

        public int getNetto(int tries, int pair, int gamehcp)
        {
            int netto = 0;
            netto = tries - gamehcp;
            return netto;
        }

        protected void clearFields()
        {
            txb1.Text = string.Empty;
            txb2.Text = string.Empty;
            txb3.Text = string.Empty;
            txb4.Text = string.Empty;
            txb5.Text = string.Empty;
            txb6.Text = string.Empty;
            txb7.Text = string.Empty;
            txb8.Text = string.Empty;
            txb9.Text = string.Empty;
            txb10.Text = string.Empty;
            txb11.Text = string.Empty;
            txb12.Text = string.Empty;
            txb13.Text = string.Empty;
            txb14.Text = string.Empty;
            txb15.Text = string.Empty;
            txb16.Text = string.Empty;
            txb17.Text = string.Empty;
            txb18.Text = string.Empty;
            lbUserMessage.Text = "";
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            // hämtar in värdena i globaler
            Session["tournamentIndex"] = tournamentIndex.ToString();
            Session["participantIndex"] = participantIndex.ToString();

            int accessId = Convert.ToInt32(Session["IdAccess"]);
            FormsAuthentication.RedirectFromLoginPage(accessId.ToString(), false);
            Response.Redirect("resultat.aspx");
        }

        public bool checkField()
        {
            bool check = true;
            if ((txb1.Text == string.Empty) || (txb2.Text == string.Empty) || (txb3.Text == string.Empty) ||
                (txb4.Text == string.Empty) || (txb5.Text == string.Empty) || (txb6.Text == string.Empty) ||
                (txb7.Text == string.Empty) || (txb8.Text == string.Empty) || (txb9.Text == string.Empty) ||
                (txb10.Text == string.Empty) || (txb11.Text == string.Empty) || (txb12.Text == string.Empty) ||
                (txb13.Text == string.Empty) || (txb14.Text == string.Empty) || (txb15.Text == string.Empty) ||
                (txb16.Text == string.Empty) || (txb17.Text == string.Empty) || (txb18.Text == string.Empty))
            {
                check = false;
                lbUserMessage.Text = "Samtliga fält måste innehålla heltal.";
            }
            return check;
        }
    }
}
