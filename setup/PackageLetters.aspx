<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="PackageLetters.aspx.vb" Inherits="setup_PackageLetters" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>Name:</td>
            <td><asp:TextBox ID="txtName" runat="server"></asp:TextBox></td>
            <td><asp:Button ID="btnSearch" runat="server" Text="Search" /><asp:Button ID="btnNew" runat="server" Text="New" /></td>
            
        </tr>
    </table>
    <asp:GridView ID="gvLetters" runat="server" AutoGenerateSelectButton="True">
    </asp:GridView>
</asp:Content>

