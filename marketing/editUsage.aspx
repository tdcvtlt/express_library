<%@ Page Language="VB" AutoEventWireup="false" CodeFile="editUsage.aspx.vb" Inherits="marketing_editUsage" %>
<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc1" %>
<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc2" %>
<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc3" %>
<%@ Register src="../controls/Notes.ascx" tagname="Notes" tagprefix="uc5" %>

<%@ Register src="../controls/SyncDateField.ascx" tagname="SyncDateField" tagprefix="uc4"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Usage</title>
    <link type="text/css" rel="Stylesheet" href="../styles/master.css" />
    <script language=javascript type ="text/javascript">
        function Refresh_Notes() {
            __doPostBack('LinkButton3', '');
        }
    </script>
</head>

<body>
    <form id="form1" runat="server">
    <asp:scriptmanager runat="server"></asp:scriptmanager>

  <asp:multiview runat="server" id="MultiView1">
   <asp:View runat="server" id = "ViewA">
    <asp:multiview runat="server" id="MultiView2">
        <asp:view runat ="server" id="ViewC">
            <ul id = "menu">
            <li><asp:LinkButton id="LinkButton1" runat="server">Usage Details</asp:LinkButton></li>
            <li><asp:LinkButton id="LinkButton2" runat="server">Rooms</asp:LinkButton></li>
            <li><asp:LinkButton id="LinkButton3" runat="server">Notes</asp:LinkButton></li>
            <li><asp:LinkButton id="LinkButton4" runat="server">Events</asp:LinkButton></li>
            <li><asp:LinkButton id="LinkButton5" runat="server">Reservations</asp:LinkButton></li>
            </ul>
            <asp:MultiView runat="server" id = "MultiView3">
                <asp:View runat="server" id = "usageView">
                    <table>
                        <tr>
                            <td>Owner: </td>
                            <td><asp:TextBox runat="server" id = "ownerTxt" readonly style="width: 128px"></asp:TextBox>
                                <asp:Button runat="server" Text="..." onclick="Unnamed2_Click2"></asp:Button></td>        
                            <td>Date Created:</td>
                            <td><asp:TextBox runat="server" id = "dateCreatedTxt" readonly></asp:TextBox></td>
                        </tr>    
                        <tr>
                            <td>Contract:</td>
                            <td><asp:DropDownList ID="ddContract" runat="server" AutoPostBack = "true" OnSelectedIndexChanged = "ddContract_SelectedIndexChanged"></asp:DropDownList></td>
                            <td>Usage Year:</td>
                            <td><asp:DropDownList ID="ddUsageYear" runat="server"></asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td>Type:</td>
                            <td>
                                <uc2:Select_Item ID="siType" runat="server" />
                            </td>
                            <td>Sub-Type:</td>
                            <td>
                                <uc2:Select_Item ID="siSubType" runat="server" />
            
                            </td>
                        </tr>
                        <tr>
                            <td>Category:</td>
                            <td>
                                <uc2:Select_Item ID="siCategory" runat="server" />
                            </td>
                            <td>Amount Promised:</td>
                            <td><asp:TextBox ID="amtPromisedTxt" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Inventory:</td>
                            <td>
                                <asp:DropDownList ID="ddInventory" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>Status:</td>
                            <td>
                                <uc2:Select_Item ID="siStatus" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>UnitType:</td>
                            <td>
                                <uc2:Select_Item ID="siUnitType" runat="server" />
                            </td>
                            <td>RoomType:</td>
                            <td>
                                <uc2:Select_Item ID="siRoomType" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>Days:</td>
                            <td>
                                <asp:DropDownList ID="ddDays" runat="server" autopostback = "true" onSelectedIndexChanged = "ddDays_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>Points:</td>
                            <td>
                                <asp:TextBox ID="pointsTxt" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">In-Date:</td>
                            <td><div style="display:none;">
                                <asp:TextBox runat="server" id = "inDateTxt" readonly></asp:TextBox>
                                <asp:Button runat="server" Text="..." onclick="Unnamed2_Click1"></asp:Button><asp:Calendar ID="Calendar1" runat="server" visible = "False"></asp:Calendar>
                                </div>
                                <uc4:SyncDateField ID="SyncDateField1" runat="server" />
                            </td>
                            <td>Out-Date:</td>
                            <td><asp:TextBox runat="server" id = "outDateTxt" readonly></asp:TextBox></td>
                        </tr>
                        <tr><td><asp:Button runat="server" Text="Save" id = "btnSave" 
                                onclick="btnSave_Click" Height="26px"></asp:Button></td><td>
                            <asp:Button runat="server" Text="Save & Close" ID="Unnamed5" onclick="Unnamed5_Click"></asp:Button></td>
                            <td>
                                <asp:LinkButton ID="LinkButton6" runat="server">Print Confirmation Letter</asp:LinkButton>
                                </td>    
                        </tr>
                    </table>
                </asp:View>
                <asp:View runat="server" id = "roomsView">
                          <asp:GridView id = "gvRooms" runat="server" EnableModelValidation="True" Autogeneratecolumns = "False" emptyDataText = "No Records" OnRowDataBound = "gvRooms_RowDataBound">
                            <Columns>
                                <asp:BoundField HeaderText="RoomID" DataField="RoomID"></asp:BoundField>
                                <asp:BoundField HeaderText="Room" DataField="RoomNumber"></asp:BoundField>
                                <asp:BoundField HeaderText="Style" DataField="Style"></asp:BoundField>
                                <asp:BoundField HeaderText="Removable" DataField="Removable"></asp:BoundField>
                                <asp:ButtonField CommandName="RemoveRoom" Text="Remove"></asp:ButtonField>
                            </Columns>
                        </asp:GridView>
                        <asp:Button runat="server" Text="Add Room" onclick="Unnamed2_Click"></asp:Button>
                        <br /><br />
                        <asp:Label ID="lblAddRoom" runat="server" visible = "False">Select a Room to Add:</asp:Label>
                        <asp:GridView id = "gvAddRooms" runat="server" visible = "false" 
                            AutoGenerateSelectButton="True" EmptyDataText="No Rooms to Add" OnRowDataBound = "gvAddRooms_RowDataBound"></asp:GridView>
                </asp:View>
                <asp:View runat="server" id = "notesView">
                    <!--<uc5:Notes ID="Notes1" runat="server" KeyField="UsageID" />-->
                    <asp:GridView runat="server" id = "gvNotes" EmptyDataText="No Records" ></asp:GridView>
                    <asp:Button runat="server" Text="Add Note" onclick="Unnamed6_Click"></asp:Button>
                </asp:View>    
                <asp:View runat="server" id = "eventsView">
                    <asp:GridView runat="server" id = "gvEvents" EmptyDataText="No Records"></asp:GridView>
                </asp:View>
                <asp:View runat="server" id = "reservationsView">
                    <asp:GridView ID="gvReservations" runat="server"  EmptyDataText = "No Records" onrowDataBound = "gvReservations_RowDataBound" 
                        AutoGenerateSelectButton="True" >
                    </asp:GridView>
                </asp:View>
                <asp:View runat="server" id = "addNoteView">
                    <table>
                        <tr>
                            <td>Note Record:</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtNote" runat="server" Height="128px" Width="232px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button runat="server" onclick="Unnamed7_Click" Text="Add Note" />
                                <asp:Button runat="server" Text="Cancel" onclick="Unnamed8_Click"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </asp:View>
           </asp:multiview>
        </asp:View>
    </asp:multiview>
   </asp:View>
        <asp:View runat="server" id = "ViewB">
            Search By Name (Last Name, FirstName): <asp:textbox runat="server" id = "filterTxt"></asp:textbox> 
            <asp:button runat="server" id = "btnSearch" text="Search" onclick="btnSearch_Click" />
            <div style="height:410px; width:600px;overflow:auto; ">
            <asp:gridview runat="server" id = "gvOwners" AutoGenerateSelectButton="True" 
                EmptyDataText="No Records"></asp:gridview>
            </div>
        </asp:View>
  </asp:multiview>
    <asp:label runat="server" id = "lblErr"></asp:label>
    </form>
</body>
</html>
