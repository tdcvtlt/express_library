<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="GolfInvoice.aspx.vb" Inherits="Reports_FrontDesk_GolfInvoice" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>
<%@ Register Src="~/controls/DateField.ascx" TagPrefix="uc1" TagName="DateField" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>Start Date:</td>
        </tr>
        <tr>
            <td>
                <uc1:DateField runat="server" ID="sDate" />
            </td>
        </tr>
        <tr>
            <td>End Date:</td>
        </tr>
        <tr>
            <td>
                <uc1:DateField runat="server" ID="eDate" />
            </td>
        </tr>
        <tr><td colspan="3">Golf Course:</td></tr>
        <tr>
            <td>
                <asp:ListBox ID="ListBox1" runat="server" SelectionMode="Multiple"></asp:ListBox>
            </td>
            <td>
                <asp:Button ID="Button1" runat="server" Text="<<" /><br />
                <asp:Button ID="Button2" runat="server" Text=">>" /><br />
                <asp:Button ID="BtnAll" runat="server" Text = "All >>" /><br />
                <asp:Button ID="btnRemove" runat="server" Text = "All <<" /><br />
            </td>
            <td><asp:ListBox ID="ListBox2" runat="server" SelectionMode="Multiple"></asp:ListBox></td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnRun" runat="server" Text="Run Report" /></td>
        </tr>
    </table>
         <asp:HiddenField ID="hfShowReport" Value = "0" runat="server" />
    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
        AutoDataBind="True" GroupTreeImagesFolderUrl="" Height="50px"  ToolbarImagesFolderUrl="" 
        ToolPanelView="None" ToolPanelWidth="200px" Width="350px" />
</asp:Content>

