﻿<%@ Page Title="Equiant Exceptions" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="AFCException.aspx.vb" Inherits="Reports_MIS_AFCException" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Button ID="btnRun" runat="server" Text="Run Report" />
    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
        AutoDataBind="True" GroupTreeImagesFolderUrl="" Height="50px"  ToolbarImagesFolderUrl="" 
        ToolPanelView="None" ToolPanelWidth="200px" Width="350px" />
        <asp:HiddenField ID="hfShowReport" Value = "1" runat="server" />
</asp:Content>

