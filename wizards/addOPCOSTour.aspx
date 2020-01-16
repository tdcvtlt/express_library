<%@ Page Language="VB" AutoEventWireup="false" CodeFile="addOPCOSTour.aspx.vb" Inherits="wizards_addOPCOSTour" %>
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
    <div>
        <table>
            <tr>
                <td>Tour Date:</td>
                <td><uc1:DateField ID="dteDateBooked" runat="server" /></td>
            </tr>
            <tr>
                <td>Tour Time:</td>
                <td><uc2:Select_Item ID="siTourTime" runat="server" /></td>
            </tr>
            <tr>
                <td>Booked By:</td>
                <td><asp:DropDownList ID="ddPersonnel" runat="server"></asp:DropDownList>
                </td>
            </tr>
        </table>
    </div>
    <div style="height:300px;overflow:auto">
        <asp:gridview runat="server" EmptyDataText = "No Records" id = "gvPremiums" AutoGenerateColumns = "False" onRowDataBound = "gvPremiums_RowDataBound">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:checkbox ID="chkPremium" runat = "server" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="PremiumID" DataField="PremiumID"></asp:BoundField>
            <asp:BoundField HeaderText="Premium" DataField="PremiumName"></asp:BoundField>
            <asp:TemplateField HeaderText="Qty">
                <ItemTemplate>
                    <asp:DropDownList runat="server" id = "ddQty"></asp:DropDownList>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Amt">
                <ItemTemplate>
                    <asp:TextBox runat="server" id = "txtAmt"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        </asp:gridview>
    </div>
    <asp:button runat="server" text="Add Tour" onclick="Unnamed1_Click"/>
    </form>
</body>
</html>
