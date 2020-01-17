<%@ Page Title="Escrow Balance Analysis" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="Escrowdeposits.aspx.vb" Inherits="Reports_Accounting_Escrowdeposits" %>

<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>Account:</td>
            <td>
                <asp:DropDownList ID="ddAccount" runat="server"></asp:DropDownList></td>
        </tr>
        <tr>
            <td>Cut Off Date:</td>
            <td>
                <uc1:DateField ID="dfEdate" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btnRun" runat="server" Text="Run Report" />
            </td>
        </tr>
    </table>


  <asp:HiddenField ID="hfShowReport" Value = "0" runat="server" />
    
    
    
    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
        AutoDataBind="true" ToolPanelView="None" />
    
    
    
</asp:Content>

