<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="DoNotSellListOverRide.aspx.vb" Inherits="Accounting_DoNotSellListOverRide" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    Press the Button Below to Generate An OverRide Code For the Do Not Sell List: </br></br>
    Code: <asp:TextBox ID="txtCode" runat="server"></asp:TextBox> </br>
    <asp:Label ID="lbErr" runat="server" Text=""></asp:Label>
    </br>
    <asp:Button ID="Button1" runat="server" Text="Generate OverRide Code" />
</asp:Content>

