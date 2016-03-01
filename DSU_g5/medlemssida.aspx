<%@ Page Title="" Language="C#" MasterPageFile="~/headsite.Master" AutoEventWireup="true" CodeBehind="medlemssida.aspx.cs" Inherits="DSU_g5.medlemssida" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/CSS_medlemssida.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section id="medlemssida">
        <asp:Label ID="lbIdMember" runat="server" Text="Medlems ID"></asp:Label>
        <asp:TextBox ID="tbIdMember" runat="server" ></asp:TextBox>

        <asp:Label ID="lbFirstName" runat="server" Text="Förnamn"></asp:Label>
        <asp:TextBox ID="tbFirstName" runat="server" ></asp:TextBox>

        <asp:Label ID="lbLastName" runat="server" Text="Efternamn"></asp:Label>
        <asp:TextBox ID="tbLastName" runat="server"></asp:TextBox>
        <asp:Label ID="lbAddress" runat="server" Text="Address"></asp:Label>
        <asp:TextBox ID="tbAddress" runat="server"></asp:TextBox>

        <asp:Label ID="lbPostalCode" runat="server" Text="Postkod"></asp:Label>
        <asp:TextBox ID="tbPostalCode" runat="server"></asp:TextBox>

        <asp:Label ID="lbCity" runat="server" Text="Stad"></asp:Label>
        <asp:TextBox ID="tbCity" runat="server"></asp:TextBox>

        <asp:Label ID="lbMail" runat="server" Text="E-post"></asp:Label>
        <asp:TextBox ID="tbMail" runat="server"></asp:TextBox>

        <asp:Label ID="lbGender" runat="server" Text="Kön"></asp:Label>
        <asp:TextBox ID="tbGender" runat="server"></asp:TextBox>

        <asp:Label ID="lbHcp" runat="server" Text="HCP"></asp:Label>
        <asp:TextBox ID="tbHcp" runat="server"></asp:TextBox>

        <asp:Label ID="lbGolfId" runat="server" Text="Golf Id"></asp:Label>
        <asp:TextBox ID="tbGolfId" runat="server"></asp:TextBox>
        
         <asp:Label ID="lbMemberCategory" runat="server" Text="Medlems kategori"></asp:Label>
        <asp:TextBox ID="txMemberCategory" runat="server"></asp:TextBox>

        <asp:Button ID="btAdd" runat="server" Text="Lägg till" />
        <asp:Button ID="btUpdate" runat="server" Text="Uppdatera" />
        <asp:Button ID="btRemove" runat="server" Text="Ta bort" />
     </section>

</asp:Content>
