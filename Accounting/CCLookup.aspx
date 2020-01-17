<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="CCLookup.aspx.vb" Inherits="Accounting_CCLookup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>Last 4 Digits of Card:</td>
            <td><asp:TextBox runat="server" id = "txtNumber" MaxLength="4"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Expiration (MMDD):</td>
            <td><asp:TextBox runat="server" id = "txtExp" MaxLength="4"></asp:TextBox></td>
        </tr>
        <tr>
            <td><asp:Button runat="server" Text="Search" onclick="Unnamed1_Click"></asp:Button></td>
        </tr>
    </table>

    <asp:GridView runat="server" id = "gvCCTrans" EmptyDataText = "No Records"></asp:GridView>
</asp:Content>

