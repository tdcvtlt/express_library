
Partial Class security_EditJobApplicant
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oApp As New clsJobPostingApplicant
            oApp.ApplicantID = Request("ID")
            oApp.Load()
            lbFirst.Text = oApp.FirstName
            lbLast.Text = oApp.LastName
            lbPhone.Text = oApp.HomePhone
            lbMobile.Text = oApp.MobilePhone
            oApp = Nothing
            MultiView1.ActiveViewIndex = 0
        End If
    End Sub

    Protected Sub Description_Link_Click(sender As Object, e As EventArgs) Handles Description_Link.Click
        If Request("ID") > 0 Then
            Dim oApp As New clsJobPostingApplication
            gvApplications.DataSource = oApp.List_Apps_By_User(Request("ID"))
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvApplications.DataKeyNames = sKeys
            gvApplications.DataBind()
            oApp = Nothing
            MultiView1.ActiveViewIndex = 1
        End If
    End Sub

    Protected Sub Applicants_Link_Click(sender As Object, e As EventArgs) Handles Applicants_Link.Click
        If Request("ID") > 0 Then
            MultiView1.ActiveViewIndex = 2
            Notes1.KeyField = "JobApplicantID"
            Notes1.KeyValue = Request("ID")
            Notes1.Display()
        End If
    End Sub

    Protected Sub Job_Link_Click(sender As Object, e As EventArgs) Handles Job_Link.Click
        MultiView1.ActiveViewIndex = 0
    End Sub

    Protected Sub gvApplications_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvApplications.SelectedIndexChanged
        Dim row As GridViewRow = gvApplications.SelectedRow
        Response.Redirect("EditJobApplication.aspx?AppID=" & row.Cells(1).Text)
    End Sub
End Class
