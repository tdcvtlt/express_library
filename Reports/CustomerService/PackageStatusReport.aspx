<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="PackageStatusReport.aspx.vb" Inherits="Reports_CustomerService_PackageStatusReport" %>
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
        <tr><td colspan="3">Status:</td></tr>
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
            <td>
                <asp:ListBox ID="ListBox2" runat="server" SelectionMode="Multiple"></asp:ListBox>
            </td>
        </tr>
    </table>
        <asp:Button ID="btnRun" runat="server" Text="Run Report" />
    <asp:HiddenField ID="hfShowReport" Value = "0" runat="server" />
    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
        AutoDataBind="True" GroupTreeImagesFolderUrl="" Height="50px"  ToolbarImagesFolderUrl="" 
        ToolPanelView="None" ToolPanelWidth="200px" Width="350px" />
</asp:Content>

