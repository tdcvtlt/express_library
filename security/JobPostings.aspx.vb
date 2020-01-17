
Partial Class security_JobPostings
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Response.Redirect("EditJobPosting.aspx?JobID=0")
    End Sub

    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Dim oJobs As New clsJobPosting
        gvJobs.DataSource = oJobs.List_Job_Postings(ddActive.SelectedValue)
        Dim sKeys(0) As String
        sKeys(0) = "JobID"
        gvJobs.DataKeyNames = sKeys
        gvJobs.DataBind()
        oJobs = Nothing
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            ddActive.Items.Add(New ListItem("ALL", -1))
            ddActive.Items.Add(New ListItem("Active", 1))
            ddActive.Items.Add(New ListItem("InActive", 0))
        End If
    End Sub

    Protected Sub gvJobs_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvJobs.SelectedIndexChanged
        Dim row As GridViewRow = gvJobs.SelectedRow
        Response.Redirect("EditJobPosting.aspx?JobID=" & row.Cells(1).Text)
    End Sub
End Class
