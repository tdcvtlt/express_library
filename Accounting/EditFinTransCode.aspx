<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditFinTransCode.aspx.vb" Inherits="Accounting_EditFinTransCode" %>

<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        
        <table class="style1">
            <tr>
                <td>
                    ID:</td>
                <td>
                    <asp:TextBox ID="txtID" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Trans Code:</td>
                <td>
                    <uc1:Select_Item ID="siTransCode" runat="server" ComboItem="TransCode" />
                </td>
            </tr>
            <tr>
                <td>
                    Description:</td>
                <td>
                    <asp:TextBox ID="txtDesc" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Amount:</td>
                <td>
                    <asp:TextBox ID="txtAmount" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Transaction Type:</td>
                <td>
                    <uc1:Select_Item ID="siTransType" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    Merchant Account:</td>
                <td>
                    <asp:DropDownList ID="ddAccount" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    GL Account:</td>
                <td>
                    <asp:DropDownList ID="ddGL" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Active:</td>
                <td>
                    <asp:CheckBox ID="ckActive" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnSave" runat="server" Text="Save" />
                    <asp:Button ID="btnClose" runat="server" Text="Close" />
                </td>
            </tr>
        </table>
        
    </div>
    <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
    </form>
</body>
</html>
