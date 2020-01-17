<%@ Page Title="Closing Officer Report" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="ClosingOfficerReport.aspx.vb" Inherits="Reports_Sales_ClosingOfficerReport" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="ucDatePicker" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>




<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div>
<table style="border-collapse:collapse;">
<tr>
    <td>Start Date</td>
    <td colspan="2"><ucDatePicker:DateField runat="server" ID="ucDateStart" /></td>
</tr>
<tr>
    <td>End Date</td>
    <td colspan="2"><ucDatePicker:DateField runat="server" ID="ucDateEnd" /></td>
</tr>
<tr>
    <td>Personnel</td>
    <td><asp:DropDownList ID="ddPersonnelList" runat="server" /></td>
    <td><asp:Button runat="server" ID="btAddPersonnel" Text="Add" /></td>
</tr>
<tr>
    <td>Selected Personnel</td>
    <td><asp:ListBox Rows="6" runat="server" ID="lbSelectedPersonnel" Width="200px" /></td>
    <td><asp:Button runat="server" ID="btRemovePersonel" Text="Remove" /></td>
</tr>
<tr>
    <td colspan="1">&nbsp;</td>
    <td colspan="2"><asp:CheckBox ID="cbShowDetail" runat="server" Text="Show Detail" /></td>

</tr>
</table>

<div>
<asp:Button ID="btRunReport" runat="server" Text="Run Report" />
<asp:Button ID="btPrintable" runat="server" Text="Printable Version" />
</div>


</div>


<div id="ReportSection">
    <asp:Literal ID="Lit1" runat="server"></asp:Literal>
</div>

</asp:Content>

