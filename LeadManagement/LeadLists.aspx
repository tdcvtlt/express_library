<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="LeadLists.aspx.vb" Inherits="LeadManagement_LeadLists" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:GridView ID="gvLeadLists" runat="server" EnableModelValidation="True" AutoGenerateColumns="false" EmptyDataText="No Lists Created" OnRowDataBound = "gvLeadLists_RowDataBound">
        <Columns>
            <asp:BoundField HeaderText="ListID" DataField="ID"></asp:BoundField>
            <asp:BoundField HeaderText="Date Created" DataField="DateCreated"></asp:BoundField>
            <asp:BoundField HeaderText="Date Revoked" DataField="DateRevoked"></asp:BoundField>
            <asp:BoundField HeaderText="Vendor" DataField="Vendor"></asp:BoundField>
            <asp:BoundField HeaderText="Lead Count" DataField="Leads"></asp:BoundField>
            <asp:BoundField HeaderText="Description" DataField="Description"></asp:BoundField>                            
            <asp:ButtonField CommandName="Revoke List" Text="Revoke List"></asp:ButtonField>
        </Columns>
    </asp:GridView>
                <ul id="menu">
                <li>
                    <asp:LinkButton ID="lbAdd" runat="server">Create Lead List</asp:LinkButton></li>
            </ul>
</asp:Content>

