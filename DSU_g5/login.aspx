﻿<%@ Page Title="" Language="C#" MasterPageFile="~/headsite.Master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="DSU_g5.login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/CSS_login.css" rel="stylesheet" />
    <style type="text/css">
        #Select1 {
            width: 177px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">    
    <section id="loginsida">
        <br />
        <br />
        <br />
        <asp:Label ID="lbUserName" runat="server" Text="Användarnamn"></asp:Label>
        <br />
        <asp:TextBox ID="tbUserName" runat="server" MaxLength="20" Height="25px" Width="180px" ></asp:TextBox>
        <br />
        <br />
        <asp:Label ID="lbUserPassword" runat="server" Text="Lösenord"></asp:Label>
        <br />
        <asp:TextBox ID="tbUserPassword" runat="server" MaxLength="20" TextMode="Password" Height="25px" Width="180px" ></asp:TextBox>
        <br />
        <br />
        <asp:Button ID="btLoggIn" runat="server" Text="Logga in" Height="42px" Width="120px" OnClick="btLoggIn_Click" ></asp:Button>
        <br />
        <asp:Label ID="lbUserMessage" runat="server" CssClass="UserMessage" ForeColor="Red" Font-Bold="True"></asp:Label>
        <br />
    </section>
</asp:Content>
