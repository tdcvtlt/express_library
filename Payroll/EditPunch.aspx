<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditPunch.aspx.vb" Inherits="Payroll_EditPunch" %>
<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>
<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style2
        {
            width: 185px;
        }
        .style3
        {
            width: 87px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server"><asp:scriptmanager runat="server"></asp:scriptmanager>
    <div>
        <table style="width: 388px; margin-right: 30px">
            <tr>
                <td class="style3">Date In:</td>
                <td class="style2"><uc1:DateField ID="dteInDate" runat="server"></uc1:DateField></td>
                <td><asp:button runat="server" text="Clear" onclick="Unnamed5_Click" /></td>
            </tr>
            <tr>
                <td class="style3">Time In:</td>
                <td class="style2"><asp:dropdownlist runat="server" id = "ddTIHour"></asp:dropdownlist><asp:dropdownlist runat="server" id = "ddTIMinute"></asp:dropdownlist><asp:dropdownlist runat="server" id = "ddTIAMPM"></asp:dropdownlist></td>
            </tr>
            <tr>
                <td class="style3">Date Out:</td>
                <td class="style2"><uc1:DateField ID="dteOutDate" runat="server" /></td>
                <td><asp:button runat="server" text="Clear" onclick="Unnamed9_Click" /></td>
            </tr>
            <tr>
                <td class="style3">Time Out:</td>
                <td class="style2"><asp:dropdownlist runat="server"  id ="ddTOHour"></asp:dropdownlist><asp:dropdownlist runat="server" id = "ddTOMinute"></asp:dropdownlist><asp:dropdownlist runat="server" id = "ddTOAMPM"></asp:dropdownlist></td>
            </tr>
            <tr>
                <td class="style3">Department:</td>
                <td class="style2">
                    <asp:DropDownList ID="ddDept" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="style3">Punch Type:</td>
                <td class="style2">
                    <uc2:Select_Item ID="siPunchType" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="style3"><asp:button runat="server" text="Save" 
                        onclick="Unnamed12_Click" /></td>
                <td class="style2"><asp:button runat="server" text="Cancel" 
                        onclick="Unnamed1_Click" /></td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
