<%@ Page Title="Master Inventory List" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="MasterInventoryList.aspx.vb" Inherits="Accounting_MasterInventoryList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <iframe style = "width:600px;height:600px;overflow:auto;" 
        src="imported/masterinventorylist.asp" frameborder="0"></iframe>
</asp:Content>

