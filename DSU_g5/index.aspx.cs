using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DSU_g5
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<news> newsList = new List<news>();
            newsList = methods.getNewsList();
            ddlNewsName.DataSource = newsList;
            ddlNewsName.DataBind();

            if (!IsPostBack)
            {
                //Hämta de senaste nyheterna
                DataTable dt = new DataTable();
                dt = methods.getLatestNews();
                RepeaterNews.DataSource = dt;
                RepeaterNews.DataBind();

                populateNewsSortDropdowns();

                //Fyll ddlShowAmount
                List<string> amount = new List<string> { "5", "10", "20", "30" };
                ddlShowAmount.DataSource = amount;
                ddlShowAmount.DataBind();
            }
        }

        protected void ddlNewsName_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList newsName = (DropDownList)sender;
            ListItem li = newsName.SelectedItem;
            string value = li.Text;
        }

        //Lägger värden i dropdown-menyer för val av år och månad
        protected void populateNewsSortDropdowns()
        {
            //år
            int thisYear = Convert.ToInt32(DateTime.Now.Year);
            int firstYear = 2015;
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

        //Hämtar nyheter baserat på val av år och månad
        protected void btnNewsSort_Click(object sender, EventArgs e)
        {
            string startYear = ddlStartYear.Text;
            string endYear = ddlEndYear.Text;
            string em = ddlEndMonth.Text;

            string startDate = startYear + "-" + (ddlStartMonth.SelectedIndex + 1).ToString().PadLeft(2, '0') + "-01";
            string endDate = endYear + "-" + (ddlEndMonth.SelectedIndex + 1).ToString().PadLeft(2, '0');

            if (em == "Februari")
            {
                if (DateTime.IsLeapYear(Convert.ToInt32(endYear)))
                {
                    endDate += "-29";
                }
                else
                {
                    endDate += "-28";
                }
            }
            else if (em == "April" || em == "Juni" || em == "September" || em == "November")
            {
                endDate += "-30";
            }
            else
            {
                endDate += "-31";
            }

            DataTable dt = methods.getNewsByDates(startDate, endDate);
            RepeaterNews.DataSource = dt;
            RepeaterNews.DataBind();
        }
    }
}