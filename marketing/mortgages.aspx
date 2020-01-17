<%@ Page Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="mortgages.aspx.vb" Inherits="marketing_mortgages" title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
Filter: <asp:DropDownList ID="ddFilter" runat="server">
        <asp:ListItem Value="ContractNumber">Contract Number</asp:ListItem>
        <asp:ListItem Value="MortgageID">MortgageID</asp:ListItem>
        
    </asp:DropDownList><br />
    Enter search value: <br />
    <asp:TextBox ID="txtFilter" runat="server"></asp:TextBox><asp:Button ID="btnSearch" runat="server" Text="Query" /><br />
    <asp:GridView ID="gvMortgages" runat="server">
        <AlternatingRowStyle BackColor="#C7E3D7" />
        <Columns>
            <asp:TemplateField HeaderText="Select">
                <ItemTemplate>
                    <a href="<%=request.applicationpath%>/marketing/editmortgage.aspx?mortgageid=<%#container.Dataitem("ID")%>" title="Edit">Edit</a>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
</asp:Content>

