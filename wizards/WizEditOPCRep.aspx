<%@ Page Language="VB" AutoEventWireup="false" CodeFile="WizEditOPCRep.aspx.vb" Inherits="wizards_WizEditOPCRep" %>

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
                <td>Rep:</td>
                <td>
                    <asp:dropdownlist runat="server" id = "ddOPCRep"></asp:dropdownlist>
                </td>
            </tr>
            <tr>
                <td>Location:</td>
                <td>
                    <asp:dropdownlist runat="server" id = "ddOPCLoc"></asp:dropdownlist>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:button runat="server" text="Submit" onclick="Unnamed1_Click" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
