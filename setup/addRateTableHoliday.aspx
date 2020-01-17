<%@ Page Language="VB" AutoEventWireup="false" CodeFile="addRateTableHoliday.aspx.vb" Inherits="setup_addRateTableHoliday" %>

<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

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
                <td>Start Date:</td>
                <td>
                    <uc1:DateField ID="dteSDate" runat="server" />
                </td>
            </tr>
            <tr>
                <td>End Date:</td>
                <td>
                    <uc1:DateField ID="dteEDate" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Rate:</td>
                <td>
                    <asp:TextBox ID="txtAmount" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" /></td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
