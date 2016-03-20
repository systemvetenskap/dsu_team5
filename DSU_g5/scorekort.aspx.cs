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
       public int g_acc_member;
       public int g_tournament_id; 
          
       #region PAGE_LOAD
        protected void Page_Load(object sender, EventArgs e)
        {
            g_tournament_id = Convert.ToInt32(Session["TournamentId"]);
            g_acc_member = Convert.ToInt32(Session["AccMember"]);
        }
       #endregion

        #region BTN
        protected void btntries_Click(object sender, EventArgs e)
        {
            newResult.tourId = g_tournament_id;
            newResult.memberId = g_acc_member;

            //newResult.pair = Convert.ToInt32(row.Cells[3].Text);
            //newResult.hcp = Convert.ToInt32(row.Cells[4].Text);
            //newResult.gamehcp = Convert.ToInt32(row.Cells[6].Text);
            //newResult.netto = Convert.ToInt32(row.Cells[7].Text);

            newResult.courseId = 1;
            newResult.tries = Convert.ToInt32(txb1.Text);
            methods.addTries(newResult);

            newResult.courseId = 2;
            newResult.tries = Convert.ToInt32(txb2.Text);
            methods.addTries(newResult);

            newResult.courseId = 3; 
            newResult.tries = Convert.ToInt32(txb3.Text);
            methods.addTries(newResult);

            newResult.courseId = 4; 
            newResult.tries = Convert.ToInt32(txb4.Text);
            methods.addTries(newResult);

            newResult.courseId = 5; 
            newResult.tries = Convert.ToInt32(txb5.Text);
            methods.addTries(newResult);

            newResult.courseId = 6; 
            newResult.tries = Convert.ToInt32(txb6.Text);
            methods.addTries(newResult);

            newResult.courseId = 7; 
            newResult.tries = Convert.ToInt32(txb7.Text);
            methods.addTries(newResult);

            newResult.courseId = 8; 
            newResult.tries = Convert.ToInt32(txb8.Text);
            methods.addTries(newResult);

            newResult.courseId = 9; 
            newResult.tries = Convert.ToInt32(txb9.Text);
            methods.addTries(newResult);

            newResult.courseId = 10; 
            newResult.tries = Convert.ToInt32(txb10.Text);
            methods.addTries(newResult);

            newResult.courseId = 11; 
            newResult.tries = Convert.ToInt32(txb11.Text);
            methods.addTries(newResult);

            newResult.courseId = 12; 
            newResult.tries = Convert.ToInt32(txb12.Text);
            methods.addTries(newResult);

            newResult.courseId = 13; 
            newResult.tries = Convert.ToInt32(txb13.Text);
            methods.addTries(newResult);

            newResult.courseId = 14; 
            newResult.tries = Convert.ToInt32(txb14.Text);
            methods.addTries(newResult);

            newResult.courseId = 15; 
            newResult.tries = Convert.ToInt32(txb15.Text);
            methods.addTries(newResult);

            newResult.courseId = 16; 
            newResult.tries = Convert.ToInt32(txb16.Text);
            methods.addTries(newResult);

            newResult.courseId = 17; 
            newResult.tries = Convert.ToInt32(txb17.Text);
            methods.addTries(newResult);

            newResult.courseId = 18; 
            newResult.tries = Convert.ToInt32(txb18.Text);
            methods.addTries(newResult);

            clearFields();

        }

        protected void btnupdate_Click(object sender, EventArgs e)
        {
           

            newResult.tourId = g_tournament_id;
            newResult.memberId = g_acc_member;

            //newResult.pair = Convert.ToInt32(row.Cells[3].Text);
            //newResult.hcp = Convert.ToInt32(row.Cells[4].Text);
            //newResult.gamehcp = Convert.ToInt32(row.Cells[6].Text);
            //newResult.netto = Convert.ToInt32(row.Cells[7].Text);

            newResult.courseId = 1;
            newResult.tries = Convert.ToInt32(txb1.Text);
            methods.updateTries(newResult);

            newResult.courseId = 2;
            newResult.tries = Convert.ToInt32(txb2.Text);
            methods.updateTries(newResult);

            newResult.courseId = 3;
            newResult.tries = Convert.ToInt32(txb3.Text);
            methods.updateTries(newResult);

            newResult.courseId = 4;
            newResult.tries = Convert.ToInt32(txb4.Text);
            methods.updateTries(newResult);

            newResult.courseId = 5;
            newResult.tries = Convert.ToInt32(txb5.Text);
            methods.updateTries(newResult);

            newResult.courseId = 6;
            newResult.tries = Convert.ToInt32(txb6.Text);
            methods.updateTries(newResult);

            newResult.courseId = 7;            
            newResult.tries = Convert.ToInt32(txb7.Text);
            methods.updateTries(newResult);

            newResult.courseId = 8;
            newResult.tries = Convert.ToInt32(txb8.Text);
            methods.updateTries(newResult);

            newResult.courseId = 9;
            newResult.tries = Convert.ToInt32(txb9.Text);
            methods.updateTries(newResult);

            newResult.courseId = 10;
            newResult.tries = Convert.ToInt32(txb10.Text);
            methods.updateTries(newResult);

            newResult.courseId = 11;
            newResult.tries = Convert.ToInt32(txb11.Text);
            methods.updateTries(newResult);

            newResult.courseId = 12;
            newResult.tries = Convert.ToInt32(txb12.Text);
            methods.updateTries(newResult);

            newResult.courseId = 13;
            newResult.tries = Convert.ToInt32(txb13.Text);
            methods.updateTries(newResult);

            newResult.courseId = 14;
            newResult.tries = Convert.ToInt32(txb14.Text);
            methods.updateTries(newResult);

            newResult.courseId = 15;
            newResult.tries = Convert.ToInt32(txb15.Text);
            methods.updateTries(newResult);

            newResult.courseId = 16;
            newResult.tries = Convert.ToInt32(txb16.Text);
            methods.updateTries(newResult);

            newResult.courseId = 17;
            newResult.tries = Convert.ToInt32(txb17.Text);
            methods.updateTries(newResult);
            
            newResult.courseId = 18;
            newResult.tries = Convert.ToInt32(txb18.Text);
            methods.updateTries(newResult);

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