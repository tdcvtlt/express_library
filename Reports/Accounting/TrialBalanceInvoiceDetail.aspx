<%@ Page Title="Trial Balance" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="TrialBalanceInvoiceDetail.aspx.vb" Inherits="Reports_Accounting_TrialBalanceInvoiceDetail" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>
<%@ Register Src="~/controls/DateField.ascx" TagPrefix="uc1" TagName="DateField" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>Cutoff Date:</td>
            <td><uc1:DateField runat="server" id="CODate" /></td>
        </tr>
        <tr>
            <td colspan="2"><asp:Button ID="btnRun" runat="server" Text="Run Report" /></td>
        </tr>
    </table>
    
    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
        AutoDataBind="True" GroupTreeImagesFolderUrl="" Height="50px"  ToolbarImagesFolderUrl="" 
        ToolPanelView="None" ToolPanelWidth="200px" Width="350px" />
</asp:Content>


