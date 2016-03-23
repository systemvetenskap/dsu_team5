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
            <asp:Label ID="lbIdMember" CssClass="memberlabel" runat="server" Text="Medlems-ID"></asp:Label>
            <asp:TextBox ID="tbIdMember" CssClass="membertextbox" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="lbFirstName" CssClass="memberlabel" runat="server" Text="Förnamn"></asp:Label>
            <asp:TextBox ID="tbFirstName" CssClass="membertextbox" runat="server"></asp:TextBox>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="tbFirstName" ValidationExpression="([A-Z]|Å|Ä|Ö|[a-z]|å|ä|ö)*" ErrorMessage ="Endast text" ForeColor="Red" Font-Bold="True"></asp:RegularExpressionValidator>
            <br />
            <asp:Label ID="lbLastName" CssClass="memberlabel" runat="server" Text="Efternamn"></asp:Label>
            <asp:TextBox ID="tbLastName" CssClass="membertextbox" runat="server"></asp:TextBox>
        <asp:RegularExpressionValidator ID="regexpSSN1" runat="server" ControlToValidate="tbLastName" ValidationExpression="([A-Z]|Å|Ä|Ö|[a-z]|å|ä|ö)*" ErrorMessage ="Endast text" ForeColor="Red" Font-Bold="True"></asp:RegularExpressionValidator>
            <br />        
            <asp:Label ID="lbAddress" CssClass="memberlabel" runat="server" Text="Address"></asp:Label>
            <asp:TextBox ID="tbAddress" CssClass="membertextbox" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="lbPostalCode" CssClass="memberlabel" runat="server" Text="Postkod"></asp:Label>
            <asp:TextBox ID="tbPostalCode" CssClass="membertextbox" runat="server"></asp:TextBox>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="tbPostalCode" ValidationExpression="([0-9]|\s){5,6}" ErrorMessage ="Endast numeriskt värde" ForeColor="Red" Font-Bold="True"></asp:RegularExpressionValidator>
            <br />
            <asp:Label ID="lbCity" CssClass="memberlabel" runat="server" Text="Stad"></asp:Label>
            <asp:TextBox ID="tbCity" CssClass="membertextbox" runat="server"></asp:TextBox>
            <asp:RegularExpressionValidator runat="server" id="RegularExpressionValidator2" ControlToValidate="tbCity" ValidationExpression="([A-Z]|Å|Ä|Ö|[a-z]|å|ä|ö)*" ErrorMessage ="Endast text" ForeColor="Red" Font-Bold="True"></asp:RegularExpressionValidator>
            <br />
            <asp:Label ID="lbMail" CssClass="memberlabel" runat="server" Text="E-post"></asp:Label>
            <asp:TextBox ID="tbMail" CssClass="membertextbox" runat="server"></asp:TextBox>
            <asp:RegularExpressionValidator runat="server" id="RegularExpressionValidator3" ControlToValidate="tbMail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ErrorMessage ="Felaktig E-post" ForeColor="Red" Font-Bold="True"></asp:RegularExpressionValidator>
            <br />
            <asp:Label ID="lbGender" CssClass="memberlabel" runat="server" Text="Kön"></asp:Label>
            <asp:DropDownList ID="ddlGender" CssClass="membertextbox" runat="server" ></asp:DropDownList>
            <br />
            <asp:Label ID="lbHcp" CssClass="memberlabel" runat="server" Text="HCP"></asp:Label>
            <asp:TextBox ID="tbHcp" CssClass="membertextbox" runat="server" ></asp:TextBox>
            <asp:RangeValidator ID="rvTextbox" runat="server" ErrorMessage="Värde mellan -10 & 54" Display="Dynamic" ControlToValidate="tbHcp" Type="Double" MinimumValue="-10" MaximumValue="54" ForeColor="Red" Font-Bold="True"></asp:RangeValidator>
            <br />
            <asp:Label ID="lbGolfId" CssClass="memberlabel" runat="server" Text="Golf-ID"></asp:Label>
            <asp:TextBox ID="tbGolfId" CssClass="membertextbox" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="lbCategory" CssClass="memberlabel" runat="server" Text="Medlemskategori" ></asp:Label>
            <asp:DropDownList ID="ddlCategory" CssClass="membertextbox" runat="server" ></asp:DropDownList>     
            <br />
            <asp:Label ID="lbAccessCategory" CssClass="memberlabel" runat="server" Text="Behörighet" ></asp:Label>
            <asp:DropDownList ID="ddlAccessCategory" CssClass="membertextbox" runat="server" ></asp:DropDownList>     
            <br />        
            <asp:Label ID="lbPayment" CssClass="memberlabel" runat="server" Text="Betalning" ></asp:Label>
            <asp:CheckBox ID="cbPayment" CssClass="membercheckbox" runat="server" />
            <br />
            <br />
            <asp:Label ID="lbIdUser" CssClass="memberlabel" runat="server" Text="Användar-ID"></asp:Label>
            <asp:TextBox ID="tbIdUser" CssClass="membertextbox" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="lbUserName" CssClass="memberlabel" runat="server" Text="Användarnamn"></asp:Label>
            <asp:TextBox ID="tbUserName" CssClass="membertextbox" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="lbUserPassword" CssClass="memberlabel" runat="server" Text="Lösenord"></asp:Label>
            <asp:TextBox ID="tbUserPassword" CssClass="membertextbox" runat="server"></asp:TextBox>
            <br />
            <asp:Label ID="lbFkIdMember" CssClass="memberlabel" runat="server" Text="Medlems-ID"></asp:Label>
            <asp:TextBox ID="tbFkIdMember" CssClass="membertextbox" runat="server"></asp:TextBox>
            <br />
        </div>
        <div id="medlemslista">
            <asp:Label ID="lbSearch" CssClass="memberlabel" runat="server" Text="Sök medlem"></asp:Label>
            <br />
            <asp:TextBox ID="tbSearch" CssClass="membertextbox" runat="server" Width="209px"></asp:TextBox>
            <asp:HiddenField ID="hfSearchMember" runat="server" OnValueChanged="hfSearchMember_ValueChanged" />
            <br />
            <br />
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
