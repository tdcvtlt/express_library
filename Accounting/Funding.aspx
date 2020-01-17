<%@ Page Title="Funding" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="Funding.aspx.vb" Inherits="Accounting_Funding" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ul id = "menu">
        <li><asp:LinkButton runat="server" id = "LinkButton1">Fundings</asp:LinkButton></li>
        <li><asp:LinkButton runat="server" id = "LinkButton2">Exit Fundings</asp:LinkButton></li>
    </ul>
    <asp:MultiView runat="server" id = "MultiView1">
        <asp:View runat="server" id = "fundingView">
        Funding #: <asp:TextBox runat="server" id = "txtFunding"></asp:TextBox><asp:Button runat="server" Text="List" id = "btnListFunding"></asp:Button><asp:Button runat="server" Text="New" id = "btnNewFunding"></asp:Button>
        <div style="height:600px;width:600px;overflow:auto;"><asp:GridView runat="server" AutoGenerateSelectButton="True" 
                EmptyDataText="No Records" id = "gvFundings" onRowDataBound = "gvFundings_RowDataBound"></asp:GridView></div>
        </asp:View>    
        <asp:View runat="server" id = "exitfundingView">
        Funding #: <asp:TextBox runat="server" id = "txtExitFunding"></asp:TextBox><asp:Button runat="server" Text="List" id = "btnListExitFunding"></asp:Button><asp:Button runat="server" Text="New" id = "btnNewExitFunding"></asp:Button> 
        <div style="height:600px;width:600px;overflow:auto;"><asp:GridView runat="server" AutoGenerateSelectButton="True" 
                EmptyDataText="No Records" id = "gvExitFundings" onRowDataBound = "gvExitFundings_RowDataBound"></asp:GridView></div>
        </asp:View>
    </asp:MultiView>
</asp:Content>

