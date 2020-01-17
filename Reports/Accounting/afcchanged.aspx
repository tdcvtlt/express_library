<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="afcchanged.aspx.vb" Inherits="Reports_Contracts_afcchanged" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div id="main">
<h2>REPORT - AFC STATUS</h2>
<table style="border-collapse:collapse;">
<tr>
    <td><strong>Start Date:</strong></td>
    <td><uc1:DateField ID="sdate" runat="server" /></td>
</tr>
<tr>
    <td><strong>End Date:</strong></td>
    <td><uc1:DateField ID="edate" runat="server" /></td>
</tr>
<tr>
    <td>&nbsp;</td>
    <td valign="bottom" style="height:50px;display:block;"><asp:Button ID="RunReport" Text="Run Report" runat="server"  Width="120" Height="35" /></td>    
</tr>
</table>
</div>

<br /><br /><br />
<div id="crystalreportId">
    <CR:CrystalReportViewer ID="crystalViewer1" runat="server" AutoDataBind="true"   />
</div>

</asp:Content>

