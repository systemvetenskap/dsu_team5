<%@ Page Title="" Language="C#" MasterPageFile="~/headsite.Master" AutoEventWireup="true" CodeBehind="bokning.aspx.cs" Inherits="DSU_g5.bokning" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/CSS_bokning.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <asp:Calendar ID="calBokning" runat="server" OnSelectionChanged="calBokning_SelectionChanged"></asp:Calendar>
    <br />
    <asp:Button ID="BookedByMember" runat="server" Text="Boka in medlem" OnClick="btnBookedByMember_Click" />
    <asp:Button ID="UnBookedByMember" runat="server" Text="Avboka tid för medlem" OnClick="btnUnBookedByMember_Click" />
    <br />
    <asp:TextBox ID="tbBookAnotherMember" runat="server"></asp:TextBox>

    <br />
    <asp:Label ID="lblLoggedInUserId" runat="server" Text="Inloggad ID"></asp:Label>
    <br />
    <asp:ListBox ID="lbGamesMemberIsBookedOn" runat="server" OnSelectedIndexChanged="lbGamesMemberIsBookedOn_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
    <br />

    <asp:Label ID="lblInfoAboutGameId" runat="server" Text="Här visas information om den valda bokningen i listan ovan."></asp:Label>
    <br />
    <div id="bokningar">
        <asp:GridView ID="grvBokning" runat="server" OnDataBound="grvBokning_DataBound"></asp:GridView>
        <div id="bokningarInfo">
            <p id="pBokningarInfo" runat="server"></p>
            <asp:ListBox ID="lbBookedMembers" runat="server" OnSelectedIndexChanged="lbBookedMembers_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
            <br />
            <asp:Button ID="BtnDelMemberFromGame" runat="server" Text="Ta bort" OnClick="BtnDelMemberFromGame_Click" />
        </div>
        <div id="bokningarAdmin">
            <asp:ListBox ID="lbAllMembers" runat="server" Rows="25" OnSelectedIndexChanged="lbAllMembers_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
            <br />
            <asp:Button ID="BtnBookMember" runat="server" Text="Boka medlem" OnClick="BtnBookMember_Click" />
        </div>
    </div>
    <br />

    <asp:HiddenField ID="hfPlaceholderMemberId" runat="server" />
    <asp:HiddenField ID="hfChosenDate" runat="server" />
    <asp:HiddenField ID="hfTimeId" runat="server" />
    <asp:HiddenField ID="hfBookedMembersFromList" runat="server" />
    <asp:HiddenField ID="hfChosenGameByMem" runat="server" />

</asp:Content>
