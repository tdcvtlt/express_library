<%@ Page Title="Vacation Club Tours" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="VacationClubTours.aspx.vb" Inherits="wizards_VacationClubTours" %>
<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:multiview id = "MultiView1" runat="server">
    <asp:View runat="server" id = "View1">
        <asp:GridView runat="server" id = "gvVCTours" 
            EmptyDataText = "No Tours Being Conducted At This Time.">
            <Columns>
                <asp:ButtonField CommandName="Select" ShowHeader="True" Text="Check Out"></asp:ButtonField>
            </Columns>
        </asp:GridView>
    </asp:View>
    <asp:View runat="server" id = "View2">
    <table>
        <tr>
            <td>New Tour Status:</td>
            <td><uc1:Select_Item ID="siTourStatus" runat="server" /></td>
        </tr>
        <tr>
            <td colspan = '2'>Check the Premiums That Were Issued:</td>
        </tr>
    </table>
    <asp:GridView runat="server" id = "gvPremiums" EnableModelValidation="True">
        <Columns>
            <asp:TemplateField HeaderText="Select">
                <ItemTemplate>
                    <asp:checkbox ID="PremSelect" runat = "server" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:Button runat="server" Text="Check Out" onclick="Unnamed1_Click"></asp:Button>
    <asp:HiddenField runat="server" id = "hfTourID"></asp:HiddenField>
    </asp:View>
    <asp:View runat="server" id = "ViewDeny">ACCESS DENIED</asp:View>
</asp:multiview>
</asp:Content>

