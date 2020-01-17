<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditPayment.aspx.vb" Inherits="general_EditPayment" %>

<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc1" %>
<%@ Register src="../controls/username.ascx" tagname="username" tagprefix="uc2" %>
<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    
        <table class="style1">
            <tr>
                <td>
                    Payment Method:</td>
                <td>
                    <asp:label runat="server" id = "lblPM"></asp:label>
                    <asp:dropdownlist runat="server" id = "ddAdjustments"></asp:dropdownlist>
                </td>
            </tr>
            <tr>
                <td>
                    Description:</td>
                <td>
                    <asp:TextBox ID="txtDesc" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Pos=Refund, Neg=Payment</td>
                <td>
                    <asp:RadioButton ID="rbPos" runat="server" GroupName="PosNeg" 
                        Text="Positive " />
                    <asp:RadioButton ID="rbNeg" runat="server" GroupName="PosNeg" Text="Negative" />
                </td>
            </tr>
            <tr>
                <td>
                    Amount:</td>
                <td>
                    <asp:TextBox ID="txtAmount" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Reference:</td>
                <td>
                    <asp:TextBox ID="txtRef" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Transaction Date:</td>
                <td>
                    <asp:textbox runat="server" id = "txtTransDate" readonly></asp:textbox>
                </td>
            </tr>
            <tr>
                <td>
                    Created By:</td>
                <td>
                    <asp:textbox runat="server" id = "txtUserName" readonly></asp:textbox>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: center">
                    <asp:Button ID="btnSave" runat="server" Text="Save" />
                    <asp:Button ID="btnClose" runat="server" style="text-align: center" 
                        Text="Close" />
                </td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
