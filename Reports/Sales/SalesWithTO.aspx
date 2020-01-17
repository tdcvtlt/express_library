<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="SalesWithTO.aspx.vb" Inherits="Reports_Sales_SalesWithTO" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="ucDatePicker" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table style="border-collapse:collapse">
<tr>
    <td>Contract Start Date</td>
    <td><ucDatePicker:DateField runat="server" ID="ucStartDate" /></td>
</tr>
<tr>
    <td>Contract End Date</td>
    <td><ucDatePicker:DateField runat="server" ID="ucEndDate" /></td>
</tr>
<tr>
    <td><asp:Button runat="server" ID="btRunReport" Text="Run Report" /></td>
</tr>
</table>
</div>
<div>
    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />
</div>

</asp:Content>

