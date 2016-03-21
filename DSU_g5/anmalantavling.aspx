<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/headsite.Master" CodeBehind="anmalantavling.aspx.cs" Inherits="DSU_g5.anmalantavling" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/CSS_anmalantavling.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section>    
    <div id ="tournamentInfo" runat="server">
        <br />
        <asp:Label ID="lblAllTournaments" runat="server" Text="<u>Välj en tävling nedan</u>"></asp:Label>
        <br />
        <asp:DropDownList ID="ddlAllTournaments" runat="server" CssClass="dropdown" AutoPostBack="true" OnSelectedIndexChanged="ddlAllTournaments_SelectedIndexChanged"></asp:DropDownList>
        <br />
        <%--<asp:Label ID="lblTournamentInfo" runat="server" Text="Här visas information om vald tävling."></asp:Label>--%>
        <br />
        <asp:Label ID="lblTourName" runat="server" Text="Tävlingsnamn"></asp:Label>
        <asp:TextBox ID="tbTourName" runat="server" ReadOnly="True"></asp:TextBox>
        <br />
        <asp:Label ID="lblTourInfo" runat="server" Text="Tävlingsinfo"></asp:Label>
        <asp:TextBox ID="tbTourInfo" runat="server" ReadOnly="True"></asp:TextBox>
        <br />
        <asp:Label ID="lblTourDate" runat="server" Text="Tävlingsdatum"></asp:Label>
        <asp:TextBox ID="tbTourDate" runat="server" ReadOnly="True"></asp:TextBox>
        <br />
        <asp:Label ID="lblRegStart" runat="server" Text="Registreringsstart"></asp:Label>
        <asp:TextBox ID="tbRegStart" runat="server" ReadOnly="True"></asp:TextBox>
        <br />
        <asp:Label ID="lblRegEnd" runat="server" Text="Registreringsslut"></asp:Label>
        <asp:TextBox ID="tbRegEnd" runat="server" ReadOnly="True"></asp:TextBox>
        <br />
        <asp:Label ID="lblTourStart" runat="server" Text="Start"></asp:Label>
        <asp:TextBox ID="tbTourStart" runat="server" ReadOnly="True"></asp:TextBox>
        <br />
        <asp:Label ID="lblTourEnd" runat="server" Text="Slut"></asp:Label>
        <asp:TextBox ID="tbTourEnd" runat="server" ReadOnly="True"></asp:TextBox>
        <br />
        <asp:Label ID="lblContactPerson" runat="server" Text="Kontaktperson"></asp:Label>
        <asp:TextBox ID="tbContactPerson" runat="server" ReadOnly="True"></asp:TextBox>
        <br />
        <asp:Label ID="lblHole" runat="server" Text="Antal hål"></asp:Label>
        <asp:TextBox ID="tbHole" runat="server" ReadOnly="True"></asp:TextBox>
        <br />
        <!-- <asp:GridView ID="gvTourInfo" runat="server" OnDataBound="gvTourInfo_DataBound"></asp:GridView> -->
    </div>

    <div id="tourMemberAdmin" runat="server">
        <script src="http://code.jquery.com/jquery-1.10.2.js"></script>
        <asp:Label ID="lblAllMembers" runat="server" Text="<u>Välj en medlem att registrera på tävling</u>"></asp:Label>
        <br />
        <asp:ListBox ID="lbMembersTournament" runat="server" Rows="10" OnSelectedIndexChanged="lbMembersTournament_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
        <br />
        <asp:Label ID="lblSearchMember" runat="server" Text="Sök medlem"></asp:Label>
        <br />
        <asp:TextBox ID="tbSearchMember" runat="server"></asp:TextBox>
        <br />
        <asp:Label ID="lblMemberInfo" runat="server" Text="Här visas nformation om vald medlem."></asp:Label>

        <script>
                document.getElementById("ContentPlaceHolder1_tbSearchMember").addEventListener("input", ListBoxFilter);
                function ListBoxFilter() {
                    var input = $("#ContentPlaceHolder1_tbSearchMember").val();
                    var regex = new RegExp(input, "i");
                    var antalPoster = $("#ContentPlaceHolder1_lbMembersTournament").children().length;
                    for (i = 0; i < antalPoster; i++) {
                        var namn = $("#ContentPlaceHolder1_lbMembersTournament").children()[i].innerHTML;
                        if (!namn.match(regex)) {
                            $("#ContentPlaceHolder1_lbMembersTournament option:eq(" + i + ")").hide();
                        }
                        else {
                            $("#ContentPlaceHolder1_lbMembersTournament option:eq(" + i + ")").show();
                        }
                    }
                }                
            </script>

    </div>
    <div id="regButton" runat="server">
        <asp:Button ID="btnRegMemberOnTour" runat="server" Text="Registrera medlem på tävling" OnClick="btnRegMemberOnTour_Click"/>
        <br />
        <asp:Label ID="lblConfirmation" runat="server" Text="" Font-Bold="true" ForeColor="Green"></asp:Label>
    </div>
        <br />
    <asp:HiddenField ID="hfTourId" runat="server" />
    <asp:HiddenField ID="hfMemberId" runat="server" />
    </section>
</asp:Content>


    