<%@ Page Title="Concierge Report" AspCompat="true" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="ConciergeReport.aspx.vb" Inherits="Reports_FrontDesk_ConciergeReport" %>

<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>Start Date:</td>
            <td><uc1:DateField ID="SDate" runat="server" /></td>
        </tr>
        <tr>
            <td>End Date:</td>
            <td><uc1:DateField ID="EDate" runat="server" /></td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btnReport" Text="Run Report" runat="server" />
                <asp:Button ID="btnExcel" Text="Get Excel" runat="server" />
            </td>
        </tr>
    </table>
    <asp:Literal ID="Lit" runat="server"></asp:Literal>
</asp:Content>

