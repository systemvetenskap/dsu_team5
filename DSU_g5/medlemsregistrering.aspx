<%@ Page Title="" Language="C#" MasterPageFile="~/headsite.Master" AutoEventWireup="true" CodeBehind="medlemsregistrering.aspx.cs" Inherits="DSU_g5.medlemsregistrering" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/CSS_medlemsregistrering.css" rel="stylesheet" />
    <style type="text/css">
        #Select1 {
            width: 177px;
        }
    </style>
    <script>
        $(function () {
            $("#ContentPlaceHolder1_tbSearch").autocomplete({
                source: members,
                focus: function (event, ui) {
                    $("#ContentPlaceHolder1_tbSearch").val(ui.item.label);
                    return false;
                },
                select: function (event, ui) {
                    $("#ContentPlaceHolder1_tbSearch").val(ui.item.label);
                    $("#ContentPlaceHolder1_hfSearchMember").val(ui.item.value);
                    __doPostBack();
                    return false;
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section id="medlemsregistrering">
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
            <asp:TextBox ID="tbPostalCode" CssClass="membertextbox" runat="server"></asp:TextBox>
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
            <asp:TextBox ID="tbHcp" CssClass="membertextbox" runat="server" ></asp:TextBox>
            <br />
            <asp:Label ID="lbGolfId" CssClass="memberlabel" runat="server" Text="Golf Id"></asp:Label>
            <asp:TextBox ID="tbGolfId" CssClass="membertextbox" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="lbCategory" CssClass="memberlabel" runat="server" Text="Medlems kategori" ></asp:Label>
            <asp:DropDownList ID="ddlCategory" CssClass="membertextbox" runat="server" ></asp:DropDownList>     
            <br />
            <asp:Label ID="lbAccessCategory" CssClass="memberlabel" runat="server" Text="Access Kategori" ></asp:Label>
            <asp:DropDownList ID="ddlAccessCategory" CssClass="membertextbox" runat="server" ></asp:DropDownList>     
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
            <br />
        </div>
        <div id="medlemslista">
            <%--<script src="http://code.jquery.com/jquery-1.10.2.js"></script>
            <asp:Label ID="lbMembersList" CssClass="memberlabel" runat="server" Text="Medlemslista"></asp:Label>
            <br />
            <asp:ListBox ID="lblMembers" CssClass="membertextbox" runat="server" Height="191px" Width="215px" AutoPostBack="true" OnSelectedIndexChanged="lblMembers_SelectedIndexChanged" ></asp:ListBox>
            <br />--%>
            <asp:Label ID="lbSearch" CssClass="memberlabel" runat="server" Text="Sök medlem"></asp:Label>
            <br />
            <asp:TextBox ID="tbSearch" CssClass="membertextbox" runat="server" Width="209px"></asp:TextBox>
            <asp:HiddenField ID="hfSearchMember" runat="server" OnValueChanged="hfSearchMember_ValueChanged" />
            <br />
            <br />
            <script>
                //document.getElementById("ContentPlaceHolder1_tbSearch").addEventListener("input", ListBoxFilter);
                function ListBoxFilter() {
                    var input = $("#ContentPlaceHolder1_tbSearch").val();
                    var regex = new RegExp(input, "i");
                    var antalPoster = $("#ContentPlaceHolder1_lblMembers").children().length;
                    for (i = 0; i < antalPoster; i++) {
                        var namn = $("#ContentPlaceHolder1_lblMembers").children()[i].innerHTML;
                        if (!namn.match(regex)) {
                            $("#ContentPlaceHolder1_lblMembers option:eq(" + i + ")").hide();
                        }
                        else {
                            $("#ContentPlaceHolder1_lblMembers option:eq(" + i + ")").show();
                        }
                    }
                }                
            </script>
        </div>
        <div id="knappar">
            <asp:Button ID="btSave" CssClass="memberbutton" runat="server" Text="Spara" OnClick="btSave_Click" />
            <asp:Button ID="btRemove" CssClass="memberbutton" runat="server" Text="Ta bort" OnClick="btRemove_Click" />
            <asp:Button ID="btClear" CssClass="memberbutton" runat="server" Text="Rensa" OnClick="btClear_Click" />
            <br />
            <asp:Label ID="lbUserMessage" runat="server" CssClass="UserMessage"></asp:Label>
            <br />
        </div>
    </section> 
</asp:Content>
