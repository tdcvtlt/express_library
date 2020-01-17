<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EditMaintenanceFeeCode.aspx.vb" Inherits="EditMaintenanceFeeCode" %>

<%@ Register src="~/controls/Events.ascx" tagname="Events" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ul id="menu">
        <li <%if  MultiView1.ActiveViewIndex = 0 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Main_Link" runat="server">Maintenance Fee Code</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 1 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Mapping_Link" runat="server">Mapping</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 2 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Events_Link" runat="server">Events</asp:LinkButton></li>
        
    </ul>
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="vwMain" runat="server">
            <table>
                <tr>
                    <td>Maintenance Fee Code ID:</td>
                    <td><asp:TextBox ID="txtID" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Code:</td>
                    <td><asp:TextBox ID="txtCode" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Phase:</td>
                    <td>
                        <asp:DropDownList ID="ddPhase" runat="server">
                            <asp:ListItem>(Empty)</asp:ListItem>
                            <asp:ListItem>Combo</asp:ListItem>
                            <asp:ListItem>Cottage</asp:ListItem>
                            <asp:ListItem>Townes</asp:ListItem>
                            <asp:ListItem>Estates</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>Size:</td>
                    <td>
                        <asp:DropDownList ID="ddSize" runat="server">
                            <asp:ListItem Value="0">(Empty)</asp:ListItem>
                            <asp:ListItem>1</asp:ListItem>
                            <asp:ListItem>2</asp:ListItem>
                            <asp:ListItem>3</asp:ListItem>
                            <asp:ListItem>4</asp:ListItem>
                            <asp:ListItem>5</asp:ListItem>
                            <asp:ListItem>6</asp:ListItem>
                            <asp:ListItem>7</asp:ListItem>
                            <asp:ListItem>8</asp:ListItem>
                            <asp:ListItem>9</asp:ListItem>
                            <asp:ListItem>10</asp:ListItem>
                            <asp:ListItem>11</asp:ListItem>
                            <asp:ListItem>12</asp:ListItem>
                            <asp:ListItem>13</asp:ListItem>
                            <asp:ListItem>14</asp:ListItem>
                            <asp:ListItem>15</asp:ListItem>
                            <asp:ListItem>16</asp:ListItem>
                            <asp:ListItem>17</asp:ListItem>
                            <asp:ListItem>18</asp:ListItem>
                            <asp:ListItem>19</asp:ListItem>
                            <asp:ListItem>20</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td valign="top">Description:</td>
                    <td><asp:TextBox ID="txtDescription" runat="server" Height="101px" 
                            TextMode="MultiLine" Width="262px"></asp:TextBox></td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="vwMapping" runat="server">
            <asp:GridView ID="gvMapping" runat="server" EmptyDataText="No Mapping">
                <AlternatingRowStyle BackColor="#C7E3D7" />
                    <Columns>
                        <asp:TemplateField HeaderText="Select">
                            <ItemTemplate>
                                <a href="#" title="Edit" onclick = "javascript:modal.mwindow.open('<%=request.applicationpath%>/accounting/maintenancefeecode2fintrans.aspx?codeid=-1&mapid=<%#container.Dataitem("maintenancefeecode2fintransid")%>&linkid=<% Dim oID As String = Me.ID & "$lbRefresh"
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
            <ul id="menu">
                <li><a href="javascript:modal.mwindow.open('<%=request.applicationpath%>/accounting/maintenancefeecode2fintrans.aspx?codeid=<%=txtID.text%>&linkid=<% Dim oID As String = Me.ID & "$lbRefresh"
            Dim oTest As Object = Me.FindControl("lbRefresh")
            While Not (oTest Is Nothing)
                If Left(LCase(oTest.id), 18) = "contentplaceholder" Then
                    oID = "ctl00$" & oTest.id & "$" & oID
                End If
                oTest = oTest.parent
            End While
            response.write (oID) %>','win01',350,350);">Add New</a>
        </li>
                <li ><asp:LinkButton ID="lbRefresh" runat="server">Refresh</asp:LinkButton></li>
            </ul>
        </asp:View>
        <asp:View ID="vwEvents" runat="server">
            
            <uc1:Events ID="ucEvents" runat="server" KeyField="MaintenanceFeeCodeID" />
            
        </asp:View>
        <asp:View ID="vwDenied" runat="server">
            ACCESS DENIED
        </asp:View>
    </asp:MultiView>
     <ul id="menu">
        <li ><asp:LinkButton ID="lbSave" runat="server">Save</asp:LinkButton></li>
        
    </ul>
</asp:Content>

