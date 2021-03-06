﻿using System;
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
        }

        protected void btnTourSort_Click(object sender, EventArgs e)
        {

        }

        protected void btnRegister_Click(object sender, CommandEventArgs e)
        {
            string pageID = e.CommandArgument.ToString();

            Response.Write("<script>alert('" + ID + "')</script>");
            Response.Redirect("anmalantavling.aspx?ContentId=" + pageID);
        }

        protected void btnResults_Command(object sender, CommandEventArgs e)
        {
            string pageID = e.CommandArgument.ToString();
            Response.Redirect("tavlingsresultat.aspx?ContentId=" + pageID);
        }
    }
}

