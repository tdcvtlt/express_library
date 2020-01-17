<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="BacktoBackRes.aspx.vb" Inherits="Reports_FrontDesk_BacktoBackRes" %>
<%@ Register    assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" 
                namespace="CrystalDecisions.Web" tagprefix="CR" %>

<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="dtc" %>
<%@ Register src="../../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table style="border-collapse:collapse;">
<tr>
    <td>Start Date:</td>
    <td><dtc:DateField ID="SDATE" runat="server" /></td>
</tr>
<tr>
    <td>End Date:</td>
    <td><dtc:DateField ID="EDATE" runat="server" /></td>
</tr>
<tr>
    <td><asp:Button ID="RunReport" Text="Run Report" runat="server" /></td>
    <td>&nbsp;</td>
</tr>
</table>
<br />
<div id="crystalreportId">
    <CR:CrystalReportViewer ID="CrystalViewer" runat="server"   />
</div>

</asp:Content>

