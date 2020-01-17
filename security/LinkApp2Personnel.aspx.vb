
Partial Class security_LinkApp2Personnel
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oJobApp As New clsJobPostingApplication
            Dim oApp As New clsJobPostingApplicant
            Dim oPers As New clsPersonnel
            oJobApp.ApplicationID = Request("ID")
            oJobApp.Load()
            oApp.ApplicantID = oJobApp.ApplicantID
            oApp.Load()
            gvPersonnel.DataSource = oPers.Lookup_By_FullName(oApp.FirstName, oApp.LastName)
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvPersonnel.DataKeyNames = sKeys
            gvPersonnel.DataBind()
            oPers = Nothing
            oApp = Nothing
            oJobApp = Nothing
        End If
    End Sub

    Protected Sub gvPersonnel_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvPersonnel.SelectedIndexChanged
        Dim row As GridViewRow = gvPersonnel.SelectedRow
        Dim oDocs As New clsUploadedDocs
        Dim oCombo As New clsComboItems
        Dim oApp As New clsJobPostingApplication
        Dim oUser As New clsJobPostingApplicant
        Dim oPers As New clsPersonnel
        Dim oJob As New clsJobPosting
        Dim oPers2Dept As New clsPersonnel2Dept
        Dim persID As Integer = 0

        oApp.ApplicationID = Request("ID")
        oApp.Load()
        oUser.ApplicantID = oApp.ApplicantID
        oUser.Load()
        oPers.PersonnelID = 0
        oPers.Load()
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
        oDocs.Copy_Docs("JobPostingApplicationHRID", Request("ID"), "PersonnelID", row.Cells(1).Text)
        oDocs.Copy_Docs("JobPostingApplicationID", Request("ID"), "PersonnelID", row.Cells(1).Text)
        oApp.UserID = Session("UserDBID")
        oApp.ApplicationID = Request("ID")
        oApp.Load()
        oApp.StatusID = oCombo.Lookup_ID("JobApplicationStatus", "Re-Hire")
        oApp.Save()
        oApp = Nothing
        oDocs = Nothing
        oCombo = Nothing
        oPers = Nothing
        oPers2Dept = Nothing
        oJob = Nothing
        oUser = Nothing
        ClientScript.RegisterClientScriptBlock(Me.GetType, "Refresh", "window.opener.Refresh_App();window.close();", True)
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If IsNumeric(txtID.Text) And txtID.Text > 0 Then
            Dim oPers As New clsPersonnel
            gvPersonnel.DataSource = oPers.Lookup_By_ID(txtID.Text)
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvPersonnel.DataKeyNames = sKeys
            gvPersonnel.DataBind()
            oPers = Nothing
        End If
    End Sub
End Class
