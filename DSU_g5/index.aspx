<%@ Page Title="" Language="C#" MasterPageFile="~/headsite.Master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="DSU_g5.index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/CSS_index.css" rel="stylesheet" />
    <link href="CSS/CSS_admin.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">      
    <h2 id="headline_1" class="headline">Nyheter</h2>
    <p class="post-info">Senast uppdaterad 2015-10-25</p>
        <div class="dropdown">
               
               <asp:DropDownList ID="ddlNewsName" CSSclass="newsddl" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlNewsName_SelectedIndexChanged"></asp:DropDownList>
              
       </div>
       <div class="news">
            
       </div>

    <article id="news_1" class="news"></article>
    <h2 id="headline_2" class="headline"></h2>
    <article id="news_2" class="news"></article>
    <h2 id="headline_3" class="headline"></h2>
    <article id="news_3" class="news"></article>
    <h2 id="headline_4" class="headline"></h2>    
    <article id="news_4" class="news">
        
    </article>
</asp:Content>
