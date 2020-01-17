<%@ Page Language="VB" AutoEventWireup="false" CodeFile="editleadtemplatemapping.aspx.vb" Inherits="LeadManagement_editleadtemplatemapping" %>

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
                <td>Column Name:</td>
                <td>
                    <asp:DropDownList ID="ddColumnName" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Column Number:</td>
                <td>
                    <asp:DropDownList ID="ddColNum" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Convert Value:</td>
                <td>
                    <asp:CheckBox ID="ckConvert" runat="server" />
                </td>
            </tr>
            
        </table>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td>Old Value: </td>
                        <td>
                            <asp:TextBox ID="txtOld" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>New Value:</td>
                        <td>
                            <asp:TextBox ID="txtNew" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Button ID="btnAdd" runat="server" Text="Add" />
                            <asp:Button ID="btnUpdate" runat="server" Text="Update" />
                            <asp:Button ID="btnRemove" runat="server" Text="Remove" />
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="gvItems" runat="server" AutoGenerateSelectButton="True">
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
        <ul id="menu">
            <li>
                <asp:LinkButton ID="lbSave" runat="server">Save</asp:LinkButton></li>
            <li>
                <asp:LinkButton ID="lbClose" runat="server">Close</asp:LinkButton></li>
        </ul>
    </div>
    <asp:HiddenField ID="hfID" runat="server" Value="0" />
    <asp:HiddenField ID="hfLTID" runat="server" Value="0" />
    </form>
</body>
</html>
