Imports Microsoft.VisualBasic
Partial Class marketing_OverviewsAuto
    Inherits System.Web.UI.Page

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        Dim oOV As New clsCombinedOverviewAuto
        Dim sDate As String
        If dteOverView.Selected_Date = "" Then
            sDate = ""
        Else
            sDate = CDate(dteOverView.Selected_Date).ToString
        End If
        gvOverViews.dataSource = oOV.List(sDate, ddLocation.SelectedValue)
        gvOverViews.DataBind()
        oOV = Nothing
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub gvOverViews_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvOverViews.SelectedIndexChanged
        Dim row As gridViewrow = gvOverViews.SelectedRow
        Response.Redirect("CombinedOverViewAuto.aspx?overviewid=" & row.Cells(1).Text)
    End Sub

    Protected Sub gvOverViews_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then
            If e.Row.RowIndex > -1 Then
                e.Row.Cells(2).Text = CDate(e.Row.Cells(2).Text).ToShortDateString
            End If
            e.Row.Cells(1).Visible = False
        End If
    End Sub
End Class
