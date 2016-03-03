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

            //lbAllMembers.DataSource = methods.showAllMembersForBooking();
            //lbAllMembers.DataBind();

            if(!IsPostBack)
            {
                lbAllMembers.DataValueField = "mID";
                lbAllMembers.DataTextField = "namn";
            lbAllMembers.DataSource = methods.showAllMembersForBooking();
            lbAllMembers.DataBind();
            }

        }

        protected void BtnShowTable_Click(object sender, EventArgs e)
        {
            int totRow = 7;
            int currentRowCount;
            int totCellsInRow = 11;
            int currentCellCount;


            for (currentRowCount = 1; currentRowCount <= totRow; currentRowCount++)
            {
                TableRow tRow = new TableRow();
                TestTable.Rows.Add(tRow);

                for (currentCellCount = 1; currentCellCount <= totCellsInRow; currentCellCount++)
                {
                    TableCell tCell = new TableCell();
                    tRow.Cells.Add(tCell);

                    string textInCell = currentRowCount + " " + currentCellCount;

                    tCell.Controls.Add(new LiteralControl("Rad/Celln"));
                    HyperLink h = new HyperLink();
                    h.Text = currentRowCount + ":" + currentCellCount;
                    h.NavigateUrl = "http://www.nba.com/";
                    tCell.Controls.Add(h);

                    if(currentRowCount % 2 != 0)
                    {
                        //tRow.BorderColor = System.Drawing.Color.Black;
                        //tRow.BorderStyle =
                        tRow.BackColor = System.Drawing.Color.Red;
                    
                    }

                    else if(currentRowCount % 2 == 0)
                    {
                        //tRow.BorderColor = System.Drawing.Color.Yellow;
                        tRow.BackColor = System.Drawing.Color.Green;
                    }
                }
            }
        }

        protected void btnTest0800_Click(object sender, EventArgs e)
        {
            lblTest.Text = "Hej";
        }



        protected void calBokning_SelectionChanged(object sender, EventArgs e)
        {
            selectedDate = calBokning.SelectedDate;
            //lblTest.Text = sender.ToString();
            lblTest.Text = selectedDate.ToString();            
            tbDates.Text = selectedDate.ToString();
           
        }


        protected void Button1_Click(object sender, EventArgs e)
        { //try/catch verkar inte fungera. Systemet krashar när man inte väljer datum
            try
            {
                //string chosenDate = tbDates.Text;
                //trimDate = chosenDate.Substring(0, 10);
                //trimDateTime = Convert.ToDateTime(trimDate);
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
            string placeholderMid = lblPlaceholderMemberId.Text;
            int memberID = Convert.ToInt32(placeholderMid);


            string chosenDate = tbDates.Text;
            trimDate = chosenDate.Substring(0, 10);
            trimDateTime = Convert.ToDateTime(trimDate);


            methods.bookMember(trimDateTime, 11, memberID);
        }



        protected void lbAllMembers_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBox lb = (ListBox)sender;
            ListItem li = lb.SelectedItem;
            



            //string mid = this.lbAllMembers.SelectedItem.Text.ToString();
            mid = li.Value;
            lblPlaceholderMemberId.Text = mid;
            Debug.WriteLine(mid);

            



            //member m = new member();
            //m.firstName = li.Text.Split(' ')[0];

            //selectedMember = (member)lb.Items[li.Selected].Attributes.;

            //selectedMember = member.Parse(li);

           // selectedMember = (member)lbAllMembers.SelectedItem;


        }
    }
}