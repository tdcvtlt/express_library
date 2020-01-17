<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="ToursByStatus.aspx.vb" Inherits="Reports_Tours_ToursByStatus" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register src="../../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc2" %>

<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="ucDatePicker" %>
<%@ Register src="../../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div>
<table style="border-collapse:collapse;">
<tr>
    <td>Tour Status:</td>
    <td>
        <uc1:Select_Item ID="siStatus" runat="server" />
    </td>
</tr>
<tr>
    <td>Tour Location</td>
    <td><uc2:Select_Item ID="siTourLocation" runat="server" /> </td>
</tr>
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

