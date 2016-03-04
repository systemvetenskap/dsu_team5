<%@ Page Title="" Language="C#" MasterPageFile="~/headsite.Master" AutoEventWireup="true" CodeBehind="bokning.aspx.cs" Inherits="DSU_g5.bokning" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/CSS_bokning.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    
    <asp:Calendar ID="calBokning" runat="server" OnSelectionChanged="calBokning_SelectionChanged">

    </asp:Calendar>




 <%--   <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button1" />--%>



    <br />
    <br />
    <br />

<%--    <asp:Label ID="lblTest" runat="server" Text="Label"></asp:Label>
    <asp:ListBox ID="ListBox1" runat="server"></asp:ListBox>--%>

    <br />
    <br />
    <div id="bokningar">
        <asp:GridView ID="grvBokning" runat="server" OnDataBound="grvBokning_DataBound"></asp:GridView>
        <div id="bokningarInfo">
            <asp:ListBox ID="lbBookedMembers" runat="server" OnSelectedIndexChanged="lbBookedMembers_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
            <p id="pBokningarInfo" runat="server"></p>
        </div>
    </div>
    <br />
    <br />

    <asp:ListBox ID="lbAllMembers" runat="server" Rows="25" SelectionMode="Multiple" OnSelectedIndexChanged="lbAllMembers_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
    <asp:Button ID="BtnBookMember" runat="server" Text="Boka medlem" OnClick="BtnBookMember_Click" />

    <asp:Button ID="BtnDelMemberFromGame" runat="server" Text="Ta bort" OnClick="BtnDelMemberFromGame_Click" />


        <asp:Calendar ID="startCalendar" runat="server">
    </asp:Calendar>
        <asp:Calendar ID="endCalendar" runat="server">
    </asp:Calendar>
    <asp:Button ID="btnAddSeason" runat="server" Text="Sesong" OnClick="btnAddSeason_Click"/>
    <asp:HiddenField ID="hfPlaceholderMemberId" runat="server" />
    <asp:HiddenField ID="hfChosenDate" runat="server" />
    <asp:HiddenField ID="hfTimeId" runat="server" />
    <asp:HiddenField ID="hfBookedMembersFromList" runat="server" />

</asp:Content>
