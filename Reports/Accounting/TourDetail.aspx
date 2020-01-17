<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="TourDetail.aspx.vb" Inherits="Reports_Accounting_TourDetail" %>

<%@ Register    assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" 
                namespace="CrystalDecisions.Web" tagprefix="CR" %>

<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="dtc" %>
<%@ Register src="../../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div id="container">

<div id="criteria">
<table style="border-collapse:collapse;">
<tr>
    <td>Start Date:</td>
    <td><dtc:DateField ID="SDATE" runat="server" /></td>
</tr>
<tr>
    <td>End Date:</td>
    <td><dtc:DateField ID="EDATE" runat="server" /></td>
</tr>
<%--<tr>
    <td>Tour Location</td>
    <td><uc2:Select_Item ID="siTourLocation" runat="server" /> </td>
</tr>--%>
<tr>
    <td><asp:Button ID="RunReport" Text="Run Report" runat="server" /></td>
    <td>&nbsp;</td>
</tr>
</table>


</div>

<br />
<br />
<br />




<div id="crystalreportId">
    <CR:CrystalReportViewer ID="CrystalViewer" runat="server"   />
</div>

</div>




</asp:Content>
