﻿<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="OutstandingPreauths.aspx.vb" Inherits="Reports_Vendors_OutstandingPreauths" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>Start Date:</td>
            <td>
                <uc1:datefield ID="sd" runat="server" />
            </td>
            <td rowspan="5" valign="top">
                
            </td>
        </tr>
        <tr>
            <td>End Date:</td>
            <td>
                <uc1:datefield ID="ed" runat="server" />
            </td>
            <td />
        </tr>
        <tr>
            <td>
            <asp:Button ID="btnRun" runat="server" Text="Run Report" />
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hfShowReport" Value = "0" runat="server" />
    
    
    
    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
        AutoDataBind="true" ToolPanelView="None" />
</asp:Content>

