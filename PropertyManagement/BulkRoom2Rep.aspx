<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="BulkRoom2Rep.aspx.vb" Inherits="PropertyManagement_BulkRoom2Rep" %>

<%@ Register Src="~/controls/DateField.ascx" TagPrefix="uc1" TagName="DateField" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .auto-style1 {
            height: 33px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ul id="menu">
        <li <%if  MultiView1.ActiveViewIndex = 0 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Add_Link" runat="server">Assign Techs</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 1 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Remove_Link" runat="server">UnAssign Techs</asp:LinkButton></li>
    </ul>
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View1" runat="server">
        <table>
        <tr>
            <td>Maintenance Tech:</td>
            <td colspan ="2">
                <asp:DropDownList ID="ddTech" runat="server"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>Expiration Date:</td>
            <td>
                <uc1:DateField runat="server" ID="dfExpirationDate" />
            </td>
        </tr>
        <tr>
            <td>Rooms:</td>

        </tr>
        <tr>
            <td>
                <asp:ListBox ID="lbRooms" runat="server"  SelectionMode="Multiple" Height="130px" Width="148px"></asp:ListBox>
            </td>
            <td>
                <asp:Button ID="btnAllRooms" runat="server" Text="ALL>>" /><br />
                <asp:Button ID="btnAdd" runat="server" Text=">>" style="height: 29px" /><br />
                <asp:Button ID="btnRemove" runat="server" Text="<<" /><br />
                <asp:Button ID="btnRemoveAll" runat="server" Text="<< ALL" />
            </td>
            <td>
                <asp:ListBox ID="lbRoomsAdded" runat="server"  SelectionMode="Multiple" Height="130px" Width="148px"></asp:ListBox>
            </td>
        </tr>
        <tr>
            <td class="auto-style1">
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" />

            </td>
        </tr>
    </table>
            </asp:View>
        <asp:View ID="View2" runat="server">
            <table>
                <tr>
                    <td>Maintenance Tech:</td>
                    <td colspan ="2">
                    <asp:DropDownList ID="ddRemoveTech" runat="server" AutoPostBack="True"></asp:DropDownList>
                    </td>
                </tr>
            </table>
            <asp:GridView ID="gvTechRooms" runat="server" AutoGenerateColumns ="false" OnRowDataBound ="gvTechRooms_RowDataBound" EmptyDataText ="No Records">
                <Columns>
                    <asp:TemplateField HeaderText="Select">
                        <ItemTemplate>
                            <asp:checkbox ID="RoomSelect" runat = "server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID">
                    </asp:BoundField>
                    <asp:BoundField DataField="RoomNumber" HeaderText="RoomNumber" 
                    SortExpression="RoomNumber"></asp:BoundField>
                    <asp:BoundField DataField="ExpirationDate" HeaderText="Expiration Date" 
                    SortExpression="ExpirationDate"></asp:BoundField>
                </Columns>
            </asp:GridView>
            <asp:Button ID="btnRemoveRoom" runat="server" Text="Remove Rooms" />
        </asp:View>
        </asp:MultiView>
        <asp:Literal ID="litError" runat="server"></asp:Literal>
</asp:Content>

