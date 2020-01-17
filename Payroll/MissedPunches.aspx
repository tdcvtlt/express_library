<%@ Page Title="Missed Punches" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="MissedPunches.aspx.vb" Inherits="Payroll_MissedPunches" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:MultiView runat="server" id = "MultiView1">
    <asp:View runat="server" id = "View_0">
        <ul id="menu">
            <!--<li <%if  MultiView2.ActiveViewIndex = 0 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="MgrApproved_Link" runat="server">Manager Approved</asp:LinkButton></li> -->
            <li <%if  MultiView2.ActiveViewIndex = 1 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="MgrPending_Link" runat="server">Pending Manager Approval</asp:LinkButton></li>
            <!--<li <%if  MultiView2.ActiveViewIndex = 2 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="MgrDenied_Link" runat="server">Manager Denied</asp:LinkButton></li> -->
        </ul>
        <asp:MultiView runat="server" id = "MultiView2">
            <asp:View runat="server" id = "View_1">
                <asp:GridView runat="server" id = "gvMgrApproved" autoGenerateColumns = "false" onRowDataBound = "gvMgrApproved_RowDataBound" EmptyDataText = "No Punches">
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="ID"></asp:BoundField>
                        <asp:BoundField DataField="Employee" HeaderText="Employee"></asp:BoundField>
                        <asp:BoundField DataField="Department" HeaderText="Department"></asp:BoundField>
                        <asp:BoundField DataField="PunchMissed" HeaderText="Punch Missed">
                        </asp:BoundField>
                        <asp:BoundField DataField="PunchTime" HeaderText="Punch Time"></asp:BoundField>
                        <asp:BoundField DataField="Reason" HeaderText="Reason"></asp:BoundField>
                        <asp:TemplateField HeaderText="Approve">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbApprove" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Deny">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbDeny" runat="server" />
                            </ItemTemplate>                        
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:Button runat="server" Text="Process Missed Punches" 
                    onclick="Unnamed1_Click"></asp:Button>
            </asp:View>
            <asp:View runat="server" id = "View_2">
                <asp:GridView runat="server" id = "gvMgrPending" autoGenerateColumns = "false" onRowDataBound = "gvMgrPending_RowDataBound" EmptyDataText = "No Punches">
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="ID"></asp:BoundField>
                        <asp:BoundField DataField="Employee" HeaderText="Employee"></asp:BoundField>
                        <asp:BoundField DataField="Department" HeaderText="Department"></asp:BoundField>
                        <asp:BoundField DataField="PunchMissed" HeaderText="Punch Missed">
                        </asp:BoundField>
                        <asp:BoundField DataField="PunchTime" HeaderText="Punch Time"></asp:BoundField>
                        <asp:BoundField DataField="Reason" HeaderText="Reason"></asp:BoundField>
                        <asp:TemplateField HeaderText="Approve">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbApprove" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Deny">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbDeny" runat="server" />
                            </ItemTemplate>                        
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:Button runat="server" Text="Process Missed Punches" 
                    onclick="Unnamed2_Click"></asp:Button>
            </asp:View>
            <asp:View runat="server" id = "View_3">
            <asp:GridView runat="server" id = "gvMgrDenied" autoGenerateColumns = "false" onRowDataBound = "gvMgrDenied_RowDataBound" EmptyDataText = "No Punches">
                  <Columns>
                        <asp:BoundField DataField="ID" HeaderText="ID"></asp:BoundField>
                        <asp:BoundField DataField="Employee" HeaderText="Employee"></asp:BoundField>
                        <asp:BoundField DataField="Department" HeaderText="Department"></asp:BoundField>
                        <asp:BoundField DataField="PunchMissed" HeaderText="Punch Missed">
                        </asp:BoundField>
                        <asp:BoundField DataField="PunchTime" HeaderText="Punch Time"></asp:BoundField>
                        <asp:BoundField DataField="Reason" HeaderText="Reason"></asp:BoundField>
                        <asp:TemplateField HeaderText="Approve">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbApprove" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Deny">
                            <ItemTemplate>
                                <asp:CheckBox ID="cbDeny" runat="server" />
                            </ItemTemplate>                        
                        </asp:TemplateField>
                    </Columns>
            </asp:GridView>
            <asp:Button runat="server" Text="Process Missed Punches" onclick="Unnamed3_Click"></asp:Button>
            </asp:View>
        </asp:MultiView>
    </asp:View>
    <asp:View runat="server" id = "View_A">
    <asp:GridView runat="server" id = "gvDeptMissed" autoGenerateColumns = "false" onRowDataBound = "gvDeptMissed_RowDataBound" EmptyDataText = "No Punches">
        <Columns>
            <asp:BoundField DataField="ID" HeaderText="ID"></asp:BoundField>
            <asp:BoundField DataField="Employee" HeaderText="Employee"></asp:BoundField>
            <asp:BoundField DataField="Department" HeaderText="Department"></asp:BoundField>
            <asp:BoundField DataField="PunchMissed" HeaderText="Punch Missed"></asp:BoundField>
            <asp:BoundField DataField="PunchTime" HeaderText="Punch Time"></asp:BoundField>
            <asp:BoundField DataField="Reason" HeaderText="Reason"></asp:BoundField>
            <asp:TemplateField HeaderText="Approve">
                <ItemTemplate>
                    <asp:CheckBox ID="cbApprove" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Deny">
            <ItemTemplate>
                <asp:CheckBox ID="cbDeny" runat="server" />
            </ItemTemplate>                        
            </asp:TemplateField>
        </Columns>    
    </asp:GridView>
    <asp:Button runat="server" Text="Process Missed Punches" onclick="Unnamed4_Click"></asp:Button>
    </asp:View>
</asp:MultiView>

</asp:Content>

