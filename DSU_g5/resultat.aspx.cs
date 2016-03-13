using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace DSU_g5
{
    public partial class resultat : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<results> resultsList = new List<results>();
            gvParticipantResults.CssClass = "resultstable";
            resultsList = methods.getParticipantResults();
            
            gvParticipantResults.DataSource = resultsList;
            gvParticipantResults.DataBind();
        }

        protected void gvParticipantResults_DataBound(object sender, EventArgs e)
        {
            GridView gridview = (GridView)sender;

            foreach (GridViewRow row in gridview.Rows)
            {
                row.Cells[5].Attributes.Add("contenteditable", "true");
            }
        }

    }
}