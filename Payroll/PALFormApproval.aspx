<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="PALFormApproval.aspx.vb" Inherits="Payroll_PALFormApproval" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:MultiView runat="server" ID="MultiView1">
    <asp:View runat="server" id = "View_1">
        <asp:GridView runat="server" id = "gvPendingRequests" 
            autoGenerateColumns = "False" EmptyDataText = "No Pending PAL Requests" 
            EnableModelValidation="True">
            <Columns>
                <asp:BoundField DataField="ID" HeaderText="ID"></asp:BoundField>
                <asp:BoundField DataField="Employee" HeaderText="Employee"></asp:BoundField>
                <asp:BoundField DataField="Department" HeaderText="Department"></asp:BoundField>
                <asp:BoundField DataField="DateCreated" HeaderText="Date Submitted"></asp:BoundField>
                <asp:BoundField DataField="TotalPALHours" HeaderText="PAL Hours"></asp:BoundField>
                <asp:BoundField DataField="TotalSSLBHours" HeaderText="SSLB Hours"></asp:BoundField>
                <asp:BoundField DataField="TotalUnpaidHours" HeaderText="Unpaid Hours"></asp:BoundField>
                <asp:BoundField DataField="Leave Type" HeaderText="Leave Type"></asp:BoundField>

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
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbDetails" runat="server" onclick="lbDetails_Click">Details</asp:LinkButton>
                    </ItemTemplate>                        
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:Button ID="btnProcessPending" runat="server" Text="Process PAL Requests"></asp:Button>
    </asp:View>
</asp:MultiView>
</asp:Content>

