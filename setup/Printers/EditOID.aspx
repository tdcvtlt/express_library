<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditOID.aspx.vb" Inherits="setup_Printers_EditOID" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Edit OID</title>
    <link href="../../styles/master.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td>OID:</td>
                <td><asp:TextBox ID="OID" runat="server" Width="220px"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Description:</td>
                <td><asp:TextBox ID="Description" runat="server" Width="220px"></asp:TextBox></td>
            </tr>
        </table>
        <ul id="menu" style="max-width:350px;">
            <li><asp:LinkButton ID="lbSave" runat="server">Save</asp:LinkButton></li>
            <li><asp:LinkButton ID="lbClose" runat="server">Close</asp:LinkButton></li>
        </ul>
    </div>
    </form>
</body>
</html>
