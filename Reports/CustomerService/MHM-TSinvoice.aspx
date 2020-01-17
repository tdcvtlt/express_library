<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="MHM-TSinvoice.aspx.vb" Inherits="Reports_CustomerService_MHM_TSinvoice" %>
<%@ Register Src="~/controls/DateField.ascx" TagName="DateField" TagPrefix="uc1" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
            <tr>
            <td>Start Date:</td>
            <td>
                <uc1:DateField ID="dteSDate" runat="server" />
                </td>
        </tr>
        <tr>
            <td>End Date:</td>
            <td>
                <uc1:DateField ID="dteEDate" runat="server" />
            </td>
        </tr>
        <tr>
            <td />
            <td><asp:Button ID="btReportRun" runat="server" Text="Run Report"></asp:Button></td>
        </tr>
    </table>
    <br />
    <asp:Label ID="lbErr" runat="server"></asp:Label>
    <br />  
    <cr:crystalreportviewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" ToolPanelView="None" />
</asp:Content>

