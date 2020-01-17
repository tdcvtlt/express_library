<%@ Page Title="Deposit Summary" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="DepositSummary.aspx.vb" Inherits="Reports_Accounting_DepositSummary" %>

<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<%@ Register src="../../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>Start Date:</td>
            <td><uc1:DateField ID="dfTransDate" runat="server" /></td>
        </tr>
        <tr>
            <td>End Date:</td>
            <td><uc1:DateField ID="dfEndDate" runat="server" /></td>
        </tr>
        <tr>
            <td>Account:</td>
            <td><asp:DropDownList ID="ddMA" runat="server"></asp:DropDownList></td>
        </tr>
        <tr>
            <td>Trans Code:</td>
            <td>
                <asp:DropDownList ID="ddTC" runat="server">
                </asp:DropDownList>
                
            </td>
        </tr>
        <tr>
            <td>Payment Method:</td>
            <td><uc2:Select_Item ID="siPM" runat="server" /></td>
        </tr>
        <tr>
            <td colspan="2"><asp:Button ID="btnRunReport" runat="server" Text="Run Report" /></td>
        </tr>
    </table>
  <asp:HiddenField ID="hfShowReport" Value = "0" runat="server" />
    
    
    
    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
        AutoDataBind="true" ToolPanelView="None" />
    
    
    
</asp:Content>

