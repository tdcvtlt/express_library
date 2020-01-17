<%@ Control Language="VB" AutoEventWireup="false" CodeFile="UploadedDocs.ascx.vb" Inherits="controls_UploadedDocs" %>
<asp:GridView ID="gvDocs" runat="server" EmptyDataText="No Records" AutoGenerateColumns="false" AutoGenerateEditButton="false">
    <Columns>
        <asp:BoundField DataField="ID" HeaderText="ID" />
        <asp:BoundField DataField="Name" headertext = "Name"/>
        <asp:BoundField DataField="DateUploaded" HeaderText="Date Uploaded" />
        <asp:TemplateField>
            <ItemTemplate>
                <a href="#" onclick="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/movedoc.aspx?ID=<%#container.dataitem("ID")%>','Edit',350,350);">Move</a>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>            
                <a href="#" onclick="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/renamedoc.aspx?ID=<%#container.dataitem("ID")%>','Edit',350,350);">Rename</a>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <a href="#" title = "View" onclick="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/uploadeddocs.aspx?f=view&id=<%#container.dataitem("ID")%>&d=<%=hfKeyField.value %>&path=<%#Replace(Server.UrlEncode(Replace(Container.DataItem("Path") & "", "%", "%25")), "'", "%27") %>','Edit',800,600);">View</a>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <a href="#" title = "Remove/Delete" onclick="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/uploadeddocs.aspx?f=delete&id=<%#container.dataitem("ID")%>&d=<%=hfKeyField.value %>&path=<%#Replace(server.urlencode(container.Dataitem("Path") & ""), "'", "%27")%>','Edit',800,600);">Remove/Delete</a>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>

<asp:FileUpload ID="fu" runat="server" />
<asp:Button ID="Upload" runat = "server" Text = "Upload" />
<asp:HiddenField ID="hfKeyField" Value = "" runat="server" />
<asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
<asp:HiddenField ID="hfKeyValue" Value = "0" runat="server" />
<asp:HiddenField ID="hfFileID" Value = "0" runat="server" />
