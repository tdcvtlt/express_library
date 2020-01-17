<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="CampaignSalesSummaryPerVendor.aspx.vb" Inherits="Reports_Sales_CampaignSalesSummaryPerVendor" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="ucDatePicker" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<table style="border-collapse:collapse">
<tr>
    <td>Select A Campaign </td>
    <td><asp:DropDownList runat="server" ID="ddCampaign" Width="160px" /></td>
</tr>
<tr>
    <td style="">Start Date</td>
    <td style=""><ucDatePicker:DateField runat="server" ID="sd" /></td>
</tr>
<tr>
    <td>End Date</td>
    <td><ucDatePicker:DateField runat="server" ID="ed" /></td>
</tr>
<tr>
    <td></td>
    <td>
        <div><asp:CheckBox runat="server" ID="cbOW" Text="Include OW" /></div>
    </td>
</tr>
<tr>
    <td colspan="2">&nbsp;</td>
</tr>
<tr>
    <td>&nbsp;</td>
    <td>
        <asp:Button runat="server" ID="btRunReport" Text="Run Report" />
        <asp:Button runat="server" ID="btPrintable" Text="Printable Version" />
    </td>
</tr>
<tr>
    <td colspan="2">&nbsp;</td>
</tr>
<tr>
    <td>&nbsp;</td>
    <td><asp:Label runat="server" ID="Err"></asp:Label></td>
</tr>
</table>


<br />
<div>
    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" ToolPanelView = "None" />
    <asp:HiddenField ID="hfShowReport" runat="server" Value="0" />
</div>
</asp:Content>

