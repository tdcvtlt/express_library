
Partial Class setup_PackageLetters
    Inherits System.Web.UI.Page

    Protected Sub btnSearch_Click(sender As Object, e As System.EventArgs) Handles btnSearch.Click
        Dim oPkgLetter As New clsPackageLetters
        gvLetters.DataSource = oPkgLetter.Search_Letters(txtName.Text)
        Dim sKeys(0) As String
        sKeys(0) = "PackageLetterID"
        gvLetters.DataKeyNames = sKeys
        gvLetters.DataBind()
        oPkgLetter = Nothing
    End Sub

    Protected Sub btnNew_Click(sender As Object, e As System.EventArgs) Handles btnNew.Click
        Response.Redirect("editPackageLetter.aspx?LetterID=0")
    End Sub

    Protected Sub gvLetters_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvLetters.SelectedIndexChanged
        Dim row As GridViewRow = gvLetters.SelectedRow
        Response.Redirect("EditPackageLetter.aspx?LetterID=" & row.Cells(1).Text)
    End Sub
End Class
