
Partial Class PropertyManagement_Projects_EditArea
    Inherits System.Web.UI.Page

    Private Sub lbSave_Click(sender As Object, e As EventArgs) Handles lbSave.Click
        Dim oa As New clsProjectAreas
        oa.AreaID = txtAreaID.Text
        oa.Load()
        oa.Area = txtArea.Text
        oa.Description = txtDescription.Text
        oa.Save()
        oa = Nothing
        Response.Redirect("areas.aspx")
    End Sub

    Private Sub PropertyManagement_Projects_EditArea_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            txtAreaID.Text = Request("ID")
            If txtAreaID.Text = "" Or Not (IsNumeric(txtAreaID.Text)) Then txtAreaID.Text = 0
            Dim oA As New clsProjectAreas
            oA.AreaID = Request("ID")
            oA.Load()
            txtArea.Text = oA.Area
            txtDescription.Text = oA.Description
            oA = Nothing
        End If
    End Sub

End Class
