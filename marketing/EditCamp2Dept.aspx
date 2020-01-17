<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditCamp2Dept.aspx.vb" Inherits="marketing_EditCamp2Dept" %>

<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td>Department:</td>
                <td>
                    <uc1:Select_Item ID="siDept" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Location:</td>
                <td>
                    <uc1:Select_Item ID="siLoc" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="Button1" runat="server" Text="Button" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
