<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="CreateAFCFile.aspx.vb" Inherits="Accounting_CreateAFCFile" %>

<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>Maintenance Fee:</td>
            <td>
                <asp:DropDownList ID="ddMF" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>From:</td>
            <td>
                <uc1:DateField ID="dfStart" runat="server" />
            </td>
        </tr>
        <tr>
            <td>To:</td>
            <td>
                <uc1:DateField ID="dfEnd" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="GetWorkBook" runat="server" Text="Generate File" />
            </td>
        </tr>
    </table>
    <asp:Label ID="lblErr" runat="server" Text="" ForeColor="Red"></asp:Label>
</asp:Content>

