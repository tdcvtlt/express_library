<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="SalesByType.aspx.vb" Inherits="Reports_Sales_SalesByType" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="ucDatePicker" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div style="font-family:@DFKai-SB">
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
    <td>Unit Type</td>
    <td><asp:DropDownList runat="server" ID="ddUnit" Width="200px"></asp:DropDownList></td>
</tr>
<tr>
    <td><asp:Button runat="server" ID="btRunReport" Text="Run Report" /></td>
    <td><asp:Button runat="server" ID="btPrintable" Text="Printable Version" /></td>
</tr>
</table>
</div>

<div>
    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />
</div>


</script>
</asp:Content>

