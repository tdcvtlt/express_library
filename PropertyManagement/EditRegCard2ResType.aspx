<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditRegCard2ResType.aspx.vb" Inherits="PropertyManagement_EditRegCard2ResType" %>

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
                <td>Res Type:</td>
                <td>
                    <uc1:Select_Item ID="siResType" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Active:</td>
                <td>
                    <asp:CheckBox ID="cbActive" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="Save" runat="server" Text="Button" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
