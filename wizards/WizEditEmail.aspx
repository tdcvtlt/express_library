<%@ Page Title="" Language="VB" AutoEventWireup="false" CodeFile="WizEditEmail.aspx.vb" Inherits="wizards_WizEditEmail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Edit Email</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td>Record ID:</td>
                <td>
                    <asp:TextBox ID="txtEmailID" readonly="true" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Prospect ID:</td>
                <td>
                    <asp:TextBox ID="txtProspectID" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Email:</td>
                <td>
                    <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Active:</td>
                <td>
                    <asp:CheckBox ID="ckActive" runat="server" /></td>
            </tr>
            <tr>
                <td>Primary:</td>
                <td>
                    <asp:CheckBox ID="ckPrimary" runat="server" /></td>
            </tr>
        </table>
        <asp:Button ID="btnSave" runat="server" Text="Save" />
        <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
    </div>
    </form>
</body>
</html>
