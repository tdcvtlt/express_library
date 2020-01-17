<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EditPMTeam.aspx.vb" Inherits="Maintenance_EditPMTeam" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script language=javascript type ="text/javascript">
        function Refresh_Members() {
            __doPostBack('ctl00$ContentPlaceHolder1$Members_Link', '');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ul id="menu">
        <li <%if  MultiView1.ActiveViewIndex = 0 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Team_Link" runat="server">Team</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 1 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Members_Link" runat="server">Members</asp:LinkButton></li>
    </ul>
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View1" runat="server">
            <table>
                <tr>
                    <td>ID:</td>
                    <td><asp:TextBox ID="txtID" runat="server" ReadOnly="true"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Name:</td>
                    <td><asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Active:</td>
                    <td>
                        <asp:CheckBox ID="cbActive" runat="server" /></td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnSave" runat="server" Text="Save" /></td>
                </tr>
            </table>

        </asp:View>
        <asp:View ID="View2" runat="server">
            <asp:GridView runat="server" id = "gvMembers" EnableModelValidation="True" autogenerateselectbutton ="true" AutoGenerateColumns = "false" EmptyDataText = "No Records" onRowDataBound = "gvMembers_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="ID"></asp:BoundField>
                    <asp:BoundField DataField="Personnel" HeaderText="Personnel"></asp:BoundField>
                    <asp:BoundField DataField="TeamLeader" HeaderText ="Team Leader"></asp:BoundField>
                    <asp:ButtonField CommandName="Remove" Text="Remove"></asp:ButtonField>
                </Columns>
            </asp:GridView>
            <asp:Button ID="btnAdd" runat="server" Text="Add Team Member" />
        </asp:View>
    </asp:MultiView>
</asp:Content>

