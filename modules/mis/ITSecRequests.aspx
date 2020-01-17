<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="ITSecRequests.aspx.vb" Inherits="mis_ITSecRequests" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>
                Enter Request ID:
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="txtRequest" runat="server"></asp:TextBox></td>
            <td>
                <asp:Button ID="Button1" runat="server" Text="Query" /></td>
        </tr>
    </table>
    <asp:GridView ID="gvRequests" runat="server" AutoGenerateSelectButton = "true" EmptyDataText = "No Records" OnRowDataBound = "gvRequests_RowDataBound"><AlternatingRowStyle BackColor="#66FF99" />
    </asp:GridView>

</asp:Content>

