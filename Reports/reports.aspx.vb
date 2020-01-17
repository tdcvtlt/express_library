
Partial Class Reports_reports
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("Vendors") = "0" Then
            Dim oReports As New clsReports
            gvReports.DataSource = oReports.List_Group(Request("c"))
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvReports.DataKeyNames = sKeys
            gvReports.DataBind()
            oReports = Nothing
        End If
    End Sub

    Protected Sub gvReports_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvReports.SelectedIndexChanged
        Response.Redirect(Request("c") & "/default.aspx?report=" & gvReports.SelectedRow.Cells(4).Text)

    End Sub
End Class
