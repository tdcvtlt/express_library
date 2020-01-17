<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="Queues.aspx.vb" Inherits="setup_Queues" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="Table">
        <table>
            <tr>
                <td>Search Queues:</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td><asp:TextBox ID="txtQueue" runat="server"></asp:TextBox></td>
                <td><asp:Button ID="btnSearch" runat="server" Text="Query" /></td>
                <td><asp:Button ID="btnNew" runat="server" Text="New" /></td>
            </tr>
        </table>
    </div>
    <div id="Grid">
        <asp:GridView ID="GridView1" runat="server" AutoGenerateSelectButton="false" DataKeyNames="QueueID" AutoGenerateColumns="true" EmptyDataText="No Records" GridLines="Horizontal">
            <SelectedRowStyle BackColor="#CCFFFF" Wrap="true" />
            <AlternatingRowStyle BackColor="#CCFFCC" />
            <Columns>
                <asp:HyperLinkField HeaderText="Edit" DataNavigateUrlFields="QueueID" DataNavigateUrlFormatString="editqueues.aspx?queueid={0}" DataTextField="QueueID" />
            </Columns>
        </asp:GridView>

    </div>
</asp:Content>

