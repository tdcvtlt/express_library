<%@ Page Title="Premium Report" AspCompat="true" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="PremiumReport.aspx.vb" Inherits="Reports_Accounting_PremiumReport" %>

<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>Start Date:</td>
            <td>
                <uc1:DateField ID="dfSDate" runat="server" />
            </td>
        </tr>
        <tr>
            <td>End Date:</td>
            <td>
                <uc1:DateField ID="dfEDate" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btnRun" runat="server" Text="Run Report" />
                <asp:Button ID="btnExcel" runat="server" Text = "Export" />
                
                
            </td>
        </tr>
    </table>
    <asp:Literal ID="Lit1" runat="server"></asp:Literal>
</asp:Content>

