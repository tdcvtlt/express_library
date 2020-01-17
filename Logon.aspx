<%@ Page Language="VB" AutoEventWireup="true" CodeFile="Logon.aspx.vb" Inherits="Logon"  AspCompat = "true"%>



<html>
<head><title>Login</title></head>
	<body onload="document.forms[0].txtUsername.focus();">
		<form id="Login" method="post" runat="server">
		    <table>
		        <tr>
		            <td><asp:Label ID="Label2" Runat="server">Username:</asp:Label></td>
		            <td><asp:TextBox ID="txtUsername" Runat="server"></asp:TextBox></td>
		        </tr>
		        <tr>
		            <td><asp:Label ID="Label3" Runat="server">Password:</asp:Label></td>
		            <td><asp:TextBox ID="txtPassword" Runat="server" TextMode="Password"></asp:TextBox></td>
		        </tr>
		        <tr>
		            <td><asp:Label ID="Label1" Runat="server">Domain:</asp:Label></td>
		            <td><asp:TextBox ID="txtDomain" Runat="server">KCP</asp:TextBox></td>
		        </tr>
		        <tr>
		            <td colspan="2"><asp:Button ID="btnLogin" Runat="server" Text="Login" OnClick="Login_Click"></asp:Button></td>
		        </tr>
		    </table>
			<asp:Label ID="errorLabel" Runat="server" ForeColor="#ff3300"></asp:Label><br />
			<asp:CheckBox Visible="false" ID="chkPersist" Runat="server" Text="Persist Cookie" />
		</form>
	</body>
</html>


