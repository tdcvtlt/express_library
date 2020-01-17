Imports System
Imports System.Data
Partial Class Payroll_PALRequestForm
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim oPersonnel As New clsPersonnel
            Dim oPers2Dept As New clsPersonnel2Dept
            oPersonnel.PersonnelID = Request("PersonnelID")
            oPersonnel.Load()
            lblEmployee.Text = oPersonnel.FirstName & " " & oPersonnel.LastName
            oPersonnel = Nothing
            ddDept.DataSource = oPers2Dept.Get_My_Depts(Request("PersonnelID"))
            ddDept.DataTextField = "Department"
            ddDept.DataValueField = "DepartmentID"
            ddDept.DataBind()
            lblDate.Text = System.DateTime.Now.ToShortDateString
            oPers2Dept = Nothing
            oPersonnel = Nothing

            Dim dt As New DataTable
            dt.Columns.Add("Start Date")
            dt.Columns.Add("End Date")
            dt.Columns.Add("PAL Hours")
            dt.Columns.Add("Unpaid Hours")

            Session("dtPALHours") = dt
            gvDates.DataSource = dt
            gvDates.DataBind()
            Session("PALHours") = 0
            Session("UnPaidHours") = 0
            Session("DeptID") = ddDept.SelectedValue
            lblPALHours.Text = Session("PALHours")
            lblUnpaidHours.Text = Session("UnPaidHours")
        Else
            gvDates.DataSource = Session("dtPalHours")
            gvDates.DataBind()
            lblDate.Text = System.DateTime.Now.ToShortDateString
            ddDept.SelectedValue = Session("DeptID")
        End If
    End Sub

    Protected Sub lbAddDates_Click(sender As Object, e As System.EventArgs) Handles lbAddDates.Click
        If dteSDate.Selected_Date = "" Or dteEDate.Selected_Date = "" Or Not (IsNumeric(CInt(txtPALHours.Text))) Or Not (IsNumeric(CInt(txtUnPaidHours.Text))) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "AjaxCall", "alert('Please Make Sure All Fields are Valid.');", True)
        ElseIf DateDiff("d", dteSDate.Selected_Date, dteEDate.Selected_Date) < 0 Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "AjaxCall", "alert('Please Make Sure the End Date is Greater than the Start Date.');", True)
        ElseIf txtPALHours.Text <= 0 And txtUnPaidHours.Text <= 0 Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "AjaxCall", "alert('Please Specify the Number of Hours You Wish to Request Off.');", True)
        ElseIf txtPALHours.Text > 0 And txtUnPaidHours.Text > 0 Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "AjaxCall", "alert('PAL and UnPaid Requests Must Be Made Seperately.');", True)
        Else
            Dim i As Integer = 0
            Dim dayChk As Integer = 0
            For i = 0 To cbWeekDays.Items.Count - 1
                If cbWeekDays.Items(i).Selected = True Then
                    dayChk += 1
                End If
            Next
            If dayChk > 5 Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "AjaxCall", "alert('You May Only Select 5 Week Days.');", True)
            ElseIf dayChk = 0 Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType, "AjaxCall", "alert('You Must Select a Day Of the Week.');", True)
            Else
                Dim bProceed As Boolean = False
                Dim strtDay As Integer = CDate(dteSDate.Selected_Date).DayOfWeek
                For i = 0 To cbWeekDays.Items.Count - 1
                    If cbWeekDays.Items(i).Selected = True Then
                        If cbWeekDays.Items(i).Value = CDate(dteSDate.Selected_Date).DayOfWeek.ToString Then
                            bProceed = True
                            Exit For
                        End If
                    End If
                Next

                If bProceed Then
                    Dim tempDate As Date = CDate(dteSDate.Selected_Date)
                    Dim dt As DataTable
                    dt = Session("dtPalHours")
                    Dim dr As DataRow
                    Do While DateTime.Compare(tempDate, CDate(dteEDate.Selected_Date)) < 1
                        For i = 0 To cbWeekDays.Items.Count - 1
                            If cbWeekDays.Items(i).Selected And CDate(tempDate).DayOfWeek.ToString = cbWeekDays.Items(i).Value Then
                                dr = dt.NewRow
                                dr("Start Date") = tempDate.ToShortDateString
                                dr("End Date") = tempDate.ToShortDateString
                                dr("PAL Hours") = txtPALHours.Text
                                dr("UnPaid Hours") = txtUnPaidHours.Text
                                dt.Rows.Add(dr)
                                Session("PALHours") = CDbl(Session("PALHours")) + CDbl(txtPALHours.Text)
                                Session("UnPaidHours") = CDbl(Session("UnPaidHours")) + CDbl(txtUnPaidHours.Text)
                            End If
                        Next
                        tempDate = tempDate.AddDays(1)
                    Loop
                    Session("dtPalHours") = dt
                    lblPALHours.Text = Session("PALHours")
                    lblUnpaidHours.Text = Session("UnPaidHours")
                    gvDates.DataSource = dt
                    gvDates.DataBind()
                    dteSDate.Selected_Date = ""
                    dteEDate.Selected_Date = ""
                    txtPALHours.Text = 0
                    txtUnPaidHours.Text = 0
                    For i = 0 To cbWeekDays.Items.Count - 1
                        cbWeekDays.Items(i).Selected = False
                    Next
                Else
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "AjaxCall", "alert('The Day of the Week of Your Start Date is Not Checked.');", True)
                End If
            End If
        End If
    End Sub

    Protected Sub btnSubmit_Click(sender As Object, e As System.EventArgs) Handles btnSubmit.Click
        If gvDates.Rows.Count < 1 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('Please Add at Least One Date Request.');", True)
        Else
            Dim palID As Integer = 0
            Dim oPALForm As New clsPALRequest
            Dim oPALDates As New clsPALRequestDates
            Dim bProceed As Boolean = True
            Dim sErr As String = ""
            'If oPALForm.Check_PAL_Balance(Request("PersonnelID")) < CDbl(lblPALHours.Text) Then
            'bProceed = False
            'sErr = "You Do Not Have Enough PAL Hours to Meet this Request."
            'ElseIf oPALForm.Check_SSLB_Balance(Request("PersonnelID")) < CDbl(lblSSLBHours.Text) Then
            ' bProceed = False
            'sErr = "You Do Not Have Enough SSLB Hours to Meet this Request."
            'End If
            If bProceed Then
                oPALForm.PersonnelID = Request("PersonnelID")
                oPALForm.DepartmentID = ddDept.SelectedValue
                oPALForm.DateCreated = System.DateTime.Now
                oPALForm.TotalPALHours = lblPALHours.Text
                oPALForm.TotalUnpaidHours = lblUnpaidHours.Text
                oPALForm.Scheduled = True
                oPALForm.Save()
                palID = oPALForm.PALRequestID
                For Each Row As GridViewRow In gvDates.Rows
                    oPALDates.PALRequestDateID = 0
                    oPALDates.Load()
                    oPALDates.PALRequestID = palID
                    oPALDates.StartDate = Row.Cells(1).Text
                    oPALDates.EndDate = Row.Cells(2).Text
                    oPALDates.PALHours = Row.Cells(3).Text
                    oPALDates.UnpaidHours = Row.Cells(4).Text
                    oPALDates.Save()
                Next
                oPALDates = Nothing
                oPALForm = Nothing
                Session("dt") = Nothing
                Session("PALHours") = Nothing
                Session("UnPaidHours") = Nothing
                Session("DeptID") = Nothing
                ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('PAL Request Submitted.');window.close();", True)
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('" & sErr & "');", True)
            End If
            oPALDates = Nothing
            oPALForm = Nothing
        End If
    End Sub



    Protected Sub lbDeleted_Click(sender As Object, e As System.EventArgs)
        Dim row As GridViewRow = CType(CType(sender, Control).Parent.Parent, GridViewRow)
        Dim dt As New DataTable
        Dim dRow As DataRow
        dt = Session("dtPalHours")
        dRow = dt.Rows(row.RowIndex)
        Session("PALHours") = Session("PALHours") - CDbl(dRow("PAL Hours"))
        Session("UnPaidHours") = Session("UnPaidHours") - CDbl(dRow("UnPaid Hours"))
        dt.Rows.Remove(dRow)
        Session("dtPalHours") = dt
        gvDates.DataSource = dt
        gvDates.DataBind()
        lblPALHours.Text = Session("PALHours")
        lblUnpaidHours.Text = Session("UnPaidHours")
    End Sub
End Class
