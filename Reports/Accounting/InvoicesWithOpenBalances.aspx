<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="InvoicesWithOpenBalances.aspx.vb" Inherits="Reports_Accounting_InvoicesWithOpenBalances" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<table>
        <tr>
            <td>Cut-Off Date:</td>
            <td colspan="2">
                <uc1:DateField ID="sd" runat="server" />
            </td>
        </tr>        
        <tr>
            <td>Account</td>
            <td colspan="2"> 
                <asp:DropDownList runat="server" ID="ddlMerchantAccounts">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                &nbsp;
            </td>            
        </tr>
        <tr>
            <td></td>
            <td colspan="2"><asp:Button ID="btnRun" runat="server" Text="Run Report" /></td>
        </tr>
        <tr>
            <td colspan="3">
                &nbsp;
            </td>            
        </tr>
        <tr>
            <td></td>
            <td colspan="2"><asp:Label runat="server" ID="Err"></asp:Label></td>
        </tr>
    </table>

    <br />
    <cr:crystalreportviewer ID="CrystalReportViewer1" runat="server" 
        AutoDataBind="true" ToolPanelView="None" />
</asp:Content>

