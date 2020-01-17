﻿<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Comments.ascx.vb" Inherits="controls_Comments" %>

<asp:HiddenField ID="hfKeyValue" Value="0" runat="server" />
    <asp:HiddenField ID="hfKeyField" Value = "" runat="server" />
<div class="ListGrid">
                <asp:GridView ID="gvComments" runat="server" EmptyDataText="No Records">
                    <AlternatingRowStyle BackColor="#C7E3D7" />
                    <Columns>
                        <asp:TemplateField HeaderText="Select">
                            <ItemTemplate>
                                <a href="#" title="Edit" onclick = "javascript:modal.mwindow.open('<%=request.applicationpath%>/general/editcomment.aspx?commentid=<%#container.Dataitem("COMMENTID")%>&KeyField=<%=hfKeyField.value %>&keyvalue=<%=hfKeyValue.value%>&linkid=<% Dim oID As String = Me.ID & "$lbRefresh"
    Dim oTest As Object = Me.FindControl("lbRefresh")
    While Not (oTest Is Nothing)
        If Left(LCase(oTest.id), 18) = "contentplaceholder" Then
            oID = "ctl00$" & oTest.id & "$" & oID
        End If
        oTest = oTest.parent
    End While
response.write (oID) %>','win01',350,350);">Edit</a>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
    
</div>

<ul id="menu">
    <%If cint(hfKeyValue.Value) > 0 Then%>
        <li><a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/editcomment.aspx?commentid=0&KeyField=<%=hfKeyField.value %>&keyvalue=<%=hfKeyValue.value%>&linkid=<% Dim oID As String = Me.ID & "$lbRefresh"
            Dim oTest As Object = Me.FindControl("lbRefresh")
            While Not (oTest Is Nothing)
                If Left(LCase(oTest.id), 18) = "contentplaceholder" Then
                    oID = "ctl00$" & oTest.id & "$" & oID
                End If
                oTest = oTest.parent
            End While
            response.write (oID) %>','win01',350,350);">Add New</a>
        </li>
    <%End If%>
    <li><asp:LinkButton ID="lbRefresh" runat="server">Refresh</asp:LinkButton></li>
</ul>