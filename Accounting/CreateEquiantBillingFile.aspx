<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="CreateEquiantBillingFile.aspx.vb" Inherits="Accounting_CreateEquiantFile" %>

<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>
<%@ Register Src="~/controls/DateField.ascx" TagPrefix="uc2" TagName="DateField" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        
        <tr>
            <td valign="top">Start Date:</td>
            <td>
                <uc2:DateField runat="server" ID="dfStart" />
            </td>
        </tr>
        <tr>
            <td valign="top">End Date:</td>
            <td><uc2:DateField runat="server" ID="dfEnd" /></td>
        </tr>
        
        <tr>
            <td>
                <asp:Button ID="GetWorkBook" runat="server" Text="Generate File" />
            </td>
            <td>
                <asp:Button ID="GetCancellations" runat="server" Text="Generate Cancellation File" />
            </td>
        </tr>
    </table>
    <asp:Label ID="lblErr" runat="server" Text="" ForeColor="Red"></asp:Label>
</asp:Content>

