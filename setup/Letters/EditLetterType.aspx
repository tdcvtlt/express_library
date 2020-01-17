<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EditLetterType.aspx.vb" Inherits="setup_Letters_EditLetterType" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>Letter Type:</td>
            <td><asp:Label ID="lblLetterType" runat="server" Text=""/></td>
            <td></td>
        </tr>
    </table>
    <ul id = "menu">
        <li><asp:LinkButton ID="lbViews" runat="server">Views</asp:LinkButton></li>
        <li><asp:LinkButton ID="lbTags" runat="server">Tags</asp:LinkButton></li>
    </ul>
    
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View1" runat="server">
            <asp:GridView ID="gvViews" runat="server" AutoGenerateSelectButton="False" DataKeyNames="ViewID" AutoGenerateColumns="true" 
                    EmptyDataText="No Records" GridLines="Horizontal">
                <SelectedRowStyle BackColor="#CCFFFF" Wrap="True" />
                <AlternatingRowStyle BackColor="#CCFFCC" />        
                <Columns>
                    <asp:TemplateField HeaderText="Select">
                            <ItemTemplate>
                                <a href="#" title="Edit" onclick = "javascript:modal.mwindow.open('<%=request.applicationpath%>/setup/letters/editView.aspx?ViewID=<%#container.Dataitem("VIEWID")%>&linkid=<% Dim oID As String = Me.ID & "$lbRefresh"
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
            <ul id = "menu">
                <li><a href="#" OnClick= "javascript:modal.mwindow.open('<%=request.applicationpath%>/setup/letters/editView.aspx?ViewID=0&linkid=<% Dim oID As String = Me.ID & "$lbRefresh"
    Dim oTest As Object = Me.FindControl("lbRefresh")
    While Not (oTest Is Nothing)
        If Left(LCase(oTest.id), 18) = "contentplaceholder" Then
            oID = "ctl00$" & oTest.id & "$" & oID
        End If
        oTest = oTest.parent
    End While
response.write (oID) %>&typeid=<%=request("ID") %>','win01',350,350);">Add</a></li>
            </ul>
        </asp:View>
        <asp:View ID="View2" runat="server">
            <asp:GridView ID="gvTags" runat="server" AutoGenerateSelectButton="False" DataKeyNames="TagID" AutoGenerateColumns="true" 
                    EmptyDataText="No Records" GridLines="Horizontal">
                <SelectedRowStyle BackColor="#CCFFFF" Wrap="True" />
                <AlternatingRowStyle BackColor="#CCFFCC" />        
                <Columns>
                    <asp:TemplateField HeaderText="Select">
                            <ItemTemplate>
                                <a href="#" title="Edit" onclick = "javascript:modal.mwindow.open('<%=request.applicationpath%>/setup/letters/edittag.aspx?TagID=<%#container.Dataitem("TAGID")%>&linkid=<% Dim oID As String = Me.ID & "$lbRefresh"
    Dim oTest As Object = Me.FindControl("lbRefresh")
    While Not (oTest Is Nothing)
        If Left(LCase(oTest.id), 18) = "contentplaceholder" Then
            oID = "ctl00$" & oTest.id & "$" & oID
        End If
        oTest = oTest.parent
    End While
response.write (oID) %>&typeid=<%=request("ID") %>','win01',350,350);">Edit</a>
                            </ItemTemplate>
                        </asp:TemplateField>
                    
                              
                </Columns>
            </asp:GridView>
            <ul id = "menu">
                <li><a href="#" OnClick= "javascript:modal.mwindow.open('<%=request.applicationpath%>/setup/letters/editTag.aspx?TagID=0&linkid=<% Dim oID As String = Me.ID & "$lbRefresh"
    Dim oTest As Object = Me.FindControl("lbRefresh")
    While Not (oTest Is Nothing)
        If Left(LCase(oTest.id), 18) = "contentplaceholder" Then
            oID = "ctl00$" & oTest.id & "$" & oID
        End If
        oTest = oTest.parent
    End While
response.write (oID) %>&typeid=<%=request("ID") %>','win01',350,350);">Add</a></li>
            </ul>
        </asp:View>
    </asp:MultiView>
    <ul id = "menu">
        <li><asp:LinkButton ID="lbRefresh" runat="server">Refresh</asp:LinkButton></li>
    </ul>

</asp:Content>

