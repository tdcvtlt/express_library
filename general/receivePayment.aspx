<%@ Page Language="VB" AutoEventWireup="false" CodeFile="receivePayment.aspx.vb" Inherits="general_receivePayment" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:scriptmanager runat="server"></asp:scriptmanager>
        <asp:UpdatePanel runat="server" id = "UpdatePanel1">
            <ContentTemplate>
                <asp:Label runat="server" id = "lblWaiting"></asp:Label>    
                <br />
                <asp:Label runat="server" id="lblResponse"></asp:Label>
                <asp:Timer runat="server" id = "tmrCheck" Enabled="False"></asp:Timer>
                <asp:HiddenField ID="hfTickCounter" Value="0" runat="server" />
                <asp:HiddenField ID="hfCCTransID" Value="0" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
