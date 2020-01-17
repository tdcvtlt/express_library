<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SSLBRequestForm.aspx.vb" Inherits="Payroll_SSLBRequestForm" %>

<%@ Register src="../controls/DateField.ascx" tagname="DateField" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
            .style1
        {
            
            height: 104px;
            width: 700px;
        }
 p.MsoNormal
	{margin-bottom:.0001pt;
	font-size:12.0pt;
	font-family:"Arial","sans-serif";
	        margin-left: 0in;
            margin-right: 0in;
            margin-top: 0in;
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
                <table class="style1">
                    <tr>
                        <td align="left" height="132" style="padding-top:0in;padding-right:
  9.35pt;padding-bottom:0in;padding-left:9.35pt" valign="top">
                            <div style="mso-element: para-border-div; border: solid black 4.5pt; padding: 1.0pt 1.0pt 1.0pt 1.0pt; background: #F2F2F2; mso-shading: windowtext; mso-pattern: gray-5 auto">
                                <p align="center" class="MsoNormal" 
                                    style="text-align: center; background: #F2F2F2; mso-shading: windowtext; mso-pattern: gray-5 auto; border: none; mso-border-alt: solid black 4.5pt; padding: 0in; mso-padding-alt: 1.0pt 1.0pt 1.0pt 1.0pt; mso-element: frame; mso-element-frame-width: 6.7in; mso-element-frame-height: 1.1in; mso-element-frame-hspace: 9.35pt; mso-element-wrap: around; mso-element-anchor-vertical: paragraph; mso-element-anchor-horizontal: page; mso-element-left: 64.15pt; mso-element-top: .05pt">
                                    <b style="mso-bidi-font-weight:normal">
                                    <span style="font-size:24.0pt;mso-bidi-font-size:10.0pt;font-family:&quot;Times New Roman&quot;,&quot;serif&quot;;
  font-variant:small-caps">Request For Pay <o:p></o:p></span></b>
                                </p>
                                <p align="center" class="MsoNormal" 
                                    style="text-align: center; background: #F2F2F2; mso-shading: windowtext; mso-pattern: gray-5 auto; border: none; mso-border-alt: solid black 4.5pt; padding: 0in; mso-padding-alt: 1.0pt 1.0pt 1.0pt 1.0pt; mso-element: frame; mso-element-frame-width: 6.7in; mso-element-frame-height: 1.1in; mso-element-frame-hspace: 9.35pt; mso-element-wrap: around; mso-element-anchor-vertical: paragraph; mso-element-anchor-horizontal: page; mso-element-left: 64.15pt; mso-element-top: .05pt">
                                    <b style="mso-bidi-font-weight:normal">
                                    <span style="font-size:24.0pt;mso-bidi-font-size:10.0pt;font-family:&quot;Times New Roman&quot;,&quot;serif&quot;;
  font-variant:small-caps">Supplemental Sick Leave Bank</span></b><span style="font-variant:small-caps"><o:p></o:p></span></p>
                        </td>
                    </tr>
                </table>
            </center> 
            
                    <div id = "innerContent">
            <b>This form is initiated by the manager to Human Resources to request pay from the employee’s “Supplemental Sick Leave Bank.”  The VP of Human Resources will approve/deny this request based on the guidelines of the policy.</b>
    
            <br /><br />
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>    

            <table>
                <tr>
                    <td>Employee Name:</td>
                    <td>
                        <asp:DropDownList ID="ddEmployees" runat="server">
                        </asp:DropDownList>
                        
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
            Employee will be absent from work on the date(s) listed below:
            <br />

            <table>
                <tr>
                    <td>Start Date:</td>
                    <td><uc1:DateField ID="dteSDate" runat="server" /></td>
                    <td>End Date:</td>
                    <td><uc1:DateField ID="dteEDate" runat="server" /></td>
                </tr>
                <tr>
                    <td colspan = "5">Select Days of the Week Employee Will Be Taking Off:</td>
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
                    <td>SSLB Hours Per:</td>
                    <td><asp:TextBox ID="txtSSLBHours" runat="server" Width="33px">0</asp:TextBox></td>
                        <td colspan="2">
                            Unpaid Hours Per Day:
                            <asp:TextBox ID="txtUnPaidHours" runat="server" Width="33px">0</asp:TextBox>
                        </td>
                        <td>
                            <asp:LinkButton ID="lbAddDates" runat="server">Add Date(s)</asp:LinkButton>
                        </td>
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
                <td>Total SSLB Hours To Be Paid:</td>
                <td>
                    <asp:Label ID="lblSSLBHours" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Total Unpaid Hours:</td>
                <td>
                    <asp:Label ID="lblUnpaidHours" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>

        <br />
        Does the employee meet the following conditions?
        <ul style="list-style-type:none;">
            <li>1)	Has the employee been employed for 180 days (6 months)?  YES  or NO</li>
            <li>2)	Has the employee been absent greater than 5 days (1 week)?  YES or NO</li>
            <li>3)	Is the absence due to the employee’s personal prolonged illness or disability?  YES or NO</li>
            <li>4)	Has a doctor’s excuse been submitted by the employee? YES or NO</li>
        </ul>

        <br />

If the answer is NO to any of the questions listed, then the employee is not eligible at this time.  Please contact Human Resources to discuss the employee’s prolonged absence, further documentation may be required.

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
