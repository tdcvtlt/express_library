<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="CheckinsVsTours.aspx.vb" Inherits="Reports_CustomerService_CheckinsVsTours" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


<div style="position:relative;">
    <div style="float:left;">
        <label><h3>Check In/Out</h3></label>
        <table>
            <tr>
                <td ><label>Start Date:</label></td>
                <td style="width:240px;">
                    <uc1:DateField ID="dteSDate" Selected_Date="" runat="server" />
                </td>
            </tr>
            <tr>
                <td><label>End Date:</label></td>
                <td>
                    <uc1:DateField ID="dteEDate" Selected_Date="" runat="server" />
                </td>
            </tr>
            <tr>
                <td></td>
                <td><asp:Button ID="btn_Submit" runat="server" Text="Submit"></asp:Button></td>
            </tr>
        </table>
    </div>

    <div style="float:left;margin-left:40px;width:250px;">
        <label><h3>Tour Status</h3></label>        
        <asp:CheckBoxList runat="server" ID="cbl1" RepeatColumns="2" RepeatDirection="Horizontal" ></asp:CheckBoxList>    
    </div>

    <div style="float:left;margin-left:40px;width:250px;">
        
        <label><h3>Reservation Type</h3></label>
        <asp:CheckBoxList runat="server" ID="cbl2"></asp:CheckBoxList>
    </div>
</div>


<div style="clear:left;">
    <br /><br />
    <asp:Literal ID="lit" runat="server"></asp:Literal>
</div>

</asp:Content>

