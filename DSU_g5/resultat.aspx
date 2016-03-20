<%@ Page Title="" Language="C#" MasterPageFile="~/headsite.Master" AutoEventWireup="true" CodeBehind="resultat.aspx.cs" Inherits="DSU_g5.resultat" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/CSS_resultat.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="result" runat="server">
        <div id="tournament" runat="server">
            <script src="http://code.jquery.com/jquery-1.10.2.js"></script>
            <asp:Label ID="lbTournamentList" CssClass="tournamentlabel" runat="server" Text="Tävlingar"></asp:Label>
            <br /> 
            <asp:ListBox ID="lblTournamentList" CssClass="tournamentlistbox" runat="server" AutoPostBack="true" OnSelectedIndexChanged="lblTournamentList_SelectedIndexChanged" ></asp:ListBox>
            <br />
            <asp:Label ID="lbSearchTournament" CssClass="tournamentlabel" runat="server" Text="Sök tävling"></asp:Label>
            <br />
            <asp:TextBox ID="tbSearchTournament" CssClass="tournamenttextbox" runat="server" ></asp:TextBox>  
            <script>
                document.getElementById("ContentPlaceHolder1_tbSearchTournament").addEventListener("input", ListBoxFilter);
                function ListBoxFilter() {
                    var input = $("#ContentPlaceHolder1_tbSearchTournament").val();
                    var regex = new RegExp(input, "i");
                    var antalPoster = $("#ContentPlaceHolder1_lblTournamentList").children().length;
                    for (i = 0; i < antalPoster; i++) {
                        var namn = $("#ContentPlaceHolder1_lblTournamentList").children()[i].innerHTML;
                        if (!namn.match(regex)) {
                            $("#ContentPlaceHolder1_lblTournamentList option:eq(" + i + ")").hide();
                        }
                        else {
                            $("#ContentPlaceHolder1_lblTournamentList option:eq(" + i + ")").show();
                        }
                    }
                }
            </script>
            <br />
            <br />
            <br />
            <asp:Label ID="lbParticipantList" CssClass="tournamentlabel" runat="server" Text="Tävlingsdeltagare"></asp:Label>
            <br />
            <asp:ListBox ID="lblParticipantList" CssClass="tournamentlistbox" runat="server" AutoPostBack="true" OnSelectedIndexChanged="lblParticipantList_SelectedIndexChanged" ></asp:ListBox>
            <br />
            <asp:Label ID="lbSearchParticipant" CssClass="tournamentlabel" runat="server" Text="Sök deltagare"></asp:Label>
            <br />
            <asp:TextBox ID="tbSearchParticipant" CssClass="tournamenttextbox" runat="server" ></asp:TextBox>
            <script>
                document.getElementById("ContentPlaceHolder1_tbSearchParticipant").addEventListener("input", ListBoxFilter);
                function ListBoxFilter() {
                    var input = $("#ContentPlaceHolder1_tbSearchParticipant").val();
                    var regex = new RegExp(input, "i");
                    var antalPoster = $("#ContentPlaceHolder1_lblParticipantList").children().length;
                    for (i = 0; i < antalPoster; i++) {
                        var namn = $("#ContentPlaceHolder1_lblParticipantList").children()[i].innerHTML;
                        if (!namn.match(regex)) {
                            $("#ContentPlaceHolder1_lblParticipantList option:eq(" + i + ")").hide();
                        }
                        else {
                            $("#ContentPlaceHolder1_lblParticipantList option:eq(" + i + ")").show();
                        }
                    }
                }
            </script>
        </div> 
        <div id="tournamentresult" runat="server">            
            <div id="usertable" runat="server" >
                <asp:GridView ID="gvParticipantResults" runat="server" AutoPostBack="true" ></asp:GridView>
            </div>
            <div id="userbuttons" runat="server">
                <asp:Button ID="btSave" CssClass="userbutton" runat="server" Text="Spara" OnClick="btSave_Click" />
                <asp:Button ID="btRemove" CssClass="userbutton" runat="server" Text="Ta bort" OnClick="btRemove_Click" />
            </div>
            <div id="usermessage" runat="server">
                 <asp:Label ID="lbUserMessage" Text="Testar labellen" runat="server" ></asp:Label>
            </div>
        </div>
        <div id="state" runat="server">
            <asp:Label ID="lblstate" Text="" runat="server" ></asp:Label>
        </div>        
    </div>
</asp:Content>
