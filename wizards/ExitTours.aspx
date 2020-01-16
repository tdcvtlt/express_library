<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="ExitTours.aspx.vb" Inherits="wizards_ExitTours" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ul id = "menu">
        <li><asp:LinkButton runat="server" id = "LinkButton1">Today's Tours</asp:LinkButton></li>
        <li><asp:LinkButton runat="server" id = "LinkButton2">Outstanding Tours</asp:LinkButton></li>
    </ul>
    <asp:MultiView runat="server" id = "MultiView1">
    <asp:View runat="server">
        <asp:GridView id = "gvTours" runat="server" 
            EmptyDataText="No Tours Being Conducted At This Time." EnableModelValidation="True">
            <Columns>
                <asp:ButtonField CommandName="Select" ShowHeader="True" Text="Check Out"></asp:ButtonField>
            </Columns>
        </asp:GridView>
<asp:Label runat="server" id = "lblErr"></asp:Label >
    </asp:View>
    <asp:View runat="server">
        <asp:GridView id = "gvOutstandingTours" runat="server" 
            EmptyDataText="No Outstanding Tours to Be Checked Out." EnableModelValidation="True">
            <Columns>
                <asp:ButtonField CommandName="Select" ShowHeader="True" Text="Check Out"></asp:ButtonField>
            </Columns>
        </asp:GridView>
    </asp:View>
    </asp:MultiView>
</asp:Content>

