<%@ Page Language="VB" AutoEventWireup="false" CodeFile="removebatchitem.aspx.vb" Inherits="wizards_Accounting_removebatchitem" %>

<%@ Register Src="~/controls/Select_Item.ascx" TagPrefix="uc1" TagName="Select_Item" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td>Remove Contract:</td>
                <td><asp:TextBox ID="txtContractID" runat="server" ReadOnly="true"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Reason:</td>
                <td>
                    <uc1:Select_Item runat="server" ID="siReason" />
                </td>
            </tr>
            <tr>
                <td>New Contract Status:</td>
                <td>
                    <uc1:Select_Item runat="server" ID="siCS" />
                </td>
            </tr>
            <tr>
                <td>New Contract Sub-Status:</td>
                <td>
                    <uc1:Select_Item runat="server" ID="siCSS" />
                </td>
            </tr>
            <tr>
                <td>New Mortgage Status:</td>
                <td>
                    <uc1:Select_Item runat="server" ID="siMS" />
                </td>
            </tr>
            <tr>
                <td>New Maintenance Fee Status:</td>
                <td>
                    <uc1:Select_Item runat="server" ID="siMFS" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnSave" runat="server" Text="Remove" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblError" ForeColor="Red" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
