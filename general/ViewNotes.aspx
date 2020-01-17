<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ViewNotes.aspx.vb" Inherits="general_ViewNotes" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:GridView ID="gvNotes" runat="server" OnRowDataBound ="gvNotes_RowDataBound" EmptyDataText="No Records"></asp:GridView>
    </div>
    </form>
</body>
</html>
