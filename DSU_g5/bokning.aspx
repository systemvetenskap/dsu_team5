<%@ Page Title="" Language="C#" MasterPageFile="~/headsite.Master" AutoEventWireup="true" CodeBehind="bokning.aspx.cs" Inherits="DSU_g5.bokning" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/CSS_bokning.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="bokningar" runat="server">
        <div id="calenderDiv">
            <asp:Calendar ID="calBokning" runat="server" OnSelectionChanged="calBokning_SelectionChanged" OnDayRender="calBokning_DayRender"></asp:Calendar>
        </div>

        <div id ="bookMember" runat="server">
            <%-- DIV 1 LBLOGGEDINUSER ID --%>
            <asp:Label ID="lblLoggedInUserId" runat="server" Text="Inloggad ID"></asp:Label>
            <br />
            <br />
            <asp:Label ID="lblAnotherMember" runat="server" Text="<u>Fyll i MedlemsID nedan</u>"></asp:Label>
            <br />
            <%-- DIV 1 TBOOKANOTHERMEMBER --%>
            <asp:TextBox ID="tbBookAnotherMember" runat="server"></asp:TextBox>
            <br />
            <%-- DIV 1 BOOKEDBYMEMBER --%>
            <asp:Button ID="btnBookedByMember" runat="server" Text="Boka in medlem" OnClick="btnBookedByMember_Click" /> 
            <br />
        </div>

        <div id ="gridViewTider" runat="server">
            <asp:GridView ID="grvBokning" runat="server" OnDataBound="grvBokning_DataBound"></asp:GridView>
        </div>

        <div id="bokningarInfo" runat="server">
            <p id="pBokningarInfo" runat="server"></p>
            <asp:ListBox ID="lbBookedMembers" runat="server" OnSelectedIndexChanged="lbBookedMembers_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
            <br />
            <asp:Button ID="BtnDelMemberFromGame" runat="server" Text="Ta bort" OnClick="BtnDelMemberFromGame_Click" />
        </div>
    </div>
        <div id="bokningarAdmin" runat="server">
            <script src="http://code.jquery.com/jquery-1.10.2.js"></script>
            <div id="listBoxMedlemar">
                <asp:ListBox ID="lbAllMembers" runat="server" Rows="10" OnSelectedIndexChanged="lbAllMembers_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
            </div>
            <br />
            <div id="searchMember">
                <asp:Label ID="lblSearchMember" runat="server" Text="Sök på medlem i fältet nedan:"></asp:Label>
                <br />
                <asp:TextBox ID="tbSearchMember" runat="server"></asp:TextBox>
                <br />
                <asp:Button ID="BtnBookMember" runat="server" Text="Boka medlem" OnClick="BtnBookMember_Click" />
            </div>

            <script>
                document.getElementById("ContentPlaceHolder1_tbSearchMember").addEventListener("input", ListBoxFilter);
                function ListBoxFilter() {
                    var input = $("#ContentPlaceHolder1_tbSearchMember").val();
                    var regex = new RegExp(input, "i");
                    var antalPoster = $("#ContentPlaceHolder1_lbAllMembers").children().length;
                    for (i = 0; i < antalPoster; i++) {
                        var namn = $("#ContentPlaceHolder1_lbAllMembers").children()[i].innerHTML;
                        if (!namn.match(regex)) {
                            $("#ContentPlaceHolder1_lbAllMembers option:eq(" + i + ")").hide();
                        }
                        else {
                            $("#ContentPlaceHolder1_lbAllMembers option:eq(" + i + ")").show();
                        }
                    }
                }                
            </script>

        </div>
    <br />
    <div id="member" runat="server">
        <br />
        <div id="membersGames" runat="server">
        <%-- DIV 2 LBGAMEIDINFO --%>
            <asp:Label ID="lblGameIdInfo" runat="server" Text="<u>Mina bokningar</u>"></asp:Label>
            <br />
            <%-- DIV 2 LBGAMESMEMBERISBOOKEDON --%>
            <asp:ListBox ID="lbGamesMemberIsBookedOn" runat="server" OnSelectedIndexChanged="lbGamesMemberIsBookedOn_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
            <br />
            <%-- DIV 2 LBINFOABOUTGAMID --%>
            <asp:Label ID="lblInfoAboutGameId" runat="server" Text="Här visas information om den valda bokningen i listan ovan."></asp:Label>
            <br />
             <%-- DIV 2 UNBOOKEDBYMEMBER --%>
            <asp:Button ID="UnBookedByMember" runat="server" Text="Avboka min bokning" OnClick="btnUnBookedByMember_Click" />
            <br />
        </div>
        <br />
        <br />
        <div id="memberBookedBy" runat="server">
            <%-- DIV 3 LBINFOBOOKEDBY --%>
            <asp:Label ID="lblInfoBookedBy" runat="server" Text="<u>Alla medlemsbokningar som du är bokningsansvarig för</u>"></asp:Label>
            <br />
            <%-- DIV 3 LBGAMESMEMBERISBOOKABLEBY --%>
            <asp:ListBox ID="lbGamesMemberIsBookableBy" runat="server" OnSelectedIndexChanged="lbGamesMemberIsBookableBy_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
            <br />
            <%-- DIV 3 LBLBOOKEDBYINFOGAME --%>
            <asp:Label ID="lblBookedByInfoGame" runat="server" Text="Här visas information om den valda bokningen ovan."></asp:Label>
            <br />
            <%-- DIV 3 BTNUNBOOKMEMBERBYBOOKEDBY --%>
            <asp:Button ID="btnUnBookMemberByBookedBy" runat="server" Text="Avboka tid" OnClick="btnUnBookMemberByBookedBy_Click" />
        </div>
    </div>

    <asp:HiddenField ID="hfPlaceholderMemberId" runat="server" />
    <asp:HiddenField ID="hfChosenDate" runat="server" />
    <asp:HiddenField ID="hfTimeId" runat="server" />
    <asp:HiddenField ID="hfBookedMembersFromList" runat="server" />
    <asp:HiddenField ID="hfChosenGameByMem" runat="server" />
    <asp:HiddenField ID="hfBookedByChosenGameId" runat="server" />

</asp:Content>
