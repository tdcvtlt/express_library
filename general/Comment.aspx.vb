
Partial Class general_Comment
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oComment As New clsComments
            gvComments.DataSource = oComment.List_Comments(Request("KeyField"), Request("KeyValue"))
            gvComments.DataBind()
            oComment = Nothing
        End If
    End Sub

    Protected Sub Unnamed1_Click1(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("addComments.aspx?KeyField=" & Request("KeyField") & "&KeyValue=" & Request("KeyValue") & "")
    End Sub
End Class
