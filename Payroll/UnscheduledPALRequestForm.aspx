<%@ Page Language="VB" AutoEventWireup="false" CodeFile="UnscheduledPALRequestForm.aspx.vb" Inherits="Payroll_UnscheduledPALRequestForm" %>

<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 450px;
            height: 104px;
        }
        #innerContent
        {
            margin-left: 88px;
            margin-right: 77px;
        }
    </style>
</head>
<body>

    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <center>
            <img alt="" class="style1" src="../images/kcp_logo.bmp" /></center>
            <br />
            <center><b>EMPLOYEE ABSENCE REPORT</b></center>
            <center><b>(UNSCHEDULED LEAVE)</b></center>
            <br /><br />
            <div id = "innerContent">
            <b>This form is initiated by employee to request pay for time off due to unscheduled absences (illness, emergencies, etc.).  Five (5) consecutive Paid Annual Leave (PAL) days must first be used before accessing the Supplemental Sick Leave Bank (SSLB).</b>
    
            <br /><br />
            
            <table>
                <tr>
                    <td>Employee Name:</td>
                    <td>
                        <asp:Label ID="lblEmployee" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>Department:</td>
                    <td>
                        <asp:DropDownList ID="ddDept" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>Date:</td>
                    <td>
                        <asp:Label ID="lblDate" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
            
            <br />
            I was absent from work on the date(s) listed below:
            <br />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>    
            <table>
                <tr>
                    <td>Start Date:</td>
                    <td><uc1:DateField ID="dteSDate" runat="server" /></td>
                    <td>End Date:</td>
                    <td><uc1:DateField ID="dteEDate" runat="server" /></td>
                </tr>
                <tr>
                    <td colspan = "5">Select Days of the Week You Will Be Taking Off:</td>
                </tr>
                <tr>
                    <td colspan = "5">
                        <asp:CheckBoxList ID="cbWeekDays" runat="server" 
                            RepeatDirection="Horizontal">
                            <asp:ListItem>Monday</asp:ListItem>
                            <asp:ListItem>Tuesday</asp:ListItem>
                            <asp:ListItem>Wednesday</asp:ListItem>
                            <asp:ListItem>Thursday</asp:ListItem>
                            <asp:ListItem>Friday</asp:ListItem>
                            <asp:ListItem>Saturday</asp:ListItem>
                            <asp:ListItem>Sunday</asp:ListItem>
                        </asp:CheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td colspan = "2">PAL Hours Per Day:
                    <asp:TextBox ID="txtPALHours" runat="server" Width="33px">0</asp:TextBox></td>
                    <!--<td>SSLB Hours:</td>
                    <td><asp:TextBox ID="txtSSLBHours" runat="server" Width="33px">0</asp:TextBox></td>-->
                    <td colspan = "2">Unpaid Hours Per Day:
                    <asp:TextBox ID="txtUnPaidHours" runat="server" Width="33px">0</asp:TextBox></td>

                </tr>
                <tr>
                    <td>Reason:</td>
                    <td colspan = "3">
                        <asp:TextBox ID="txtReason" runat="server" Width="494px"></asp:TextBox>
                    </td>
                    <td><asp:LinkButton ID="lbAddDates" runat="server">Add Date(s)</asp:LinkButton></td>
                </tr>
            </table>

        <asp:GridView ID="gvDates" runat="server"  EnableModelValidation="True">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lbDeleted" runat="server" onclick="lbDeleted_Click">Delete</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>
        
        <br />
        <table>
            <tr>
                <td>Total PAL Hours To Be Paid:</td>
                <td>
                    <asp:Label ID="lblPALHours" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <!--<tr>
                <td>Total SSLB Hours To Be Paid:</td>
                <td>
                    <asp:Label ID="lblSSLBHours" runat="server" Text=""></asp:Label>
                </td>
            </tr>-->
            <tr>
                <td>Total Unpaid Hours:</td>
                <td>
                    <asp:Label ID="lblUnpaidHours" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
        <br />
        Doctor's Excuse Attached <asp:CheckBox ID="cbDocNote" runat="server" />
        <br /><br />
        <i>I certify that this absence is authorized and in accordance with current policies.  I authorize KCP to deduct money from my paycheck for any benefits which are taken in error or not fully earned. In the event I resign or am terminated, KCP may withhold money for overpaid benefits from my final check.</i>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="gvDates" />
</Triggers> 
        </asp:UpdatePanel>
        <br />
        <asp:Button ID="btnSubmit" runat="server" Text="Submit Leave Request" />
</div>
    </div>

    </form>
</body>
</html>
