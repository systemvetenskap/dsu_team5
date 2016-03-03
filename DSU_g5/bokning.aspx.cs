using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;



namespace DSU_g5
{
    public partial class bokning : System.Web.UI.Page
    {
        DateTime selectedDate;
        string trimDate;
        DateTime trimDateTime;
        string mid;

        member selectedMember;

        protected void Page_Load(object sender, EventArgs e)
        {
            List<DateTime> tider = new List<DateTime>();
            
            populateGrvBokning();
            lbBookedMembers.Items.Clear();

            if(!IsPostBack)
            {
                lbAllMembers.DataValueField = "mID"; //Får värdet av DataTable och lagrar member_id som en sträng i "mID".
                lbAllMembers.DataTextField = "namn"; //Får värdet av den sammanslagna kolumnen "namn" som en sträng.
                lbAllMembers.DataSource = methods.showAllMembersForBooking();
                lbAllMembers.DataBind();
            }


        }

 
        protected void calBokning_SelectionChanged(object sender, EventArgs e)
        {
            selectedDate = calBokning.SelectedDate;
            hfChosenDate.Value = selectedDate.ToShortDateString();
            //lblTest.Text = selectedDate.ToString();

            populateGrvBokning();
            pBokningarInfo.InnerHtml = "";
        }


        protected void Button1_Click(object sender, EventArgs e)
        { //try/catch verkar inte fungera. Systemet krashar när man inte väljer datum
            //try
            //{
            //    ListBox1.DataSource = methods.getBookedMember(trimDateTime);
            //    ListBox1.DataBind();
            //}
            //catch (NpgsqlException ex)
            //{
            //    //Debug.WriteLine(ex.Message);
            //    Response.Write("<script>alert('" + "Välj ett datum" + "')</script>");
            //}
        }

        protected void populateGrvBokning()
        {
            try
            {
                //DateTime datum = new DateTime();
                DateTime datum = Convert.ToDateTime(hfChosenDate.Value);
                List<member> bookedMembers = new List<member>();
                bookedMembers = methods.getBookedMember(datum);

                int hours = 11;
                int startingHour = 8;
                DataTable dt = new DataTable();

                //skapa kolumner
                for (int i = 0; i < hours; i++)
                {
                    dt.Columns.Add((startingHour + i).ToString().PadLeft(2, '0'));
                }

                //lägg på rader och cellvärden
                for (int i = 0; i < 6; i++)
                {
                    DataRow dr = dt.NewRow();
                    foreach (DataColumn dc in dt.Columns)
                    {
                        //avbryt efter första raden om vi är på den sista kolumnen
                        if (Convert.ToInt32(dc.ColumnName) == startingHour + hours - 1 && i > 0)
                        {
                            break;
                        }
                        else
                        {
                            dr[dc.ColumnName] = ":" + i + "0";                       
                        }
                    }
                    dt.Rows.Add(dr);
                }

                grvBokning.DataSource = dt;
                grvBokning.DataBind();
            }
            catch (Exception ex)
            {
                //Response.Write("<script>alert('" + ex.Message + "')</script>");
            }
        }

        protected void grvBokning_DataBound(object sender, EventArgs e)
        {
            try
            {
                //DateTime datum = new DateTime(2016, 3, 7);
                DateTime datum = Convert.ToDateTime(hfChosenDate.Value);
                List<games> gamesList = methods.getGamesByDate(datum);

                GridView gridview = (GridView)sender;

                int column = 0;
                foreach (GridViewRow gr in gridview.Rows)
                {
                    foreach (TableCell tc in gr.Cells)
                    {
                        //radindex + 1 + columnindex * 6
                        int timeID = gr.RowIndex + 1 + column * 6;

                        if (timeID < 62)
                        {
                            string deltagare = "";
                            string klass = "lbCell";

                            //loopa igenom bokningar för att hitta deltagare och bokningsmöjlighet
                            foreach (games g in gamesList)
                            {
                                if (g.timeId == timeID)
                                {
                                    double totalHcp = 0;
                                    foreach (member m in g.memberInGameList)
                                    {
                                        //deltagarnas kön
                                        if (m.gender == "Male")
                                        {
                                            deltagare += "M";
                                        }
                                        else 
                                        {
                                            deltagare += "F";
                                        }

                                        //deltagarnas totala handicap
                                        totalHcp += m.hcp;
                                    }

                                    if (g.memberInGameList.Count >= 4 || totalHcp > 100)
                                    {
                                        klass = "lbCell_full";
                                    }
                                }
                            }

                            //lägg till linkbutton
                            LinkButton lb = new LinkButton();
                            lb.Text = tc.Text + "<br/><br/>" + deltagare;                            
                            lb.CssClass = klass;
                            lb.CommandArgument = timeID.ToString();
                            lb.Click += new EventHandler(lb_Click);
                            tc.Controls.Add(lb);

                            column++;
                        }                        
                    }
                    column = 0;
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        private void lb_Click(object sender, EventArgs e)
        {
            try
            {
                //hämta data om bokningar på vald tid
                LinkButton lb = sender as LinkButton;
                string timeId = lb.CommandArgument;
                DateTime datum = Convert.ToDateTime(hfChosenDate.Value);
                hfTimeId.Value = timeId;
                List<games> gamesList = methods.getGamesByDate(datum);

                int gameId = 0;
                foreach (games g in gamesList)
                {
                    if (g.timeId.ToString() == timeId)
                    {
                        gameId = g.gameId;
                    }
                }

                lbBookedMembers.DataValueField = "mID";
                lbBookedMembers.DataTextField = "namn";
                lbBookedMembers.DataSource = methods.showAllMembersForBookingByDateAndTime(datum, Convert.ToInt32(timeId));
                lbBookedMembers.DataBind();

                //presentera om golfrunda och deltagare: datum, tid, deltagare, handicap, golf-ID, totalt handicap
                string info = datum.ToShortDateString();
                double totalHcp = 0;
                int iteration = 0;

                foreach (games g in gamesList)
                {
                    if (g.timeId.ToString() == timeId)
                    {
                        if (iteration < 1)
                        {
                            info += "<br/>" + g.time.ToShortTimeString();
                        }
                        
                        foreach (member m in g.memberInGameList)
                        {
                            info += "<br/><br/>" + m.firstName + " " + m.lastName + "<br/>Handicap: " + m.hcp + "<br/>Golf-ID: " + m.golfId;
                            totalHcp += m.hcp;
                        }
                        iteration++;
                    }
                }

                info += "<br/><br/>Totalt handicap: " + Math.Round(totalHcp, 2);
                pBokningarInfo.InnerHtml = info;
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "')</script>");
            }
        }


        protected void BtnBookAll_Click(object sender, EventArgs e)
        {
            string placeholderMid = hfPlaceholderMemberId.Value;
            int memberID = Convert.ToInt32(placeholderMid);
            
            string chosenDate = hfChosenDate.Value;
            trimDate = chosenDate.Substring(0, 10);
            trimDateTime = Convert.ToDateTime(trimDate.Substring(0, 10));
            
            string placeholderTid = hfTimeId.Value;
            int timeID = Convert.ToInt32(placeholderTid);

            
            //11 är nu hårdkodat och är TimeID. Detta ska bytas ut mot det man väljer i datagriden.
            methods.bookMember(trimDateTime, timeID, memberID);
        }



        protected void lbAllMembers_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox lb = (ListBox)sender;
            ListItem li = lb.SelectedItem;
            

            mid = li.Value; //memberID
            hfPlaceholderMemberId.Value = mid;

            Debug.WriteLine(mid);

        }

        protected void BtnDelMemberFromGame_Click(object sender, EventArgs e)
        {
            //SKAPA EN VOIDMETOD();
            string placeholderMid = hfPlaceholderMemberId.Value;
            int memberID = Convert.ToInt32(placeholderMid);
            
            string chosenDate = hfChosenDate.Value;
            trimDate = chosenDate.Substring(0, 10);
            trimDateTime = Convert.ToDateTime(trimDate.Substring(0, 10));
            
            string placeholderTid = hfTimeId.Value;
            int timeID = Convert.ToInt32(placeholderTid);

            methods.unBookMember(trimDateTime, timeID, memberID);
        }

        protected void btnAddSeason_Click(object sender, EventArgs e)
        {
            DateTime startDate = startCalendar.SelectedDate;
            DateTime endDate = endCalendar.SelectedDate;

            while (startDate <= endDate)
            {
                methods.addSeason(startDate);
                startDate = startDate.AddDays(1);
            }
        }
    }
}