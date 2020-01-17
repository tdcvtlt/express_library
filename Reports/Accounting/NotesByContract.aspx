<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="NotesByContract.aspx.vb" Inherits="Reports_Accounting_NotesByContract" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    Enter Contract Number: 
    <asp:TextBox ID="txtKCP" runat="server"></asp:TextBox><br />
    <asp:Button ID="btnRun" runat="server" Text="Run Report" />
    <asp:HiddenField ID="hfShowReport" Value = "0" runat="server" />
    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
        AutoDataBind="true" EnableParameterPrompt="False" ToolPanelView="None" />
</asp:Content>

