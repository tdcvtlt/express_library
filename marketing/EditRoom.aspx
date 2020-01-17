<%@ Page Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="editRoom.aspx.vb" Inherits="marketing_editRoom" Title="Editing A Room"%>

<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>
<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc2" %>
<%@ Register src="../controls/UserFields.ascx" tagname="UserFields" tagprefix="uc3" %>


<%@ Register src="../controls/Notes.ascx" tagname="Notes" tagprefix="uc4" %>


<%@ Register src="../controls/Events.ascx" tagname="Events" tagprefix="uc5" %>
<%@ Register Src="~/controls/UploadedDocs.ascx" TagPrefix="uc1" TagName="UploadedDocs" %>



<asp:content ID="Content1" contentplaceholderid="head" runat="server">

<style type="text/css">
        
.option-first
{
    font-style:italic;
    font-size:1.3em;
}
                
.ul-none
{
    list-style-type:none;
}


.tooltip
{
    position:absolute;
    z-index:2;
    background:#efd;
    border:1px solid #ccc;
    padding:3px;
}

.selected
{
    color:Aqua;
    background-color:Silver;
}

.transparent {
	zoom: 1;
	filter: alpha(opacity=50);
	opacity: 0.5;
	font-style:italic;
}
          
</style>

<script language="javascript" type ="text/javascript">
    function Refresh_Lockout(roomNum)
    {
        document.getElementById("ctl00$ContentPlaceHolder1$txtLockOut").value = roomNum;
    } 
    
    
</script>
</asp:content>

<asp:content ID="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">

    
    <ul id="menu">
        <li <%if  MultiView1.ActiveViewIndex = 0 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Rooms_Link" runat="server">Room</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 1 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="MaintRequest_Link" runat="server">Maint. Requests</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 2 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Amenities_Link" runat="server">Amenities</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 3 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="UserFields_Link" runat="server">User Fields</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 4 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Notes_Link" runat="server">Notes</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 5 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Events_Link" runat="server">Events</asp:LinkButton></li>
    </ul>
    <ul id="menu">
        <li <%if  MultiView1.ActiveViewIndex = 6 Then : Response.Write("class=""current""") : End If %>><asp:LinkButton ID="PM_Link" runat="server">Preventive Maintenance</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 7 Then : Response.Write("class=""current""") : End If %>><asp:LinkButton ID="UploadedFiles_Link" runat="server">Uploaded Files</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 8 Then : Response.Write("class=""current""") : End If %>><asp:LinkButton ID="QuickChecks_Link" runat="server">Quick Checks</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 9 Then : Response.Write("class=""current""") : End If %>><asp:LinkButton ID="Refurbs_Link" runat="server">Refurbs</asp:LinkButton></li>
    </ul>
        <asp:MultiView ID="MultiView1" runat="server">
                <asp:View ID="Rooms_View" runat="server">
                    <table>
                        <tr>
                            <td>RoomID:</td>
                            <td><asp:TextBox ID="txtRoomID" runat="server" ReadOnly>0</asp:TextBox></td>
                            <td>Unit Name:</td>
                            <td>
                            <%If Request("RoomID") = 0 then %>
                                <asp:DropDownList runat="server" id = "ddUnits"></asp:DropDownList>
                            <%Else %>
                            <asp:TextBox ID="txtUnit" runat="server"></asp:TextBox>
                            <%End If %>
                            </td>
                        </tr>
                        <tr>
                            <td>Room Number:</td>
                            <td><asp:TextBox ID="txtRoomNumber" runat="server"></asp:TextBox></td>
                            <td>Lockout:</td>
                            <td><asp:TextBox ID="txtLockOut" runat="server" readonly></asp:TextBox>
                                <asp:Button runat="server" Text="..." onclick="Unnamed1_Click"></asp:Button>
                            </td>
                        </tr>
                        <tr>
                            <td>Type:</td>
                            <td><uc2:Select_Item ID="siType" runat="server" /></td>
                            <td>SubType:</td>
                            <td><uc2:Select_Item ID="siSubType" runat="server" /></td>
                        </tr>
                        <%--<tr>
                            <td>Status:</td>
                            <td><uc2:Select_Item ID="siStatus" runat="server" /></td>
                            <td>Status Date:</td>
                            <td>
                            <asp:TextBox ID="txtStatusDate" runat="server" readonly></asp:TextBox></td>
                        </tr>--%>
                        <tr>
                            <td>Maintenance Status</td>
                            <td><uc2:Select_Item ID="siMaintStatus" runat="server" /></td>
                            <td>Maint. Status Date:</td>
                            <td><asp:TextBox ID="txtMaintStatusDate" runat="server" READONLY></asp:TextBox></td>                            
                        </tr>
                        <tr>
                            <td>Housekeeping Status:</td>
                            <td>
                                <uc2:Select_Item ID="siHKStatus" runat="server" />
                            </td>
                            <td>H.K. Status Date:</td>
                            <td>
                                <asp:TextBox ID="txtHKStatusDate" runat="server" READONLY=""></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Extension:</td>
                            <td>
                                <asp:TextBox ID="txtExtension" runat="server" ReadOnly=""></asp:TextBox>
                            </td>
                            <td>
                                Max Occupancy:</td>
                            <td>
                                <asp:TextBox ID="txtMaxOcc" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button runat="server" Text="Update" id="btnUpdate"></asp:Button>
                            </td>
                        </tr>
                    </table>
                    <asp:Label runat="server" Text="Label" id = "lblMessage"></asp:Label>
                    <asp:TextBox ID="txtUnitID" Visible="False" runat="server"></asp:TextBox>
                    <asp:TextBox ID="txtCRMSID" runat="server"></asp:TextBox>
                </asp:View>
                <asp:View ID="MaintRequest_View" runat="server">
                    <asp:GridView ID="gvMaintRequests" runat="server" EmptyDataText = "No Maint. Requests" AutoGenerateSelectButton="True">
                    </asp:GridView>
                </asp:View>
                <asp:View ID="Amenities_View" runat="server">
                    <asp:GridView ID="gvAmenities" runat="server" EmptyDataText = "No Amenities" AutoGenerateSelectButton="True">
                    </asp:GridView>
                </asp:View>
                <asp:View ID="UserFields_View" runat="server">
                    <uc3:UserFields ID="UF" runat="server" />
                </asp:View>
                <asp:View ID="Notes_View" runat="server">
                    <uc4:Notes ID="Notes1" runat="server" keyfield = "RoomID"/>
                </asp:View>
                <asp:View ID="Events_View" runat="server">

                    <uc5:Events ID="Events1" runat="server" />

                </asp:View>


                <asp:View ID="PMItems_View" runat="server">                      
                    
                    <h2>Preventive Maintenance</h2>

                    <ul>
                        <li style="list-style-type:none; float:left; text-indent:-4px; margin-right:16px;"><asp:Button runat="server" ID="refresh_btn_top" Text="Refresh" /></li>
                        <li style="list-style-type:none; float:left; text-indent:-4px; margin-right:16px;"><asp:Button runat="server" ID="add_btn" Text="Add" /></li>                        
                    </ul>

                    
                    <br /><br />                    
                    <div>
                        <asp:UpdatePanel runat="server" ID="upQAZ">
                            <ContentTemplate>      
                                <asp:Label runat="server" ID="lblStatus"></asp:Label>                          
                                <asp:GridView runat="server" ID="gvPM" AutoGenerateColumns="false" DataKeyNames="item2trackid">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Action ">
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="AddLink" CommandName="Add" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "item2trackid") & "-" & DataBinder.Eval(Container.DataItem, "PMItemID") %>' OnClick="OnButtonLinkClick"  Text="Add"></asp:LinkButton>                  
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action ">
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="ChangeLink" CommandName="Change" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "item2trackid") & "-" & DataBinder.Eval(Container.DataItem, "PMItemID") %>' OnClick="OnButtonLinkClick"  Text="Change"></asp:LinkButton>                  
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action ">
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="RemoveLink" CommandName="Remove" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "item2trackid") & "-" & DataBinder.Eval(Container.DataItem, "PMItemID") %>' OnClick="OnButtonLinkClick"  Text="Remove"></asp:LinkButton>                  
                                            </ItemTemplate>
                                        </asp:TemplateField>                                                                                 
                                        <asp:BoundField DataField="Name" HeaderText="Name" />                                                    
                                        <asp:BoundField DataField="Category" HeaderText="Category" />                    
                                        <asp:BoundField DataField="Life" HeaderText="Life Span" /> 
                                        <asp:BoundField DataField="ExtraText" HeaderText="Location/Description" /> 
                                        <asp:BoundField DataField="DateAdded" HeaderText="Date Added" DataFormatString="{0:d}" />
                                        <asp:BoundField DataField="DateRemoved" HeaderText="Date Removed" DataFormatString="{0:d}" />
                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="refresh_btn_top" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>                    
                    </div>                   
                </asp:View>    
            <asp:View ID="UploadedFiles" runat="server">
                <uc1:UploadedDocs runat="server" ID="UploadedDocs" />
            </asp:View>
            <asp:View ID="QuickChecks" runat="server">
                <asp:GridView ID="gvQuickChecks" runat="server" AutoGenerateSelectButton="True" 
                       EmptyDataText="No Records" GridLines="Horizontal">
                    <SelectedRowStyle BackColor="#CCFFFF" Wrap="True" />
                    <AlternatingRowStyle BackColor="#CCFFCC" />
                </asp:GridView>
            </asp:View>                 
            <asp:View ID="Refurbs" runat="server">
                <asp:GridView ID="gvRefurbs" runat="server" AutoGenerateSelectButton="True" 
                       EmptyDataText="No Records" GridLines="Horizontal">
                    <SelectedRowStyle BackColor="#CCFFFF" Wrap="True" />
                    <AlternatingRowStyle BackColor="#CCFFCC" />
                </asp:GridView>
            </asp:View>      
            </asp:MultiView>  
</asp:content>
