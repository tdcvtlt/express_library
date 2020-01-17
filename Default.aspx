<%@ Page Language="VB" MasterPageFile="~/crms.master" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" title="CRMS" %>

<%@ Register src="controls/LeadManagement/GlobalCalendar.ascx" tagname="GlobalCalendar" tagprefix="uc1" %>

<%@ Register src="controls/LeadManagement/leads.ascx" tagname="leads" tagprefix="uc2" %>

<%@ Register src="controls/LeadManagement/tasks.ascx" tagname="tasks" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
   <script language="javascript" type="text/javascript">
       function Toggle(ele) {
           document.getElementById(ele).style.display = (document.getElementById(ele).style.display == '') ? 'none' : '';
       }

       function Toggle_All() {
           var a = new Array('globalcalendar', 'Leads', 'MyLeads', 'MyLeadsWithoutTasks', 'MyTasksTasksAssigned', 'TasksAssigned', 'MyCompleted', 'OthersCompleted');
           for (i = 0; i < a.length; i++) {
               document.getElementById(a[i]).style.display = (document.getElementById(a[i]).style.display == '') ? 'none' : '';
           }
           document.getElementById('Toggle').innerHTML = (document.getElementById('Toggle').innerHTML == 'Collapse All') ? 'Expand All' : 'Collapse All';
       }
   </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <%Response.Write("Hello " & Session.Item("FirstName") & " " & Session.Item("LastName"))%>
        <%
        Dim ta() As String
        ta = Split(Session("Groups"), "|")
        If ta.Contains("Lead Management") Then
         %>
         <a href="javascript:Toggle_All();"><div id="Toggle">Collapse All</div></a>
        <table>
            <tr>
                <td valign="top">
                    <a href="javascript:Toggle('globalcalendar');">Calendar:</a><br />
                    <div id="globalcalendar" style="width:466px;height:224px;border:thin solid black;">    
                        <uc1:GlobalCalendar ID="GlobalCalendar1" runat="server" />
                    </div>
                    
                    
                </td>
                <td valign="top">
                    <a href="javascript:Toggle('MyTasksTasksAssigned');">My Tasks:</a><br />
                    <div id="MyTasksTasksAssigned" style="width:466px;height:224px;border:thin solid black;overflow:auto;">    
                        <uc3:tasks ID="tasks1" runat="server" />
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <a href="javascript:Toggle('MyLeads');">My Leads:</a><br />
                    <div id="MyLeads" 
                        style="width:939px; height:374px; border:thin solid black;overflow:auto;">    
                        <uc2:leads ID="leads2" runat="server" />
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <a href="javascript:Toggle('MyLeads');">My Leads:</a><br />
                    <div id="Logins" 
                        style="width:939px; height:374px; border:thin solid black;overflow:auto;">    
                        <asp:GridView ID="gvLogins" runat="server">
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>
        
        
        <% 
        End If
        ta = Nothing
        %>
</asp:Content>

