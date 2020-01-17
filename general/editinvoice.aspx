<%@ Page Language="VB" AutoEventWireup="false" CodeFile="editinvoice.aspx.vb" Inherits="general_editinvoice" %>

<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>
<%@ Register src="../controls/username.ascx" tagname="username" tagprefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <table>
            <tr>
                <td>
                    Trans Code:
                </td>
                <td>
                    <asp:DropDownList ID="ddTransCodes" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Amount:</td>
                <td><asp:TextBox ID="txtAmount" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Reference:</td>
                <td>
                    <asp:TextBox ID="txtReference" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td valign="top">Trans Date:</td>
                <td>
                    <uc1:DateField ID="dtTD" runat="server" />
                </td>
            </tr>
            <tr>
                <td valign="top">Due Date:</td>
                <td>
                    <uc1:DateField ID="dtDD" runat="server" />
                </td>
            </tr>

            <tr>
                <td valign="top">User:</td>
                <td>
                    <uc2:username ID="cnUserName" runat="server" />
                </td>
            </tr>

        </table>
        
        
    
    </div>
    <asp:Button ID="btnSave" runat="server" Text="Save" />
    <asp:Button ID="btnClose" runat="server" Text="Close" />
    <p>
        <asp:Label ID="lblErr" runat="server" ForeColor="Red"></asp:Label>
    </p>
    </form>
</body>
</html>
