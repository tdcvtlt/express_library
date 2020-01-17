<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AddCalEvent.aspx.vb" Inherits="LeadManagement_AddCalEvent" %>

<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>
<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
    
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View1" runat="server">
            <div>
                <asp:RadioButtonList ID="rblEventType" runat="server" 
                    RepeatDirection="Horizontal">
                    <asp:ListItem>Individual</asp:ListItem>
                    <asp:ListItem>Department</asp:ListItem>
                </asp:RadioButtonList>
                <table>
                    <tr>
                        <td colspan="2">
                            <asp:DropDownList ID="ddWho" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">Date:</td>
                        <td><uc1:DateField ID="dfEventDate" runat="server" /></td>
                    </tr>
                    <tr>
                        <td valign="top">Type:</td>
                        <td align="left"><uc2:Select_Item ID="siEventType" runat="server" /></td>
                    </tr>
                    <tr>
                        <td valign="top">Description:</td>
                        <td><asp:TextBox ID="txtDesc" runat="server" Height="82px" TextMode="MultiLine" 
                                Width="316px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="btnSave" runat="server" Text="Save" />
                            <asp:Button ID="btnClose" runat="server" Text="Close" />
                        </td>
                    </tr>
                </table>
            </div>
        </asp:View>
        <asp:View ID="View2" runat="server">
            <table>
                <tr>
                    <td>Disposition:</td>
                    <td>
                        <uc2:Select_Item ID="Select_Item1" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Notes:</td>
                    <td>
                        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Terminate Lead</td>
                    <td>
                        <asp:CheckBox ID="CheckBox1" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Rep:</td>
                    <td>
                        <asp:DropDownList ID="DropDownList1" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>Follow Up Date:</td>
                    <td>
                        <uc1:DateField ID="DateField1" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Follow Up Method:</td>
                    <td>
                        <uc2:Select_Item ID="Select_Item2" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Next Activity Description:</td>
                    <td>
                        <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="btnComplete" runat="server" Text="Complete" />
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>
    </form>
</body>
</html>
