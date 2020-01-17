<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="InventoryVSContract.aspx.vb" Inherits="Reports_Accounting_OpenWorkOrders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:button runat="server" text="Export" id="btnExport" />
    <asp:gridview id="gbWorkOrders" runat="server"  AutoGenerateColumns="False" 
    EmptyDataText="No Records" GridLines="Horizontal" EnableModelValidation="True">
        <SelectedRowStyle BackColor="#CCFFFF" Wrap="True" />
        <AlternatingRowStyle BackColor="#CCFFCC" />        
        <Columns>
            <asp:BoundField DataField="ContractNumber" HeaderText="KCP" />
            <asp:BoundField DataField="SubType" HeaderText="Sub Type" />
            <asp:BoundField DataField="Season" HeaderText="Season" />
            <asp:BoundField DataField="Frequency" HeaderText="Frequency" />
            <asp:BoundField DataField="Week" HeaderText="Week" />
            <asp:BoundField DataField="UnitType" HeaderText="Unit Type" />
            <asp:BoundField DataField="InventoryFrequency" HeaderText="Inv Frequency" />                  
            <asp:BoundField DataField="Size" HeaderText="Inventory Size" />   
            <asp:BoundField DataField="InvSeason" HeaderText="Inventory Season" />   
        </Columns></asp:gridview>
</asp:Content>

