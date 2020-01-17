<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditHotelAccom.aspx.vb" Inherits="setup_EditHotelAccom" %>

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
                <td>Room Type:</td>
                <td>
                    <uc1:Select_Item ID="siRoomType" runat="server" />
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
