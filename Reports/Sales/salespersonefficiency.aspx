<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="salespersonefficiency.aspx.vb" Inherits="Reports_Sales_salespersonefficiency" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="ucDatePicker" %>
<%@ Register src="../../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div>
<table style="border-collapse:collapse">
<tr>
    <td><strong>Tour Location</strong></td>    
    <td style="width:50px;">&nbsp</td>
    <td><strong>Start Date</strong></td>    
</tr>
<tr>    
    <td><uc2:Select_Item ID="siTourLocation" runat="server" /></td>
    <td />
    <td><ucDatePicker:DateField runat="server" ID="sd" /></td>
</tr>
<tr>
    <td><strong>Title</strong></td>    
    <td></td>
    <td><strong>End Date</strong></td>    
</tr>

<tr>    
    <td><uc2:Select_Item ID="siTitle" runat="server" style="width:100%" /></td>
    <td />
    <td><ucDatePicker:DateField runat="server" ID="ed" /></td>
</tr>
<tr>
    <td colspan="2" />
    <td style="height:40px;"><asp:Button runat="server" ID="btRun" Text="Run Report" /></td>
</tr>
</table>
</div>
<div>
    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />
</div>



</asp:Content>

