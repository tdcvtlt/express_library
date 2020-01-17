<%@ Page Language="VB" AutoEventWireup="false" CodeFile="addSecGroupMember.aspx.vb" Inherits="security_addSecGroupMember" %>

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
                <td>Group:</td>
                <td><asp:dropdownlist runat="server" id = "ddMembers"></asp:dropdownlist></td>
            </tr>
            <tr>
                <td colspan = '2'><asp:button runat="server" text="Add Member" 
                        onclick="Unnamed1_Click" /></td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
