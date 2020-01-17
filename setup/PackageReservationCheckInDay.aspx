<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PackageReservationCheckInDay.aspx.vb" Inherits="setup_PackageReservationCheckInDay" %>

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
                <td>Check-In Day:</td>
                <td>
                    <asp:DropDownList ID="ddCheckInDay" runat="server"></asp:DropDownList></td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" /></td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
