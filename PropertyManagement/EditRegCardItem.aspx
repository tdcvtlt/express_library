<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditRegCardItem.aspx.vb" Inherits="setup_EditRegCardItem" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 19px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td class="style1">Priority:</td>
                <td>
                    <asp:DropDownList ID="ddPriority" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="style1">Active:</td>
                <td><asp:CheckBox ID="cbActive" runat="server" /></td>
            </tr>
            <tr>
                <td align = 'center' colspan = '2'>Item Text</td>
            </tr>
            <tr>
                <td colspan = '2'>
                    <asp:TextBox ID="txtItem" runat="server" Height="214px" Width="414px" 
                        TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align = 'center' colspan = '2'>
                    <asp:Button ID="Button1" runat="server" Text="Save" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
