<%@ Control Language="VB" AutoEventWireup="false" CodeFile="UserFields.ascx.vb" Inherits="controls_UserFields" %>

<div class="ListGrid">
    
    <asp:GridView ID="gvUserFields" runat="server" AutoGenerateColumns="False">
        <AlternatingRowStyle BackColor="#C7E3D7" />
        <Columns>
           
            <asp:TemplateField HeaderText="Select">
                <ItemTemplate>
                    <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/edituserfield.aspx?id=<%#container.Dataitem("ID")%>&UFID=<%#container.Dataitem("UFID")%>&KeyValue=<%#container.Dataitem("KeyVal")%>&linkid=<% Dim oID As String = Me.ID & "$lbRefresh"
    Dim oTest As Object = Me.FindControl("lbRefresh")
    While Not (oTest Is Nothing)
        If Left(LCase(oTest.id), 18) = "contentplaceholder" Then
            oID = "ctl00$" & oTest.id & "$" & oID
        End If
        oTest = oTest.parent
    End While
response.write (oID) %>','Edit',350,350);">
                    Edit</a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText = "Field" DataField = "UFName" />
            <asp:BoundField HeaderText = "Value" DataField = "UFValue" />
        </Columns>
    </asp:GridView>
</div>
<ul id="menu">
        <li><a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/edituserfield.aspx?keyfield=<%=keyfield%>&keyvalue=<%=keyvalue%>&id=0&linkid=<% Dim oID As String = Me.ID & "$lbRefresh"
    Dim oTest As Object = Me.FindControl("lbRefresh")
    While Not (oTest Is Nothing)
        If Left(LCase(oTest.id), 18) = "contentplaceholder" Then
            oID = "ctl00$" & oTest.id & "$" & oID
        End If
        oTest = oTest.parent
    End While
response.write (oID) %>','Edit',350,350);">Add User Field</a></li>
        <li><asp:LinkButton ID="lbRefresh" runat="server">Refresh</asp:LinkButton></li>
    </ul>
<asp:HiddenField ID="hfUFTable" Value = "" runat="server" />
<asp:HiddenField ID="hfKeyValue" Value = "0" runat="server" />
<asp:HiddenField ID="hfUFID" Value = "0" runat="server" />

