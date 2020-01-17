
Partial Class Maintenance_PrintWorkOrder
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oRequest As New clsRequest
            litRequest.Text = oRequest.Print_Request(Request("RequestID"))
            oRequest = Nothing
        End If
    End Sub
End Class
