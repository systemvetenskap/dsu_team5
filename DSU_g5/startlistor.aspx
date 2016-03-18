<%@ Page Title="" Language="C#" MasterPageFile="~/headsite.Master" AutoEventWireup="true" CodeBehind="startlistor.aspx.cs" Inherits="DSU_g5.startlistor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/CSS_startlistor.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section>
        <div id="startlistor">
            <div id="allaStartlistor">
                <asp:DropDownList ID="ddlTournamentList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTournamentList_SelectedIndexChanged"></asp:DropDownList>
                <%--<input id="tbMembersPerGroup" type="number" min="3" max="4" runat="server" maxlength="1" />
                --%><br />
                <asp:TextBox ID="tbMemPerGroup" runat="server" MaxLength="1" Text="3" TextMode="Number"></asp:TextBox>
                <br />
                <asp:RangeValidator ID="rvTextbox" runat="server" ErrorMessage="Värdet måste vara 3 eller 4" Display="Dynamic" ControlToValidate="tbMemPerGroup" Type="Integer" MinimumValue="3" MaximumValue="4"></asp:RangeValidator>
                <br />
                <br />
                <%--<asp:ListBox ID="LsbParticipants" runat="server" AutoPostBack="true" OnSelectedIndexChanged="LsbParticipants_SelectedIndexChanged" Rows="7"></asp:ListBox>--%>
                <asp:Button ID="btnStartlist" runat="server" Text="Skapa startgrupper" OnClick="btnStartlist_Click" />
                <br />
                <asp:GridView ID="gvRandom" runat="server" OnSelectedIndexChanged="gvRandom_SelectedIndexChanged" ></asp:GridView>
                <br />
            </div>

            <div>

            </div>
            

        </div>

    </section>
    <asp:HiddenField ID="hfTourId" runat="server" />
</asp:Content>
