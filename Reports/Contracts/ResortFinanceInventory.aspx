<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="ResortFinanceInventory.aspx.vb" Inherits="Reports_Contracts_ResortFinanceInventory" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


<div id="ReportViewer">
<h2 style="margin:45px">Resort Finance Inventory</h2>
<br />
    <CR:CrystalReportViewer ID="CrystalViewer" runat="server" />
</div>

</asp:Content>

