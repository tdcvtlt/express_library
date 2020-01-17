<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EditLetterTagStyle.aspx.vb" Inherits="setup_Letters_EditLetterTagStyle"  ValidateRequest="false"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>Tag Style ID:</td>
            <td>
                <asp:TextBox ID="txtID" ReadOnly="true" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Style:</td>
            <td>
                <asp:TextBox ID="txtStyle" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Starting Tag:</td>
            <td>
                <asp:TextBox ID="txtStartingTag" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>Item Tag:</td>
            <td>
                <asp:TextBox ID="txtItemTag" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btnSave" runat="server" Text="Save" />
            </td>
        </tr>
    </table>
</asp:Content>

