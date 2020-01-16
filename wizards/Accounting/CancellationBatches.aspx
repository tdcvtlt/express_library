<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="CancellationBatches.aspx.vb" Inherits="wizards_Accounting_CancellationBatches" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>ID: </td>
            <td>
                <asp:TextBox ID="txtQuery" runat="server"></asp:TextBox>
                <asp:Button ID="btnQuery" runat="server" Text="Search"/>
                <asp:Button ID="btnNew" runat="server" Text="New"/>
            </td>
        </tr>
    </table>
    <div class="ListGrid">
        <asp:GridView ID="gvBatches" runat="server" AutoGenerateSelectButton="False" DataKeyNames="BatchID" AutoGenerateColumns="true" 
    EmptyDataText="No Records" GridLines="Horizontal">
        <SelectedRowStyle BackColor="#CCFFFF" Wrap="True" />
        <AlternatingRowStyle BackColor="#CCFFCC" />        
        <Columns>
            <asp:HyperLinkField HeaderText="Edit" 
                        DataNavigateUrlFields="BatchID" 
                        DataNavigateUrlFormatString="editCancellationBatch.aspx?bid={0}" DataTextField="BatchID" 
                        />
                    
                              
        </Columns>
    </asp:GridView>
    </div>
</asp:Content>

