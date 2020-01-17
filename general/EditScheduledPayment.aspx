<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditScheduledPayment.aspx.vb" Inherits="general_EditScheduledPayment" %>

<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">    <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    <div>
        <table>
            <tr>
                <td>
                    Payment Method:</td>
                <td>
                    <asp:label runat="server" id = "lblPM"></asp:label>
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
                    Scheduled Date:</td>
                <td>
                    
                    <uc1:DateField ID="dteScheduledDate" runat="server" />
                    
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
