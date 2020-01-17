<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddField.aspx.vb" Inherits="PropertyManagement_AddField" %>

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
        <td>
            <asp:Label ID="lblFieldName" runat="server" Text="Enter Field Name"></asp:Label>
            </td>
            <td>
            <asp:TextBox ID="txtFieldName" runat="server"></asp:TextBox>
            </td>
            </tr>
            <tr>
            <td>
            <asp:Label
            ID="lblWO" runat="server" Text="Requires Work Order"></asp:Label>
            </td>
            <td>
            <asp:CheckBox ID="CheckBox1"
            runat="server" />
            </td>
            </tr>
            <tr>
            <td>
                <asp:button runat="server" text="Button" />
                </td>
            </tr>
            </table>
    </div>
    </form>
</body>

</html>
