<%@ Page Title="Owner Purchase Detail" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="OwnerPurchaseDetail.aspx.vb" Inherits="Reports_Contracts_OwnerPurchaseDetail" aspCompat="true"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table>
    <tr>
        <td>
            Enter a Number
        </td>
        <td>
            <asp:TextBox ID="txtNumber" runat="server"></asp:TextBox>
        </td>
        <td>
            <asp:Button ID="btnAdd" runat="server" Text="ADD >>>" />
            <br />
            <asp:Button ID="btnRemove" runat="server" Text="Remove <<<" />
        </td>
    </tr>
    <tr>
        <td colspan="3">
            <asp:ListBox ID="liNumber" runat="server"></asp:ListBox>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Button ID="btnReport" runat="server" Text="Button" />
        </td>
    </tr>
</table>
    <asp:Literal ID="litReport" runat="server"></asp:Literal>
</asp:Content>

