<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditPremiumBundlePremium.aspx.vb" Inherits="setup_EditPremiumBundlePremium" %>

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
                <td>Premium:</td>
                <td><asp:DropDownList ID="ddPremium" runat="server"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Qty</td>
                <td>
                    <asp:DropDownList ID="ddQty" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>CostEA:</td>
                <td><asp:TextBox ID="txtCostEA" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Active:</td>
                <td><asp:CheckBox ID="cbActive" runat="server" /></td>
            </tr>
            <tr>
                <td><asp:Button ID="btnSave" runat="server" Text="Save" /></td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
