<%@ Page Title="" Language="C#" MasterPageFile="~/headsite.Master" AutoEventWireup="true" CodeBehind="skapatavling.aspx.cs" Inherits="DSU_g5.skapatavling" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/CSS_skapatavling.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section>
        <h2>Skapa tävling</h2>
        <div id="tournamentInfo">
            <asp:Label ID="lblName" runat="server" Text="Tävlingens namn"></asp:Label>
            <br />
            <asp:TextBox ID="tbName" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="lblGameForm" runat="server" Text="Tävlingsform"></asp:Label>
            <br />
            <asp:DropDownList ID="ddlGameForm" runat="server"></asp:DropDownList>
            <br />
            <asp:Label ID="lblMemberCategory" runat="server" Text="Medlemskategori"></asp:Label>
            <br />
            <asp:DropDownList ID="ddlMemberCategory" runat="server"></asp:DropDownList>
            <br />
            <asp:Label ID="lblInformation" runat="server" Text="Information"></asp:Label>
            <br />
            <textarea id="taInformation" runat="server" cols="36" rows="16"></textarea>
            <br />
            <asp:Label ID="lblContactPerson" runat="server" Text="Kontaktperson"></asp:Label>
            <br />
            <asp:ListBox ID="lbContactPerson" runat="server"></asp:ListBox>
            <br />
            <asp:Label ID="lblSokContactPerson" runat="server" Text="Sök person"></asp:Label>
            <br />
            <asp:TextBox ID="tbSokContactPerson" runat="server"></asp:TextBox>
        </div>
        <div id="tournamentTimes">
            <asp:Label ID="lblDate" runat="server" Text="Tävlingens datum"></asp:Label>
            <asp:Calendar ID="calDate" runat="server"></asp:Calendar>
            <br />
            <asp:Label ID="lblStartTime" runat="server" Text="Tid (00:00)"></asp:Label>
            <br />
            <asp:TextBox ID="tbStartTime" runat="server" Text=":"></asp:TextBox>
            <asp:Label ID="lblEndTime" runat="server" Text="  till  "></asp:Label>
            <asp:TextBox ID="tbEndTime" runat="server" Text=":"></asp:TextBox>
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
            <div id="sponsors_1">
                <asp:Label ID="lblSponsors" runat="server" Text="Sponsorer"></asp:Label>
                <br />
                <asp:ListBox ID="lbSponsors" runat="server"></asp:ListBox>
                <br />
                <asp:Button ID="btnSponsorsRemove" runat="server" Text="Ta bort" OnClick="btnSponsorsRemove_Click" />
            </div>
            <div id="sponsors_2">
                <asp:Label ID="lblFormerSponsors" runat="server" Text="Tidigare sponsorer"></asp:Label>
                <br />
                <asp:ListBox ID="lbFormerSponsors" runat="server"></asp:ListBox>
                <br />
                <asp:Button ID="btnFormerSponsorsAdd" runat="server" Text="Lägg till" OnClick="btnFormerSponsorsAdd_Click" />
            </div>
            <div id="sponsors_3">
                <asp:Label ID="lblNewSponsor" runat="server" Text="Registrera ny sponsor"></asp:Label>
                <br />
                <asp:Label ID="lblNewSponsorName" runat="server" Text="Namn"></asp:Label>
                <br />
                <asp:TextBox ID="tbNewSponsorName" runat="server"></asp:TextBox>
                <br />
                <asp:Label ID="lblNewSponsorPhone" runat="server" Text="Telefon"></asp:Label>
                <br />
                <asp:TextBox ID="tbNewSponsorPhone" runat="server"></asp:TextBox>
                <br />
                <asp:Button ID="btnNewSponsorAdd" runat="server" Text="Registrera" OnClick="btnNewSponsorAdd_Click" />
            </div>
        </div>
        <asp:Button ID="btnSave" runat="server" Text="Spara" OnClick="btnSave_Click" />
        <asp:Button ID="btnClear" runat="server" Text="Rensa" OnClick="btnClear_Click" />
    </section>
</asp:Content>
