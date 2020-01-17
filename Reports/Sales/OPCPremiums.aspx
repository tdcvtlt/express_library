<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="OPCPremiums.aspx.vb" Inherits="Reports_Sales_OPCPremiums" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="ucDatePicker" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>







<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div>
<table style="border-collapse:collapse">
<tr>
    <td style="width:115px;">Start Date</td>
    <td style="width:250px;"><div style="margin-left:50px;"><ucDatePicker:DateField runat="server" ID="dpFrom" /></div></td>
</tr>
<tr>
    <td>End Date</td>
    <td><div style="margin-left:50px"><ucDatePicker:DateField runat="server" ID="dpTo" /></div></td>
</tr>
</table>
<br /><br />
<div>
    <div style="width:300px;">
    <asp:Button runat="server" ID="btRunReport" Text="Run Report" />
    </div>
    
</div>

<br />
<div>
    <CR:CrystalReportViewer ID="CRViewer" runat="server" AutoDataBind="true" ToolPanelView="None" />
</div>
</div>

</asp:Content>

