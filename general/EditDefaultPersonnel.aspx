<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EditDefaultPersonnel.aspx.vb" Inherits="general_EditDefaultPersonnel" %>

<%@ Register src="../controls/Events.ascx" tagname="Events" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<ul id="menu">
            <li <%if  MultiView1.ActiveViewIndex = 0 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Main" runat="server">Main</asp:LinkButton></li>
            <li <%if  MultiView1.ActiveViewIndex = 1 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Events" runat="server">Events</asp:LinkButton></li>
            
        </ul>
        <asp:MultiView ID="MultiView1" runat="server">
                    <asp:View ID="View1" runat="server">
    <table>
        <tr>
            <td colspan = '2' align = 'center' style="background-color:Aqua;"><B>Billing Code - Campaign Combination</B></td>
        </tr>
        <tr>
            <td>Billing Code:</td>
            <td><asp:DropDownList runat="server" id = "ddBillingCode" onSelectedIndexChanged = "ddBillingCode_SelectedIndexChanged" autoPostBack = "true"></asp:DropDownList></td>
        </tr>
        <tr>
            <td>Campaign:</td>
            <td><asp:DropDownList runat="server" id = "ddCampaigns" onSelectedIndexChanged = "ddCampaigns_SelectedIndexChanged" autoPostBack = "true"></asp:DropDownList></td>
        </tr>
        <tr>
            <td colspan = '2' align = 'center' style="background-color:Aqua;"><B><i>Personnel Record to Add to Above Combination</i></B></td>
        </tr>
        <tr>
            <td>Personnel:</td>
            <td><asp:DropDownList runat="server" id = "ddPersonnel"></asp:DropDownList></td>
        </tr>
        <tr>
            <td>Title:</td>
            <td><asp:DropDownList runat="server" id = "ddTitle"></asp:DropDownList></td>
        </tr>
        <tr>
            <td>Fixed Amount:</td>
            <td><asp:TextBox runat="server" id = "txtFixedAmount" value = '0'></asp:TextBox></td>
        </tr>
        <tr>
            <td>Commission Percentage:</td>
            <td><asp:TextBox runat="server" id = "txtCommPercentage"  value = '0'></asp:TextBox></td>
        </tr>
        <tr>
            <td><asp:Button runat="server" Text="Add Combination" onclick="Unnamed1_Click"></asp:Button>
                
                    
            </td>
        </tr>
    </table>

    <asp:GridView runat="server" id = "gvConPersonnel" 
        onRowDataBound = "gvConPersonnel_RowDataBound" autoGenerateColumns = "False" 
        EnableModelValidation="True" EmptyDataText = "No Records">
        <Columns>
            <asp:BoundField DataField="ID" HeaderText="ID"></asp:BoundField>
            <asp:BoundField DataField="Personnel" HeaderText="Personnel"></asp:BoundField>
            <asp:BoundField DataField="Title" HeaderText="Title"></asp:BoundField>
            <asp:BoundField DataField="FixedAmount" HeaderText="Fixed Amount">
            </asp:BoundField>
            <asp:BoundField DataField="PercentageAmount" HeaderText="Percentage">
            </asp:BoundField>
            <asp:ButtonField CommandName="Remove" Text="Remove"></asp:ButtonField>
        </Columns>
    
    
    </asp:GridView>
        
    </asp:View>
    <asp:View ID="View2" runat="server">
        <uc1:Events ID="Events1" runat="server" />
        </asp:View>
    </asp:MultiView>
</asp:Content>

