using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DSU_g5
{
    public partial class tavlingsresultat : System.Web.UI.Page
    {
        string tourQuery;
        protected void Page_Load(object sender, EventArgs e)
        {
            tourQuery = Request.QueryString["ContentId"];

            if (!IsPostBack)
            {
                DataTable dt = methods.getResultsTable(Convert.ToInt32(tourQuery));
                gvResults.DataSource = dt;
                gvResults.DataBind();

                List<tournament> tourList = new List<tournament>();
                tourList = methods.getTourList();

                ddlAllTournaments.DataValueField = "id_tournament";
                ddlAllTournaments.DataTextField = "tour_name";
                ddlAllTournaments.DataSource = tourList;
                ddlAllTournaments.DataBind();

                ddlAllTournaments.SelectedValue = tourQuery;
            }
        }

        protected void ddlAllTournaments_SelectedIndexChanged(object sender, EventArgs e)
        {
            int tourId = Convert.ToInt32(ddlAllTournaments.SelectedValue);
            DataTable dt = methods.getResultsTable(Convert.ToInt32(tourId));
            gvResults.DataSource = dt;
            gvResults.DataBind();
        }
    }
}