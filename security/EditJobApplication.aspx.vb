
Partial Class security_EditJobApplication
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            siStatus.ComboItem = "JobApplicationStatus"
            siStatus.Connection_String = Resources.Resource.cns
            siStatus.Label_Caption = ""
            siStatus.Load_Items()

            Dim oApp As New clsJobPostingApplication
            Dim oUser As New clsJobPostingApplicant
            Dim oJob As New clsJobPosting
            oApp.ApplicationID = Request("AppID")
            oApp.Load()
            ddJobPosting.DataSource = oJob.List_Jobs(oApp.JobID)
            ddJobPosting.DataTextField = "Title"
            ddJobPosting.DataValueField = "JobID"
            ddJobPosting.DataBind()
            txtID.Text = oApp.ApplicationID
            siStatus.Selected_ID = oApp.StatusID
            ddJobPosting.SelectedValue = oApp.JobID
            oUser.ApplicantID = oApp.ApplicantID
            oUser.Load()
            lbUser.Text = oUser.FirstName & " " & oUser.LastName
            oJob.JobID = oApp.JobID
            oJob.Load()
            lbJob.Text = oJob.Title
            oUser = Nothing
            oJob = Nothing
            oApp = Nothing
            MultiView1.ActiveViewIndex = 0
        End If
    End Sub

    Protected Sub lbJob_Click(sender As Object, e As EventArgs) Handles lbJob.Click
        Dim oJob As New clsJobPostingApplication
        oJob.ApplicationID = txtID.Text
        oJob.Load()
        Response.Redirect("EditJobPosting.aspx?JobID=" & oJob.JobID)
        oJob = Nothing
    End Sub

    Protected Sub lbUser_Click(sender As Object, e As EventArgs) Handles lbUser.Click
        Dim oJob As New clsJobPostingApplication
        oJob.ApplicationID = txtID.Text
        oJob.Load()
        Response.Redirect("EditJobApplicant.aspx?ID=" & oJob.ApplicantID)
        oJob = Nothing
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim oApp As New clsJobPostingApplication
        Dim oCombo As New clsComboItems
        oApp.ApplicationID = txtID.Text
        oApp.Load()
        oApp.UserID = Session("UserDBID")
        If oCombo.Lookup_ComboItem(siStatus.Selected_ID) = "Hired" And oCombo.Lookup_ComboItem(oApp.StatusID) <> "Hired" And oCombo.Lookup_ComboItem(oApp.StatusID) <> "Re-Hire" Then
            Create_PersonnelRecord(oApp.ApplicationID)
            oApp.JobID = ddJobPosting.SelectedValue
            oApp.StatusID = siStatus.Selected_ID
            oApp.Save()
            Response.Redirect("EditJobApplication.aspx?AppID=" & txtID.Text)
        ElseIf oCombo.Lookup_ComboItem(siStatus.Selected_ID) = "Re-Hire" And oCombo.Lookup_ComboItem(oApp.StatusID) <> "Re-Hire" And oCombo.Lookup_ComboItem(oApp.StatusID) <> "Hired" Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/security/LinkApp2Personnel.aspx?ID=" & txtID.Text & "','win01',400,400);", True)
        Else
            oApp.JobID = ddJobPosting.SelectedValue
            oApp.StatusID = siStatus.Selected_ID
            oApp.Save()
            Response.Redirect("EditJobApplication.aspx?AppID=" & txtID.Text)
        End If


        oApp = Nothing
        oCombo = Nothing

    End Sub

    Protected Sub Description_Link_Click(sender As Object, e As EventArgs) Handles Description_Link.Click
        If txtID.Text > 0 Then
            If CheckSecurity("JobPosting", "ViewHRDocs", , , Session("UserDBID")) Then
                MultiView1.ActiveViewIndex = 1
                UploadedDocs1.KeyField = "JobPostingApplicationHRID"
                UploadedDocs1.KeyValue = txtID.Text
                UploadedDocs1.List()
            End If
        End If
    End Sub

    Protected Sub Applicants_Link_Click(sender As Object, e As EventArgs) Handles Applicants_Link.Click
        If txtID.Text > 0 Then
            MultiView1.ActiveViewIndex = 3
            Notes1.KeyField = "JobApplication"
            Notes1.KeyValue = txtID.Text
            Notes1.Display()
            'lblNotesError.Text = Notes1.Error_Message
        End If
    End Sub

    Protected Sub Job_Link_Click(sender As Object, e As EventArgs) Handles Job_Link.Click
        MultiView1.ActiveViewIndex = 0
    End Sub

    Private Sub Create_PersonnelRecord(ByVal appID As Integer)
        Dim oApp As New clsJobPostingApplication
        Dim oUser As New clsJobPostingApplicant
        Dim oJob As New clsJobPosting
        Dim oPers As New clsPersonnel
        Dim oPers2Dept As New clsPersonnel2Dept
        Dim oCombo As New clsComboItems
        Dim persID As Integer = 0
        Dim oDocs As New clsUploadedDocs

        oApp.ApplicationID = appID
        oApp.Load()
        oUser.ApplicantID = oApp.ApplicantID
        oUser.Load()
        oPers.PersonnelID = 0
        oPers.UserID = Session("UserDBID")
        oPers.Load()
        oPers.FirstName = oUser.FirstName
        oPers.LastName = oUser.LastName
        oPers.BirthDate = oUser.DOB
        oPers.SSN = oUser.SSN
        oPers.Save()
        persID = oPers.PersonnelID
        oJob.JobID = oApp.JobID
        oJob.Load()
        oPers2Dept.Personnel2Dept = 0
        oPers2Dept.Load()
        oPers2Dept.PersonnelID = persID
        oPers2Dept.DepartmentID = oJob.DepartmentID
        oPers2Dept.CompanyID = oCombo.Lookup_ID("PayrollCompany", oCombo.Lookup_ComboItem(oJob.CompanyID))
        oPers2Dept.Save()

        oDocs.UserID = Session("UserDBID")
        oDocs.Copy_Docs("JobPostingApplicationHRID", appID, "PersonnelID", persID)
        oDocs.Copy_Docs("JobPostingApplicationID", appID, "PersonnelID", persID)
        oApp.StatusID = siStatus.Selected_ID
        oApp.UserID = Session("UserDBID")
        oApp.Save()
        oPers = Nothing
        oPers2Dept = Nothing
        oJob = Nothing
        oCombo = Nothing
        oApp = Nothing
        oUser = Nothing
        oDocs = Nothing
    End Sub

    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click
        If txtID.Text > 0 Then
            MultiView1.ActiveViewIndex = 2
            UploadedDocs2.KeyField = "JobPostingApplicationID"
            UploadedDocs2.KeyValue = txtID.Text
            UploadedDocs2.List()
        End If
    End Sub

    Protected Sub btnRelease_Click(sender As Object, e As EventArgs) Handles btnRelease.Click
        If txtID.Text > 0 Then
            If CheckSecurity("JobPosting", "ReleaseApplication", , , Session("UserDBID")) Then
                Dim oApp As New clsJobPostingApplication
                oApp.UserID = Session("UserDBID")
                oApp.ApplicationID = txtID.Text
                oApp.Load()
                oApp.Completed = False
                oApp.Save()
                oApp = Nothing
                ClientScript.RegisterClientScriptBlock(Me.GetType, "pop", "alert('Application Released.');", True)
                'Response.Redirect("EditJobApplication.aspx?AppID=" & txtID.Text)
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "pop", "alert('Access Denied.');", True)
            End If
        End If
    End Sub
End Class
