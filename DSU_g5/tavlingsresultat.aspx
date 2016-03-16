<%@ Page Title="" Language="C#" MasterPageFile="~/headsite.Master" AutoEventWireup="true" CodeBehind="tavlingsresultat.aspx.cs" Inherits="DSU_g5.tavlingsresultat" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/CSS_tavlingsresultat.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section>
        <h2>Tävlingsresultat</h2>
        <div id="dropdown">
            <asp:Label ID="lblAllTournaments" runat="server" Text="<u>Välj en tävling nedan</u>"></asp:Label>
            <br />
            <asp:DropDownList ID="ddlAllTournaments" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAllTournaments_SelectedIndexChanged"></asp:DropDownList>
            <br />
        </div>
        <div id="table">
            <asp:GridView ID="gvResults" runat="server" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField DataField="namn" HeaderText="Namn" />
                    <asp:BoundField DataField="resultat" HeaderText="Resultat" />
                </Columns>
            </asp:GridView>
        </div>
    </section>
</asp:Content>
