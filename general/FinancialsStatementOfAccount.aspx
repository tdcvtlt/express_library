<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FinancialsStatementOfAccount.aspx.vb" Inherits="general_FinancialsStatementOfAccount" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>

<div id="container">
    <div style="text-align:center;line-height:.9em;">
    <h1>STATEMENT OF ACCOUNT</h1>
    <h3><strong><asp:Literal ID="Guest" runat="server"></asp:Literal></strong></h3>
    <h3><strong><asp:Literal ID="Street" runat="server"></asp:Literal></strong></h4>
    <h3><strong><asp:Literal ID="CityStateZip" runat="server"></asp:Literal></strong></h4>
    <h2><strong><asp:Literal ID="Kcp" runat="server"></asp:Literal></strong></h2>
    </div>
    
    <form id="form1" runat="server">
    <div>
    <asp:Literal ID="LIT" runat="server"></asp:Literal>
    </div>
    </form>
</div>
</body>
</html>
