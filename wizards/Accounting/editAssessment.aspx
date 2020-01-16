<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="editAssessment.aspx.vb" Inherits="wizards_Accounting_editAssessment" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<%@ Register Src="~/controls/DateField.ascx" TagPrefix="uc1" TagName="DateField" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <ul id="menu">
            <li <%if mvMain.ActiveViewIndex = 0 Then : Response.Write("class=""current""") : End If %>><asp:LinkButton ID="Setup" runat="server">Setup</asp:LinkButton></li>
            <li <%if  mvMain.ActiveViewIndex = 1 Then : Response.Write("class=""current""") : End If %>><asp:LinkButton ID="Assessment" runat="server">Assessment</asp:LinkButton></li>
            <li <%if  mvMain.ActiveViewIndex = 2 Then : Response.Write("class=""current""") : End If %>><asp:LinkButton ID="Print" runat="server">Print</asp:LinkButton></li>
        </ul>
    <asp:MultiView runat="server" ID="mvMain">
        <asp:View runat="server" ID="View1">
            Assessment Type: <asp:DropDownList ID="ddType" runat="server" AutoPostBack="True"></asp:DropDownList>
            <asp:Table ID="tblDetails" runat="server">
                <asp:TableRow ID="trY2A" runat="server">
                    <asp:TableCell>Year to Assess:</asp:TableCell>
                    <asp:TableCell><asp:DropDownList ID="ddY2A" runat="server"></asp:DropDownList></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="trCutOffDate" runat="server">
                    <asp:TableCell>Cut-Off Date:</asp:TableCell>
                    <asp:TableCell><uc1:DateField runat="server" ID="dfCutoffDate" /></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="trDueDate" runat="server">
                    <asp:TableCell>Due Date:</asp:TableCell>
                    <asp:TableCell><uc1:DateField runat="server" ID="dfDueDate" /></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="trLFAmount" runat="server">
                    <asp:TableCell>Late Fee Amount:</asp:TableCell>
                    <asp:TableCell><asp:TextBox ID="txtLFAmount" runat="server"></asp:TextBox></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="trDaysLate" runat="server">
                    <asp:TableCell>Days Late:</asp:TableCell>
                    <asp:TableCell><asp:TextBox ID="txtDaysLate" runat="server"></asp:TextBox></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="trStatuses" runat="server">
                    <asp:TableCell ColumnSpan="2">
                        Please Choose Contract Status(es) to Assess:
                        <table>
                            <tr>
                                <td><asp:ListBox ID="lstNewStatus" runat="server"></asp:ListBox></td>
                                <td>
                                    <asp:Button ID="btnAdd" runat="server" Text=">>" /><br />
                                    <asp:Button ID="btnRemove" runat="server" Text="<<" />
                                </td>
                                <td><asp:ListBox ID="lstStatus" runat="server"></asp:ListBox></td>
                            </tr>
                        </table>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="trButtons" runat="server">
                    <asp:TableCell ColumnSpan="2">
                        <asp:Button ID="btnPreview" runat="server" Text="Save/Preview" />
                        <asp:Button ID="btnAssess" runat="server" Text="Assess" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </asp:View>
        <asp:View runat="server" ID="View2">
            <asp:GridView ID="GridView1" runat="server" AutoGenerateSelectButton="true" DataKeyNames="BatchItemID" AutoGenerateColumns="true" EmptyDataText="No Records" GridLines="Horizontal">
                <SelectedRowStyle BackColor="#CCFFFF" Wrap="True" />
                <AlternatingRowStyle BackColor="#CCFFCC" />        
                
            </asp:GridView>
        </asp:View>
        <asp:View runat="server" ID="View3">
            <CR:CrystalReportViewer ID="crViewer" runat="server" AutoDataBind="true" />
        </asp:View>
    </asp:MultiView>
</asp:Content>


