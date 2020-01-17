<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EditProject.aspx.vb" Inherits="PropertyManagement_Projects_EditProject" %>

<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<%@ Register src="../../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc2" %>

<%@ Register src="../../controls/UserFields.ascx" tagname="UserFields" tagprefix="uc3" %>

<%@ Register src="../../controls/PersonnelTrans.ascx" tagname="PersonnelTrans" tagprefix="uc4" %>

<%@ Register src="../../controls/Campaign.ascx" tagname="Campaign" tagprefix="uc5" %>

<%@ Register src="../../controls/Notes.ascx" tagname="Notes" tagprefix="uc6" %>

<%@ Register src="../../controls/Events.ascx" tagname="Events" tagprefix="uc7" %>

<%@ Register src="../../controls/SoldInventory.ascx" tagname="SoldInventory" tagprefix="uc8" %>

<%@ Register src="../../controls/UploadedDocs.ascx" tagname="UploadedDocs" tagprefix="uc9" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script language=javascript type ="text/javascript">
    function Refresh_Areas()
    {
        __doPostBack('ctl00$ContentPlaceHolder1$Areas_Link','');
    }
    function Refresh_Rooms() {
        __doPostBack('ctl00$ContentPlaceHolder1$Rooms_Restrict_Link', '');

    }
    function Refresh_Progress() {
        __doPostBack('ctl00$ContentPlaceHolder1$Values_Link', '');
    }
    function Refresh_Docs() {
        __doPostBack('ctl00$ContentPlaceHolder1$UploadedDocs_Link', '');
    }
    function Refresh_Parts() {
        __doPostBack('ctl00$ContentPlaceHolder1$Items_Link', '');
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ul id="menu">
        <li <%If MultiView1.ActiveViewIndex = 0 Then : Response.Write("class=""current""") : End If%>><asp:LinkButton ID="Project_Link" runat="server">Project</asp:LinkButton></li>
        <li <%If MultiView1.ActiveViewIndex = 1 Then : Response.Write("class=""current""") : End If%>><asp:LinkButton ID="Values_Link" runat="server">Progress</asp:LinkButton></li>
        <li <%If MultiView1.ActiveViewIndex = 2 Then : Response.Write("class=""current""") : End If%>><asp:LinkButton ID="Areas_Link" runat="server">Areas</asp:LinkButton></li>
        <li <%If MultiView1.ActiveViewIndex = 3 Then : Response.Write("class=""current""") : End If%>><asp:LinkButton ID="Rooms_Link" runat="server">Rooms</asp:LinkButton></li>
        <li <%If MultiView1.ActiveViewIndex = 9 Then : Response.Write("class=""current""") : End If%>><asp:LinkButton ID="Items_Link" runat="server">Items</asp:LinkButton></li>
    </ul>
    <ul id="menu">
        <li <%If MultiView1.ActiveViewIndex = 6 Then : Response.Write("class=""current""") : End If%>><asp:LinkButton ID="Notes_Link" runat="server">Notes</asp:LinkButton></li>
        <li <%If MultiView1.ActiveViewIndex = 7 Then : Response.Write("class=""current""") : End If%>><asp:LinkButton ID="Personnel_Link" runat="server">Personnel</asp:LinkButton></li>
        <li <%If MultiView1.ActiveViewIndex = 8 Then : Response.Write("class=""current""") : End If%>><asp:LinkButton ID="UserFields_Link" runat="server">User Fields</asp:LinkButton></li>
        <li <%If MultiView1.ActiveViewIndex = 4 Then : Response.Write("class=""current""") : End If%>><asp:LinkButton ID="UploadedDocs_Link" runat="server">Uploaded Docs</asp:LinkButton></li>
        <li <%If MultiView1.ActiveViewIndex = 5 Then : Response.Write("class=""current""") : End If%>><asp:LinkButton ID="Events_Link" runat="server">Events</asp:LinkButton></li>
    </ul>
    
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="Project_View" runat="server">
             <table>
                <tr>
                    <td>Project ID:</td>
                    <td>
                        <asp:TextBox ID="txtProjectID" runat="server" readonly></asp:TextBox>
                    </td>
                    <td>Date Created:</td>
                    <td>
                        <uc1:DateField ID="dfDateCreated" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Name:</td>
                    <td><asp:TextBox ID="txtName" runat="server"></asp:TextBox></td>
                    <td>Status:</td>
                    <td>
                        <uc2:Select_Item ID="siStatus" runat="server" />
                    </td>
                    
                </tr>
                
                <tr><td><asp:Button ID="btnSave3" runat="server" Text="Save" />
                    <asp:Button ID="btnExport" runat="server" Text="Create Excel" />
                    </td></tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="Progress_View" runat="server">
            <div class="ListGrid">
            <asp:GridView ID="gvProgress" runat="server" EmptyDataText="No Records"  >
                <AlternatingRowStyle BackColor="#C7E3D7" />
                <Columns>
                    <asp:TemplateField HeaderText="Select">
                        <ItemTemplate>
                            <a href="#" onclick="javascript:modal.mwindow.open('UpdateProgress.aspx?PID=<%#container.Dataitem("ProjectID")%>&RoomID=<%#container.Dataitem("RoomID") %>','win01',650,550)" title="Edit">Edit</a>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            </div>
        </asp:View>
        <asp:View ID="Areas_View" runat="server">
            <div class="ListGrid">
            <asp:GridView ID="gvAreas" runat="server">
                <AlternatingRowStyle BackColor="#C7E3D7" />
                
            </asp:GridView>
            </div>
            <ul id="menu">
                <li><a href="#" onclick="javascript:modal.mwindow.open('<%=request.applicationpath%>/propertymanagement/projects/addarea.aspx?id=<%=request("ID") %>','win01',350,350)">Add Area</a></li>
            </ul>
            <br />
        </asp:View>
        <asp:View ID="Rooms_View" runat="server">
            <div class="ListGrid">
            <asp:GridView runat="server" id = "gvRooms" AutoGenerateSelectButton="True" EmptyDataText="No Records" ></asp:GridView>
            </div>
            <ul id="menu">
                <li><a href="#" onclick="javascript:modal.mwindow.open('<%=request.applicationpath%>/propertymanagement/projects/addroom.aspx?id=<%=request("ID") %>','win01',350,350)">Add Room</a></li>
            </ul>
            
        </asp:View>
        <asp:View ID="UploadedDocs_View" runat="server">
            <uc9:UploadedDocs ID="ucDocs" runat="server" EnableTheming="True" />
        </asp:View>
        <asp:View ID="Events_View" runat="server">
            <uc7:Events ID="ucEvents" runat="server" />
        </asp:View>
        <asp:View ID="Notes_View" runat="server">
            <uc6:Notes ID="ucNotes" runat="server" />
        </asp:View>
        <asp:View ID="Personnel_View" runat="server">
            <uc4:PersonnelTrans ID="PersonnelTrans1" runat="server" KeyField="CONTRACTID" />
        </asp:View>
        <asp:View ID="UserFields_View" runat="server">
            <uc3:UserFields ID="UF" runat="server" />
        </asp:View>
        <asp:View ID="Items_View" runat="server">
            <div class="ListGrid">
                <asp:GridView runat="server" id = "gvItems" AutoGenerateSelectButton="True" EmptyDataText="No Records" ></asp:GridView>
            </div>
            <ul id="menu">
                <li><a href="#" onclick="javascript:modal.mwindow.open('<%=request.applicationpath%>/Maintenance/partfilter.aspx?RequestID=<%=Request("ID") %>&partLocation=Project&transType=addPart','win01',690,450)">Add Item</a></li>
            </ul>
        </asp:View>
        <asp:View runat="server" id ="DENIED">
            ACCESS DENIED
        </asp:View>
    </asp:MultiView>
    
    <asp:Label ID="lblProjectError" runat="server" Text="" ForeColor="Red"></asp:Label>
</asp:Content>

