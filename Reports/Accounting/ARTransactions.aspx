<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="ARTransactions.aspx.vb" Inherits="Reports_Accounting_ARTransactions" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<%@ Register src="../../controls/username.ascx" tagname="username" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div>

    <table>
        <tr>
            <td>Transaction Start Date:</td>
            <td>
                <uc1:DateField ID="sdate" runat="server" />
            </td>
        </tr>
        <tr>
            <td>Transaction End Date:</td>
            <td>
                <uc1:DateField ID="edate" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="3"><asp:Button ID="btnRun" runat="server" Text="Run Report" /></td>
        </tr>
    </table>

        <asp:HiddenField ID="hfShowReport" Value = "0" runat="server" />

    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" GroupTreeStyle-ShowLines="false" ToolPanelView="None" />
</div>
</asp:Content>

