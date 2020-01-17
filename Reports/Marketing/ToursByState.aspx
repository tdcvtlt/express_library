<%@ Page Title="Tours By State" AspCompat="true" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="ToursByState.aspx.vb" Inherits="Reports_Marketing_ToursByState" %>

<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div style="width:45%">
<fieldset>
<legend>Please pick parameters to generate the report you need</legend>
    <table>
        <tr>
            <td>Start Date:</td>
            <td colspan="3">
                <uc1:datefield ID="dfStartDate" runat="server" />
            </td>
        </tr>
        <tr>
            <td>End Date:</td>
            <td colspan="3">
                <uc1:datefield ID="dfEndDate" runat="server" />
            </td>
        </tr>
        <tr>
            <td>No Show Status:</td>
            <td>NQ Status:</td>
            <td>Qualified Status:</td>
            <td>Cancelled Status:</td>
        </tr>
        <tr>
            <td><asp:DropDownList ID="ddNS" runat="server"></asp:DropDownList></td>
            <td><asp:DropDownList ID="ddNQ" runat="server"></asp:DropDownList></td>
            <td><asp:DropDownList ID="ddQS" runat="server"></asp:DropDownList></td>
            <td><asp:DropDownList ID="ddCS" runat="server"></asp:DropDownList></td>
        </tr>
        <tr>
            <td><asp:ListBox ID="lbNS" runat="server"></asp:ListBox> </td>
            <td><asp:ListBox ID="lbNQ" runat="server"></asp:ListBox> </td>
            <td><asp:ListBox ID="lbQS" runat="server"></asp:ListBox> </td>
            <td><asp:ListBox ID="lbCS" runat="server"></asp:ListBox> </td>
        </tr>
        <tr>
            <td><asp:button ID="btnAddNS" runat="server" Text="Add" /><br /><asp:Button ID="btnRemoveNS" runat="server" Text = "Remove" /> </td>
            <td><asp:button ID="btnAddNQ" runat="server" Text="Add" /><br /><asp:Button ID="btnRemoveNQ" runat="server" Text = "Remove" /> </td>
            <td><asp:button ID="btnAddQS" runat="server" Text="Add" /><br /><asp:Button ID="btnRemoveQS" runat="server" Text = "Remove" /> </td>
            <td><asp:button ID="btnAddCS" runat="server" Text="Add" /><br /><asp:Button ID="btnRemoveCS" runat="server" Text = "Remove" /> </td>
        </tr>
    </table>


</fieldset>
</div>


    <div>
    <asp:Button ID = "btnReport" runat="server" Text = "Get Report" 
        style="height: 26px" />
    <asp:Button ID = "btnExcel" runat="server" Text = "Create Excel" />
    <br />
    <br />
    <br />
    <br />
    
    <asp:HiddenField ID="hfShowReport" Value="0" runat="server" />
    <br />
    <br />
    
    </div>


<div>
    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" 
        AutoDataBind="true" ToolPanelView="None" />

    <br />
    <br />
    <br />
    <br />

</div>
</asp:Content>

