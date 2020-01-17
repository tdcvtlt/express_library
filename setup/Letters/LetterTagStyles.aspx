<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="LetterTagStyles.aspx.vb" Inherits="setup_Letters_LetterTagStyles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="Label1" runat="server" Text="Tag Style ID:"></asp:Label><br />
        <asp:TextBox ID="filter" runat="server"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" Text="Query" />
        <asp:Button ID="btnNew" runat="server" Text="New" />
        <br />
        <div style="height:200px;width:600px;overflow:auto; ">
            <asp:GridView ID="GridView1" runat="server" AutoGenerateSelectButton="False" DataKeyNames="TagStyleID" AutoGenerateColumns="true" 
            EmptyDataText="No Records" GridLines="Horizontal">
                <SelectedRowStyle BackColor="#CCFFFF" Wrap="True" />
                <AlternatingRowStyle BackColor="#CCFFCC" />        
                <Columns>
                    <asp:HyperLinkField HeaderText="Edit" 
                                DataNavigateUrlFields="TagStyleID" 
                                DataNavigateUrlFormatString="editLetterTagStyle.aspx?id={0}" DataTextField="TagStyleID" 
                                />
                    
                              
                </Columns>
            </asp:GridView>
        </div>
    <asp:Label runat="server" id = "lblErr"></asp:Label>
</asp:Content>

