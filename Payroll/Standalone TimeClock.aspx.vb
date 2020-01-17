
Partial Class Payroll_TimeClock
    Inherits System.Web.UI.Page

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        Dim persID As Integer = 0
        If txtSwipe.Text <> "" Then
            Dim oTimeCard As New clsPersonnelTimeCards
            persID = oTimeCard.get_Personnel_ID(txtSwipe.Text)
            If persID > 0 Then
                hfPersID.Value = persID
                Dim oPersonnel As New clsPersonnel
                Dim oPersPunch As New clsPersonnelPunch
                oPersonnel.PersonnelID = persID
                oPersonnel.Load()
                lblGreeting.Text = "<b>Welcome " & oPersonnel.FirstName & " " & oPersonnel.LastName & "!</b>"
                lblActivity.Text = oPersPunch.Get_Last_Activity(persID)
                oPersonnel = Nothing
                oPersPunch = Nothing
                MultiView1.ActiveViewIndex = 1
                Timer1.Enabled = True
            Else
                lblPWErr.Text = "Invalid ID. Please Try Again."
            End If
        Else
            lblPWErr.Text = "Invalid ID. Please Try Again."
        End If
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            MultiView1.ActiveViewIndex = 0
        Else
            Response.Expires = 0
            Response.Cache.SetNoStore()
            Response.AppendHeader("Pragma", "no-cache")
        End If
    End Sub

    Protected Sub lbPI_Click(sender As Object, e As System.EventArgs) Handles lbPI.Click
        'Clock Check
        Dim oPersClock As New clsPersonnelTimeClock
        If oPersClock.Validate_Clock(Request.ServerVariables("REMOTE_ADDR")) Then
            Dim oPers2Dept As New clsPersonnel2Dept
            Dim oPersPunch As New clsPersonnelPunch
            Dim deptCount As Integer = oPers2Dept.Get_Dept_Count(hfPersID.Value)
            If deptCount = 0 Then
                Punch_In(hfPersID.Value, 0)
            ElseIf deptCount = 1 Then
                Punch_In(hfPersID.Value, oPers2Dept.Get_Active_Dept(hfPersID.value))
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/Payroll/DeptSelect.aspx?PersonnelID=" & hfPersID.Value & "&ClockType=ClockIn','win01',350,150);", True)
            End If
            oPers2Dept = Nothing
            oPersPunch = Nothing
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('You May Not Clock In From This Machine.');", True)
        End If
        oPersClock = Nothing
    End Sub

    Private Sub Punch_In(persID As Integer, deptID As Integer)
        Dim oPersPunch As New clsPersonnelPunch
        Dim oPersTimeClock As New clsPersonnelTimeClock
        Dim mp As String = ""
        If oPersPunch.Val_Punch_In(persID, deptID) Then
            Dim oCombo As New clsComboItems
            mp = oPersPunch.Missed_Punch_Check(persID, deptID, "In")
            oPersPunch.PunchID = 0
            oPersPunch.Load()
            oPersPunch.PersonnelID = persID
            oPersPunch.DepartmentID = deptID
            oPersPunch.TimeIn = System.DateTime.Now
            oPersPunch.DateIn = System.DateTime.Now.ToShortDateString
            oPersPunch.InClockID = oPersTimeClock.Get_Clock_ID(Request.ServerVariables("REMOTE_ADDR")) 'clockID ***********need to do clock 
            oPersPunch.PunchTypeID = oCombo.Lookup_ID("TimeClockPunchType", "Work")
            oPersPunch.Save()
            lblID.Text = oPersPunch.Err
            If mp <> "" Then
                ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('" & mp & "');", True)
            End If
            lblActivity.Text = oPersPunch.Get_Last_Activity(persID)
            MultiView1.ActiveViewIndex = 1
            oCombo = Nothing
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('You have clocked in within 2 hours and have not clocked out. You are unable to clock in again until clocking out.');", True)
            MultiView1.ActiveViewIndex = 1
        End If
        oPersTimeClock = Nothing
        oPersPunch = Nothing
    End Sub

    Protected Sub lbSignOut_Click(sender As Object, e As System.EventArgs) Handles lbSignOut.Click
        Session("TCID") = ""
        Response.Redirect("TimeClock.aspx")
    End Sub

    Protected Sub lbPO_Click(sender As Object, e As System.EventArgs) Handles lbPO.Click
        Dim oPersClock As New clsPersonnelTimeClock
        If oPersClock.Validate_Clock(Request.ServerVariables("REMOTE_ADDR")) Then
            Dim pID As Integer = 0
            Dim oPersPunch As New clsPersonnelPunch
            pID = oPersPunch.Get_Current_Punch(hfPersID.Value)
            If pID > 0 Then
                oPersPunch.PunchID = pID
                oPersPunch.Load()
                oPersPunch.TimeOut = System.DateTime.Now
                oPersPunch.DateOut = System.DateTime.Now.ToShortDateString
                oPersPunch.OutClockID = oPersClock.Get_Clock_ID(Request.ServerVariables("REMOTE_ADDR")) 'clockID ******GET CLOCK
                oPersPunch.Save()
                lblActivity.Text = oPersPunch.Get_Last_Activity(hfPersID.Value)
                MultiView1.ActiveViewIndex = 1
            Else
                Dim oPers2Dept As New clsPersonnel2Dept
                Dim deptCount As Integer = oPers2Dept.Get_Dept_Count(hfPersID.Value)
                If deptCount = 0 Then
                    Punch_Out(hfPersID.Value, 0)
                ElseIf deptCount = 1 Then
                    Punch_Out(hfPersID.Value, oPers2Dept.Get_Active_Dept(hfPersID.value))
                Else
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/Payroll/DeptSelect.aspx?PersonnelID=" & hfPersID.Value & "&ClockType=ClockOut','win01',350,150);", True)
                End If
                oPers2Dept = Nothing
            End If
            oPersPunch = Nothing
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('You May Not Clock Out From This Machine.');", True)
        End If
    End Sub

    Private Sub Punch_Out(ByVal persID As Integer, ByVal deptID As Integer)
        Dim oPersPunch As New clsPersonnelPunch
        Dim oPersTimeClock As New clsPersonnelTimeClock
        If oPersPunch.Val_Punch_Out(persID, deptID) Then
            Dim oCombo As New clsComboItems
            oPersPunch.PunchID = 0
            oPersPunch.Load()
            oPersPunch.DepartmentID = deptID
            oPersPunch.PersonnelID = persID
            oPersPunch.TimeOut = System.DateTime.Now
            oPersPunch.DateOut = System.DateTime.Now.ToShortDateString
            oPersPunch.OutClockID = oPersTimeClock.Get_Clock_ID(Request.ServerVariables("REMOTE_ADDR")) 'clockID ******Need CLOCK
            oPersPunch.PunchTypeID = oCombo.Lookup_ID("TimeClockPunchType", "Work")
            oPersPunch.Save()
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('You have just punched out without punching in. You will need to submit a missed punch form.');", True)
            lblActivity.Text = oPersPunch.Get_Last_Activity(persID)
            MultiView1.ActiveViewIndex = 1
            oCombo = Nothing
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('You have already clocked out within 1 hour of clocking in. You must clock in first.');", True)
            MultiView1.ActiveViewIndex = 1
        End If
        oPersTimeClock = Nothing
        oPersPunch = Nothing
    End Sub

    Protected Sub lbCIDept_Click(sender As Object, e As System.EventArgs) Handles lbCIDept.Click
        Punch_In(hfPersID.Value, hfDeptID.Value)
    End Sub

    Protected Sub lbCODept_Click(sender As Object, e As System.EventArgs) Handles lbCODept.Click
        Punch_Out(hfPersID.value, hfDeptID.Value)
    End Sub

    Protected Sub lbMissedPunch_Click(sender As Object, e As System.EventArgs) Handles lbMissedPunch.Click
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/Payroll/MissedPunchRequest.aspx?PersonnelID=" & hfPersID.Value & "','win01',350,350);", True)
    End Sub

    Protected Sub lbHours_Click(sender As Object, e As System.EventArgs) Handles lbHours.Click
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/Payroll/ViewHours.aspx?PersonnelID=" & hfPersID.Value & "','win01',650,650);", True)
    End Sub

    Protected Sub Timer1_Tick(sender As Object, e As System.EventArgs) Handles Timer1.Tick
        Response.Redirect("TimeClock.aspx")
    End Sub

    Protected Sub Unnamed3_Click(sender As Object, e As System.EventArgs)
        Response.Redirect("https://crms.kingscreekplantation.com/crmsnet")
    End Sub
End Class
