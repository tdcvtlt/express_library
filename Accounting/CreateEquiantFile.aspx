<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="CreateEquiantFile.aspx.vb" Inherits="Accounting_CreateEquiantFile" %>

<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td colspan="3">Select Funding(s)</td>
        </tr>
        <tr>
            <td valign="top">
                <asp:DropDownList ID="ddFunding" runat="server">
                </asp:DropDownList>
            </td>
            <td>
                <asp:Button ID="btnAdd" runat="server" Text=">>" /><br />
                <asp:Button ID="btnRemove" runat="server" Text="<<" />
            </td>
            <td>
                <asp:ListBox ID="lstFunding" runat="server" Rows="5"></asp:ListBox>
            </td>
        </tr>
        <tr>
            <td>Lender:</td>
            <td><asp:dropdownlist ID="ddLender" runat="server">
                <asp:ListItem Value="0">Select One</asp:ListItem>
                <asp:ListItem Value="446">Colebrook</asp:ListItem>
                <asp:ListItem Value="388">Liberty</asp:ListItem>
                <asp:ListItem Value="110">Developer</asp:ListItem>
                </asp:dropdownlist></td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="GetWorkBook" runat="server" Text="Generate File" />
            </td>
            <td>
                <asp:Button ID="GetCancellations" runat="server" Text="Generate Cancellation File" />
            </td>
        </tr>
    </table>
    <asp:Label ID="lblErr" runat="server" Text="" ForeColor="Red"></asp:Label>
</asp:Content>

