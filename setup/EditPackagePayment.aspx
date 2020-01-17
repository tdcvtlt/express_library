<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditPackagePayment.aspx.vb" Inherits="setup_EditPackagePayment" %>

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
                <td>ID:</td>
                <td>
                    <asp:TextBox ID="txtID" runat="server" ReadOnly = "true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Method:</td>
                <td>
                    <uc1:Select_Item ID="siMethod" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Adjustment:</td>
                <td>
                    <asp:CheckBox ID="cbAdjustment" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Positive Adjustment:</td>
                <td>
                    <asp:CheckBox ID="cbPosNeg" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Fixed Amount:</td>
                <td>
                    <asp:CheckBox ID="cbFixedAmount" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Amount:</td>
                <td>
                    <asp:TextBox ID="txtAmount" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnSave" runat="server" Text="Submit" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
