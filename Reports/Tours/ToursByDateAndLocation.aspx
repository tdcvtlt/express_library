<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="ToursByDateAndLocation.aspx.vb" Inherits="Reports_Tours_ToursByDateAndLocation" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>


<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="ucDatePicker" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div>
<table style="border-collapse:collapse;">
<tr>
    <td>Select A Location</td>
    <td><asp:DropDownList ID="Location" runat="server"></asp:DropDownList></td>
</tr>
<tr>
    <td>Start Date</td>
    <td><ucDatePicker:DateField ID="SDate" runat="server" /></td>
</tr>
<tr>
    <td>End Date</td>
    <td><ucDatePicker:DateField ID="EDate" runat="server" /></td>
</tr>
</table>
<div>
<asp:Button runat="server" ID="RunReport" Text="Run Report" />
</div>


<div id="ReportViewer">
    <CR:CrystalReportViewer ID="CrystalViewer" runat="server" AutoDataBind="true" />
</div>


</div>
</asp:Content>

