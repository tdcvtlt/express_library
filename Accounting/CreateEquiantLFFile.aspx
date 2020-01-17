<%@ Page Title="Create Equiant Late Fee File" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="CreateEquiantLFFile.aspx.vb" Inherits="Accounting_CreateEquiantMFFile" %>

<%@ Register Src="~/controls/DateField.ascx" TagPrefix="uc2" TagName="DateField" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>From:</td>
            <td>
                <uc2:DateField ID="dfStart" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                To:
            </td>
            <td>
                <uc2:datefield runat="server" id="dfEnd" />
            </td>
        </tr>
        <tr>
            <td>Invoice:</td>
            <td><asp:ListBox ID="lbTC" runat="server" Rows="5"></asp:ListBox></td>
            <td>
                <asp:Button ID="addTC" Text=">>" runat="server" /><br />
                <asp:Button ID="remTC" Text="<<" runat="server" />
            </td>
            <td><asp:ListBox ID="lbSelectedTC" runat="server" Rows="5"></asp:ListBox></td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="GetWorkBook" runat="server" Text="Generate File" />
            </td>
        </tr>
    </table>
    <asp:Label ID="lblErr" runat="server" Text="" ForeColor="Red"></asp:Label>
</asp:Content>

