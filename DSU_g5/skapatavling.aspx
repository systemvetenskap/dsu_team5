<%@ Page Title="" Language="C#" MasterPageFile="~/headsite.Master" AutoEventWireup="true" CodeBehind="skapatavling.aspx.cs" Inherits="DSU_g5.skapatavling" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/CSS_skapatavling.css" rel="stylesheet" />
    <script src="http://code.jquery.com/jquery-1.10.2.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section>
        <h2>Skapa tävling</h2>
        <div id="tournamentInfo">
            <div id="info">
                <asp:Label ID="lblName" runat="server" Text="Tävlingens namn"></asp:Label>
                <br />
                <asp:TextBox ID="tbName" runat="server"></asp:TextBox>
                <br />
                <asp:Label ID="lblGameForm" runat="server" Text="Tävlingsform"></asp:Label>
                <br />
                <asp:DropDownList ID="ddlGameForm" runat="server"></asp:DropDownList>
                <br />
                <asp:Label ID="lblMemberCategory" runat="server" Text="Medlemskategori"></asp:Label>
                <br />
                <asp:DropDownList ID="ddlMemberCategory" runat="server"></asp:DropDownList>
            </div>
            <div id="infobox">
                <asp:Label ID="lblInformation" runat="server" Text="Information"></asp:Label>
                <br />
                <textarea id="taInformation" runat="server" cols="36" rows="16"></textarea>
            </div>
        </div>
        <div id="tournamentTimes">
            <div id="date">
                <asp:Label ID="lblDate" runat="server" Text="Tävlingens datum"></asp:Label>
                <asp:Calendar ID="calDate" runat="server"></asp:Calendar>
            </div>
            <div id="time">
                <asp:Label ID="lblStartTime" runat="server" Text="Starttid (00:00)"></asp:Label>
                <br />
                <asp:TextBox ID="tbStartTime" runat="server" Text=":"></asp:TextBox>
                <br />
                <br />
                <asp:Label ID="lblEndTime" runat="server" Text="Sluttid (00:00)"></asp:Label>
                <br />
                <asp:TextBox ID="tbEndTime" runat="server" Text=":"></asp:TextBox>
            </div>
        </div>
        <div id="regTimesFold" class="foldable" onclick="toggleSection('registrationTimes')"><p>Kontaktperson & registreringstider</p></div>
        <div id="registrationTimes">
            <div id="contact">
                <asp:Label ID="lblContactPerson" runat="server" Text="Kontaktperson"></asp:Label>
                <br />
                <asp:ListBox ID="lbContactPerson" runat="server"></asp:ListBox>
                <br />
                <asp:Label ID="lblSokContactPerson" runat="server" Text="Sök person"></asp:Label>
                <br />
                <asp:TextBox ID="tbSokContactPerson" runat="server"></asp:TextBox>
            </div>
            <div>
                <asp:Label ID="lblRegStart" runat="server" Text="Första registreringsdatum"></asp:Label>
                <asp:Calendar ID="calRegStart" runat="server"></asp:Calendar>
            </div>
            <div>
                <asp:Label ID="lblRegEnd" runat="server" Text="Sista registreringsdatum"></asp:Label>
                <asp:Calendar ID="calRegEnd" runat="server"></asp:Calendar>
            </div>
            <div>
                <asp:Label ID="lblPublishList" runat="server" Text="Startlistor publiceras"></asp:Label>
                <asp:Calendar ID="calPublishList" runat="server"></asp:Calendar>
            </div>
        </div>
        <div id="sponsorsFold" class="foldable" onclick="toggleSection('sponsors')"><p>Sponsorer</p></div>
        <div id="sponsors">
            <div id="sponsors_1">
                <asp:Label ID="lblSponsors" runat="server" Text="Sponsorer"></asp:Label>
                <br />
                <asp:ListBox ID="lbSponsors" runat="server"></asp:ListBox>
                <br />
                <asp:Button ID="btnSponsorsRemove" runat="server" Text="Ta bort" OnClick="btnSponsorsRemove_Click" />
            </div>
            <div id="sponsors_2">
                <asp:Label ID="lblFormerSponsors" runat="server" Text="Tidigare sponsorer"></asp:Label>
                <br />
                <asp:ListBox ID="lbFormerSponsors" runat="server"></asp:ListBox>
                <br />
                <asp:Button ID="btnFormerSponsorsAdd" runat="server" Text="Lägg till" OnClick="btnFormerSponsorsAdd_Click" />
            </div>
            <div id="sponsors_3">
                <asp:Label ID="lblNewSponsor" runat="server" Text="Registrera ny sponsor"></asp:Label>
                <br />
                <asp:Label ID="lblNewSponsorName" runat="server" Text="Namn"></asp:Label>
                <br />
                <asp:TextBox ID="tbNewSponsorName" runat="server"></asp:TextBox>
                <br />
                <asp:Label ID="lblNewSponsorPhone" runat="server" Text="Telefon"></asp:Label>
                <br />
                <asp:TextBox ID="tbNewSponsorPhone" runat="server"></asp:TextBox>
                <br />
                <asp:Button ID="btnNewSponsorAdd" runat="server" Text="Registrera" OnClick="btnNewSponsorAdd_Click" />
            </div>
        </div>
        <asp:Button ID="btnSave" runat="server" Text="Spara" OnClick="btnSave_Click" />
        <asp:Button ID="btnClear" runat="server" Text="Rensa" OnClick="btnClear_Click" />
        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
        <asp:HiddenField ID="hfregistrationTimesFolded" runat="server" />
        <asp:HiddenField ID="hfsponsorsFolded" runat="server" />
    </section>
    <script>
        document.getElementById("ContentPlaceHolder1_tbSokContactPerson").addEventListener("input", ListBoxFilter);
            function ListBoxFilter() {
                var input = $("#ContentPlaceHolder1_tbSokContactPerson").val();
                var regex = new RegExp(input, "i");
                var antalPoster = $("#ContentPlaceHolder1_lbContactPerson").children().length;
                for (i = 0; i < antalPoster; i++) {
                    var namn = $("#ContentPlaceHolder1_lbContactPerson").children()[i].innerHTML;
                    if (!namn.match(regex)) {
                        $("#ContentPlaceHolder1_lbContactPerson option:eq(" + i + ")").hide();
                    }
                    else {
                        $("#ContentPlaceHolder1_lbContactPerson option:eq(" + i + ")").show();
                    }
                }
            }

        function toggleSection(section) {
            if ($("#ContentPlaceHolder1_hf"+ section +"Folded").val() == "true"){
                $("#"+ section).show();
                $("#ContentPlaceHolder1_hf"+ section +"Folded").val("false");
            }
            else{
                $("#"+ section).hide();
                $("#ContentPlaceHolder1_hf"+ section +"Folded").val("true");
            }
        }

        function isPostBack(){
                return document.referrer.indexOf(document.location.href) > -1;
            }

        if (isPostBack()){
            if ($("#ContentPlaceHolder1_hfregistrationTimesFolded").val() == "true"){
                $("#registrationTimes").hide();
            }
            else {
                $("#registrationTimes").show();
            }
            if ($("#ContentPlaceHolder1_hfsponsorsFolded").val() == "true"){
                $("#sponsors").hide();
            }
            else {
                $("#sponsors").show();
            }
        }
        else {
            toggleSection("registrationTimes");
            toggleSection("sponsors");
        }
    </script>
</asp:Content>
