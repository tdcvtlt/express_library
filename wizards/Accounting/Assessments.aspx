<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="Assessments.aspx.vb" Inherits="wizards_Accounting_Assessments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="Label2" runat="server" Text="Filter: "></asp:Label>
    <asp:DropDownList ID="ddFilter" runat="server">
        <asp:ListItem Value="BatchID">BatchID</asp:ListItem>
        <asp:ListItem Selected="True" Value="Name">Name</asp:ListItem>
    </asp:DropDownList>
    <br />
    <br />
    <asp:Label ID="Label1" runat="server" Text="Enter Filter Value:"></asp:Label><br />
    <asp:TextBox ID="filter" runat="server"></asp:TextBox>
    <asp:Button ID="btnQuery" runat="server" Text="Query" />
    <asp:Button ID="btnNew" runat="server" Text="New" />
    <br />
    <div style="height:200px;width:600px;overflow:auto; ">
    <asp:GridView ID="GridView1" runat="server" AutoGenerateSelectButton="False" DataKeyNames="BatchID" AutoGenerateColumns="true" 
    EmptyDataText="No Records" GridLines="Horizontal">
        <SelectedRowStyle BackColor="#CCFFFF" Wrap="True" />
        <AlternatingRowStyle BackColor="#CCFFCC" />        
        <Columns>
            <asp:HyperLinkField HeaderText="Edit" 
                        DataNavigateUrlFields="BatchID" 
                        DataNavigateUrlFormatString="editAssessment.aspx?batchid={0}" DataTextField="BatchID" 
                        />
                    
                              
        </Columns>
    </asp:GridView>
    </div>
    <asp:Label runat="server" id = "lblErr"></asp:Label>
</asp:Content>

