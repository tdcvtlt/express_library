<%@ Page Title="Internal Do Not Call" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="DNC.aspx.vb" Inherits="marketing_DNC" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>Number to Add:<br />Format: 7573456760</td>
            <td>
                <asp:TextBox ID="txtNew" runat="server"></asp:TextBox>
                <asp:Button ID="btnAdd" runat="server" Text="Add" />
            </td>
        </tr>
        <tr>
            <td colspan = "2"><asp:Label ID="lblAddStatus" runat="server" Text=""></asp:Label></td>
        </tr>
        <tr>
            <td>Number to Check:<br />Format: 7573456760</td>
            <td>
                <asp:TextBox ID="txtCheck" runat="server"></asp:TextBox>
                <asp:Button ID="btnCheck" runat="server" Text="Check" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:LinkButton ID="LinkButton1" runat="server">Download Full Internal List</asp:LinkButton>
            </td>
        </tr>
    </table>
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View1" runat="server">
            <asp:GridView ID="gvResults" runat="server" EmptyDataText="No records found">
                </asp:GridView>
            <asp:Button ID="btnExport" runat="server" Text="Export" />
        </asp:View>
    </asp:MultiView>
    
</asp:Content>

