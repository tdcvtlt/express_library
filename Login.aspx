<%' @ Page Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="Login.aspx.vb" Inherits="Login" title="Untitled Page" %>
<%@ Page Language="VB" AutoEventWireup="true" CodeFile="Login.aspx.vb" Inherits="Login"  AspCompat = "true"%>



<html>
<head><title>Login</title></head>
<%' <asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    '</asp:Content>
    '<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
%>
<body>
	<form id="Login" method="post" runat="server">
    <table>
        <tr>
            <td><asp:Label ID="Label1" runat="server" Text="User Name:"></asp:Label></td>
            <td><asp:TextBox ID="UserName" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td><asp:Label ID="Label2" runat="server" Text="Password:"></asp:Label></td>
            <td>
                <input id="Password" type="password" runat="server" /></td>
        </tr>
        <tr>
            <td colspan = "2">
                <asp:Button ID="Submit" runat="server" Text="Login" />
                <asp:Button ID="Reset" runat="server" Text="Clear Form" />
                <asp:HiddenField ID="referrer" Value="default.aspx" runat="server" />
            </td>            
        </tr>
    </table>
    <div id="sError"></div> 
    </form>
</body>
</html>
<%  '</asp:Content> %>

