<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditPers2Team.aspx.vb" Inherits="security_EditPers2Team" %>
<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc2" %>

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
                <td>Sales Team:</td>
                <td><uc2:Select_Item ID="siSalesTeam" runat="server" /></td>
            </tr>
            <tr>
                <td><asp:button runat="server" text="Save" onclick="Unnamed1_Click" /></td>
            </tr>
        </table>
        <asp:label runat="server" id = "lblErr"></asp:label>
    </div>
    </form>
</body>
</html>
