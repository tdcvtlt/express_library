<%@ Page Language="VB" AutoEventWireup="false" CodeFile="editUserField.aspx.vb" Inherits="general_editUserField" %>

<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server" >
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:MultiView ID="MultiView1" runat="server">
            <asp:View ID="vEdit" runat="server">
                <asp:Label ID="lblUFName" runat="server" Text="Label Update Edit"></asp:Label>
                <asp:TextBox ID="txtUFValue" runat="server"></asp:TextBox>
                &nbsp;<uc1:DateField ID="dteUFValue" runat="server" />
                <asp:CheckBox runat="server" ID="ckUFValue">
            </asp:CheckBox>
            
            </asp:View>
            <asp:View ID="vNew" runat="server">
                <asp:Label ID="lblUFName1" runat="server" Text="Label Update"></asp:Label>
                <asp:TextBox ID="txtUFValue1" runat="server"></asp:TextBox>
                <uc1:DateField ID="dteUFValue1" runat="server" />
                <asp:CheckBox runat="server" ID="ckUFValue1">
            </asp:CheckBox>
            </asp:View>
            <asp:View ID="vNewField" runat="server">
                <asp:Label ID="lblUFName2" runat="server" Text="Name: "></asp:Label>
                <asp:TextBox ID="txtUFName" runat="server"></asp:TextBox>
                <br />
                <uc2:Select_Item ID="siDataType" runat="server" />
            </asp:View>
        </asp:MultiView>
        <br />
        <asp:Button ID="btnSave"  runat="server" Text="Save" style="height: 26px" />
        
    </div>
    <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
    </form>
</body>
</html>
