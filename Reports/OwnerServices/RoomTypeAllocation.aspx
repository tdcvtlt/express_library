<%@ Page Title="Room Type Allocation" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="RoomTypeAllocation.aspx.vb" Inherits="Reports_OwnerServices_RoomTypeAllocation" %>

<%@ Register Src="~/controls/DateField.ascx" TagPrefix="uc1" TagName="DateField" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>Start Date:</td>
            <td><uc1:DateField runat="server" id="dfSDate" />
            </td>
        </tr>
        <tr>
            <td>End Date:</td>
            <td><uc1:DateField runat="server" id="dfEDate" />
            </td>
        </tr>
        <tr>
            <td>Week 1 Start Date:</td>
            <td><uc1:DateField runat="server" id="dfFirstDate" />
            </td>
        </tr>
        <tr>
            <td>Unit Type:</td>
            <td><asp:DropDownList ID="ddUnitType" runat="server"></asp:DropDownList></td>
        </tr>
        <tr>
            <td colspan="2"><asp:Button ID="btnCreateExcel" runat="server" Text="Export (Excel)" /></td>
        </tr>
        <tr>
            <td colspan="2"><asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label></td>
        </tr>
    </table>
</asp:Content>

