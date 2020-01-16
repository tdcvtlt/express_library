<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditPersonnelTrans.aspx.vb" Inherits="general_EditPersonnelTrans" %>

<%@ Register src="../../controls/UserFields.ascx" tagname="UserFields" tagprefix="uc1" %>
<%@ Register src="../../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc2" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .style2
        {
            width: 103px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <table class="style1">
        <tr>
            <td class="style2">
                Pers. Trans. ID:</td>
            <td>
                <asp:TextBox ID="txtPersonnelTransID" runat="server" ReadOnly="True" 
                    Width="76px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style2">
                Personnel:</td>
            <td>
                <asp:DropDownList ID="ddlPersonnel" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="style2" colspan="2">
                <uc2:Select_Item ID="Select_Item1" runat="server" ComboItem="PersonnelTitle" 
                    Label_Caption="Title:" />
            </td>
        </tr>
        <tr>
            <td class="style2">
                Fixed Amount:</td>
            <td>
                <asp:TextBox ID="txtFA" runat="server" Width="76px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="style2">
                Comm. Perc.:</td>
            <td>
                <asp:TextBox ID="txtCP" runat="server" Width="75px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                Date Created: <uc3:DateField ID="dfDateCreated" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                Date Posted: <uc3:DateField ID="dfDatePosted" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="style2">
                <asp:Button ID="btnSave" runat="server" Text="Save" />
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style2">
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
    </form>
</body>
</html>
