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
        }


        protected void Button1_Click(object sender, EventArgs e)
        { //try/catch verkar inte fungera. Systemet krashar när man inte väljer datum
            try
            {
                ListBox1.DataSource = methods.getBookedMember(trimDateTime);
                ListBox1.DataBind();
            }
            catch (NpgsqlException ex)
            {
                //Debug.WriteLine(ex.Message);
                Response.Write("<script>alert('" + "Välj ett datum" + "')</script>");
            }
        }

        protected void populateGrvBokning()
        {
            DateTime datum = new DateTime();
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
                    if (Convert.ToInt32(dc.ColumnName) == startingHour + hours - 1 && i > 0){
                        break;
                    }
                    else
                    {
                        //int timeID = i + 1 + dt.Columns.IndexOf(dc) * 6;
                        dr[dc.ColumnName] = ":" + i + "0";
                        //+medlemmar inbokade                        
                    }
                }
                dt.Rows.Add(dr);
            }

            grvBokning.DataSource = dt;
            grvBokning.DataBind();
        }

        protected void grvBokning_DataBound(object sender, EventArgs e)
        {
            DateTime datum = new DateTime(2016, 3, 5);
            List<games> gamesList = methods.getGamesByDate(datum);

            try
            {
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
            LinkButton lb = sender as LinkButton;
            string msg = lb.CommandArgument;

            Response.Write("<script>alert('"+ msg +"')</script>");
        }


        protected void BtnBookAll_Click(object sender, EventArgs e)
        {
            string placeholderMid = hfPlaceholderMemberId.Value;
            int memberID = Convert.ToInt32(placeholderMid);


            string chosenDate = hfChosenDate.Value;

            trimDate = chosenDate.Substring(0, 10);
            trimDateTime = Convert.ToDateTime(trimDate.Substring(0, 10));
            

            //11 är nu hårdkodat och är TimeID. Detta ska bytas ut mot det man väljer i datagriden.
            methods.bookMember(trimDateTime, 11, memberID);
        }





        protected void lbAllMembers_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox lb = (ListBox)sender;
            ListItem li = lb.SelectedItem;
            

            mid = li.Value;
            hfPlaceholderMemberId.Value = mid;

            Debug.WriteLine(mid);

        }
    }
}