<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditpackageTourPremium.aspx.vb" Inherits="setup_EditpackageTourPremium" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:RadioButtonList ID="RadioButtonList1" runat="server" 
            RepeatDirection="Horizontal" AutoPostBack="True">
            <asp:ListItem Selected = "True">Premium</asp:ListItem>
            <asp:ListItem>Premium Bundle</asp:ListItem>
        </asp:RadioButtonList>
        <asp:MultiView ID="MultiView1" runat="server">
            <asp:View ID="View1" runat="server">
                <table>
                    <tr>
                        <td>Premium:</td>
                        <td>
                            <asp:DropDownList ID="ddPremium" runat="server" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Status:</td>
                        <td>
                            <asp:DropDownList ID="ddStatus" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Qty:</td>
                        <td>
                            <asp:DropDownList ID="ddQty" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Cost EA:</td>
                        <td>
                            <asp:TextBox ID="txtCostEA" runat="server" ReadOnly="true"></asp:TextBox></td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="View2" runat="server">
                <table>
                    <tr>
                        <td>Premium Bundle:</td>
                        <td>
                            <asp:DropDownList ID="ddPremBundle" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </asp:View>
        </asp:MultiView>

        <div>
            <table>
                <tr><td>Optional:</td><td><asp:CheckBox ID="cbOptional" runat="server" /></td></tr>                
            </table>        
        </div>
        <p>
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" />
        </p>    
        
    </div>
    </form>
</body>
</html>
