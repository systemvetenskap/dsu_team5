<%@ Page Title="" Language="C#" MasterPageFile="~/headsite.Master" AutoEventWireup="true" CodeBehind="scorekort.aspx.cs" Inherits="DSU_g5.scorekort" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="CSS/CSS_scorekort.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section>
        <div id="tries1-9">
            <asp:Label ID="lblslagkort" runat="server" Text="Slagkort"></asp:Label>
            <br />
            <asp:Label ID="lbl1" runat="server" Text="Hål 1"></asp:Label>
            <asp:TextBox ID="txb1" runat="server" ></asp:TextBox>
            <asp:CompareValidator ControlToValidate="txb1" runat="server" ErrorMessage="Antal slag måste anges i siffror" Operator="DataTypeCheck" Type="Integer" ></asp:CompareValidator>
            <br />
            <asp:Label ID="lbl2" runat="server" Text="Hål 2"></asp:Label>
            <asp:TextBox ID="txb2" runat="server"></asp:TextBox>
            <asp:CompareValidator ControlToValidate="txb2" runat="server" ErrorMessage="Antal slag måste anges i siffror" Operator="DataTypeCheck" Type="Integer" ></asp:CompareValidator>
            <br />
            <asp:Label ID="lbl3" runat="server" Text="Hål 3"></asp:Label>
            <asp:TextBox ID="txb3" runat="server"></asp:TextBox>
            <asp:CompareValidator ControlToValidate="txb3" runat="server" ErrorMessage="Antal slag måste anges i siffror" Operator="DataTypeCheck" Type="Integer" ></asp:CompareValidator>
            <br />
            <asp:Label ID="lbl4" runat="server" Text="Hål 4"></asp:Label>
            <asp:TextBox ID="txb4" runat="server"></asp:TextBox>
            <asp:CompareValidator ControlToValidate="txb4" runat="server" ErrorMessage="Antal slag måste anges i siffror" Operator="DataTypeCheck" Type="Integer" ></asp:CompareValidator>
            <br />
            <asp:Label ID="lbl5" runat="server" Text="Hål 5"></asp:Label>
            <asp:TextBox ID="txb5" runat="server"></asp:TextBox>
            <asp:CompareValidator ControlToValidate="txb5" runat="server" ErrorMessage="Antal slag måste anges i siffror" Operator="DataTypeCheck" Type="Integer" ></asp:CompareValidator>
            <br />
            <asp:Label ID="lbl6" runat="server" Text="Hål 6"></asp:Label>
            <asp:TextBox ID="txb6" runat="server"></asp:TextBox>
            <asp:CompareValidator ControlToValidate="txb6" runat="server" ErrorMessage="Antal slag måste anges i siffror" Operator="DataTypeCheck" Type="Integer" ></asp:CompareValidator>
            <br />
            <asp:Label ID="lbl7" runat="server" Text="Hål 7"></asp:Label>
            <asp:TextBox ID="txb7" runat="server" ></asp:TextBox>
            <asp:CompareValidator ControlToValidate="txb7" runat="server" ErrorMessage="Antal slag måste anges i siffror" Operator="DataTypeCheck" Type="Integer" ></asp:CompareValidator>
            <br />
            <asp:Label ID="lbl8" runat="server" Text="Hål 8"></asp:Label>
            <asp:TextBox ID="txb8" runat="server"></asp:TextBox>
            <asp:CompareValidator ControlToValidate="txb8" runat="server" ErrorMessage="Antal slag måste anges i siffror" Operator="DataTypeCheck" Type="Integer" ></asp:CompareValidator>
            <br />
            <asp:Label ID="lbl9" runat="server" Text="Hål 9"></asp:Label>
            <asp:TextBox ID="txb9" runat="server"></asp:TextBox>
            <asp:CompareValidator ControlToValidate="txb9" runat="server" ErrorMessage="Antal slag måste anges i siffror" Operator="DataTypeCheck" Type="Integer" ></asp:CompareValidator>
            <br />
            
            </div>
            <div id="tries10-18">
            <asp:Label ID="lbl10" runat="server" Text="Hål 10"></asp:Label>
            <asp:TextBox ID="txb10" runat="server"></asp:TextBox>
            <asp:CompareValidator ControlToValidate="txb10" runat="server" ErrorMessage="Antal slag måste anges i siffror" Operator="DataTypeCheck" Type="Integer" ></asp:CompareValidator>
            <br />
            <asp:Label ID="lbl11" runat="server" Text="Hål 11"></asp:Label>
            <asp:TextBox ID="txb11" runat="server"></asp:TextBox>
            <asp:CompareValidator ControlToValidate="txb11" runat="server" ErrorMessage="Antal slag måste anges i siffror" Operator="DataTypeCheck" Type="Integer" ></asp:CompareValidator>
            <br />
            <asp:Label ID="lbl12" runat="server" Text="Hål 12"></asp:Label>
            <asp:TextBox ID="txb12" runat="server"></asp:TextBox>
            <asp:CompareValidator ControlToValidate="txb12" runat="server" ErrorMessage="Antal slag måste anges i siffror" Operator="DataTypeCheck" Type="Integer" ></asp:CompareValidator>
            <br />
            <asp:Label ID="lbl13" runat="server" Text="Hål 13"></asp:Label>
            <asp:TextBox ID="txb13" runat="server"></asp:TextBox>
            <asp:CompareValidator ControlToValidate="txb13" runat="server" ErrorMessage="Antal slag måste anges i siffror" Operator="DataTypeCheck" Type="Integer" ></asp:CompareValidator>
            <br />
            <asp:Label ID="lbl14" runat="server" Text="Hål 14"></asp:Label>
            <asp:TextBox ID="txb14" runat="server"></asp:TextBox>
            <asp:CompareValidator ControlToValidate="txb14" runat="server" ErrorMessage="Antal slag måste anges i siffror" Operator="DataTypeCheck" Type="Integer" ></asp:CompareValidator>
            <br />
            <asp:Label ID="lbl15" runat="server" Text="Hål 15"></asp:Label>
            <asp:TextBox ID="txb15" runat="server"></asp:TextBox>
            <asp:CompareValidator ControlToValidate="txb15" runat="server" ErrorMessage="Antal slag måste anges i siffror" Operator="DataTypeCheck" Type="Integer" ></asp:CompareValidator>
            <br />
            <asp:Label ID="lbl16" runat="server" Text="Hål 16"></asp:Label>
            <asp:TextBox ID="txb16" runat="server"></asp:TextBox>
            <asp:CompareValidator ControlToValidate="txb16" runat="server" ErrorMessage="Antal slag måste anges i siffror" Operator="DataTypeCheck" Type="Integer" ></asp:CompareValidator>
            <br />
            <asp:Label ID="lbl17" runat="server" Text="Hål 17"></asp:Label>
            <asp:TextBox ID="txb17" runat="server"></asp:TextBox>
            <asp:CompareValidator ControlToValidate="txb17" runat="server" ErrorMessage="Antal slag måste anges i siffror" Operator="DataTypeCheck" Type="Integer" ></asp:CompareValidator>
            <br />
            <asp:Label ID="lbl18" runat="server" Text="Hål 18"></asp:Label>
            <asp:TextBox ID="txb18" runat="server"></asp:TextBox>
            <asp:CompareValidator ControlToValidate="txb18" runat="server" ErrorMessage="Antal slag måste anges i siffror" Operator="DataTypeCheck" Type="Integer" ></asp:CompareValidator>
            <br />
            <asp:Button ID="btnupdate" runat="server" Text="Uppdatera resultat" OnClick="btnupdate_Click" />
            <br />
            <asp:Button ID="btnReturn" runat="server" Text="Tillbaka till resultatlista"  OnClick="btnReturn_Click" />
            <br />
            </div>
            <div id="usermessage" runat="server">
                 <asp:Label ID="lbUserMessage" Text="Testar labellen" runat="server" ></asp:Label>
         </div>
    </section>
</asp:Content>
