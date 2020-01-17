<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EditSalesInventory.aspx.vb" Inherits="marketing_EditSalesInventory" %>

<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc1" %>
<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc2" %>

<%@ Register src="../controls/Events.ascx" tagname="Events" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <ul id="menu">
        <li <%if  MultiView1.ActiveViewIndex = 0 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="SalesInventory_Link" runat="server">Sales Inventory</asp:LinkButton>
            
         </li>
        <li <%if  MultiView1.ActiveViewIndex = 1 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Contracts_Link" runat="server">Contracts</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 2 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="History_Link" runat="server">History</asp:LinkButton></li>
        <li <%if  MultiView1.ActiveViewIndex = 3 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Events_Link" runat="server">Events</asp:LinkButton></li>
    </ul>
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="vwSalesInventory" runat="server">
            <table>
                <tr>
                    <td>Unit:</td>
                    <td>
                        <asp:Label ID="lblUnit" runat="server" Text="Label"></asp:Label>
                    </td>
                    <td>Week:</td>
                    <td>
                        <asp:Label ID="lblWeek" runat="server" Text="Label"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>Status:</td>
                    <td>
                        <uc1:Select_Item ID="siStatus" runat="server" />
                    </td>
                    <td>Status Date:</td>
                    <td>
                        <uc2:DateField ID="dfStatusDate" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Season:</td>
                    <td>
                        <uc1:Select_Item ID="siSeason" runat="server" />
                    </td>
                    <td>Week Type:</td>
                    <td>
                        <uc1:Select_Item ID="siWeekType" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Inventory Type:</td>
                    <td>
                        <uc1:Select_Item ID="siInventoryType" runat="server" />
                    </td>
                    <td>Inventory SubType:</td>
                    <td>
                        <uc1:Select_Item ID="siInventorySubType" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>Budgeted Price:</td>
                    <td>
                        <asp:TextBox ID="txtBudgetedPrice" runat="server"></asp:TextBox>
                    </td>
                    <td>Points Value:</td>
                    <td>
                        <asp:TextBox ID="txtPoints" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>ID:</td>
                    <td>
                        <asp:Label ID="lblID" runat="server" Text="Label"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:Button ID="btnSave" runat="server" Text="Save" />
        </asp:View>
        <asp:View ID="vwContracts" runat="server">
            <asp:GridView ID="gvContracts" runat="server" AutoGenerateSelectButton="True">
            </asp:GridView>
        </asp:View>
        <asp:View ID="vwHistory" runat="server">
            <asp:GridView ID="gvHistory" runat="server" AutoGenerateSelectButton="True">
            </asp:GridView>
        </asp:View>
        <asp:View ID="vwEvents" runat="server">
            <uc3:Events ID="cEvents" runat="server" />
        </asp:View>
    
    
    </asp:MultiView>
</asp:Content>

