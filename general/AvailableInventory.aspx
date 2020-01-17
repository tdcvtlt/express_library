<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AvailableInventory.aspx.vb" Inherits="general_AvailableInventory" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TextBox ID="txtFilter" runat="server"></asp:TextBox><asp:Button ID="btnFilter"
            runat="server" Text="Filter" />
        <asp:GridView ID="gvInventory" runat="server" AutoGenerateColumns="true" AutoGenerateSelectButton="true" EmptyDataText="No Records">
            <AlternatingRowStyle BackColor="#C7E3D7" />
        </asp:GridView>
    </div>
    </form>
</body>
</html>
