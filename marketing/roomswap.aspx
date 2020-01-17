<%@ Page Title="" Language="VB" AutoEventWireup="false" CodeFile="roomswap.aspx.vb" Inherits="marketing_roomswap" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<body>
    <form id="form1" runat="server">
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View1" runat="server">
            Reason: <asp:TextBox runat="server" id = "reasonTxt" Width="264px"></asp:TextBox>
            <asp:Button runat="server" Text="Search" onclick="Unnamed1_Click"></asp:Button>
        </asp:View>
        <asp:View ID="View2" runat="server">
            Reason: <asp:TextBox runat="server" id = "reasonIHTxt" Width="270px"></asp:TextBox>
            <br />
            Date Moved: <asp:DropDownList runat="server" id = "ddInDate"></asp:DropDownList>
            <asp:Button runat="server" Text="Search" onclick="Unnamed2_Click"></asp:Button>
        </asp:View>
        <asp:View ID="View3" runat="server">
            <asp:label runat="server" id="lblRoomNumber"></asp:label> is part of a Multi-Bedroom Reservation. <br />Would you like to swap anyway?
            <br /><asp:Button runat="server" Text="Yes" id = "yesSwapBtn"></asp:Button> <asp:Button runat="server" Text="No" id = "noSwapBtn"></asp:Button>
            <asp:label runat="server" id="lblReason" visible = "false"></asp:label>
            <asp:label runat="server" id="lblRoomID" visible = "false"></asp:label>

        </asp:View>
        <asp:View runat="server" id = "View4">ACCESS DENIED</asp:View>
        <asp:View ID="View5" runat="server">
            <asp:Label runat="server" ID="lblUsageMessage"></asp:Label> is part of a Multi-Bedroom Usage. <br />Would you like to swap anyway?
            <br />
            <asp:Button runat="server" Text="Yes" id = "btnUsageYes"></asp:Button> <asp:Button runat="server" Text="No" id = "BtnUsageNo"></asp:Button>
            <asp:label runat="server" id="lblUsageReason" visible = "false"></asp:label>
            <asp:label runat="server" id="lblUsageRoomID" visible = "false"></asp:label>
        </asp:View>

    </asp:MultiView>
    <asp:GridView ID="gvRooms" runat="server" AutoGenerateSelectButton="True" 
        EmptyDataText="No Records">
    </asp:GridView>

    <asp:label id="lblSpares" runat="server" visible = "false"><B>Spares</B><br /></asp:label>
    <asp:GridView ID="gvSpares" runat="server" AutoGenerateSelectButton="True" 
        EmptyDataText="No Records">
    </asp:GridView>

    <asp:label id = "lblErr" runat="server"></asp:label>
    </form> 
</body>
</html>