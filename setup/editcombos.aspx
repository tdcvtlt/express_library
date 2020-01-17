<%@ Page Language="VB" AutoEventWireup="false" CodeFile="editcombos.aspx.vb" Inherits="setup_editcombos" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
        <asp:MultiView ID="MultiView1" runat="server">
            <asp:View ID="vwCombo" runat="server">
                <table class="style1">
                    <tr>
                        <td>
                            Name:</td>
                        <td>
                            <asp:TextBox ID="txtComboName" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Description:</td>
                        <td>
                            <asp:TextBox ID="txtComboDescription" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;</td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="vwItem" runat="server">
                <table class="style1">
                    <tr>
                        <td>
                            Item:</td>
                        <td>
                            <asp:TextBox ID="txtItemName" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Description:</td>
                        <td>
                            <asp:TextBox ID="txtItemDescription" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Active:</td>
                        <td>
                            <asp:CheckBox ID="ckItemActive" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;</td>
                    </tr>
                </table>
            </asp:View>
        </asp:MultiView>
    
    </div>
    <p>
        <asp:Button ID="btnSave" runat="server" Text="Save" />
        <asp:Button ID="btnClose" runat="server" Text="Close" />
    </p>
    <asp:Label ID="lblErr" runat="server" ForeColor="Red"></asp:Label>
    </form>
</body>
</html>
