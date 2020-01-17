<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditWebSource.aspx.vb" Inherits="setup_EditWebSource" %>

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
                <td>Web Source:</td>
                <td>
                    <uc1:Select_Item ID="siWebSource" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Active:</td>
                <td><asp:CheckBox ID="cbActive" runat="server" /></td>
            </tr>
            <tr>
                <td><asp:Button ID="btnSave" runat="server" Text="Submit" /></td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
