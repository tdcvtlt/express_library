
Partial Class setup_ReservationLetters
    Inherits System.Web.UI.Page

    Protected Sub btnSearch_Click(sender As Object, e As System.EventArgs) Handles btnSearch.Click
        Dim oResLetter As New clsReservationLetters
        gvLetters.DataSource = oResLetter.Search_Letters(txtName.Text)
        Dim sKeys(0) As String
        sKeys(0) = "ID"
        gvLetters.DataKeyNames = sKeys
        gvLetters.DataBind()
        oResLetter = Nothing
    End Sub

    Protected Sub btnNew_Click(sender As Object, e As System.EventArgs) Handles btnNew.Click
        Response.Redirect("EditReservationLetter.aspx?ID=0")
    End Sub

    Protected Sub gvLetters_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvLetters.SelectedIndexChanged
        Dim row As GridViewRow = gvLetters.SelectedRow
        Response.Redirect("EditReservationLetter.aspx?ID=" & row.Cells(1).Text)
    End Sub
End Class
