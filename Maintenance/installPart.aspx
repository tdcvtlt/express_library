<%@ Page Language="VB" AutoEventWireup="false" CodeFile="installPart.aspx.vb" Inherits="Maintenance_installPart" %>
<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="Form2" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <asp:table runat="server" id = "Table1">
            <asp:TableRow runat="server">
                <asp:TableCell runat="server">Qty Installed</asp:TableCell>
                <asp:TableCell runat="server"><asp:DropDownList runat="server" id = "ddQty"></asp:DropDownList></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow runat="server">
                <asp:TableCell runat="server">Installed By:</asp:TableCell>
                <asp:TableCell runat="server"><asp:DropDownList runat="server" id = "ddTechnician"></asp:DropDownList></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow runat="server" visible = "False">
                <asp:TableCell runat="server">Date Moved:</asp:TableCell>
                <asp:TableCell runat="server"><uc1:DateField ID="dteMovedDate" runat="server" /></asp:TableCell>
            </asp:TableRow>
        </asp:table>
        <asp:button runat="server" text="Submit" onclick="Unnamed1_Click" />
    </div>
    </form>
</body>
</html>
