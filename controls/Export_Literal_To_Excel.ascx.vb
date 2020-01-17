
Partial Class controls_Export_Literal_To_Excel
    Inherits System.Web.UI.UserControl

    Dim _Lit1 As Literal

    Protected Sub btnExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport.Click
        If _Lit1 Is Nothing Then Exit Sub
        If _Lit1.Text <> "" Then
            Response.Clear()
            Response.ContentType = "application/vnd.ms-excel"
            Response.AddHeader("Content-Disposition", "attachment; filename=premiums.xls")
            Response.Write(_Lit1.Text)
            Response.End()
        End If
    End Sub

    Public Property Literal As Literal
        Get
            Return _Lit1
        End Get
        Set(ByVal value As Literal)
            _Lit1 = value
        End Set
    End Property




    Public Event Click As EventHandler

    Protected Sub OnClick(ByVal sender As Object, ByVal e As System.EventArgs)

        RaiseEvent Click(Me, EventArgs.Empty)


    End Sub
End Class
