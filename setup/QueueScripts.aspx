<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="QueueScripts.aspx.vb" Inherits="setup_QueueScripts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="table">
    <table>
        <tr>
            <td>
                ScriptID:
            </td>
            <td>
            
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="txtScript" runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:Button ID="btnSearch" runat="server" Text="Search" /><asp:Button ID="btnNew"
                    runat="server" Text="New" />
            </td>
        </tr>
    </table>
    </div>
    <div id="grid">
        <div id="Div1">
        <asp:GridView ID="GridView1" runat="server" AutoGenerateSelectButton="false" DataKeyNames="ScriptID" AutoGenerateColumns="true" EmptyDataText="No Records" GridLines="Horizontal">
            <SelectedRowStyle BackColor="#CCFFFF" Wrap="true" />
            <AlternatingRowStyle BackColor="#CCFFCC" />
            <Columns>
                <asp:HyperLinkField HeaderText="Edit" DataNavigateUrlFields="ScriptID" DataNavigateUrlFormatString="editScripts.aspx?scriptid={0}" DataTextField="ScriptID" />
            </Columns>
        </asp:GridView>

        </div>
    </div>
</asp:Content>

