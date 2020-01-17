<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PrintWorkOrder.aspx.vb" Inherits="Maintenance_PrintWorkOrder" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body onload="window.print();window.close();">
    <form id="form1" runat="server">
    <div>
        <asp:literal runat="server" id = "litRequest"></asp:literal>
    </div>
    </form>
</body>
</html>
