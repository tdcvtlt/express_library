<%@ Page Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="editUnit.aspx.vb" Inherits="marketing_editUnit" Title="Editing A Unit"%>

<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>
<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc2" %>


<%@ Register src="../controls/Events.ascx" tagname="Events" tagprefix="uc3" %>


<asp:content ID="Content1" contentplaceholderid="head" runat=server>
    <script type = "text/javascript" language = "javascript">
        function Refresh_Ties()
        {
            __doPostBack('ctl00$ContentPlaceHolder1$UnitTies_Link', '');
        }
        function Refresh_Inventory() {
            __doPostBack('ctl00$ContentPlaceHolder1$SalesInventory_Link', '');
        }
    </script>
</asp:content>
<asp:content ID="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
    <ul id="menu">
        <li <%if  MultiView1.ActiveViewIndex = 0 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Units_Link" runat="server">Units</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 1 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="SalesInventory_Link" runat="server">SalesInventory</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 2 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Rooms_Link" runat="server">Rooms</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 3 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="UnitTies_Link" runat="server">Unit Ties</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 4 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Events_Link" runat="server">Events</asp:LinkButton></li>
    </ul>

        <asp:MultiView ID="MultiView1" runat="server">
                <asp:View ID="Unit_View" runat="server">
                    <table>
                        <tr>
                            <td>UnitID:</td>
                            <td><asp:TextBox ID="txtUnitID" runat="server" ReadOnly>0</asp:TextBox></td>
                            <td>Unit Name:</td>
                            <td><asp:TextBox ID="txtUName" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Address:</td>
                            <td><asp:TextBox ID="txtUAddress" runat="server"></asp:TextBox></td>
                            <td>City:</td>
                            <td><asp:TextBox ID="txtUCity" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>State:</td>
                            <td><uc2:Select_Item ID="siState" runat="server" /></td>
                            <td>Postal Code:</td>
                            <td><asp:TextBox ID="txtUZip" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>Type:</td>
                            <td><uc2:Select_Item ID="siUType" runat="server" /></td>
                            <td>SubType:</td>
                            <td><uc2:Select_Item ID="siUSubType" runat="server" /></td>
                        </tr>
                        <tr>
                            <td>Status:</td>
                            <td><uc2:Select_Item ID="siUStatus" runat="server" /></td>
                            <td>Style:</td>
                            <td><uc2:Select_Item ID="siUStyle" runat="server" /></td>
                        </tr>
                        <tr>
                            <td>Budgeted Price:</td>
                            <td><asp:TextBox ID="txtBudgetedPrice" runat="server" ReadOnly>0</asp:TextBox></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button runat="server" Text="Update" id="btnUpdate"></asp:Button>
                            </td>
                        </tr>
                    </table>
                    <asp:Label runat="server" Text="Label" id = "lblMessage"></asp:Label>
                    <asp:TextBox ID="txtRoomID" Visible="False" runat="server"></asp:TextBox>
                    <asp:TextBox ID="txtUnitRoomID" Visible="False" runat="server"></asp:TextBox>
                </asp:View>
                <asp:View ID="SalesInventory_View" runat="server">
                    <div style="height: 271px;overflow:auto">
                    <asp:GridView ID="gvSalesInventory" runat="server" EmptyDataText = "No Sales Inventory" AutoGenerateSelectButton="True">
                        <AlternatingRowStyle BackColor="#C7E3D7" />
                    </asp:GridView>
                    </div>
                    <asp:Button runat="server" Text="Add Sales Inventory" onclick="Unnamed1_Click1"></asp:Button>
                </asp:View>
                <asp:View ID="Rooms_View" runat="server">
                    <asp:GridView ID="gvRooms" runat="server" EmptyDataText = "No Rooms" AutoGenerateSelectButton="True">
                        <AlternatingRowStyle BackColor="#C7E3D7" />
                    </asp:GridView>
                    <asp:Button ID="btnAddRoom" runat="server" Text = "Add Room" />
                </asp:View>
                <asp:View ID="UnitTies_View" runat="server">
                    <asp:GridView ID="gvUnitTies" runat="server" EmptyDataText = "No Unit Ties" 
                        AutoGenerateColumns = "False" EnableModelValidation="True">
                        <AlternatingRowStyle BackColor="#C7E3D7" />
                        
                        <Columns>
                            <asp:BoundField DataField="Unit2UnitID" HeaderText="ID"></asp:BoundField>
                            <asp:BoundField DataField="Unit" HeaderText="Unit"></asp:BoundField>
                            <asp:ButtonField CommandName="Remove" Text="Remove"></asp:ButtonField>
                        </Columns>
                        
                    </asp:GridView>
                    <asp:Button runat="server" Text="Add Unit Tie" onclick="Unnamed1_Click"></asp:Button>
                </asp:View>
                <asp:View ID="Events_View" runat="server">
                    <uc3:Events ID="Events1" runat="server" />
                </asp:View>
                <asp:View runat="server" id = "DENIED">
                    ACCESS DENIED
                </asp:View>
            </asp:MultiView>
        
</asp:content>