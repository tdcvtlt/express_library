<%@ Page Language="VB" AutoEventWireup="false" CodeFile="editVendor.aspx.vb" Inherits="security_editVendor" %>

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
                <td>Vendor</td>
                <td><asp:dropdownlist runat="server" id = "ddVendors"></asp:dropdownlist></td>
            </tr>
            <tr>
                <td>Admin:</td>
                <td><asp:checkbox runat="server" id = "cbAdmin"></asp:checkbox></td>
            </tr>
            <tr>
                <td>Manager:</td>
                <td><asp:checkbox runat="server" id = "cbManager"></asp:checkbox></td>
            </tr>
            <tr>
                <td><asp:button runat="server" text="Save" onclick="Unnamed1_Click" /></td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
