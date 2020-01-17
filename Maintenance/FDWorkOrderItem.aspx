<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FDWorkOrderItem.aspx.vb" Inherits="Maintenance_FDWorkOrderItem" %>
<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc1" %>
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
                <td>Category:</td>
                <td><asp:dropdownlist runat="server" id = "ddCategory"></asp:dropdownlist></td>
            </tr>
            <tr>
                <td>Issue:</td>
                <td><uc1:Select_Item ID="siIssue" runat="server" /></td>
            </tr>
            <tr>
                <td>Description:</td>
                <td><asp:textbox runat="server" Width="214px" id = "txtDesc"></asp:textbox></td>
            </tr>
        </table>
        <asp:button runat="server" text="Submit" onclick="Unnamed3_Click" />
    </div>
    </form>
</body>
</html>
