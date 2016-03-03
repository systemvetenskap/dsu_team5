using System;
using System.Collections.Generic;
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
        }

        protected void ddlNewsName_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList newsName = (DropDownList)sender;
            ListItem li = newsName.SelectedItem;
            string value = li.Text;
            //int id = Convert.ToInt32(li.Value);

        }
    }
}