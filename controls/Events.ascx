<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Events.ascx.vb" Inherits="controls_Events" %>
<div class="ListGrid">
    <asp:GridView ID="gvEvents" runat="server" EmptyDataText="No Records">
        <AlternatingRowStyle BackColor="#C7E3D7" />
    </asp:GridView>
    <asp:Label ID="lblError" runat="server" Text="Label" ForeColor="Red"></asp:Label>
</div>
<asp:HiddenField ID="hfKeyValue" Value = "0" runat="server" />
<asp:HiddenField ID="hfKeyField" Value = "" runat="server" />
