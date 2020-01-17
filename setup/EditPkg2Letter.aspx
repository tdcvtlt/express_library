<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditPkg2Letter.aspx.vb" Inherits="setup_EditPkg2Letter" %>

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
                <td>
                    Letter:
                </td>
                <td>
                    <asp:DropDownList ID="ddLetters" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Subject:</td>
                <td><asp:TextBox ID="txtSubject" runat="server" Width="263px"></asp:TextBox></td>
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
