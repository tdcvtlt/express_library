<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="ScheduledPayments.aspx.vb" Inherits="Reports_Accounting_Scheduled_Payments" %>

<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ul id="menu">
        <li><asp:LinkButton ID="lbScheduled" runat="server">Scheduled</asp:LinkButton></li>
        <li><asp:LinkButton ID="lbReceived" runat="server">Received</asp:LinkButton></li>
    </ul>
    <table>
        <tr>
            <td>Select Account:</td>
            <td>
                <asp:DropDownList ID="ddAccount" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>Start Date:</td>
            <td>
                <uc1:DateField ID="dfSDate" runat="server" />
            </td>
        </tr>
        <tr>
            <td>End Date:</td>
            <td>
                <uc1:DateField ID="dfEDate" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btnRun" runat="server" Text="Run Report" />
                <asp:Button ID="btnExcel" runat="server" Text="Export" />
            </td>
        </tr>
    </table>
    <asp:Label ID="lblType" runat="server" Font-Bold="True" Font-Size="Medium" 
        Font-Underline="True"></asp:Label>
    <asp:GridView ID="gvReport" runat="server" EmptyDataText="No Records">
    </asp:GridView>
    
    <asp:HiddenField ID="hfScheduled" Value = "1" runat="server" />
</asp:Content>

