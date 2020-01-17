<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="UsageByDay.aspx.vb" Inherits="Reports_OwnerServices_UsageByDay" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


<table border="0">

<tr>
<td>Date From:</td>
<td><uc1:DateField ID="sdate" runat="server" /></td>
</tr>
<tr>
<td>Date To:</td>
<td><uc1:DateField ID="edate" runat="server" /></td>
</tr>
<tr>
    <td>&nbsp;</td>
    <td>
        <table>
            <tr>
                <td>
                    <asp:ListBox runat="server" ID="lbL" Rows="7" Width="130px"></asp:ListBox>
                    &nbsp;
                </td>
                <td valign="middle">
                    <asp:Button runat="server" ID="btL" Text=">" Font-Bold="true" 
                        style="width: 40px" />
                    &nbsp;
                    <br />
                    <asp:Button runat="server" ID="btLAll" Text=">>" Font-Bold="true" 
                        style="width: 40px" />
                    &nbsp;
                    <br />
                    

                    <asp:Button runat="server" ID="btR" Text="<" Font-Bold="true" style="width: 40px" />
                    &nbsp;
                    <br />
                    <asp:Button runat="server" ID="btRAll" Text="<<" Font-Bold="true" 
                        style="width: 40px" />
                    &nbsp;
                    <br />
                </td>                
                <td>
                    &nbsp;                    
                    <asp:ListBox runat="server" ID="lbR" Rows="7" Width="130px" ></asp:ListBox>
                </td>
            </tr>
        </table>    
    </td>
</tr>
<tr>
    <td colspan="2"><asp:Button ID="btSubmit" runat="server" Text="Submit" /></td>
</tr>

</table>

<div>
<CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" GroupTreeStyle-ShowLines="false" ToolPanelView="None" />
</div>

</asp:Content>

