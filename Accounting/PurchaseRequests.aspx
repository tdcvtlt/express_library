<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="PurchaseRequests.aspx.vb" Inherits="Accounting_PurchaseRequests" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <p>
        Filter: <asp:DropDownList ID="ddPRStatus" runat="server"></asp:DropDownList>
        <br />
        Enter Purchase Request ID:
        <br />
        <asp:TextBox runat="server" id = "txtFilter"></asp:TextBox>
        <asp:Button runat="server" Text="Query" onclick="Unnamed1_Click"></asp:Button>
        <asp:Button runat="server" Text="New" onclick="Unnamed2_Click"></asp:Button>
    </p>
    <div style="height: 359px">
        <asp:GridView runat="server" id = "gvPReq" AutoGenerateSelectButton="True" 
            EmptyDataText="No records" onRowDataBound = "gvPReq_RowDataBound"></asp:GridView>
    </div>
</asp:Content>

