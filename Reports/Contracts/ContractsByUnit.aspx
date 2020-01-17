<%@ Page Title="Contracts By Unit" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="ContractsByUnit.aspx.vb" Inherits="Reports_Contracts_ContractsByUnit" aspCompat ="true"%>

<%@ Register src="../../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
    <tr>
        <td>
            Unit:
        </td>
        <td>
        
            <asp:ListBox ID="liUnit" runat="server"></asp:ListBox>
        
        </td>
    </tr>
    <tr>
    <td>
        <asp:Button ID="btnReport" runat="server" Text="Get_Report" />
    
    </td>
    <td>
        <asp:Button ID="btnExcel" runat="server" Text="Excel" />
    </td>
    </tr>
    </table>
    <asp:Literal ID="litReport" runat="server"></asp:Literal>
    
</asp:Content>

