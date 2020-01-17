
Partial Class PropertyManagement_Projects_AddArea
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            ddAreas.DataSource = (New clsProjectAreas).List
            ddAreas.DataValueField = "AreaID"
            ddAreas.DataTextField = "Area"
            ddAreas.DataBind()
        End If
    End Sub

    Protected Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Dim opa As New clsProject2Area
        opa.Project2Area = 0
        opa.Load()
        opa.AreaID = ddAreas.SelectedValue
        opa.ProjectID = Request("ID")
        opa.Save()
        opa = Nothing
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "Refresh", "window.opener.Refresh_Areas();", True)
        Close()
    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Close()
    End Sub

    Private Sub Close()
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "Close", "window.close();", True)
    End Sub
End Class
