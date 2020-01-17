<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="RefundRequests.aspx.vb" Inherits="Accounting_RefundRequests" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    Account: <asp:DropDownList runat="server" id = "ddAccounts"></asp:DropDownList>
    <asp:Button runat="server" Text="Query" onclick="Unnamed1_Click"></asp:Button>
    <asp:GridView runat="server" id = "gvRefundRequests" 
        EmptyDataText = "No Requests For This Account." EnableModelValidation="True" autoGenerateColumns = "False" OnRowDataBound = "gvRefundRequests_RowDataBound">
        <Columns>
           <asp:BoundField DataField="ID" HeaderText="ID"></asp:BoundField>
            <asp:BoundField DataField="Prospect" HeaderText="Name"></asp:BoundField>
            <asp:BoundField DataField="Account" HeaderText="Account"></asp:BoundField>
            <asp:BoundField DataField="Amount" HeaderText="Amount"></asp:boundField>
            <asp:BoundField DataField="RequestedBy" HeaderText="Requested By"></asp:BoundField>
            <asp:BoundField DataField="DateRequested" HeaderText="Date Requested"></asp:BoundField>
            <asp:BoundField DataField="Reason" HeaderText="Reason"></asp:BoundField>
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
    <asp:Button runat="server" Text="Process Selected" id = "btnProcess" onclick="Unnamed2_Click" visible = "False" OnClientClick="this.disabled=true;__doPostBack('ctl00$ContentPlaceHolder1$btnProcess','');"></asp:Button>
    <asp:Label runat="server" id = "lblErr"></asp:Label>
</asp:Content>

