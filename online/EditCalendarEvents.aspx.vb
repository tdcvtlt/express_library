
Partial Class online_EditCalendarEvents
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Load_Items()
        End If
    End Sub

    Private Sub Load_Items()
        Dim oCI As New clsComboItems
        ddCalendar.DataSource = oCI.Load_ComboItems("EventCalendar")
        ddCalendar.DataValueField = "Comboitemid"
        ddCalendar.DataTextField = "Comboitem"
        ddCalendar.DataBind()
        oCI = Nothing
        Dim oEv As New clsCalendarEvents
        txtID.Text = Request("ID")
        oEv.EventID = txtID.Text
        oEv.Load()
        dfDate.Selected_Date = oEv.EventDate
        ddCalendar.SelectedValue = oEv.CalendarID
        ddHour.SelectedValue = Hour(CDate("1/1/12 " & oEv.Time))
        ddMin.SelectedValue = Minute(CDate("1/1/12 " & oEv.Time))
        ddAMPM.SelectedValue = Right(oEv.Time, 2)
        txtDesc.Text = oEv.Description
        cbActive.Checked = oEv.Active
        oEv = Nothing
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If CheckSecurity("OnlineCalendar", "Edit", , , Session("UserDBID")) Then
            If Valid() Then
                Dim oEv As New clsCalendarEvents
                oEv.EventID = txtID.Text
                oEv.Load()
                oEv.CalendarID = ddCalendar.SelectedValue
                oEv.EventDate = dfDate.Selected_Date
                oEv.Time = ddHour.SelectedValue & ":" & ddMin.SelectedValue & " " & ddAMPM.SelectedValue
                oEv.Description = txtDesc.Text
                oEv.Active = cbActive.Checked
                oEv.Save()
                oEv = Nothing
                Response.Redirect("Calendarevents.aspx")
            End If
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Access", "alert('Access Denied');", True)
        End If
    End Sub

    Private Function Valid() As Boolean
        Dim bAns As Boolean = False
        Dim sErr As String = ""
        sErr &= IIf(dfDate.Selected_Date = "", "Please select a Date.\n", "")
        sErr &= IIf(ddHour.SelectedIndex = -1 Or ddMin.SelectedIndex = -1 Or ddAMPM.SelectedIndex = -1, "Please select a Time\n", "")
        sErr &= IIf(txtDesc.Text = "", "Please enter an event", "")
        bAns = IIf(sErr = "", True, False)
        If sErr <> "" Then ClientScript.RegisterClientScriptBlock(Me.GetType, "error", "alert('" & sErr & "');", True)
        Return bAns
    End Function
End Class
