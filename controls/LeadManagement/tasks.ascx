<%@ Control Language="VB" AutoEventWireup="false" CodeFile="tasks.ascx.vb" Inherits="controls_LeadManagement_tasks" %>
        Calendar Window: <asp:DropDownList ID="ddWindow" runat="server">
            <asp:ListItem Value="0">Today</asp:ListItem>
            <asp:ListItem Value="7">1 Week</asp:ListItem>
            <asp:ListItem Value="14">2 Weeks</asp:ListItem>
            <asp:ListItem Value="21">3 Weeks</asp:ListItem>
            <asp:ListItem Value="28">1 Month</asp:ListItem>
            <asp:ListItem Value="182">6 Months</asp:ListItem>
            <asp:ListItem Value="365">1 Year</asp:ListItem>
                </asp:DropDownList>
        <asp:MultiView ID="MultiView1" runat="server">
            <asp:View ID="View1" runat="server">
                <div style="height:157px;overflow:auto;border-top:thin solid black;">
                    <asp:GridView ID="gvMyTasks" runat="server" AutoGenerateSelectButton="True" 
                        EmptyDataText="No Tasks" AutoGenerateColumns="False" 
                        EnableModelValidation="True">
                        <Columns>
                            <asp:BoundField DataField="EventDate" HeaderText="Date" />
                            <asp:BoundField DataField="EventType" HeaderText="Type" />
                            <asp:BoundField DataField="personnelid" HeaderText="Assigned To" />
                            <asp:BoundField DataField="Event" HeaderText="Event" />
                            <asp:BoundField DataField="contactmethod" HeaderText="ContactMethod" 
                                Visible="False" />
                            <asp:BoundField DataField="prospectid" HeaderText="prospectid" 
                                Visible="False" />
                            <asp:BoundField DataField="Name" HeaderText="Name" Visible="False" />
                            <asp:BoundField DataField="CalendarEntryID" HeaderText="CalendarEntryID" 
                                Visible="False" />
                        </Columns>
                        <SelectedRowStyle BackColor="#CCFFFF" Wrap="True" />
                    <AlternatingRowStyle BackColor="#CCFFCC" />
                    </asp:GridView>
                </div>
                <!--<ul id="leadmenu">
                    <li><a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/leadmanagement/addcalevent.aspx?id=0&linkid=<% Dim oID As String = Me.ID & "$lbRefresh"
                        Dim oTest As Object = Me.FindControl("lbRefresh")
                        While Not (oTest Is Nothing)
                            If Left(LCase(oTest.id), 18) = "contentplaceholder" Then
                                oID = "ctl00$" & oTest.id & "$" & oID
                            End If
                            oTest = oTest.parent
                        End While
                        response.write (oID) %>','win01',450,350);">Add New</a>
                    </li>
                    <li><asp:LinkButton ID="lbRefresh" runat="server">Refresh</asp:LinkButton></li>
                </ul>-->
            </asp:View>
            <asp:View ID="View2" runat="server">
                <asp:GridView ID="gvOthersTasks" runat="server" AutoGenerateSelectButton="true" EmptyDataText="No Tasks For Others">
                <SelectedRowStyle BackColor="#CCFFFF" Wrap="True" />
                <AlternatingRowStyle BackColor="#CCFFCC" />
                </asp:GridView>
            </asp:View>
            <asp:View ID="View3" runat="server">
                <asp:GridView ID="gvMyCompleted" runat="server" AutoGenerateSelectButton="true" EmptyDataText="No Tasks Completed">
                <SelectedRowStyle BackColor="#CCFFFF" Wrap="True" />
                <AlternatingRowStyle BackColor="#CCFFCC" />
                </asp:GridView>
            </asp:View>
            <asp:View ID="View4" runat="server">
                <asp:GridView ID="gvOthersCompleted" runat="server" AutoGenerateSelectButton="true" EmptyDataText="No Tasks Completed by others">
                <SelectedRowStyle BackColor="#CCFFFF" Wrap="True" />
                <AlternatingRowStyle BackColor="#CCFFCC" />
                </asp:GridView>
            </asp:View>
        </asp:MultiView>
        <asp:Label ID="lblException" runat="server" Text="" />
