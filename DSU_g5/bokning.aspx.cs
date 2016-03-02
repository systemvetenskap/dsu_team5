using System;
using System.Collections.Generic;
using System.Data;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            List<DateTime> tider = new List<DateTime>();
            populateGrvBokning();

            lbAllMembers.DataSource = methods.showAllMembersForBooking();
            lbAllMembers.DataBind();

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
        {
            string chosenDate = tbDates.Text;
            trimDate = chosenDate.Substring(0, 10);
            trimDateTime = Convert.ToDateTime(trimDate);
            ListBox1.DataSource = methods.getBookedMember(trimDateTime);
            ListBox1.DataBind();
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
                        int timeID = i + 1 + dt.Columns.IndexOf(dc) * 6;
                        dr[dc.ColumnName] = ":" + i + "0" + " " + timeID;
                        //+medlemmar inbokade                        
                    }
                }
                dt.Rows.Add(dr);
            }

            grvBokning.DataSource = dt;
            grvBokning.DataBind();
        }
    }
}