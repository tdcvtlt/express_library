<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="SalesTeams.aspx.vb" Inherits="Reports_HR_SalesTeams" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

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
            <td colspan = "2">
                <asp:Button ID="Button1" runat="server" Text="Run Report" />
            </td>
        </tr>
    </table>
    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
        AutoDataBind="True" GroupTreeImagesFolderUrl="" Height="50px" 
        ToolbarImagesFolderUrl="" 
        ToolPanelView="None" ToolPanelWidth="200px" Width="350px" />
        <asp:HiddenField ID="hfShowReport" Value = "0" runat="server" />
</asp:Content>

