<%@ Page Title="" Language="C#" MasterPageFile="~/headsite.Master" AutoEventWireup="true" CodeBehind="skapatavling.aspx.cs" Inherits="DSU_g5.skapatavling" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/CSS_skapatavling.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section>
        <h2>Skapa tävling</h2>
        <div id="tournamentInfo">
            <asp:Label ID="lblName" runat="server" Text="Tävlingens namn"></asp:Label>
            <asp:TextBox ID="tbName" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="lblGameForm" runat="server" Text="Tävlingsform"></asp:Label>
            <asp:DropDownList ID="ddlGameForm" runat="server"></asp:DropDownList>
            <br />
            <asp:Label ID="lblMemberCategory" runat="server" Text="Medlemskategori"></asp:Label>
            <asp:DropDownList ID="ddlMemberCategory" runat="server"></asp:DropDownList>
            <br />
            <asp:Label ID="lblInformation" runat="server" Text="Information"></asp:Label>
            <textarea id="taInformation" runat="server" cols="36" rows="16"></textarea>
        </div>
        <div id="contactPerson">
            <asp:Label ID="lblContactPerson" runat="server" Text="Kontaktperson"></asp:Label>
            <asp:ListBox ID="lbContactPerson" runat="server"></asp:ListBox>
            <br />
            <asp:TextBox ID="tbSokContactPerson" runat="server"></asp:TextBox>
        </div>
        <div id="tournamentTimes">
            <asp:Label ID="lblDate" runat="server" Text="Tävlingens datum"></asp:Label>
            <asp:Calendar ID="calDate" runat="server"></asp:Calendar>
            <br />
            <asp:Label ID="lblStartTime" runat="server" Text="Starttid"></asp:Label>
            <asp:TextBox ID="tbStartTime" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="lblEndTime" runat="server" Text="Sluttid"></asp:Label>
            <asp:TextBox ID="tbEndTime" runat="server"></asp:TextBox>
            <br />
        </div>
        <div id="registrationTimes">
            <asp:Label ID="lblRegStart" runat="server" Text="Första registreringsdatum"></asp:Label>
            <asp:Calendar ID="calRegStart" runat="server"></asp:Calendar>
            <br />
            <asp:Label ID="lblRegEnd" runat="server" Text="Sista registreringsdatum"></asp:Label>
            <asp:Calendar ID="calRegEnd" runat="server"></asp:Calendar>
            <br />
            <asp:Label ID="lblPublishList" runat="server" Text="Startlistor publiceras"></asp:Label>
            <asp:Calendar ID="calPublishList" runat="server"></asp:Calendar>
            <br />
        </div>
        <div id="sponsors">

        </div>
        <asp:Button ID="btnSave" runat="server" Text="Spara" OnClick="btnSave_Click" />
        <asp:Button ID="btnClear" runat="server" Text="Rensa" />
    </section>
</asp:Content>
