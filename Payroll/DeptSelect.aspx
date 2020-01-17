<%@ Page Language="VB" AutoEventWireup="false" CodeFile="DeptSelect.aspx.vb" Inherits="Payroll_DeptSelect" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        Select Department: <br />
        <asp:radiobuttonlist runat="server" RepeatDirection="Horizontal" id = "cbDepts" onSelectedIndexChanged = "cbDepts_SelectedIndexChanged" autoPostBack = "true"></asp:radiobuttonlist>
    </div>
    <asp:Label runat="server" id = "lblErr"></asp:Label>
    </form>
</body>
</html>
