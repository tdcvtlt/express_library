<%@ Page Title="Upgrade/Extra Nights Report" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="UpgradesExtraNights.aspx.vb" Inherits="Reports_Rentals_PackageAgent" %>

<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>Start Date:</td>
            <td><uc1:DateField ID="dfStartDate" runat="server" /></td>
        </tr>
        <tr>
            <td>End Date:</td>
            <td><uc1:DateField ID="dfEndDate" runat="server" /></td>
        </tr>
        <tr>
            <td colspan="1"><asp:Button ID="btnRunReport" runat="server" Text="Run Report" /></td>
            <td>&nbsp;</td>
        </tr>
    </table>
  <asp:HiddenField ID="hfShowReport" Value = "0" runat="server" />
    
    
    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
        AutoDataBind="true" ToolPanelView="None"/>
   
</asp:Content>