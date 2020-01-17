<%@ Page Title="Rental Pool Limits" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="RentalPoolLimits.aspx.vb" Inherits="PropertyManagement_RentalPoolLimits" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>
                Select Year:
            </td>
            <td>
                <asp:DropDownList ID="ddYears" runat="server" DataTextField="Year" DataValueField="Year" />
            </td>
            <td><asp:Button runat="server" Text="View/Edit Year" onclick="Unnamed1_Click"></asp:Button></td>
            <td><asp:Button runat="server" Text="Add Year" onclick="Unnamed2_Click"></asp:Button></td>
        </tr>
    </table>
    <asp:Label runat="server" id = "lblErr"></asp:Label>
    <div style="height: 476px; overflow:scroll; width: 501px;">
    <asp:GridView ID="gvPoolLimits" runat="server" EnableModelValidation="True" EmptyDataText = "No Records" AutoGenerateColumns = "False" onRowDataBound = "gvPoolLimits_RowDataBound">
        <Columns>
            <asp:BoundField DataField="ID" HeaderText="ID"></asp:BoundField>
            <asp:BoundField DataField="UnitType" HeaderText="UnitType"></asp:BoundField>
            <asp:BoundField DataField="RoomType" HeaderText="RoomType"></asp:BoundField>
            <asp:BoundField DataField="Category" HeaderText="Category"></asp:BoundField>
            <asp:BoundField DataField="Qty" HeaderText="Qty"></asp:BoundField>
            <asp:TemplateField HeaderText="Qty Allowed">
                <ItemTemplate>
                    <asp:TextBox ID="txtQtyAllowed" runat="server"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="QtyUsed" HeaderText="Qty Used"></asp:BoundField>
        </Columns>
    </asp:GridView>
    </div>
    <asp:Button runat="server" Text="Save" Visible="False" id = "btnSave"></asp:Button>
</asp:Content>

