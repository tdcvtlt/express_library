
Imports System.Net.Mail
Partial Class mis_editworkorder
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Load_Selects()
            If Request("WorkOrderID") <> 0 Then
                Load_Values()
                If IsNumeric(Request("WorkOrderID")) Then
                    If CInt(Request("WorkOrderID")) > 0 Then
                        Dim oE As New clsEvents
                        Dim sErr As String = ""

                        If Not (oE.Find_View_Event("WorkOrderID", Request("WorkOrderID"), Resources.Resource.ViewEventTime, Session("UserDBID"), sErr)) Then
                            If sErr <> "" Then Response.Write(sErr)
                            If Not (oE.Create_View_Event("WorkOrderID", Request("WorkOrderID"), Resources.Resource.ViewEventTime, Session("UserDBID"), sErr)) Then
                                If sErr <> "" Then Response.Write(sErr)
                            End If
                        End If
                        oE = Nothing
                    End If
                End If
            End If
            MultiView1.ActiveViewIndex = 0
        End If

        If Not Me.Request.UrlReferrer Is Nothing Then
            Me.Back.Visible = True
            Me.Back.NavigateUrl = "workorders.aspx" 'Me.Request.UrlReferrer.ToString
        Else
            Me.Back.Visible = False
        End If


    End Sub

    Private Sub Load_Values()
        Dim oWorkOrder As New clsWorkOrder
        Dim oPers As New clsPersonnel
        Dim oPers2Dept As New clsPersonnel2Dept
        Dim oCombo As New clsComboItems
        oWorkOrder.WorkOrderID = Request("WorkOrderID")
        oWorkOrder.Load()
        oPers.PersonnelID = oWorkOrder.RequestedByID
        oPers.Load()
        lblID.Text = oWorkOrder.WorkOrderID
        ddDepartment.Items.Add(New ListItem(oCombo.Lookup_ComboItem(oWorkOrder.DepartmentID), oWorkOrder.DepartmentID))
        lblRequestedBy.Text = oPers.FirstName + " " + oPers.LastName
        Table1.Rows(0).Visible = True
        lblDateRequested.Text = oWorkOrder.DateEntered
        Table1.Rows(1).Visible = True
        Location.Selected_ID = oWorkOrder.LocationID
        SUbLocation.Selected_ID = oWorkOrder.SubLocationID
        PriorityLevel.Selected_ID = oWorkOrder.PriorityLevelID
        ddType.SelectedValue = oWorkOrder.RequestTypeID
        txtSubject.Text = oWorkOrder.Subject
        dteReqDueDate.Selected_Date = oWorkOrder.ReqDueDate
        siResponsibleParty.Selected_ID = oWorkOrder.ResponsiblePartyID
        txtDescription.Text = oWorkOrder.Description
        Status.Selected_ID = oWorkOrder.StatusID
        Select Case ddType.SelectedItem.Text
            Case "Change"
                txtOldValue.Text = oWorkOrder.OldValue
                txtNewValue.Text = oWorkOrder.NewValue
                For i = 15 To 23
                    If i = 15 Or i = 16 Then
                        Table1.Rows(i).Visible = True
                    Else
                        Table1.Rows(i).Visible = False
                    End If
                Next
                lblDesc.Text = "Description:"
            Case "New Installation"
                siInstallationType.Selected_ID = oWorkOrder.InstallationTypeID
                txtEstCost.Text = oWorkOrder.Cost
                For i = 15 To 23
                    If i = 17 Or i = 18 Then
                        Table1.Rows(i).Visible = True
                    Else
                        Table1.Rows(i).Visible = False
                    End If
                Next
                lblDesc.Text = "Description:"
            Case "New User"
                txtLName.Text = oWorkOrder.LastName
                txtFname.Text = oWorkOrder.FirstName
                ddSupervisor.SelectedValue = oWorkOrder.SupervisorID
                For i = 15 To 23
                    If i = 19 Or i = 20 Or i = 21 Then
                        Table1.Rows(i).Visible = True
                    Else
                        Table1.Rows(i).Visible = False
                    End If
                Next
                lblDesc.Text = "User Needs:"
            Case "Problem"
                siProblemArea.Selected_ID = oWorkOrder.ProblemAreaID
                For i = 15 To 23
                    If i = 22 Then
                        Table1.Rows(i).Visible = True
                    Else
                        Table1.Rows(i).Visible = False
                    End If
                Next
                lblDesc.Text = "Description:"
            Case "Work Stoppage"
                txtPhone.Text = oWorkOrder.PhoneNumber
                For i = 15 To 23
                    If i = 23 Then
                        Table1.Rows(i).Visible = True
                    Else
                        Table1.Rows(i).Visible = False
                    End If
                Next
                lblDesc.Text = "Description:"
        End Select

        If oWorkOrder.Approved = 1 And oPers2Dept.Member_Of_WOParty(oWorkOrder.ResponsiblePartyID, Session("UserDBID")) Then
            ddAssignedTo.SelectedValue = oWorkOrder.AssignedToID
            Status.Selected_ID = oWorkOrder.StatusID
            dteAssignDueDate.Selected_Date = oWorkOrder.AssignDueDate
            For i = 10 To 14
                Table1.Rows(i).Visible = True
            Next
        End If

        If oWorkOrder.Approved = 0 And oPers2Dept.Manager_Check(Session("UserDBID"), ddDepartment.SelectedValue) Then
            btnSave.Visible = False
            btnApprove.Visible = True
            btnDeny.Visible = True
        End If
        oWorkOrder = Nothing
        oPers2Dept = Nothing
        oPers = Nothing
        oCombo = Nothing
    End Sub

    Private Sub Load_Selects()
        Location.Connection_String = Resources.Resource.cns
        Location.ComboItem = "Location"
        Location.Label_Caption = ""
        Location.Load_Items()

        SubLocation.Connection_String = Resources.Resource.cns
        SubLocation.ComboItem = "SubLocation"
        SubLocation.Label_Caption = ""
        SubLocation.Load_Items()

        PriorityLevel.Connection_String = Resources.Resource.cns
        PriorityLevel.ComboItem = "PriorityLevel"
        PriorityLevel.Label_Caption = ""
        PriorityLevel.Load_Items()

        Status.Connection_String = Resources.Resource.cns
        Status.ComboItem = "ITWorkOrderStatus"
        Status.Label_Caption = ""
        Status.Load_Items()

        siResponsibleParty.Connection_String = Resources.Resource.cns
        siResponsibleParty.COmboItem = "WorkOrderResponsibleParty"
        siResponsibleParty.Label_Caption = ""
        siResponsibleParty.Load_Items()

        siProblemArea.Connection_String = Resources.Resource.cns
        siProblemArea.ComboItem = "ITProblemArea"
        siProblemArea.Label_Caption = ""
        siProblemArea.Load_Items()

        siInstallationType.Connection_String = Resources.Resource.cns
        siInstallationType.ComboItem = "InstallationType"
        siInstallationType.Label_Caption = ""
        siInstallationType.Load_Items()

        Dim oCombo As New clsComboItems
        ddType.DataSource = oCombo.Load_ComboItems("RequestType")
        ddType.DataTextField = "ComboItem"
        ddType.DataValueField = "ComboItemID"
        ddType.DataBind()
        oCombo = Nothing

        Dim oPers2Dept As New clsPersonnel2Dept
        If Request("WorkOrderID") = 0 Then
            ddDepartment.DataSource = oPers2Dept.Get_My_Depts(Session("UserDBID"))
            ddDepartment.DataTextField = "Department"
            ddDepartment.DataValueField = "DepartmentID"
            ddDepartment.DataBind()
        End If

        ddAssignedTo.Items.Add(New ListItem("", 0))
        ddAssignedTo.DataSource = oPers2Dept.Get_My_Dept_Members(Session("UserDBID"))
        ddAssignedTo.DataTextField = "Personnel"
        ddAssignedTo.DataValueField = "PersonnelID"
        ddAssignedTo.AppendDataBoundItems = True
        ddAssignedTo.DataBind()

        ddSupervisor.DataSource = oPers2Dept.Get_My_Supervisors(Session("UserDBID"))
        ddSupervisor.DataTextField = "Personnel"
        ddSupervisor.DataValueField = "PersonnelID"
        ddSupervisor.DataBind()
        oPers2Dept = Nothing
    End Sub

    Protected Sub ddType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddType.SelectedIndexChanged
        Select Case ddType.SelectedItem.Text
            Case "Adjustment"
                For i = 15 To 23
                    Table1.Rows(i).Visible = False
                Next
            Case "Change"
                For i = 15 To 23
                    If i = 16 Or i = 17 Then
                        Table1.Rows(i).Visible = True
                    Else
                        Table1.Rows(i).Visible = False
                    End If
                Next
                lblDesc.Text = "Description:"
            Case "New Installation"
                For i = 15 To 23
                    If i = 18 Or i = 19 Then
                        Table1.Rows(i).Visible = True
                    Else
                        Table1.Rows(i).Visible = False
                    End If
                Next
                lblDesc.Text = "Description:"
            Case "New User"
                For i = 15 To 23
                    If i = 20 Or i = 21 Or i = 22 Then
                        Table1.Rows(i).Visible = True
                    Else
                        Table1.Rows(i).Visible = False
                    End If
                Next
                lblDesc.Text = "User Needs:"
            Case "Problem"
                For i = 15 To 23
                    If i = 23 Then
                        Table1.Rows(i).Visible = True
                    Else
                        Table1.Rows(i).Visible = False
                    End If
                Next
                lblDesc.Text = "Description:"
            Case "Project"
                For i = 15 To 23
                    Table1.Rows(i).Visible = False
                Next
                lblDesc.Text = "Description:"
            Case "Report"
                For i = 15 To 23
                    Table1.Rows(i).Visible = False
                Next
                lblDesc.Text = "Report Request:"
            Case "Work Stoppage"
                For i = 15 To 24
                    If i = 24 Then
                        Table1.Rows(i).Visible = True
                    Else
                        Table1.Rows(i).Visible = False
                    End If
                Next
                lblDesc.Text = "Description:"
            Case Else
                For i = 15 To 23
                    Table1.Rows(i).Visible = False
                Next
                lblDesc.Text = "Description:"
        End Select
    End Sub

    Protected Sub lbWorkOrder_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbWorkOrder.Click
        MultiView1.ActiveViewIndex = 0
    End Sub

    Protected Sub lbNotes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbNotes.Click
        If Request("WorkOrderID") > 0 Then
            Notes1.KeyValue = Request("WOrkOrderID")
            Notes1.Display()
            MultiView1.ActiveViewIndex = 1
        End If
    End Sub

    Protected Sub lbEvents_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbEvents.Click
        If Request("WorkOrderID") > 0 Then
            ucEvents.KeyField = "WorkOrderID"
            ucEvents.KeyValue = Request("WorkOrderID")
            ucEvents.List()
            MultiView1.ActiveViewIndex = 3
        End If
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs)
        If siResponsibleParty.Selected_ID < 1 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Please Select a Responsible Party.');", True)
        Else
            Dim oWorkorder As New clsWorkOrder
            Dim oPers As New clsPersonnel
            Dim oPers2Dept As New clsPersonnel2Dept
            Dim oCombo As New clsComboItems
            oWorkorder.WorkOrderID = Request("WorkOrderID")
            oWorkorder.Load()
            If Request("WorkOrderID") = 0 Then
                oWorkorder.RequestedByID = Session("UserDBID")
                oWorkorder.DateEntered = System.DateTime.Now
                oWorkorder.StatusID = oCombo.Lookup_ID("ITWorkOrderStatus", "Not Started")
            End If
            oWorkorder.DepartmentID = ddDepartment.SelectedValue
            oWorkorder.LocationID = Location.Selected_ID
            oWorkorder.SubLocationID = SubLocation.Selected_ID
            oWorkorder.PriorityLevelID = PriorityLevel.Selected_ID
            oWorkorder.RequestTypeID = ddType.SelectedValue
            oWorkorder.Subject = txtSubject.Text
            oWorkorder.ReqDueDate = dteReqDueDate.Selected_Date
            oWorkorder.ResponsiblePartyID = siResponsibleParty.Selected_ID
            oWorkorder.Description = txtDescription.Text
            oWorkorder.UserID = Session("UserDBID")
            Select Case ddType.SelectedItem.Text
                Case "Change"
                    oWorkorder.OldValue = txtOldValue.Text
                    oWorkorder.NewValue = txtNewValue.Text
                Case "New Installation"
                    oWorkorder.InstallationTypeID = siInstallationType.Selected_ID
                    oWorkorder.Cost = txtEstCost.Text
                Case "New User"
                    oWorkorder.LastName = txtLName.Text
                    oWorkorder.FirstName = txtFName.Text
                    oWorkorder.SupervisorID = ddSupervisor.SelectedValue
                Case "Problem"
                    oWorkorder.ProblemAreaID = siProblemArea.Selected_ID
                Case "Work Stoppage"
                    oWorkorder.PhoneNumber = txtPhone.Text
            End Select
            Dim emailTo As String = ""
            Dim emailFrom As String = ""
            Dim body As String = ""
            Dim bRespParty As Boolean = False
            Dim bApproval As Boolean = False
            Dim bManagers As Boolean = False
            Dim bAssignedTo As Boolean = False
            Dim bSenderReq As Boolean = False
            Dim bNewReq As Boolean = False
            If Request("WorkOrderID") = 0 Then
                If oPers2Dept.Manager_Check(Session("UserDBID"), ddDepartment.SelectedValue) Then
                    oWorkorder.Approved = 1
                    oWorkorder.ApprovedBy = Session("UserDBID")
                    bRespParty = True
                    bApproval = True
                Else
                    bManagers = True
                    bSenderReq = True
                    bNewReq = True
                    oWorkorder.Approved = 0
                End If
            Else
                If oWorkorder.Approved = 1 Then
                    oWorkorder.StatusID = Status.Selected_ID
                    oWorkorder.AssignDueDate = dteAssignDueDate.Selected_Date
                    If oWorkorder.AssignedToID <> ddAssignedTo.SelectedValue Then
                        bAssignedTo = True
                    End If
                    oWorkorder.AssignedToID = ddAssignedTo.SelectedValue
                    bSenderReq = True
                    bRespParty = True
                Else
                    bManagers = True
                    bSenderReq = True
                End If
            End If
            oWorkorder.Save()
            body = Build_Body(oWorkorder.WorkOrderID)
            oPers.PersonnelID = Session("UserDBID")
            oPers.Load()
            emailFrom = oPers.Email
            If bRespParty = True Then
                emailTo = oWorkorder.Get_Resp_Party_Email(oWorkorder.ResponsiblePartyID)
                If bApproval Then
                    Send_Mail(emailTo, emailFrom, "Work Order Approved", body)
                Else
                    Send_Mail(emailTo, emailFrom, "Work Order", body)
                End If
                If ddType.SelectedItem.Text = "Work Stoppage" Then
                    'Send_Mail("7578176334@vtext.com", emailFrom, "Work Stoppage", Left(txtDescription.Text, 100))
                    Send_Mail("7578176334@vtext.com", emailFrom, "Work Stoppage", Left(txtDescription.Text, 160 - Len("7578176334@vtext.com") - Len(emailFrom) - Len("Work Stoppage")))
                    Send_Mail("7572545316@vtext.com", emailFrom, "Work Stoppage", Left(txtDescription.Text, 160 - Len("7572545316@vtext.com") - Len(emailFrom) - Len("Work Stoppage")))
                End If
            End If
            If bManagers = True Then
                Dim mgrEmails() As String
                mgrEmails = oPers2Dept.Get_Manager_Emails(oWorkorder.DepartmentID)
                If Not (mgrEmails(0) = "") Then
                    For i = 0 To UBound(mgrEmails)
                        If bNewReq = True Then
                            Send_Mail(mgrEmails(i), emailFrom, "Work Order Requested For Approval", body)
                        Else
                            Send_Mail(mgrEmails(i), emailFrom, "Requested Work Order Change", body)
                        End If
                    Next
                End If
            End If
            If bAssignedTo = True Then
                oPers.PersonnelID = oWorkorder.AssignedToID
                oPers.Load()
                emailTo = oPers.Email
                Send_Mail(emailTo, emailFrom, "Work Order Assigned To You", body)
            End If
            oPers.PersonnelID = oWorkorder.RequestedByID
            oPers.Load()
            emailTo = oPers.Email
            Send_Mail(emailTo, emailFrom, "Work Order", body)
            oWorkorder = Nothing
            oPers = Nothing
            oPers2Dept = Nothing
            Response.Redirect("workorders.aspx")
        End If
    End Sub

    Protected Sub btnApprove_Click(sender As Object, e As System.EventArgs)
        Dim oWorkOrder As New clsWorkOrder
        Dim oCombo As New clsComboItems
        oWorkOrder.WorkOrderID = Request("WorkOrderID")
        oWorkOrder.Load()
        oWorkOrder.Approved = 1
        oWorkOrder.DateApproved = Now
        oWorkOrder.ApprovedBy = Session("UserDBID")
        oWorkOrder.StatusID = oCombo.Lookup_ID("ITWorkOrderStatus", "Not Started")
        oWorkOrder.LocationID = Location.Selected_ID
        oWorkOrder.SubLocationID = SubLocation.Selected_ID
        oWorkOrder.PriorityLevelID = PriorityLevel.Selected_ID
        oWorkOrder.RequestTypeID = ddType.SelectedValue
        oWorkOrder.Subject = txtSubject.Text
        oWorkOrder.ReqDueDate = dteReqDueDate.Selected_Date
        oWorkOrder.ResponsiblePartyID = siResponsibleParty.Selected_ID
        oWorkOrder.Description = txtDescription.Text
        Select Case ddType.SelectedItem.Text
            Case "Change"
                oWorkOrder.OldValue = txtOldValue.Text
                oWorkOrder.NewValue = txtNewValue.Text
            Case "New Installation"
                oWorkOrder.InstallationTypeID = siInstallationType.Selected_ID
                oWorkOrder.Cost = txtEstCost.Text
            Case "New User"
                oWorkOrder.LastName = txtLName.Text
                oWorkOrder.FirstName = txtFname.Text
                oWorkOrder.SupervisorID = ddSupervisor.SelectedValue
            Case "Problem"
                oWorkOrder.ProblemAreaID = siProblemArea.Selected_ID
            Case "Work Stoppage"
                oWorkOrder.PhoneNumber = txtPhone.Text
        End Select
        oWorkOrder.UserID = Session("UserDBID")
        oWorkOrder.Save()
        'Send Email to Responsible Party
        Dim oPers As New clsPersonnel
        Dim emailFrom As String = ""
        Dim emailTo As String = ""
        Dim body As String = Build_Body(oWorkOrder.WorkOrderID)
        oPers.PersonnelID = Session("UserDBID")
        oPers.Load()
        emailFrom = oPers.Email
        emailTo = oWorkOrder.Get_Resp_Party_Email(siResponsibleParty.Selected_ID)
        Send_Mail(emailTo, emailFrom, "Work Order Approved", body)
        'Send Email to Person who entered
        oPers.PersonnelID = oWorkOrder.RequestedByID
        oPers.Load()
        emailTo = oPers.Email
        Send_Mail(emailTo, emailFrom, "Work Order Approved", body)
        If ddType.SelectedItem.Text = "Work Stoppage" Then
            Send_Mail("7578176334@vtext.com", emailFrom, "Work Stoppage", Left(txtDescription.Text, 160 - Len("7578176334@vtext.com") - Len(emailFrom) - Len("Work Stoppage")))
            Send_Mail("7572545316@vtext.com", emailFrom, "Work Stoppage", Left(txtDescription.Text, 160 - Len("7572545316@vtext.com") - Len(emailFrom) - Len("Work Stoppage")))
        End If
        oPers = Nothing
        oCombo = Nothing
        oWorkOrder = Nothing
    End Sub

    Protected Sub btnDeny_Click(sender As Object, e As System.EventArgs)
        Dim oWorkOrder As New clsWorkOrder
        Dim oCombo As New clsComboItems
        oWorkOrder.WorkOrderID = Request("WorkOrderID")
        oWorkOrder.Load()
        oWorkOrder.StatusID = oCombo.Lookup_ID("ITWorkOrderStatus", "Cancelled")
        oWorkOrder.Approved = -1
        oWorkOrder.ApprovedBy = Session("UserDBID")
        oWorkOrder.UserID = Session("UserDBID")
        oWorkOrder.Save()
        oCombo = Nothing
        'SEND DENY EMAIL
        Dim opers As New clsPersonnel
        Dim emailFrom As String = ""
        Dim emailTo As String = ""
        opers.PersonnelID = Session("UserDBID")
        opers.Load()
        emailFrom = opers.Email
        opers.PersonnelID = oWorkOrder.RequestedByID
        opers.Load()
        emailTo = opers.Email
        Send_Mail(emailTo, emailFrom, "Work Order Denied", Build_Body(oWorkOrder.WorkOrderID))
        opers = Nothing
        oWorkOrder = Nothing
    End Sub

    Function Build_Body(ByVal WOID As Integer) As String
        Dim body As String = ""
        Dim oWorkOrder As New clsWorkOrder
        Dim oPers As New clsPersonnel
        Dim oCombo As New clsComboItems
        oWorkOrder.WorkOrderID = WOID
        oWorkOrder.Load()
        body = "<html><H2>" & oCombo.Lookup_ComboItem(oWorkOrder.RequestTypeID) & "</H2>"
        body = body & "<br>"
        body = body & "<table>"
        body = body & "<tr><td><B>Requested By:</B></td>"
        oPers.PersonnelID = oWorkOrder.RequestedByID
        oPers.Load()
        body = body & "<td><i>" & oPers.FirstName & " " & oPers.LastName & "</i></td></tr>"
        body = body & "<tr><td><B>Work Order ID:</B></td><td><i>" & oWorkOrder.WorkOrderID & "</i></td></tr>"
        body = body & "<tr><td><B>Location:</B></td><td><i>" & oCombo.Lookup_ComboItem(oWorkOrder.LocationID) & "</i></td></tr>"
        body = body & "<tr><td><B>Sub Location:</B></td><td><i>" & oCombo.Lookup_ComboItem(oWorkOrder.SubLocationID) & "</i></td></tr>"
        body = body & "<tr><td><B>Department:</B></td><td><i>" & oCombo.Lookup_ComboItem(oWorkOrder.DepartmentID) & "</i></td></tr>"
        body = body & "<tr><td><B>Subject:</B></td><td><i>" & oWorkOrder.Subject & "</i></td></tr>"
        body = body & "<tr><td><B>Status:</B></td><td><i>" & oCombo.Lookup_ComboItem(oWorkOrder.StatusID) & "</i></td></tr>"
        If oWorkOrder.AssignedToID > 0 Then
            body = body & "<tr><td><B>Assigned To:</B></td>"
            oPers.PersonnelID = oWorkOrder.RequestedByID
            oPers.Load()
            body = body & "<td><i>" & oPers.FirstName & " " & oPers.LastName & "<i></td></tr>"
        End If
        Select Case oCombo.Lookup_ComboItem(oWorkOrder.RequestTypeID)
            Case "Change"
                body = body & "<tr><td><B>Old Value:</B></td><td><i>" & oWorkOrder.OldValue & "</i></td></tr>"
                body = body & "<tr><td><B>New Value:</B></td><td><i>" & oWorkOrder.NewValue & "</i></td></tr>"
                body = body & "<tr><td><B>Explanation:</b></td></tr>"
            Case "New User"
                body = body & "<tr><td><B>Employee Name:</B></td><td><i>" & oWorkOrder.FirstName & " " & oWorkOrder.LastName & "</i></td></tr>"
                oPers.PersonnelID = oWorkOrder.SupervisorID
                oPers.Load()
                body = body & "<tr><td><B>Supervisor:</B></td><td><i>" & oPers.FirstName & " " & oPers.LastName & "</i></td></tr>"
                body = body & "<tr><td><B>User Needs:</b></td></tr>"
            Case "New Installation"
                body = body & "<tr><td><B>Installation Type:</b></td><td><i>" & oCombo.Lookup_ComboItem(oWorkOrder.InstallationTypeID) & "<i></td></tr>"
                body = body & "<tr><td><B>Estimated Cost:</b></td><td><i>" & oWorkOrder.Cost & "<i></td></tr>"
                body = body & "<tr><td><B>Description:</b></td></tr>"
            Case "Problem"
                body = body & "<tr><td><B>Problem Area:</b></td><td><i>" & oCombo.Lookup_ComboItem(oWorkOrder.ProblemAreaID) & "<i></td></tr>"
                body = body & "<tr><td><B>Description:</b></td></tr>"
            Case "Report"
                body = body & "<tr><td><B>Report Request:</b></td></tr>"
            Case "Work Stoppage"
                body = body & "<tr><td><b>Phone Number:</b></td><td><i>" & oWorkOrder.PhoneNumber & "</i></td></tr>"
                body = body & "<tr><td><B>Description:</b></td></tr>"
            Case Else
                body = body & "<tr><td><B>Description:</b></td></tr>"
        End Select
        body = body & "</table>"
        body = body & "<i>" & oWorkOrder.Description & "</i><br>"
        body = body & "<B>Notes:</B><br>"
        Dim oNotes As New clsNotes
        body = body & oNotes.Get_Body_Notes("WorkOrderID", oWorkOrder.WorkOrderID)
        body = body & "</html>"
        oNotes = Nothing
        oPers = Nothing
        oWorkOrder = Nothing
        oCombo = Nothing
        Return body
    End Function

    Protected Sub lbDocuments_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbDocuments.Click
        If lblID.Text <> "0" And lblID.Text <> "" Then
            MultiView1.ActiveViewIndex = 2
            UploadedDocs1.KeyField = "WorkOrderID"
            UploadedDocs1.KeyValue = lblID.Text
            UploadedDocs1.List()
        End If
    End Sub
End Class
