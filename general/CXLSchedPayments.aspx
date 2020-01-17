<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CXLSchedPayments.aspx.vb" Inherits="general_CXLSchedPayments" %>

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
                <td>Reason:</td>
                <td><asp:textbox runat="server" id = "txtReason"></asp:textbox></td>
            </tr>
            <tr>
                <td colspan = '2'><asp:button runat="server" text="Cancel Payments" 
                        onclick="Unnamed1_Click" /></td>
            </tr>
        </table>
    </div>
    <asp:gridview runat="server" id = "gvSchedPayments">
        <AlternatingRowStyle BackColor="#C7E3D7" />
        <Columns>
            <asp:templatefield>
                <ItemTemplate>
                    <asp:CheckBox ID="cbID" runat="server" />
                </ItemTemplate>
            </asp:templatefield>
        </Columns>
    </asp:gridview>
    </form>
</body>
</html>
