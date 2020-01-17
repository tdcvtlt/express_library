<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="OPCSolicitorSalesSummary.aspx.vb" Inherits="Reports_Sales_OPCSolicitorSalesSummary" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="ucDatePicker" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


<div>
<table style="border-collapse:collapse">
<tr>
    <td>Please Select A Solicitor</td>
    <td><asp:DropDownList runat="server" ID="ddSolicitor" Width="160px" /></td>
</tr>
<tr>
    <td>Please Select A Location</td>
    <td><asp:DropDownList runat="server" ID="ddLocation" Width="160px" /></td>
</tr>
<tr>
    <td>Please Select A Vendor</td>
    <td><asp:DropDownList runat="server" ID="ddVendor" Width="160px" /></td>
</tr>
<tr>
    <td style="">Start Date</td>
    <td style=""><ucDatePicker:DateField runat="server" ID="dpFrom" /></td>
</tr>
<tr>
    <td>End Date</td>
    <td><ucDatePicker:DateField runat="server" ID="dpTo" /></td>
</tr>
<tr>
    <td colspan="2">
        <asp:RadioButtonList runat="server" ID="rdExclusive" RepeatDirection="Horizontal">
            <asp:ListItem Text="MAL/MAL-TS" Value="1"></asp:ListItem>
            <asp:ListItem Text="MAL" Value="2"></asp:ListItem>
            <asp:ListItem Text="MAL-TS" Value="3"></asp:ListItem>
        </asp:RadioButtonList>
    </td>
</tr>
<tr>
    <td colspan="2">
        <asp:Button ID="btRunReport" runat="server" Text="Run Report" />
        <asp:Button ID="btPrint" runat="server" Text="Printable Version" />
    </td>    
</tr>
</table>
</div>


<br /><br />



<div>
    <CR:CrystalReportViewer ID="CRViewer" runat="server" ToolPanelView="None" AutoDataBind="true" />
</div>
</asp:Content>

