<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="ContactCheckIn.aspx.vb" Inherits="Reports_FrontDesk_ContactCheckIn" %>

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
    <td><ucDatePicker:DateField ID="dfSDate" runat="server" /></td>
</tr>
<tr>
    <td>End Date</td>
    <td><ucDatePicker:DateField ID="dfEDate" runat="server" /></td>
</tr>
</table>
<div>
<asp:Button runat="server" ID="btnRun" Text="Run Report" />

</div>

      <asp:HiddenField ID="hfShowReport" Value = "0" runat="server" />
<div>
    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />
</div>
</div>
</asp:Content>

