
Partial Class security_EditJobPosting
    Inherits System.Web.UI.Page

    Protected Sub lbSaveJob_Click(sender As Object, e As EventArgs) Handles lbSaveJob.Click
        Dim oJob As New clsJobPosting
        oJob.JobID = txtID.Text
        oJob.Load()
        oJob.UserID = Session("UserDBID")
        oJob.Title = txtTitle.Text
        oJob.CompanyID = siCompany.Selected_ID
        oJob.DepartmentID = siDepartment.Selected_ID
        oJob.Positions = ddPositions.SelectedValue
        oJob.WebSiteID = siWebSite.Selected_ID
        oJob.StatusID = siStatus.Selected_ID
        oJob.TypeID = siType.Selected_ID
        oJob.Active = cbActive.Checked
        oJob.Summary = txtSummary.Text
        oJob.Save()
        'Response.Write(oJob.Err)
        Response.Redirect("EditJobPosting.aspx?JobID=" & oJob.JobID)
        oJob = Nothing
    End Sub


    Protected Sub Description_Link_Click(sender As Object, e As EventArgs) Handles Description_Link.Click
        If txtID.Text > 0 Then
            Dim oJob As New clsJobPosting
            oJob.JobID = txtID.Text
            oJob.Load()
            CKEditor1.Text = oJob.Description
            oJob = Nothing
            MultiView1.ActiveViewIndex = 1
        End If
    End Sub

    Protected Sub Job_Link_Click(sender As Object, e As EventArgs) Handles Job_Link.Click
        MultiView1.ActiveViewIndex = 0
    End Sub

    Protected Sub lbSaveDesc_Click(sender As Object, e As EventArgs) Handles lbSaveDesc.Click
        Dim oJob As New clsJobPosting
        oJob.JobID = txtID.Text
        oJob.Load()
        oJob.UserID = Session("UserDBID")
        oJob.Description = CKEditor1.Text
        oJob.Save()
        'Response.Write(oJob.Err)
        oJob = Nothing
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            siCompany.ComboItem = "JobPostingCompany"
            siCompany.Connection_String = Resources.Resource.cns
            siCompany.Label_Caption = ""
            siCompany.Load_Items()

            siDepartment.ComboItem = "Department"
            siDepartment.Connection_String = Resources.Resource.cns
            siDepartment.Label_Caption = ""
            siDepartment.Load_Items()

            siWebSite.ComboItem = "JobPostingWebSite"
            siWebSite.Connection_String = Resources.Resource.cns
            siWebSite.Label_Caption = ""
            siWebSite.Load_Items()

            siStatus.ComboItem = "JobPostingStatus"
            siStatus.Connection_String = Resources.Resource.cns
            siStatus.Label_Caption = ""
            siStatus.Load_Items()

            siType.ComboItem = "JobPostingType"
            siType.Connection_String = Resources.Resource.cns
            siType.Label_Caption = ""
            siType.Load_Items()


            For i = 1 To 10
                ddPositions.Items.Add(New ListItem(i, i))
            Next

            Dim oJob As New clsJobPosting
            oJob.JobID = Request("JobID")
            oJob.Load()
            txtID.Text = oJob.JobID
            txtSummary.Text = oJob.Summary
            cbActive.Checked = oJob.Active
            txtTitle.Text = oJob.Title
            siCompany.Selected_ID = oJob.CompanyID
            siWebSite.Selected_ID = oJob.WebSiteID
            siStatus.Selected_ID = oJob.StatusID
            siType.Selected_ID = oJob.TypeID
            siDepartment.Selected_ID = oJob.DepartmentID
            ddPositions.SelectedValue = oJob.Positions
            oJob = Nothing
            MultiView1.ActiveViewIndex = 0
        End If
    End Sub

    Protected Sub Applicants_Link_Click(sender As Object, e As EventArgs) Handles Applicants_Link.Click
        If txtID.Text > 0 Then
            Dim oApp As New clsJobPostingApplication
            gvApplications.DataSource = oApp.List_Apps_By_Job(txtID.Text)
            Dim sKeys(0) As String
            sKeys(0) = "ApplicationID"
            gvApplications.DataKeyNames = sKeys
            gvApplications.DataBind()
            oApp = Nothing
            MultiView1.ActiveViewIndex = 2
        End If
    End Sub

    Protected Sub gvApplications_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvApplications.SelectedIndexChanged
        Dim row As GridViewRow = gvApplications.SelectedRow
        Response.Redirect("EditJobApplication.aspx?AppID=" & row.Cells(1).Text)
    End Sub

    Protected Sub Notes_Link_Click(sender As Object, e As EventArgs) Handles Notes_Link.Click
        If txtID.Text > 0 Then
            MultiView1.ActiveViewIndex = 3
            Notes1.KeyField = "JobID"
            Notes1.KeyValue = txtID.Text
            Notes1.Display()
        End If
    End Sub

    Protected Sub Events_Link_Click(sender As Object, e As EventArgs) Handles Events_Link.Click
        If txtID.Text > 0 Then
            MultiView1.ActiveViewIndex = 4
            Events1.KeyField = "JobID"
            Events1.KeyValue = txtID.Text
            Events1.List()
        End If
    End Sub
End Class
