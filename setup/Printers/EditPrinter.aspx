<%@ Page Title="Edit Printer" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EditPrinter.aspx.vb" Inherits="setup_Printers_EditPrinter" %>

<%@ Register Src="~/controls/Events.ascx" TagPrefix="uc1" TagName="Events" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ul id="menu">
        <li <%if  MultiView1.ActiveViewIndex = 0 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Printer" runat="server">Printer</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 1 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="OIDs" runat="server">OIDs</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 2 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Reads" runat="server">Reads</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 3 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Events" runat="server">Events</asp:LinkButton></li>
    </ul>
    <asp:multiview id="MultiView1" runat="server">
        <asp:View runat="server">
            <table>
                <tr>
                    <td>PrinterID:</td>
                    <td><asp:TextBox id="PrinterID" readonly="true" runat="server">0</asp:TextBox></td>
                </tr>
                <tr>
                    <td>Name:</td>
                    <td><asp:TextBox id="Name" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Host Name:</td>
                    <td><asp:TextBox id="HostName" runat="server"></asp:TextBox></td>
                </tr>
            </table>
            <ul id="menu">
                <li><asp:LinkButton ID="Save" runat="server">Save</asp:LinkButton></li>
            </ul>
        </asp:View>
        <asp:View runat="server">
            <asp:GridView ID="gvOIDs" runat="server" EmptyDataText="No Records">
                    <AlternatingRowStyle BackColor="#C7E3D7" />
                    <Columns>
                        <asp:TemplateField HeaderText="Select">
                            <ItemTemplate>
                                <a href="#" title="Edit" onclick = "javascript:modal.mwindow.open('editoid.aspx?oidid=<%#Container.DataItem("OIDID")%>&printerid=<%=PrinterID.Text%>&linkid=<% Dim oID As String = Me.ID & "$lbRefresh"
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
                    </Columns></asp:GridView>
            <ul id="menu">
                <%If CInt(PrinterID.Text) > 0 Then%>
        <li><a href="javascript:modal.mwindow.open('editoid.aspx?printerid=<%=Printerid.Text%>&linkid=<% Dim oID As String = Me.ID & "$lbRefresh"
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
        </asp:View>
        <asp:View runat="server">
            <asp:GridView ID="gvReads" runat="server"></asp:GridView>
        </asp:View>
        <asp:View runat="server">
            <uc1:Events runat="server" ID="Events1" />
        </asp:View>
    </asp:multiview>
</asp:Content>

