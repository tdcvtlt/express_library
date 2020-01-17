<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="ContractsByStatus.aspx.vb" Inherits="Reports_Sales_ContractsByStatus" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>






<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div>
<table style="border-collapse:collapse;">
<tr>
<td>Status Choices</td>
<td><asp:DropDownList ID="ddStatuses" runat="server" Width="120px"></asp:DropDownList></td>
<td>&nbsp;</td>
<td>Selected</td>
<td><asp:ListBox Rows="5" runat="server" ID="lbStatuses"></asp:ListBox></td>
</tr>
<tr>
<td colspan="5"><asp:Button runat="server" ID="btAddTo" Text=" > " Width="50px" />&nbsp;&nbsp;
    <asp:Button runat="server" ID="btRemoveFrom" Text=" < " Width="50px" />      
</td>
</tr>
<tr>
<td><asp:Button runat="server" ID="btRunReport" Text="Run Report" Width="80px" /> </td>
</tr>
</table>
</div>


<div>
    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />
</div>

</asp:Content>

