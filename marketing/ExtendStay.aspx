<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ExtendStay.aspx.vb" Inherits="marketing_ExtendStay" %>
<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>
<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Extend Stay</title>
    <link type="text/css" rel="Stylesheet" href="../styles/master.css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager> New Check Out Date: <uc1:DateField id="dteExtend" runat="server" />
    <table>
        <tr>
        <td>
        BD: <asp:DropDownList id="ddBedrooms" runat="server">
            <asp:ListItem value = '1'>1</asp:ListItem>
            <asp:ListItem value = '1BD-DWN'>1BD-DWN</asp:ListItem>
            <asp:ListItem value = '1BD-UP'>1BD-UP</asp:ListItem>
            <asp:ListItem value = '2'>2</asp:ListItem>
            <asp:ListItem value = '3'>3</asp:ListItem>
            <asp:ListItem value = '4'>4</asp:ListItem>    
            </asp:DropDownList> </td><td>Unit Type: </td><td><uc2:Select_Item id="siUnitType" runat="server" /></td><td> Inv Type: </td><td><uc2:Select_Item id="siInvType" runat="server" /></td> <Td>Spares: 
                <input id="chkSpares" type="checkbox" runat = "server"/> </Td><td>
                    <asp:Button id="Button2" runat="server" text="Search" Height="26px" /></td>
        </tr>
        </table>
        <div style="height:300px;width:600px;overflow:auto;">
<asp:GridView ID="gvRooms" runat="server" EmptyDataText = "No Records" OnRowDataBound = "gvRooms_RowDataBound" 
            RowStyle-Wrap = "true" EnableModelValidation="True" AutoGenerateColumns="False">
            <Columns>
                <asp:TemplateField HeaderText="Select">
                    <ItemTemplate>
                        <asp:checkbox ID="RoomSelect" runat = "server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="RoomID" HeaderText="RoomID" SortExpression="RoomID">
                </asp:BoundField>

                <asp:BoundField DataField="RoomNumber" HeaderText="RoomNumber" 
                    SortExpression="RoomNumber"></asp:BoundField>
                <asp:BoundField DataField="RoomType" HeaderText="RoomType" 
                    SortExpression="RoomType"></asp:BoundField>
                <asp:BoundField DataField="RoomSubType" HeaderText="RoomSubType" 
                    SortExpression="RoomSubType"></asp:BoundField>

            </Columns>
<RowStyle Wrap="True"></RowStyle>
        </asp:GridView>
    <asp:label runat="server" id = "lblSpares" visible = "false"><br /><b>Spares:</b><br /></asp:label>
<asp:GridView ID="gvSpares" runat="server" EmptyDataText = "No Records"  
            RowStyle-Wrap = "true" EnableModelValidation="True">
            <Columns>
                <asp:TemplateField HeaderText="Select">
                    <ItemTemplate>
                        <asp:checkbox ID="SpareSelect" runat = "server" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        </div>
        <br />
    <asp:button id = "RoomSelectBtn" runat="server" text="Select Rooms" visible = "false"/>
    <asp:Label ID="lblErr" runat="server"></asp:Label>
    </form>
</body>
</html>
