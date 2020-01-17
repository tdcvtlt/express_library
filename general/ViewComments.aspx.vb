
Partial Class general_ViewComments
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Me.Notes1.KeyField = Request("KeyField")
            Me.Notes1.KeyValue = Request("KeyValue")
            Me.Notes1.Display()
        End If
    End Sub
End Class
