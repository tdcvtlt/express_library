<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="VCInvoice.aspx.vb" Inherits="Reports_CustomerService_VCInvoice" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="ucDatePicker" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div id="container">
<table>
<tr>
    <td>Start Date</td>
    <td><ucDatePicker:DateField runat="server" ID="SDATE" Selected_Date="" /></td>
</tr>
<tr>
    <td>End Date</td>
    <td><ucDatePicker:DateField runat="server" ID="EDATE" Selected_Date="" /></td>
</tr>
</table>
<div>
<asp:Button ID="RunReport" runat="server" Text="Run Report" />
<asp:Button ID="Printable" runat="server" Text="Printable Version" />
</div>

<br /><br />
<asp:Literal ID="LIT" runat="server"></asp:Literal>
</div>
</asp:Content>

