<%@ Page Title="Occupancy By Allocation" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="OccupancyByAllocation.aspx.vb" Inherits="Reports_FrontDesk_OccupancyByAllocation" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="ucDatePicker" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div>
<table style="border-collapse:collapse;">
<tr>
    <td>Start Date</td>
    <td><ucDatePicker:DateField ID="SDATE" runat="server" /></td>
</tr>
<tr>
    <td>End Date</td>
    <td><ucDatePicker:DateField ID="EDATE" runat="server" /></td>
</tr>
</table>
<div>
<asp:Button runat="server" ID="RunReport" Text="Run Report" />

</div>


<div>
    <CR:CrystalReportViewer ID="CrystalViewer" runat="server" AutoDataBind="true" />
</div>
</div>
</asp:Content>

