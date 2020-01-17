<%@ Page Title="Create Equiant Conversion File" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="CreateEquiantConversionFile.aspx.vb" Inherits="Accounting_CreateEquiantConversionFile" %>

<%@ Register Src="~/controls/DateField.ascx" TagPrefix="uc1" TagName="DateField" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>Start Date:</td>
            <td><uc1:DateField runat="server" ID="dfStart" /></td>
        </tr>
        <tr>
            <td>End Date:</td>
            <td><uc1:DateField runat="server" ID="dfEnd" /></td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="GetWorkbook" runat="server" Text="Generate File" />
            </td>
        </tr>
    </table>
    <asp:Label ID="lblErr" runat="server" Text="" ForeColor="Red"></asp:Label>
</asp:Content>

