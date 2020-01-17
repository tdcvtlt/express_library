<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Select_Item.ascx.vb" Inherits="Select_Item" %>
<table width="100%">
    <tr>
        <td align = "left"><asp:Label ID="Label1" runat="server" Text=""></asp:Label></td>        
        <td>
            <asp:DropDownList ID="DropDownList1" runat="server">
                <asp:ListItem Value="0"> (empty)</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
</table>
<asp:Label ID="lblErr" runat="server" ForeColor="Red"></asp:Label>
