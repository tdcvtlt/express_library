<%@ Page Language="VB" AutoEventWireup="false" CodeFile="editComment.aspx.vb" Inherits="general_addComment" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Edit Comment</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table align="center">
            <tr>
                <th colspan="2" align="center">Comment Record</th>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:TextBox ID="txtComment" runat="server" 
                        TextMode="MultiLine" Rows="10" width="250px"></asp:TextBox></td>
            </tr>
            <tr>
                <td>User Name:</td>
                <td><asp:TextBox readonly="true" ID="txtUser" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Date Created:</td>
                <td><asp:TextBox ReadOnly="true" ID="txtDate" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="btnSave" runat="server" Text="Save" />
                    <input type="button" value="Close" onclick="javascript:window.opener.document.forms[0].submit();window.close();" />        
                </td>
            </tr>
        </table>    
    </div>
    <asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>
    </form>
</body>
</html>
