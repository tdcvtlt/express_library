<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="TradeShowVendors.aspx.vb" Inherits="Reports_HR_TradeShowVendors" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div style="border:1px gray solid;margin-top:10px;">
    <h4 style="padding-left:20px;">TRADE SHOW VENDORS: FAITH-TS  +  G&P-TS</h4>
</div>
    <br />

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
            <td>
                <asp:Button ID="btnReport" runat="server" Text="Run Report" />
                <asp:Button ID="btnExcel" runat="server" Text="Excel" />
            </td>
        </tr>
    </table>

    <asp:Literal runat="server" id = "litReport"></asp:Literal>
</asp:Content>

