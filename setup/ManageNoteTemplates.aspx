<%@ Page Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="ManageNoteTemplates.aspx.vb" Inherits="setup_ManageNoteTemplates" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ul id="menu">
        <li><a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/general/EditNoteTemplate.aspx?ID=0','win01',350,350);">Add New</a></li>
        <li><asp:LinkButton ID="lbRefresh" runat="server">Refresh</asp:LinkButton></li>
    </ul>
    <asp:GridView ID="gvTemplates" runat="server" EmptyDataText="No Records">
            <AlternatingRowStyle BackColor="#C7E3D7" />
            <Columns>
                <asp:TemplateField HeaderText="Select">
                    <ItemTemplate>
                        <a href="#" title="Edit" onclick = "javascript:modal.mwindow.open('<%=Request.ApplicationPath%>/general/EditNoteTemplate.aspx?ID=<%#Container.DataItem("NOTETEMPLATEID")%>&linkid=<% Dim oID As String = Me.ID & "$lbRefresh"
                            Dim oTest As Object = Me.FindControl("lbRefresh")
                            While Not (oTest Is Nothing)
                                If Left(LCase(oTest.id), 18) = "contentplaceholder" Then
                                    oID = "ctl00$" & oTest.id & "$" & oID
                                End If
                                oTest = oTest.parent
                            End While
                            Response.write (oID) %>','win01',350,350);">Edit</a>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
    </asp:GridView>
    
</asp:Content>
