<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="MarketingReservations.aspx.vb" Inherits="Reports_Marketing_MarketingReservations" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <table style="border-collapse:collapse;">
<tr>
    <td>Start Date:</td>
    <td>End Date:</td>
</tr>
<tr>
    <td><uc1:DateField runat="server" ID="ucStartDate" /></td>
    <td><uc1:DateField runat="server" ID="ucEndDate" /></td>
</tr>
<tr>
    <td>Reservation Type</td>
    <td><asp:DropDownList ID="ddReservationType" runat="server" Width="180">        
        </asp:DropDownList>
    </td>
</tr>
<tr>
    <td>Status:</td>
    <td>&nbsp;</td>    
</tr>
<tr>
    <td>
        <asp:DropDownList ID="ddStatus" runat="server" Width="180" /><br />   
        <asp:Button ID="btAdd" runat="server" Text="Add" Width="80" /><br />
        <asp:Button ID="btRemove" runat="server" Text="Remove" Width="80" /> <br />
        <asp:Button ID="btRemoveAll" runat="server" Text="Remove All" Width="80" />
    </td>
    <td><asp:ListBox ID="lbStatus" runat="server" Width="180"></asp:ListBox><br /></td>
</tr>
<tr>
    <td><asp:Button ID="btRun" Text="Run Report" runat="server" Width="80" /></td>
    <td><asp:Button ID="btExcel" runat="server" Text="Get Excel" Width="80" /></td>
</tr>
</table>

<br />
<div>
<CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />
</div>
    

</asp:Content>

