<%@ Page Title="Pender Review" AspCompat="true" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="PenderReview.aspx.vb" Inherits="Reports_Accounting_PenderReview" %>

<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!--Date:--> <uc1:DateField ID="dfDate" runat="server" Visible="False" />
    <asp:RadioButtonList ID="rbList" runat="server" 
        RepeatDirection="Horizontal">
        <asp:ListItem>Line</asp:ListItem>
        <asp:ListItem>Exit</asp:ListItem>
        
    </asp:RadioButtonList>
    
    <asp:Button ID="btnReport" runat="server" Text="Run Report" />
    <asp:Button ID="btnExcel" runat="server" Text="Excel" /><br />
    <asp:Literal ID="Lit1" runat="server"></asp:Literal>

    

    

    

</asp:Content>

