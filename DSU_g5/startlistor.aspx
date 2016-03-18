<%@ Page Title="" Language="C#" MasterPageFile="~/headsite.Master" AutoEventWireup="true" CodeBehind="startlistor.aspx.cs" Inherits="DSU_g5.startlistor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/CSS_startlistor.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section>
        <div id="startlistor">
            <div id="allaStartlistor">
                <asp:DropDownList ID="ddlTournamentList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTournamentList_SelectedIndexChanged" CssClass="dropdown"></asp:DropDownList>
                <%--<input id="tbMembersPerGroup" type="number" min="3" max="4" runat="server" maxlength="1" />
                --%><br />
                <asp:Label ID="lblMemPerGroup" runat="server" Text="Antal medlemmar i grupp: "></asp:Label>
                <asp:TextBox ID="tbMemPerGroup" runat="server" MaxLength="1" Text="3" TextMode="Number" Width="36px" Height="16px"></asp:TextBox>
                <br />
                <asp:RangeValidator ID="rvTextbox" runat="server" ErrorMessage="Värdet måste vara 3 eller 4" Display="Dynamic" ControlToValidate="tbMemPerGroup" Type="Integer" MinimumValue="3" MaximumValue="4" ForeColor="Red" Font-Bold="True"></asp:RangeValidator>
                <br />
                <%--<asp:ListBox ID="LsbParticipants" runat="server" AutoPostBack="true" OnSelectedIndexChanged="LsbParticipants_SelectedIndexChanged" Rows="7"></asp:ListBox>--%>
                <asp:Button ID="btnStartlist" runat="server" Text="Skapa startgrupper" OnClick="btnStartlist_Click" />
                <br />
                <br />
                <asp:GridView ID="gvRandom" runat="server" ></asp:GridView>
                <br />
            </div>

            <div id="startlistorMedStarttider">
                <asp:DropDownList ID="ddlTourWithStarttime" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTourWithStarttime_SelectedIndexChanged"></asp:DropDownList>
                <br />
                <br />
                <asp:GridView ID="gvHasStartlist" runat="server"></asp:GridView>
            </div>

            

        </div>

    </section>
    <asp:HiddenField ID="hfTourId" runat="server" />
    <asp:HiddenField ID="hfTourWithST" runat="server" />
</asp:Content>
