<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="PaymentsAppliedHistory.aspx.vb" Inherits="Reports_Accounting_PaymentsAppliedHistory" %>
<%@ Register    assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" 
                namespace="CrystalDecisions.Web" tagprefix="CR" %>

<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="dtc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="criteria">
<table style="border-collapse:collapse;">
<tr>
    <td colspan="2">&nbsp;</td>    
</tr>
<tr>
    <td>Start Date:</td>
    <td><dtc:DateField ID="sd" runat="server" /></td>
</tr>
<tr>
    <td>End Date:</td>
    <td><dtc:DateField ID="ed" runat="server" /></td>
</tr>
<tr>
    <td colspan="2">&nbsp;</td>
</tr>
<tr>
    <td></td>
    <td><asp:RadioButton runat="server" ID="cbIsContractID" GroupName="contract" Text="CONTRACT ID" />
        <asp:RadioButton runat="server" ID="cbIsKCP"  GroupName="contract" Text="KCP NO." />        
    </td>    
</tr>
<tr>
    <td>Contract</td>
    <td><asp:TextBox runat="server" ID="tbKCP" Width="100%"></asp:TextBox>
    </td>    
</tr>

<tr>
    <td>&nbsp;</td>
    <td><asp:Button ID="btnSubmit" Text="Run Report" runat="server" /></td>
</tr>
</table>

<br />
<asp:Label runat="server" ID="msg"></asp:Label>
</div>

<br />
<br />

<CR:CrystalReportViewer ID="CrystalViewer" runat="server"   />
</asp:Content>

