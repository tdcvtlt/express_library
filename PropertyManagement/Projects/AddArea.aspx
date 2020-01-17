<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddArea.aspx.vb" Inherits="PropertyManagement_Projects_AddArea" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        Select Area: <asp:DropDownList ID="ddAreas" runat="server"></asp:DropDownList><br />
        <asp:Button ID="btnAdd" runat="server" Text="ADD" /><asp:Button ID="btnCancel" runat="server" Text="Cancel" />
    </div>
    </form>
</body>
</html>
