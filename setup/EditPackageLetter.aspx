<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EditPackageLetter.aspx.vb" Inherits="setup_EditPackageLetter" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Literal ID="Literal1" runat="server">Name:</asp:Literal> 
    <asp:TextBox ID="txtName" runat="server" Width="227px"></asp:TextBox><asp:Button
        ID="Button1" runat="server" Text="Save Letter" />
    <asp:LinkButton ID="lbTagMap" runat="server">Tag Mapping</asp:LinkButton>
        <CKEditor:CKEditorControl ID="CKEditor1" runat="server" Height = "800" Width = "1000"></CKEditor:CKEditorControl>
</asp:Content>

