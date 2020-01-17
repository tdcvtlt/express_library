<%@ Page Title="Out Of Service Rooms" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="OutOfService.aspx.vb" Inherits="Reports_OwnerServices_OutOfService" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="ucDatePicker" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div>
<table style="border-collapse:collapse;">
<tr>
    <td>Start Date</td>
    <td><ucDatePicker:DateField ID="SDate" runat="server" Selected_Date="" /></td>
</tr>
<tr>
    <td>End Date</td>
    <td><ucDatePicker:DateField ID="EDate" runat="server" Selected_Date="" /></td>
</tr>

</table>

<div>
    <div>

                
        <asp:Button runat="server" ID="RunReport" Text="Run Report" />
        <asp:Button runat="server" ID="BtnExcel" Text="Excel Report" Enabled="false" />

    </div>
</div>
<br />

<div>
    <asp:GridView ID="gvReport" runat="server" EnableModelValidation="True" AutoGenerateColumns="False" EmptyDataText="No Rooms Out Of Service">
        <Columns>
            <asp:BoundField DataField="DateAllocated" HeaderText="Date" DataFormatString="{0:d}" />
            <asp:HyperLinkField HeaderText="Edit" 
                        DataNavigateUrlFields="RoomID" 
                        DataNavigateUrlFormatString="https://crms.kingscreekplantation.com/crmsnet/marketing/editRoom.aspx?roomid={0}" DataTextField="RoomNumber" 
                        />
            <asp:BoundField DataField="Note" HeaderText="Note" />
            <asp:BoundField DataField="CreatedBy" HeaderText="CreatedBy" />
            <asp:BoundField DataField="DateCreated" HeaderText="DateCreated" />
        </Columns>
    </asp:GridView>
<asp:Literal ID="LIT" runat="server"></asp:Literal>
</div>



</div>
</asp:Content>

