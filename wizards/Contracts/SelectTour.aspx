<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SelectTour.aspx.vb" Inherits="wizards_Contracts_SelectTour" Title="Select Tour" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        Today's Tours - <asp:Label ID="lblDate" runat="server" Text=""></asp:Label>
        <asp:GridView ID="gvTours" runat="server" AutoGenerateColumns="true" AutoGenerateSelectButton="true">
            <AlternatingRowStyle BackColor="#C7E3D7" />
        </asp:GridView>
    </div>
    </form>
</body>
</html>
