<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditPoints.aspx.vb" Inherits="general_EditPoints" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div style ="display:none">
            <table>
                <tr>
                    <td>ID:</td>
                    <td>
                        <asp:TextBox ID="txtID" ReadOnly="true" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>AvailYear:</td>
                    <td>
                        <asp:DropDownList ID="ddAvailYear" readonly="true" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>UsageYear:</td>
                    <td>
                        <asp:DropDownList ID="ddUsageYear" readonly="true" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>Expiration:</td>
                    <td>
                        <asp:TextBox ID="txtExp" ReadOnly="true" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>StayLoc:</td>
                    <td>
                        <asp:DropDownList ID="ddStayLoc" readonly="true" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </div>
    <div>
        <table>
            <tr>
                <td>TransType:</td>
                <td>
                    <asp:DropDownList ID="ddTransType" readonly="true" runat="server" Enabled="False">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>PosNeg:</td>
                <td>
                    <asp:RadioButtonList ID="rblPosNeg" runat="server" readonly="true" RepeatDirection="Horizontal" Enabled="False">
                        <asp:ListItem>Pos</asp:ListItem>
                        <asp:ListItem>Neg</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td>Points:</td>
                <td>
                    <asp:TextBox ID="txtPoints" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Comments:</td>
                <td>
                    <asp:TextBox ID="txtComments" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Confirmation Number:</td>
                <td>
                    <asp:TextBox ID="txtConf" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Created By:</td>
                <td>
                    <asp:TextBox ID="txtCreatedBy" ReadOnly="true" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>TransDate:</td>
                <td>
                    <asp:TextBox ID="txtTransDate" ReadOnly="true" runat="server"></asp:TextBox>
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
    </form>
</body>
</html>
