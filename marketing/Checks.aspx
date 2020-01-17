<%@ Page Title="Checks" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="Checks.aspx.vb" Inherits="marketing_Checks" %>
<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>
<%@ Register src="../controls/Select_Item.ascx" tagname="Select_Item" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<table><tr><td>Select Location:</td><td><uc2:Select_Item ID="siLocation" runat="server" /></td></tr></table>
<ul id = "menu">
    <li <%if  MultiView1.ActiveViewIndex = 0 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Add_Link" runat="server">Add Checks</asp:LinkButton></li>
    <li <%if  MultiView1.ActiveViewIndex = 1 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Print_Link" runat="server">Print Checks</asp:LinkButton></li>
    <li <%if  MultiView1.ActiveViewIndex = 2 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Void_Link" runat="server">Void Checks</asp:LinkButton></li>
    <li <%if  MultiView1.ActiveViewIndex = 3 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Lookup_Link" runat="server">Lookup Checks</asp:LinkButton></li>
    <li <%if  MultiView1.ActiveViewIndex = 4 then: response.write ("class=""current"""):end if %>><asp:LinkButton ID="Report_Link" runat="server">Report</asp:LinkButton></li>
</ul>
<asp:MultiView runat="server" id = "MultiView1">
    <asp:View runat="server" id = "Add_View">
        <table>
            <tr>
                <td>Starting Page Position:</td>
                <td><asp:DropDownList runat="server" id = "ddPagePos"></asp:DropDownList></td>
            </tr>
            <tr>
                <td>Starting Number:</td>
                <td><asp:TextBox runat="server" id = "txtStartNum"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Ending Number:</td>
                <td><asp:TextBox runat="server" id = "txtEndNum"></asp:TextBox></td>
            </tr>
            <tr>
                <td><asp:Button runat="server" Text="Enter Checks" onclick="Unnamed1_Click"></asp:Button></td>
            </tr>
        </table>
        <asp:Label runat="server" id = "lblMsg"></asp:Label>
    </asp:View>
    <asp:View runat="server" id = "Print_View">
        <table>
            <tr>
                <td>Starting Number:</td>
                <td><asp:TextBox runat="server" id = "txtChksStart"></asp:TextBox></td>
            </tr>
            <tr>
                <td>Check Date:</td>
                <td><uc1:DateField ID="dtechkDate" runat="server" /></td>
            </tr>
            <tr>
                <td><asp:Button runat="server" Text="Print Checks" onclick="Unnamed2_Click1"></asp:Button></td>
            </tr>
        </table>
        <asp:GridView runat="server" id = "gvChecksPrint" EmptyDataText = "No Records" onRowDataBound = "gvChecksPrint_RowDataBound">
            <Columns>
                 <asp:TemplateField HeaderText="Print">
                    <ItemTemplate>
                        <asp:checkbox ID="chkPrint" runat = "server" AutoPostBack="true" OnCheckedChanged = "chkPrint_CheckedChanged"/>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </asp:View>
    <asp:View runat="server" id = "Void_View">
        Check Number: <asp:TextBox runat="server" id = "txtVoidNum"></asp:TextBox> 
        <asp:Button runat="server" Text="Void" onclick="Unnamed2_Click"></asp:Button><br />
        <asp:Label runat="server" id = "lblVoid"></asp:Label>
    </asp:View>
    <asp:View runat = "server" id = "Lookup_View">
        Check Number: <asp:TextBox runat="server" id = "txtLookup"></asp:TextBox> 
        <asp:Button runat="server" Text="Lookup" onclick="Unnamed3_Click"></asp:Button>
        <br />
        <asp:GridView runat="server" id = "gvChkLookup" EmptyDataText = "No Records"></asp:GridView>
    </asp:View>
    <asp:View runat = "server" id = "Report_View">
        <table>
            <tr>
                <td>Start Date:</td>
                <td><uc1:DateField ID="dteStartDate" runat="server" /></td>
            </tr>
            <tr>
                <td>End Date:</td>
                <td><uc1:DateField ID="dteEndDate" runat="server" /></td>
            </tr>
            <tr>
                <td colspan = '2'><asp:Button runat="server" Text="Run Report" 
                        onclick="Unnamed4_Click"></asp:Button></td>
            </tr>
        </table>
        <br />
        <asp:GridView runat="server" id = "gvCheckRpt" EmptyDataText = "No Records"></asp:GridView>
    </asp:View>

</asp:MultiView>
</asp:Content>

