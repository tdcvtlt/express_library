<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditPackage2Vendor.aspx.vb" Inherits="setup_EditPackage2Vendor" %>

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
                <td>Vendor:</td>
                <td>
                    <asp:DropDownList ID="ddVendors" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Display:</td>
                <td>
                    <asp:CheckBox ID="cbDisplay" runat="server" /></td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnSave" runat="server" Text="Submit" /></td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
