<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EditAccommodation.aspx.vb" Inherits="setup_EditAccommodation" %>

<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc1" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script language=javascript type ="text/javascript">
        function Refresh_Resort()
        {
            __doPostBack('ctl00$ContentPlaceHolder1$LinkButtonResortRoomTypes', '');
        }
        function Refresh_Hotel() {
            __doPostBack('ctl00$ContentPlaceHolder1$LinkButtonHotelRoomTypes', '');

        }
        function Refresh_CheckIns() {
            __doPostBack('ctl00$ContentPlaceHolder1$LinkButtonCheckIn', '');

        }
    </script>
    <style type="text/css">
        .style1
        {
            width: 605px;
        }
        .style2
        {
            width: 155px;
        }
        .style3
        {
            width: 168px;
        }
        .style4
        {
            width: 96px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ul id="menu">
    <li <% if MultiView1.ActiveViewIndex = 0 then: response.write("class=""current"""):end if %>>
        <asp:LinkButton ID="LinkButtonAccom" runat="server">Accommodation</asp:LinkButton>
    </li>    
    
    <li <% if MultiView1.ActiveViewIndex = 1 then: response.write("class=""current"""):end if %>>
        <asp:LinkButton ID="LinkButtonResortRoomTypes" runat="server">Resort Room Types</asp:LinkButton>        
    </li>    

    <li <% if MultiView1.ActiveViewIndex = 2 then: response.write("class=""current"""):end if %>>
        <asp:LinkButton ID="LinkButtonHotelRoomTypes" runat="server">Hotel Room Types</asp:LinkButton>        
    </li>    

    <li <% if MultiView1.ActiveViewIndex = 3 then: response.write("class=""current"""):end if %>>
        <asp:LinkButton ID="LinkButtonDirections" runat="server">Directions</asp:LinkButton>        
    </li>    
        <li <% if MultiView1.ActiveViewIndex = 4 then: response.write("class=""current"""):end if %>>
        <asp:LinkButton ID="LinkButtonCheckIn" runat="server">Check-In Locations</asp:LinkButton>        
    </li>    
</ul>

    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View1" runat="server">
            <table class="style1">
                <tr>
                    <td class="style2">AccommodationID:</td>
                    <td class="style3"><asp:TextBox ID="txtAccomID" runat="server" ReadOnly="true"></asp:TextBox></td>
                    <td class="style4">Name:</td>
                    <td class="style3"><asp:TextBox ID="txtAccomName" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Location</td>
                    <td><uc1:Select_Item ID="siAccomLoc" runat="server" /></td>
                    <td class="style2">URL</td>
                    <td class="style3"><asp:TextBox ID="txtURL" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="style2">Description</td>
                    <td colspan = "3"><asp:TextBox ID="txtDesc" runat="server" Width="412px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="style2">Online Description</td>
                    <td colspan = "3"><asp:TextBox ID="txtOnlineDesc" runat="server" Width="412px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="style2">Address</td>
                    <td class="style3"><asp:TextBox ID="txtAddress" runat="server"></asp:TextBox></td>
                    <td class="style4">City</td>
                    <td class="style3"><asp:TextBox ID="txtCity" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="style2">State</td>
                    <td class="style3"><uc1:Select_Item ID="siState" runat="server" />
                    </td>
                    <td class="style4">Postal Code</td>
                    <td class="style3"><asp:TextBox ID="txtPostal" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Active:</td>
                    <td>
                        <asp:CheckBox ID="cbActive" runat="server" /></td>
                </tr>
                <tr>
                    <td class="style2"><asp:Button ID="btnSave" runat="server" Text="Save" /></td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="View2" runat="server">
            <asp:GridView ID="gvResortRoomTypes" runat="server" EmptyDataText="No Records" GridLines="Horizontal" 
                AutoGenerateColumns="true" AutoGenerateSelectButton="true" BorderStyle="None">
                <SelectedRowStyle BackColor="#CCFFFF" Wrap="true" />
                <AlternatingRowStyle BackColor="#CCFFCC" />
            </asp:GridView>
            <asp:Button ID="btnAddResort" runat="server" Text="Add" />
        </asp:View>
        <asp:View ID="View3" runat="server">
            <asp:GridView ID="gvHotelRoomTypes" runat="server" EmptyDataText="No Records" GridLines="Horizontal" 
                AutoGenerateColumns="true" AutoGenerateSelectButton="true" BorderStyle="None">
                <SelectedRowStyle BackColor="#CCFFFF" Wrap="true" />
                <AlternatingRowStyle BackColor="#CCFFCC" />
            </asp:GridView>
            <asp:Button ID="btnAddHotel" runat="server" Text="Add" />
        </asp:View>
        <asp:View ID="View4" runat="server">
            <asp:Button ID="Button1" runat="server" Text="Save" />
            <CKEditor:CKEditorControl ID="CKEditor1" runat="server" Height = "800" Width = "1000"></CKEditor:CKEditorControl>
        </asp:View>
                <asp:View ID="View5" runat="server">
            <asp:GridView ID="gvCheckInLocations" runat="server" EmptyDataText="No Records" GridLines="Horizontal" 
                AutoGenerateColumns="true" AutoGenerateSelectButton="true" BorderStyle="None">
                <SelectedRowStyle BackColor="#CCFFFF" Wrap="true" />
                <AlternatingRowStyle BackColor="#CCFFCC" />
            </asp:GridView>
            <asp:Button ID="Button2" runat="server" Text="Add" />
        </asp:View>
    </asp:MultiView>
</asp:Content>

