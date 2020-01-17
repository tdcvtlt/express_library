<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="TollNumber.aspx.vb" Inherits="setup_TollNumber" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="table">
    <table>
        <tr>
            <td>
                Search Toll Numbers:
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="txtToll" runat="server"></asp:TextBox> 
            </td>
            <td>
                <asp:Button ID="btnSearch" runat="server" Text="Search" /><asp:Button ID="btnNew"
                    runat="server" Text="Add New" />
            </td>
        </tr>
    </table>
    </div>
    <div id = "Grid">
        <div id="Div1">
        <asp:GridView ID="GridView1" runat="server" AutoGenerateSelectButton="false" DataKeyNames="QueueID" AutoGenerateColumns="true" EmptyDataText="No Records" GridLines="Horizontal">
            <SelectedRowStyle BackColor="#CCFFFF" Wrap="true" />
            <AlternatingRowStyle BackColor="#CCFFCC" />
            <Columns>
                <asp:HyperLinkField HeaderText="Edit" DataNavigateUrlFields="TollID" DataNavigateUrlFormatString="editTollNumbers.aspx?tollid={0}" DataTextField="TollID" />
            </Columns>
        </asp:GridView>

    </div>
    </div>
</asp:Content>

