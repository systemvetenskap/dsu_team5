<%@ Page Title="" Language="C#" MasterPageFile="~/headsite.Master" AutoEventWireup="true" CodeBehind="admin.aspx.cs" Inherits="DSU_g5.admin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/CSS_admin.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <section id="adminsida">
         <div id="news">
             <asp:Label ID="lblNewNews" CSSclass="newslabel" runat="server" Text="Lägg till nyhet"></asp:Label>
             <br />
             <asp:TextBox ID="txtNewNews" CSSclass="newstextbox" runat="server">Nyhetsnamn</asp:TextBox>
             <br />
             <asp:Label ID="lblUpdateNews" CSSclass="newslabel" runat="server" Text="Uppdatera nyhet"></asp:Label>
             <br />
             <asp:DropDownList ID="ddlNewsName" CSSclass="newsddl" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlNewsName_SelectedIndexChanged"></asp:DropDownList>
             <br />
             <textarea id="textNews" runat="server" cols="36" rows="16"></textarea>
             <br />
             <asp:Button ID="publishNews" CSSclass="newsbutton" runat="server" Text="Publicera nyheter" OnClick="btnPublish_Click" />
         
             <br />
             <asp:Button ID="updateNews" CSSclass="newsbutton" runat="server" Text="Uppdatera nyhet" OnClick="btnUpdateNews_Click" />
             <br />
             <asp:Button ID="removeNews" CSSclass="newsbutton" runat="server" Text="Ta bort nyhet" OnClick="btnRemoveNews_Click" />
             <br />
             <asp:Button ID="btnMailNews" runat="server" Text="Maila nyhetsbrev" Width="199px" OnClick="btnMailNews_Click" />
         </div>
         <div id="seasons">
             <asp:Label ID="lblSeason" runat="server" Text="Registrera säsong"></asp:Label>
             <div id="seasonStart">
                <asp:Label ID="lblSeasonStart" runat="server" Text="Säsongsstart"></asp:Label>
                 <br/>
                <asp:Calendar ID="startCalendar" runat="server"></asp:Calendar>
             </div>
             <div id="seasonEnd">
                <asp:Label ID="lblSeasonEnd" runat="server" Text="Säsongsslut"></asp:Label>
                <asp:Calendar ID="endCalendar" runat="server"></asp:Calendar>
             </div>
             <asp:Button ID="btnAddSeason" runat="server" Text="Registrera säsong" OnClick="btnAddSeason_Click"/>
         </div>
     </section>
</asp:Content>
