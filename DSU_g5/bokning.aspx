<%@ Page Title="" Language="C#" MasterPageFile="~/headsite.Master" AutoEventWireup="true" CodeBehind="bokning.aspx.cs" Inherits="DSU_g5.bokning" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Calendar ID="calBokning" runat="server"></asp:Calendar>


<%--    <table style="width: 100%;">
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
    </table>

    <asp:Table ID="Table1" runat="server"></asp:Table>--%>



    <asp:Button ID="BtnShowTable" runat="server" Text="Button" OnClick="BtnShowTable_Click" />



    <asp:Table ID="Table1" runat="server">

    </asp:Table>




    <table id="tabletable" style="width: 100%;">
        <tr>
            <th>08</th>
            <th>09</th>
            <th>10</th>
            <th>11</th>
            <th>12</th>
            <th>13</th>
            <th>14</th>
            <th>15</th>
            <th>16</th>
            <th>17</th>
            <th>18</th>
        </tr>
        
        <tr>
            <td>00 <asp:Button ID="btnTest0800" runat="server" Text="Button" OnClick="btnTest0800_Click" /></td>
            <td>00</td>
            <td>00</td>
            <td>00</td>
            <td>00</td>
            <td>00</td>
            <td>00</td>
            <td>00</td>
            <td>00</td>
            <td>00</td>
        </tr>

        <tr>
            <td>10</td>
            <td>10</td>
            <td>10</td>
            <td>10</td>
            <td>10</td>
            <td>10</td>
            <td>10</td>
            <td>10</td>
            <td>10</td>
            <td>10</td>
            <%--<td>10</td>--%>
        </tr>


        <tr>
            <td>20</td>
            <td>20</td>
            <td>20</td>
            <td>20</td>
            <td>20</td>
            <td>20</td>
            <td>20</td>
            <td>20</td>
            <td>20</td>
            <td>20</td>
            <%--<td>20</td>--%>
        </tr>

        <tr>
            <td>30</td>
            <td>30</td>
            <td>30</td>
            <td>30</td>
            <td>30</td>
            <td>30</td>
            <td>30</td>
            <td>30</td>
            <td>30</td>
            <td>30</td>
            <%--<td>30</td>--%>
        </tr>

        <tr>
            <td>40</td>
            <td>40</td>
            <td>40</td>
            <td>40</td>
            <td>40</td>
            <td>40</td>
            <td>40</td>
            <td>40</td>
            <td>40</td>
            <td>40</td>
            <%--<td>40</td>--%>
        </tr>

        <tr>
            <td>50</td>
            <td>50</td>
            <td>50</td>
            <td>50</td>
            <td>50</td>
            <td>50</td>
            <td>50</td>
            <td>50</td>
            <td>50</td>
            <td>50</td>
            <%--<td>50</td>--%>
        </tr>
    

    </table>
    <br />
    <br />

    <asp:Label ID="lblTest" runat="server" Text="Label"></asp:Label>



</asp:Content>
