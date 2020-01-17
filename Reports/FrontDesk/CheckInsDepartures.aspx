<%@ Page Title="Check-Ins / Departures" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="CheckInsDepartures.aspx.vb" Inherits="Reports_FrontDesk_CheckInsDepartures" AspCompat="true" %>

<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td colspan = "2">
                <asp:RadioButtonList ID="rblType" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="checkins">Check-Ins</asp:ListItem>
                    <asp:ListItem Value="departures">Departures</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td>Start Date:</td>
            <td><uc1:DateField ID="SDate" runat="server" /></td>
        </tr>
        <tr>
            <td>End Date:</td>
            <td><uc1:DateField ID="EDate" runat="server" /></td>
        </tr>
        <tr>
            <td colspan = "2"><asp:CheckBox ID="cbRoom" runat="server" Text="Sort by Room" /></td>
        </tr>
        <tr>
            <td colspan = "2">    <asp:Button ID="btnRun" runat="server" Text="Run Report" />
    <asp:Button ID="btnPrintable" runat="server" Text="Print" />
    <asp:Button ID="btnExcel" runat="server" Text="Export" />
</td>
        </tr>
    </table>
    <asp:Literal ID="Lit" runat="server"></asp:Literal>

</asp:Content>

