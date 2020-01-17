<%@ Page Title="Equiant Exceptions" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EquiantExceptions.aspx.vb" Inherits="Accounting_EquiantExceptions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <fieldset>
        <legend>Exceptions</legend>
    
        <asp:Button runat="server" ID="btnExport" Text="Export to Excel" />
        <div>
            <asp:GridView ID="gvExceptions" runat="server" AllowSorting="true"  OnSorting="Exceptions_Sorting"></asp:GridView>
        </div>
    </fieldset>
    <fieldset>
        <legend>Duplicate Transactions</legend>
        <asp:Button runat="server" ID="btnExportDuplicates" Text="Export Duplicates to Excel" />
        <asp:Button runat="server" ID="btnAckAll" Text="Acknowledge All" />
        <div>
            <asp:GridView ID="gvDuplicates" runat="server"></asp:GridView>
        </div>
    </fieldset>
    
</asp:Content>

