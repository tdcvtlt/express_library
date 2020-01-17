<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Premiums.ascx.vb" Inherits="controls_Premiums" %>
<div class="ListGrid">
    <asp:GridView ID="gvPremiums" runat="server" AutoGenerateColumns="true" EmptyDataText="No Records" onRowDataBound = "gvPremiums_RowDataBound">
                       <AlternatingRowStyle BackColor="#C7E3D7" />
                       <Columns>
                       <asp:TemplateField HeaderText="Select">
                            <ItemTemplate>
                                <a href="#" title="Edit" onclick = "javascript:modal.mwindow.open('<%=request.applicationpath%>/general/editpremiumissued.aspx?piid=<%#container.Dataitem("ID")%>&KF=<%=keyfield %>&kv=<%=keyvalue%>&pb=<% Dim oID As String = Me.ID & "$lbRefresh"
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
                           <asp:TemplateField HeaderText="Signature">
                               <ItemTemplate>
                                   <a href="#" onclick="javascript:modal.mwindow.open('<%=request.applicationpath%>/marketing/PremiumSignatureReceipt.aspx?PremiumIssuedID=<%#Container.DataItem("ID") %>', 'receipt', 510, 510);">Receipt</a>
                               </ItemTemplate>
                           </asp:TemplateField>
                        </Columns>
                </asp:GridView>

</div>
<ul id="menu">
    <li><a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/editpremiumissued.aspx?piid=0&KF=<%=hfKeyField.value %>&kv=<%=hfKeyValue.value%>&pb=<% Dim oID As String = Me.ID & "$lbRefresh"
            Dim oTest As Object = Me.FindControl("lbRefresh")
            While Not (oTest Is Nothing)
                If Left(LCase(oTest.id), 18) = "contentplaceholder" Then
                    oID = "ctl00$" & oTest.id & "$" & oID
                End If
                oTest = oTest.parent
            End While
            response.write (oID) %>','win01',350,350);">Add <%=IIf(hfKeyField.Value.ToLower() = "reservationid", "Gift", "Premium") %></a></li>
    <li><a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/SwapPremiumIssued.aspx?Field=<%=hfKeyField.value %>&id=<%=hfKeyValue.value%>&pb=<% response.Write(oID) %>','win01',350,350);">Swap <%=IIf(hfKeyField.Value.ToLower() = "reservationid", "Gift", "Premium") %></a></li>
    <li><asp:LinkButton ID="lbRefresh" runat="server">Refresh</asp:LinkButton></li>
</ul>
<asp:Label ID="lblErr" runat="server" ForeColor="Red"></asp:Label>
<asp:HiddenField ID="hfKeyField" runat="server" />
<asp:HiddenField ID="hfKeyValue" runat="server" />

