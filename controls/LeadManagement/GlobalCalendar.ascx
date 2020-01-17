<%@ Control Language="VB" AutoEventWireup="false" CodeFile="GlobalCalendar.ascx.vb" Inherits="controls_LeadManagement_GlobalCalendar" %>
        Calendar Window: <asp:DropDownList ID="DropDownList1" 
    runat="server" AutoPostBack="True">
            <asp:ListItem Value="0">Today</asp:ListItem>
            <asp:ListItem Value="7">1 Week</asp:ListItem>
            <asp:ListItem Value="14">2 Weeks</asp:ListItem>
            <asp:ListItem Value="21">3 Weeks</asp:ListItem>
            <asp:ListItem Value="28">1 Month</asp:ListItem>
            <asp:ListItem Value="182">6 Months</asp:ListItem>
            <asp:ListItem Value="365">1 Year</asp:ListItem>
                </asp:DropDownList>
        <div style="height:157px;overflow:auto;border-top:thin solid black;">

        <asp:GridView ID="GridView1" runat="server" EmptyDataText="No Events">
        </asp:GridView>
        </div>
        <!--<ul id="leadmenu">
            <%If cint(hfKeyValue.Value) > -10 Then%>
                <li><a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/leadmanagement/addcalevent.aspx?id=0&linkid=<% Dim oID As String = Me.ID & "$lbRefresh"
                    Dim oTest As Object = Me.FindControl("lbRefresh")
                    While Not (oTest Is Nothing)
                        If Left(LCase(oTest.id), 18) = "contentplaceholder" Then
                            oID = "ctl00$" & oTest.id & "$" & oID
                        End If
                        oTest = oTest.parent
                    End While
                    response.write (oID) %>','win01',425,350);">Add New</a>
                </li>
            <%End If%>
            <li><asp:LinkButton ID="lbRefresh" runat="server">Refresh</asp:LinkButton>
        
            </li>
        </ul>-->

        <asp:HiddenField ID="hfKeyValue" runat="server" Value = "0" />

        <asp:HiddenField ID="hfKeyField" runat="server" Value = "0" />
