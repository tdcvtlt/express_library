<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditResortAccom.aspx.vb" Inherits="setup_EditResortAccom" %>

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
                <td>Unit Type:</td>
                <td>
                    <uc1:Select_Item ID="siUnitType" runat="server" />
                </td>
            </tr>
            <tr>
                <td>BedRooms:</td>
                <td>
                    <asp:DropDownList ID="ddBedroom" runat="server">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem Value="1">1BD</asp:ListItem>
                        <asp:ListItem>1BD-DWN</asp:ListItem>
                        <asp:ListItem>1BD-UP</asp:ListItem>
                        <asp:ListItem Value="2">2BD</asp:ListItem>
                        <asp:ListItem Value="3">3BD</asp:ListItem>
                        <asp:ListItem Value="4">4BD</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Max Occupancy:</td>
                <td>
                    <asp:TextBox ID="txtMaxOcc" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>RateTable:</td>
                <td>
                    <asp:DropDownList ID="ddRateTable" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnSave" runat="server" Text="Save" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
