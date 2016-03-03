<%@ Page Title="" Language="C#" MasterPageFile="~/headsite.Master" AutoEventWireup="true" CodeBehind="admin.aspx.cs" Inherits="DSU_g5.admin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/CSS_admin.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <section id="adminsida">
        
        
        <asp:Label ID="lblNewNews" CSSclass="newslabel" runat="server" Text="Lägg till nyhet"></asp:Label>
        <br />
        <asp:TextBox ID="txtNewNews" CSSclass="newstextbox" runat="server">Nyhetsnamn</asp:TextBox>
        <br />
        <asp:Label ID="lblUpdateNews" CSSclass="newslabel" runat="server" Text="Uppdatera nyhet"></asp:Label>
        <br />
        <asp:DropDownList ID="ddlNewsName" CSSclass="newsddl" runat="server" OnSelectedIndexChanged="ddlNewsName_SelectedIndexChanged"></asp:DropDownList>
        <br />
 
        <textarea id="textNews" runat="server" cols="24" rows="20">jfksjsdlg</textarea>
        <br />
        <asp:Button ID="publishNews" CSSclass="newsbutton" runat="server" Text="Publicera nyheter" OnClick="btnPublish_Click" />
        <br />
        <asp:Button ID="updateNews" CSSclass="newsbutton" runat="server" Text="Uppdatera nyhet" OnClick="btnUpdateNews_Click" />
        <br />
        <asp:Button ID="removeNews" CSSclass="newsbutton" runat="server" Text="Ta bort nyhet" OnClick="btnRemoveNews_Click" />
        <br />

     </section>
</asp:Content>
