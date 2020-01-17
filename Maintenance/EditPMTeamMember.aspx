<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditPMTeamMember.aspx.vb" Inherits="Maintenance_EditPMTeamMember" %>

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
                <td>Personnel:</td>
                <td>
                    <asp:DropDownList ID="ddPersonnel" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Team Leader:</td>
                <td>
                    <asp:CheckBox ID="cbLeader" runat="server" /></td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnSave" runat="server" Text="Save" /></td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
