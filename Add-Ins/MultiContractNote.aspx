<%@ Page Title="Multi-Contract Note" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="MultiContractNote.aspx.vb" Inherits="Add_Ins_MultiContractNote" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>

        <tr>
            <td colspan="4">
                <asp:RadioButtonList ID="rblType" runat="server" 
                    RepeatDirection="Horizontal" AutoPostBack="True">
                    <asp:ListItem Value="csv">Upload CSV</asp:ListItem>
                    <asp:ListItem Value="kcp">Enter Contract Numbers</asp:ListItem>
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td colspan="4">&nbsp;</td>
        </tr>
        <tr>

            <td colspan="4">
                <asp:MultiView runat="server" ID="MultiView1">
                    <asp:View ID="View1" runat="server">
                        <table>
                            <tr>
                                 <td>Note Record:</td>
                                <td>Contract #: </td>
                                <td><asp:TextBox runat="server" id = "txtContract"></asp:TextBox></td>
                                <td><asp:Button runat="server" Text="Add" onclick="Unnamed1_Click"></asp:Button></td>
                            </tr>
                            <tr>
                                <td><asp:TextBox runat="server" Height="96px" Width="170px" id = "txtNote"></asp:TextBox></td>
                                <td rowspan = "3" colspan = "2">
                                    <asp:ListBox runat="server" id = "lbContract" Height="172px" Width="198px"></asp:ListBox>
                                </td>
                            </tr>
                            <tr>
                                <td><asp:CheckBox runat="server" id = "cbContract" checked></asp:CheckBox> Add To Contract</td>
                            </tr>
                            <tr>
                                <td><asp:CheckBox runat="server" id = "cbOwner"></asp:CheckBox> Add To Owner</td>
                                <td><asp:Button runat="server" Text="Remove" onclick="Unnamed2_Click"></asp:Button></td>
                            </tr>
                            <tr>
                                <td><asp:Button runat="server" Text="Save" onclick="Unnamed3_Click"></asp:Button></td>
                            </tr>
                        </table>

                    </asp:View>

                    <asp:View ID="View2" runat="server">
                        Select CSV file: <asp:FileUpload ID="file1" runat="server" />
                        <asp:Button ID="btnUpload" runat="server" Text="Upload" /><br />
                        <br /><br />
                        <asp:MultiView ID="MultiView2" runat="server">
                            <asp:View ID="View3" runat="server">
                                <table>
                                    <tr>
                                        <td colspan="4">
                                            <h3>Note</h3>
                                            <p>Use note below when Contract Note is checked to ignore</p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:TextBox Rows="4" runat="server" ID="tbNote" Width="100%" Height="80px"></asp:TextBox>
                                        </td>
                                    </tr>
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
                                        <td>Contract Note:</td>
                                        <td>
                                            <asp:CheckBox ID="cbNote" runat="server" Text="Ignore" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddNote" runat="server"></asp:DropDownList>
                                        </td>
                                    </tr>                                    

                                    <tr>
                                       <td colspan = "3">
                                            <asp:CheckBox runat="server" id = "cbOwner2"></asp:CheckBox> Add To Owner
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

