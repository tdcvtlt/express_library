<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="PMSchedule.aspx.vb" Inherits="Maintenance_PMSchedule" %>

<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox><asp:TextBox ID="TextBox2"
        runat="server"></asp:TextBox>
    
    <uc1:DateField ID="DateField1" runat="server" />
    <asp:Label ID="Label3" runat="server" Text="Label1"></asp:Label>
    <asp:DropDownList ID="DropDownList1" runat="server">
    </asp:DropDownList>
    <asp:Label ID="Label2" runat="server" Text="Label2"></asp:Label>
    <asp:DropDownList ID="DropDownList2" runat="server">
    </asp:DropDownList>
    <asp:Label ID="Label1" runat="server" Text="Label3"></asp:Label>
    <asp:DropDownList ID="DropDownList3" runat="server">
    </asp:DropDownList>
    <asp:Button ID="Button1" runat="server" Text="Don't Click" />
</asp:Content>

