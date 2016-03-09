<%@ Page Title="" Language="C#" MasterPageFile="~/headsite.Master" AutoEventWireup="true" CodeBehind="bokning.aspx.cs" Inherits="DSU_g5.bokning" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/CSS_bokning.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <asp:Calendar ID="calBokning" runat="server" OnSelectionChanged="calBokning_SelectionChanged" OnDayRender="calBokning_DayRender"></asp:Calendar>
    <br />
    <div id="bokningar">
        <asp:GridView ID="grvBokning" runat="server" OnDataBound="grvBokning_DataBound"></asp:GridView>
        <div id="bokningarInfo">
            <p id="pBokningarInfo" runat="server"></p>
            <asp:ListBox ID="lbBookedMembers" runat="server" OnSelectedIndexChanged="lbBookedMembers_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
            <br />
            <asp:Button ID="BtnDelMemberFromGame" runat="server" Text="Ta bort" OnClick="BtnDelMemberFromGame_Click" />
        </div>
        <div id="bokningarAdmin" runat="server">
            <script src="http://code.jquery.com/jquery-1.10.2.js"></script>
            <asp:ListBox ID="lbAllMembers" runat="server" Rows="25" OnSelectedIndexChanged="lbAllMembers_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
            <br />
            <asp:Label ID="lblSearchMember" runat="server" Text="Sök på medlem i fältet nedan:"></asp:Label>
            <br />
            <asp:TextBox ID="tbSearchMember" runat="server"></asp:TextBox>
            <br />
            <asp:Button ID="BtnBookMember" runat="server" Text="Boka medlem" OnClick="BtnBookMember_Click" />
            
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
    </div>
    <br />
    <div id="admin" runat="server">
        <asp:Button ID="BookedByMember" runat="server" Text="Boka in medlem" OnClick="btnBookedByMember_Click" />
        <asp:Button ID="UnBookedByMember" runat="server" Text="Avboka min bokning" OnClick="btnUnBookedByMember_Click" />
        <br />
        <asp:TextBox ID="tbBookAnotherMember" runat="server"></asp:TextBox>

        <br />
        <asp:Label ID="lblLoggedInUserId" runat="server" Text="Inloggad ID"></asp:Label>
        <br />
        <br />
        <br />
        <br />
        <asp:Label ID="lblGameIdInfo" runat="server" Text="<u>Nedan visas alla dina bokningar</u>"></asp:Label>
        <br />

        <asp:ListBox ID="lbGamesMemberIsBookedOn" runat="server" OnSelectedIndexChanged="lbGamesMemberIsBookedOn_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
        <br />

        <asp:Label ID="lblInfoAboutGameId" runat="server" Text="Här visas information om den valda bokningen i listan ovan."></asp:Label>
        <br />
        <br />
        <br />
        <br />
        <asp:Label ID="lblInfoBookedBy" runat="server" Text="<u>Nedan visas alla bokningar som du är bokningsansvarig för</u>"></asp:Label>
        <br />
        <asp:ListBox ID="lbGamesMemberIsBookableBy" runat="server" OnSelectedIndexChanged="lbGamesMemberIsBookableBy_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
        <br />
        <asp:Label ID="lblBookedByInfoGame" runat="server" Text="Här visas information om det valda gameId:t ovan."></asp:Label>
        <br />
        <br />
        <asp:Button ID="btnUnBookMemberByBookedBy" runat="server" Text="Avboka tid som du är bokningsansvarig för" OnClick="btnUnBookMemberByBookedBy_Click" />
        <br />
        <br />
    </div>

    <asp:HiddenField ID="hfPlaceholderMemberId" runat="server" />
    <asp:HiddenField ID="hfChosenDate" runat="server" />
    <asp:HiddenField ID="hfTimeId" runat="server" />
    <asp:HiddenField ID="hfBookedMembersFromList" runat="server" />
    <asp:HiddenField ID="hfChosenGameByMem" runat="server" />
    <asp:HiddenField ID="hfBookedByChosenGameId" runat="server" />

</asp:Content>
