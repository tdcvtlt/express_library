<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="OpenWorkOrders.aspx.vb" Inherits="Reports_Accounting_OpenWorkOrders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:button runat="server" text="Export" id="btnExport" />
    <asp:gridview id="gbWorkOrders" runat="server" DataKeyNames="RequestID" AutoGenerateColumns="False" 
    EmptyDataText="No Records" GridLines="Horizontal" EnableModelValidation="True">
        <SelectedRowStyle BackColor="#CCFFFF" Wrap="True" />
        <AlternatingRowStyle BackColor="#CCFFCC" />        
        <Columns>
            <asp:HyperLinkField HeaderText="RequestID" 
                        DataNavigateUrlFields="RequestID" 
                        DataNavigateUrlFormatString="~/Maintenance/editRequest.aspx?requestid={0}" DataTextField="RequestID" 
                        />
                    
                              
            <asp:BoundField DataField="RequestStatus" HeaderText="Req Status" />
            <asp:BoundField DataField="ItemNumber" HeaderText="ItemNumber" />
            <asp:BoundField DataField="QTY" HeaderText="Qty" />
            <asp:BoundField DataField="ItemDesc" HeaderText="Item Desc" />
            <asp:BoundField DataField="ItemStatus" HeaderText="Item Status" />
                    
                              
        </Columns></asp:gridview>
</asp:Content>

