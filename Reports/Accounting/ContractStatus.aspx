<%@ Page Title="Contract Status Report" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="ContractStatus.aspx.vb" Inherits="Reports_Accounting_ContractStatus" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr><td colspan="3">Status:</td><td colspan = "3">Sub-Status:</td></tr>
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
            <td>
                <asp:ListBox ID="lbSubStatus" runat="server" SelectionMode="Multiple"></asp:ListBox>
            </td>
            <td>
                <asp:Button ID="Button3" runat="server" Text="<<" /><br />
                <asp:Button ID="Button4" runat="server" Text=">>" /><br />
                <asp:Button ID="Button5" runat="server" Text = "All >>" /><br />
                <asp:Button ID="Button6" runat="server" Text = "All <<" /><br />
            </td>
            <td>
                <asp:ListBox ID="lbSubStatus2" runat="server" SelectionMode="Multiple"></asp:ListBox>
            </td>
        </tr>
        <tr><td colspan="3">MF Status:</td><td colspan = "3">Mortgage Status:</td></tr>
        <tr>
            <td>
                <asp:ListBox ID="lbmfStatus" runat="server" SelectionMode="Multiple"></asp:ListBox>
            </td>
            <td>
                <asp:Button ID="Button7" runat="server" Text="<<" /><br />
                <asp:Button ID="Button8" runat="server" Text=">>" /><br />
                <asp:Button ID="Button9" runat="server" Text = "All >>" /><br />
                <asp:Button ID="Button10" runat="server" Text = "All <<" /><br />
            </td>
            <td>
                <asp:ListBox ID="lbmfStatus2" runat="server" SelectionMode="Multiple"></asp:ListBox>
            </td>
            <td>
                <asp:ListBox ID="lbmortStatus" runat="server" SelectionMode="Multiple"></asp:ListBox>
            </td>
            <td>
                <asp:Button ID="Button11" runat="server" Text="<<" /><br />
                <asp:Button ID="Button12" runat="server" Text=">>" /><br />
                <asp:Button ID="Button13" runat="server" Text = "All >>" /><br />
                <asp:Button ID="Button14" runat="server" Text = "All <<" /><br />
            </td>
            <td>
                <asp:ListBox ID="lbmortStatus2" runat="server" SelectionMode="Multiple"></asp:ListBox>
            </td>
        </tr>
        <tr><td colspan="3">Conversion Status:</td><td colspan = "3">&nbsp;</td></tr>
        <tr>
            <td>
                <asp:ListBox ID="lbConvStatus" runat="server" SelectionMode="Multiple"></asp:ListBox>
            </td>
            <td>
                <asp:Button ID="Button15" runat="server" Text="<<" /><br />
                <asp:Button ID="Button16" runat="server" Text=">>" /><br />
                <asp:Button ID="Button17" runat="server" Text = "All >>" /><br />
                <asp:Button ID="Button18" runat="server" Text = "All <<" /><br />
            </td>
            <td>
                <asp:ListBox ID="lbConvStatus2" runat="server" SelectionMode="Multiple"></asp:ListBox>
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
    <asp:Button ID="btnRun" runat="server" Text="Run Report" />
    <asp:HiddenField ID="hfShowReport" Value = "0" runat="server" />
    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
        AutoDataBind="True" GroupTreeImagesFolderUrl="" Height="50px"  ToolbarImagesFolderUrl="" 
        ToolPanelView="None" ToolPanelWidth="200px" Width="350px" />
</asp:Content>

