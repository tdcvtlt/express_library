
Partial Class setup_Accommodations
    Inherits System.Web.UI.Page

    Protected Sub btnNew_Click(sender As Object, e As System.EventArgs) Handles btnNew.Click
        Response.Redirect("EditAccommodation.aspx?AccommodationID=0")
    End Sub

    Protected Sub btnSearch_Click(sender As Object, e As System.EventArgs) Handles btnSearch.Click
        Dim oAccom As New clsAccom
        gvAccommodations.DataSource = oAccom.Search_Accoms(txtFilter.Text)
        Dim sKeys(0) As String
        sKeys(0) = "ID"
        gvAccommodations.DataKeyNames = sKeys
        gvAccommodations.DataBind()
        oAccom = Nothing
    End Sub

    Protected Sub gvAccommodations_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvAccommodations.SelectedIndexChanged
        Dim row As GridViewRow = gvAccommodations.SelectedRow
        Response.Redirect("EditAccommodation.aspx?AccommodationID=" & row.Cells(1).Text)
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If IsPostBack Then
            btnSearch_Click(sender, e)
        End If
    End Sub
End Class
