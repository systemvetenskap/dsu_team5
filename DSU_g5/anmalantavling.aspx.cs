﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DSU_g5
{
    public partial class anmalantavling : System.Web.UI.Page
    {
        member selectedMember;
        tournament selectedTournament;
        string tourQuery;

        public users inloggadUser = new users();
        public int accessId;

        protected void Page_Load(object sender, EventArgs e)
        {
            tourQuery = Request.QueryString["ContentId"];

            inloggadUser.idUser = Convert.ToInt32(Session["idUser"]);
            inloggadUser.fkIdMember = Convert.ToInt32(Session["IdMember"]);
            accessId = Convert.ToInt32(Session["IdAccess"]);


            if(!IsPostBack)
            {
                if (accessId != 2 && accessId != 3)
                {
                    tourMemberAdmin.Visible = false;
                    btnRegMemberOnTour.Text = "Boka in mig på tävling";
                    hfMemberId.Value = Convert.ToString(inloggadUser.fkIdMember);
                }
                else
                {
                    regButton.Attributes.Add("style", "margin-left: 40%");
                }


                List<tournament> tourList = new List<tournament>();
                tourList = methods.getTourList();


                ddlAllTournaments.DataValueField = "id_tournament";
                ddlAllTournaments.DataTextField = "tour_name";
                ddlAllTournaments.DataSource = tourList;
                ddlAllTournaments.DataBind();

                ddlAllTournaments.Items.Insert(0, "Välj Tävling");

                ddlAllTournaments.SelectedValue = tourQuery;


                tournament tour = new tournament();
                tour.id_tournament = Convert.ToInt32(tourQuery);
                hfTourId.Value = tour.id_tournament.ToString();
                selectedTournament = methods.GetTournament(tour.id_tournament);
               
                //Tar med Queryvärdet och fyller textboxar.
                infoAboutTourTBs(Convert.ToInt32(tourQuery));
            }

            //array till kontaktpersonssökningstextboxkontroll
            List<member> memberList = new List<member>();
            memberList = methods.getMemberList();

            DataTable members = methods.showAllMembersForBooking();
            foreach (DataRow dr in members.Rows)
            {
                ClientScript.RegisterArrayDeclaration("members",
                "{label: '" + dr["namn"] + "', value: '" + dr["mID"] + "'}");
            }
        }

        #region SELECTED INDEX CHANGED
        protected void ddlAllTournaments_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            lblConfirmation.Text = "";
            DropDownList ddl = (DropDownList)sender;
            ListItem li = ddl.SelectedItem;

            if (ddl.SelectedItem.Value != "Välj Tävling")
            {

                tournament tour = new tournament();
                tour.id_tournament = Convert.ToInt32(ddl.SelectedItem.Value);
                //int tournamentID = Convert.ToInt32(ddl.SelectedItem.Value);

                hfTourId.Value = tour.id_tournament.ToString();

                selectedTournament = methods.GetTournament(tour.id_tournament);

                //lblTournamentInfo.Text = selectedTournament.id_tournament + " " + selectedTournament.tour_name;

                //infoAboutTourTBs(tour.id_tournament);
                tbTourName.Text = selectedTournament.tour_name;
                tbTourInfo.Text = selectedTournament.tour_info;
                tbGameform.Text = methods.getGameformName(selectedTournament.gameform);
                tbTourDate.Text = selectedTournament.tour_date.ToShortDateString();
                tbRegStart.Text = selectedTournament.registration_start.ToShortDateString();
                tbRegEnd.Text = selectedTournament.registration_end.ToShortDateString();
                tbTourStart.Text = selectedTournament.tour_start_time.ToShortTimeString();
                tbTourEnd.Text = selectedTournament.tour_end_time.ToShortTimeString();
                tbContactPerson.Text = Convert.ToString(methods.ContactPersonName(tour.id_tournament));
                tbHole.Text = selectedTournament.hole.ToString();
            
            // Gridview
            // gvTourInfo.DataSource = methods.GetInfoAboutTour(tour.id_tournament);
            // gvTourInfo.DataBind();
            
            }
            else
            {
                ClearTextBoxes();
            }


        }

        protected void lbMembersTournament_SelectedIndexChanged(object sender, EventArgs e)
        {
            //lblConfirmation.Text = "";
            //ListBox lb = (ListBox)sender;
            //ListItem li = lb.SelectedItem;

            //member mem = new member();
            //mem.memberId = Convert.ToInt32(li.Value);
            ////int memberId = Convert.ToInt32(li.Value);

            //hfMemberId.Value = mem.memberId.ToString();

            //selectedMember = methods.getMember(mem.memberId);

            //lblMemberInfo.Text = selectedMember.memberId + " " + selectedMember.firstName + " " + selectedMember.lastName + " " + selectedMember.gender + " " + selectedMember.hcp;
        }

        #endregion




        #region KNAPPAR
        protected void btnRegMemberOnTour_Click(object sender, EventArgs e)
        {
            string message = null;

            if(hfTourId.Value != "" && hfTourId.Value != "0")
            {
                if (hfMemberId.Value != "")
                {
                    int memId = Convert.ToInt32(hfMemberId.Value);
                    if (methods.IsPayed(memId) == true)
                    {
                        int tourId = Convert.ToInt32(hfTourId.Value);
                        // KÖR PÅ.
                        int result = 0;

                        methods.RegMemberOnTour(tourId, memId, result, out message);

                        if (message != null)
                        {
                            if (accessId != 2 && accessId != 3)
                            {
                                Response.Write("<script>alert('" + "Du är redan inbokad på denna tävling." + "')</script>");
                            }
                            else
                            {
                                Response.Write("<script>alert('" + message + "')</script>");
                            }

                            lblConfirmation.Text = "";
                        }
                        else
                        {
                            lblConfirmation.Text = "Registrering av medlem genomförd.";
                        }
                    }
                    else
                    {
                        if (accessId != 2 && accessId != 3)
                        {
                            Response.Write("<script>alert('" + "Du måste betala medlemsavgiften för att kunna anmäla dig på en tävling." + "')</script>");
                        }
                        else
                        {
                            Response.Write("<script>alert('" + "Medlemsavgiften är inte betald för den valda medlemen." + "')</script>");
                        }
                    }
                }

                else
                {
                    Response.Write("<script>alert('" + "Välj en medlem i listan" + "')</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('" + "Välj en tävling i listan." + "')</script>");
            }
        }

        #endregion


        private void infoAboutTourTBs(int tourId)
        {
            tournament t = new tournament();
            t = methods.GetTournament(Convert.ToInt32(tourQuery));
            
            
            tbTourName.Text = t.tour_name;
            tbTourInfo.Text = t.tour_info;
            tbGameform.Text = methods.getGameformName(t.gameform);
            tbTourDate.Text = t.tour_date.ToShortDateString();
            tbRegStart.Text = t.registration_start.ToShortDateString();
            tbRegEnd.Text = t.registration_end.ToShortDateString();
            tbTourStart.Text = t.tour_start_time.ToShortTimeString();
            tbTourEnd.Text = t.tour_end_time.ToShortTimeString();
            tbContactPerson.Text = Convert.ToString(methods.ContactPersonName(tourId));
            tbHole.Text = t.hole.ToString();


        }

        private void ClearTextBoxes()
        {
            hfTourId.Value = "";

            tbTourName.Text = string.Empty;
            tbTourInfo.Text = string.Empty;
            tbGameform.Text = string.Empty;
            tbTourDate.Text = string.Empty;
            tbRegStart.Text = string.Empty;
            tbRegEnd.Text = string.Empty;
            tbTourStart.Text = string.Empty;
            tbTourEnd.Text = string.Empty;
            tbContactPerson.Text = string.Empty;
            tbHole.Text = string.Empty;
        }

        protected void gvTourInfo_DataBound(object sender, EventArgs e)
        {

        }

        protected void hfMemberId_ValueChanged(object sender, EventArgs e)
        {
            lblConfirmation.Text = "";
            HiddenField hf = (HiddenField)sender;
            int memberId = Convert.ToInt32(hf.Value);

            selectedMember = methods.getMember(memberId);

            lblMemberInfo.Text = selectedMember.memberId + " " + selectedMember.firstName + " " + selectedMember.lastName + " " + selectedMember.gender + " " + selectedMember.hcp + " hcp";
        }
    }
}