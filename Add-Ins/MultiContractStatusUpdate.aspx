<%@ Page Title="Multi-Contract Status Update" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="MultiContractStatusUpdate.aspx.vb" Inherits="Add_Ins_MultiContractStatusUpdate" %>

<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <table>
        
        <tr>
            <td colspan="3">
                <asp:RadioButtonList ID="rblType" runat="server" 
                    RepeatDirection="Horizontal" AutoPostBack="True">
                    <asp:ListItem Value="csv">Upload CSV</asp:ListItem>
                    <asp:ListItem Value="kcp">Enter Contract Numbers</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td colspan = "3">
                <asp:MultiView ID="MultiView1" runat="server">
                    <asp:View ID="View1" runat="server">
                        <table>
                            <tr>
                                <th>Status</th>
                                <th>Update</th>
                                <th>New Value</th>
                            </tr>
                            <tr>
                                <td>Contract Status:</td>
                                <td><asp:CheckBox ID="cbCS" runat="server" AutoPostBack="True" /></td>
                                <td><uc1:Select_Item ID="siContractStatus" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>Contract Sub-Status:</td>
                                <td><asp:CheckBox ID="cbCSS" runat="server" AutoPostBack="True" /></td>
                                <td><uc1:Select_Item ID="siContractSubStatus" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>Maintenance Fee Status:</td>
                                <td><asp:CheckBox ID="cbMFS" runat="server" AutoPostBack="True" /></td>
                                <td><uc1:Select_Item ID="siMaintenanceFeeStatus" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>Mortgage Status:</td>
                                <td><asp:CheckBox ID="cbMS" runat="server" AutoPostBack="True" /></td>
                                <td><uc1:Select_Item ID="siMortgageStatus" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>Conversion Status:</td>
                                <td><asp:CheckBox ID = "cbCvS" runat="server" AutoPostBack="true" /></td>
                                <td><uc1:Select_Item ID="siConversionStatus" runat="server" /></td>
                            </tr>
                            <tr>
                                <td>
                                    KCP:</td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtKCP" runat="server"></asp:TextBox>
                                    <asp:Button ID="btnAdd" runat="server" Text="Add" />
                                    <asp:Button ID="btnRemove" runat="server" Text="Remove" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:ListBox ID="lstKCP" runat="server" Height="155px" Width="268px"></asp:ListBox>
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="View2" runat="server">
                        Select CSV file: <asp:FileUpload ID="file1" runat="server" />
                        <asp:Button ID="btnUpload" runat="server" Text="Upload" /><br />
                        <asp:MultiView ID="MultiView2" runat="server">
                            <asp:View ID="View3" runat="server">
                                <table>
                                    <tr>
                                        <td>ContractID Column:</td>
                                        <td>
                                            <asp:CheckBox ID="cbKCPID" runat="server" Text="Ignore" />
                                        </td>
                                        <td><asp:DropDownList ID="ddKCPID" runat="server"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td>Contract # Column:</td>
                                        <td>
                                            <asp:CheckBox ID="cbKCP" runat="server" Text="Ignore" />
                                        </td>
                                        <td><asp:DropDownList ID="ddKCP" runat="server"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td>New Contract Status Column:</td>
                                        <td><asp:CheckBox ID="cbNCS" runat="server" Text="Ignore" /></td>
                                        <td><asp:DropDownList ID="ddNCS" runat="server"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td>New Contract Sub-Status Column:</td>
                                        <td><asp:CheckBox ID="cbNCSS" runat="server" Text="Ignore" /></td>
                                        <td><asp:DropDownList ID="ddNCSS" runat="server"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td>New Maintenance Fee Column:</td>
                                        <td><asp:CheckBox ID="cbNMF" runat="server" Text="Ignore" /></td>
                                        <td><asp:DropDownList ID="ddMF" runat="server"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td>New Mortgage Status Column:</td>
                                        <td><asp:CheckBox ID="cbNMS" runat="server" Text="Ignore" /></td>
                                        <td><asp:DropDownList ID="ddMS" runat="server"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td>New Conversion Status Column:</td>
                                        <td><asp:CheckBox ID="cbNCvS" runat="server" Text= "Ignore" /></td>
                                        <td><asp:DropDownList ID="ddCvS" runat="server"></asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">Do not update Status Date:
                                            <asp:CheckBox ID="cbIgnoreStatusDate" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                        </asp:MultiView>
                    </asp:View>
                </asp:MultiView>
            </td>
        </tr>
    </table>
    <asp:Label ID="lblStatus" runat="server" Text="Label"></asp:Label>
    <ul id="menu">
        <li><asp:LinkButton ID="lbProcess" runat="server">Process</asp:LinkButton></li>
    </ul>
    <asp:HiddenField ID="hfFile" Value = "" runat="server" />

</asp:Content>

