<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="TourDetail.aspx.vb" Inherits="Reports_Tours_TourDetail" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="ucDatePicker" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


<div>
<table >
<tr>
    <td>Tour Location</td>
    <td><asp:DropDownList ID="ddLocation" runat="server"></asp:DropDownList></td>
</tr>
<tr>
    <td>Start Date</td>
    <td><ucDatePicker:DateField runat="server" ID="ucDateStart" /></td>
</tr>
<tr>
    <td>End Date</td>
    <td><ucDatePicker:DateField runat="server" ID="ucDateEnd" /></td>
</tr>
</table>
<div>
<asp:Button ID="btRunReport" runat="server" Text="Run Report" />
<asp:Button ID="btPrintable" runat="server" Text="Printable Version" />
</div>
</div>

<div>
<asp:Literal ID="LIT" runat="server"></asp:Literal>
</div>
</asp:Content>

