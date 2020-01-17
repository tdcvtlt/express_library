<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EditPersonnelGroup.aspx.vb" Inherits="security_EditPersonnelGroup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script language=javascript type ="text/javascript">
    function Refresh_Members()
    {
        __doPostBack('ctl00$ContentPlaceHolder1$Groups_Link','');
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ul id="menu">
        <li <%if  MultiView1.ActiveViewIndex = 0 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Personnel_Link" runat="server">Personnel Group</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 1 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Security_Link" runat="server">Security</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 2 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Groups_Link" runat="server">Members</asp:LinkButton></li>
    </ul>
<asp:MultiView runat="server" id = "MultiView1">
<asp:View runat="server" id = "View1">
    <table>
        <tr>
            <td>GroupID:</td>
            <td><asp:TextBox runat="server" id = "txtGroupID" readonly>0</asp:TextBox></td>
            <td>Group Name:</td>
            <td><asp:TextBox runat="server" id = "txtGroupName"></asp:TextBox></td>
        </tr>
        <tr>
            <td><asp:Button runat="server" Text="Save" onclick="Unnamed1_Click"></asp:Button></td>
        </tr>
    </table>
</asp:View>
<asp:View runat="server" id = "Security_View">
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
<asp:View runat="server" id = "ViewMembers">
    <asp:GridView runat="server" id = "gvMembers" EnableModelValidation="True" autogeneratecolumns = "false" EmptyDataText = "No Records" onRowDataBound = "gvMembers_RowDataBound">
    
        <Columns>
            <asp:BoundField DataField="ID" HeaderText="ID"></asp:BoundField>
            <asp:BoundField DataField="Personnel" HeaderText="Personnel"></asp:BoundField>
            <asp:ButtonField CommandName="Remove" Text="Remove"></asp:ButtonField>
        </Columns>
    
    </asp:GridView>
    <asp:Button runat="server" Text="Add Member" onclick="Unnamed4_Click"></asp:Button>
</asp:View>
<asp:View runat="server" id = "ViewDenied">
    ACCESS DENIED
</asp:View>
</asp:MultiView>
</asp:Content>

