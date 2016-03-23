<%@ Page Title="" Language="C#" MasterPageFile="~/headsite.Master" AutoEventWireup="true" CodeBehind="admin.aspx.cs" Inherits="DSU_g5.admin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/CSS_admin.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <section id="adminsida">
         <div id="newsdiv" >
             <asp:Label ID="lblNewNews" CSSclass="newslabel" runat="server" Text="Lägg till nyhet"></asp:Label>
             <br />
             <asp:TextBox ID="txtNewNews" CSSclass="newstextbox" runat="server" AutoPostBack="true" OnTextChanged="txtNewNews_TextChanged"></asp:TextBox>
             <br />
             <asp:Label ID="lblUpdateNews" CSSclass="newslabel" runat="server" Text="Uppdatera nyhet"></asp:Label>
             <br />
             <asp:DropDownList ID="ddlNewsName" CSSclass="dropdown" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlNewsName_SelectedIndexChanged"></asp:DropDownList>
             <br />
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
              <br />
         </div>
         <div id="seasons">
             <asp:Label ID="lblSeasonInst" runat="server" Text="Registrera säsong: Välj Säsongsstart och Säsongsslut samt Tider, tryck Registrera säsong"></asp:Label>
             <br />
             <br />
             <asp:Label ID="lblCloseInst" runat="server" Text="Stäng banan: Välj Säsongsstart och Tider för stängning, tryck Stäng banan"></asp:Label>
             <br />
             <br />
             <asp:Label ID="lblSeason" runat="server" Text="Registrera säsong"></asp:Label>
             <div id="calendars">
                 <div id="seasonStart">
                    <asp:Label ID="lblSeasonStart" runat="server" Text="Säsongsstart/Stänga banan"></asp:Label>
                    <br />
                    <asp:TextBox ID="tbSeasonStartCal" CssClass="calendar" runat="server"></asp:TextBox>
                   <!-- <asp:Calendar ID="startCalendar" runat="server"></asp:Calendar> -->
                 </div>
                 <div id="seasonEnd">
                    <asp:Label ID="lblSeasonEnd" runat="server" Text="Säsongsslut"></asp:Label>
                    <br />
                    <asp:TextBox ID="tbSeasonEndCal" CssClass="calendar" runat="server"></asp:TextBox>
                   <!-- <asp:Calendar ID="endCalendar" runat="server"></asp:Calendar> -->
                 </div>
             </div>
             <div id="times">
                <asp:Label ID="lblSeasonTimes" runat="server" Text="Tider (00:00)"></asp:Label>
                <br />
                <asp:TextBox ID="txtFrom" runat="server"></asp:TextBox> - 
                <asp:TextBox ID="txtTo" runat="server"></asp:TextBox>
             </div>
             <div id="seasonButtons">
                <asp:Button ID="btnAddSeason" runat="server" Text="Registrera säsong" OnClick="btnAddSeason_Click" Width="162px"/>
                <asp:Button ID="btnRemoveDate" runat="server" Text="Stäng Banan" OnClick ="btnRemoveDate_Click" Width="163px" />
                <br />
                <asp:Label ID="lblConformation" runat="server" Text=""></asp:Label>
             </div>
         </div>
     </section>
     <asp:HiddenField ID="hfNewsId" runat="server" />
</asp:Content>
