<%@ Page Title="Gift Card Allocation" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="GiftCards.aspx.vb" Inherits="general_GiftCards" %>

<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>Starting #:</td>
            <td>
                <asp:TextBox ID="txtStart" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Ending #:</td>
            <td>
                <asp:TextBox ID="txtEnd" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Balance:</td>
            <td>
                <asp:TextBox ID="txtBalance" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Activate:</td>
            <td>
                <asp:CheckBox ID="ckActive" runat="server" /></td>
        </tr>
        <tr>
            <td>Locations:</td>
            <td><uc1:Select_Item ID="siLocations" runat="server" /></td>
            <td><asp:Button ID="btnAdd" runat="server" Text="Add" />
            </td>
        </tr>
        <tr>
            <td colspan = '3'>
                <asp:ListBox ID="ddLocations" runat="server" Height="89px" Width="192px">
                </asp:ListBox>
                <asp:Button ID="btnRemove" runat="server" Text="Remove" />
                
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" /></td>
        </tr>
    </table>
    <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
</asp:Content>

