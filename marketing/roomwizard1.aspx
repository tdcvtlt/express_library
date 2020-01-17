<%@ Page Language="VB" AutoEventWireup="false" CodeFile="roomwizard1.aspx.vb" Inherits="marketing_roomwizard1" %>

<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc1" %>
<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <link type="text/css" rel="Stylesheet" href="../styles/master.css" />
<script language="javascript" type="text/javascript">
// <!CDATA[

function Radio1_onclick() { 
}

function Button1_onclick() {

}

// ]]>
function Button1_onclick() {
}

function Add_Room() {
    __doPostBack('LinkButton12', '');
}

</script>
</head>
<body>

    <form id="form1" runat="server">    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <ul id = "menu">
        <li><asp:LinkButton id="LinkButton2" runat="server">Room Filter</asp:LinkButton></li>
        <li><asp:LinkButton id="LinkButton1" runat="server">Owner Usage</asp:LinkButton></li>
    </ul>
    <asp:MultiView id="MultiView1" runat="server">
        <asp:View id="View1" runat="server">
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
            </asp:DropDownList> </td><td>Unit Type: </td><td><uc1:Select_Item id="siUnitType" runat="server" /></td><td> Inv Type: </td><td><uc1:Select_Item id="siInvType" runat="server" /></td> <Td>Spares: 
                <input id="chkSpares" type="checkbox" runat = "server"/> </Td><td>
                    <asp:Button id="Button2" runat="server" text="Search" Height="26px" /></td>
        </tr>
        </table>
        </asp:View>
        <asp:View id="View2" runat="server">
            <asp:MultiView id = "MultiView2" runat="server">
                <asp:View id = "View3" runat="server">
                    <ul id = "menu">
                        <li><asp:LinkButton id = "linkButton3" runat="server">Same Owner</asp:LinkButton></li>
                        <li><asp:LinkButton id = "linkButton4" runat="server">Different Owner</asp:LinkButton></li>        
                    </ul>  
                    <asp:MultiView id="MultiView3" runat="server">
                        <asp:View id = "view5" runat = "server">
                            Usage Year: <asp:DropDownList id = "dd1" runat="server"></asp:DropDownList>
                            <asp:Button ID="Button9" runat="server" Text="Search" />
                        </asp:view>
                        <asp:view id = "view6" runat="server">
                            <ul id = "menu">
                                <li><asp:LinkButton id = "linkButton10" runat="server">By Name</asp:LinkButton></li>
                                <li><asp:LinkButton id = "linkButton11" runat="server">By KCP</asp:LinkButton></li>
                            </ul>
                            <asp:multiview id = "MultiView4" runat = "server">
                                <asp:view id = "view7" runat="server">
                                    Usage Year: <asp:DropDownList id = "dd2" runat="server"></asp:DropDownList>
                                    Owner Name: <asp:TextBox id = "OwnerName" runat="server"></asp:TextBox>
                                    <asp:Button runat="server" Text="Search" onclick="Unnamed1_Click1"></asp:Button>
                                </asp:view>
                                <asp:view id ="view8" runat="server">
                                    Usage Year: <asp:DropDownList id="dd3" runat="server"></asp:DropDownList>
                                    KCP#: <asp:TextBox id = "KCPNum" runat="server"></asp:TextBox>
                                    <asp:Button ID="Button3" runat="server" Text="Search" style="height: 26px" />
                                </asp:view>
                            </asp:multiview>
                        </asp:view>
                    </asp:MultiView>      
                </asp:View>
                <asp:View id = "View4" runat="server">
                    <ul id = "menu">
                        <li><asp:LinkButton id = "linkButton5" runat="server">By Name</asp:LinkButton></li>
                        <li><asp:LinkButton id = "linkButton6" runat="server">II Number</asp:LinkButton></li>
                        <li><asp:LinkButton id = "linkButton7" runat="server">RCI Number</asp:LinkButton></li>
                        <li><asp:LinkButton id = "linkButton8" runat="server">ICE Number</asp:LinkButton></li>
                        <li><asp:LinkButton id = "linkButton9" runat="server">RCI Points Number</asp:LinkButton></li>
                    </ul>
                    <asp:MultiView id = "MultiView5" runat = "server">
                        <asp:View id = "View5A" runat = "server">
                            Usage Year: <asp:DropDownList id = "dd4" runat="server"></asp:DropDownList>
                            Owners Name: <asp:TextBox id="txtOwnerName" runat="server"></asp:TextBox>
                            <asp:Button ID="Button4" runat="server" Text="Search" />
                        </asp:View>
                        <asp:View id = "View5B" runat="server">
                            Usage Year: <asp:DropDownList id = "dd5" runat="server"></asp:DropDownList>
                            II Membership Number: <asp:TextBox id="txtIINumber" runat="server"></asp:TextBox>
                            <asp:Button ID="Button5" runat="server" Text="Search" />
                        </asp:View>
                        <asp:View id = "View5C" runat = "server">
                            Usage Year: <asp:DropDownList id = "dd6" runat="server"></asp:DropDownList>
                            RCI Membership Number: <asp:TextBox id="txtRCINumber" runat="server"></asp:TextBox>
                            <asp:Button ID="Button6" runat="server" Text="Search" />
                        </asp:View>
                        <asp:View id = "View5D" runat = "server">
                            Usage Year: <asp:DropDownList id = "dd7" runat="server"></asp:DropDownList>
                            ICE Membership Number: <asp:TextBox id="txtICENumber" runat="server"></asp:TextBox>
                            <asp:Button ID="Button7" runat="server" Text="Search" />
                        </asp:View>
                        <asp:View id = "View5E" runat = "server">
                            Usage Year: <asp:DropDownList id = "dd8" runat="server"></asp:DropDownList>
                            RCI Memership Number: <asp:TextBox id="txtRCIPoints" runat="server"></asp:TextBox>
                            <asp:Button ID="Button8" runat="server" Text="Search" />
                        </asp:View>
                    </asp:MultiView>
                </asp:View>
                <asp:View id = "ViewA" runat = "server">
                    Option Not Available:
                </asp:View>            
            </asp:MultiView>
        </asp:View>
    </asp:MultiView>
    <div>
        <asp:label runat="server" id = "lblRoomErr"></asp:label>
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
                <asp:BoundField DataField="Category" HeaderText = "Category"
                    SortExpression="Category"></asp:BoundField>

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
        <asp:Label ID = "labelErr" runat="server"></asp:Label>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        <asp:button id = "RoomSelectBtn" runat="server" text="Select Rooms" visible = "false"/>
    <asp:LinkButton ID="LinkButton12" runat="server"></asp:LinkButton>
    </ContentTemplate>
    </asp:UpdatePanel>
        <asp:label runat="server" id = "lblSpareErr"></asp:label>
    </div>
    
    
    
    </form>
</body>
</html>
