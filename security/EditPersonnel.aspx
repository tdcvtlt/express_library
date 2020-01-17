<%@ Page Title="Edit Personnel" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EditPersonnel.aspx.vb" Inherits="security_EditPersonnel" %>
<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>
<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc2" %>
<%@ Register src="../controls/UserFields.ascx" tagname="UserFields" tagprefix="uc3" %>
<%@ Register src="../controls/Notes.ascx" tagname="Notes" tagprefix="uc4" %>
<%@ Register src="../controls/Events.ascx" tagname="Events" tagprefix="uc5" %>
<%@ Register src="../controls/UploadedDocs.ascx" tagname="UploadedDocs" tagprefix="uc6" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script language=javascript type ="text/javascript">
    function Refresh_Dept()
    {
        __doPostBack('ctl00$ContentPlaceHolder1$Dept_Link','');
    }
    
    function Refresh_Teams()
    {
        __doPostBack('ctl00$ContentPlaceHolder1$Teams_Link','');
    }
    function Refresh_Cards() {
        __doPostBack('ctl00$ContentPlaceHolder1$TimeCard_Link', '');
    }
    function Refresh_TimeClock() {
        __doPostBack('ctl00$ContentPlaceHolder1$TimeClock_Link', '');
    }
    function Refresh_Sec_Groups() {
        __doPostBack('ctl00$ContentPlaceHolder1$Groups_Link', '');
    }
    function Refresh_Vendors() {
        __doPostBack('ctl00$ContentPlaceHolder1$Vendor_Link', '');
    }
    function Refresh_Docs() {
        __doPostBack('ctl00$ContentPlaceHolder1$UploadedDocs_Link', '');
    }
    function Refresh_Sec_Request() {
        __doPostBack('ctl00$ContentPlaceHolder1$ITSec_Link', '');
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ul id = "menu">
        <li <%if  MultiView1.ActiveViewIndex = 5 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="TimeCard_Link" runat="server">Time Cards</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 6 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Notes_Link" runat="server">Notes</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 7 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Event_Link" runat="server">Events</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 8 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="UserFields_Link" runat="server">User Fields</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 9 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="UploadedDocs_Link" runat="server">Uploaded Docs</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 10 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="TimeClock_Link" runat="server">Time Clock</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 13 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="ITSec_Link" runat="server">IT Sec Reqs</asp:LinkButton></li>
   </ul>
    <ul id="menu">
        <li <%if  MultiView1.ActiveViewIndex = 0 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Personnel_Link" runat="server">Personnel</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 1 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Security_Link" runat="server">Security</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 2 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Groups_Link" runat="server">Security Groups</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 3 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Teams_Link" runat="server">Sales Teams</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 4 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Dept_Link" runat="server">Departments</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 11 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Vendor_Link" runat="server">Vendors</asp:LinkButton></li>
    </ul>
    <asp:MultiView runat="server" id = "MultiView1">
        <asp:View runat="server" id = "Personnel_View">
            <table>
                <tr>
                    <td>PersonnelID:</td>
                    <td><asp:TextBox runat="server" id = "txtPersonnelID" readonly>0</asp:TextBox></td>
                    <td>Status:</td>
                    <td><uc2:Select_Item ID="siStatusID" runat="server" /></td>
                    <td>Active:</td>
                    <td><asp:CheckBox runat="server" id = "cbActive"></asp:CheckBox></td>
                </tr>
                <tr>
                    <td>First Name:</td>
                    <td><asp:TextBox runat="server" id = "txtFirstName"></asp:TextBox></td>
                    <td>Last Name:</td>
                    <td><asp:TextBox runat="server" id = "txtLastName"></asp:TextBox></td>
                    <td>SSN:</td>
                    <td><asp:TextBox runat="server" id = "txtSSN"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Email:</td>
                    <td><asp:TextBox runat="server" id = "txtEmail"></asp:TextBox></td>
                    <td>Hire Date:</td>
                    <td><uc1:DateField ID="dteHireDate" runat="server" /></td>
                    <td>Term Date:</td>
                    <td><uc1:DateField ID="dteTermDate" runat="server" /></td>
                </tr>
                <tr>
                    <td>UserName:</td>
                    <td><asp:TextBox runat="server" id = "txtUserName"></asp:TextBox></td>
                    <td>Act As TO:</td>
                    <td><asp:CheckBox runat="server" id = "cbTO"></asp:CheckBox></td>
                    <td>Sales Rotor:</td>
                    <td><uc2:Select_Item ID="siRotorTypeID" runat="server" /></td>
                </tr>
                <tr>
                    <td>Reports To:</td>
                    <td>
                        <asp:DropDownList ID="ddReportsToID" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td colspan="4">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan = '2'><asp:Button runat="server" Text="Save Personnel" 
                            onclick="Unnamed1_Click2"></asp:Button></td>
                </tr>
            </table>
        </asp:View>    
        <asp:View runat="server" id = "Security_View">
        <asp:UpdatePanel runat="server" id = "CT1"><ContentTemplate>
        <div style = "float:left; width:29.9%;height:300px;">
            Areas<br />
                <asp:ListBox runat="server" id = "lbGroups" Width="234px" Height="264px" autopostback = "true" onSelectedIndexChanged = "lbGroups_SelectedIndexChanged"></asp:ListBox>            
        </div>
        <div style = "float:right; width:70%;height:300px;overflow:auto" >
            Items <br />
            <asp:GridView runat="server" autoGenerateColumns = "False" id = "gvSecItems" onRowDataBound = "gvSecItems_RowDataBound"
                EnableModelValidation="True" EmptyDataText = "No Records">
                
                <Columns>
                    <asp:TemplateField HeaderText="Allow">
                        <ItemTemplate>
                            <asp:CheckBox ID="cbAllow" runat="server" autopostback = "true" 
                                oncheckedchanged="cbAllow_CheckedChanged" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ID" HeaderText="ID"></asp:BoundField>
                    <asp:BoundField DataField="Item" HeaderText="Item"></asp:BoundField>
                </Columns>
                
            </asp:GridView>
        </div>
        </ContentTemplate></asp:UpdatePanel>
        <table>
            <tr>
                <td>Area Name:</td>
                <td><asp:TextBox runat="server" id = "txtArea"></asp:TextBox></td>
                <td><asp:Button runat="server" Text="Add Area" onclick="Unnamed2_Click"></asp:Button></td>
            </tr>
            <tr>
                <td>Item Name:</td>
                <td><asp:TextBox runat="server" id = "txtItem"></asp:TextBox></td>
                <td><asp:Button runat="server" Text="Add Item" onclick="Unnamed3_Click1"></asp:Button></td>
            </tr>
        </table>
        </asp:View>
        <asp:View runat="server" id = "SecurityGrp_View">
            <asp:GridView runat="server" id = "gvPersonnelGroups" 
                EmptyDataText = "No Records" AutoGenerateColumns = "False" 
                EnableModelValidation="True" onRowDataBound = "gvPersonnelGroups_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="ID"></asp:BoundField>
                    <asp:BoundField DataField="GroupName" HeaderText="GroupName"></asp:BoundField>
                    <asp:ButtonField CommandName="Remove" Text="Remove"></asp:ButtonField>
                </Columns>
            </asp:GridView>
            <asp:Button runat="server" Text="Add Group" onclick="Unnamed4_Click1"></asp:Button>
        </asp:View>
        <asp:View runat="server" id = "SalesTeam_View">
            <asp:GridView runat="server" id = "gvSalesTeams" autoGenerateColumns = "False" 
                onRowDataBound = "gvSalesTeams_RowDataBound" EnableModelValidation="True" EmptyDataText = "No Teams Assigned">
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="ID"></asp:BoundField>
                    <asp:BoundField DataField="SalesTeam" HeaderText="Sales Team"></asp:BoundField>
                    <asp:ButtonField CommandName="Remove" Text="Remove"></asp:ButtonField>
                </Columns>
            </asp:GridView>
            <asp:Button runat="server" Text="Add Sales Team" onclick="Unnamed1_Click1"></asp:Button>
        </asp:View>
        <asp:View runat="server" id = "Dept_View">
            <asp:GridView runat="server" id = "gvPersDept" 
                EmptyDataText = "No Departments Assigned" 
                onRowDataBound = "gvPersDept_RowDataBound" AutoGenerateSelectButton="True"></asp:GridView>
            <asp:Button runat="server" Text="Add Department" onclick="Unnamed1_Click"></asp:Button>
        </asp:View>
        <asp:View runat="server" id = "TimeCard_View">
            <asp:GridView ID="gvTimeCards" runat="server" AutoGenerateSelectButton="True" EmptyDataText = "No Cards Assigned"></asp:GridView>
            <asp:Button runat="server" Text="Add Time Card" onclick="Unnamed3_Click"></asp:Button>
        </asp:View>
        <asp:View runat="server" id = "Notes_View">
            <uc4:Notes ID="Notes1" runat="server" KeyField = "PersonnelID" />
        </asp:View>
        <asp:View runat="server" id = "Event_View">
            <uc5:Events ID="upEvents" runat="server" />
        </asp:View>
        <asp:View runat="server" id = "UserField_View">
            <uc3:UserFields ID="UF" runat="server" />
        </asp:View>
        <asp:View runat="server" id = "UploadDoc_View">
        
            <uc6:UploadedDocs ID="ucDocs" runat="server" />
        
        </asp:View>
        <asp:View runat="server" id = "TimeClock_View">
            <div style="height: 193px;overflow:auto; width: 772px;">
            <asp:GridView runat="server" id = "gvTimeClock" autoGenerateColumns = "False" 
                EmptyDataText = "No Punches" EnableModelValidation="True" 
                    onRowDataBound = "gvTimeClock_RowDataBound" AutoGenerateSelectButton="True">
                <Columns>
                    <asp:BoundField DataField="PunchID" HeaderText="PunchID"></asp:BoundField>
                    <asp:BoundField DataField="DateIn" HeaderText="Date In"></asp:BoundField>
                    <asp:BoundField DataField="TimeIn" HeaderText="Time In"></asp:BoundField>
                    <asp:BoundField DataField="InManual" HeaderText="In Manual"></asp:BoundField>
                    <asp:BoundField DataField="DateOut" HeaderText="Date Out"></asp:BoundField>
                    <asp:BoundField DataField="TimeOut" HeaderText="Time Out"></asp:BoundField>
                    <asp:BoundField DataField="OutManual" HeaderText="Out Manual"></asp:BoundField>
                    <asp:BoundField DataField="Department" HeaderText="Department"></asp:BoundField>
                    <asp:BoundField DataField="PunchType" HeaderText="Punch Type"></asp:BoundField>
                    <asp:BoundField DataField="MissedPunchApproval" HeaderText="MissedPunchApproval"></asp:BoundField>
                    <asp:ButtonField CommandName="DeletePunch" Text="Delete Punch Row"></asp:ButtonField>
                </Columns>            
            </asp:GridView>
            </div>
            <asp:Button runat="server" Text="Add Punch" onclick="Unnamed4_Click"></asp:Button>
        </asp:View>
        <asp:View runat="server" id = "Vendor_View">
            <asp:GridView runat="server" id = "gvVendors" autoGenerateSelectButton = "True" 
                autogeneratecolumns = "False" onRowDataBound = "gvVendors_RowDataBound" 
                EnableModelValidation="True">
            
                <Columns>
                    <asp:BoundField DataField="Vendor2PersonnelID" HeaderText="Vendor2PersonnelID"></asp:BoundField>
                    <asp:BoundField DataField="Vendor" HeaderText="Vendor">
                    </asp:BoundField>
                    <asp:BoundField DataField="Admin" HeaderText="Admin">
                    </asp:BoundField>
                    <asp:BoundField DataField="Manager" HeaderText="Manager">
                    </asp:BoundField>
                    <asp:ButtonField CommandName="Remove" Text="Remove">
                    </asp:ButtonField>
                </Columns>
            
            </asp:GridView>
            <asp:Button runat="server" Text="Add Vendor" onclick="Unnamed8_Click"></asp:Button>
        </asp:View>
        <asp:View runat="server" id = "DENIED">
        ACCESS DENIED
        </asp:View>
        <asp:View runat="server" ID = "ITRequest_View">
            <asp:GridView runat = "server" ID = "gvITRequests" AutoGenerateSelectButton = "true" EmptyDataText = "No Records">
            
            </asp:GridView>
            <asp:Button ID="Button1" runat="server" Text="New Security Request" />
        </asp:View>
    </asp:MultiView>

</asp:Content>

