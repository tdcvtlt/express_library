<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="CCApprovals.aspx.vb" Inherits="Accounting_CCApprovals" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    Account: <asp:DropDownList runat="server" id = "ddAccounts"></asp:DropDownList>
    <asp:Button runat="server" Text="Query" onclick="Unnamed1_Click"></asp:Button>
    <asp:GridView runat="server" id = "gvTransactions" 
        EmptyDataText = "No Requests For This Account." EnableModelValidation="True" onRowDataBound = "gvTransactions_RowDataBound" autogeneratecolumns = "false">
        <Columns>
           <asp:BoundField DataField="ID" HeaderText="ID"></asp:BoundField>
            <asp:BoundField DataField="Prospect" HeaderText="Name"></asp:BoundField>
            <asp:BoundField DataField="Account" HeaderText="Account"></asp:BoundField>
            <asp:BoundField DataField="Amount" HeaderText = "Amount" />
            <asp:BoundField DataField="DateCreated" HeaderText="Date Requested"></asp:BoundField>
            <asp:BoundField DataField="Number" HeaderText="Number"></asp:BoundField>
            <asp:BoundField DataField="Expiration" HeaderText="Expiration"></asp:BoundField>
            <asp:BoundField DataField="TransType" HeaderText="TransType"></asp:BoundField>
            <asp:BoundField DataField="RequestedBy" HeaderText="Requested By"></asp:BoundField>
            <asp:TemplateField HeaderText="Approve">
                <ItemTemplate>
                    <asp:CheckBox ID="cbApprove" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Deny">
                <ItemTemplate>
                    <asp:CheckBox ID="cbDeny" runat="server" />
                </ItemTemplate>
            </asp:TemplateField>     
            <asp:TemplateField HeaderText="Details">
                <ItemTemplate>
                    <asp:LinkButton runat="server" id = "lbDetails" onclick="lbDetails_Click">Details</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>    
    </asp:GridView>
    <asp:Button runat="server" Text="Process Selected" id = "btnProcess" 
        onclick="Unnamed2_Click" visible = "False" 
        onclientclick="this.disabled = true;__doPostBack('ctl00$ContentPlaceHolder1$btnProcess','')"></asp:Button>
    <asp:Label runat="server" id = "lblErr"></asp:Label>
</asp:Content>

