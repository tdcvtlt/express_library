<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditView.aspx.vb" Inherits="setup_Letters_EditView" %>

<%@ Register src="../../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc1" %>

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
                <th colspan="2" align="center">Letter Views</th>
            </tr>
            <tr>
                <td>View:</td>
                <td>
                    <asp:DropDownList ID="ddView" runat="server" AutoPostBack="True">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Source:</td>
                <td>
                    <asp:CheckBox ID="ckSource" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Key Field Name:</td>
                <td>
                    <asp:DropDownList ID="ddKeyFieldName" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Type:</td>
                <td>
                    <uc1:Select_Item ID="siType" runat="server" />
                </td>
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
