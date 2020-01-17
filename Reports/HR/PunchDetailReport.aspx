<%@ Page Title="Punch Detail Report" AspCompat="true" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="PunchDetailReport.aspx.vb" Inherits="Reports_HR_PunchDetailReport" %>

<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script language="javascript" type="text/javascript">
        function Printable() {
            var mWin = window.open();
            mWin.document.write(document.getElementById('printableversion').innerHTML);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table>
    <tr>
        <td>Filter By: </td>
        <td><asp:RadioButtonList ID="rblFilter" runat="server" autopostback="true"
            RepeatDirection="Horizontal">
            <asp:ListItem>Company</asp:ListItem>
            <asp:ListItem>Department</asp:ListItem>
            <asp:ListItem>Employee</asp:ListItem>
        </asp:RadioButtonList></td>
    </tr>
    </table>
    <table>
        <tr>
            <td colspan="2">Department:
                <asp:DropDownList ID="ddDepartment" runat="server">
                </asp:DropDownList>
                <asp:Button ID="btnAdd" runat="server" Text="Add" /><br />
                <asp:CheckBox ID="cbInactive" runat="server" Text="Inactive" />
            </td>
            <td rowspan="2">List<br />
                <asp:ListBox ID="lbDepts" runat="server"></asp:ListBox></td>
        </tr>
        <tr>
            <td>Start Date:</td>
            <td>
                <uc1:DateField ID="dfSDate" runat="server" />
            </td>
        </tr>
        <tr>
            <td>End Date:</td>
            <td>
                <uc1:DateField ID="dfEDate" runat="server" />
            </td>
        </tr>
    </table>
    <asp:Button ID="btnRun" runat="server" Text="Run Report" />
    <asp:Button ID="btnExcel" runat="server" Text="Printable Version" 
        onclientclick="Printable();" />
    <asp:HiddenField ID="hfShowReport" Value = "0" runat="server" />
    <div id = "printableversion">
    <asp:Literal ID="Lit1" runat="server"></asp:Literal>
    </div>
</asp:Content>

