<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditVendorRep2Tour.aspx.vb" Inherits="marketing_EditVendorRep2Tour" %>

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
                <td>Location:</td>
                <td><asp:DropDownList ID="ddLocations" runat="server"></asp:DropDownList></td>
            </tr>
            <tr>
                <td>Solicitor:</td>
                <td><asp:DropDownList ID="ddReps" runat="server"></asp:DropDownList></td>
            </tr>
            <tr>
                <td><asp:Button ID="btnSave" runat="server" Text="Save" /></td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
