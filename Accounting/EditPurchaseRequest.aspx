<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EditPurchaseRequest.aspx.vb" Inherits="Accounting_EditPurchaseRequest" %>

<%@ Register src="../controls/Events.ascx" tagname="Events" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script language="javascript" type ="text/javascript">
        function Refresh_Parts(transType) {
            if (transType == 'pReq') {
                __doPostBack('ctl00$ContentPlaceHolder1$Items_Link', '');
            }
        }
        function assign_Vendor(vendor, vendorID) {
            var str;
            str = vendor.replace(/&amp;/i, "&");
            document.getElementById("ctl00_ContentPlaceHolder1_txtVendor").value = str;
            document.getElementById("ctl00_ContentPlaceHolder1_hfVendorID").value = str;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 
    <ul id="menu">
        <li <%if  MultiView1.ActiveViewIndex = 0 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Request_Link" runat="server">Purchase Request</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 1 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Items_Link" runat="server">Purchase Req. Items</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 2 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Events_Link" runat="server">Events</asp:LinkButton></li>

    </ul>
    <asp:MultiView id = "MultiView1" runat="server">
    <asp:View runat="server" id = "prView">
    <asp:Table ID="Table1" runat="server">
        <asp:TableRow runat="server">
            <asp:TableCell runat="server">Purchase Request ID:</asp:TableCell>
            <asp:TableCell runat="server"><asp:TextBox runat="server" id = "txtPurchaseReqID" readonly="true"></asp:TextBox></asp:TableCell>
            <asp:TableCell runat="server">Date Created:</asp:TableCell>
            <asp:TableCell runat="server"><asp:TextBox runat="server" id = "txtDateCreated" readonly="true"></asp:TextBox></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow runat="server">
            <asp:TableCell runat="server">Purchase Order ID:</asp:TableCell>
            <asp:TableCell runat="server"><asp:TextBox runat="server" id = "txtPurchaseOrderID" readonly="true"></asp:TextBox></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow runat="server">
            <asp:TableCell runat="server">Vendor:</asp:TableCell>
            <asp:TableCell runat="server"><asp:TextBox runat="server" id = "txtVendor" readonly = "true"></asp:TextBox></asp:TableCell>
            <asp:TableCell runat="server"><asp:Button runat="server" Text="Add Vendor" id = "btnVendor" visible = "False"></asp:Button></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow runat="server" visible = "False">
            <asp:TableCell runat="server">Vendor Description:</asp:TableCell>
            <asp:TableCell runat="server"><asp:TextBox runat="server" id = "txtVendorDesc" readonly="true"></asp:TextBox></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow runat="server">
            <asp:TableCell runat="server">Status:</asp:TableCell>
            <asp:TableCell runat="server"><asp:TextBox runat="server" id = "txtStatus" readonly="true"></asp:TextBox></asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <asp:HiddenField runat="server" id = "hfVendorID"></asp:HiddenField>
    <asp:Button runat="server" Text="Save" id = "btnSave"></asp:Button> <asp:Button runat="server" id = "btnPrint" Text="Print Request" visible = "False"></asp:Button><asp:Button runat="server" Text="Approve" id = "btnApprove" visible = "False"></asp:Button><asp:Button runat="server" Text="Deny" id = "btnDeny" visible = "False"></asp:Button>
    <asp:Label runat="server" id = "lblPRErr"></asp:Label>
    </asp:View>
    <asp:View runat="server" id = "prItemsView">
        <asp:GridView runat="server" id = "gvPRItems" EnableModelValidation="True" Autogeneratecolumns = "False" emptyDataText = "No Records" OnRowDataBound = "gvPRItems_RowDataBound">
        <Columns>
        <asp:BoundField HeaderText="ID" DataField="Item2RequestID"></asp:BoundField>
        <asp:BoundField HeaderText="Item Number" DataField="ItemNumber"></asp:BoundField>
        <asp:BoundField HeaderText="Description" DataField="Description"></asp:BoundField>
        <asp:BoundField HeaderText="Qty" DataField="Qty"></asp:BoundField>
        <asp:BoundField HeaderText="Amount" DataField="Amount"></asp:BoundField>
        <asp:BoundField HeaderText="Location" DataField="Location"></asp:BoundField>
        <asp:BoundField HeaderText="Purpose" DataField="Purpose"></asp:BoundField>
        <asp:BoundField HeaderText="EditItem" DataField="EditItem"></asp:BoundField>
        <asp:ButtonField CommandName="EditItem" Text="Edit"></asp:ButtonField>
        <asp:BoundField HeaderText="Remove" DataField="Remove"></asp:BoundField>
        <asp:ButtonField CommandName="RemovePart" Text="Remove"></asp:ButtonField>
        </Columns></asp:GridView>
        <asp:Button runat="server" Text="Add Item" onclick="Unnamed1_Click"></asp:Button>
    </asp:View>
    <asp:View runat="server" id = "EventsView">
        <uc1:Events ID="Events1" runat="server" />
    </asp:View>
    </asp:MultiView>
</asp:Content>

