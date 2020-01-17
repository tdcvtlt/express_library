<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="ToursByDateCreated.aspx.vb" Inherits="Reports_Tours_ToursByDateCreated" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="ucDatePicker" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div>
<table style="border-collapse:collapse;">
<tr>
    <td>Start Date</td>
    <td><ucDatePicker:DateField ID="ucDateStart" runat="server" /></td>
</tr>
<tr>
    <td>End Date</td>
    <td><ucDatePicker:DateField ID="ucDateEnd" runat="server" /></td>
</tr>
<tr>
    <td>Campaign</td>
    <td><asp:DropDownList ID="ddCampaign" runat="server"></asp:DropDownList></td>
</tr>
</table>
<div>
<asp:Button runat="server" ID="btRunReport" Text="Run Report" />
<asp:Button ID="btPrintable" runat="server" Text="Printable Version" />
<asp:Button ID="btToExcel" runat="server" Text="Export To Excel" />
</div>


<div>
<asp:Literal ID="LIT" runat="server"></asp:Literal>
</div>
</div>
</asp:Content>

