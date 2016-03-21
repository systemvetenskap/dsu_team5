<%@ Page Title="" Language="C#" MasterPageFile="~/headsite.Master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="DSU_g5.index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/CSS_index.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">      
    <h2 id="headline_1" class="headline">Nyheter</h2>
       <!--<div class="dropdown">               
               <asp:DropDownList ID="ddlNewsName" CSSclass="newsddl" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlNewsName_SelectedIndexChanged"></asp:DropDownList>              
       </div>-->
       <div id="news">
           <div id="newsSort">
               <div id="startDate">
                   <asp:Label ID="lblStartDate" runat="server" Text="Visa nyheter från:"></asp:Label>
                   <asp:DropDownList ID="ddlStartYear" runat="server"></asp:DropDownList>
                   <asp:DropDownList ID="ddlStartMonth" runat="server"></asp:DropDownList>
               </div>
               <div id="endDate">
                   <asp:Label ID="lblEndDate" runat="server" Text="Till:"></asp:Label>
                   <asp:DropDownList ID="ddlEndYear" runat="server"></asp:DropDownList>
                   <asp:DropDownList ID="ddlEndMonth" runat="server"></asp:DropDownList>
               </div>
               <asp:Button ID="btnNewsSort" runat="server" Text="Sök" OnClick="btnNewsSort_Click" />
           </div>
           <asp:Repeater ID="RepeaterNews" runat="server">
               <ItemTemplate>
                   <div class="newsItem">
                       <h2><%# Eval("news_name") %></h2>
                       <p class="newsDate"><%# Eval("news_date").ToString().Split(' ')[0] %></p>
                       <p class="newsText"><%# Eval("news_info") %></p>
                       <br />
                       <hr />
                   </div>
               </ItemTemplate>
           </asp:Repeater>
       </div>

    <!-- <article id="news_1" class="news"></article>
    <h2 id="headline_2" class="headline"></h2>
    <article id="news_2" class="news"></article>
    <h2 id="headline_3" class="headline"></h2>
    <article id="news_3" class="news"></article>
    <h2 id="headline_4" class="headline"></h2>    
    <article id="news_4" class="news">
        
    </article> -->
</asp:Content>
