<%@ Page Title="Printers" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="Printers.aspx.vb" Inherits="setup_Printers_Printers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ul id="menu">
        <li><asp:LinkButton ID="Add" runat="server">Add</asp:LinkButton></li>
    </ul>
    <asp:gridview id="gvPrinters" runat="server" AutoGenerateSelectButton="False" DataKeyNames="PrinterID" AutoGenerateColumns="true" 
    EmptyDataText="No Records" GridLines="Horizontal">
        <SelectedRowStyle BackColor="#CCFFFF" Wrap="True" />
        <AlternatingRowStyle BackColor="#CCFFCC" />        
        <Columns>
            <asp:HyperLinkField HeaderText="Edit" 
                        DataNavigateUrlFields="PrinterID" 
                        DataNavigateUrlFormatString="editPrinter.aspx?PrinterID={0}" DataTextField="PrinterID" 
                        />
                    
                              
        </Columns>
    </asp:gridview>
</asp:Content>

