<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="LocationInvoice.aspx.vb" Inherits="Reports_CustomerService_LocationInvoice" AspCompat="true" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table>
            <tr>
            <td>Start Date:</td>
            <td>
                <uc1:datefield ID="dteSDate" runat="server" />
                </td>
        </tr>
        <tr>
            <td>End Date:</td>
            <td>
                <uc1:datefield ID="dteEDate" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan = "2"><asp:Button ID="Button1" runat="server" Text="Button" onclick="Unnamed1_Click"></asp:Button><asp:Button
                    ID="Button2" runat="server" Text="Excel" /></td>
        </tr>
    </table>
    <br />
    <asp:Literal runat="server" id = "litReport"></asp:Literal>
</asp:Content>

