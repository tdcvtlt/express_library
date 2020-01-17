
Partial Class setup_Letters_EditLetterTagStyle
    Inherits System.Web.UI.Page

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim ostyle As New clsLetterTagStyles
        ostyle.TagStyleID = txtID.Text
        ostyle.Load()
        ostyle.Style = txtStyle.Text
        ostyle.StartingTag = txtStartingTag.Text
        ostyle.ItemTag = txtItemTag.Text
        ostyle.Save()
        ostyle = Nothing
        Response.Redirect("lettertagstyles.aspx")
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            If Request("id") = "" Or Request("id") = "0" Or Request("id") = 0 Then
                txtID.Text = 0
            Else
                txtID.Text = Request("id")
            End If
            Dim oStyle As New clsLetterTagStyles
            oStyle.TagStyleID = txtID.Text
            oStyle.Load()
            txtStyle.Text = oStyle.Style
            txtStartingTag.Text = oStyle.StartingTag
            txtItemTag.Text = oStyle.ItemTag
            oStyle = Nothing
        End If
    End Sub
End Class
