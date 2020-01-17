<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="BadDebtsAdjustment.aspx.vb" Inherits="Add_Ins_BadDebtsAdjustment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div>
    <fieldset>        
         <asp:RadioButtonList ID="rblType" runat="server" 
                    RepeatDirection="Horizontal" Font-Bold="true" Font-Italic="false" AutoPostBack="True">
                    <asp:ListItem Value="csv">Upload CSV</asp:ListItem>
                    <asp:ListItem Value="kcp">Enter Contract Numbers</asp:ListItem>
                </asp:RadioButtonList>
    </fieldset>
   

    <h1 style="font-variant:small-caps">bad debts adjustments</h1>

    <asp:MultiView runat="server" ID="mv1">
        <asp:View runat="server" ID="view1">
            
             Select CSV file: <asp:FileUpload ID="file1" runat="server" />
                        <asp:Button ID="btnUpload" runat="server" Text="Upload" /><br />
                        <br /><br />
                                              
            <br />
            <table>
                <tr>
                    <td>KCP ID Column</td>
                    <td><asp:CheckBox runat="server" ID="cbIgnoreID" Text="Ignore" /></td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlKcpID"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>KCP Column</td>
                    <td><asp:CheckBox runat="server" ID="cbIgnoreKCP" Text="Ignore" /></td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlKcp"></asp:DropDownList>
                    </td>
                </tr> 
                <tr>                    
                    <td colspan="4">&nbsp;</td>
                </tr>                                            
            </table>
        </asp:View>
        
        <asp:View runat="server" ID="view2">
                      
            <table>                                 
                <tr>
                    <td>
                        KCP Number                 
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="tbKCP"></asp:TextBox>
                    </td>                   
                    <td>
                        <asp:Button runat="server" ID="btnKcpAdd" Width="100%" Text="Add" />
                    </td>
                </tr> 
                <tr>
                    <td>KCP Invoice</td>
                    <td colspan="2">
                        <asp:DropDownList runat="server" ID="ddInvoice"></asp:DropDownList>                        
                    </td>
                </tr>                                              
                <tr>
                    <td colspan="2">
                        <asp:ListBox runat="server" ID="lbKCPs"  SelectionMode="Multiple" Width="100%" Font-Bold="true" ></asp:ListBox>
                    </td>
                    <td valign="top">
                        <asp:Button runat="server" ID="btnKcpRemove" Width="100%" Text="Remove" />
                    </td>
                </tr>
            </table>
            
        </asp:View>
    </asp:MultiView>

    <asp:Label ID="lblStatus" runat="server" Text="..." Font-Bold="true"></asp:Label>
    <asp:HiddenField ID="hfFile" Value = "" runat="server" />

     <ul id="menu">
        <li><asp:LinkButton ID="lbProcess" runat="server">Process</asp:LinkButton></li>
    </ul>
</div>
</asp:Content>

