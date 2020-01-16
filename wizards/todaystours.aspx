<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="todaystours.aspx.vb" Inherits="wizards_todaystours" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ul id = "menu">
        <li><asp:LinkButton runat="server" id = "LinkButton1">KCP</asp:LinkButton></li>
        <li><asp:LinkButton runat="server" id = "lbWoodbridge">Woodbridge</asp:LinkButton></li>
        <li><asp:LinkButton runat="server" id = "LinkButton2">Vacation Club</asp:LinkButton></li>    
        <li><asp:LinkButton runat="server" id = "LinkButton3">Insert Tour</asp:LinkButton></li>
    </ul>
<asp:MultiView runat="server" id = "MultiView1">
<asp:View runat="server">
<input type="button" value="Printable" onclick="var mwin = window.open();mwin.document.write(document.getElementById('printable').innerHTML);" />
<div id="printable">
    <asp:GridView id = "gvTours" runat="server" 
        EmptyDataText="No Tours Scheduled Today." EnableModelValidation="True">
    <Columns>
        <asp:ButtonField CommandName="Select" ShowHeader="True" Text="Check-In"></asp:ButtonField>
    </Columns>
    </asp:GridView>
</div>
</asp:View>
<asp:View runat="server">
Phone Number: <asp:TextBox runat="server" id = "txtPhoneNumber"></asp:TextBox>
    <asp:Button runat="server" Text="Search" onclick="Unnamed3_Click"></asp:Button>
<br />
<asp:GridView runat="server" id = "gvPros" EmptyDataText = "No Records" 
        AutoGenerateSelectButton="True"></asp:GridView>
        <ul id = "menu">
            <li><asp:LinkButton runat="server" id = "LinkButton4">Insert New Prospect</asp:LinkButton></li>
        </ul>
</asp:View>
</asp:MultiView>
</asp:Content>

