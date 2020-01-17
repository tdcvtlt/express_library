<%@ Page Language="VB" AutoEventWireup="false" CodeFile="UpdateProgress.aspx.vb" Inherits="PropertyManagement_Projects_UpdateProgress" %>

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
                <td>Project Name:</td>
                <td><asp:Label ID="lblProject" runat="server" Text="Label"></asp:Label></td>
            </tr>
            <tr>
                <td>Room Number:</td>
                <td><asp:Label ID="lblRoom" runat="server" Text="Label"></asp:Label></td>
            </tr>
        </table><hr />
        Details:<br />
        <asp:Table ID="tblAreas" runat="server" EnableViewState="true"></asp:Table>
        <hr />
        <asp:Button ID="btnSave" runat="server" Text="Save Updates" />
    </div>
    </form>
</body>
</html>
