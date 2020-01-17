
Partial Class setup_RateTables
    Inherits System.Web.UI.Page

    Protected Sub btnSearch_Click(sender As Object, e As System.EventArgs) Handles btnSearch.Click
        Dim oRateTable As New clsRateTable
        gvRateTables.DataSource = oRateTable.Search_Tables(txtFilter.Text)
        Dim sKeys(0) As String
        sKeys(0) = "ID"
        gvRateTables.DataKeyNames = sKeys
        gvRateTables.DataBind()
        oRateTable = Nothing
    End Sub


    Protected Sub gvRateTables_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvRateTables.SelectedIndexChanged
        Dim row As GridViewRow = gvRateTables.SelectedRow
        Response.Redirect("EditRateTable.aspx?ID=" & row.Cells(1).Text)

    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If IsPostBack Then
            btnSearch_Click(sender, e)
        End If
    End Sub

    Protected Sub btnNew_Click(sender As Object, e As System.EventArgs) Handles btnNew.Click
        Response.Redirect("EditRateTable.aspx?ID=0")
    End Sub
End Class
