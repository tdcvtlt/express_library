<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="CEPastDuePayments.aspx.vb" Inherits="Reports_OwnerServices_CEPastDuePayments" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div>

    <table>
        <tr>
            <td colspan="3"><asp:Button ID="Button3" runat="server" Text="Run Report" /></td>
        </tr>
    </table>

        <asp:HiddenField ID="hfShowReport" Value = "0" runat="server" />






<CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" 
            ToolPanelView="None" />



        



</div>


</asp:Content>

