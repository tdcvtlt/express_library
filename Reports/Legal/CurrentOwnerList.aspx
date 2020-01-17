<%@ Page Title="Current Owners List" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="CurrentOwnerList.aspx.vb" Inherits="Reports_Legal_CurrentOwnerList" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div>

        <asp:HiddenField ID="hfShowReport" Value = "0" runat="server" />

    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
            AutoDataBind="true" ToolPanelView="None" />
</div>
</asp:Content>

