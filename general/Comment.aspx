﻿<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Comment.aspx.vb" Inherits="general_Comment" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:gridview runat="server" id = "gvComments" EmptyDateText = "No Records"></asp:gridview>
        <asp:button runat="server" text="Add Comment" onclick="Unnamed1_Click1" />
    </div>
    </form>
</body>
</html>
