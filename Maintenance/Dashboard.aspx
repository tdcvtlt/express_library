<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Dashboard.aspx.vb" Inherits="Maintenance_Dashboard" %>

<%@ Register assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI" tagprefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Dashboard</title>    
    <script src="../scripts/jquery-1.9.1.min.js"></script>
    <script src="../scripts/bootstrap.min.js"></script>
    <style type="text/css">
        .auto-style1 {
            text-align: left;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="container-fluid">
                    <div class="row">
                        <div style="text-align:center"><b>Maintenance Dashboard - Last Updated: <%=Date.Now %></b></div>
                        <table width="100%">
                            <tr>
                                <td style="vertical-align:top;">
                                    <asp:GridView ID="GridView1" runat="server" EnableModelValidation="True" AutoGenerateColumns="False" Width="100%" >
                                        <AlternatingRowStyle BackColor="#FFCCFF" />
                                        <Columns>
                                            <asp:BoundField DataField="Name" HeaderText="Name" >
                                            <ItemStyle HorizontalAlign="Left" />
                                            </asp:BoundField>

                                            <asp:BoundField DataField="Today" HeaderText="Total" >
                                            <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Completed" HeaderText="Completed" >
                                            <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="NotStarted" HeaderText="Not Started" >
                                            <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Other" HeaderText="Other" >
                                            <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>

                                            <%--<asp:BoundField DataField="WTD" HeaderText="Total" >
                                            <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="WTDCompleted" HeaderText="Completed" >
                                            <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="WTDNotStarted" HeaderText="Not Started" >
                                            <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="WTDOther" HeaderText="Other" >
                                            <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>--%>

                                            <asp:BoundField DataField="MTD" HeaderText="Total" >
                                            <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="MTDCompleted" HeaderText="Completed" >
                                            <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="MTDNotStarted" HeaderText="Not Started" >
                                            <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="MTDOther" HeaderText="Other" >
                                            <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                            
                            
                                        </Columns>
                                    </asp:GridView>
                                </td>
                                <td style="vertical-align:top;" rowspan="2">
                                    <asp:GridView  ID="gvQC" runat="server" EnableModelValidation="True" AutoGenerateColumns="false" Width="100%" ShowFooter="True">
                                        <AlternatingRowStyle BackColor="#FFCCFF" />
                                        <Columns>
                                            <asp:BoundField DataField="Section" HeaderText="Section" HeaderStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Due" HeaderText="Due" HeaderStyle-HorizontalAlign="Center" >
                                            <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="InProgress" HeaderText="In Progress" HeaderStyle-HorizontalAlign="Center" >
                                            <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Completed" HeaderText="Completed" HeaderStyle-HorizontalAlign="Center" >
                                            <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Tomorrow" HeaderText="Tomorrow" HeaderStyle-HorizontalAlign="Center" >
                                            <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                    
                                </td>
                                <td>
                                    <asp:GridView ID="gvMasterCorp" runat="server" EnableModelValidation="true" AutoGenerateColumns="false" Width="100%" ShowFooter="True">
                                        <AlternatingRowStyle BackColor="#FFCCFF" />
                                        <Columns>
                                            <asp:BoundField DataField="Section" HeaderText="Section" HeaderStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Due" HeaderText="Due" HeaderStyle-HorizontalAlign="Center" >
                                            <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="EarlyCheckIn" HeaderText="EarlyCheckIns" HeaderStyle-HorizontalAlign="Center" >
                                            <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Completed" HeaderText="Completed" HeaderStyle-HorizontalAlign="Center" >
                                            <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Tomorrow" HeaderText="Tomorrow" HeaderStyle-HorizontalAlign="Center" >
                                            <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                 </td>
                            </tr>
                            <tr>
                                <td style="vertical-align:top;">
                                    <asp:GridView  ID="GridView2" runat="server" EnableModelValidation="True" AutoGenerateColumns="false" Width="100%">
                                        <AlternatingRowStyle BackColor="#FFCCFF" />
                                        <Columns>
                                            <asp:BoundField DataField="Operation" HeaderText="Operation" HeaderStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Today" HeaderText="Today" HeaderStyle-HorizontalAlign="Center" >
                                            <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CheckedIn" HeaderText="Completed" HeaderStyle-HorizontalAlign="Center" >
                                            <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Tomorrow" HeaderText="Tomorrow" HeaderStyle-HorizontalAlign="Center" >
                                            <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="2 Days Out" HeaderText="2 Days Out" HeaderStyle-HorizontalAlign="Center" >
                                            <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                        
                    </div>
                    <%--<div style="text-align:center"><b>Frontdesk Dashboard</b></div>
                    <table style="width:100%;">
                        <tr>
                            <td style="vertical-align:top;">
                                <asp:GridView  ID="GridView2" runat="server" EnableModelValidation="True" AutoGenerateColumns="false" Width="100%">
                                    <AlternatingRowStyle BackColor="#FFCCFF" />
                                    <Columns>
                                        <asp:BoundField DataField="Operation" HeaderText="Operation" HeaderStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Today" HeaderText="Today" HeaderStyle-HorizontalAlign="Center" >
                                        <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CheckedIn" HeaderText="Completed" HeaderStyle-HorizontalAlign="Center" >
                                        <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Tomorrow" HeaderText="Tomorrow" HeaderStyle-HorizontalAlign="Center" >
                                        <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="2 Days Out" HeaderText="2 Days Out" HeaderStyle-HorizontalAlign="Center" >
                                        <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                            <td style="vertical-align:top;">
                                <asp:GridView  ID="gvFDStaff" runat="server" EnableModelValidation="True" AutoGenerateColumns="false" Width="100%" >
                                    <AlternatingRowStyle BackColor="#FFCCFF" />
                                    <Columns>
                                        <asp:BoundField DataField="Name" HeaderText="Name" HeaderStyle-HorizontalAlign="Center">
                                        <ItemStyle HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Today" HeaderText="Today" HeaderStyle-HorizontalAlign="Center" >
                                        <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="WTD" HeaderText="WTD" HeaderStyle-HorizontalAlign="Center" >
                                        <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="MTD" HeaderText="MTD" HeaderStyle-HorizontalAlign="Center" >
                                        <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>--%>
                        
                        
                        
                            
                    </div>
                    
            </ContentTemplate>        
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="tick" />
            </Triggers>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="15">
            <ProgressTemplate>
                <img src="../images/progress-square.gif" />
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:Timer ID="Timer1" runat="server" Interval="5000">
        </asp:Timer>
    </form>
</body>
</html>
