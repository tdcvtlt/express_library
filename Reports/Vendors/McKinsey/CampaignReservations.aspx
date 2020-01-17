<%@ Page Title="Campaign Reservations" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="CampaignReservations.aspx.vb" Inherits="Reports_Vendors_McKinsey_CampaignReservations" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>


<%@ Register src="../../../controls/DateField.ascx" tagname="DateField" tagprefix="ucDatePicker" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div>
<table style="border-collapse:collapse;">

<tr>
    <td>Start Date</td>
    <td><ucDatePicker:DateField ID="dteSDate" runat="server" /></td>
</tr>
<tr>
    <td>End Date</td>
    <td><ucDatePicker:DateField ID="dteEDate" runat="server" /></td>
</tr>
</table>
<div>
<asp:Button ID="btnRun" runat="server" Text="Run Report" />
</div>
    <asp:HiddenField ID="hfShowReport" Value = "0" runat="server" />

<div id="ReportViewer">
       <cr:crystalreportviewer ID="CrystalReportViewer1" runat="server" 
        AutoDataBind="true" ToolPanelView="None" />
</div>


</div>
</asp:Content>

