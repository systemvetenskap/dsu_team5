using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DSU_g5
{
    public partial class tavlingar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            List<tournament> tourList = new List<tournament>();
            tourList = methods.getTourList();
            ddlTourName.DataSource = tourList;
            ddlTourName.DataBind();

            if (!IsPostBack)
            {
                //Hämta de senaste nyheterna
                DataTable dt = new DataTable();
                dt = methods.getLatestTour();
                RepeaterTour.DataSource = dt;
                RepeaterTour.DataBind();

                populateNewsSortDropdowns();

            }
        }

        protected void ddlTourName_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlTourName = (DropDownList)sender;
            ListItem li = ddlTourName.SelectedItem;
            string value = li.Text;
        }

        protected void populateNewsSortDropdowns()
        {
            //år
            int thisYear = Convert.ToInt32(DateTime.Now.Year);
            int firstYear = 2010;
            List<int> years = new List<int>();

            for (int i = firstYear; i <= thisYear; i++)
            {
                years.Add(i);
            }

            years.Sort((y, x) => x.CompareTo(y));
            ddlStartYear.DataSource = years;
            ddlStartYear.DataBind();
            ddlEndYear.DataSource = years;
            ddlEndYear.DataBind();

            //månader
            List<string> months = new List<string> 
            {
                "Januari",
                "Februari",
                "Mars",
                "April",
                "Maj",
                "Juni",
                "Juli",
                "Augusti",
                "September",
                "Oktober",
                "November",
                "December"
            };

            ddlStartMonth.DataSource = months;
            ddlStartMonth.DataBind();
            ddlEndMonth.DataSource = months;
            ddlEndMonth.DataBind();
        }
        protected void btnTourSort_Click(object sender, EventArgs e)
        {
            string startYear = ddlStartYear.Text;
            string endYear = ddlEndYear.Text;
            string em = ddlEndMonth.Text;

            string startDate = startYear + "-" + (ddlStartMonth.SelectedIndex + 1).ToString().PadLeft(2, '0') + "-01";
            string endDate = endYear + "-" + (ddlEndMonth.SelectedIndex + 1).ToString().PadLeft(2, '0');

            if (em == "Februari")
            {
                endDate += "-28";
            }
            else if (em == "April" || em == "Juni" || em == "September" || em == "November")
            {
                endDate += "-31";
            }
            else
            {
                endDate += "-30";
            }

            DataTable dt = methods.getTourByDates(startDate, endDate);
            RepeaterTour.DataSource = dt;
            RepeaterTour.DataBind();
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {

            Response.Redirect("anmalantavling.aspx");


            ////ANVÄND IDT FRÅN .aspx!
            ////int i = '<%# Eval("id_tournament")%>';

            ////RepeaterTour.Items.

            ////string s = "<%# Eval(tour_name) %>";



            //foreach (RepeaterItem x in RepeaterTour.Items)
            //{
            //    Label l = (Label)x.FindControl("lblTest");

            //    tezt.Text += l.Text + "<br />";
            //}

            ////foreach (RepeaterItem y in RepeaterTour.Items)
            ////{
            ////    <h2> 
            ////}



            ////tournament t = new tournament();
            ////t.id_tournament = Convert.ToInt32(RepeaterTour.DataSource.ToString());
            ////t.id_tournament = Convert.ToInt32(RepeaterTour.DataSource.GetType());


            ////RepeaterTour.Items.OfType<Header.InnerHtml.Contains<h2>>();

            //int i = RepeaterTour.Items.Count;

            ////if(RepeaterTour.Items.OfType<Header.InnerHtml.Contains<h2>>)

            
            
            //if (RepeaterTour.ClientID.Contains("<%#Eval(id_tournament)%>"))
            //{
            //    Response.Redirect("anmalantavling.aspx");
            //}







            //RepeaterTour.FindControl(Convert.ToString(1));
            //if(RepeaterTour.HasControls())
            //{
            //    //Response.Redirect("anmalantavling.aspx");
            //}
            
            
            ////Response.Write("<script>alert('" + s + "')</script>");

            ////Response.Redirect("anmalantavling.aspx");
        }
    }
}

