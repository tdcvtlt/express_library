
Partial Class Payroll_MissedPunches
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oPers2Dept As New clsPersonnel2Dept
            Dim oMissedPunch As New clsPersonnelMissedPunch
            If oPers2Dept.Member_Of(Session("UserDBID"), "HR") Then
                'gvMgrApproved.DataSource = oMissedPunch.Get_Missed_Punches("1")
                'gvMgrApproved.DataBind()

                gvMgrPending.DataSource = oMissedPunch.Get_Missed_Punches("0")
                gvMgrPending.DataBind()
                'MultiView2.ActiveViewIndex = 1

                MultiView1.ActiveViewIndex = 0
                MultiView2.ActiveViewIndex = 1
            Else
                gvDeptMissed.DataSource = oMissedPunch.Get_Dept_Missed_Punches(Session("UserDBID"))
                gvDeptMissed.DataBind()
                MultiView1.ActiveViewIndex = 1
            End If
            oPers2Dept = Nothing
            oMissedPunch = Nothing
        End If
    End Sub

    Protected Sub MgrApproved_Link_Click(sender As Object, e As System.EventArgs) Handles MgrApproved_Link.Click
        Dim oMissedPunch As New clsPersonnelMissedPunch
        gvMgrApproved.DataSource = oMissedPunch.Get_Missed_Punches("1")
        gvMgrApproved.DataBind()
        MultiView2.ActiveViewIndex = 0
        oMissedPunch = Nothing
    End Sub
    Protected Sub gvMgrApproved_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Punches" Then
            e.Row.Cells(0).Visible = False
        End If
    End Sub
    Protected Sub MgrPending_Link_Click(sender As Object, e As System.EventArgs) Handles MgrPending_Link.Click
        Dim oMissedPunch As New clsPersonnelMissedPunch
        gvMgrPending.DataSource = oMissedPunch.Get_Missed_Punches("0")
        gvMgrPending.DataBind()
        MultiView2.ActiveViewIndex = 1
        oMissedPunch = Nothing
    End Sub
    Protected Sub gvMgrPending_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Punches" Then
            e.Row.Cells(0).Visible = False
        End If
    End Sub
    Protected Sub MgrDenied_Link_Click(sender As Object, e As System.EventArgs) Handles MgrDenied_Link.Click
        Dim oMissedPunch As New clsPersonnelMissedPunch
        gvMgrDenied.DataSource = oMissedPunch.Get_Missed_Punches("-1")
        gvMgrDenied.DataBind()
        MultiView2.ActiveViewIndex = 2
        oMissedPunch = Nothing
    End Sub
    Protected Sub gvMgrDenied_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Punches" Then
            e.Row.Cells(0).Visible = False
        End If
    End Sub
    Protected Sub gvDeptMissed_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Punches" Then
            e.Row.Cells(0).Visible = False
        End If
    End Sub

    Protected Sub Unnamed4_Click(sender As Object, e As System.EventArgs)
        Dim bProceed As Boolean = True
        Dim sErr As String = ""
        Dim oPers As New clsPersonnel
        oPers.PersonnelID = Session("UserDBID")
        oPers.Load()

        For Each row As GridViewRow In gvDeptMissed.Rows
            Dim cbApprove As CheckBox = row.FindControl("cbApprove")
            Dim cbDeny As CheckBox = row.FindControl("cbDeny")
            If cbApprove.Checked And cbDeny.Checked Then
                bProceed = False
                sErr = "You Can Only Select Approve OR Deny. Not Both."
            End If
            If cbApprove.Checked Or cbDeny.Checked Then
                If row.Cells(1).Text = oPers.LastName & ", " & oPers.FirstName Then
                    sErr = "You Can Not Approve/Deny Your Own Missed Punches"
                    bProceed = False
                End If
            End If
        Next
        oPers = Nothing

        If bProceed Then
            Dim oMissedPunch As New clsPersonnelMissedPunch
            For Each row As GridViewRow In gvDeptMissed.Rows
                Dim cbApprove As CheckBox = row.FindControl("cbApprove")
                Dim cbDeny As CheckBox = row.FindControl("cbDeny")
                If cbApprove.Checked Then
                    oMissedPunch.MissedPunchID = row.Cells(0).Text
                    oMissedPunch.Load()
                    oMissedPunch.UserID = Session("UserDBID")
                    oMissedPunch.ManagerApproved = "1"
                    oMissedPunch.ManagerID = Session("UserDBID")
                    oMissedPunch.Save()
                ElseIf cbDeny.Checked Then
                    oMissedPunch.MissedPunchID = row.Cells(0).Text
                    oMissedPunch.Load()
                    oMissedPunch.UserID = Session("UserDBID")
                    oMissedPunch.ManagerApproved = "-1"
                    oMissedPunch.ManagerID = Session("UserDBID")
                    oMissedPunch.Save()
                    'Added 5/7/2013 Undo punch when missed punch is denied
                    Dim oPersPunch As New clsPersonnelPunch
                    oPersPunch.Wipe_Punch(oMissedPunch.PunchID, oMissedPunch.PunchIn)
                    oPersPunch.PunchID = oMissedPunch.PunchID
                    oPersPunch.Load()
                    oPersPunch.UserID = Session("UserDBID")
                    If oMissedPunch.PunchIn Then
                        oPersPunch.InManual = False
                        oPersPunch.InUserID = 0
                        oPersPunch.InClockID = 0
                    Else
                        oPersPunch.OutManual = False
                        oPersPunch.OutUserID = 0
                        oPersPunch.OutClockID = 0
                    End If
                    oPersPunch.Save()
                    'If oMissedPunch.LinkedMissedPunchID > 0 Then
                    oPersPunch.PunchID = oMissedPunch.PunchID
                    oPersPunch.Load()
                    If (IsDBNull(oPersPunch.TimeIn) And IsDBNull(oPersPunch.TimeOut)) Or (oPersPunch.TimeIn = "" And oPersPunch.TimeOut = "") Then
                        oPersPunch.Delete_Punch(oMissedPunch.PunchID)
                    End If
                    'End If
                    oPersPunch = Nothing
                    'End 5/7/2013 addition
                End If
            Next
            gvDeptMissed.DataSource = oMissedPunch.Get_Dept_Missed_Punches(Session("UserDBID"))
            gvDeptMissed.DataBind()
            oMissedPunch = Nothing
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('" & sErr & "');", True)
        End If
    End Sub

    Protected Sub Unnamed3_Click(sender As Object, e As System.EventArgs)
        Dim bProceed As Boolean = True
        Dim sErr As String = ""
        For Each row As GridViewRow In gvmgrDenied.Rows
            Dim cbApprove As CheckBox = row.FindControl("cbApprove")
            Dim cbDeny As CheckBox = row.FindControl("cbDeny")
            If cbApprove.Checked And cbDeny.Checked Then
                bProceed = False
                sErr = "You Can Only Select Approve OR Deny. Not Both."
            End If
        Next

        If bProceed Then
            Dim oMissedPunch As New clsPersonnelMissedPunch
            For Each row As GridViewRow In gvmgrDenied.Rows
                Dim cbApprove As CheckBox = row.FindControl("cbApprove")
                Dim cbDeny As CheckBox = row.FindControl("cbDeny")
                If cbApprove.Checked Then
                    Dim oMissedPunchB As New clsPersonnelMissedPunch
                    Dim oComboitems As New clsComboItems
                    Dim oPersPunch As New clsPersonnelPunch
                    Dim punchID As Integer
                    Dim sDate() As String
                    oMissedPunch.MissedPunchID = row.Cells(0).Text
                    oMissedPunch.Load()
                    oMissedPunch.UserID = Session("UserDBID")
                    oMissedPunch.HRApproved = "1"
                    oMissedPunch.HRID = Session("UserDBID")
                    sDate = oMissedPunch.PunchTime.ToString.Split(" ")
                    If oMissedPunch.PunchID = 0 Then
                        oMissedPunchB.MissedPunchID = oMissedPunch.LinkedMissedPunchID
                        oMissedPunchB.Load()
                        If oMissedPunchB.PunchID = 0 Then
                            oPersPunch.PunchID = 0
                            oPersPunch.Load()
                            oPersPunch.UserID = Session("UserDBID")
                            oPersPunch.PersonnelID = oMissedPunch.PersonnelID
                            oPersPunch.DepartmentID = oMissedPunch.DepartmentID
                            oPersPunch.PunchTypeID = oComboitems.Lookup_ID("TimeClockPunchType", "Work")
                            If oMissedPunch.PunchIn Then
                                oPersPunch.TimeIn = oMissedPunch.PunchTime
                                oPersPunch.InManual = True
                                oPersPunch.InUserID = Session("UserDBID")
                                oPersPunch.DateIn = sDate(0)
                            Else
                                oPersPunch.TimeOut = oMissedPunch.PunchTime
                                oPersPunch.OutManual = True
                                oPersPunch.OutUserID = Session("UserDBID")
                                oPersPunch.DateOut = sDate(0)
                            End If
                            oPersPunch.Save()
                            punchID = oPersPunch.PunchID
                        Else
                            punchID = oMissedPunchB.PunchID
                            oPersPunch.PunchID = punchID
                            oPersPunch.Load()
                            oPersPunch.UserID = Session("UserDBID")
                            If oMissedPunch.PunchIn Then
                                oPersPunch.TimeIn = oMissedPunch.PunchTime
                                oPersPunch.InManual = True
                                oPersPunch.InUserID = Session("UserDBID")
                                oPersPunch.DateIn = sDate(0)
                            Else
                                oPersPunch.TimeOut = oMissedPunch.PunchTime
                                oPersPunch.OutManual = True
                                oPersPunch.OutUserID = Session("UserDBID")
                                oPersPunch.DateOut = sDate(0)
                            End If
                            oPersPunch.Save()
                        End If
                        oMissedPunch.PunchID = punchID
                    Else
                        oPersPunch.PunchID = oMissedPunch.PunchID
                        oPersPunch.Load()
                        oPersPunch.UserID = Session("UserDBID")
                        If oMissedPunch.PunchIn Then
                            oPersPunch.TimeIn = oMissedPunch.PunchTime
                            oPersPunch.InManual = True
                            oPersPunch.InUserID = Session("UserDBID")
                            oPersPunch.DateIn = sDate(0)
                        Else
                            oPersPunch.TimeOut = oMissedPunch.PunchTime
                            oPersPunch.OutManual = True
                            oPersPunch.OutUserID = Session("UserDBID")
                            oPersPunch.DateOut = sDate(0)
                        End If
                        oPersPunch.Save()
                    End If
                    oMissedPunch.Save()
                ElseIf cbDeny.Checked Then
                    oMissedPunch.MissedPunchID = row.Cells(0).Text
                    oMissedPunch.Load()
                    oMissedPunch.UserID = Session("UserDBID")
                    oMissedPunch.HRApproved = "-1"
                    oMissedPunch.HRID = Session("UserDBID")
                    oMissedPunch.Save()
                End If
            Next
            gvMgrDenied.DataSource = oMissedPunch.Get_Missed_Punches("-1")
            gvMgrDenied.DataBind()
            oMissedPunch = Nothing
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('" & sErr & "');", True)
        End If
    End Sub

    Protected Sub Unnamed2_Click(sender As Object, e As System.EventArgs)
        Dim bProceed As Boolean = True
        Dim sErr As String = ""
        For Each row As GridViewRow In gvmgrPending.Rows
            Dim cbApprove As CheckBox = row.FindControl("cbApprove")
            Dim cbDeny As CheckBox = row.FindControl("cbDeny")
            If cbApprove.Checked And cbDeny.Checked Then
                bProceed = False
                sErr = "You Can Only Select Approve OR Deny. Not Both."
            End If
        Next

        If bProceed Then
            Dim oMissedPunch As New clsPersonnelMissedPunch
            For Each row As GridViewRow In gvmgrPending.Rows
                Dim cbApprove As CheckBox = row.FindControl("cbApprove")
                Dim cbDeny As CheckBox = row.FindControl("cbDeny")
                If cbApprove.Checked Then
                    oMissedPunch.MissedPunchID = row.Cells(0).Text
                    oMissedPunch.Load()
                    oMissedPunch.UserID = Session("UserDBID")
                    oMissedPunch.HRApproved = "1"
                    oMissedPunch.HRID = Session("UserDBID")
                    'Replaced 5/7/2013 
                    'Dim oMissedPunchB As New clsPersonnelMissedPunch
                    'Dim oComboitems As New clsComboItems
                    'Dim oPersPunch As New clsPersonnelPunch
                    'Dim punchID As Integer = 0
                    'Dim sDate() As String
                    'sDate = oMissedPunch.PunchTime.ToString.Split(" ")
                    'If oMissedPunch.PunchID = 0 Then
                    '    oMissedPunchB.MissedPunchID = oMissedPunch.LinkedMissedPunchID
                    '    oMissedPunchB.Load()
                    '    If oMissedPunchB.PunchID = 0 Then
                    '        oPersPunch.PunchID = 0
                    '        oPersPunch.Load()
                    '        oPersPunch.UserID = Session("UserDBID")
                    '        oPersPunch.PersonnelID = oMissedPunch.PersonnelID
                    '        oPersPunch.DepartmentID = oMissedPunch.DepartmentID
                    '        oPersPunch.PunchTypeID = oComboitems.Lookup_ID("TimeClockPunchType", "Work")
                    '        If oMissedPunch.PunchIn Then
                    '            oPersPunch.TimeIn = oMissedPunch.PunchTime
                    '            oPersPunch.InManual = True
                    '            oPersPunch.InUserID = Session("UserDBID")
                    '            oPersPunch.DateIn = sDate(0)
                    '        Else
                    '            oPersPunch.TimeOut = oMissedPunch.PunchTime
                    '            oPersPunch.OutManual = True
                    '            oPersPunch.OutUserID = Session("UserDBID")
                    '            oPersPunch.DateOut = sDate(0)
                    '        End If
                    '        oPersPunch.Save()
                    '        punchID = oPersPunch.PunchID
                    '    Else
                    '        punchID = oMissedPunchB.PunchID
                    '        oPersPunch.PunchID = punchID
                    '        oPersPunch.Load()
                    '        oPersPunch.UserID = Session("UserDBID")
                    '        If oMissedPunch.PunchIn Then
                    '            oPersPunch.TimeIn = oMissedPunch.PunchTime
                    '            oPersPunch.InManual = True
                    '            oPersPunch.InUserID = Session("UserDBID")
                    '            oPersPunch.DateIn = sDate(0)
                    '        Else
                    '            oPersPunch.TimeOut = oMissedPunch.PunchTime
                    '            oPersPunch.OutManual = True
                    '            oPersPunch.OutUserID = Session("UserDBID")
                    '            oPersPunch.DateOut = sDate(0)
                    '        End If
                    '        oPersPunch.Save()
                    '    End If
                    '    oMissedPunch.PunchID = punchID
                    'Else
                    '    oPersPunch.PunchID = oMissedPunch.PunchID
                    '    oPersPunch.Load()
                    '    oPersPunch.UserID = Session("UserDBID")
                    '    If oMissedPunch.PunchIn Then
                    '        oPersPunch.TimeIn = oMissedPunch.PunchTime
                    '        oPersPunch.InManual = True
                    '        oPersPunch.InUserID = Session("UserDBID")
                    '        oPersPunch.DateIn = sDate(0)
                    '    Else
                    '        oPersPunch.TimeOut = oMissedPunch.PunchTime
                    '        oPersPunch.OutManual = True
                    '        oPersPunch.OutUserID = Session("UserDBID")
                    '        oPersPunch.DateOut = sDate(0)
                    '    End If
                    '    oPersPunch.Save()
                    'End If
                    oMissedPunch.Save()
                ElseIf cbDeny.Checked Then
                    oMissedPunch.MissedPunchID = row.Cells(0).Text
                    oMissedPunch.Load()
                    oMissedPunch.UserID = Session("UserDBID")
                    oMissedPunch.HRApproved = "-1"
                    oMissedPunch.HRID = Session("UserDBID")
                    oMissedPunch.Save()
                    'Added 5/7/2013 Undo punch when missed punch is denied
                    Dim oPersPunch As New clsPersonnelPunch
                    oPersPunch.Wipe_Punch(oMissedPunch.PunchID, oMissedPunch.PunchIn)
                    oPersPunch.PunchID = oMissedPunch.PunchID
                    oPersPunch.Load()
                    oPersPunch.UserID = Session("UserDBID")
                    If oMissedPunch.PunchIn Then
                        oPersPunch.InManual = False
                        oPersPunch.InUserID = 0
                        oPersPunch.InClockID = 0
                    Else
                        oPersPunch.OutManual = False
                        oPersPunch.OutUserID = 0
                        oPersPunch.OutClockID = 0
                    End If
                    oPersPunch.Save()
                    'If oMissedPunch.LinkedMissedPunchID > 0 Then
                    oPersPunch.PunchID = oMissedPunch.PunchID
                    oPersPunch.Load()
                    If (IsDBNull(oPersPunch.TimeIn) And IsDBNull(oPersPunch.TimeOut)) Or (oPersPunch.TimeIn = "" And oPersPunch.TimeOut = "") Then
                        oPersPunch.Delete_Punch(oMissedPunch.PunchID)
                    End If
                    'End If
                    oPersPunch = Nothing
                    'End 5/7/2013 addition
                End If
            Next
            gvMgrPending.DataSource = oMissedPunch.Get_Missed_Punches("0")
            gvMgrPending.DataBind()
            oMissedPunch = Nothing
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('" & sErr & "');", True)
        End If
    End Sub

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        Dim bProceed As Boolean = True
        Dim sErr As String = ""
        For Each row As GridViewRow In gvmgrApproved.Rows
            Dim cbApprove As CheckBox = row.FindControl("cbApprove")
            Dim cbDeny As CheckBox = row.FindControl("cbDeny")
            If cbApprove.Checked And cbDeny.Checked Then
                bProceed = False
                sErr = "You Can Only Select Approve OR Deny. Not Both."
            End If
        Next

        If bProceed Then
            Dim oMissedPunch As New clsPersonnelMissedPunch
            For Each row As GridViewRow In gvmgrApproved.Rows
                Dim cbApprove As CheckBox = row.FindControl("cbApprove")
                Dim cbDeny As CheckBox = row.FindControl("cbDeny")
                If cbApprove.Checked Then
                    Dim oMissedPunchB As New clsPersonnelMissedPunch
                    Dim oComboitems As New clsComboItems
                    Dim oPersPunch As New clsPersonnelPunch
                    Dim punchID As Integer
                    Dim sDate() As String
                    oMissedPunch.MissedPunchID = row.Cells(0).Text
                    oMissedPunch.Load()
                    oMissedPunch.HRApproved = "1"
                    oMissedPunch.HRID = Session("UserDBID")
                    oMissedPunch.UserID = Session("UserDBID")
                    sDate = oMissedPunch.PunchTime.ToString.Split(" ")
                    If oMissedPunch.PunchID = 0 Then
                        oMissedPunchB.MissedPunchID = oMissedPunch.LinkedMissedPunchID
                        oMissedPunchB.Load()
                        If oMissedPunchB.PunchID = 0 Then
                            oPersPunch.PunchID = 0
                            oPersPunch.Load()
                            oPersPunch.UserID = Session("UserDBID")
                            oPersPunch.PersonnelID = oMissedPunch.PersonnelID
                            oPersPunch.DepartmentID = oMissedPunch.DepartmentID
                            oPersPunch.PunchTypeID = oComboitems.Lookup_ID("TimeClockPunchType", "Work")
                            If oMissedPunch.PunchIn Then
                                oPersPunch.TimeIn = oMissedPunch.PunchTime
                                oPersPunch.InManual = True
                                oPersPunch.InUserID = Session("UserDBID")
                                oPersPunch.DateIn = sDate(0)
                            Else
                                oPersPunch.TimeOut = oMissedPunch.PunchTime
                                oPersPunch.OutManual = True
                                oPersPunch.OutUserID = Session("UserDBID")
                                oPersPunch.DateOut = sDate(0)
                            End If
                            oPersPunch.Save()
                            punchID = oPersPunch.PunchID
                        Else
                            punchID = oMissedPunchB.PunchID
                            oPersPunch.PunchID = punchID
                            oPersPunch.Load()
                            oPersPunch.UserID = Session("UserDBID")
                            If oMissedPunch.PunchIn Then
                                oPersPunch.TimeIn = oMissedPunch.PunchTime
                                oPersPunch.InManual = True
                                oPersPunch.InUserID = Session("UserDBID")
                                oPersPunch.DateIn = sDate(0)
                            Else
                                oPersPunch.TimeOut = oMissedPunch.PunchTime
                                oPersPunch.OutManual = True
                                oPersPunch.OutUserID = Session("UserDBID")
                                oPersPunch.DateOut = sDate(0)
                            End If
                            oPersPunch.Save()
                        End If
                        oMissedPunch.PunchID = punchID
                    Else
                        oPersPunch.PunchID = oMissedPunch.PunchID
                        oPersPunch.Load()
                        oPersPunch.UserID = Session("UserDBID")
                        If oMissedPunch.PunchIn Then
                            oPersPunch.TimeIn = oMissedPunch.PunchTime
                            oPersPunch.InManual = True
                            oPersPunch.InUserID = Session("UserDBID")
                            oPersPunch.DateIn = sDate(0)
                        Else
                            oPersPunch.TimeOut = oMissedPunch.PunchTime
                            oPersPunch.OutManual = True
                            oPersPunch.OutUserID = Session("UserDBID")
                            oPersPunch.DateOut = sDate(0)
                        End If
                        oPersPunch.Save()
                    End If
                    oMissedPunch.Save()
                ElseIf cbDeny.Checked Then
                    oMissedPunch.MissedPunchID = row.Cells(0).Text
                    oMissedPunch.Load()
                    oMissedPunch.UserID = Session("UserDBID")
                    oMissedPunch.HRApproved = "-1"
                    oMissedPunch.HRID = Session("UserDBID")
                    oMissedPunch.Save()
                End If
            Next
            gvMgrApproved.DataSource = oMissedPunch.Get_Missed_Punches("1")
            gvMgrApproved.DataBind()
            oMissedPunch = Nothing
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('" & sErr & "');", True)
        End If
    End Sub
End Class
