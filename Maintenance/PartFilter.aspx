<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PartFilter.aspx.vb" Inherits="Maintenance_PartFilter" %>
<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Styles/mainstyle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
    <div id = "contentleft">
        <asp:table runat="server" id = "Table1">
            <asp:TableRow runat="server" visible = "False">
                <asp:TableCell runat="server">Filter 0:</asp:TableCell>
                <asp:TableCell runat="server"><asp:dropdownlist runat="server" id = "ddFilter0" onSelectedIndexChanged = "ddFilter0_SelectedIndexChanged" autopostback = "true"></asp:dropdownlist></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow runat="server">
                <asp:TableCell runat="server">Filter 1:</asp:TableCell>
                <asp:TableCell runat="server"><asp:dropdownlist runat="server" id = "ddFilter1" onSelectedIndexChanged = "ddFilter1_SelectedIndexChanged" autopostback = "true"></asp:dropdownlist></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow runat="server">
                <asp:TableCell runat="server">Filter 2:</asp:TableCell>
                <asp:TableCell runat="server"><asp:dropdownlist runat="server" id = "ddFilter2" onSelectedIndexChanged = "ddFilter2_SelectedIndexChanged" autopostback = "true"></asp:dropdownlist></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow runat="server">
                <asp:TableCell runat="server">Filter 3:</asp:TableCell>
                <asp:TableCell runat="server"><asp:dropdownlist runat="server" id = "ddFilter3"></asp:dropdownlist></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow runat="server">
                <asp:TableCell runat="server">Qty Needed:</asp:TableCell>
                <asp:TableCell runat="server"><asp:dropdownlist runat="server" id = "ddQty"></asp:dropdownlist></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow runat="server" Visible="False">
                <asp:TableCell runat="server">Moved From:</asp:TableCell>
                <asp:TableCell runat="server"><asp:dropdownlist runat="server" id = "ddMoveFrom"></asp:dropdownlist></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow runat="server" Visible="False">
                <asp:TableCell runat="server">Res ID:</asp:TableCell>
                <asp:TableCell runat="server"><asp:TextBox runat="server" id = "txtReservationID"></asp:TextBox></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow runat="server" Visible="False">
                <asp:TableCell runat="server">Room:</asp:TableCell>
                <asp:TableCell runat="server"><asp:dropdownlist runat="server" id = "ddRooms"></asp:dropdownlist></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow runat="server" Visible="False">
                <asp:TableCell runat="server">Return Date:</asp:TableCell>
                <asp:TableCell runat="server"><uc1:DateField ID="dteReturnDate" runat="server" /></asp:TableCell>
            </asp:TableRow>
        </asp:table>
        <asp:button runat="server" text="Search" autopostback = "false" onclick="Unnamed3_Click" />
        <br />
        <asp:table runat="server" id = "Table2" visible = "False">
            <asp:TableRow runat="server">
                <asp:TableCell runat="server" colspan = '2'><b>Manual Entry:</b></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow runat="server">
                <asp:TableCell runat="server">Part Number:</asp:TableCell>
                <asp:TableCell runat="server"><asp:TextBox runat="server" id = "txtItem"></asp:TextBox></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow runat="server">
                <asp:TableCell runat="server">Qty Needed:</asp:TableCell>
                <asp:TableCell runat="server"><asp:dropdownlist runat="server" id = "ddManualQty"></asp:dropdownlist></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow runat="server">
                <asp:TableCell runat="server">Cost EA:</asp:TableCell>
                <asp:TableCell runat="server"><asp:TextBox runat="server" id = "txtCostEA"></asp:TextBox></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow runat="server">
                <asp:TableCell runat="server"><asp:Button runat="server" Text="Submit" id = "btnManual" onclick="btnManual_Click"></asp:Button></asp:TableCell>
            </asp:TableRow>
        </asp:table>
        <asp:label runat="server" id = "lblErr"></asp:label>
    </div>
    <div id = "contentright" style="top: auto"><asp:gridview runat="server" 
            id = "gvParts" EmptyDataText = "No Records" AutoGenerateSelectButton="True"></asp:gridview></div>
    

    </form>
</body>
</html>
