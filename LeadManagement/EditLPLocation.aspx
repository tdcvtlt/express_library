<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditLPLocation.aspx.vb" Inherits="LeadManagement_EditLPLocation" %>

<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc1" %>

<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
                <asp:ScriptManager runat="server"></asp:ScriptManager>

    <div>
        <table>
            <tr>
                <td>Location:</td>
                <td>
                    <uc1:Select_Item ID="siLocation" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Start Date:</td>
                <td>
                    <uc2:DateField ID="dteSDate" runat="server" />
                </td>

            </tr>
            <tr>
                <td>End Date:</td>
                <td>
                    <uc2:DateField ID="dteEDate" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Active:</td>
                <td>
                    <asp:CheckBox ID="cbActive" runat="server" />
                </td>

            </tr>
            <tr>
                <td>Version:</td>
                <td>
                    <asp:TextBox ID="txtVersion" runat="server" ReadOnly ="true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="Button1" runat="server" Text="Save" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
