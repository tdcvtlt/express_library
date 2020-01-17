<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="EditRateTable.aspx.vb" Inherits="setup_EditRateTable" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<ul id="menu">
    <li <% if MultiView1.ActiveViewIndex = 0 then: response.write("class=""current"""):end if %>>
        <asp:LinkButton ID="lnkRateTable" runat="server">RateTable</asp:LinkButton>
    </li>    
</ul>
    <asp:MultiView ID="MultiView1" runat="server">
        <asp:View ID="vwRateTable" runat="server">
            <table>
                <tr>
                    <td>ID</td>
                    <td><asp:TextBox ID="txtID" runat="server" ReadOnly="true"></asp:TextBox></td>
                    <td>Name</td>
                    <td><asp:TextBox ID="txtName" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td colspan = "2">
                        <asp:Button ID="btnSave" runat="server" Text="Save" />
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="vwRates" runat="server">
        </asp:View>
    </asp:MultiView>
</asp:Content>

