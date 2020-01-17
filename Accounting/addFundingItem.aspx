<%@ Page Language="VB" AutoEventWireup="false" CodeFile="addFundingItem.aspx.vb" Inherits="Accounting_addFundingItem" %>

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
                <td>KCP #:</td>
                <td><asp:textbox runat="server" id = "txtKCP"></asp:textbox></td>
            </tr>
            <tr>
                <td>Presales:</td>
                <td><asp:checkbox runat="server" id = "chkPreSales"></asp:checkbox></td>
            </tr>
            <tr>
                <td colspan = '2'><asp:button runat="server" text="Submit To Funding" 
                        onclick="Unnamed1_Click" /></td>
            </tr>
        </table>
    </div>
    <asp:label runat="server" id = "lblErr"></asp:label>
    </form>
</body>
</html>
