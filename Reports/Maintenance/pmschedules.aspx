<%@ Page Title="" Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="pmschedules.aspx.vb" Inherits="Reports_Maintenance_pmschedules" %>
<%@ Register src="../../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div style="margin-left:10px;">
                    
<h2>pm schedules</h2>
<table cellpadding="3" cellspacing="3">
<tr>
    <td>Service Date</td>
    <td>
        <uc1:DateField ID="sdate" runat="server" />
    </td>
    <td rowspan="6" valign="top" style="width:200px;">
        <h3>ROOMS</h3>
        <div style="overflow:auto;height:200px;">                       
            <asp:CheckBoxList runat="server" ID="rmCheckBoxList"></asp:CheckBoxList>
        </div>
    </td> 
    <td rowspan="6" valign="top" style="width:200px;">
        <h3>BUILDINGS</h3>  
        <div style="overflow:auto;height:200px;">                      
            <asp:CheckBoxList runat="server" ID="bldCheckBoxList"></asp:CheckBoxList>
        </div>
    </td>  
    <td rowspan="6" valign="top" style="width:200px;">
        <h3>TASKS</h3>   
        <div style="overflow:auto;height:200px;">                     
            <asp:CheckBoxList runat="server" ID="tskCheckBoxList"></asp:CheckBoxList>
        </div>    
    </td>
    <td rowspan="6" valign="top" style="width:200px;">
        <h3>PM ITEMS</h3>      
        <div style="overflow:auto;height:200px;">                  
            <asp:CheckBoxList runat="server" ID="itmCheckBoxList"></asp:CheckBoxList>
        </div>    
    </td>     
</tr>
<tr>
    <td>End Date</td>
    <td>
        <uc1:DateField ID="edate" runat="server" />
    </td>    
</tr>   
<tr>
    <td colspan="6"></td>
</tr>
<tr>
    <td colspan="6"></td>
    
</tr>
<tr>
    <td colspan="6"></td>    
</tr>
<tr>
    <td colspan="6"></td>
</tr>                
</table>
<br />
<div>
    <asp:Button runat="server" ID="submitButton" Text="Report" />
    <br />
    <br />
    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />
</div>
</div>
</asp:Content>

