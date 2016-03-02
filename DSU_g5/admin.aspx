<%@ Page Title="" Language="C#" MasterPageFile="~/headsite.Master" AutoEventWireup="true" CodeBehind="admin.aspx.cs" Inherits="DSU_g5.admin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <section id="adminsida">
        <asp:Label ID="lblNews" runat="server" Text="Nyheter"></asp:Label>
        <textarea id="textNews" runat="server" cols="24" rows="20">jfksjsdlg</textarea>
        <asp:Button ID="publishNews" runat="server" Text="Publicera nyheter" OnClick="btnPublish_Click" />
     </section>
</asp:Content>
