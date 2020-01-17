<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditTimeCard.aspx.vb" Inherits="security_EditTimeCard" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td>Description:</td>
                <td><asp:textbox runat="server" id = "txtDesc"></asp:textbox></td>
            </tr>
            <tr>
                <td>Swipe:</td>
                <td><asp:textbox runat="server" id = "txtSwipe" TextMode="Password"></asp:textbox>
                </td>
            </tr>
            <tr>
                <td>Active:</td>
                <td><asp:checkbox runat="server" id = "cbActive"></asp:checkbox></td>
            </tr>
            <tr>
                <td><asp:button runat="server" text="Save" onclick="Unnamed1_Click" /></td>
            </tr>
        </table>
    </div>
    <asp:Label runat="server" id = "lblErr"></asp:Label>
    </form>
</body>
</html>
