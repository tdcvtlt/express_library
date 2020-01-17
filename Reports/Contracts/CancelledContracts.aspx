<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="CancelledContracts.aspx.vb" Inherits="Reports_Contracts_CancelledContracts" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div id="container">

<div id="criteria">
<table style="border-collapse:collapse;">
<tr>
    <td>Start Date:</td>
    <td><asp:TextBox ID="SDATE" runat="server" Font-Names="Cambria"></asp:TextBox></td>
</tr>
<tr>
    <td>End Date:</td>
    <td><asp:TextBox ID="EDATE" runat="server" Font-Names="Cambria"></asp:TextBox></td>
</tr>
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

