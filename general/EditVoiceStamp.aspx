<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditVoiceStamp.aspx.vb" Inherits="general_EditVoiceStamp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Edit VoiceStamp</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table align="center">
            <tr>
                <th colspan="2" align="center">VoiceStamp Record</th>
            </tr>
            <tr>
                <td>VS Number:</td>
                <td><asp:TextBox id="txtVSNumber" runat="server"></asp:TextBox></td>
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
