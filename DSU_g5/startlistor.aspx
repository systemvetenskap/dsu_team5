<%@ Page Title="" Language="C#" MasterPageFile="~/headsite.Master" AutoEventWireup="true" CodeBehind="startlistor.aspx.cs" Inherits="DSU_g5.startlistor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/CSS_startlistor.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section>
        <div id="startlistor">
            <asp:DropDownList ID="ddlTournamentList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTournamentList_SelectedIndexChanged"></asp:DropDownList>
            <br />
            <asp:ListBox ID="LsbParticipants" runat="server" AutoPostBack="true" OnSelectedIndexChanged="LsbParticipants_SelectedIndexChanged"></asp:ListBox>
            <br />
            <asp:Button ID="btnStartlist" runat="server" Text="Skapa startgrupper" OnClick="btnStartlist_Click" />
            <br />
            <asp:GridView ID="gvRandom" runat="server" OnSelectedIndexChanged="gvRandom_SelectedIndexChanged" ></asp:GridView>
            <br />
            

        </div>

    </section>
</asp:Content>
