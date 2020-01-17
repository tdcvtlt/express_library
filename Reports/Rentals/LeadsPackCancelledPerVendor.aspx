<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="LeadsPackCancelledPerVendor.aspx.vb" Inherits="Reports_Rentals_LeadsPackCancelledPerVendor" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">   
    <table>            
        <tr>
            <td>Purchased After:</td>
            <td>
                <uc1:DateField ID="dteEDate" runat="server" />
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td><asp:Button ID="submit" runat="server" Text="Run Report" style="height: 26px"></asp:Button></td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>            
        </tr>
        <tr>
            <td></td>
            <td><asp:Label runat="server" ID="Err"></asp:Label></td>
        </tr>
    </table>

    <br />
<cr:crystalreportviewer ID="CrystalReportViewer1" runat="server" 
        AutoDataBind="true" ToolPanelView="None" />
</asp:Content>

