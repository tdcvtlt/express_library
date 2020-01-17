<%@ Page Language="VB" AutoEventWireup="false" CodeFile="addAuthUser.aspx.vb" Inherits="marketing_addAuthUser" %>

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
                <td>FirstName:</td>
                <td><asp:textbox runat="server" id = "txtFirstName"></asp:textbox></td>
            </tr>
            <tr>
                <td>LastName:</td>
                <td><asp:textbox runat="server" id = "txtLastName"></asp:textbox></td>
            </tr>
            <tr>
                <td><asp:button runat="server" text="Save" onclick="Unnamed1_Click" /></td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
