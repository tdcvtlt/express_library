<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="ResCheckins.aspx.vb" Inherits="PropertyManagement_ResCheckins" %>

<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>
<%@ Register Src="~/controls/DateField.ascx" TagPrefix="uc2" TagName="DateField" %>
<%@ Register Src="~/controls/SyncDateField.ascx" TagPrefix="uc1" TagName="SyncDateField" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript" language="javascript" src = "<%=request.applicationpath%>/scripts/scw.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>
                Start Date:
            </td>
            <td>

                <uc1:DateField ID="dteSDate" runat="server" />

            </td>
        </tr>
        <tr>
            <td>End Date:</td>
            <td>
                <uc1:DateField ID="dteEDate" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="Button1" runat="server" Text="Button" /></td>
        </tr>
    </table>
    <asp:GridView ID="gvCheckIns" runat="server" AutoGenerateColumns="False" EnableModelValidation="True" OnRowDataBound="gvCheckIns_RowDataBound" EmptyDataText="No Records">
        <Columns>
            <asp:ButtonField CommandName ="Complete" Text="Update" />
            <asp:BoundField HeaderText="ID" DataField="ID"></asp:BoundField>
            <asp:BoundField DataField="ReservationID" HeaderText="ResID" />
            <asp:BoundField HeaderText="CheckIndate" DataField="CheckIndate"></asp:BoundField>
            <asp:BoundField HeaderText="CheckOutdate" DataField="CheckOutdate"></asp:BoundField>
            <asp:BoundField HeaderText="FirstName" DataField="FirstName"></asp:BoundField>
            <asp:BoundField HeaderText="LastName" DataField="LastName"></asp:BoundField>
            <asp:BoundField HeaderText="Rooms" DataField ="Rooms"></asp:BoundField>
            <asp:BoundField HeaderText="Extensions" DataField="Extensions"></asp:BoundField>
            <asp:TemplateField HeaderText="Status">
                <ItemTemplate>
                    <asp:DropDownList runat="server" id="ddStatus"></asp:DropDownList>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Follow-Up Date">
                <ItemTemplate>
                    <asp:TextBox runat="server" ID="dteFollow" onClick="scwShow(this,this);" ReadOnly="false"></asp:TextBox>
                    
                </ItemTemplate>
            </asp:TemplateField>
            <asp:ButtonField CommandName="AddNote" Text="Add Note" />
            <asp:ButtonField CommandName="ViewNote" Text="View Notes" />
        </Columns>
    </asp:GridView>
    <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
</asp:Content>

