<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="ContractsByStatus.aspx.vb" Inherits="Reports_Contracts_ContractsByStatus" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<%@ Register src="../../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div>

    <table>
        <tr>
            <td>Status:</td>
            <td>
                <uc2:Select_Item ID="siStatus" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="3"><asp:Button ID="btnRun" runat="server" Text="Run Report" /></td>
        </tr>
    </table>

        <asp:HiddenField ID="hfShowReport" Value = "0" runat="server" />

    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />
</div>
</asp:Content>

