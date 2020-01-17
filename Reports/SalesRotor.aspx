<%@ Page Title="Sales Rotation" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="SalesRotor.aspx.vb" Inherits="Reports_SalesRotor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Literal runat="server" id = "SalesRotor" Mode="PassThrough"></asp:Literal>
<asp:Label runat="server" id = "lblErr"></asp:Label>
</asp:Content>

