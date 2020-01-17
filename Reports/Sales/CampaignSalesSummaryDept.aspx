<%@ Page Title="Campaign Sales Summary by Department" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="CampaignSalesSummaryDept.aspx.vb" Inherits="Reports_Sales_CampaignSalesSummaryDept" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="ucDatePicker" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div>
<table style="border-collapse:collapse">
<tr>
    <td>Please Select A Campaign </td>
    <td><asp:DropDownList runat="server" ID="ddCampaign" Width="160px" /></td>
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
    <td></td>
    <td>
        <div><asp:CheckBox runat="server" ID="cbIncludeOW" Text="Include OW" /></div>
    </td>
</tr>
</table>
</div>


<br /><br />
<div>
    <div><asp:Button runat="server" ID="btRunReport" Text="Run Report" />
    <span><asp:Button runat="server" ID="btPrintable" Text="Printable Version" /></span>
    </div>
    
</div>

<br />
<div>
    <CR:CrystalReportViewer ID="CRViewer" runat="server" AutoDataBind="true" ToolPanelView = "None" />
    <asp:HiddenField ID="hfShowReport" runat="server" Value="0" />
</div>
</asp:Content>

