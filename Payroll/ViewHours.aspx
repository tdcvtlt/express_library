<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ViewHours.aspx.vb" Inherits="Payroll_ViewHours" %>
<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:scriptmanager runat="server"></asp:scriptmanager>
    <div>
    <table>
        <tr>
            <td>Start Date:</td>
            <td><uc1:DateField ID="dteStartDate" runat="server" /></td>
            <td>End Date:</td>
            <td><uc1:DateField ID="dteEndDate" runat="server" /></td>
            <td>
                <asp:Button ID="Button1" runat="server" Text="Search" />
            </td>
        </tr>
    </table>
    </div>
    <asp:literal id = "litHrs" runat="server" Mode="PassThrough"></asp:literal>
    <asp:label runat="server" id = "lblErr"></asp:label>
    </form>
</body>
</html>
