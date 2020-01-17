<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="SalesReport.aspx.vb" Inherits="Reports_Sales_SalesReport" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="ucDatePicker" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div style="font-family:@DFKai-SB">
<table style="border-collapse:collapse">
<tr>
    <td>Start Date</td>
    <td><ucDatePicker:DateField runat="server" ID="ucStartDate" /></td>
</tr>
<tr>
    <td>End Date</td>
    <td><ucDatePicker:DateField runat="server" ID="ucEndDate" /></td>
</tr>

<tr>
    <td colspan="2"><asp:Button runat="server" ID="btRunReport" Text="Run Report" /></td>    
</tr>
</table>


</div>


<div>
    <CR:CrystalReportViewer ID="CRViewer" runat="server" ToolPanelView="None" AutoDataBind="true" />
</div>

<
</asp:Content>

