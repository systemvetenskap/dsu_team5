using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DSU_g5
{
    public partial class admin : System.Web.UI.Page
    {
        public news chosenNews;
        //public int news_id;
      
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                populateNewsNameList();
                ddlNewsName.Items.Insert(0, "Välj nyhet");
            }
        }

        #region KNAPPAR
        protected void btnPublish_Click(object sender, EventArgs e)
        {
            if (textNews.InnerText != "")
            {
                if (txtNewNews.Text != "")
                {

                news newNews = new news();
                newNews.newsName = txtNewNews.Text;
                newNews.newsInfo = textNews.InnerText;
                newNews.newsDate = DateTime.Now;
                methods.addNews(newNews);
                populateNewsNameList();
                    ddlNewsName.Items.Insert(0, "Välj nyhet");

                //    methods.addNews(newNews);
                Response.Write("<script>alert('Publicering klar')</script>");
                txtNewNews.Text = string.Empty;
                updateNews.Visible = true;
                removeNews.Visible = true;
                ddlNewsName.Visible = true;
                lblUpdateNews.Visible = true;

                    hfNewsId.Value = "";
            }
            else
            {
                    Response.Write("<script>alert('" + "Fyll i nyhetsrubrik." + "')</script>");
            }
         }
            else
            {
                Response.Write("<script>alert('Fyll i nyhetsinfo.')</script>");
            }
        }
        protected void btnUpdateNews_Click(object sender, EventArgs e)
        {
            if (hfNewsId.Value != "" && Convert.ToInt32(hfNewsId.Value) != 0)
            {
            news newNews = new news();
            newNews.newsId = Convert.ToInt32(ddlNewsName.SelectedItem.Value);         
            newNews.newsName = txtNewNews.Text;
            newNews.newsInfo = textNews.InnerText;
            newNews.newsDate = DateTime.Now;
          
            methods.updateNews(newNews);
            methods.getNewsList();
            populateNewsNameList();

            txtNewNews.Text = string.Empty;
            updateNews.Visible = true;
            removeNews.Visible = true;
            ddlNewsName.Visible = true;
            lblUpdateNews.Visible = true;

                hfNewsId.Value = "";

            //if (methods.updateNews(newNews) == true)
            //{
            //    methods.updateNews(newNews);
                Response.Write("<script>alert('Uppdatering klar.')</script>");
                populateNewsNameList();
                ddlNewsName.Items.Insert(0, "Välj nyhet");
            //}
        }
            else
            {
                Response.Write("<script>alert('" + "Du måste välja en nyhet." + "')</script>");
            }
        }
        protected void btnRemoveNews_Click(object sender, EventArgs e)
        {

            if (hfNewsId.Value != "" && Convert.ToInt32(hfNewsId.Value) != 0)
            {
            if (textNews.InnerText != "")
            {
                news newNews = new news();
                newNews.newsId = Convert.ToInt32(ddlNewsName.SelectedItem.Value);
                methods.removeNews(newNews);
                populateNewsNameList();

                //if (methods.removeNews(newNews) == true)
                //{
                //    methods.removeNews(newNews);
                Response.Write("<script>alert('Nyhet är borttagen')</script>");
                    populateNewsNameList();
                    ddlNewsName.Items.Insert(0, "Välj nyhet");
                //}
                txtNewNews.Text = string.Empty;
                updateNews.Visible = true;
                removeNews.Visible = true;
                ddlNewsName.Visible = true;
                lblUpdateNews.Visible = true;

                    hfNewsId.Value = "";
            }
            else
            {
                    Response.Write("<script>alert('Välj en nyhet att ta bort.')</script>");
            }
        }
            else
            {
                Response.Write("<script>alert('" + "Du måste välja en nyhet." + "')</script>");
            }

        }
        protected void btnMailNews_Click(object sender, EventArgs e)
        {
            if (textNews.InnerText != "")
            {
                string nyhetsbrev = textNews.InnerText;
                string rubrik = ddlNewsName.SelectedItem.ToString();
                methods.SkickaMail(nyhetsbrev, rubrik);

                //if (methods.SkickaMail(nyhetsbrev, rubrik) == true)
                //{
                methods.SkickaMail(nyhetsbrev, rubrik);
                Response.Write("<script>alert('Nyhetsbrev är sänt till medlemmar.')</script>");
                //}
                txtNewNews.Text = "";
                updateNews.Visible = true;
                removeNews.Visible = true;
                ddlNewsName.Visible = true;
                lblUpdateNews.Visible = true;
            }
            else
            {
                Response.Write("<script>alert('Välj en nyhet att maila.')</script>");
            }      
        }
        protected void btnAddSeason_Click(object sender, EventArgs e)
        {
            //DateTime startDate = startCalendar.SelectedDate;
            //DateTime endDate = endCalendar.SelectedDate;
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            DateTime.TryParse(tbSeasonStartCal.Text, out startDate);
            DateTime.TryParse(tbSeasonEndCal.Text, out endDate);

            if (startDate == DateTime.MinValue || endDate == DateTime.MinValue)
            {
                Response.Write("<script>alert('Välj till- och från-datum.')</script>");
            
            }

            else if (startDate > endDate)
            {
                Response.Write("<script>alert('Välj från-datum i vänstra kalendern och till-datum i den högra ')</script>");
            }
            else
            {
                lblConformation.Text = "Du har lagt till " + startDate.ToShortDateString() + " till " + endDate.ToShortDateString();
                while (startDate <= endDate)
                {
                    methods.addSeason(startDate);
                    startDate = startDate.AddDays(1);
                }
            }
        }

        protected void btnRemoveDate_Click(object sender, EventArgs e)
        {
            //DateTime startDate = startCalendar.SelectedDate;
            //DateTime endDate = endCalendar.SelectedDate;
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            DateTime.TryParse(tbSeasonStartCal.Text, out startDate);
            DateTime.TryParse(tbSeasonEndCal.Text, out endDate);

            if (startDate != DateTime.MinValue && endDate == DateTime.MinValue)
            {
                try
                {
                    
                    DateTime startTime = DateTime.Parse(txtFrom.Text);
                    DateTime endTime = DateTime.Parse(txtTo.Text);
                    lblConformation.Text = "Du har stängt banan på datum: " + startDate.ToShortDateString() + "\n och på tiderna " + startTime.ToShortTimeString() + " till " + endTime.ToShortTimeString() + ".";
                    //int startTime = int.Parse(txtFrom.Text);
                    //int endTime = int.Parse(txtTo.Text);
                    methods.stangbanan(startDate, startTime, endTime);
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('Ett fel uppstod. Mer information:\n" + ex.Message + "')</script>");
                }
            }
            else if (startDate == DateTime.MinValue || endDate == DateTime.MinValue)
            {
                Response.Write("<script>alert('Välj till- och från-datum.')</script>");
            }
            else if (startDate > endDate)
            {
                Response.Write("<script>alert('Välj från-datum i övre kalendern och till-datum i den undre.')</script>");
            }
            else
            {
                if (txtTo.Text != "" || txtFrom.Text != "")
                {
                    Response.Write("<script>alert('Välj endast datum i översta tabellen när du vill stänga banan på specifik tid.')</script>");
                }
                else
                {
                    lblConformation.Text = "Du har tagit bort " + startDate.ToShortDateString() + " till " + endDate.ToShortDateString();
                    while (startDate <= endDate)
                    {
                        methods.removeSeason(startDate, endDate);
                        startDate = startDate.AddDays(1);
                    }
                }
            }


        }


        #endregion


        #region VOIDMETODER
        public void populateNewsNameList()
        {
            List<news> newsList = new List<news>();
            newsList = methods.getNewsList();
            List<ListItem> nyListaNews = new List<ListItem>();

            foreach (news ne in newsList)
            {
                nyListaNews.Add(new ListItem(ne.newsName, ne.newsId.ToString()));
            }
            ddlNewsName.DataTextField = "Text";
            ddlNewsName.DataValueField = "Value";
            ddlNewsName.DataSource = nyListaNews;
            ddlNewsName.DataBind();
            //ddlNewsName.Items.Insert(0, "Välj nyhet");

            textNews.InnerText = string.Empty;
        }

        #endregion


        #region SELECTEDINDEXCHANGED
        protected void ddlNewsName_SelectedIndexChanged(object sender, EventArgs e)
        {
            publishNews.Visible = false;
            lblNewNews.Visible = false;
            

            DropDownList newsName = (DropDownList)sender;
            ListItem li = newsName.SelectedItem;


            if (li.Value == "Välj nyhet")
            {
                lblNewNews.Visible = true;
                txtNewNews.Text = string.Empty;
                textNews.InnerText = "";
                publishNews.Visible = true;
                updateNews.Visible = false;
                removeNews.Visible = false;
                btnMailNews.Visible = false;
                hfNewsId.Value = "";
            }

            else
            {
                updateNews.Visible = true;
                removeNews.Visible = true;
                btnMailNews.Visible = true;

            news newNews = new news();
                //news_id = Convert.ToInt32(li.Value);
                hfNewsId.Value = li.Value;

           
                newNews = methods.getNews(Convert.ToInt32(hfNewsId.Value));
            textNews.InnerText = newNews.newsInfo;
            txtNewNews.Text = newNews.newsName;
            }
            
        }

        #endregion


        #region TextChanged
        protected void txtNewNews_TextChanged(object sender, EventArgs e)
        {
            updateNews.Visible = true;
            removeNews.Visible = true;
            ddlNewsName.Visible = true;
            lblUpdateNews.Visible = true;
            
        }

        #endregion

       
    }
}
