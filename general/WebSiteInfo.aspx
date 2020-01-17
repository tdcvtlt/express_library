<%@ Page Language="VB" AutoEventWireup="false" CodeFile="WebSiteInfo.aspx.vb" Inherits="general_WebSiteInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:scriptmanager runat="server"></asp:scriptmanager>
    <asp:multiview runat="server" id = "MultiView1">
    <asp:View runat="server" id = "View1">
    <div>
        <table>
            <tr>
                <td>User Name:</td>
                <td><asp:textbox runat="server" id = "txtUName"></asp:textbox></td>
            </tr>
            <tr>
                <td>Password</td>
                <td><asp:textbox runat="server" id = "txtPassword" TextMode="Password"></asp:textbox></td>
            </tr>
            <tr>
                <td>Email:</td>
                <td><asp:textbox runat="server" id = "txtEmail"></asp:textbox></td>
            </tr>
            <tr>
                <td>Validation Code:</td>
                <td><asp:TextBox runat="server" ID="txtValCode"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Date Created:</td>
                <td><asp:textbox runat="server" id = "txtDateCreated" readonly></asp:textbox></td>
            </tr>
            <tr>
                <td>Confirmed:</td>
                <td><asp:textbox runat="server" id = "txtConfirmed" readonly></asp:textbox></td>
            </tr>
            <tr style="font-weight: 700">
                <td colspan = '2' align = 'center'><asp:button runat="server" text="Update" 
                        onclick="Unnamed1_Click" /><asp:button runat="server" text="Cancel" 
                        onclick="Unnamed2_Click" /></td>
            </tr>
        </table>
    </div>
    </asp:View>
    <asp:View runat="server" id = "View2">
        No Account Has Been Set Up For This Owner.
        <br />
        <asp:Button runat="server" Text="Close" onclick="Unnamed3_Click"></asp:Button>
    </asp:View>
    </asp:multiview>
    <asp:hiddenfield runat="server" id = "hfAcctID"></asp:hiddenfield>
    </form>
</body>
</html>
