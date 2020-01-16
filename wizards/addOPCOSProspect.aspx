<%@ Page Language="VB" AutoEventWireup="false" CodeFile="addOPCOSProspect.aspx.vb" Inherits="wizards_addOPCOSProspect" %>
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
                <td>First Name:</td>
                <td><asp:textbox runat="server" id = "txtFName"></asp:textbox></td>
                <td>Last Name:</td>
                <td><asp:textbox runat="server" id = "txtLName"></asp:textbox></td>
                <td>Home Phone:</td>
                <td><asp:textbox runat="server" id = "txtHPhone"></asp:textbox></td>
            </tr>
            <tr>
                <td>Address:</td>
                <td colspan = '5'><asp:textbox runat="server" id = "txtAddress" Width="552px"></asp:textbox></td>
            </tr>
            <tr>
                <td>City:</td>
                <td><asp:textbox runat="server" id = "txtCity"></asp:textbox></td>
                <td>State:</td>
                <td><uc1:Select_Item ID="siState" runat="server" /></td>
                <td>Postal Code:</td>
                <td><asp:textbox runat="server" id = "txtPostalCode"></asp:textbox></td>
            </tr>
            <tr>
                <td colspan = '6' align = 'center'><asp:button runat="server" text="Set Tour Date" 
                        onclick="Unnamed1_Click" /></td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
