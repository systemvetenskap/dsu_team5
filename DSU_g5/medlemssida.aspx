<%@ Page Title="" Language="C#" MasterPageFile="~/headsite.Master" AutoEventWireup="true" CodeBehind="medlemssida.aspx.cs" Inherits="DSU_g5.medlemssida" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/CSS_medlemssida.css" rel="stylesheet" />
    <style type="text/css">
        #Select1 {
            width: 177px;
        }
        .memberbutton {}
    </style>
    <script src="http://code.jquery.com/jquery-1.10.2.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">    
    <section id="medlemssida">
        <div id="medlemsgolfrundor">
             <asp:GridView ID="gvGameMember" CssClass="usergrid" runat="server" AutoPostBack="true" ></asp:GridView>
             <asp:Button ID="btBookCancelGame" CssClass="userbutton" runat="server" Text="Boka/avboka spel"/>
        </div>
        <div id="regMedlemstavlingarFold" class="foldable" onclick="toggleSection('medlemstavlingar')"><p>Medlemstavlingar</p></div>
        <div id="medlemstavlingar">
             <asp:GridView ID="gvMemberTournament" CssClass="usergrid" runat="server" AutoPostBack="true" ></asp:GridView>
             <asp:Button ID="btBookCancelTournament" CssClass="userbutton" runat="server" Text="Boka/avboka tävling"/>
        </div>
        <div id="medlemsuppgifterFold" class="foldable" onclick="toggleSection('medlemsuppgifter')"><p>Medlemsuppgifter</p></div>
        <div id="medlemsuppgifterE">
            <div id="medlemsuppgifter">
                <asp:Label ID="lbIdMember" CssClass="memberlabel" runat="server" Text="Medlems ID"></asp:Label>
                <asp:TextBox ID="tbIdMember" CssClass="membertextbox" runat="server"></asp:TextBox>
                <br />
                <asp:Label ID="lbFirstName" CssClass="memberlabel" runat="server" Text="Förnamn"></asp:Label>
                <asp:TextBox ID="tbFirstName" CssClass="membertextbox" runat="server"></asp:TextBox>
                <br />
                <asp:Label ID="lbLastName" CssClass="memberlabel" runat="server" Text="Efternamn"></asp:Label>
                <asp:TextBox ID="tbLastName" CssClass="membertextbox" runat="server"></asp:TextBox>
                <br />        
                <asp:Label ID="lbAddress" CssClass="memberlabel" runat="server" Text="Address"></asp:Label>
                <asp:TextBox ID="tbAddress" CssClass="membertextbox" runat="server"></asp:TextBox>
                <br />
                <asp:Label ID="lbPostalCode" CssClass="memberlabel" runat="server" Text="Postkod"></asp:Label>
                <asp:TextBox ID="tbPostalCode" CssClass="membertextbox" runat="server" MaxLength="5" input type ="number"></asp:TextBox>
                <br />
                <asp:Label ID="lbCity" CssClass="memberlabel" runat="server" Text="Stad"></asp:Label>
                <asp:TextBox ID="tbCity" CssClass="membertextbox" runat="server"></asp:TextBox>
                <br />
                <asp:Label ID="lbMail" CssClass="memberlabel" runat="server" Text="E-post"></asp:Label>
                <asp:TextBox ID="tbMail" CssClass="membertextbox" runat="server"></asp:TextBox>
                <br />
                <asp:Label ID="lbGender" CssClass="memberlabel" runat="server" Text="Kön"></asp:Label>
                <asp:DropDownList ID="ddlGender" CssClass="membertextbox" runat="server" ></asp:DropDownList>
                <br />
                <asp:Label ID="lbHcp" CssClass="memberlabel" runat="server" Text="HCP"></asp:Label>
                <asp:TextBox ID="tbHcp" CssClass="membertextbox" runat="server" ReadOnly="true" ></asp:TextBox>
                <br />
                <asp:Label ID="lbGolfId" CssClass="memberlabel" runat="server" Text="Golf Id"></asp:Label>
                <asp:TextBox ID="tbGolfId" CssClass="membertextbox" runat="server" ReadOnly="true"> </asp:TextBox>
                <br />
                <asp:Label ID="lbCategory" CssClass="memberlabel" runat="server" Text="Medlems kategori" ></asp:Label>
                <asp:DropDownList ID="ddlCategory" CssClass="membertextbox" runat="server" AutoPostBack="true" ></asp:DropDownList>     
                <br />
                <asp:Label ID="lbAccessCategory" CssClass="memberlabel" runat="server" Text="Access Kategori" ></asp:Label>
                <asp:DropDownList ID="ddlAccessCategory" CssClass="membertextbox" runat="server" AutoPostBack="true"></asp:DropDownList>     
                <br />        
                <asp:Label ID="lbPayment" CssClass="memberlabel" runat="server" Text="Betalning" ></asp:Label>
                <asp:CheckBox ID="cbPayment" CssClass="membercheckbox" runat="server" />
                <br />
                <br />
                <asp:Label ID="lbIdUser" CssClass="memberlabel" runat="server" Text="AnvändarId"></asp:Label>
                <asp:TextBox ID="tbIdUser" CssClass="membertextbox" runat="server"></asp:TextBox>
                <br />
                <asp:Label ID="lbUserName" CssClass="memberlabel" runat="server" Text="Användarnamn"></asp:Label>
                <asp:TextBox ID="tbUserName" CssClass="membertextbox" runat="server"></asp:TextBox>
                <br />
                <asp:Label ID="lbUserPassword" CssClass="memberlabel" runat="server" Text="Lösenord"></asp:Label>
                <asp:TextBox ID="tbUserPassword" CssClass="membertextbox" runat="server"></asp:TextBox>
                <br />
                <asp:Label ID="lbFkIdMember" CssClass="memberlabel" runat="server" Text="MedlemsId"></asp:Label>
                <asp:TextBox ID="tbFkIdMember" CssClass="membertextbox" runat="server"></asp:TextBox>
            </div>
                <div id="userbuttons">
                 <asp:Button ID="btSave" CssClass="userbutton" runat="server" Text="Uppdatera" OnClick="btSave_Click" />
                 <asp:Button ID="btRemove" CssClass="userbutton" runat="server" Text="Ta bort" OnClick="btRemove_Click" />
                 <asp:Label ID="Label2" runat="server" CssClass="usermessage"></asp:Label>
                 <br />
                 <asp:Label ID="lbUserMessage" runat="server" CssClass="usermessage"></asp:Label>
                 <br />
            </div>
        </div>
        <asp:HiddenField ID="hfmedlemstavlingarFolded" runat="server" />
        <asp:HiddenField ID="hfmedlemsuppgifterFolded" runat="server" />     
    </section>
    <script>
        function toggleSection(section) {
            if ($("#ContentPlaceHolder1_hf" + section + "Folded").val() == "true") {
                $("#" + section).show();
                $("#ContentPlaceHolder1_hf" + section + "Folded").val("false");
            }
            else {
                $("#" + section).hide();
                $("#ContentPlaceHolder1_hf" + section + "Folded").val("true");
            }
        }

        function isPostBack() {
            return document.referrer.indexOf(document.location.href) > -1;
        }

        if (isPostBack()) {
            if ($("#ContentPlaceHolder1_hfmedlemstavlingarFolded").val() == "true") {
                $("#medlemstavlingar").hide();
            }
            else {
                $("#medlemstavlingar").show();
            }
            if ($("#ContentPlaceHolder1_hfsponsorsFolded").val() == "true") {
                $("#medlemsuppgifter").hide();
            }
            else {
                $("#medlemsuppgifter").show();
            }
        }
        else {
            toggleSection("medlemstavlingar");
            toggleSection("medlemsuppgifter");
        }
    </script>
</asp:Content>
