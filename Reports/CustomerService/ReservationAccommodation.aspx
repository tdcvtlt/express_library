<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="ReservationAccommodation.aspx.vb" Inherits="Reports_CustomerService_ReservationAccommodation" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div id="MainContent">
<table border="0">
<tr>
    <td>Reservation Location:</td>
    <td><asp:DropDownList runat="server" ID="DDL_Location"></asp:DropDownList></td>
</tr>
<tr>
    <td>Hotel:</td>
    <td><asp:DropDownList runat="server" ID="DDL_Hotel"></asp:DropDownList></td>
</tr>
<tr>
    <td>Status:</td>
    <td><asp:DropDownList runat="server" ID="DDL_Status"></asp:DropDownList></td>
</tr>
<tr>
<td>Check In Date:</td>
<td><uc1:DateField ID="DF_CheckIn" runat="server" /></td>
</tr>
<tr>
<td>Check Out Date:</td>
<td><uc1:DateField ID="DF_CheckOut" runat="server" /></td>
</tr>
<tr>
<td>Date Booked From:</td>
<td><uc1:DateField ID="DF_BookedFrom" runat="server" /></td>
</tr>
<tr>
<td>Date Booked To:</td>
<td><uc1:DateField ID="DF_BookedTo" runat="server" /></td>
</tr>
<tr>
<td>&nbsp;</td>
<td><asp:CheckBox ID="CheckBoxIncludeTour" runat="server" Text="Include Tour?" /></td>
</tr>
<tr>
    <td colspan="2"><asp:Button ID="BTN_Run" runat="server" Text="Run Report" /></td>
</tr>

</table>
</div>

<div>
<asp:Label ID="STATUS_MESSAGE" runat="server"></asp:Label>
</div>


<br /><br />
<div id="reportsection" runat="server"></div>






<asp:HiddenField ID="hfShowReport" Value="0" runat="server" />
<div>
<CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" GroupTreeStyle-ShowLines="false" ToolPanelView="None" />
</div>
 

</asp:Content>

