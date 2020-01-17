<%@ Page Language="VB" AutoEventWireup="false" CodeFile="vendorFilter.aspx.vb" Inherits="Accounting_vendorFilter" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    Vendor Name Filter: <asp:textbox runat="server" id = "txtVendor"></asp:textbox>
    <asp:button runat="server" text="Search" onclick="Unnamed1_Click" />
    <div style="height: 375px;overflow:auto">
        <asp:gridview runat="server" id = "gvVendors" AutoGenerateSelectButton="True" 
            EmptyDataText="No Records" ></asp:gridview>
        <asp:button runat="server" text="New Vendor" onclick="Unnamed2_Click" />
    </div>
    </form>
</body>
</html>
