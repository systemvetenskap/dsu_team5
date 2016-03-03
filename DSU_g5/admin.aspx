<%@ Page Title="" Language="C#" MasterPageFile="~/headsite.Master" AutoEventWireup="true" CodeBehind="admin.aspx.cs" Inherits="DSU_g5.admin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <section id="adminsida">
        
        <asp:Label ID="lblNewNews" runat="server" Text="Uppdatera/ta bort nyhet" ></asp:Label>
        <asp:TextBox ID="txtNewNews" runat="server"></asp:TextBox>
        <asp:DropDownList ID="ddlNewsName" runat="server"></asp:DropDownList>
        <asp:ListBox ID="lsbOldNews" runat="server"></asp:ListBox>
        <br />
         
        <asp:Label ID="lblNews" runat="server" Text="Nyheter"></asp:Label>
        <textarea id="textNews" runat="server" cols="24" rows="20">jfksjsdlg</textarea>
        <asp:Button ID="publishNews" runat="server" Text="Publicera nyheter" OnClick="btnPublish_Click" />
        <asp:Button ID="updateNews" runat="server" Text="Uppdatera nyhet" OnClick="btnUpdateNews_Click" />
        <asp:Button ID="removeNews" runat="server" Text="Ta bort nyhet" OnClick="btnRemoveNews_Click" />
   

     </section>
</asp:Content>
