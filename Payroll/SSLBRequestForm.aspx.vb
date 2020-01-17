Imports System
Imports System.Data
Partial Class Payroll_SSLBRequestForm
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim oPers2Dept As New clsPersonnel2Dept

            ddEmployees.DataSource = oPers2Dept.Get_MyMgr_Dept_Members(Request("PersonnelID"))
            ddEmployees.DataValueField = "PersonnelID"
            ddEmployees.DataTextField = "Personnel"
            ddEmployees.DataBind()

            ddDept.DataSource = oPers2Dept.Get_My_Depts(Request("PersonnelID"))
            ddDept.DataTextField = "Department"
            ddDept.DataValueField = "DepartmentID"
            ddDept.DataBind()
            lblDate.Text = System.DateTime.Now.ToShortDateString
            oPers2Dept = Nothing

            Dim dt As New DataTable
            dt.Columns.Add("Start Date")
            dt.Columns.Add("End Date")
            dt.Columns.Add("SSLB Hours")
            dt.Columns.Add("Unpaid Hours")

            Session("dtSSLBHours") = dt
            gvDates.DataSource = dt
            gvDates.DataBind()
            Session("SSLBHours") = 0
            Session("UnPaidHours") = 0
            'Session("DeptID") = ddDept.SelectedValue
            'Session("PersonnelID") = ddEmployees.SelectedValue
            lblSSLBHours.Text = Session("SSLBHours")
            lblUnpaidHours.Text = Session("UnPaidHours")
        Else
            gvDates.DataSource = Session("dtSSLBHours")
            gvDates.DataBind()
            lblDate.Text = System.DateTime.Now.ToShortDateString
            'ddDept.SelectedValue = Session("DeptID")
            'ddEmployees.SelectedValue = Session("PersonnelID")
        End If
    End Sub

    Protected Sub lbAddDates_Click(sender As Object, e As System.EventArgs) Handles lbAddDates.Click
        'Session("DeptID") = ddDept.SelectedValue
        'Session("PersonnelID") = ddEmployees.SelectedValue

        If dteSDate.Selected_Date = "" Or dteEDate.Selected_Date = "" Or Not (IsNumeric(CInt(txtSSLBHours.Text))) Or Not (IsNumeric(CInt(txtUnPaidHours.Text))) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "AjaxCall", "alert('Please Make Sure All Fields are Valid.');", True)
        ElseIf DateDiff("d", dteSDate.Selected_Date, dteEDate.Selected_Date) < 0 Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "AjaxCall", "alert('Please Make Sure the End Date is Greater than the Start Date.');", True)
        ElseIf txtSSLBHours.Text <= 0 And txtUnPaidHours.Text <= 0 Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "AjaxCall", "alert('Please Specify the Number of Hours You Wish to Request Off.');", True)
        ElseIf txtSSLBHours.Text > 0 And txtUnPaidHours.Text > 0 Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "AjaxCall", "alert('SSLB and UnPaid Requests Must Be Made Seperately.');", True)
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
                    dt = Session("dtSSLBHours")
                    Dim dr As DataRow
                    Do While DateTime.Compare(tempDate, CDate(dteEDate.Selected_Date)) < 1
                        For i = 0 To cbWeekDays.Items.Count - 1
                            If cbWeekDays.Items(i).Selected And CDate(tempDate).DayOfWeek.ToString = cbWeekDays.Items(i).Value Then
                                dr = dt.NewRow
                                dr("Start Date") = tempDate.ToShortDateString
                                dr("End Date") = tempDate.ToShortDateString
                                dr("SSLB Hours") = txtSSLBHours.Text
                                dr("UnPaid Hours") = txtUnPaidHours.Text
                                dt.Rows.Add(dr)
                                Session("SSLBHours") = CDbl(Session("SSLBHours")) + CDbl(txtSSLBHours.Text)
                                Session("UnPaidHours") = CDbl(Session("UnPaidHours")) + CDbl(txtUnPaidHours.Text)
                            End If
                        Next
                        tempDate = tempDate.AddDays(1)
                    Loop
                    Session("dtSSLBHours") = dt
                    lblSSLBHours.Text = Session("SSLBHours")
                    lblUnpaidHours.Text = Session("UnPaidHours")
                    gvDates.DataSource = dt
                    gvDates.DataBind()
                    dteSDate.Selected_Date = ""
                    dteEDate.Selected_Date = ""
                    txtSSLBHours.Text = 0
                    txtUnPaidHours.Text = 0
                    'ddEmployees.SelectedValue = Session("PersonnelID")
                    'ddDept.SelectedValue = Session("DeptID")
                    For i = 0 To cbWeekDays.Items.Count - 1
                        cbWeekDays.Items(i).Selected = False
                    Next
                Else
                    ScriptManager.RegisterStartupScript(Me, Me.GetType, "AjaxCall", "alert('The Day of the Week of Your Start Date is Not Checked.');", True)
                End If
            End If
        End If
    End Sub

    Protected Sub lbDeleted_Click(sender As Object, e As System.EventArgs)
        Dim row As GridViewRow = CType(CType(sender, Control).Parent.Parent, GridViewRow)
        Dim dt As New DataTable
        Dim dRow As DataRow
        dt = Session("dtSSLBHours")
        dRow = dt.Rows(row.RowIndex)
        Session("SSLBHours") = Session("SSLBHours") - CDbl(dRow("SSLB Hours"))
        Session("UnPaidHours") = Session("UnPaidHours") - CDbl(dRow("UnPaid Hours"))
        dt.Rows.Remove(dRow)
        Session("dtSSLBHours") = dt
        gvDates.DataSource = dt
        gvDates.DataBind()
        lblSSLBHours.Text = Session("SSLBHours")
        lblUnpaidHours.Text = Session("UnPaidHours")
    End Sub

    Protected Sub btnSubmit_Click(sender As Object, e As System.EventArgs) Handles btnSubmit.Click
        If gvDates.Rows.Count < 1 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('Please Add at Least One Date Request.');", True)
        Else
            Dim sslbID As Integer = 0
            Dim oSSLBForm As New clsSSLBRequest
            Dim oSSLBDates As New clsSSLBRequestDates
            Dim oPers2Dept As New clsPersonnel2Dept
            Dim oCombo As New clsComboItems
            Dim bProceed As Boolean = True
            Dim sErr As String = ""
            If oSSLBForm.Check_SSLB_Balance(ddEmployees.SelectedValue) < CDbl(lblSSLBHours.Text) Then
                bProceed = False
                sErr = "You Do Not Have Enough SSLB Hours to Meet this Request. " & ddEmployees.SelectedValue
            ElseIf Not (oPers2Dept.Member_Of(ddEmployees.SelectedValue, oCombo.Lookup_ComboItem(ddDept.SelectedValue))) Then
                bProceed = False
                sErr = "Requested Employee Does Not Belong to Requested Department."
            End If

            If bProceed Then
                oSSLBForm.PersonnelID = ddEmployees.SelectedValue
                oSSLBForm.DepartmentID = ddDept.SelectedValue
                oSSLBForm.DateCreated = System.DateTime.Now
                oSSLBForm.TotalSSLBHours = lblSSLBHours.Text
                oSSLBForm.TotalUnpaidHours = lblUnpaidHours.Text
                oSSLBForm.RequestedByID = Session("UserDBID")
                oSSLBForm.Save()
                sslbID = oSSLBForm.SSLBRequestID
                For Each Row As GridViewRow In gvDates.Rows
                    oSSLBDates.SSLBRequestDateID = 0
                    oSSLBDates.Load()
                    oSSLBDates.SSLBRequestID = sslbID
                    oSSLBDates.StartDate = Row.Cells(1).Text
                    oSSLBDates.EndDate = Row.Cells(2).Text
                    oSSLBDates.SSLBHours = Row.Cells(3).Text
                    oSSLBDates.UnpaidHours = Row.Cells(4).Text
                    oSSLBDates.Save()
                Next
                oSSLBDates = Nothing
                oSSLBForm = Nothing
                Session("dt") = Nothing
                Session("SSLBHours") = Nothing
                Session("UnPaidHours") = Nothing
                Session("DeptID") = Nothing
                ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('SSLB Request Submitted.');window.close();", True)
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('" & sErr & "');", True)
                ddDept.SelectedValue = Session("DeptID")
                ddEmployees.SelectedValue = Session("PersonnelID")
            End If
            oSSLBDates = Nothing
            oSSLBForm = Nothing
            oPers2Dept = Nothing
            oCombo = Nothing
        End If
    End Sub
End Class
