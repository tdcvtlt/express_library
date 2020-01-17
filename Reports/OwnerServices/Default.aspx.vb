
Partial Class Reports_OwnerServices_Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Server.MapPath(Request("report")) <> "" Then
            Dim ta() As String
            ta = Split(Session("Groups"), "|")
            'If ta.Contains("Owner Services Reports") Then
            Response.Redirect(Request("report"))
            'Else
            '    Response.Write("Access Denied")
            'End If
        Else
            Response.Write("This Report cannot be found.")
        End If
    End Sub
End Class
