Imports Microsoft.VisualBasic
Partial Class Payroll_MissedPunchRequest
    Inherits System.Web.UI.Page

    Protected Sub MPType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles MPType.SelectedIndexChanged
        If MPType.SelectedIndex = 0 Then
            MultiView1.ActiveViewIndex = 0
        Else
            MultiView1.ActiveViewIndex = 1
        End If
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            For i = 1 To 12
                ddPunchHour.Items.Add(i)
                ddPunchInHour.Items.Add(i)
                ddPunchOutHour.Items.Add(i)
            Next
            For i = 0 To 59
                If i = 0 Then
                    ddPunchMinute.Items.Add("00")
                    ddPunchInMinute.Items.Add("00")
                    ddPunchOutMinute.Items.Add("00")
                ElseIf i < 10 Then
                    ddPunchMinute.Items.Add("0" & i)
                    ddPunchInMinute.Items.Add("0" & i)
                    ddPunchOutMinute.Items.Add("0" & i)
                Else
                    ddPunchMinute.Items.Add(i)
                    ddPunchInMinute.Items.Add(i)
                    ddPunchOutMinute.Items.Add(i)
                End If
            Next
            ddPunchAMPM.Items.Add("AM")
            ddPunchAMPM.Items.Add("PM")
            ddPunchInAMPM.Items.Add("AM")
            ddPunchInAMPM.Items.Add("PM")
            ddPunchOutAMPM.Items.Add("AM")
            ddPunchOutAMPM.Items.Add("PM")

            Dim oPers2Dept As New clsPersonnel2Dept
            ddPunchDept.DataSource = oPers2Dept.Get_Depts(Request("PersonnelID"))
            ddPunchDept.DataTextField = "Department"
            ddPunchDept.DataValueField = "DepartmentID"
            ddPunchDept.DataBind()
            ddShiftDept.DataSource = oPers2Dept.Get_Depts(Request("PersonnelID"))
            ddShiftDept.DataTextField = "Department"
            ddShiftDept.DataValueField = "DepartmentID"
            ddShiftDept.DataBind()

            If ddShiftDept.Items.Count = 0 Then
                ddShiftDept.Items.Add(0)
                ddPunchDept.Items.Add(0)
            End If
        End If
    End Sub

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        Dim bProceed As Boolean = True
        Dim sErr As String = ""
        Dim pDate As Date
        Dim punchID As Integer = 0
        Dim punchIn As Boolean = True
        If cbPType.SelectedIndex < 0 Then
            bProceed = False
            sErr = "Please Select If You Are Clocking In Or Clocking Out."
        ElseIf dtePunchDate.Selected_Date = "" Or txtPunchReason.Text = "" Then
            bProceed = False
            sErr = "Please Be Sure To FIll In All Fields."
        End If
        If bProceed Then
            pDate = CDate(dtePunchDate.Selected_Date & " " & ddPunchHour.SelectedValue & ":" & ddPunchMinute.SelectedValue & ":00 " & ddPunchAMPM.SelectedValue)
            If cbPType.SelectedIndex = 1 Then
                punchIn = False
            End If
            Dim oMissedPunch As New clsPersonnelMissedPunch
            punchID = oMissedPunch.Get_Missing_PunchID(Request("PersonnelID"), ddPunchDept.SelectedValue, pDate, punchIn)
            If punchID = 0 Then
                If punchIn = False Then
                    bProceed = False
                    sErr = "Could Not Find Missed Punch."
                End If
            Else
                '**** See if Missed Punch already submitted for this punch
                If Not (oMissedPunch.Check_For_Submitted_Punch(punchID, punchIn)) Then
                    bProceed = False
                    sErr = "You Have Already Submitted a Missed Punch Request for This Punch."
                End If
            End If

            If bProceed Then
                oMissedPunch.MissedPunchID = 0
                oMissedPunch.Load()
                oMissedPunch.PersonnelID = Request("PersonnelID")
                oMissedPunch.PunchTime = pDate
                oMissedPunch.DepartmentID = ddPunchDept.SelectedValue
                oMissedPunch.PunchIn = punchIn
                oMissedPunch.PunchID = punchID
                oMissedPunch.Reason = txtPunchReason.Text
                oMissedPunch.ManagerApproved = 0
                oMissedPunch.HRApproved = 0
                oMissedPunch.DateCreated = System.DateTime.Now
                oMissedPunch.Save()
                Dim oPersPunch As New clsPersonnelPunch
                Dim oPersTimeClock As New clsPersonnelTimeClock
                oPersPunch.PunchID = punchID
                oPersPunch.Load()
                oPersPunch.UserID = Request("PersonnelID")
                If punchID = 0 Then
                    oPersPunch.DepartmentID = ddPunchDept.SelectedValue
                    oPersPunch.PersonnelID = Request("PersonnelID")
                    Dim oCombo As New clsComboItems
                    oPersPunch.PunchTypeID = oCombo.Lookup_ID("TimeclockPunchType", "Work")
                    oCombo = Nothing
                End If
                If punchIn Then
                    oPersPunch.TimeIn = pDate
                    oPersPunch.DateIn = dtePunchDate.Selected_Date
                    oPersPunch.InManual = True
                    oPersPunch.InUserID = Request("PersonnelID")
                    oPersPunch.InManDate = System.DateTime.Now
                    oPersPunch.InClockID = oPersTimeClock.Get_Clock_ID(Request.ServerVariables("REMOTE_ADDR"))
                Else
                    oPersPunch.TimeOut = pDate
                    oPersPunch.DateOut = dtePunchDate.Selected_Date
                    oPersPunch.OutManual = True
                    oPersPunch.OutUserID = Request("PersonnelID")
                    oPersPunch.OutManDate = System.DateTime.Now
                    oPersPunch.OutClockID = oPersTimeClock.Get_Clock_ID(Request.ServerVariables("REMOTE_ADDR"))
                End If
                oPersPunch.Save()
                If punchID = 0 Then
                    oMissedPunch.PunchID = oPersPunch.PunchID
                    oMissedPunch.Save()
                End If
                oPersTimeClock = Nothing
                oPersPunch = Nothing
                If oMissedPunch.Err <> "" Then
                    lblErr.Text = oMissedPunch.Err
                Else
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('Missed Punch Request Submitted.');window.close();", True)
                End If
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('" & sErr & "');", True)
            End If
            oMissedPunch = Nothing
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('" & sErr & "');", True)
        End If
    End Sub

    Protected Sub Unnamed2_Click(sender As Object, e As System.EventArgs)
        Dim bProceed As Boolean = True
        Dim sErr As String = ""
        Dim piDate As Date
        Dim poDate As Date
        If dtePunchInDate.Selected_Date = "" Or txtShiftReason.Text = "" Or dtePunchOutDate.Selected_Date = "" Then
            bProceed = False
            sErr = "Please Be Sure To FIll In All Fields."
        End If
        If bProceed Then
            piDate = CDate(dtePunchInDate.Selected_Date & " " & ddPunchInHour.SelectedValue & ":" & ddPunchInMinute.SelectedValue & ":00 " & ddPunchInAMPM.SelectedValue)
            poDate = CDate(dtePunchOutDate.Selected_Date & " " & ddPunchOutHour.SelectedValue & ":" & ddPunchOutMinute.SelectedValue & ":00 " & ddPunchOutAMPM.SelectedValue)
            If Date.Compare(piDate, poDate) >= 0 Then
                'bProceed = False
                'sErr = "Make Sure The Punch Out Date is Greater than the Punch In Date."
            End If
        End If
        If bProceed Then
            Dim oPersPunch As New clsPersonnelPunch
            Dim oPersTimeClock As New clsPersonnelTimeClock
            Dim oCombo As New clsComboItems
            Dim punchID As Integer = 0
            oPersPunch.PunchID = 0
            oPersPunch.Load()
            oPersPunch.PersonnelID = Request("PersonnelID")
            oPersPunch.TimeIn = piDate
            oPersPunch.TimeOut = poDate
            oPersPunch.DateIn = dtePunchInDate.Selected_Date
            oPersPunch.DateOut = dtePunchOutDate.Selected_Date
            oPersPunch.InManual = True
            oPersPunch.InUserID = Request("PersonnelID")
            oPersPunch.OutManual = True
            oPersPunch.OutUserID = Request("PersonnelID")
            oPersPunch.InManDate = System.DateTime.Now
            oPersPunch.OutManDate = System.DateTime.Now
            oPersPunch.DepartmentID = ddShiftDept.SelectedValue
            oPersPunch.InClockID = oPersTimeClock.Get_Clock_ID(Request.ServerVariables("REMOTE_ADDR"))
            oPersPunch.OutClockID = oPersTimeClock.Get_Clock_ID(Request.ServerVariables("REMOTE_ADDR"))
            oPersPunch.PunchTypeID = oCombo.Lookup_ID("TimeclockPunchType", "Work")
            oPersPunch.Save()
            punchID = oPersPunch.PunchID
            oPersPunch = Nothing
            oPersTimeClock = Nothing
            oCombo = Nothing
            Dim oMissedPunchA As New clsPersonnelMissedPunch
            Dim oMissedPunchB As New clsPersonnelMissedPunch
            Dim punchA As Integer = 0
            Dim punchB As Integer = 0
            oMissedPunchA.MissedPunchID = 0
            oMissedPunchA.Load()
            oMissedPunchA.PersonnelID = Request("PersonnelID")
            oMissedPunchA.PunchIn = True
            oMissedPunchA.PunchTime = piDate
            oMissedPunchA.DepartmentID = ddShiftDept.SelectedValue
            oMissedPunchA.Reason = txtShiftReason.Text
            oMissedPunchA.HRApproved = 0
            oMissedPunchA.ManagerApproved = 0
            oMissedPunchA.DateCreated = System.DateTime.Now
            oMissedPunchA.PunchID = punchID
            oMissedPunchA.Save()
            punchA = oMissedPunchA.MissedPunchID

            oMissedPunchB.MissedPunchID = 0
            oMissedPunchB.PersonnelID = Request("PersonnelID")
            oMissedPunchB.PunchIn = False
            oMissedPunchB.PunchTime = poDate
            oMissedPunchB.DepartmentID = ddShiftDept.SelectedValue
            oMissedPunchB.Reason = txtShiftReason.Text
            oMissedPunchB.LinkedMissedPunchID = punchA
            oMissedPunchB.ManagerApproved = 0
            oMissedPunchB.HRApproved = 0
            oMissedPunchB.DateCreated = System.DateTime.Now
            oMissedPunchB.PunchID = punchID
            oMissedPunchB.Save()
            punchB = oMissedPunchB.MissedPunchID

            oMissedPunchA.LinkedMissedPunchID = punchB
            oMissedPunchA.Save()
            If oMissedPunchA.Err <> "" Or oMissedPunchB.Err <> "" Then
                lblErr.Text = oMissedPunchA.Err & " " & oMissedPunchB.Err
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('Missed Punch Request Submitted.');window.close();", True)
            End If
            oMissedPunchA = Nothing
            oMissedPunchB = Nothing

        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('" & sErr & "');", True)
        End If
    End Sub
End Class
