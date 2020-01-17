<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditSpecialNeed.aspx.vb" Inherits="general_EditSpecialNeed" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table align="center">
            <tr>
                <th colspan="2" align="center">Special Need Record</th>
            </tr>
            <tr>
                <td>Special Need:<br /><asp:label runat="server" visible = "false" id = "lblNeed">Value:</asp:label></td>
                <td><asp:dropdownlist runat="server" id = "ddNeeds" autopostback = "true" onSelectedIndexChanged = "ddNeeds_SelectedIndexChanged"></asp:dropdownlist><br />
                    <asp:TextBox id="txtNeed" runat="server" visible = "False"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="btnSave" runat="server" Text="Save" />
                    <input type="button" value="Close" onclick="javascript:window.opener.document.forms[0].submit();window.close();" />        
                    <asp:label runat="server" id="Label1"></asp:label>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
