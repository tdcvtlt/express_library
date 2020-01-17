<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditLPImage.aspx.vb" Inherits="LeadManagement_EditLPImage" %>

<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc1" %>

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
                <td>Location:</td>
                <td>
                    <uc1:Select_Item ID="siLocation" runat="server" />
                </td>
            </tr>
            <tr>
                <td>URL:</td>
                <td>
                    <asp:TextBox ID="txtURL" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Picture:</td>
                <td>
                    <asp:CheckBox ID="cbPicture" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Active</td>
                <td>
                    <asp:CheckBox ID="cbActive" runat="server" />
                </td>
            </tr>
            <tr>
                <td><asp:Button ID="btnSave" runat="server" Text="Save" /></td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
