<%@ Page Title="Meridian Trial Balance" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="trialbalance-meridian.aspx.vb" Inherits="Reports_Accounting_trialbalance_meridian" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>
<%@ Register Src="~/controls/DateField.ascx" TagPrefix="uc1" TagName="DateField" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>            
        <tr>
            <td>End Date:</td>
            <td>
                <uc1:DateField ID="dteEDate" runat="server" />
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td><asp:Button ID="submit" runat="server" Text="Run Report"></asp:Button></td>
        </tr>
    </table>


<br />

<CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
        AutoDataBind="True" GroupTreeImagesFolderUrl="" Height="50px"  ToolbarImagesFolderUrl="" 
        ToolPanelView="None" ToolPanelWidth="200px" Width="350px" />
</asp:Content>

