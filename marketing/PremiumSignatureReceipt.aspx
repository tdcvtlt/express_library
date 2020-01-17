<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PremiumSignatureReceipt.aspx.vb" Inherits="marketing_PremiumSignatureReceipt" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        
        <div>
            <asp:Image runat="server" ID="signature" Width="500" Height="500" />
        </div>
        <hr />
        <asp:Label runat="server" ID="premium" Font-Bold="true" Font-Size="20"></asp:Label>
        <br />
        <asp:Label runat="server" ID="date" Font-Size="18" Font-Italic="true"></asp:Label>
    </form>
</body>
</html>
