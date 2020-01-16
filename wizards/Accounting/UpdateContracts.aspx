<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="UpdateContracts.aspx.vb" Inherits="wizards_Accounting_UpdateContracts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="../../scripts/scw.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:table ID="table1" runat="server" EnableViewState="False">
        <asp:tablerow>
            <asp:tableheadercell>KCP</asp:tableheadercell>
            <asp:tableheadercell>Conveyance instrument #</asp:tableheadercell>
            <asp:tableheadercell>Date recorded</asp:tableheadercell>
            <asp:tableheadercell>Conveyance Type </asp:tableheadercell>
        </asp:tablerow>

    </asp:table>
</asp:Content>

