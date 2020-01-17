<%@ Page Title="Checks Printed" AspCompat="true" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="ChecksPrinted.aspx.vb" Inherits="Reports_Accounting_ChecksPrinted" %>

<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>Start Date:</td>
            <td>
                <uc1:DateField ID="dfStartDate" runat="server" />
            </td>
        </tr>
        <tr>
            <td>End Date:</td>
            <td>
                <uc1:DateField ID="dfEndDate" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                
                <asp:Button ID="btnRun" runat="server" Text="Run Report" />
                <asp:Button ID="btnExport" runat="server" Text="Export" />
                
            </td>
        </tr>
    </table> 
    <asp:Literal ID="Lit1" runat="server"></asp:Literal>
</asp:Content>

