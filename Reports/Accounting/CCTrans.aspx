<%@ Page Title="CCTrans Report" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="CCTrans.aspx.vb" Inherits="Reports_Accounting_CCTrans" aspcompat = "true"%>

<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>Account:</td>
            <td>
                <asp:DropDownList ID="ddAccounts" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>Start Date:</td>
            <td>
                <uc1:DateField ID="dteSDate" runat="server" />
            </td>
        </tr>
        <tr>
            <td>End Date:</td>
            <td>
                <uc1:DateField ID="dteEDate" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="Button1" runat="server" Text="Run Report" />
                <asp:Button ID="Button2" runat="server" Text="Excel" />
            </td>
        </tr>
    </table>

    <asp:Literal runat="server" id = "litReport"></asp:Literal>
</asp:Content>

