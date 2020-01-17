Imports System.Data.SqlClient

Partial Class controls_LeadManagement_tasks
    Inherits System.Web.UI.UserControl

    Public MyTasks As Boolean = True
    Public Completed As Boolean = False
    Public Window As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If MyTasks Then
                If Completed Then
                    MultiView1.ActiveViewIndex = 2
                    TasksICompleted()
                Else
                    MultiView1.ActiveViewIndex = 0
                    My_Tasks()
                End If
            Else
                If Completed Then
                    MultiView1.ActiveViewIndex = 3
                    TasksOthersCompleted()
                Else
                    MultiView1.ActiveViewIndex = 1
                    TasksIAssigned()
                End If
            End If
        End If
    End Sub

    Sub Run_Query(ByVal sSQL As String, ByRef GridView1 As GridView)
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("Replace This", cn)
        Dim dr As SqlDataReader
        Try
            cn.Open()
            cm.CommandText = sSQL
            dr = cm.ExecuteReader
            GridView1.DataSource = dr
            Dim ka(0) As String
            ka(0) = "prospectid"
            GridView1.DataKeyNames = ka
            GridView1.DataBind()
            cn.Close()
        Catch ex As Exception
            lblException.Text = ex.ToString
        Finally
            If cn.State <> Data.ConnectionState.Closed Then cn.Close()
            cn = Nothing
            cm = Nothing
            dr = Nothing
        End Try
    End Sub

    Private Sub My_Tasks()
        Run_Query("Select g.EventDate, et.comboitem as eventtype, cm.comboitem as contactmethod, " & _
         "p.prospectid, ltrim(rtrim(p.firstname)) + ' ' + ltrim(rtrim(p.lastname)) as Name, g.calendarentryid, g.personnelid, g.event " & _
            "from t_GlobalCalendar g " & _
             "left outer join t_Comboitems et on et.comboitemid = g.eventtypeid " & _
             "left outer join t_ProspectContact pc on pc.pros2contactid = g.contactid " & _
             "left outer join t_Comboitems cm on cm.comboitemid = pc.contactmethodid " & _
             "left outer join t_Prospect p on p.prospectid = pc.prospectid " & _
            "where g.completed <> 1 " & _
             "and g.personnelid = '" & Session("UserDBID") & "'" & _
             "and g.eventdate <=getdate() + " & Window, gvMyTasks)
    End Sub

    Private Sub TasksIAssigned()
        Run_Query("Select g.EventDate, et.comboitem as eventtype, cm.comboitem as contactmethod, " & _
         "p.prospectid, ltrim(rtrim(p.firstname)) + ' ' + ltrim(rtrim(p.lastname)) as Name, " & _
             "g.calendarentryid, g.personnelid, g.event, " & _
             "per.assignedto " & _
            "from t_GlobalCalendar g " & _
             "left outer join t_Comboitems et on et.comboitemid = g.eventtypeid " & _
             "left outer join t_ProspectContact pc on pc.pros2contactid = g.contactid " & _
             "left outer join t_Comboitems cm on cm.comboitemid = pc.contactmethodid " & _
             "left outer join t_Prospect p on p.prospectid = pc.prospectid " & _
             "left outer join ( " & _
              "select * from t_Rep2Pros where dateremoved is null " & _
              ") l on l.prospectid = p.prospectid " & _
             "left outer join ( " & _
              "select personnelid, rtrim(ltrim(firstname)) as assignedto from t_Personnel " & _
             ") per on per.personnelid = g.personnelid " & _
            "where g.completed <> 1 " & _
             "and g.personnelid <> '" & Session("UserDBID") & "' " & _
             "and g.eventdate <=getdate()  + " & Window & _
             " and l.personnelid = '" & Session("UserDBID") & "'", gvOthersTasks)
    End Sub

    Private Sub TasksICompleted()
        Run_Query("Select g.EventDate, et.comboitem as eventtype, cm.comboitem as contactmethod, " & _
         "p.prospectid, ltrim(rtrim(p.firstname)) + ' ' + ltrim(rtrim(p.lastname)) as Name, " & _
             "g.calendarentryid, g.personnelid, g.event " & _
            "from t_GlobalCalendar g " & _
             "left outer join t_Comboitems et on et.comboitemid = g.eventtypeid " & _
             "left outer join t_ProspectContact pc on pc.pros2contactid = g.contactid " & _
             "left outer join t_Comboitems cm on cm.comboitemid = pc.contactmethodid " & _
             "left outer join t_Prospect p on p.prospectid = pc.prospectid " & _
            "where g.completed = 1 " & _
             "and g.personnelid = '" & Session("UserDBID") & "' " & _
             "and g.eventdate >getdate()-1 +" & Window, gvMyCompleted)
    End Sub

    Private Sub TasksOthersCompleted()
        Run_Query("Select g.EventDate, et.comboitem as eventtype, cm.comboitem as contactmethod, " & _
         "p.prospectid, ltrim(rtrim(p.firstname)) + ' ' + ltrim(rtrim(p.lastname)) as Name, " & _
             "g.calendarentryid, g.personnelid, g.event, " & _
             "per.assignedto " & _
            "from t_GlobalCalendar g " & _
             "left outer join t_Comboitems et on et.comboitemid = g.eventtypeid " & _
             "left outer join t_ProspectContact pc on pc.pros2contactid = g.contactid " & _
             "left outer join t_Comboitems cm on cm.comboitemid = pc.contactmethodid " & _
             "left outer join t_Prospect p on p.prospectid = pc.prospectid " & _
             "left outer join ( " & _
              "select * from t_Rep2Pros where dateremoved is null " & _
              ") l on l.prospectid = p.prospectid " & _
             "left outer join ( " & _
              "select personnelid, rtrim(ltrim(firstname)) as assignedto from t_Personnel " & _
             ") per on per.personnelid = g.personnelid " & _
            "where g.completed = 1 " & _
             "and g.personnelid <> '" & Session("UserDBID") & "' " & _
             "and l.personnelid = '" & Session("UserDBID") & "'" & _
             "and g.eventdate >getdate()-1 +" & Window, gvOthersCompleted)

    End Sub

    Protected Sub gvMyTasks_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvMyTasks.SelectedIndexChanged

    End Sub

    Protected Sub gvOthersTasks_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvOthersTasks.SelectedIndexChanged

    End Sub

    Protected Sub gvMyCompleted_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvMyCompleted.SelectedIndexChanged

    End Sub

    Protected Sub gvOthersCompleted_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvOthersCompleted.SelectedIndexChanged

    End Sub

End Class
