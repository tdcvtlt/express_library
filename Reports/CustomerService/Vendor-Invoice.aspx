﻿<%@ Page Title="Vendor Invoice" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="Vendor-Invoice.aspx.vb" Inherits="Reports_CustomerService_Vendor_Invoice" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
            <tr>
            <td>Start Date:</td>
            <td>
                <uc1:DateField ID="dteSDate" runat="server" />
                </td>
        </tr>
        <tr>
            <td>End Date:</td>
            <td>
                <uc1:DateField ID="dteEDate" runat="server" />
            </td>
        </tr>
        <tr>
            <td>Vendor:</td>
            <td>
                <asp:DropDownList ID="ddVendors" runat="server"></asp:DropDownList></td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:RadioButtonList ID="rblResort" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem>KCP</asp:ListItem>
                    <asp:ListItem Selected="True">OutSide</asp:ListItem>
                </asp:RadioButtonList></td>
        </tr>
        <tr>
            <td colspan = "2"><asp:Button ID="Button1" runat="server" Text="Run Report" onclick="Unnamed1_Click"></asp:Button></td>
        </tr>
    </table>
      <asp:HiddenField ID="hfShowReport" Value = "0" runat="server" />
    <cr:crystalreportviewer ID="CrystalReportViewer1" runat="server" 
        AutoDataBind="true" ToolPanelView="None" />
    
</asp:Content>

