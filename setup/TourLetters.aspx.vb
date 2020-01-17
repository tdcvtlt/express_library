
Partial Class setup_TourLetters
    Inherits System.Web.UI.Page


    Protected Sub btnSearch_Click(sender As Object, e As System.EventArgs) Handles btnSearch.Click

        Dim oTourLetter As New clsTourLetters()

        gvLetters.DataSource = oTourLetter.Search_Letters(txtName.Text)
        Dim sKeys(0) As String
        sKeys(0) = "ID"
        gvLetters.DataKeyNames = sKeys
        gvLetters.DataBind()        
    End Sub

    Protected Sub btnNew_Click(sender As Object, e As System.EventArgs) Handles btnNew.Click
        Response.Redirect("EditTourLetter.aspx?ID=0")
    End Sub

    Protected Sub gvLetters_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvLetters.SelectedIndexChanged
        Dim row As GridViewRow = gvLetters.SelectedRow
        Response.Redirect("EditTourLetter.aspx?ID=" & row.Cells(1).Text)
    End Sub
End Class
