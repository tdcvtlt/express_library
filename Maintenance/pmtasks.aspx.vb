Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Reflection


Partial Class Maintenance_pmtasks
    Inherits System.Web.UI.Page

    Private pmItemID As Int32 = 0
    Private rooms As List(Of DataRow)
    Private buildings As List(Of DataRow)

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim pm As New clsPreventiveMaintenance()
        rooms = pm.Rooms.AsEnumerable().ToList()
        buildings = pm.Buildings.AsEnumerable().ToList()

        If IsPostBack = False Then

            Dim view_index() As String = {"mvItem2Track", "mvTask", "mvPMItem2Track", "vwItemEditDelete", "vwItemAdd", "vwPMItemChangeRemove"}

            Dim pmitem2trackid As String = Request.QueryString("pmitem2trackid")
            Dim action As String = Request.QueryString("action")

            mv.SetActiveView(mv.Views(Array.IndexOf(view_index, Request.QueryString("mvView"))))

            With item_category
                .Items.Clear()
                .AppendDataBoundItems = True
                .Items.Add(New ListItem("", "-2"))
                .DataSource = pm.Categories
                .DataValueField = "comboitemid"
                .DataTextField = "comboitem"
                .DataBind()
            End With

            With task_interval
                .Items.Clear()               
                .Items.AddRange(Enumerable.Range(0, 61).Select(Function(x) New ListItem With {.Text = x, .Value = x}).ToArray())
                .DataBind()
            End With

            With task_category
                .Items.Clear()
                .Items.AddRange(pm.Departments.Select(Function(x) New ListItem With {.Text = x, .Value = x}).ToArray())
                .DataBind()
            End With

            With task_issue
                .Items.Clear()
                .DataSource = pm.Issues
                .DataValueField = "comboitemid"
                .DataTextField = "comboitem"
                .DataBind()
            End With

            Dim oPMTeam As New clsPMTeams
            ddTeam.DataSource = oPMTeam.List_Teams("")
            ddTeam.Items.Add(New ListItem("", 0))
            ddTeam.AppendDataBoundItems = True
            ddTeam.DataValueField = "PMTeamID"
            ddTeam.DataTextField = "Name"
            ddTeam.DataBind()
            oPMTeam = Nothing

            If mv.GetActiveView() Is mvItem2Track Then
                item_label.Text = "PM ITEM - " & action.ToUpper()
                With pm.Item2Track
                    .item2trackid = Request.QueryString("pmitem2trackid")
                    .Load()
                    item_name.Text = .name
                    item_description.Text = .description
                    item_category.SelectedValue = .categoryid
                    item_life.Text = .life
                    item_great.Text = .gpPart
                End With

            ElseIf mv.GetActiveView() Is mvTask Then

                task_label.Text = "TASK - " & action.ToUpper()                
                With pm.Task
                    .TaskID = Request.QueryString("taskid")
                    .Load()
                    task_name.Text = .Name
                    task_description.Text = .Description
                    task_interval.Text = .Interval
                    task_category.Text = .Category
                    task_issue.SelectedValue = .IssueID
                End With

            ElseIf mv.GetActiveView() Is mvPMItem2Track Then
                gvPMItem2Track.DataSource = New clsPreventiveMaintenance().List()
                gvPMItem2Track.DataBind()

            ElseIf mv.GetActiveView() Is vwItemAdd Then
                Dim [type] = Request("type")
                gvwItemAddList.DataKeyNames = New String() {[type]}
                gvwItemAddList.Attributes.Add("item2trackid", Request.QueryString("item2trackid"))
                If [type] = "pmbuildingid" Then
                    gvwItemAddList.Attributes.Add("type", [type])
                    gvwItemAddList.DataSource = pm.Buildings
                ElseIf [type] = "roomid" Then
                    gvwItemAddList.Attributes.Add("type", [type])
                    gvwItemAddList.DataSource = pm.Rooms
                End If
                gvwItemAddList.DataBind()
            ElseIf mv.GetActiveView() Is vwItemEditDelete Then
                Dim pm_id = Request("pmitemid")
                With New clsPMItems
                    .PMItemID = pm_id
                    .Load()
                    If pm_id > 0 Then
                        If .KeyField.ToLower() = "pmbuildingid" Then
                            tbItemName.Text = pm.Buildings.AsEnumerable().Single(Function(x) x("pmbuildingid").ToString() = .KeyValue)("name").ToString().ToUpper()
                        ElseIf .KeyField.ToLower() = "roomid" Then
                            tbItemName.Text = pm.Rooms.AsEnumerable().Single(Function(x) x("roomid").ToString() = .KeyValue)("roomnumber").ToString().ToUpper()
                        End If
                    End If
                    tbItemDescription.Text = .Description
                    dfdItemDateAdded.Selected_Date = .DateAdded
                    If .DateRemoved.Length > 0 Then
                        dfdItemDateRemoved.Selected_Date = .DateRemoved
                        btnItemSubmit.Enabled = False
                    End If
                End With
                btnItemSubmit.Attributes.Add("pmitemid", pm_id)

            ElseIf mv.GetActiveView() Is vwPMItemChangeRemove Then
                With New clsPMItems()
                    .PMItemID = Request.QueryString("pmitemid")
                    .Load()
                    PMItem_Description.Text = .Description.Trim()
                    PMItem_df_DateAdded.Selected_Date = .DateAdded
                    PMItem_df_DateRemoved.Selected_Date = .DateRemoved
                    If .DateRemoved.Length > 0 Then
                        PMItem_Submit.Visible = False
                    End If
                    If Request.QueryString("action") = "Change" And .DateRemoved.Length = 0 Then
                        PMItem_df_DateRemoved.Visible = False
                    End If
                End With
            End If
        End If
    End Sub

    Protected Sub item_submit_Click(sender As Object, e As System.EventArgs) Handles item_submit.Click
        Dim item As New clsPMItems2Track()
        With item
            .item2trackid = Request.QueryString("pmitem2trackid")
            .Load()

            Dim prev_life = .life

            .name = item_name.Text.Trim()
            .description = item_description.Text.Trim()
            .life = item_life.Text.Trim()
            .categoryid = item_category.SelectedValue            
            .gpPart = item_great.Text.Trim()
            .Save()

            Dim schedule = New clsPreventiveMaintenance.Schedule()
            If .life <> prev_life And prev_life > 0 Then
                schedule.UpdateItem2Track(.item2trackid, -1)
            End If
        End With
    
        'ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), DateTime.Now.ToLongTimeString(), "$(window.opener.document.getElementById('ctl00_ContentPlaceHolder1_refresh_btn_top')).trigger('click');window.close();", True)
        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), DateTime.Now.ToLongTimeString(), "window.opener.navigate();window.close();", True)
    End Sub

    Protected Sub task_submit_Click(sender As Object, e As System.EventArgs) Handles task_submit.Click
        Dim task As New clsPMTasks()
        With task
            .Item2TrackID = Request.QueryString("pmitem2trackid")
            .Load()

            Dim prev_interval = .Interval

            .TaskID = Request.QueryString("taskid")
            .Name = task_name.Text.Trim()
            .Description = task_description.Text.Trim()
            .Interval = task_interval.SelectedItem.Text.Trim()
            .Category = task_category.SelectedItem.Text.Trim()
            .IssueID = task_issue.SelectedItem.Value
            .Active = IIf(Request.QueryString("action") = "add" Or Request.QueryString("action") = "edit", True, IIf(Request.QueryString("action") = "remove", False, True))
            .TeamID = ddTeam.SelectedValue
            .Save()

            Dim schedule = New clsPreventiveMaintenance.Schedule()
            If .Active = False Then
                schedule.RemoveTask(.TaskID)
            ElseIf Convert.ToInt16(Request.QueryString("taskid")) = 0 Then
                schedule.CreateTask(.Item2TrackID, -1)
            ElseIf .Interval <> prev_interval Then
                schedule.UpdateTask(.TaskID, .Item2TrackID)
            End If
        End With
        '        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), DateTime.Now.ToLongTimeString(), "$(window.opener.document.getElementById('ctl00_ContentPlaceHolder1_refresh_btn_top')).trigger('click');window.close();", True)
        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), DateTime.Now.ToLongTimeString(), "window.opener.navigate();window.close();", True)

    End Sub

    Protected Sub OnClickHandler(ByVal sender As Object, ByVal e As EventArgs)
        Dim lnk As LinkButton = CType(sender, LinkButton)
        Dim gvr As GridViewRow = lnk.Parent.Parent
        Dim gv As GridView = CType(gvr.NamingContainer, GridView)
        Dim t As TextBox = CType(gvr.FindControl("extraText"), TextBox)

        Select Case lnk.CommandName
            Case "select"

                If t.Text.Length = 0 Then
                    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), DateTime.Now.ToLongTimeString(), "alert('Please provide location for this item.');", True)
                    Return
                End If
                With New clsPMItems
                    .PMItemID = Request.QueryString("pmitemid")
                    .Load()
                    .KeyValue = lnk.CommandArgument
                    .KeyField = "pmbuildingid"
                    .Item2TrackID = Request.QueryString("pmitem2trackid")
                    .DateAdded = DateTime.Now.ToShortDateString()
                    .Description = t.Text.Trim()
                    .Save()

                    Dim schedule = New clsPreventiveMaintenance.Schedule()
                    schedule.CreateItem2Track(Request.QueryString("pmitem2trackid"), .PMItemID)
                End With
                'ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), DateTime.Now.ToLongTimeString(), "$(window.opener.document.getElementById('ctl00_ContentPlaceHolder1_refresh_btn_top')).trigger('click');window.close();", True)
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), DateTime.Now.ToLongTimeString(), "window.opener.navigate();window.close();", True)

        End Select
    End Sub

    Protected Sub OnButtonLinkClick(ByVal sender As Object, ByVal e As EventArgs)
        Dim lnk As LinkButton = CType(sender, LinkButton)
        Dim gvr As GridViewRow = lnk.Parent.Parent
        Dim gv As GridView = CType(gvr.NamingContainer, GridView)
        Dim t As TextBox = CType(gvr.FindControl("extraText"), TextBox)

        Select Case lnk.CommandName
            Case "select"
                If t.Text.Length = 0 Then
                    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), DateTime.Now.ToLongTimeString(), "alert('Please provide location for this item.');", True)
                    Return
                Else
                    With New clsPMItems
                        .PMItemID = 0
                        .Load()
                        .KeyValue = Request.QueryString("KeyValue")
                        .KeyField = "RoomID"
                        .Item2TrackID = lnk.CommandArgument
                        .DateAdded = DateTime.Now.ToShortDateString()
                        .Description = t.Text.Trim()
                        .Save()

                        Dim schedule = New clsPreventiveMaintenance.Schedule()
                        schedule.CreateItem2Track(lnk.CommandArgument, .PMItemID)
                        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), DateTime.Now.ToLongTimeString(), "window.opener.navigate();window.close();", True)
                        'ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), DateTime.Now.ToLongTimeString(), "$(window.opener.document.getElementById('ctl00_ContentPlaceHolder1_refresh_btn_top')).trigger('click');window.close();", True)
                    End With
                End If
        End Select
    End Sub

    Protected Sub btnItemSubmit_Click(sender As Object, e As System.EventArgs) Handles btnItemSubmit.Click
        Dim pm_id = btnItemSubmit.Attributes("pmitemid")
        With New clsPMItems
            .PMItemID = pm_id
            .Load()
            .Description = tbItemDescription.Text.Trim()
            If dfdItemDateRemoved.Selected_Date.Length > 0 Then .DateRemoved = dfdItemDateRemoved.Selected_Date
            .Save()
            If dfdItemDateRemoved.Selected_Date.Length > 0 Then
                With New clsPreventiveMaintenance.Schedule()                    
                    .Schedule_Remove(pm_id)
                End With
            End If
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), DateTime.Now.ToLongTimeString(), "window.opener.navigate();window.close();", True)

            'ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), DateTime.Now.ToLongTimeString(), _
            ' "$(window.opener.document.getElementById('ctl00_ContentPlaceHolder1_refresh_btn_top'))" & _
            '".trigger('click');window.close();", True)
        End With
    End Sub

    Protected Sub btItemAddSubmit_Click(sender As Object, e As System.EventArgs) Handles btItemAddSubmit.Click
        If tbItemAddDescription.Text.Length = 0 Then
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), DateTime.Now.ToLongTimeString(), "alert('Enter description for selection(s).');", True)
        ElseIf dfItemAddDateAdd.Selected_Date.Length = 0 Then
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), DateTime.Now.ToLongTimeString(), "alert('Select Date Add for selection(s).');", True)
        Else
            For Each gvr As GridViewRow In gvwItemAddList.Rows
                If CType(gvr.FindControl("cbItemAddList"), CheckBox).Checked Then
                    With New clsPMItems
                        .PMItemID = 0
                        .Load()
                        .KeyValue = gvwItemAddList.DataKeys(gvr.RowIndex).Value
                        .KeyField = gvwItemAddList.Attributes("type")
                        .Item2TrackID = gvwItemAddList.Attributes("item2trackid")
                        .DateAdded = dfItemAddDateAdd.Selected_Date
                        .Description = tbItemAddDescription.Text.Trim()
                        .Save()

                        Dim schedule = New clsPreventiveMaintenance.Schedule()
                        schedule.CreateItem2Track(.Item2TrackID, .PMItemID)
                    End With
                End If
            Next
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), DateTime.Now.ToLongTimeString(), "window.opener.navigate();window.close();", True)
            'ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), DateTime.Now.ToLongTimeString(), _
            ' "$(window.opener.document.getElementById('ctl00_ContentPlaceHolder1_refresh_btn_top')).trigger('click');window.close();", True)
        End If
    End Sub

    Protected Sub gvwItemAddList_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwItemAddList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If gvwItemAddList.Attributes("type") = "roomid" Then
                e.Row.Cells(1).Text = rooms.First(Function(x) x("roomid").ToString() = gvwItemAddList.DataKeys(e.Row.RowIndex).Value)("roomnumber").ToString()
                e.Row.Cells(2).Text = rooms.First(Function(x) x("roomid").ToString() = gvwItemAddList.DataKeys(e.Row.RowIndex).Value)("phone").ToString()
            Else
                e.Row.Cells(1).Text = buildings.First(Function(x) x("pmbuildingid").ToString() = gvwItemAddList.DataKeys(e.Row.RowIndex).Value)("name").ToString()
                e.Row.Cells(2).Text = buildings.First(Function(x) x("pmbuildingid").ToString() = gvwItemAddList.DataKeys(e.Row.RowIndex).Value)("description").ToString()
            End If
        End If
    End Sub

    
    
    Protected Sub mulItemSubmit_Click(sender As Object, e As System.EventArgs) Handles mulItemSubmit.Click
        Dim counter = 0
        For Each row As GridViewRow In gvPMItem2Track.Rows
            Dim t As TextBox = CType(row.FindControl("extraText"), TextBox)
            Dim df = row.FindControl("df_ItemDateAdded")
            Dim cb As CheckBox = CType(row.FindControl("cbItemAddList"), CheckBox)
            Dim mi_type As Type = df.GetType()
            Dim mi_prop_info As PropertyInfo = mi_type.GetProperty("Selected_Date")
            If cb.Checked Then
                If t.Text.Length = 0 Or mi_prop_info.GetValue(df, Nothing).Length = 0 Then
                    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), DateTime.Now.ToLongTimeString(), "alert('Please ensure added date and extra text are filled in for the choices selected.');", True)
                    Return
                Else
                    counter += 1
                End If
            End If
        Next

        If counter = 0 Then
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), DateTime.Now.ToLongTimeString(), "window.opener.navigate();window.close();", True)
            'ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), DateTime.Now.ToLongTimeString(), "$(window.opener.document.getElementById('ctl00_ContentPlaceHolder1_refresh_btn_top')).trigger('click');window.close();", True)
            Return
        End If

        For Each row As GridViewRow In gvPMItem2Track.Rows
            Dim t As TextBox = CType(row.FindControl("extraText"), TextBox)
            Dim df = row.FindControl("df_ItemDateAdded")
            Dim cb As CheckBox = CType(row.FindControl("cbItemAddList"), CheckBox)
            Dim mi_type As Type = df.GetType()
            Dim mi_prop_info As PropertyInfo = mi_type.GetProperty("Selected_Date")
            If cb.Checked Then
                With New clsPMItems
                    .PMItemID = 0
                    .Load()
                    .KeyValue = Request.QueryString("KeyValue")
                    .KeyField = "RoomID"
                    .Item2TrackID = gvPMItem2Track.DataKeys(row.RowIndex).Value
                    .DateAdded = mi_prop_info.GetValue(df, Nothing).ToString()
                    .Description = t.Text.Trim()
                    .Save()

                    Dim schedule = New clsPreventiveMaintenance.Schedule()
                    schedule.CreateItem2Track(.Item2TrackID, .PMItemID)
                End With
            End If
        Next
        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), DateTime.Now.ToLongTimeString(), "window.opener.navigate();window.close();", True)
        'ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), DateTime.Now.ToLongTimeString(), "$(window.opener.document.getElementById('ctl00_ContentPlaceHolder1_refresh_btn_top')).trigger('click');window.close();", True)
    End Sub

    
    Protected Sub PMItem_Submit_Click(sender As Object, e As System.EventArgs) Handles PMItem_Submit.Click
        With New clsPMItems()
            .PMItemID = Request.QueryString("pmitemid")
            .Load()
            .Description = PMItem_Description.Text.Trim()
            Dim schedule = New clsPreventiveMaintenance.Schedule()

            If Request.QueryString("action") = "Remove" And PMItem_df_DateRemoved.Selected_Date.Length > 0 Then
                .DateRemoved = PMItem_df_DateRemoved.Selected_Date
                .Save()
                schedule.Schedule_Remove(.PMItemID)
            ElseIf Convert.ToDateTime(PMItem_df_DateAdded.Selected_Date) <> Convert.ToDateTime(.DateAdded) Then
                .DateAdded = PMItem_df_DateAdded.Selected_Date
                .Save()
                schedule.UpdateItem2Track(.Item2TrackID, .PMItemID)
            End If
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), DateTime.Now.ToLongTimeString(), "window.opener.navigate();window.close();", True)
            'ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), DateTime.Now.ToLongTimeString(), "$(window.opener.document.getElementById('ctl00_ContentPlaceHolder1_refresh_btn_top')).trigger('click');window.close();", True)
        End With
    End Sub
End Class
