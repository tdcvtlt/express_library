<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditUsageRestrictor.aspx.vb" Inherits="marketing_EditUsageRestrictor" %>
<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>
<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style = "width:30%;height:400px;float:left;">
    <B>Select Usage Restriction<B> <br />
    <asp:listbox runat="server" id = "lbRestrictions" Height="202px" Width="190px"></asp:listbox>
    <br />
        <asp:button runat="server" text="Select" id = "btnSelect" /><asp:button runat="server" text="Insert" id = "btnInsert" /><asp:button runat="server" text="Edit" id = "btnEdit" /><asp:button runat="server" text="Delete" id = "btnDelete" />
        <asp:label runat="server" id = "lblErr"></asp:label>
    </div>
    <div style = "width:70%;height:400px;float:right;overflow:auto;">
        <asp:table runat="server" id = "Table1" BorderStyle="Solid" visible = "False">
            <asp:TableRow runat="server" BorderStyle="Solid" BorderWidth="2px">
                <asp:TableCell runat="server" BorderStyle="Solid" BorderWidth="2px">Name:</asp:TableCell>
                <asp:TableCell runat="server" BorderStyle="Solid" BorderWidth="2px"><asp:TextBox runat="server" id = "txtName"></asp:TextBox></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow runat="server" BorderStyle="Solid" BorderWidth="2px">
                <asp:TableCell runat="server" HorizontalAlign="Right" BorderStyle="Solid" BorderWidth="2px"><asp:CheckBox runat="server" id = "chkDenial"></asp:CheckBox></asp:TableCell>
                <asp:TableCell runat="server" BorderStyle="Solid" BorderWidth="2px">General Denial</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow runat="server" BorderStyle="Solid" BorderWidth="2px">
                <asp:TableCell runat="server" HorizontalAlign="Right" BorderStyle="Solid" BorderWidth="2px"><asp:CheckBox runat="server" id = "chkMinimum" oncheckedchanged="chkMinimum_CheckedChanged" autoPostBack = "True"></asp:CheckBox></asp:TableCell>
                <asp:TableCell runat="server" BorderStyle="Solid" BorderWidth="2px">Require Minimum Stay</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow runat="server" BorderStyle="Solid" BorderWidth="2px" visible = "false">
                <asp:TableCell runat="server" BorderStyle="Solid" BorderWidth="2px"></asp:TableCell>
                <asp:TableCell runat="server" BorderStyle="Solid" BorderWidth="2px">Minimum Stay: <asp:TextBox runat="server" id = "txtMinStay" size = "4"></asp:TextBox></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow runat="server" BorderStyle="Solid" BorderWidth="2px">
                <asp:TableCell runat="server" HorizontalAlign="Right" BorderStyle="Solid" BorderWidth="2px"><asp:CheckBox runat="server" id = "chkAllowDate" OnCheckedChanged="chkAllowDate_CheckedChanged" autoPostBack = "True"></asp:CheckBox></asp:TableCell>
                <asp:TableCell runat="server" BorderStyle="Solid" BorderWidth="2px">Allow Date Range</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow runat="server" BorderStyle="Solid" BorderWidth="2px" visible = "false">
                <asp:TableCell runat="server" BorderStyle="Solid" BorderWidth="2px"></asp:TableCell>
                <asp:TableCell runat="server" BorderStyle="Solid" BorderWidth="2px">Start Date:<uc1:DateField ID="dteAllowsDate" runat="server" /></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow runat="server" BorderStyle="Solid" BorderWidth="2px" visible = "false">
                <asp:TableCell runat="server" BorderStyle="Solid" BorderWidth="2px"></asp:TableCell>
                <asp:TableCell runat="server" BorderStyle="Solid" BorderWidth="2px">End Date:<uc1:DateField ID="dteAlloweDate" runat="server" /></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow runat="server" BorderStyle="Solid" BorderWidth="2px">
                <asp:TableCell runat="server" HorizontalAlign="Right" BorderStyle="Solid" BorderWidth="2px"><asp:CheckBox runat="server" id = "chkDenyDate" oncheckedchanged="chkDenyDate_CheckedChanged" autoPostBack = "True"></asp:CheckBox></asp:TableCell>
                <asp:TableCell runat="server" BorderStyle="Solid" BorderWidth="2px">Deny Date Range</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow runat="server" BorderStyle="Solid" BorderWidth="2px" visible = "false">
                <asp:TableCell runat="server" BorderStyle="Solid" BorderWidth="2px"></asp:TableCell>
                <asp:TableCell runat="server" BorderStyle="Solid" BorderWidth="2px">Start Date: <uc1:DateField ID="dteDenysDate" runat="server" /></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow runat="server" BorderStyle="Solid" BorderWidth="2px" visible = "false">
                <asp:TableCell runat="server" BorderStyle="Solid" BorderWidth="2px"></asp:TableCell>
                <asp:TableCell runat="server" BorderStyle="Solid" BorderWidth="2px">End Date:   <uc1:DateField ID="dteDenyeDate" runat="server" /></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow runat="server" BorderStyle="Solid" BorderWidth="2px">
                <asp:TableCell runat="server" HorizontalAlign="Right" BorderStyle="Solid" BorderWidth="2px"><asp:CheckBox runat="server" id = "chkRequireDays" oncheckedchanged="chkRequireDays_CheckedChanged" autoPostBack = "True"></asp:CheckBox></asp:TableCell>
                <asp:TableCell runat="server" BorderStyle="Solid" BorderWidth="2px">Require Days Out</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow runat="server" BorderStyle="Solid" BorderWidth="2px" visible = "false">
                <asp:TableCell runat="server" BorderStyle="Solid" BorderWidth="2px"></asp:TableCell>
                <asp:TableCell runat="server" BorderStyle="Solid" BorderWidth="2px">Min Days: <asp:TextBox runat="server" id = "txtMinDays" size = "4"></asp:TextBox></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow runat="server" BorderStyle="Solid" BorderWidth="2px" visible = "false">
                <asp:TableCell runat="server" BorderStyle="Solid" BorderWidth="2px"></asp:TableCell>
                <asp:TableCell runat="server" BorderStyle="Solid" BorderWidth="2px">Max Days: <asp:TextBox runat="server" id = "txtMaxDays" size = "4"></asp:TextBox></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow runat="server" BorderStyle="Solid" BorderWidth="2px">
                <asp:TableCell runat="server" HorizontalAlign="Right" BorderStyle="Solid" BorderWidth="2px"><asp:CheckBox runat="server" id = "chkDenySeason" oncheckedchanged="chkDenySeason_CheckedChanged" autoPostBack = "True"></asp:CheckBox></asp:TableCell>
                <asp:TableCell runat="server" BorderStyle="Solid" BorderWidth="2px">Deny Season</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow runat="server" BorderStyle="Solid" BorderWidth="2px" visible = "false">
                <asp:TableCell runat="server" BorderStyle="Solid" BorderWidth="2px"></asp:TableCell>
                <asp:TableCell runat="server" BorderStyle="Solid" BorderWidth="2px">Season: <uc2:Select_Item ID="siSeason" runat="server" /></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow runat="server" BorderStyle="Solid" BorderWidth="2px">
                <asp:TableCell runat="server" BorderStyle="Solid" BorderWidth="2px" colspan  = '2'><asp:Button id = "btnURestrictSave" runat="server" Text="Save"></asp:Button><asp:Button runat="server" Text="Cancel" id = "btnCXL"></asp:Button>
                </asp:TableCell>
            </asp:TableRow>
        </asp:table>
        
    </div>
    </form>
</body>
</html>
