<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="TourCountsByWave1.aspx.vb" Inherits="Reports_Tours_TourCountsByWave1" %>

<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>

<%@ Register Src="../../controls/DateField.ascx" TagName="DateField" TagPrefix="uc1" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script language="javascript">
        var tableToExcel = (function () {
            var uri = 'data:application/vnd.ms-excel;base64,'
              , template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--><meta http-equiv="content-type" content="text/plain; charset=UTF-8"/></head><body><table>{table}</table></body></html>'
              , base64 = function (s) { return window.btoa(unescape(encodeURIComponent(s))) }
              , format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) }
            return function (table, name) {
                if (!table.nodeType) table = document.getElementById(table)
                var ctx = { worksheet: name || 'Worksheet', table: table.innerHTML }
                window.location.href = uri + base64(format(template, ctx))
            }
        })()
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table>
        <tr>
            <td>Type(s):</td>
            <td align="center">&nbsp;</td>
            <td>Selected:</td>
        </tr>
        <tr>
            <td>
                <asp:ListBox ID="ListBox1" runat="server"></asp:ListBox>
            </td>
            <td align="center">
                <asp:Button ID="Button4" runat="server" Text="<< ALL" Style="height: 29px" /><br />
                <asp:Button ID="Button1" runat="server" Text="<<" /><br />
                <asp:Button ID="Button2" runat="server" Text=">>" /><br />
                <asp:Button ID="Button5" runat="server" Text="ALL >>" />
            </td>
            <td>
                <asp:ListBox ID="ListBox2" runat="server"></asp:ListBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:ListBox ID="lbWave" runat="server"></asp:ListBox>
            </td>
            <td align="center">
                <asp:Button ID="Button3" runat="server" Text="<< ALL" Style="height: 29px" /><br />
                <asp:Button ID="Button6" runat="server" Text="<<" /><br />
                <asp:Button ID="Button7" runat="server" Text=">>" /><br />
                <asp:Button ID="Button8" runat="server" Text="ALL >>" />
            </td>
            <td>
                <asp:ListBox ID="lbWaves" runat="server"></asp:ListBox>
            </td>
        </tr>
        <tr>
            <td>Start Date:</td>
            <td>
                <uc1:DateField ID="dfStartDate" runat="server" />
            </td>
        </tr>
        <tr>
            <td>End Date:</td>
            <td>
                <uc1:DateField ID="dfEndDate" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btnRunReport" runat="server" Text="Run Report" />
                <input type="button" value="Export to Excel" onclick="if(document.getElementById('listViewReport').innerHTML.indexOf('table') >0) {tableToExcel('listViewReport', 'Tour Counts By Wave')}"/>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hfShowReport" Value="0" runat="server" />
    <div id="listViewReport">
        <asp:ListView ID="lvReport" runat="server">
            <LayoutTemplate>
                <table border="1" style="border: thin solid #000000">
                    <tbody>
                        <tr runat="server" id="itemPlaceholder" />
                    </tbody>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <%# AddGroupingRowIfChanged() %>
                <tr style="border: thin solid #000000">
                    <td id="Td1" runat="server" style="border: thin solid #000000">
                        <asp:Label ID="Label1" runat="server" Text='<%#Eval("Wave") %>' />
                    </td>
                    <td id="Td2" runat="server" style="border: thin solid #000000">
                        <asp:Label ID="Label2" runat="server" Text='<%#Eval("Day1Wave") %>' />
                    </td>
                    <td id="Td3" runat="server" style="border: thin solid #000000">
                        <asp:Label ID="Label3" runat="server" Text='<%#Eval("Day2Wave") %>' />
                    </td>
                    <td id="Td4" runat="server" style="border: thin solid #000000">
                        <asp:Label ID="Label4" runat="server" Text='<%#Eval("Day3Wave") %>' />
                    </td>
                    <td id="Td5" runat="server" style="border: thin solid #000000">
                        <asp:Label ID="Label5" runat="server" Text='<%#Eval("Day4Wave") %>' />
                    </td>
                    <td id="Td6" runat="server" style="border: thin solid #000000">
                        <asp:Label ID="Label6" runat="server" Text='<%#Eval("Day5Wave") %>' />
                    </td>
                    <td id="Td7" runat="server" style="border: thin solid #000000">
                        <asp:Label ID="Label7" runat="server" Text='<%#Eval("Day6Wave") %>' />
                    </td>
                    <td id="Td8" runat="server" style="border: thin solid #000000">
                        <asp:Label ID="Label8" runat="server" Text='<%#Eval("Day7Wave") %>' />
                    </td>
                </tr>
            </ItemTemplate>
        </asp:ListView>
    </div>

    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server"
        AutoDataBind="true" ToolPanelView="None" />

</asp:Content>

