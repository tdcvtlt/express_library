<%@ Page Title="Lead Templates" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="LeadTemplates.aspx.vb" Inherits="LeadManagement_LeadTemplates" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="ListGrid">
        <asp:GridView ID="gvTemplates" runat="server" AutoGenerateSelectButton="False" DataKeyNames="LeadTemplateID" AutoGenerateColumns="true" 
                EmptyDataText="No Records" GridLines="Horizontal">
                <SelectedRowStyle BackColor="#CCFFFF" Wrap="True" />
                <AlternatingRowStyle BackColor="#CCFFCC" />        
                <Columns>
                    <asp:HyperLinkField HeaderText="Edit" 
                                DataNavigateUrlFields="LeadTemplateID" 
                                DataNavigateUrlFormatString="editLeadTemplate.aspx?LeadTemplateID={0}" DataTextField="LeadTemplateID" 
                                />
                    
                              
                </Columns>

        </asp:GridView>
    </div>
    <ul id="menu">
        <li><asp:LinkButton ID="lbAdd" runat="server">Add New</asp:LinkButton></li>
    </ul>
</asp:Content>

