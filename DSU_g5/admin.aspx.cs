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
        public int news_id;
      
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                populateNewsNameList();
                
            }
       }
          
        protected void btnPublish_Click(object sender, EventArgs e)
        {
            news newNews = new news();
            newNews.newsName = txtNewNews.Text;
            newNews.newsInfo = textNews.InnerText;
            newNews.newsDate = DateTime.Now;
            methods.addNews(newNews);
            populateNewsNameList();

            if (methods.addNews(newNews) == true)
            {
                methods.addNews(newNews);
              //Response.Write("<script>alert('" + s + "')</script>");

            }
             

         }

        protected void btnUpdateNews_Click(object sender, EventArgs e)
        {
            news newNews = new news();
            newNews.newsId = Convert.ToInt32(ddlNewsName.SelectedItem.Value);         
            newNews.newsName = txtNewNews.Text;
            newNews.newsInfo = textNews.InnerText;
            newNews.newsDate = DateTime.Now;
          
            methods.updateNews(newNews);
            methods.getNewsList();
            populateNewsNameList();


            if (methods.updateNews(newNews) == true)
            {
                methods.updateNews(newNews);
                lblConfirmOfInput.Text = "Uppdatering klar";
            }
          
            
        }

        protected void btnRemoveNews_Click(object sender, EventArgs e)
        {
            news newNews = new news();
            newNews.newsId = Convert.ToInt32(ddlNewsName.SelectedItem.Value);
            methods.removeNews(newNews);
            populateNewsNameList();

            if (methods.removeNews(newNews) == true)
            {
                methods.removeNews(newNews);
                lblConfirmOfInput.Text = "Nyhet är borttagen";
            }
        
        }

        protected void ddlNewsName_SelectedIndexChanged(object sender, EventArgs e)
        {
            
                txtNewNews.Visible = false;
                publishNews.Visible = false;
            
            
            
            DropDownList newsName = (DropDownList)sender;
            ListItem li = newsName.SelectedItem;
            news newNews = new news();
            news_id = Convert.ToInt32(li.Value);
           
            newNews = methods.getNews(news_id); 
            textNews.InnerText = newNews.newsInfo;
           


        }
             
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
            textNews.InnerText = string.Empty;
        }

        protected void btnMailNews_Click(object sender, EventArgs e)
        {
            string nyhetsbrev = textNews.InnerText;
            string rubrik = ddlNewsName.SelectedItem.ToString();
            methods.SkickaMail(nyhetsbrev, rubrik);

            if (methods.SkickaMail(nyhetsbrev, rubrik) == true)
            {
                methods.SkickaMail(nyhetsbrev, rubrik);
                lblConfirmOfInput.Text = "Nyhetsbrev är sänt till medlemmar";
            }
        
            lblConfirmOfInput.Text = "Nyhet är borttagen";
        }

        protected void btnAddSeason_Click(object sender, EventArgs e)
        {
            DateTime startDate = startCalendar.SelectedDate;
            DateTime endDate = endCalendar.SelectedDate;
            if (startCalendar.SelectedDate == DateTime.MinValue || endCalendar.SelectedDate == DateTime.MinValue)
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
            DateTime startDate = startCalendar.SelectedDate;
            DateTime endDate = endCalendar.SelectedDate;
            if (startCalendar.SelectedDate == DateTime.MinValue || endCalendar.SelectedDate == DateTime.MinValue)
            {
                Response.Write("<script>alert('Välj till- och från-datum.')</script>");
            }
            else if (startDate > endDate)
            {
                Response.Write("<script>alert('Välj från-datum i vänstra kalendern och till-datum i den högra ')</script>");
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

        protected void txtNewNews_TextChanged(object sender, EventArgs e)
        {
            updateNews.Visible = false;
            removeNews.Visible = false;
            ddlNewsName.Visible = false;
            
        }
        //public void fillNews(int news_id)
        //{
        //    news newNews = new news();
        //    newNews = methods.getNews(news_Id);
        //    if (newNews.news_id > 0)
        //    {
        //        textNews.InnerText = newNews.newsInfo;
        //   }
        //}
       
    }
}
            
            

