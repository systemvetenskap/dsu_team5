<%@ Page Title="" Language="C#" MasterPageFile="~/headsite.Master" AutoEventWireup="true" CodeBehind="resultat.aspx.cs" Inherits="DSU_g5.resultat" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/CSS_resultat.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section id="resultatsida">
        <div id="tournament" runat="server">
            <asp:Label ID="lbTournamentList" CssClass="resultslabel" runat="server" Text="Tävlingar"></asp:Label>
            <asp:ListBox ID="lblTournamentList" CssClass="resultstextbox" runat="server" Height="100px" Width="215px" AutoPostBack="true" ></asp:ListBox>
            <asp:Label ID="lbSearchTournament" CssClass="resultslabel" runat="server" Text="Sök tävling"></asp:Label>
            <asp:TextBox ID="tbSearchTournament" CssClass="resultstextbox" runat="server" Width="209px"></asp:TextBox>  
            </div>
        <div id="participant" runat="server">
            <asp:Label ID="lbParticipantList" CssClass="resultslabel" runat="server" Text="Tävlingsdeltagare"></asp:Label>
            <asp:ListBox ID="lblParticipantList" CssClass="resultstextbox" runat="server" Height="100px" Width="215px" AutoPostBack="true" ></asp:ListBox>
            <asp:Label ID="lbSearchParticipant" CssClass="resultslabel" runat="server" Text="Sök deltagare"></asp:Label>
            <asp:TextBox ID="tbSearchParticipant" CssClass="resultstextbox" runat="server" Width="209px"></asp:TextBox>          
        </div> 
        <div id="participantresults" runat="server">
            <asp:GridView ID="gvParticipantResults" runat="server" AutoPostBack="true" OnDataBound="gvParticipantResults_DataBound" ></asp:GridView>
        </div> 
    </section>
</asp:Content>
