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
        }

        protected void btnRemoveNews_Click(object sender, EventArgs e)
        {
            news newNews = new news();
            newNews.newsId = Convert.ToInt32(ddlNewsName.SelectedItem.Value);
            methods.removeNews(newNews);
            populateNewsNameList();
        }

        protected void ddlNewsName_SelectedIndexChanged(object sender, EventArgs e)
        {
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
            
            

