<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SearchGuestRoom.aspx.vb" Inherits="PropertyManagement_SearchGuestRoom" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script language="javascript" type="text/javascript">
        function Get_Window(r)
        {
            alert(r);
        }
        /*function Refresh_Rpt(firstName, lastName, roomnumber) {
            document.getElementById('txtGuest').value = firstName + ' ' + lastName;
            document.getElementById('txtRoom').value = roomnumber;
        }*/
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
       <asp:GridView ID="GridView1" runat="server" AutoGenerateSelectButton="True" 
            EmptyDataText="No Records" GridLines="Horizontal">
        <SelectedRowStyle BackColor="#CCFFFF" Wrap="True" />
        <AlternatingRowStyle BackColor="#CCFFCC" />
    </asp:GridView>
        <asp:Label ID="lblErr" runat="server" Text=""></asp:Label>
    </div>
    </form>
</body>
</html>
