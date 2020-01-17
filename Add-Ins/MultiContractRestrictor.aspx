<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="MultiContractRestrictor.aspx.vb" Inherits="Add_Ins_MultiContractRestrictor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table>
    <tr>
        <td colspan="3">
            <fieldset>
                <legend>
                    Select:
                </legend>

                <asp:RadioButtonList ID="rblType" runat="server" 
                RepeatDirection="Horizontal" AutoPostBack="True">
                <asp:ListItem Value="csv">Upload CSV</asp:ListItem>
                <asp:ListItem Value="kcp">Enter Contract Numbers</asp:ListItem>
            </asp:RadioButtonList>
            </fieldset>
            
        </td>
    </tr>
    <tr>
        <td colspan="3">&nbsp;</td>
    </tr>

    <tr>
        <td colspan="3">
            <asp:MultiView ID="mvContainer" runat="server">
                <asp:View ID="vwContract" runat="server">
                    <table>
                        <tr>
                            <td>Restrictor:</td>
                            <td><asp:DropDownList runat="server" id = "ddRestrictor"></asp:DropDownList>
                                <asp:Button runat="server" Text="Add" onclick="Unnamed1_Click"></asp:Button>
                            </td>
                            <td>Contract #: </td>
                            <td><asp:TextBox runat="server" id = "txtContract"></asp:TextBox></td>
                            <td><asp:Button runat="server" Text="Add" id = "AddCon"></asp:Button></td>
                        </tr>
                        <tr>
                            <td colspan = "2"><asp:ListBox runat="server" id = "lbRestrictor" Height="172px" 
                                    Width="448px"></asp:ListBox></td>
                            <td colspan = "2">
                                <asp:ListBox runat="server" id = "lbContract" Height="172px" Width="198px"></asp:ListBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan = "2"><asp:Button runat="server" Text="Remove Restrictor" onclick="Unnamed2_Click"></asp:Button></td>
                            <td colspan = '2'><asp:Button runat="server" Text="Remove Contract" id = "RemCon"></asp:Button></td>
                        </tr>
                        <tr>
                            <td><asp:Button runat="server" Text="Save" style="display:none;" onclick="Unnamed3_Click"></asp:Button></td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vwCsv" runat="server">
                    <div>
                        <br />
                        <p><strong>Select CSV file:</strong></p> 
                        <asp:FileUpload ID="file1" runat="server" />
                        <asp:Button ID="btnUpload" runat="server" Text="Upload" /><br />
                        <br />
                    </div>
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
                            <td>New Contract Restrictor Column:</td>
                            <td>&nbsp;</td>
                            <td><asp:DropDownList ID="ddlRestrictor" runat="server"></asp:DropDownList></td>
                        </tr>
                    </table>
                </asp:View>
            </asp:MultiView>
        </td>
       
    </tr>        
</table>
    <br />
    <strong style="color:rebeccapurple;">
        <asp:Label runat="server" id = "lblErr"></asp:Label>
    </strong>
    
    <br />
    
    <ul id="menu">
        <li><asp:LinkButton ID="lbProcess" runat="server">Process</asp:LinkButton></li>
    </ul>
    <asp:HiddenField ID="hfFile" Value = "" runat="server" />

</asp:Content>

