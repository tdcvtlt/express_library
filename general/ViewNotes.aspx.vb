
Partial Class general_ViewNotes
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim oNotes As New clsNotes
            oNotes.KeyField = Request("KeyField")
            oNotes.KeyValue = Request("KeyValue")
            gvNotes.DataSource = oNotes.Get_Notes_Table()
            gvNotes.DataBind()
            oNotes = Nothing
        End If
    End Sub

    Protected Sub gvNotes_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then
            'If e.Row.RowIndex > -1 Then
            e.Row.Cells(3).Visible = False
                e.Row.Cells(5).Visible = False
            'End If
        End If
    End Sub
End Class
