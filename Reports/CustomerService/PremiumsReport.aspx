<%@ Page Title="Premiums Report" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="PremiumsReport.aspx.vb" Inherits="Reports_CustomerService_PremiumsReport" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
            <tr>
            <td>Start Date:</td>
            <td colspan="2" align="left">
                <uc1:DateField ID="dteSDate" runat="server" />
                </td>
        </tr>
        <tr>
            <td>End Date:</td>
            <td colspan="2" align="left">
                <uc1:DateField ID="dteEDate" runat="server" />
            </td>
        </tr>
        </table><table>
            <tr><td colspan="3" align="left">Premiums</td></tr>
        <tr>
            <td>
                <asp:ListBox ID="Prems" runat="server"></asp:ListBox>
            </td>
            <td>
                <asp:Button ID="btnAdd" runat="server" Text=">" />
                <asp:Button ID="btnAddAll" runat="server" Text=">>" /><br />
                <asp:Button ID="btnRemove" runat="server" Text="<" />
                <asp:Button ID="btnRemoveAll" runat="server" Text="<<" />
            </td>
            <td>
                <asp:ListBox ID="PremsSelected" runat ="server"></asp:ListBox>
            </td>
        </tr>
            <tr><td colspan="3" align="left">Campaigns</td></tr>
        <tr>
            <td>
                <asp:ListBox ID="Camps" runat="server"></asp:ListBox>
            </td>
            <td>
                <asp:Button ID="btnAdd1" runat="server" Text=">" />
                <asp:Button ID="btnAddAll1" runat="server" Text=">>" /><br />
                <asp:Button ID="btnRemove1" runat="server" Text="<" />
                <asp:Button ID="btnRemoveAll1" runat="server" Text="<<" />
            </td>
            <td>
                <asp:ListBox ID="CampsSelected" runat ="server"></asp:ListBox>
            </td>
        </tr>
        <tr>
            <td colspan = "2"><asp:Button ID="Button1" runat="server" Text="Run Report" onclick="Unnamed1_Click"></asp:Button></td>
        </tr>
    </table>
      <asp:HiddenField ID="hfShowReport" Value = "0" runat="server" />
    <cr:crystalreportviewer ID="CrystalReportViewer1" runat="server" 
        AutoDataBind="true" ToolPanelView="None" />
    
</asp:Content>

