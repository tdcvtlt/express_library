<%@ Page Language="VB" AutoEventWireup="false" CodeFile="editPRItem.aspx.vb" Inherits="Accounting_editPRItem" %>

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
                <td>Item:</td>
                <td><asp:textbox runat="server" id = "txtItem" readonly></asp:textbox></td>
            </tr>
            <tr>
                <td>Qty:</td>
                <td><asp:dropdownlist runat="server" id = "ddQty"></asp:dropdownlist></td>
            </tr>
            <tr>
                <td>Amount:</td>
                <td><asp:textbox runat="server" id = "txtAmount"></asp:textbox></td>
            </tr>
            <tr>
                <td>Location:</td>
                <td><asp:textbox runat="server" id = "txtLocation"></asp:textbox></td>
            </tr>
            <tr>
                <td>Purpose:</td>
                <td><asp:textbox runat="server" id = "txtPurpose"></asp:textbox></td>
            </tr>
            <tr>
                <td><asp:button runat="server" text="Save" onclick="Unnamed1_Click" /></td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
