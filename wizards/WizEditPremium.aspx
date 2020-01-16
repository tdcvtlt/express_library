<%@ Page Language="VB" AutoEventWireup="false" CodeFile="WizEditPremium.aspx.vb" Inherits="wizards_WizEditPremium" %>
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
                <td>PremiumIssued ID:</td>
                <td>
                    <asp:TextBox readonly ID="txtPremiumIssuedID" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Premium:</td>
                <td>
                    <asp:dropdownlist runat="server" id = "ddPremiumID" autopostback = "true"></asp:dropdownlist>
                </td>
            </tr>
            <tr>
                <td>Certificate Number:</td>
                <td>
                    <asp:TextBox ID="txtCertificate" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Qty Assigned:</td>
                <td>
                    <asp:dropdownlist runat="server" id = "qtyAssigned"></asp:dropdownlist>
                </td>
            </tr>
            <tr>
                <td>CostEA:</td>
                <td>
                    <asp:TextBox ID="txtCostEA" runat="server" ReadOnly = "true"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Status:</td>
                <td>
                    <uc1:Select_Item ID="siStatus" runat="server" />
                </td>
            </tr>
        </table>
        <asp:Button ID="btnSave" runat="server" Text="Save" />  
        <asp:label runat="server" text="" id = "lblPremErr"></asp:label>  
    </div>
    </form>
</body>
</html>
