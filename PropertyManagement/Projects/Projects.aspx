<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="Projects.aspx.vb" Inherits="PropertyManagement_Projects_Projects" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    Filter: <asp:DropDownList ID="ddFilter" runat="server">
        <asp:ListItem Value="Name">Title</asp:ListItem>
        <asp:ListItem Value="ProjectID">Project Number</asp:ListItem>
    </asp:DropDownList><br />
    Enter search value: <br />
    <asp:TextBox ID="txtFilter" runat="server"></asp:TextBox><asp:Button ID="btnSearch"
        runat="server" Text="Query" /><asp:Button ID="btnNew"
        runat="server" Text="New" /><br />
    <asp:GridView ID="gvProjects" runat="server">
        <AlternatingRowStyle BackColor="#C7E3D7" />
        <Columns>
            <asp:TemplateField HeaderText="Select">
                <ItemTemplate>
                    <a href="EditProject.aspx?ID=<%#container.Dataitem("ID")%>" title="Edit">Edit</a>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
</asp:Content>

