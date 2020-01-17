
Partial Class Reports_Sales_Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Server.MapPath(Request("report")) <> "" Then
            Dim ta() As String
            ta = Split(Session("Groups"), "|")
            'If ta.Contains("Tour Reports") Then
            If InStr(LCase((Request("report"))), "imported") > 0 Then
                Response.Redirect("default2.aspx?report=" & Request("report"))
            Else
                Response.Redirect(Request("report"))
            End If
            'Else
            '    Response.Write("Access Denied")
            'End If
        Else
            Response.Write("This Report cannot be found.")
        End If
    End Sub
End Class
