<%@ Page Title="Bank" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="Banking-old.aspx.vb" Inherits="PropertyManagement_Banking_old" %>
<%@ Register src="~/controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc1" %>
<%@ Register src="~/controls/DateField.ascx" tagname="DateField" tagprefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
        <tr>
            <td>UserName:</td>
            <td><asp:DropDownList runat="server" id = "ddUserName"></asp:DropDownList></td>
        </tr>
        <tr>
            <td>Exchange Comp.:</td>
            <td><asp:DropDownList runat="server" id = "ddExchCompany"></asp:DropDownList></td>
        </tr>
        <tr>
            <td>Status:</td>
            <td><asp:DropDownList runat="server" id = "ddStatus"></asp:DropDownList></td>
        </tr>
        <tr>
            <td>Start Date:</td>
            <td><uc2:DateField ID="dteStartDate" runat="server" /></td>
        </tr>
        <tr>
            <td>End Date:</td>
            <td><uc2:DateField ID="dteEndDate" runat="server" /></td>
        </tr>
        <tr>
            <td>Run By Date Entered:</td>
            <td><asp:CheckBox ID="cbDateEntered" runat="server" /></td>
        </tr>
        <tr>
            <td>Contract Number:</td>
            <td><asp:TextBox ID="txtContract" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td><asp:Button ID="Button1" runat="server" Text="List" /></td>
            <td><asp:Button ID="Button2" runat="server" Text="Create New" /></td>
        </tr>
    </table>
    <asp:Label runat="server" id = "lblErr"></asp:Label>
    <asp:MultiView runat="server" id = "MultiView1">
    <asp:View runat="server" id = "View_1">
    <input type="button" value="Printable" onclick="var mwin = window.open();mwin.document.write(document.getElementById('printable').innerHTML);" />
    <div id = "printable">
        <asp:GridView ID="gvBankedUnits" runat="server" autogenerateselectbutton = "true" EmptyDataText = "No Records" onRowDataBound = "gvBankedUnits_RowDataBound">
        </asp:GridView>
    </div>
    </asp:View>
    <asp:View runat="server" id = "View_2">
        <table border = "1">
            <tr>
                <td colspan = "2">All information in <font color = "red">RED</font> is required for initial entry.</td>
            </tr>
            <tr>
                <td><font color = "red">First Name:</font></td>
                <td>
                    <asp:TextBox ID="txtFName" runat="server" readonly></asp:TextBox>
                </td>
            </tr>        
            <tr>
                <td><font color = "red">Last Name:</font></td>
                <td>
                    <asp:TextBox ID="txtLName" runat="server" readonly></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td><font color = "red">Type Of Unit:</font></td>
                <td>
                    <uc1:Select_Item ID="siUnitType" runat="server" />
                </td>
            </tr>
            <tr>
                <td><font color = "red">Frequency:</font></td>
                <td><asp:DropDownList runat="server" id = "ddFrequency"></asp:DropDownList></td>
            </tr>
            <tr>
                <td><font color = "red">Usage:</font></td>
                <td>
                    <uc1:Select_Item ID="siSeason" runat="server" />
                </td>
            </tr>
            <tr>
                <td><font color = "red">Usage Year:</font></td>
                <td><asp:DropDownList runat="server" id = "ddYear"></asp:DropDownList></td>
            </tr>
            <tr>
                <td><font color = "red">Unit Size:</font></td>
                <td><asp:DropDownList runat="server" id = "ddUnitSize"></asp:DropDownList></td>
            </tr>
            <tr>
                <td><font color = "red">Exchange Company:</font></td>
                <td><asp:DropDownList runat="server" id = "ddBankExchCompany" autopostback = "true" onSelectedIndexChanged = "ddBankExchCompany_SelectedIndexChanged"></asp:DropDownList></td>
            </tr>
            <tr>
                <td><font color = "red">Membership Number:</font></td>
                <td>
                    <asp:TextBox ID="txtMemNumber" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td><font color = "red">Contract Number:</font></td>
                <td>
                    <asp:TextBox ID="txtConNumber" runat="server" readonly></asp:TextBox>
                    <asp:Label ID="lblContractType" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td><font color = "red"><u>Status:</u></font></td>
                <td>
                    <uc1:Select_Item ID="siStatus" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Status Date:</td>
                <td><asp:TextBox runat="server" readonly id = "txtStatusDate"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Unit Deposited:</td>
                <td><asp:DropDownList runat="server" id = "ddUnit"></asp:DropDownList>
                    <asp:Button runat="server" Text="Add" onclick="Unnamed1_Click"></asp:Button></td>
            </tr>
            <tr>
                <td colspan = '2'>
                    <asp:ListBox runat="server" Width="172px" id = "lbUnits"></asp:ListBox>
                    <asp:Button runat="server" Text="Remove" onclick="Unnamed2_Click"></asp:Button>
                </td>
            </tr>
            <tr>
                <td>Week Deposited:</td>
                <td>
                    <asp:DropDownList ID="ddWeek" runat="server"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Year Used:</td>
                <td>
                    <asp:DropDownList ID="ddYearUsed" runat="server"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Date Deposited:</td>
                <td>
                    <uc2:DateField ID="dteDateDeposited" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Confirmation Number:</td>
                <td>
                    <asp:TextBox ID="txtConfirmation" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan = '2'>
                    <asp:GridView runat="server" id = "gvNotes" EmptyDataText = "No Notes"></asp:GridView>
                </td>
            </tr>
            <tr>
                <td colspan = '2'>
                    <asp:TextBox runat="server" Height="64px" Width="214px" id = "txtNotes"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td><asp:Button runat="server" Text="Submit" onclick="Unnamed3_Click"></asp:Button>
                    <asp:Button runat="server" Text="Reset" onclick="Unnamed4_Click"></asp:Button></td>
            </tr>
           
        </table>
        <asp:HiddenField runat="server" id = "hfConID"></asp:HiddenField>
        <asp:HiddenField runat="server" id = "hfProsID"></asp:HiddenField>
        <asp:HiddenField runat="server" id = "hfDepID"></asp:HiddenField>
    </asp:View>
    </asp:MultiView>

</asp:Content>

