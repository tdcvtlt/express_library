<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SwapPremiumIssued.aspx.vb" Inherits="general_SwapPremiumIssued" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td>Issued <%=IIf(Request("Field").ToLower() = "reservationid", "Gift", "Premium") %>:</td>
                <td><asp:DropDownList ID="ddIssued" runat="server" AutoPostBack="True"></asp:DropDownList></td>
            </tr>
            <tr>
                <td>Issued Qty:</td>
                <td><asp:DropDownList id="ddIssuedQty" runat="server"></asp:DropDownList></td>
            </tr>
            <tr>
                <td>New <%=IIf(Request("Field").ToLower() = "reservationid", "Gift", "Premium") %>:</td>
                <td><asp:DropDownList ID="ddNew" runat="server"></asp:DropDownList></td>
            </tr>
            <tr>
                <td>New Qty:</td>
                <td><asp:DropDownList ID="ddNewQty" runat="server"></asp:DropDownList></td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnAdd" runat="server" Text="Add" />
                    <asp:Button ID="btnRemove" runat="server" Text="Remove" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Listbox ID="lbNew" runat="server"></asp:Listbox>
                </td>
            </tr>
            <tr>
                <td colspan="2"><asp:Button ID="btnSwap" runat="server" Text="Swap" /><asp:Button ID="btnCancel" runat="server" Text="Cancel" /></td>
            </tr>
        </table>
        <asp:Label ID="lblErr" runat="server" ForeColor="Red"></asp:Label>
    </div>
    </form>
</body>
</html>
