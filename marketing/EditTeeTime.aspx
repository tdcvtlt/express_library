<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditTeeTime.aspx.vb" Inherits="marketing_EditTeeTime" %>

<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Src="~/controls/Select_Item.ascx" TagPrefix="uc1" TagName="Select_Item" %>
<%@ Register Src="~/controls/DateField.ascx" TagPrefix="uc1" TagName="DateField" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div>
        <table>
            <tr>
                <td>Golf Course:</td>
                <td>
                    <asp:DropDownList ID="ddCourse" runat="server"></asp:DropDownList></td>
            </tr>
            <tr>
                <td>Guests:</td>
                <td>
                    <asp:DropDownList ID="ddGuests" runat="server"></asp:DropDownList></td>
            </tr>
            <tr>
                <td>Status:</td>
                <td>
                    <uc1:Select_Item runat="server" ID="siStatus" />
                </td>
            </tr>
            <tr>
                <td>Date:</td>
                <td>
                    <uc1:DateField runat="server" ID="dteDate" />
                </td>
            </tr>
            <tr>
                <td>Time:</td>
                <td>
                    <asp:DropDownList ID="ddHour" runat="server"></asp:DropDownList>:<asp:DropDownList ID="ddMinutes" runat="server"></asp:DropDownList><asp:DropDownList ID="ddAMPM" runat="server"></asp:DropDownList></td>
            </tr>
            <tr>
                <td>Contact Number:</td>
                <td>
                    <asp:TextBox ID="txtPhone" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Booking Contact:</td>
                <td>
                    <asp:TextBox ID="txtBookingContact" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Cancel Contact:</td>
                <td>
                    <asp:TextBox ID="txtCXLContact" runat="server"></asp:TextBox></td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnSave" runat="server" Text="Submit" /></td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
