<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Campaign.ascx.vb" Inherits="Campaign" %>
<table width="100%">
    <tr>
        <td>
            <asp:DropDownList ID="DropDownList1" runat="server">
                <asp:ListItem Value="0"> (empty)</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
</table>
<asp:Label ID="lblErr" runat="server" ForeColor="Red"></asp:Label>
