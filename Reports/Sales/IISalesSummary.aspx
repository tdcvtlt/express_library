﻿<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="IISalesSummary.aspx.vb" Inherits="Reports_Sales_IISalesSummary" %>

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
<tr>
    <td></td>
    <td>
        <div style="float:left;margin-left:50px;"><asp:CheckBox runat="server" ID="cbIncludeOW" Text="Include OW" /></div>
    </td>
</tr>
</table>
<br /><br />
<div>
    <div style="width:300px;"><asp:Button runat="server" ID="btRunReport" Text="Run Report" />
    <span style="margin-left:20px"><asp:Button runat="server" ID="btPrintable" Text="Printable Version" /></span>
    </div>
    
</div>

<br />
<div>
    <CR:CrystalReportViewer ID="CRViewer" runat="server" AutoDataBind="true" ToolPanelView="None" />
</div>
</div>

</asp:Content>

