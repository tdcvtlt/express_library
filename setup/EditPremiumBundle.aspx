<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EditPremiumBundle.aspx.vb" Inherits="setup_EditPremiumBundle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type="text/javascript">

    function refreshPremiums() {

        __doPostBack('ctl00$ContentPlaceHolder1$LinkButtonPremiums', '');
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ul id="menu">
        <li <% if MultiView1.ActiveViewIndex = 0 then: response.write("class=""current"""):end if %>>
            <asp:LinkButton ID="LinkButtonPremiumBundle" runat="server">Bundle</asp:LinkButton>
        </li>    
    
        <li <% if MultiView1.ActiveViewIndex = 1 then: response.write("class=""current"""):end if %>>
            <asp:LinkButton ID="LinkButtonPremiums" runat="server">Premiums</asp:LinkButton>        
        </li>   
    </ul>
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="View1" runat="server">
            <table>
                <tr>
                    <td>ID:</td>
                    <td><asp:TextBox ID="txtBundleID" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Name:</td>
                    <td><asp:TextBox ID="txtBundleName" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Description:</td>
                    <td><asp:TextBox ID="txtBundleDesc" runat="server" Width="422px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td><asp:Button ID="btnSave" runat="server" Text="Save" /></td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="View2" runat="server">
            <asp:GridView ID="gvPremiums" runat="server" AutoGenerateSelectButton="True" 
                EmptyDataText="No Records" GridLines="Horizontal">
                <SelectedRowStyle BackColor="#CCFFFF" Wrap="True" />
                <AlternatingRowStyle BackColor="#CCFFCC" />
            </asp:GridView>
            <asp:Button ID="btnNew" runat="server" Text="Add" />
        </asp:View>
    </asp:MultiView>
</asp:Content>

