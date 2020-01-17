
Partial Class Payroll_SalesRepClockIn
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If CheckSecurity("Payroll", "ClockInSalesReps", , , Session("UserDBID")) Then
                Dim i As Integer = 0
                For i = 1 To 12
                    ddHour.Items.Add(New ListItem(i, i))
                Next
                For i = 0 To 59
                    If i < 10 Then
                        ddMinute.Items.Add(New ListItem("0" & i, "0" & i))
                    Else
                        ddMinute.Items.Add(New ListItem(i, i))
                    End If
                Next
                Dim oPers2Dept As New clsPersonnel2Dept
                gvReps.DataSource = oPers2Dept.Get_ClockIn_Members("'Dayline 1','In-House','Exit'")
                Dim sKeys(0) As String
                sKeys(0) = "PersonnelID"
                gvReps.DataKeyNames = sKeys
                gvReps.DataBind()
                oPers2Dept = Nothing
            Else
                btnGo.Visible = False
            End If
        End If
    End Sub

    Protected Sub btnGo_Click(sender As Object, e As System.EventArgs) Handles btnGo.Click
        lblErrors.Text = ""
        Dim oPersTimeClock As New clsPersonnelTimeClock
        Dim bProceed As Boolean = True
        Dim sErr As String = ""
        If rbPunchType.SelectedIndex < 0 Then
            bProceed = False
            sErr = "Please Select a Punch Type"
        ElseIf dtePunchDate.Selected_Date = "" Then
            bProceed = False
            sErr = "Please Select a Punch Date"
        ElseIf Not (oPersTimeClock.Validate_Clock(Request.ServerVariables("REMOTE_ADDR"))) Then
            bProceed = False
            sErr = "Unable to Clock In From This Location."
        End If
        If bProceed Then
            Dim pDate As Date
            pDate = CDate(dtePunchDate.Selected_Date & " " & ddHour.SelectedValue & ":" & ddMinute.SelectedValue & ":00 " & ddAMPM.SelectedValue)
            Dim oPersPunch As New clsPersonnelPunch
            Dim mp As String = ""
            Dim pID As Integer = 0
            For Each Row As GridViewRow In gvReps.Rows
                mp = ""
                Dim cbEmp As CheckBox = Row.FindControl("cbEmployee")
                If cbEmp.Checked Then
                    If rbPunchType.SelectedIndex = 0 Then
                        If oPersPunch.Val_Punch_In_With_Date(Row.Cells(1).Text, Row.Cells(4).Text, pDate) Then
                            Dim oCombo As New clsComboItems
                            mp = oPersPunch.Missed_Punch_Check_With_Date(Row.Cells(1).Text, Row.Cells(4).Text, "In", pDate)
                            If mp <> "" Then
                                If lblErrors.Text = "" Then

                                    lblErrors.Text = "<b><u>Error Log</u></b><br>" & Row.Cells(2).Text & mp
                                Else
                                    lblErrors.Text = lblErrors.Text & "<br><br>" & Row.Cells(2).Text & mp
                                End If
                            Else
                                oPersPunch.PunchID = 0
                                oPersPunch.Load()
                                oPersPunch.PersonnelID = Row.Cells(1).Text
                                oPersPunch.DepartmentID = Row.Cells(4).Text
                                oPersPunch.TimeIn = pDate
                                oPersPunch.DateIn = CDate(pDate).ToShortDateString
                                oPersPunch.InManual = True
                                oPersPunch.InUserID = Session("UserDBID")
                                oPersPunch.InManDate = System.DateTime.Now
                                oPersPunch.InClockID = oPersTimeClock.Get_Clock_ID(Request.ServerVariables("REMOTE_ADDR")) 'clockID ***********need to do clock 
                                oPersPunch.PunchTypeID = oCombo.Lookup_ID("TimeClockPunchType", "Work")
                                oPersPunch.Save()
                            End If
                        Else
                            If lblErrors.Text = "" Then
                                lblErrors.Text = "<b><u>Error Log</u></b><br>" & Row.Cells(2).Text & " has clocked in within 2 hours and have not clocked out. They are unable to clock in again until clocking out."
                            Else
                                lblErrors.Text = lblErrors.Text & "<br><br>" & Row.Cells(2).Text & " has clocked in within 2 hours and have not clocked out. They are unable to clock in again until clocking out."
                            End If
                        End If
                    Else
                        mp = oPersPunch.Missed_Punch_Check_With_Date(Row.Cells(1).Text, Row.Cells(4).Text, "Out", pDate)
                        If mp <> "" Then
                            If lblErrors.Text = "" Then
                                lblErrors.Text = "<b><u>Error Log</u></b><br>" & Row.Cells(2).Text & mp
                            Else
                                lblErrors.Text = lblErrors.Text & "<br><br>" & Row.Cells(2).Text & mp
                            End If
                        Else
                            pID = oPersPunch.Get_Current_Punch_With_Date(Row.Cells(1).Text, pDate)
                            If pID > 0 Then
                                oPersPunch.PunchID = pID
                                oPersPunch.Load()
                                oPersPunch.OutUserID = Session("UserDBID")
                                oPersPunch.OutManual = True
                                oPersPunch.OutManDate = System.DateTime.Now
                                oPersPunch.TimeOut = pDate
                                oPersPunch.DateOut = CDate(pDate).ToShortDateString
                                oPersPunch.OutClockID = oPersTimeClock.Get_Clock_ID(Request.ServerVariables("REMOTE_ADDR")) 'clockID ******GET CLOCK
                                oPersPunch.Save()
                            Else
                                If lblErrors.Text = "" Then
                                    lblErrors.Text = "<b><u>Error Log</u></b><br>" & Row.Cells(2).Text & " is attempting to punch out without punching in for this shift. They will need to submit a missed punch form before They are able to punch out."
                                Else
                                    lblErrors.Text = lblErrors.Text & "<br><br>" & Row.Cells(2).Text & " is attempting to punch out without punching in for this shift. They will need to submit a missed punch form before they are able to punch out."
                                End If
                            End If
                        End If
                    End If
                End If
            Next
            oPersPunch = Nothing
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('" & sErr & "');", True)
        End If
        oPersTimeClock = Nothing
    End Sub

    Protected Sub gvReps_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then
            e.Row.Cells(1).Visible = False
            e.Row.Cells(4).Visible = False
        End If
    End Sub
End Class
