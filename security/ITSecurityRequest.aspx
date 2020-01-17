<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ITSecurityRequest.aspx.vb" Inherits="security_ITSecurityRequest" %>

<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <table>
        <tr>
            <td>Security Request:</td>
            <td><asp:DropDownList ID="ddReqType" runat="server" OnSelectedIndexChanged = "ddReqType_SelectedIndexChanged" AutoPostBack = "true">
                <asp:ListItem Value="New">New User</asp:ListItem>
                <asp:ListItem Value="Edit">Edit User</asp:ListItem>
                <asp:ListItem Value="Disable">Disable User</asp:ListItem>
                </asp:DropDownList></td>
        </tr> 
        <tr>
            <td>Requested Due Date:</td>
            <td>
                <uc1:DateField ID="dteDueDate" runat="server" />
            </td>
        </tr>

        </table>
        <asp:MultiView ID="MultiView1" runat="server">
            <asp:View ID="View1" runat="server">
                <table>
                    <tr>
                        <td>CRMS Security Groups:</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:ListBox ID="lbCRMSGroups" runat="server" Height="142px" Width="190px" SelectionMode="Multiple"></asp:ListBox>
                        </td>
                        <td>
                            <asp:Button ID="Button2" runat="server" Text="ADD" Width="129px" /><br /><br /><br />
                            <asp:Button ID="Button3" runat="server" Text="REMOVE" Width="126px" />
                        </td>
                        <td>
                            <asp:ListBox ID="lbCRMSGroupsAdd" runat="server" Height="142px" Width="190px" 
                                SelectionMode="Multiple"></asp:ListBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Windows Security Groups:</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:ListBox ID="lbWindowsGroups" runat="server" Height="142px" Width="190px" 
                                SelectionMode="Multiple"></asp:ListBox>
                        </td>
                        <td>
                            <asp:Button ID="Button4" runat="server" Text="ADD" Width="129px" /><br /><br /><br />
                            <asp:Button ID="Button5" runat="server" Text="REMOVE" />
                        </td>
                        <td>
                            <asp:ListBox ID="lbWindowsGroupsAdd" runat="server" Height="142px" 
                                Width="190px" SelectionMode="Multiple"></asp:ListBox>
                        </td>
                    </tr>
                </table>
                <asp:Table ID="Table1" runat="server">
                    
                    <asp:TableRow runat="server">

                        <asp:TableCell runat="server">Email Address:</asp:TableCell>
                        <asp:TableCell runat="server" ColumnSpan = "2"><asp:RadioButtonList ID="rbEmailType" runat="server" 
                                RepeatDirection="Horizontal" AutoPostBack = "true" OnSelectedIndexChanged = "rbEmailType_SelectedIndexChanged"><asp:ListItem Selected="True">None</asp:ListItem>
<asp:ListItem> Individual</asp:ListItem>
<asp:ListItem>Distribution Group</asp:ListItem>
<asp:ListItem>Both</asp:ListItem>
</asp:RadioButtonList>
</asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow runat="server" Visible="False">
                        <asp:TableCell ID="TableCell1" runat="server">Email Domain:</asp:TableCell>
                        <asp:TableCell ID="TableCell2" runat="server"><asp:DropDownList ID="ddEmailDomain" runat="server"></asp:DropDownList>
</asp:TableCell>
                        <asp:TableCell ID="TableCell3" runat="server"></asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow runat="server" Visible="False">
                        <asp:TableCell ID="TableCell4" ColumnSpan = "2" runat="server">Distribution Groups:</asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow runat="server" Visible="False">
                        <asp:TableCell runat="server"><asp:ListBox ID="lbDistGroups" runat="server" Height="142px" Width="190px" SelectionMode="Multiple"></asp:ListBox>
</asp:TableCell>
                        <asp:TableCell runat="server"><asp:Button ID="Button6" runat="server" Text="ADD" Width="129px" />
<br /><br /><br />
                            <asp:Button ID="Button7" runat="server" Text="REMOVE" />
</asp:TableCell>
                        <asp:TableCell runat="server"><asp:ListBox ID="lbDistGroupsAdd" runat="server" Height="142px" Width="190px" SelectionMode="Multiple"></asp:ListBox>
</asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <asp:Table ID="Table2" runat="server">

                    <asp:TableRow runat="server">
                        <asp:TableCell runat="server">Phone Setup:</asp:TableCell>
                        <asp:TableCell runat="server">
                            <asp:DropDownList ID="ddPhoneSetup" runat="server" OnSelectedIndexChanged = "ddPhoneSetup_SelectedIndexChanged" AutoPostBack = "true">
                            </asp:DropDownList>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow runat="server">
                        <asp:TableCell runat="server">Outside Line:</asp:TableCell>
                        <asp:TableCell runat="server">
                            <asp:CheckBox ID="cbDID" runat="server" /></asp:TableCell>
                    </asp:TableRow>

                </asp:Table>

            </asp:View>
            <asp:View ID="View2" runat="server">
            </asp:View>
            <asp:View ID="View3" runat="server">
                
            </asp:View>
        </asp:MultiView>
    </div>
    <asp:Button ID="btnSave" runat="server" Text="Submit" />
    </form>
</body>
</html>
