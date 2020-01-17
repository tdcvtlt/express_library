<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddRoom.aspx.vb" Inherits="PropertyManagement_Projects_AddRoom" %>

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
                <td style="vertical-align:top;">Select Room:</td>
                <td><asp:listbox ID="ddRooms" runat="server" SelectionMode="Multiple"></asp:listbox></td>
            </tr>
            <tr>
                <td colspan="2"><asp:Button ID="btnAdd" runat="server" Text="ADD" /><asp:Button ID="btnCancel" runat="server" Text="Cancel" /></td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
