<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditLPHTML.aspx.vb" Inherits="LeadManagement_EditLPHTML" %>

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
                <td>File Path:</td>
                <td>
                    <asp:TextBox ID="txtPath" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>HTML:</td>
                <td>
                    <asp:CheckBox ID="cbHTML" runat="server" /></td>
            </tr>
            <tr>
                <td>Sidebar:</td>
                <td>
                    <asp:CheckBox ID="cbSideBar" runat="server" /></td>
            </tr>
            <tr>
                <td>Terms:</td>
                <td><asp:CheckBox ID="cbTerms" runat="server" /></td>
            </tr>
            <tr>
                <td>Active:</td>
                <td><asp:CheckBox ID="cbActive" runat="server" /></td>
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
