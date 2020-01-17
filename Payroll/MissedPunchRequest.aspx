<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MissedPunchRequest.aspx.vb" Inherits="Payroll_MissedPunchRequest" %>
<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:scriptmanager runat="server"></asp:scriptmanager>
    <b>Missed Punch Request</b><br />
    <asp:radiobuttonlist runat="server" RepeatDirection="Horizontal" id = "MPType" onSelectedIndexChanged = "MPType_SelectedIndexChanged" autopostback = "true">
        <asp:ListItem>Single Punch</asp:ListItem>
        <asp:ListItem>Entire Shift</asp:ListItem>
    </asp:radiobuttonlist>
    <asp:multiview runat="server" id = "MultiView1">
        <asp:View runat="server" id = "view_1">
            <table>
                <tr>
                    <td colspan = '2'><asp:RadioButtonList id = "cbPType" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem>Punch In</asp:ListItem>
                        <asp:ListItem>Punch Out</asp:ListItem>
                        </asp:RadioButtonList></td>
                </tr>
                <tr>
                    <td>Punch Date:</td>
                    <td>
                        <uc1:DateField runat="server" id = "dtePunchDate" />
                    </td>
                </tr>
                <tr>
                    <td>Punch Time:</td>
                    <td><asp:DropDownList runat="server" id = "ddPunchHour"></asp:DropDownList><asp:DropDownList runat="server" id = "ddPunchMinute"></asp:DropDownList><asp:DropDownList runat="server" id = "ddPunchAMPM"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td>Department:</td>
                    <td>
                        <asp:DropDownList ID="ddPunchDept" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>Reason:</td>
                    <td><asp:TextBox runat="server" id = "txtPunchReason"></asp:TextBox></td>
                </tr>
                <tr>
                    <td><asp:Button runat="server" Text="Submit" onclick="Unnamed1_Click"></asp:Button></td>
                </tr>
            </table>
        </asp:View>  
        <asp:View runat="server" id = "view_2">
            <table>
                <tr>
                    <td>Punch In Date:</td>
                    <td>
                        <uc1:DateField ID="dtePunchInDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Punch In Time:</td>
                    <td><asp:DropDownList runat="server" id = "ddPunchInHour"></asp:DropDownList><asp:DropDownList runat="server" id = "ddPunchInMinute"></asp:DropDownList><asp:DropDownList runat="server" id = "ddPunchInAMPM"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td>Punch Out Date:</td>
                    <td>
                        <uc1:DateField ID="dtePunchOutDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Punch Out Time:</td>
                    <td><asp:DropDownList runat="server" id = "ddPunchOutHour"></asp:DropDownList><asp:DropDownList runat="server" id = "ddPunchOutMinute"></asp:DropDownList><asp:DropDownList runat="server" id = "ddPunchOutAMPM"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td>Department:</td>
                    <td>
                        <asp:DropDownList ID="ddShiftDept" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>Reason:</td>
                    <td>
                        <asp:TextBox ID="txtShiftReason" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td><asp:Button runat="server" Text="Submit" onclick="Unnamed2_Click"></asp:Button></td>
                </tr>
            </table>
        </asp:View>
    </asp:multiview>
    <asp:label id = "lblErr" runat="server"></asp:label>
    </form>
</body>
</html>
