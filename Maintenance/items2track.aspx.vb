Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.ComponentModel
Imports System.IO
Imports System.Reflection
Imports System.Threading
Imports System.Web.Services
Imports System.Web.Script.Serialization

Partial Class Maintenance_itemsinrom
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then
            refresh_btn_top_Click(Nothing, e)
        End If

    End Sub

    ''' <summary>
    ''' Integer parameters are in months
    ''' </summary>
    Public Function Schedules_Create(totalMonths As Integer, taskInterval As Integer, startDate As DateTime) As List(Of DateTime)
        Dim life_span = 0
        Dim months_between = 0
        Dim months_increment = 0
        Dim task_interval = 0
        Dim start_date As DateTime = DateTime.MinValue
        Dim end_date As DateTime = DateTime.MinValue
        Dim list_dates As New List(Of DateTime)
        task_interval = taskInterval
        life_span = totalMonths
        start_date = startDate
        end_date = start_date.AddMonths(life_span)
        months_between = DateDiff(DateInterval.Month, start_date, end_date)
        months_increment = months_between / task_interval
        For i As Integer = 1 To months_increment
            Dim next_date As DateTime = start_date.AddMonths(task_interval * i)
            list_dates.Add(next_date)
        Next
        Return list_dates
    End Function



    Protected Sub refresh_btn_top_Click(sender As Object, e As System.EventArgs) Handles refresh_btn_top.Click
        Dim pm As New clsPreventiveMaintenance()
        Dim sb = New StringBuilder()
        Dim ds As DataSet = pm.TableSet
        litPLM.Text = String.Empty

        sb.AppendFormat("<table id=table-parent border=0> ")
        sb.AppendFormat("<thead>")
        sb.AppendFormat("<tr><th colspan=4><a href=# id=add-item2trackid-0 style='color:white;font:bold 1.2em George, serif;'>ADD ITEM</a></th><th colspan=2>") '<a href=# id=href-setup-buildings>SETUP BUILDINGS</a></th><th colspan=2><a href=# id=href-schedules>SCHEDULES</a></th></tr>")
        sb.AppendFormat("<tr><th colspan=2>Action</th><th>Item Name</th><th>Description</th><th>Life Time</th><th>Category</th></tr>")
        sb.AppendFormat("</thead><tbody>")

        For Each dr As DataRow In pm.List(tb_search.Text.Trim()).AsEnumerable().OrderBy(Function(x) x("categoryid").ToString())
            sb.AppendFormat("<tr class=row-top>")

            Dim c_id As Int32 = dr("categoryid").ToString()
            Dim p_id As Int32 = dr("item2trackid").ToString()
            Dim category = pm.Categories.Rows.OfType(Of DataRow).SingleOrDefault(Function(x) x("comboitemid").ToString() = c_id)

            sb.AppendFormat("<td><a href=# id=remove-item2trackid-{0} style='color:white;font:bold 1.2em George, serif;' >Remove</a></td>", p_id)
            sb.AppendFormat("<td><a href=# id=edit-item2trackid-{0} style='color:white;font:bold 1.2em George, serif;'  >Edit</a></td>", p_id)
            sb.AppendFormat("<td>{0}</td>", dr("name").ToString().Trim())
            sb.AppendFormat("<td>{0}&nbsp;</td>", dr("description").ToString().Trim())
            sb.AppendFormat("<td>{0}</td>", dr("life").ToString().Trim())

            If category Is Nothing Then
                sb.AppendFormat("<td/>")
            Else
                sb.AppendFormat("<td>{0}</td>", category("comboitem").ToString())
            End If

            sb.AppendFormat("</tr>")

            sb.AppendFormat("<tr><td/><td colspan=5><div item2trackid={0} section-type='unit' class='section-label-1'>ROOM - {1}</div></td></tr>", p_id, New clsPreventiveMaintenance.Item2TrackItems().Rooms(p_id).Count)
            sb.AppendFormat("<tr><td/><td colspan=5><div item2trackid={0} section-type='building' class='section-label-1'>BUILDINGS - {1}</div></td></tr>", p_id, New clsPreventiveMaintenance.Item2TrackItems().Buildings(p_id).Count)
            sb.AppendFormat("<tr><td/><td colspan=5><div item2trackid={0} section-type='task' class='section-label-1'>TASKS - {1}</div></td></tr>", p_id, New clsPreventiveMaintenance.Item2TrackTasks().List(p_id).Where(Function(x) Convert.ToBoolean(x("active").ToString() = True)).Count)
        Next
        sb.AppendFormat("</tbody><tfoot>")
        sb.AppendFormat("<tr><td colspan=6></td></tr>")
        sb.AppendFormat("</tfoot>")
        sb.AppendFormat("</table>")
        litPLM.Text = sb.ToString()
    End Sub



    Private Structure Room
        Public ITEM2TRACKID As Int32
        Public PMITEMID As Int32
        Public KEYVALUE As Int32
        Public KEYFIELD As String
        Public DESCRIPTION As String
        Public DATEADDED As String
        Public DATEREMOVED As String
        Public ROOMNUMBER As String
    End Structure



    <WebMethod()> _
    Public Shared Shadows Function LoadUnit(item2trackid As Int32) As String
        Dim items = New clsPreventiveMaintenance.Item2TrackItems()
        Dim rooms = New clsPreventiveMaintenance.RoomItems()

        Dim sb = New StringBuilder()
        
        sb.AppendFormat("<table id=table-unit cellspacing=0 cellpadding=0 border-collapse:collapse>")
        sb.AppendFormat("<thead>")
        sb.AppendFormat("<tr><th colspan=2>Actions</th><th>Name</th><th>Date Added</th><th>Date Removed</th><th>Description</th></tr>")
        sb.AppendFormat("</thead>")
        sb.AppendFormat("<tbody>")

        Dim l As New List(Of Room)

        For Each itemRow As DataRow In items.Rooms(item2trackid).ToArray()

            Dim dateRemoved As String = itemRow("DateRemoved").ToString()
            If dateRemoved.Length > 0 Then dateRemoved = Convert.ToDateTime(dateRemoved).ToShortDateString()

            l.Add(New Room With { _
                .PMITEMID = itemRow("pmItemID").ToString(), _
                .ROOMNUMBER = rooms.RoomNumber(itemRow("keyvalue").ToString()), _
                .KEYVALUE = itemRow("keyvalue").ToString(), _
                .KEYFIELD = "roomid", _
                .DESCRIPTION = itemRow("Description").ToString().Trim(), _
                .DATEADDED = Convert.ToDateTime(itemRow("DateAdded").ToString()).ToShortDateString(), _
                .DATEREMOVED = IIf(dateRemoved.Length > 0, dateRemoved, ""), _
                .ITEM2TRACKID = item2trackid})



            sb.AppendFormat("<tr>")
            sb.AppendFormat("<td colspan=2><a href=# id=remove-ass-{0}~{1} >Remove</a></td>", itemRow("pmItemID").ToString(), item2trackid)
            sb.AppendFormat("<td>{0}</td>", rooms.RoomNumber(itemRow("keyvalue").ToString()))
            sb.AppendFormat("<td align=right>{0}</td>", Convert.ToDateTime(itemRow("DateAdded").ToString()).ToShortDateString())



            sb.AppendFormat("<td align=right>{0}</td>", dateRemoved)
            sb.AppendFormat("<td>{0}</td>", itemRow("Description").ToString())
            sb.AppendFormat("</tr>")
        Next

        Dim js = New JavaScriptSerializer()
        js.MaxJsonLength = Int32.MaxValue



        sb.AppendFormat("</tbody>")
        sb.AppendFormat("<tfoot>")        
        sb.AppendFormat("<tr><td colspan=6><a href=# id=add-roomid-{0}~{1} >Add</a></td></tr>", 0, item2trackid)
        sb.AppendFormat("</tfoot>")
        sb.AppendFormat("</table>")
        Return js.Serialize(l)
    End Function

    <WebMethod()> _
    Public Shared Shadows Function LoadTask(item2trackid As Int32) As String
        Dim sb = New StringBuilder()
        Dim tasks = New clsPreventiveMaintenance.Item2TrackTasks()

        sb.AppendFormat("<table id=table-task cellspacing=0 cellpadding=0 border-collapse:collapse>")
        sb.AppendFormat("<thead>")
        sb.AppendFormat("<tr><th colspan=2>Actions</th><th>Name</th><th>Description</th><th>Category</th><th>Issue</th><th>Interval (Months)</th></tr>")
        sb.AppendFormat("<tbody>")

        For Each taskRow As DataRow In tasks.List(item2trackid).Where(Function(x) Convert.ToBoolean(x("active").ToString()) = True).ToArray()

            sb.AppendFormat("<tr>")
            sb.AppendFormat("<td><a href=# id=remove-taskid-{0}~{1}>Remove</a></td>", taskRow("TaskID").ToString(), item2trackid)
            sb.AppendFormat("<td><a href=# id=edit-taskid-{0}~{1}>Edit</a></td>", taskRow("TaskID").ToString(), item2trackid)

            sb.AppendFormat("<td id=task-{1}>{0}</td>", taskRow("Name").ToString(), taskRow("TaskID").ToString())
            sb.AppendFormat("<td>{0}</td>", taskRow("Description").ToString())
            sb.AppendFormat("<td>{0}</td>", taskRow("Category").ToString())
            sb.AppendFormat("<td>{0}</td>", tasks.Name(taskRow("issueid").ToString()))
            sb.AppendFormat("<td align=right>{0}</td>", taskRow("Interval").ToString())
            sb.AppendFormat("</tr>")
        Next

        sb.AppendFormat("</tbody>")
        sb.AppendFormat("<tfoot>")
        sb.AppendFormat("<tr><td colspan=7>&nbsp;</td></tr>")
        sb.AppendFormat("<tr><td colspan=7><a href=# id=add-taskid-{0}~{1} >Add</a></td></tr>", 0, item2trackid)
        sb.AppendFormat("</tfoot>")
        sb.AppendFormat("</table>")

        Return sb.ToString()
    End Function

    <WebMethod()> _
    Public Shared Shadows Function LoadBuildings(item2trackid As Int32) As String
        Dim sb = New StringBuilder()
        Dim items = New clsPreventiveMaintenance.Item2TrackItems()
        Dim buildings = New clsPreventiveMaintenance.BuildingItems()

        sb.AppendFormat("<table id=table-building cellspacing=0 cellpadding=0 border-collapse:collapse>")
        sb.AppendFormat("<thead>")
        sb.AppendFormat("<tr><th colspan=2>Actions</th><th>Name</th><th>Date Added</th><th>Date Removed</th><th>Description</th></tr>")
        sb.AppendFormat("</thead>")
        sb.AppendFormat("<tbody>")

        For Each itemRow As DataRow In items.Buildings(item2trackid).ToArray()
            sb.AppendFormat("<tr>")
            sb.AppendFormat("<td><a href=# pmitemid={2} id=edit-buildingid-{0}~{1}>Edit</a></td>", itemRow("KeyValue").ToString, item2trackid, itemRow("pmitemid").ToString())
            sb.AppendFormat("<td><a href=# pmitemid={2} id=remove-buildingid-{0}~{1}>Remove</a></td>", itemRow("KeyValue").ToString, item2trackid, itemRow("pmitemid").ToString())

            sb.AppendFormat("<td>{0}</td>", buildings.Name(itemRow("KeyValue").ToString()))
            sb.AppendFormat("<td align=right>{0}</td>", IIf(itemRow("DateAdded").Equals(DBNull.Value), "", Convert.ToDateTime(itemRow("DateAdded").ToString()).ToShortDateString()))

            Dim dateRemoved As String = itemRow("DateRemoved").ToString
            If dateRemoved.Length > 0 Then dateRemoved = Convert.ToDateTime(dateRemoved).ToShortDateString()

            sb.AppendFormat("<td align=right>{0}</td>", dateRemoved)
            sb.AppendFormat("<td>{0}</td>", itemRow("Description").ToString().Trim())
            sb.AppendFormat("</tr>")
        Next

        sb.AppendFormat("</tbody>")
        sb.AppendFormat("<tfoot>")
        sb.AppendFormat("<tr><td colspan=6>&nbsp;</td></tr>")
        sb.AppendFormat("<tr><td colspan=6><a href=# id=add-buildingid-{0}~{1} pmitemid=0 >Add</a></td></tr>", 0, item2trackid)
        sb.AppendFormat("</tfoot>")
        sb.AppendFormat("</table>")

        Return sb.ToString()
    End Function

    Protected Sub lnkTabItem_Click(sender As Object, e As System.EventArgs) Handles lnkTabItem.Click
        MultiView1.SetActiveView(MultiView1_View1)
        gvTrackItems.DataSource = (New clsPreventiveMaintenance()).List_Items2Track()
        gvTrackItems.DataBind()

    End Sub

    Protected Sub lnkTabTasks_Click(sender As Object, e As System.EventArgs) Handles lnkTabTasks.Click
        MultiView1.SetActiveView(MultiView1_View2)
        gvTasks.DataSource = (New clsPreventiveMaintenance()).Get_Tasks_Table()
        gvTasks.DataBind()

        Dim ds As SqlDataSource = (New clsPreventiveMaintenance()).List(True)
        Dim dv As DataView = CType(ds.Select(DataSourceSelectArguments.Empty), DataView)
        Dim dt = dv.ToTable()

        MultiView1.SetActiveView(MultiView1_View4)
        gvTaskC.DataSource = dt
        gvTaskC.DataBind()


        Dim sb = New StringBuilder()
        sb.AppendFormat("<table BORDER=1><thead><tr><th>ROOM</th><th>BUILDING</th><th>ITEM</th><th>TASK</th></tr></thead>")
        sb.AppendFormat("<tbody>")
        For Each dr As DataRow In dt.Rows
            sb.AppendFormat("<tr>")
            sb.AppendFormat("<td>{0}</td>", dr("room").ToString())
            sb.AppendFormat("<td>{0}</td>", dr("building").ToString())
            sb.AppendFormat("<td>{0}</td>", dr("item-track-name").ToString())
            sb.AppendFormat("<td>{0}</td>", dr("task-name").ToString())
            sb.AppendFormat("</tr>")
        Next

        sb.AppendFormat("</tbody></table>")
        litTasks.Text = sb.ToString()
    End Sub

    Protected Sub gvTrackItems_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvTrackItems.SelectedIndexChanged

    End Sub

    Protected Sub gvTasks_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvTasks.SelectedIndexChanged

    End Sub

    Protected Sub gvTaskC_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvTaskC.SelectedIndexChanged

    End Sub

    Protected Sub gvTaskC_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTaskC.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("composite-keys", String.Format("{0}-{1}", e.Row.Cells(0).Text, e.Row.Cells(1).Text))
        End If
    End Sub
End Class
