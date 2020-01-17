<%@ Page Title="Lead Template" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EditLeadTemplate.aspx.vb" Inherits="LeadManagement_EditLeadTemplate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>Name:</td>
            <td colspan="3">
                <asp:TextBox ID="txtName" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Number Columns:</td>
            <td>
                <asp:DropDownList ID="ddColumns" runat="server">
                </asp:DropDownList>
            </td>
            <td>Phone Number Column:</td>
            <td>
                <asp:DropDownList ID="ddPhoneColumn" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    
    <fieldset class="ListGrid">
        <legend>Mappings</legend>
        <asp:GridView ID="gvMappings" runat="server" AutoGenerateSelectButton="False" DataKeyNames="LeadFieldMappingID" AutoGenerateColumns="true" 
            EmptyDataText="No Records" GridLines="Horizontal">
            <SelectedRowStyle BackColor="#CCFFFF" Wrap="True" />
            <AlternatingRowStyle BackColor="#CCFFCC" />        
            <Columns>
                
                 <asp:TemplateField HeaderText="Select">
                            <ItemTemplate>
                                <a href="#" title="Edit" onclick = "javascript:modal.mwindow.open('<%=request.applicationpath%>/leadmanagement/editleadtemplatemapping.aspx?mappingid=<%#container.Dataitem("LeadFieldMappingID")%>&leadtemplateid=<%=Request("LeadTemplateID") %>&linkid=<% Dim oID As String = Me.ID & "$lbRefresh"
    Dim oTest As Object = Me.FindControl("lbRefresh")
    While Not (oTest Is Nothing)
        If Left(LCase(oTest.id), 18) = "contentplaceholder" Then
            oID = "ctl00$" & oTest.id & "$" & oID
        End If
        oTest = oTest.parent
    End While
response.write (oID) %>','win01',700,350);">Edit</a>
                            </ItemTemplate>
                        </asp:TemplateField>   
                              
            </Columns>

            </asp:GridView>
        </fieldset>

    <ul id = "menu">
        <li><a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/leadmanagement/editleadtemplatemapping.aspx?mappingid=0&leadtemplateid=<%=Request("LeadTemplateID") %>&linkid=<% Dim oID As String = Me.ID & "$lbRefresh"
            Dim oTest As Object = Me.FindControl("lbRefresh")
            While Not (oTest Is Nothing)
                If Left(LCase(oTest.id), 18) = "contentplaceholder" Then
                    oID = "ctl00$" & oTest.id & "$" & oID
                End If
                oTest = oTest.parent
            End While
            response.write (oID) %>','win01',350,350);">Add New</a>
        </li>
        <li><asp:LinkButton ID="lbRefresh" runat="server">Refresh</asp:LinkButton></li> 
    </ul>
    <ul id="menu">
        <li><asp:LinkButton ID="lbSave" runat="server">Save</asp:LinkButton></li>
        <li><asp:LinkButton ID="lbCancel" runat="server">Cancel</asp:LinkButton></li>
    </ul>
</asp:Content>

