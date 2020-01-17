<%@ Page Language="VB" AutoEventWireup="false" CodeFile="maintenancefeecode2fintrans.aspx.vb" Inherits="Accounting_maintenancefeecode2fintrans" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td>Maintenance Fee Code:</td>
                <td>
                    <asp:TextBox ID="txtCode" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Transaction Code:</td>
                <td>
                    <asp:DropDownList ID="ddFinTrans" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Amount:</td>
                <td>
                    <asp:TextBox ID="txtAmount" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="btnSave" runat="server" Text="Save" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
