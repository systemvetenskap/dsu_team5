﻿<%@ Page Title="" Language="C#" MasterPageFile="~/headsite.Master" AutoEventWireup="true" CodeBehind="tavlingar.aspx.cs" Inherits="DSU_g5.tavlingar" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/CSS_tavlingar.css" rel="stylesheet" />
    <script>
        $(function () {
            var accessId = '<%= Session["IdAccess"] %>';
            if (accessId < 1) {
                $(".hideButton").hide();
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<h2 id="headline_1" class="headline">Tävlingar</h2>
       <div class="dropdown">               
               <asp:DropDownList ID="ddlTourName" CSSclass="dropdown" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTourName_SelectedIndexChanged" Visible="False"></asp:DropDownList>              
       </div>
       <div id="tournament">
          <%-- <div id="tournamentSort">
               <div id="startDate">
                   <asp:Label ID="lblStartDateTour" runat="server" Text="Visa nyheter från:"></asp:Label>
                   <asp:DropDownList ID="ddlStartYear" runat="server"></asp:DropDownList>
                   <asp:DropDownList ID="ddlStartMonth" runat="server"></asp:DropDownList>
               </div>
               <div id="endDate">
                   <asp:Label ID="lblEndDateTour" runat="server" Text="Till:"></asp:Label>
                   <asp:DropDownList ID="ddlEndYear" runat="server"></asp:DropDownList>
                   <asp:DropDownList ID="ddlEndMonth" runat="server"></asp:DropDownList>
               </div>
               <asp:Button ID="btnTourSort" runat="server" Text="Sök" OnClick="btnTourSort_Click" />
           </div>--%>
           <asp:Repeater ID="RepeaterTour" runat="server">
               <ItemTemplate>
                   <div class="TourItem">
                       <%--<h2 id="header_<%# Eval("id_tournament") %>"><%# Eval("tour_name") %></h2>--%> 
                       <h2 id="<%# Eval("id_tournament") %>"><%# Eval("tour_name") %></h2>                                           
                       <p class="tourDate"><%# Eval("tour_date").ToString().Split(' ')[0] %></p>
                       <p class="tourText"><%# Eval("tour_info") %></p>
                       <%--<asp:Label ID="lblTest" runat="server" Text="xxxx"></asp:Label>--%>

                       <asp:Button ID="btnRegister" CssClass="hideButton" runat="server" Text="Anmäl" onCommand="btnRegister_Click" CommandArgument='<%# Eval("id_tournament")%>'/>
                       <asp:Button ID="btnResults" runat="server" Text="Visa resultat" onCommand="btnResults_Command" CommandArgument='<%# Eval("id_tournament")%>'/>
                       <br />
                       <br />
                       <hr />
                   </div>
               </ItemTemplate>
           </asp:Repeater>
               <%--<asp:Label ID="tezt" runat="server" Text="labilLabel"></asp:Label>--%>
       </div>

</asp:Content>
