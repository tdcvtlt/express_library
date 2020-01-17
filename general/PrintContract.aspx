<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PrintContract.aspx.vb" Inherits="general_PrintContract" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Creating Contract Documents</title>
    
</head>
<body>
    <OBJECT ID="ScriptControl1" WIDTH=39 HEIGHT=39
		 CLASSID="CLSID:0E59F1D5-1FBE-11D0-8FF2-00A0D10038BC">
		    <PARAM NAME="_ExtentX" VALUE="1005">
		    <PARAM NAME="_ExtentY" VALUE="1005">	
	</OBJECT>
    <form id="form1" runat="server">
    <div>
        Select Document Group:
        <asp:GridView ID="gvGroups" runat="server" AutoGenerateSelectButton="true">
            <AlternatingRowStyle BackColor="#C7E3D7" />
        </asp:GridView>
    
        <asp:Literal ID="Lit" runat="server"></asp:Literal>
    </div>
    </form>
</body>
</html>
