<%@ Page Title="Bank" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="Banking.aspx.vb" Inherits="PropertyManagement_Banking" %>
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
        <asp:Table ID ="tblRequest" runat="server" BorderStyle="Solid" BorderWidth="1px">
            <asp:TableRow>
                <asp:TableCell ColumnSpan="2" BorderStyle="Solid" BorderWidth="1px">All information in <font color = "red">RED</font> is required for initial entry.</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell BorderStyle="Solid" BorderWidth="1px"><font color = "red">First Name:</font></asp:TableCell>
                <asp:TableCell BorderStyle="Solid" BorderWidth="1px"><asp:TextBox ID="txtFName" runat="server" readonly></asp:TextBox></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell BorderStyle="Solid" BorderWidth="1px"><font color = "red">Last Name:</font></asp:TableCell>
                <asp:TableCell BorderStyle="Solid" BorderWidth="1px"><asp:TextBox ID="txtLName" runat="server" readonly></asp:TextBox></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell BorderStyle="Solid" BorderWidth="1px"><font color = "red">Type Of Unit:</font></asp:TableCell>
                <asp:TableCell BorderStyle="Solid" BorderWidth="1px"><uc1:Select_Item ID="siUnitType" runat="server" /></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell BorderStyle="Solid" BorderWidth="1px"><font color = "red">Frequency:</font></asp:TableCell>
                <asp:TableCell BorderStyle="Solid" BorderWidth="1px"><asp:DropDownList runat="server" id = "ddFrequency"></asp:DropDownList></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell BorderStyle="Solid" BorderWidth="1px"><font color = "red">Usage:</font></asp:TableCell>
                <asp:TableCell BorderStyle="Solid" BorderWidth="1px"><uc1:Select_Item ID="siSeason" runat="server" /></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell BorderStyle="Solid" BorderWidth="1px"><font color = "red">Usage Year:</font></asp:TableCell>
                <asp:TableCell BorderStyle="Solid" BorderWidth="1px"><asp:DropDownList runat="server" id = "ddYear"></asp:DropDownList></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell BorderStyle="Solid" BorderWidth="1px"><font color = "red">Unit Size:</font></asp:TableCell>
                <asp:TableCell BorderStyle="Solid" BorderWidth="1px"><asp:DropDownList runat="server" id = "ddUnitSize"></asp:DropDownList></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell BorderStyle="Solid" BorderWidth="1px"><font color = "red">Exchange Company:</font></asp:TableCell>
                <asp:TableCell BorderStyle="Solid" BorderWidth="1px"><asp:DropDownList runat="server" id = "ddBankExchCompany" autopostback = "true" onSelectedIndexChanged = "ddBankExchCompany_SelectedIndexChanged"></asp:DropDownList></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell BorderStyle="Solid" BorderWidth="1px"><font color = "red">Membership Number:</font></asp:TableCell>
                <asp:TableCell BorderStyle="Solid" BorderWidth="1px"><asp:TextBox ID="txtMemNumber" runat="server"></asp:TextBox></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell BorderStyle="Solid" BorderWidth="1px"><font color = "red">Contract Number:</font></asp:TableCell>
                <asp:TableCell BorderStyle="Solid" BorderWidth="1px"><asp:TextBox ID="txtConNumber" runat="server" readonly></asp:TextBox>
                    <asp:Label ID="lblContractType" runat="server" Text=""></asp:Label></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID ="tRowStatus">
                <asp:TableCell BorderStyle="Solid" BorderWidth="1px"><u><font color = "red">Status:</font></u></asp:TableCell>
                <asp:TableCell BorderStyle="Solid" BorderWidth="1px"><asp:DropDownList runat="server" id = "ddBStatus" AutoPostBack="true" OnSelectedIndexChanged="ddBStatus_SelectedIndexChanged"></asp:DropDownList></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell BorderStyle="Solid" BorderWidth="1px">Status Date:</asp:TableCell>
                <asp:TableCell BorderStyle="Solid" BorderWidth="1px"><asp:TextBox runat="server" readonly id = "txtStatusDate"></asp:TextBox></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell BorderStyle="Solid" BorderWidth="1px">UsageID:</asp:TableCell>
                <asp:TableCell BorderStyle="Solid" BorderWidth="1px"><asp:TextBox runat="server" id = "txtUsageID"></asp:TextBox></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="tRowUType" Visible ="False">
                <asp:TableCell BorderStyle="Solid" BorderWidth="1px" ColumnSpan="2"><asp:RadioButtonList ID="rbusageType" runat="server" RepeatDirection="Horizontal" AutoPostBack="True" OnSelectedIndexChanged="rbUsageType_SelectedIndexChanged"><asp:ListItem Value="Create">Create Usage</asp:ListItem><asp:ListItem Value="Exists">Existing Usage</asp:ListItem></asp:RadioButtonList></asp:TableCell>
                
            </asp:TableRow>
            <asp:TableRow ID="tRowRoom" Visible="false">
                <asp:TableCell BorderStyle="Solid" BorderWidth="1px">Room:</asp:TableCell>
                <asp:TableCell BorderStyle="Solid" BorderWidth="1px"><asp:DropDownList runat="server" id = "ddRooms"></asp:DropDownList></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID ="tRowCI" Visible="false">
                <asp:TableCell BorderStyle="Solid" BorderWidth="1px">CheckIn:</asp:TableCell>
                <asp:TableCell BorderStyle="Solid" BorderWidth="1px"><uc2:DateField ID="dteCheckIn" runat="server" /></asp:TextBox></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID="tRowUsage" Visible="false">
                <asp:TableCell BorderStyle="Solid" BorderWidth="1px">UsageID:</asp:TableCell>
                <asp:TableCell BorderStyle="Solid" BorderWidth="1px"><asp:TextBox runat="server" id = "txtUID"></asp:TextBox></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow ID ="tRowConf" Visible="false">
                <asp:TableCell BorderStyle="Solid" BorderWidth="1px">Confirmation Number:</asp:TableCell>
                <asp:TableCell BorderStyle="Solid" BorderWidth="1px"><asp:TextBox runat="server" id = "txtConfNum"></asp:TextBox></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell BorderStyle="Solid" BorderWidth="1px" ColumnSpan="2"><asp:Button runat="server" Text="Submit" onclick="Unnamed3_Click"></asp:Button>
                    <asp:Button runat="server" Text="Reset" onclick="Unnamed4_Click"></asp:Button></asp:TableCell>                
            </asp:TableRow>
        </asp:Table>
        <asp:HiddenField runat="server" id = "hfConID"></asp:HiddenField>
        <asp:HiddenField runat="server" id = "hfProsID"></asp:HiddenField>
        <asp:HiddenField runat="server" id = "hfDepID"></asp:HiddenField>
    </asp:View>
    </asp:MultiView>

</asp:Content>

