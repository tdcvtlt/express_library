<%@ Page Title="Reservation Waitlist" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="ResWaitList.aspx.vb" Inherits="marketing_ResWaitList" %>
<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>
<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>Start Date:</td>
            <td><uc1:DateField ID="dteSDate" runat="server" /></td>
        </tr>
        <tr>
            <td>End Date:</td>
            <td><uc1:DateField ID="dteEDate" runat="server" /></td>
        </tr>
        <tr>
            <td>Unit Type:</td>
            <td><asp:DropDownList ID="ddUnitType" runat="server"></asp:DropDownList></td>
        </tr>
        <tr>
            <td>BD:</td>
            <td><asp:DropDownList ID="ddBD" runat="server"></asp:DropDownList></td>
        </tr>
        <tr>
            <td><asp:Button runat="server" Text="Query" onclick="Unnamed1_Click"></asp:Button></td>
            <td><asp:Button runat="server" Text="Add Owner" onclick="Unnamed2_Click"></asp:Button></td>
        </tr>
    </table>
    <asp:GridView runat="server" id = "gvWaitlist" 
        onRowDataBound = "gvWaitlist_RowDataBound" EmptyDatatext = "No Records" 
        EnableModelValidation="True" autogeneratecolumns = "false">
        <Columns>
            <asp:BoundField DataField="WaitlistID" HeaderText="ID"></asp:BoundField>
            <asp:BoundField DataField="DateCreated" HeaderText="Date Created"></asp:BoundField>
            <asp:BoundField DataField="Owner" HeaderText="Owner"></asp:BoundField>
            <asp:BoundField DataField="ContractNumber" HeaderText="Contract"></asp:BoundField>
            <asp:BoundField DataField="StartDate" HeaderText="InDate"></asp:BoundField>
            <asp:BoundField DataField="EndDate" HeaderText="OutDate"></asp:BoundField>
            <asp:BoundField DataField="UnitType" HeaderText="UnitType"></asp:BoundField>
            <asp:BoundField DataField="BR" HeaderText="BR"></asp:BoundField>
            <asp:BoundField DataField="ReqSeason" HeaderText="Requested Season"></asp:BoundField>
            <asp:BoundField DataField="ContractSeason" HeaderText="Contract Season"></asp:BoundField>
            <asp:BoundField DataField="UserName" HeaderText="CreatedBy"></asp:BoundField>
            <asp:ButtonField CommandName="RemoveRes" Text="Remove"></asp:ButtonField>
        </Columns>
    </asp:GridView>
<asp:Label runat="server" id = "lblErr"></asp:Label>
</asp:Content>

