<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditLPDevice.aspx.vb" Inherits="LeadManagement_EditLPDevice" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
    <div>
        <table>
            <tr>
                <td>Device:</td>
                <td>
                    <asp:TextBox ID="txtDevice" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Active:</td>
                <td>
                    <asp:CheckBox ID="cbActive" runat="server" /></td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnSave" runat="server" Text="Save" /></td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
