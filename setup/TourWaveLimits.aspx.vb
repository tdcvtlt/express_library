Imports System.Web.Services
Imports System.Data.SqlClient
Imports System.Data

Partial Class setup_TourWaveLimits
    Inherits System.Web.UI.Page

    Dim tourWaveLimits As clsTourWaveLimits

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        tourWaveLimits = New clsTourWaveLimits()

        If IsPostBack = False Then

            Dim wds = New List(Of ListItem)            
            wds.Add(New ListItem With {.Text = "Monday"})
            wds.Add(New ListItem With {.Text = "Tuesday"})
            wds.Add(New ListItem With {.Text = "Wednesday"})
            wds.Add(New ListItem With {.Text = "Thursday"})
            wds.Add(New ListItem With {.Text = "Friday"})
            wds.Add(New ListItem With {.Text = "Saturday"})
            wds.Add(New ListItem With {.Text = "Sunday"})
            cbl_weekdays.Items.AddRange(wds.ToArray())


            MultiView1.SetActiveView(View1)

            Dim dt = tourWaveLimits.GetCampaignTypes

            DropDownList5.DataSource = dt
            DropDownList5.DataTextField = "campaign_type_name"
            DropDownList5.DataValueField = "ID"

            campaign_types_checkboxlist.DataSource = dt
            campaign_types_checkboxlist.DataTextField = "campaign_type_name"
            campaign_types_checkboxlist.DataValueField = "ID"

            campaign_types_checkboxlist.DataBind()


            dt = tourWaveLimits.GetTourWaves

            tour_wave_checkboxlist.DataSource = dt
            tour_wave_checkboxlist.DataTextField = "Description"
            tour_wave_checkboxlist.DataValueField = "ComboItemID"
            tour_wave_checkboxlist.DataBind()


            DropDownList3.DataSource = Enumerable.Range(0, 31).Concat(New Integer() {60, 90}).ToArray()


            dt = tourWaveLimits.GetLocations()

            tour_locations_checkboxlist.DataSource = dt
            tour_locations_checkboxlist.DataTextField = "ComboItem"
            tour_locations_checkboxlist.DataValueField = "ComboItemID"
            tour_locations_checkboxlist.DataBind()

            tour_locations_radiobuttonlist.DataSource = dt
            tour_locations_radiobuttonlist.DataTextField = "ComboItem"
            tour_locations_radiobuttonlist.DataValueField = "ComboItemID"
            tour_locations_radiobuttonlist.DataBind()

            DropDownList4.DataSource = New String() {"Last 30 days", "Last 60 days", "Last 90 days"}

            DropDownList3.DataBind()
            DropDownList4.DataBind()
            DropDownList5.DataBind()

            cbl_wd.DataSource = New String() {"Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"}
            cbl_wd.DataBind()

            lnk_View1_Click(Nothing, EventArgs.Empty)

        End If

        tour_locations_checkboxlist.Enabled = True
        tour_wave_checkboxlist.Enabled = True
        campaign_types_checkboxlist.Enabled = True
        cbl_wd.Enabled = True


        Dim sb = New StringBuilder()
        Dim c = 0I

        sb.AppendLine("$(function(){")

        sb.AppendLine("var campaigns = new Array();")

        sb.AppendLine()


        For Each li As ListItem In campaign_types_checkboxlist.Items.OfType(Of ListItem)()

            Dim ct = tourWaveLimits.GetCampaignNames(li.Value)
            Dim rows = ct.Rows.OfType(Of DataRow).Select(Function(x) String.Format("'{0}'", x.Field(Of String)(0))).ToArray()

            sb.AppendLine()
            sb.AppendLine(String.Format("campaigns[{0}] = new Array({1});", c, String.Join(",", rows)))
            c += 1
        Next

        sb.AppendLine()
        sb.AppendLine()
        sb.AppendLine()


        sb.AppendLine("$('#ctl00_ContentPlaceHolder1_DropDownList5').change(function () { " & _
        "var html = '';  var selectedInd = $(this).get(0).selectedIndex;$('#toast').html('');" & _
        "$.each(campaigns, function (x, y) { " & _
        "   if (x == selectedInd) { " & _
        "       $.each(campaigns[x], function (v, c) {" & _
        "           html += c + ' ';" & _
        "       })" & _
        "   }" & _
        "});" & _
        "$('#toast').html(html);" & _
        "});")



        sb.AppendLine("})")

        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), DateTime.Now.ToLongTimeString(), sb.ToString(), True)


    End Sub


    Protected Sub lnk_View1_Click(sender As Object, e As System.EventArgs) Handles lnk_View1.Click

        GridView1.DataSource = Nothing
        GridView1.DataBind()

        Dim c = New DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
        Dim dt = New DataTable()

        Dim li = tour_locations_radiobuttonlist.Items.OfType(Of ListItem).Where(Function(x) x.Selected = True).FirstOrDefault()
        Dim id = 0D

        If Not li Is Nothing Then id = li.Value

        If cb_historial.Checked Then

            If DropDownList4.SelectedIndex = 0 Then
                c = c.AddDays(-30)
            ElseIf DropDownList4.SelectedIndex = 1 Then
                c = c.AddDays(-60)
            ElseIf DropDownList4.SelectedIndex = 2 Then
                c = c.AddDays(-90)
            End If


            dt = tourWaveLimits.GetViewLimits(c, id)

        ElseIf String.IsNullOrEmpty(dfNext.Selected_Date) Then

            dt = tourWaveLimits.GetViewLimits(DropDownList5.SelectedItem.Value, c, id)

        ElseIf String.IsNullOrEmpty(dfNext.Selected_Date) = False Then

            c = New DateTime(DateTime.Parse(dfNext.Selected_Date).Year, _
                             DateTime.Parse(dfNext.Selected_Date).Month, _
                             DateTime.Parse(dfNext.Selected_Date).Day)

            dt = tourWaveLimits.GetViewLimits(DropDownList5.SelectedItem.Value, c, id)

        End If


        GridView1.DataSource = dt
        GridView1.DataBind()

        MultiView1.SetActiveView(View1)
    End Sub

    Protected Sub lnk_View2_Click(sender As Object, e As System.EventArgs) Handles lnk_View2.Click

        If CheckSecurity("WaveLimits", "Add", , , Session("UserDBID")) = False Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "pop", "alert('You do not have permission to perform this function.');", True)
            Return
        End If

        ControlClear()
        MultiView1.SetActiveView(View2)
        ControlClear()
    End Sub


    Protected Sub b_save_Click(sender As Object, e As System.EventArgs) Handles b_save.Click

        Save(IIf(Integer.Parse(TourWaveLimitsID.Value) > 0, TourWaveLimitsID.Value, 0))
        TourWaveLimitsID.Value = -1

        lnk_View1_Click(Nothing, EventArgs.Empty)
    End Sub

    Protected Sub b_cancel_Click(sender As Object, e As System.EventArgs) Handles b_cancel.Click
        ControlClear()
        lnk_View1_Click(Nothing, EventArgs.Empty)
    End Sub

    Protected Sub b_refresh_Click(sender As Object, e As System.EventArgs) Handles b_refresh.Click
        lnk_View1_Click(Nothing, EventArgs.Empty)
    End Sub

    Protected Sub b_refresh_1_Click(sender As Object, e As System.EventArgs) Handles b_refresh_1.Click
        lnk_View1_Click(Nothing, EventArgs.Empty)
    End Sub


    Protected Sub GridView1_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand

        If e.CommandName.Equals("EditCurrentRow") Then

            lnk_View2_Click(Nothing, EventArgs.Empty)
            ControlBind(e.CommandArgument.ToString())
        End If
    End Sub

    Protected Sub GridView1_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then

            If cb_historial.Checked Then

                Dim dt = DateTime.Parse(e.Row.Cells(5).Text)
                Dim ts = New DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day) - New DateTime(dt.Year, dt.Month, dt.Day)

                If ts.Days > 0 Then

                    If ts.Days >= 0 And ts.Days <= 30 Then
                        e.Row.BackColor = System.Drawing.Color.FromName("Moccasin")
                    ElseIf ts.Days >= 31 And ts.Days <= 60 Then
                        e.Row.BackColor = System.Drawing.Color.FromName("Peru")
                    ElseIf ts.Days >= 60 And ts.Days <= 90 Then
                        e.Row.BackColor = System.Drawing.Color.FromName("Crimson")

                    End If
                End If
            End If
        End If
    End Sub

    Private Sub ControlBind(pkKey As Integer)

        With tourWaveLimits
            .TourWaveLimitsID = pkKey
            .Load()

            DropDownList3.SelectedValue = .MaxLimit

            dfTransDate.Selected_Date = DateTime.Parse(.EffectiveStartDate).ToString("d")
            dfEndDate.Selected_Date = DateTime.Parse(.EffectiveEndDate).ToString("d")

            TourWaveLimitsID.Value = pkKey


            tour_locations_checkboxlist.Enabled = False
            tour_wave_checkboxlist.Enabled = False
            campaign_types_checkboxlist.Enabled = False
            cbl_wd.Enabled = False


        End With

    End Sub

    Private Sub ControlClear()

        DropDownList3.SelectedIndex = 0
        dfTransDate.Selected_Date = ""
        dfEndDate.Selected_Date = ""
        TourWaveLimitsID.Value = -1

        For Each c As ListItem In tour_locations_checkboxlist.Items.OfType(Of ListItem)()
            c.Selected = False
        Next

        For Each c As ListItem In tour_wave_checkboxlist.Items.OfType(Of ListItem)()
            c.Selected = False
        Next

        For Each c As ListItem In tour_locations_radiobuttonlist.Items.OfType(Of ListItem)()
            'c.Selected = False
        Next

        For Each c As ListItem In cbl_wd.Items.OfType(Of ListItem)()
            c.Selected = False
        Next
        For Each c As ListItem In campaign_types_checkboxlist.Items.OfType(Of ListItem)()
            c.Selected = False
        Next
    End Sub

    Private ReadOnly Property StartDate As DateTime
        Get
            Return DateTime.Parse(dfTransDate.Selected_Date).ToString("d")
        End Get
    End Property

    Private ReadOnly Property EndDate As DateTime
        Get
            Return DateTime.Parse(dfEndDate.Selected_Date).ToString("d")
        End Get
    End Property

    Private ReadOnly Property Limit As Integer
        Get
            Return Integer.Parse(DropDownList3.SelectedItem.Text)
        End Get
    End Property


    Private ReadOnly Property DaySpan As Integer
        Get
            If String.IsNullOrEmpty(dfTransDate.Selected_Date) Or String.IsNullOrEmpty(dfEndDate.Selected_Date) Then Return -1

            Return (New DateTime(EndDate.Year, EndDate.Month, EndDate.Day) - _
                    New DateTime(StartDate.Year, StartDate.Month, StartDate.Day)).Days

        End Get
    End Property


    Private Function IsWeekDayToInclude(dt As DateTime) As Boolean
        dt = New DateTime(dt.Year, dt.Month, dt.Day)

        Dim c = cbl_wd.Items.OfType(Of ListItem).Where(Function(x) x.Selected = True)

        If c.Count() = 0 Then
            Return False
        Else

            Dim wd = dt.DayOfWeek
            Return IIf(c.Where(Function(x) x.Text.ToUpper() = wd.ToString().ToUpper()).Count() > 0, True, False)
        End If
    End Function

    Private Sub Save(TourWaveLimitsID As Integer)

        '
        'Editing
        If TourWaveLimitsID > 0 Then

            If CheckSecurity("WaveLimits", "Change", , , Session("UserDBID")) = False Then
                ClientScript.RegisterClientScriptBlock(Me.GetType, "pop", "alert('You do not have permission to save changes.');", True)
                Return
            Else

                Try
                    With tourWaveLimits
                        .TourWaveLimitsID = TourWaveLimitsID
                        .Load()

                        .MaxLimit = Limit
                        .Save()
                    End With
                Catch ex As Exception
                    Response.Write(String.Format("<strong>{0}</strong>", ex.Message))
                End Try
                
            End If

        Else
            If DaySpan >= 0 And Integer.Parse(DropDownList3.SelectedItem.Text) > 0 Then

                Dim sd As DateTime = StartDate

                For i As Integer = 0 To DaySpan

                    If IsWeekDayToInclude(sd) Then

                        For Each l As ListItem In tour_locations_checkboxlist.Items.OfType(Of ListItem).Where(Function(x) x.Selected = True)

                            For Each w As ListItem In tour_wave_checkboxlist.Items.OfType(Of ListItem).Where(Function(x) x.Selected = True)

                                For Each c As ListItem In campaign_types_checkboxlist.Items.OfType(Of ListItem).Where(Function(x) x.Selected = True)

                                    If tourWaveLimits.Exists(c.Value, w.Value, sd, sd, l.Value) = False Then

                                        Try

                                            With tourWaveLimits
                                                .TourWaveLimitsID = 0
                                                .Load()

                                                .CampaignTypeID = Integer.Parse(c.Value)
                                                .MaxLimit = Limit
                                                .WaveTimeID = Integer.Parse(w.Value)
                                                .EffectiveStartDate = sd
                                                .EffectiveEndDate = sd
                                                .TourLocationID = l.Value
                                                .Save()
                                            End With

                                        Catch ex As Exception
                                            Response.Write(String.Format("<strong>{0}</strong>", ex.Message))
                                        End Try
                                    End If
                                Next
                            Next
                        Next
                    End If
                    sd = sd.AddDays(1)
                Next
            End If
        End If
    End Sub

 
    Protected Sub lnk_View3_Click(sender As Object, e As System.EventArgs) Handles lnk_View3.Click

        rbl_location_tours.DataSource = tourWaveLimits.GetLocations()
        rbl_location_tours.DataTextField = "ComboItem"
        rbl_location_tours.DataValueField = "ComboItemID"
        rbl_location_tours.DataBind()

        With rbl_campaign_types
            .DataSource = tourWaveLimits.GetCampaignTypes
            .DataTextField = "campaign_type_name"
            .DataValueField = "ID"
            .DataBind()
        End With


        With ddl_wave_tours
            .DataSource = tourWaveLimits.GetTourWaves
            .DataTextField = "Description"
            .DataValueField = "ComboItemID"
            .DataBind()
        End With

        MultiView1.SetActiveView(View3)
    End Sub

    Protected Sub btn_submit_Click(sender As Object, e As System.EventArgs) Handles btn_submit.Click
        gv_change_multiple.DataSource = Nothing
        gv_change_multiple.DataBind()

        Dim locations = rbl_location_tours.Items.OfType(Of ListItem).Where(Function(x) x.Selected)
        Dim campaigns = rbl_campaign_types.Items.OfType(Of ListItem).Where(Function(x) x.Selected)
        Dim wave = ddl_wave_tours.SelectedValue

        Dim begin_date = date_begin.Selected_Date
        Dim end_date = date_end.Selected_Date                

        If locations.Count() = 0 Or campaigns.Count() = 0 Or String.IsNullOrEmpty(begin_date) Or String.IsNullOrEmpty(end_date) Then Return
        tourWaveLimits = New clsTourWaveLimits()
        Dim dt = tourWaveLimits.GetData(begin_date, end_date, locations.Single().Value, wave, campaigns.Single().Value)        
        Dim cbl_weekdays_selected = cbl_weekdays.Items.OfType(Of ListItem).Where(Function(x) x.Selected).Select(Function(x) x.Text).ToArray()
        If cbl_weekdays_selected.Count() > 0 Then
            gv_change_multiple.DataSource = dt.Rows.OfType(Of DataRow).Where(Function(x) Array.IndexOf(cbl_weekdays_selected, DateTime.Parse(x("Start Date").ToString()).DayOfWeek.ToString()) >= 0).Select(Function(x) x).CopyToDataTable()
            gv_change_multiple.DataBind()
        Else
            gv_change_multiple.DataSource = dt
            gv_change_multiple.DataBind()
        End If
    End Sub

    Protected Sub btn_save_Click(sender As Object, e As System.EventArgs) Handles btn_save.Click
        Dim limit_max = txb_max_count.Text.Trim()
        If limit_max.Length = 0 Then Return

        Dim wave_limits_id() As String = New String() {}

        For i = 0 To gv_change_multiple.Rows.Count - 1
            If gv_change_multiple.Rows(i).RowType = DataControlRowType.DataRow Then
                Array.Resize(wave_limits_id, wave_limits_id.Length + 1)
                wave_limits_id(i) = gv_change_multiple.Rows(i).Cells(0).Text
            End If
        Next

        tourWaveLimits = New clsTourWaveLimits()
        Dim records_affected = tourWaveLimits.UpdateData(limit_max, wave_limits_id)
        If records_affected = wave_limits_id.Length Then
            txb_max_count.Text = 0
            btn_submit_Click(Nothing, EventArgs.Empty)
        Else
            Response.Write("<br/><strong>Critical Update Error.</strong>")
        End If

    End Sub
End Class
