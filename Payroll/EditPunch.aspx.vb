Imports Microsoft.VisualBasic
Partial Class Payroll_EditPunch
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then

            ddTIHour.Items.Add("")
            ddTOHour.Items.Add("")
            For i = 1 To 12
                ddTIHour.Items.Add(i)
                ddTOHour.Items.Add(i)
            Next

            ddTIMinute.Items.Add("")
            ddTOMinute.Items.Add("")
            For i = 0 To 59
                If i = 0 Then
                    ddTIMinute.Items.Add("00")
                    ddTOMinute.Items.Add("00")
                ElseIf i < 10 Then
                    ddTIMinute.Items.Add("0" & i)
                    ddTOMinute.Items.Add("0" & i)
                Else
                    ddTIMinute.Items.Add(i)
                    ddTOMinute.Items.Add(i)
                End If
            Next

            siPunchType.Connection_String = Resources.Resource.cns
            siPunchType.ComboItem = "TimeClockPunchType"
            siPunchType.Label_Caption = ""
            siPunchType.Load_Items()

            ddTIAMPM.Items.Add("AM")
            ddTIAMPM.Items.Add("PM")
            ddTOAMPM.Items.Add("AM")
            ddTOAMPM.Items.Add("PM")

            Dim oPers2Dept As New clsPersonnel2Dept
            If Request("PunchID") <> "0" Then
                Dim oPersonnelPunch As New clsPersonnelPunch
                oPersonnelPunch.PunchID = Request("PunchID")
                oPersonnelPunch.Load()
                ddDept.DataSource = oPers2Dept.Get_Depts(oPersonnelPunch.PersonnelID)
                ddDept.DataTextField = "Department"
                ddDept.DataValueField = "DepartmentID"
                ddDept.DataBind()
                ddDept.SelectedValue = oPersonnelPunch.DepartmentID
                siPunchtype.Selected_ID = oPersonnelPunch.PunchTypeID
                dteInDate.Selected_Date = oPersonnelPunch.DateIn
                dteOutDate.Selected_Date = oPersonnelPunch.DateOut


                Dim sDate() As String
                Dim sTime() As String
                If oPersonnelPunch.TimeIn & "" <> "" Then
                    sDate = oPersonnelPunch.TimeIn.Split(" ")
                    If sDate.Count = 1 Then 'Midnight
                        ddTIHour.SelectedValue = 12
                        ddTIMinute.SelectedValue = "00"
                        ddTIAMPM.SelectedValue = "AM"
                    Else
                        sTime = sDate(1).Split(":")

                        If sTime(0) = "00" And sTime(1) = "00" Then
                            ddTIHour.SelectedValue = 12
                            ddTIMinute.SelectedValue = "00"
                            ddTIAMPM.SelectedValue = "AM"
                        Else
                            ddTIHour.SelectedValue = sTime(0)
                            ddTIMinute.SelectedValue = sTime(1)
                            ddTIAMPM.SelectedValue = sDate(2)
                        End If

                    End If
                End If

                If oPersonnelPunch.TimeOut & "" <> "" Then
                    sDate = oPersonnelPunch.TimeOut.Split(" ")
                    If sDate.Count = 1 Then 'Midnight
                        ddTOHour.SelectedValue = 12
                        ddTOMinute.SelectedValue = "00"
                        ddTOAMPM.SelectedValue = "AM"
                    Else
                        sTime = sDate(1).Split(":")
                        ddTOHour.SelectedValue = sTime(0)
                        ddTOMinute.SelectedValue = sTime(1)
                        ddTOAMPM.SelectedValue = sDate(2)

                    End If
                End If
                oPersonnelPunch = Nothing
            Else
                ddDept.DataSource = oPers2Dept.Get_Depts(Request("PersonnelID"))
                ddDept.DataTextField = "Department"
                ddDept.DataValueField = "DepartmentID"
                ddDept.DataBind()
            End If
            oPers2Dept = Nothing
        End If
    End Sub

    Protected Sub Unnamed5_Click(sender As Object, e As System.EventArgs)
        dteInDate.Selected_Date = ""
        ddTIHour.SelectedValue = ""
        ddTIMinute.SelectedValue = ""
    End Sub

    Protected Sub Unnamed9_Click(sender As Object, e As System.EventArgs)
        dteOutDate.Selected_Date = ""
        ddTOHour.SelectedValue = ""
        ddTOMinute.SelectedValue = ""
    End Sub

    Protected Sub Unnamed12_Click(sender As Object, e As System.EventArgs)
        Dim bProceed As Boolean = True
        Dim sErr As String = ""
        Dim inDate As Date
        Dim outDate As Date
        Dim wipeIn As Boolean = False
        Dim wipeout As Boolean = False
        If dteInDate.Selected_Date = "" Then
            If ddTIHour.SelectedValue <> "" Or ddTIMinute.SelectedValue <> "" Then
                bProceed = False
                sErr = "Please Fill In All Fields."
            End If
        Else
            If ddTIHour.SelectedValue = "" Or ddTIMinute.SelectedValue = "" Then
                bProceed = False
                sErr = "Please Fill In All Fields."
            Else
                inDate = CDate(dteInDate.Selected_Date & " " & ddTIHour.SelectedValue & ":" & ddTIMInute.SelectedValue & ":00 " & ddTIAMPM.SelectedValue)
            End If
        End If
        If dteOutDate.Selected_Date = "" Then
            If ddTOHour.SelectedValue <> "" Or ddTOMinute.SelectedValue <> "" Then
                bProceed = False
                sErr = "Please Fill In All Fields."
            End If
        Else
            If ddTOHour.SelectedValue = "" Or ddTOMinute.SelectedValue = "" Then
                bProceed = False
                sErr = "Please Fill In All Fields."
            Else
                outDate = CDate(dteOUtDate.Selected_Date & " " & ddTOHour.SelectedValue & ":" & ddTOMInute.SelectedValue & ":00 " & ddTOAMPM.SelectedValue)
            End If
        End If

        If bProceed Then
            Dim oPersPunch As New clsPersonnelPunch
            If Request("PunchID") = 0 Then
                oPersPunch.PunchID = 0
                oPersPunch.Load()
                oPersPunch.PersonnelID = Request("PersonnelID")
                oPersPunch.PunchTypeID = siPunchType.Selected_ID
                oPersPunch.DepartmentID = ddDept.SelectedValue
                If dteInDate.Selected_Date <> "" Then
                    oPersPunch.DateIn = dteInDate.Selected_Date
                    oPersPunch.TimeIn = inDate
                    oPersPunch.InManual = True
                    oPersPunch.InUserID = Session("UserDBID")
                    oPersPunch.InManDate = System.DateTime.Now
                End If
                If dteOutDate.Selected_Date <> "" Then
                    oPersPunch.DateOut = dteOutDate.Selected_Date
                    oPersPunch.TimeOut = outDate
                    oPersPunch.OutManual = True
                    oPersPunch.OutUserID = Session("UserDBID")
                    oPersPunch.OutManDate = System.DateTime.Now
                End If
                oPersPunch.Save()
            Else
                oPersPunch.PunchID = Request("PunchID")
                oPersPunch.UserID = Session("UserDBID")
                oPersPunch.Load()
                oPersPunch.PunchTypeID = siPunchType.Selected_ID
                oPersPunch.DepartmentID = ddDept.SelectedValue
                If dteInDate.Selected_Date <> "" Then
                    If oPersPunch.DateIn & "" = "" Then
                        oPersPunch.DateIn = dteInDate.Selected_Date
                        oPersPunch.TimeIn = inDate
                        oPersPunch.InManual = True
                        oPersPunch.InUserID = Session("UserDBID")
                        oPersPunch.InManDate = System.DateTime.Now
                    ElseIf (Hour(oPersPunch.TimeIn) <> Hour(inDate)) Or (Minute(oPersPunch.TimeIn) <> Minute(inDate)) Or (InStr(oPersPunch.TimeIn.ToString, "AM") > 0 And InStr(inDate.ToString, "AM") < 1) Or (InStr(oPersPunch.TimeIn.ToString, "PM") > 0 And InStr(inDate.ToString, "PM") < 1) Or (Date.Compare(oPersPunch.DateIn, CDate(dteInDate.Selected_Date)) <> 0) Then
                        oPersPunch.DateIn = dteInDate.Selected_Date
                        oPersPunch.TimeIn = inDate
                        oPersPunch.InManual = True
                        oPersPunch.InUserID = Session("UserDBID")
                        oPersPunch.InManDate = System.DateTime.Now
                    End If
                Else
                    wipeIn = True
                    oPersPunch.InManual = False
                    oPersPunch.InUserID = 0
                End If
                If dteOutDate.Selected_Date <> "" Then
                    If oPersPunch.DateOut & "" = "" Then
                        oPersPunch.DateOut = dteOutDate.Selected_Date
                        oPersPunch.TimeOut = outDate
                        oPersPunch.OutManual = True
                        oPersPunch.OutUserID = Session("UserDBID")
                        oPersPunch.OutManDate = System.DateTime.Now
                    ElseIf (Hour(oPersPunch.TimeOut) <> Hour(outDate)) Or (Minute(oPersPunch.TimeOut) <> Minute(outDate)) Or (InStr(oPersPunch.TimeOut.ToString, "AM") > 0 And InStr(outDate.ToString, "AM") < 1) Or (InStr(oPersPunch.TimeOut.ToString, "PM") > 0 And InStr(outDate.ToString, "PM") < 1) Or (Date.Compare(oPersPunch.DateOut, CDate(dteOutDate.Selected_Date)) <> 0) Then
                        oPersPunch.DateOut = dteOutDate.Selected_Date
                        oPersPunch.TimeOut = outDate
                        oPersPunch.OutManual = True
                        oPersPunch.OutUserID = Session("UserDBID")
                        oPersPunch.OutManDate = System.DateTime.Now
                    End If
                Else
                    oPersPunch.OutManual = False
                    oPersPunch.OutUserID = 0
                    wipeout = True
                End If
                oPersPunch.Save()
                If wipeIn Then
                    oPersPunch.Wipe_Punch(oPersPunch.PunchID, True)
                End If
                If wipeout Then
                    oPersPunch.Wipe_Punch(oPersPunch.PunchID, False)
                End If
            End If


            oPersPunch = Nothing
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.opener.Refresh_TimeClock();window.close();", True)
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('" & sErr & "');", True)
        End If
    End Sub

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.close();", True)
    End Sub
End Class
