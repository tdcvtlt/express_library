<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="JW-OPC Invoice.aspx.vb" Inherits="Reports_CustomerService_JW_OPC_Invoice" %>
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
        <tr>
            <td colspan = "2">
            <asp:Button ID="btnReport" runat="server" Text="Report" ></asp:Button>
            &nbsp;&nbsp;
            <asp:Button ID="btnExcel" runat="server" Text="Excel" /></td>
        </tr>
    </table>
    <br />
    <asp:Literal runat="server" id = "litReport"></asp:Literal>
</asp:Content>

