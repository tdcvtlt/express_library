<%@ Page Title="Daily Deposits" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="DailyDeposits2.aspx.vb" Inherits="Reports_Accounting_DailyDeposits" %>

<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>Date:</td>
            <td><uc1:DateField ID="dfTransDate" runat="server" /></td>
        </tr>
        <tr>
            <td>Account:</td>
            <td><asp:DropDownList ID="ddMA" runat="server" AutoPostBack="true"></asp:DropDownList></td>
        </tr>
        <tr>
            <td>Trans Code:</td>
            <td><asp:ListBox ID="lbTC" runat="server" Rows="5"></asp:ListBox></td>
            <td>
                <asp:Button ID="addTC" Text=">>" runat="server" /><br />
                <asp:Button ID="remTC" Text="<<" runat="server" />
            </td>
            <td><asp:ListBox ID="lbSelectedTC" runat="server" Rows="5"></asp:ListBox></td>
        </tr>
        <tr>
            <td>User:</td>
            <td><asp:ListBox ID="lbUser" runat="server" Rows="5"></asp:ListBox></td>
            <td>
                <asp:Button ID="addUser" Text=">>" runat="server" /><br />
                <asp:Button ID="remUser" Text ="<<" runat="server" />
            </td>
            <td><asp:ListBox ID="lbSelectedUser" runat="server" Rows="5"></asp:ListBox></td>
        </tr>
        <tr>
            <td colspan="2"><asp:Button ID="btnRunReport" runat="server" Text="Run Report" /></td>
        </tr>
    </table>
  <asp:HiddenField ID="hfShowReport" Value = "0" runat="server" />
    
    
    
    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
        AutoDataBind="true" ToolPanelView="None" />
    
    
    
</asp:Content>

