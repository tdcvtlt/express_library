
Partial Class marketing_tours
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        lblErr.text = ""
        If CheckSecurity("Tours", "List", , , Session("UserDBID")) Then
            Dim oTours As New clsTour
            gvTours.DataSource = oTours.List(50, IIf(filter.Text <> "", "TourID", ""), filter.Text, "TourID")
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvTours.DataKeyNames = sKeys
            gvTours.DataBind()
            oTours = Nothing
        Else
            lblErr.Text = "ACCESS DENIED"
        End If
    End Sub

    Protected Sub gvTours_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvTours.SelectedIndexChanged
        Response.Redirect("edittour.aspx?tourid=" & gvTours.SelectedValue)
    End Sub
    Protected Sub gvTours_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Tours" Then
            If e.Row.RowIndex > -1 Then
                If e.Row.Cells(2).Text <> "&nbsp;" Then
                    e.Row.Cells(2).Text = CDate(e.Row.Cells(2).Text).ToShortDateString
                End If
            End If
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then
            Button1_Click(sender, e)
        End If
    End Sub

End Class
