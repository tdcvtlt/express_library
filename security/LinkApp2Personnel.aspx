<%@ Page Language="VB" AutoEventWireup="false" CodeFile="LinkApp2Personnel.aspx.vb" Inherits="security_LinkApp2Personnel" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        Please Select Existing Personnel Record or Enter PersonnelID Below:</div>
        PersonnelID: <asp:TextBox ID="txtID" runat="server"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" Text="Submit" /><br />
        <asp:GridView ID="gvPersonnel" runat="server" AutoGenerateSelectButton ="true" EmptyDataText ="No Matching Records Found."></asp:GridView>
    
    </form>
</body>
</html>
