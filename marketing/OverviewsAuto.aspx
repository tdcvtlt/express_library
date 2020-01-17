<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="OverviewsAuto.aspx.vb" Inherits="marketing_OverviewsAuto" %>

<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>Location:</td>
            <td><asp:DropDownList runat="server" id = "ddLocation">
                <asp:ListItem>KCP</asp:ListItem>
                <asp:ListItem>Richmond</asp:ListItem>
                <asp:ListItem>NOVA</asp:ListItem>    
            </asp:DropDownList></td>        
        </tr>
        <tr>
            <td>Overview Date:</td>
            <td><uc1:DateField ID="dteOverView" runat="server" /></td>
        </tr>
        <tr>
            <td colspan = '2'><asp:Button runat="server" Text="Query" onclick="Unnamed1_Click"></asp:Button>
               </asp:Button></td>
        </tr>    
    </table>
    <asp:Label runat="server" id = "lblErr"></asp:Label>
    <asp:GridView runat="server" id = "gvOverViews" AutoGenerateSelectButton = "True" EmptyDataText = "No Records"></asp:GridView>
</asp:Content>
