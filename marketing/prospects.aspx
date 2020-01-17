<%@ Page Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="prospects.aspx.vb" Inherits="prospects" title="Prospect Search" %>
<%@ OutputCache Duration="1" VaryByParam="none" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="Label2" runat="server" Text="Filter: "></asp:Label>
    <asp:DropDownList ID="ddFilter" runat="server">
        <asp:ListItem Value="Address1">Address1</asp:ListItem>
        <asp:ListItem Value="City">City</asp:ListItem>
        <asp:ListItem Value="Email">Email</asp:ListItem>
        <asp:ListItem Value="ID">ID</asp:ListItem>
        <asp:ListItem Value="Name">Name</asp:ListItem>
        <asp:ListItem Selected="True" Value="Phone">Phone</asp:ListItem>
        <asp:ListItem Value="PostalCode">PostalCode</asp:ListItem>
        <asp:ListItem Value ="spousename">Spouse Name</asp:ListItem>
        <asp:ListItem Value="SpouseSSN">SpouseSSN</asp:ListItem>
        <asp:ListItem Value="SSN">SSN</asp:ListItem>
        <asp:ListItem Value="State">State</asp:ListItem>
        <asp:ListItem Value="Club Explore Membership">Club Explore Membership</asp:ListItem>
    </asp:DropDownList>
    <br />
    <br />
    <asp:Label ID="Label1" runat="server" Text="Enter Home Phone:"></asp:Label><br />
    <asp:TextBox ID="filter" runat="server"></asp:TextBox>
    <asp:Button ID="Button1"
        runat="server" Text="Query" />
    <asp:Button ID="btnNew" runat="server" Text="New" />
    <br />
    <div style="height:200px;width:600px;overflow:auto; ">
    <asp:GridView ID="GridView1" runat="server" AutoGenerateSelectButton="False" DataKeyNames="ProspectID" AutoGenerateColumns="true" 
    EmptyDataText="No Records" GridLines="Horizontal">
        <SelectedRowStyle BackColor="#CCFFFF" Wrap="True" />
        <AlternatingRowStyle BackColor="#CCFFCC" />        
        <Columns>
            <asp:HyperLinkField HeaderText="Edit" 
                        DataNavigateUrlFields="ProspectID" 
                        DataNavigateUrlFormatString="editprospect.aspx?prospectid={0}" DataTextField="ProspectID" 
                        />
                    
                              
        </Columns>
    </asp:GridView>
    </div>
    <asp:Label runat="server" id = "lblErr"></asp:Label>

    <br />
    
</asp:Content>

