<%@ Page Language="VB" AutoEventWireup="false" CodeFile="addSalesInventory.aspx.vb" Inherits="marketing_addSalesInventory" %>

<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc1" %>

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
                <td>Starting Week:</td>
                <td>
                    <asp:dropdownlist runat="server" id = "ddStartWeek"></asp:dropdownlist>
                </td>
            </tr>
            <tr>
                <td>Ending Week:</td>
                <td>
                    <asp:dropdownlist runat="server" id = "ddEndWeek"></asp:dropdownlist>
                </td>
            </tr>
            <tr>
                <td>Season:</td>
                <td>
                    <uc1:Select_Item ID="siSeason" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Budgeted Price:</td>
                <td>
                    <asp:textbox runat="server" id = "txtBudgetedPrice"></asp:textbox>
                </td>
            </tr>
            <tr>
                <td>Inventory Type:</td>
                <td>
                    <uc1:Select_Item ID="siInventoryType" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Points Value:</td>
                <td>
                    <asp:textbox runat="server" id = "txtPoints"></asp:textbox>
                </td>
            </tr>            
            <tr>
                <td>
                    <asp:Button ID="Button1" runat="server" Text="Submit" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
