using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DSU_g5
{
    public partial class scorekort : System.Web.UI.Page
    {
        results newResult = new results();

          
       #region PAGE_LOAD
        protected void Page_Load(object sender, EventArgs e)
        {
            int g_acc_member = Convert.ToInt32(Session["AccMember"]);
            int g_course_id = Convert.ToInt32(Session["CourseId"]);
            //Session["AccMember"] = g_memberId;
            //Session["GTournamentId"] = g_tournamentId;
          
        }
       #endregion

        #region BTN
        protected void btntries_Click(object sender, EventArgs e)
        {
            
            newResult.tourId = 6;
            newResult.memberId = 12;
            newResult.courseId = 1;
            //newResult.pair = Convert.ToInt32(row.Cells[3].Text);
            //newResult.hcp = Convert.ToInt32(row.Cells[4].Text);
            //newResult.gamehcp = Convert.ToInt32(row.Cells[6].Text);
            //newResult.netto = Convert.ToInt32(row.Cells[7].Text);
         
         

            newResult.tries = Convert.ToInt32(txb1.Text);
            newResult.tries = Convert.ToInt32(txb2.Text);
            newResult.tries = Convert.ToInt32(txb3.Text);
            newResult.tries = Convert.ToInt32(txb4.Text);
            newResult.tries = Convert.ToInt32(txb5.Text);
            newResult.tries = Convert.ToInt32(txb6.Text);
            newResult.tries = Convert.ToInt32(txb7.Text);
            newResult.tries = Convert.ToInt32(txb8.Text);
            newResult.tries = Convert.ToInt32(txb9.Text);
            newResult.tries = Convert.ToInt32(txb10.Text);
            newResult.tries = Convert.ToInt32(txb11.Text);
            newResult.tries = Convert.ToInt32(txb12.Text);
            newResult.tries = Convert.ToInt32(txb13.Text);
            newResult.tries = Convert.ToInt32(txb14.Text);
            newResult.tries = Convert.ToInt32(txb15.Text);
            newResult.tries = Convert.ToInt32(txb16.Text);
            newResult.tries = Convert.ToInt32(txb17.Text);
            newResult.tries = Convert.ToInt32(txb18.Text);

            methods.addTries(newResult);
            clearFields();

        }

       


        protected void btnupdate_Click(object sender, EventArgs e)
        {
            newResult.tourId = 6;
            newResult.memberId = 12;
            newResult.courseId = 1;
            //newResult.pair = Convert.ToInt32(row.Cells[3].Text);
            //newResult.hcp = Convert.ToInt32(row.Cells[4].Text);
            //newResult.gamehcp = Convert.ToInt32(row.Cells[6].Text);
            //newResult.netto = Convert.ToInt32(row.Cells[7].Text);
         

            newResult.tries = Convert.ToInt32(txb1.Text);
            newResult.tries = Convert.ToInt32(txb2.Text);
            newResult.tries = Convert.ToInt32(txb3.Text);
            newResult.tries = Convert.ToInt32(txb4.Text);
            newResult.tries = Convert.ToInt32(txb5.Text);
            newResult.tries = Convert.ToInt32(txb6.Text);
            newResult.tries = Convert.ToInt32(txb7.Text);
            newResult.tries = Convert.ToInt32(txb8.Text);
            newResult.tries = Convert.ToInt32(txb9.Text);
            newResult.tries = Convert.ToInt32(txb10.Text);
            newResult.tries = Convert.ToInt32(txb11.Text);
            newResult.tries = Convert.ToInt32(txb12.Text);
            newResult.tries = Convert.ToInt32(txb13.Text);
            newResult.tries = Convert.ToInt32(txb14.Text);
            newResult.tries = Convert.ToInt32(txb15.Text);
            newResult.tries = Convert.ToInt32(txb16.Text);
            newResult.tries = Convert.ToInt32(txb17.Text);
            newResult.tries = Convert.ToInt32(txb18.Text);

            methods.updateTries();
            clearFields();

        }
#endregion

        #region CLEAR
        protected void clearFields()
        {

            txb1.Text = string.Empty;
            txb2.Text = string.Empty;
            txb3.Text = string.Empty;
            txb4.Text = string.Empty;
            txb5.Text = string.Empty;
            txb6.Text = string.Empty;
            txb7.Text = string.Empty;
            txb8.Text = string.Empty;
            txb9.Text = string.Empty;
            txb10.Text = string.Empty;
            txb11.Text = string.Empty;
            txb12.Text = string.Empty;
            txb13.Text = string.Empty;
            txb14.Text = string.Empty;
            txb15.Text = string.Empty;
            txb16.Text = string.Empty;
            txb17.Text = string.Empty;
            txb18.Text = string.Empty;

        }
        #endregion
    }
}