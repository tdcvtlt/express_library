<%@ Page Language="VB" AutoEventWireup="false" CodeFile="WizEditNote.aspx.vb" Inherits="wizards_WizEditNote" %>

<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc1" %>

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
                <td>NoteID:</td>
                <td><asp:textbox runat="server" id = "txtNoteID"></asp:textbox></td>
            </tr>
            <%if Request("type") <> "" then %>
            <tr>
                <td>Reason:</td>
                <td>
                    <uc1:Select_Item id = "siReason" runat="server" />
                </td>
            </tr>
            <%end if %>
            <tr>
                <td>Note:</td>
            </tr>
            <tr>
                <td colspan="2" align="center"><asp:TextBox ID="txtNote" runat="server" 
                        TextMode="MultiLine" Rows="10" width="250px"></asp:TextBox></td>
            </tr>
            <tr>
                <td><asp:Button ID="btnSave" runat="server" Text="Save" /></td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
