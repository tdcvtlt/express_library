<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditPersDept.aspx.vb" Inherits="security_EditPersDept" %>
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
                <td>Department:</td>
                <%If Request("PersDeptID") = 0 Then %>
                    <td><uc1:Select_Item ID="siDept" runat="server" /></td>
                <%Else %>
                    <td><asp:textbox runat="server" id = "txtDept" Readonly></asp:textbox></td>
                <%End If %>
            </tr>
            <tr>
                <td>Active:</td>
                <td><asp:checkbox runat="server" id = "cbActive"></asp:checkbox></td>
            </tr>
            <tr>
                <td>Manager:</td>
                <td><asp:checkbox runat="server" id = "cbMgr"></asp:checkbox></td>
            </tr>
            <tr>
                <td>Clock In:</td>
                <td><asp:checkbox runat="server" id = "cbClockIn"></asp:checkbox></td>
            </tr>
            <tr>
                <td>Company:</td>
                <%If request("PersDeptID") = 0 Then %>
                    <td><uc1:Select_Item ID="siCompany" runat="server" /></td>
                <%Else %>
                    <td><asp:textbox runat="server" id = "txtCompany" readonly></asp:textbox></td>
                <%End If %>
            </tr>
            <tr>
                <td><asp:button runat="server" text="Submit" onclick="Unnamed1_Click" /></td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
