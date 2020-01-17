<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RenameDoc.aspx.vb" Inherits="general_RenameDoc" %>

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
                <td>Original File Name:</td>
                <td>
                    <asp:TextBox ID="txtOldName" runat="server" ReadOnly></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>New File Name:</td>
                <td>
                    <asp:TextBox ID="txtNewName" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                
                    <asp:Button ID="Button1" runat="server" Text="Update" />
                
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
