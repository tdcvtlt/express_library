﻿<%@ Page Language="VB" AutoEventWireup="false" CodeFile="DoNotSellListOverRide.aspx.vb" Inherits="marketing_DoNotSellListOverRide" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        This Prospect Is On The Do Not Sell List. <br />
        Please Enter the OverRide Code Generated by the FINANCE DEPARTMENT to Continue. <br /><br />
        Code: <asp:TextBox ID="txtCode" runat="server"></asp:TextBox> <br /><br />
        <asp:Button ID="btnSubmit" runat="server" Text="Submit" /> <asp:Button ID="btnClose" runat="server" Text="Cancel" /> 
    </div>
    </form>
</body>
</html>