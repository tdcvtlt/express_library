<%@ Page Language="VB" AutoEventWireup="false" CodeFile="editUsageOwner.aspx.vb" Inherits="marketing_editUsageOwner" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
<asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    Search By Name (Last Name, FirstName): <asp:textbox runat="server" id = "filterTxt"></asp:textbox> 
    <asp:button runat="server" text="Search" onclick="Unnamed1_Click" />
    <div style="height:410px; width:600px;overflow:auto; ">
    <asp:gridview runat="server" id = "gvOwners" AutoGenerateSelectButton="True" 
        EmptyDataText="No Records"></asp:gridview>
    </div>
    </form>
</body>
</html>
