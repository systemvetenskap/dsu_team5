<%@ Page Title="" Language="C#" MasterPageFile="~/headsite.Master" AutoEventWireup="true" CodeBehind="medlemssida.aspx.cs" Inherits="DSU_g5.medlemssida" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/CSS_medlemssida.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <section id="medlemssida">

        <asp:Label ID="lbIdMember" CssClass="memberlabel" runat="server" Text="Medlems ID"></asp:Label>
        <asp:TextBox ID="tbIdMember" CssClass="membertextbox" runat="server"></asp:TextBox>
        <br />

        <asp:Label ID="lbFirstName" CssClass="memberlabel" runat="server" Text="Förnamn"></asp:Label>
        <asp:TextBox ID="tbFirstName" CssClass="membertextbox" runat="server"></asp:TextBox>
        <br />

        <asp:Label ID="lbLastName" CssClass="memberlabel" runat="server" Text="Efternamn"></asp:Label>
        <asp:TextBox ID="tbLastName" CssClass="membertextbox" runat="server"></asp:TextBox>
        <br />
        
        <asp:Label ID="lbAddress" CssClass="memberlabel" runat="server" Text="Address"></asp:Label>
        <asp:TextBox ID="tbAddress" CssClass="membertextbox" runat="server"></asp:TextBox>
        <br />

        <asp:Label ID="lbPostalCode" CssClass="memberlabel" runat="server" Text="Postkod"></asp:Label>
        <asp:TextBox ID="tbPostalCode" CssClass="membertextbox" runat="server"></asp:TextBox>
        <br />

        <asp:Label ID="lbCity" CssClass="memberlabel" runat="server" Text="Stad"></asp:Label>
        <asp:TextBox ID="tbCity" CssClass="membertextbox" runat="server"></asp:TextBox>
        <br />

        <asp:Label ID="lbMail" CssClass="memberlabel" runat="server" Text="E-post"></asp:Label>
        <asp:TextBox ID="tbMail" CssClass="membertextbox" runat="server"></asp:TextBox>
        <br />

        <asp:Label ID="lbGender" CssClass="memberlabel" runat="server" Text="Kön"></asp:Label>
        <asp:DropDownList ID="ddlGender" CssClass="membertextbox" runat="server"></asp:DropDownList>
        <br />

        <asp:Label ID="lbHcp" CssClass="memberlabel" runat="server" Text="HCP"></asp:Label>
        <asp:TextBox ID="tbHcp" CssClass="membertextbox" runat="server"></asp:TextBox>
        <br />

        <asp:Label ID="lbGolfId" CssClass="memberlabel" runat="server" Text="Golf Id"></asp:Label>
        <asp:TextBox ID="tbGolfId" CssClass="membertextbox" runat="server"></asp:TextBox>
        <br />

        <asp:Label ID="lbCategory" CssClass="memberlabel" runat="server" Text="Medlems kategori" ></asp:Label>
        <asp:DropDownList ID="ddlCategory" CssClass="membertextbox" runat="server"></asp:DropDownList>
        <br />
        <br />

        <asp:Label ID="lbIdUser" CssClass="memberlabel" runat="server" Text="AnvändarId"></asp:Label>
        <asp:TextBox ID="tbIdUser" CssClass="membertextbox" runat="server"></asp:TextBox>
        <br />

        <asp:Label ID="lbUserName" CssClass="memberlabel" runat="server" Text="Användarnamn"></asp:Label>
        <asp:TextBox ID="tbUserName" CssClass="membertextbox" runat="server"></asp:TextBox>
        <br />

        <asp:Label ID="lbUserPassword" CssClass="memberlabel" runat="server" Text="Lösenord"></asp:Label>
        <asp:TextBox ID="tbUserPassword" CssClass="membertextbox" runat="server"></asp:TextBox>
        <br />

        <asp:Label ID="lbFkIdMember" CssClass="memberlabel" runat="server" Text="MedlemsId"></asp:Label>
        <asp:TextBox ID="tbFkIdMember" CssClass="membertextbox" runat="server"></asp:TextBox>
        <br />
        <br />

        <asp:Button ID="btAdd" CssClass="memberbutton" runat="server" Text="Lägg till" OnClick="btAdd_Click" />
        <asp:Button ID="btUpdate" CssClass="memberbutton" runat="server" Text="Uppdatera" />
        <asp:Button ID="btRemove" CssClass="memberbutton" runat="server" Text="Ta bort" OnClick="btRemove_Click" />

     </section>

</asp:Content>
