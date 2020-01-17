<%@ Page Language="VB" AutoEventWireup="false" CodeFile="addComments.aspx.vb" Inherits="general_addComments" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
<div>
    <table>
    <tr><td colspan = "2" align = "center"><b>Add Comment</b></td></tr>
    <tr>
    <td colspan="2" align="center"><asp:TextBox ID="txtNote" runat="server" 
                   TextMode="MultiLine" Rows="10" width="250px"></asp:TextBox></td>
            </tr>

            <tr>
                <td colspan="2" align="center">
                    <asp:button runat="server" text="Save" onclick="Unnamed1_Click" />
                    </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
