<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditNoteTemplate.aspx.vb" Inherits="general_EditNote" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Edit Note</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table align="center">
            <tr>
                <td>TemplateID:</td>
                <td><asp:textbox ID="NoteTemplateID" runat="server" ReadOnly="True"></asp:textbox></td>
            </tr>
            <tr>
                <td>Title:</td>
                <td><asp:TextBox runat="server" ID="Title"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Active:</td>
                <td><asp:checkbox ID="Active" runat="server"></asp:checkbox></td>
            </tr>
            <tr>
                <td colspan="2" align="center"><asp:TextBox ID="txtNote" runat="server" 
                        TextMode="MultiLine" Rows="10" width="250px"></asp:TextBox></td>
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
