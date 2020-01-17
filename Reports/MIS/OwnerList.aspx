<%@ Page Title="Owner List" AspCompat="true" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="OwnerList.aspx.vb" Inherits="Reports_MIS_OwnerList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
Choose Requirements (* = Required)<br />
* Select Occupancy Year<br />
<asp:DropDownList ID="ddOcc" runat="server"></asp:DropDownList><br />
<asp:CheckBox ID="cbState" runat="server" Text="By State" AutoPostBack="True" /><br />
<asp:Panel ID="pnlState" runat="server">
    <table>
        <tr>
            <td><asp:ListBox ID="lbStates" runat="server"></asp:ListBox></td>
            <td>
                <asp:Button ID="btnAddState" runat="server" Text=">>" /><br />
                <asp:Button ID="btnRemoveState" runat="server" Text="<<" />
            </td>
            <td><asp:ListBox ID="lbStates_Selected" runat="server"></asp:ListBox></td>
        </tr>
    </table>
    
</asp:Panel>

<asp:CheckBox ID="cbPhase" runat="server" Text="By Phase" AutoPostBack="True" /><br />
<asp:Panel ID="pnlPhase" runat="server">
    <table>
        <tr>
            <td><asp:ListBox ID="lbPhase" runat="server">
                <asp:ListItem>Cottage</asp:ListItem>
                <asp:ListItem>Townes</asp:ListItem>
                <asp:ListItem>Estates</asp:ListItem>
                </asp:ListBox></td>
            <td>
                <asp:Button ID="btnAddPhase" runat="server" Text=">>" /><br />
                <asp:Button ID="btnRemovePhase" runat="server" Text="<<" />
            </td>
            <td><asp:ListBox ID="lbPhase_Selected" runat="server"></asp:ListBox></td>
        </tr>
    </table>
</asp:Panel>

<asp:CheckBox ID="cbSeason" runat="server" Text="By Season" AutoPostBack="True" /><br />
<asp:Panel ID="pnlSeason" runat="server">
    <table>
        <tr>
            <td><asp:ListBox ID="lbSeason" runat="server"></asp:ListBox></td>
            <td>
                <asp:Button ID="btnAddSeason" runat="server" Text=">>" /><br />
                <asp:Button ID="btnRemoveSeason" runat="server" Text="<<" />
            </td>
            <td><asp:ListBox ID="lbSeason_Selected" runat="server"></asp:ListBox></td>
        </tr>
    </table>
</asp:Panel>

<asp:CheckBox ID="cbStatus" runat="server" Text="By Status" AutoPostBack="True" /><br />
<asp:Panel ID="pnlStatus" runat="server">
    <table>
        <tr>
            <td><asp:ListBox ID="lbStatus" runat="server"></asp:ListBox></td>
            <td>
                <asp:Button ID="btnAddStatus" runat="server" Text=">>" style="height: 26px" /><br />
                <asp:Button ID="btnRemoveStatus" runat="server" Text="<<" />
            </td>
            <td><asp:ListBox ID="lbStatus_Selected" runat="server"></asp:ListBox></td>
        </tr>
    </table>
</asp:Panel>

<asp:CheckBox ID="cbFreq" runat="server" Text="By Frequency" AutoPostBack="True" /><br />
<asp:Panel ID="pnlFreq" runat="server">
    <table>
        <tr>
            <td><asp:ListBox ID="lbFreq" runat="server"></asp:ListBox></td>
            <td>
                <asp:Button ID="btnAddFreq" runat="server" Text=">>" /><br />
                <asp:Button ID="btnRemFreq" runat="server" Text="<<" />
            </td>
            <td><asp:ListBox ID="lbFreq_Selected" runat="server"></asp:ListBox></td>
        </tr>
    </table>
</asp:Panel>

<asp:CheckBox ID="cbSubType" runat="server" Text="By Contract Sub Type" AutoPostBack="True" /><br />
<asp:Panel ID="pnlSubType" runat="server">
    <table>
        <tr>
            <td><asp:ListBox ID="lbSubType" runat="server"></asp:ListBox></td>
            <td>
                <asp:Button ID="btnAddSubType" runat="server" Text=">>" /><br />
                <asp:Button ID="btnRemSubType" runat="server" Text="<<" />
            </td>
            <td><asp:ListBox ID="lbSubType_Selected" runat="server"></asp:ListBox></td>
        </tr>
    </table>
</asp:Panel>
<asp:CheckBox ID="cbEmail" runat="server" Text="With/Without Email Address" AutoPostBack="True" /><br />
<asp:Panel ID="pnlEmail" runat="server">
    <asp:RadioButtonList ID="rblEmail" runat="server" 
        RepeatDirection="Horizontal">
        <asp:ListItem Value="1" Selected="True">With Email Address</asp:ListItem>
        <asp:ListItem Value="0">Without Email Address</asp:ListItem>
    </asp:RadioButtonList>
</asp:Panel>
<asp:CheckBox ID="cbTri" runat="server" Text="Include Triennial Owners from Prior Years" /><br />
<asp:CheckBox ID="cbCoOwner" runat="server" Text="Include Co-Owner with different address" />
    <br />
    <br />
<asp:Button ID="btnReport" runat="server" Text = "On Screen" />
<asp:Button ID="btnExcel" runat="server" Text = "To Excel" />
    <asp:Literal ID="Lit" runat="server"></asp:Literal>
    <asp:gridview id="gvOwners" runat="server"></asp:gridview>
    <asp:gridview id="gvCoOwners" runat="server"></asp:gridview>
    <asp:gridview id="gvTriOwners" runat="server"></asp:gridview>

</asp:Content>

