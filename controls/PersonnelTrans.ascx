<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PersonnelTrans.ascx.vb" Inherits="controls_PersonnelTrans" %>
 
<div class="ListGrid">
    <asp:GridView ID="gvPersonnelTrans" runat="server" AutoGenerateColumns="TRUE" EmptyDataText="No Records">
        <AlternatingRowStyle BackColor="#C7E3D7" />
        <Columns>
           
            <asp:TemplateField HeaderText="Select">
                <ItemTemplate>
                    <a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/editpersonneltrans.aspx?id=<%#container.Dataitem("ID")%>&linkid=<% Dim oID As String = Me.ID & "$lbRefresh"
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
            
        </Columns>
    </asp:GridView>
    <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>

    
</div>
<ul id="menu">
        <li><a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/editpersonneltrans.aspx?keyfield=<%=keyfield%>&keyvalue=<%=keyvalue%>&id=0&linkid=<% Dim oID As String = Me.ID & "$lbRefresh"
    Dim oTest As Object = Me.FindControl("lbRefresh")
    While Not (oTest Is Nothing)
        If Left(LCase(oTest.id), 18) = "contentplaceholder" Then
            oID = "ctl00$" & oTest.id & "$" & oID
        End If
        oTest = oTest.parent
    End While
response.write (oID) %>','Edit',350,350);">Add Personnel Record</a></li>
        <li><asp:LinkButton ID="lbRefresh" runat="server">Refresh</asp:LinkButton></li>
    </ul>
<asp:HiddenField ID="hfKeyField" Value="" runat="server" />
<asp:HiddenField ID="hfKeyValue" Value = "0" runat="server" />


