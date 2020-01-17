<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EditLPEntryForm.aspx.vb" Inherits="LeadManagement_EditLPEntryForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>ID:</td>
            <td>
                <asp:TextBox ID="txtID" runat="server" Width="612px"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Description:</td>
            <td>
                <asp:TextBox ID="txtDesc" runat="server" Width="612px"></asp:TextBox></td>
        </tr>
        <tr>
            <td>HTML Path:</td>
            <td>
                <asp:TextBox ID="txtHTML" runat="server" Width="612px"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Javascript Path:</td>
            <td>
                <asp:TextBox ID="txtJS" runat="server" Width="612px"></asp:TextBox></td>
        </tr>
        <tr>
            <td>SideBar Path:</td>
            <td>
                <asp:TextBox ID="txtSB" runat="server" Width="612px"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Terms Path:</td>
            <td>
                <asp:TextBox ID="txtTerms" runat="server" Width="612px"></asp:TextBox></td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnSave" runat="server" Text="Save" /></td>
        </tr>
    </table>
</asp:Content>

