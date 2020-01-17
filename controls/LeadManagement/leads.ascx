<%@ Control Language="VB" AutoEventWireup="false" CodeFile="leads.ascx.vb" Inherits="controls_LeadManagement_leads" %>

        <asp:MultiView ID="MultiView1" runat="server">

            <asp:View ID="View1" runat="server">
                <table>
                    <tr>
                        <td>Search By:</td>
                        <td><asp:DropDownList ID="ddFilter" runat="server">
                            <asp:ListItem Value="Address1">Address1</asp:ListItem>
                            <asp:ListItem Value="City">City</asp:ListItem>
                            <asp:ListItem Value="Email">Email</asp:ListItem>
                            <asp:ListItem Value="ID">ID</asp:ListItem>
                            <asp:ListItem Value="Name">Name</asp:ListItem>
                            <asp:ListItem Selected="True" Value="Phone">Phone</asp:ListItem>
                            <asp:ListItem Value="PostalCode">PostalCode</asp:ListItem>
                            <asp:ListItem Value="SpouseSSN">SpouseSSN</asp:ListItem>
                            <asp:ListItem Value="SSN">SSN</asp:ListItem>
                            <asp:ListItem Value="State">State</asp:ListItem>
                        </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td>Value:</td>
                        <td><asp:TextBox ID="txtFilterValue" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="btnSearch" runat="server" Text="Search" />
                        </td>
                    </tr>
                </table>   
                <asp:GridView ID="gvLeads" runat="server" AutoGenerateSelectButton="true" emptydatatext="No Records">
                <SelectedRowStyle BackColor="#CCFFFF" Wrap="True" />
                <AlternatingRowStyle BackColor="#CCFFCC" />
                </asp:GridView>
            </asp:View>
            <asp:View ID="View2" runat="server">
                <asp:GridView ID="gvLeadsWithoutTasks" runat="server" AutoGenerateSelectButton="true">
                <SelectedRowStyle BackColor="#CCFFFF" Wrap="True" />
                <AlternatingRowStyle BackColor="#CCFFCC" />
                </asp:GridView>
            </asp:View>
            <asp:View ID="View3" runat="server">
                <asp:GridView ID="gvMyLeads" runat="server" AutoGenerateSelectButton="true" 
                    EmptyDataText="No Assigned Leads">
                <SelectedRowStyle BackColor="#CCFFFF" Wrap="True" />
                <AlternatingRowStyle BackColor="#CCFFCC" />
                </asp:GridView>
            </asp:View>
        </asp:MultiView>
        <asp:Label ID="lblException" runat="server" Text="" />
    

