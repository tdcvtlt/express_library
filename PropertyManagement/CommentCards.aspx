<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="CommentCards.aspx.vb" Inherits="PropertyManagement_CommentCards" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>
                <asp:Label ID="lblID" runat="server" Text="CardID"></asp:Label>
                <asp:TextBox ID="txtID" runat="server"></asp:TextBox>        
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="Button1" runat="server" Text="List" />
                <asp:Button ID="Button2" runat="server" Text="New" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblErr" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>

    <asp:GridView ID="GridView1" runat="server" AutoGenerateSelectButton="True" 
                        EmptyDataText="No Records" GridLines="Horizontal">
        <SelectedRowStyle BackColor="#CCFFFF" Wrap="True" />
        <AlternatingRowStyle BackColor="#CCFFCC" />
    </asp:GridView>



    <asp:GridView ID="GridView2" runat="server">
    </asp:GridView>
    
</asp:Content>

