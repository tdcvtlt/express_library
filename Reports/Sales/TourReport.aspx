<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="TourReport.aspx.vb" Inherits="Reports_Sales_TourReport" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div style="width:70%">

<table style="border-collapse:collapse">
<tr>
    <td>Date:</td>
    <td><uc1:DateField runat="server" ID="dfDate" /></td>
</tr>
<tr>
    <td colspan="2"><asp:Button ID="btRunReport" runat="server" Text="Run Report" Width="70%" /></td>
</tr>    
</table>

</div>


<div>
    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true"  />
</div>
</asp:Content>


