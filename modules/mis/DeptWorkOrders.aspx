<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="DeptWorkOrders.aspx.vb" Inherits="mis_DeptWorkOrders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <ul id="menu">
        <li <%if  MultiView1.ActiveViewIndex = 0 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Submitted_Link" runat="server">Active</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 1 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="MgrApproval_Link" runat="server">Pending Manager Approval</asp:LinkButton></li>
    </ul>

    <asp:MultiView runat="server" id = "MultiView1">
        <asp:View runat="server" id = "View1">
            <asp:GridView runat="server" id = "gvActiveWO" AutoGenerateSelectButton = "true" EmptyDataText = "No Work Orders"></asp:GridView>
        </asp:View>
        <asp:View runat="server" id = "View2">
            <asp:GridView runat="server" id = "gvPendingWO" AutoGenerateSelectButton = "true" EmptyDataText = "No Work Orders"></asp:GridView>
        </asp:View>
    </asp:MultiView>

</asp:Content>

