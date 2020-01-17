<%@ Page Title="Inventory" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="Inventory.aspx.vb" Inherits="Reports_CustomerService_Inventory" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
            <tr>
            <td>Usage Year:</td>
            <td>
                <asp:DropDownList ID="ddYear" runat="server"></asp:DropDownList> 
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



