<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" 
    CodeFile="PackageIssuedCancelSave.aspx.vb" Inherits="Reports_Rentals_PackageIssuedCancelSave" %>

<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.3500.0, 
    Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div id="wrapper">

<table>
<tr>
    <td><strong>Start Date</strong></td>
    <td><uc1:DateField ID="dfStartDate" runat="server" /></td>
</tr>
<tr>
    <td><strong>End Date:</strong></td>
    <td><uc1:DateField ID="dfEndDate" runat="server" /></td>
</tr>
<tr>
    <td></td>
    <td><asp:Button ID="btnRunReport" runat="server" Text="Run Report" Height="30" Width="150" /></td>
</tr>

</table>
<br /><br />
<CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
        AutoDataBind="true" ToolPanelView="None"/>
</div>

</asp:Content>

