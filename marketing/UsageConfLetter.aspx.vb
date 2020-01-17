
Partial Class marketing_UsageConfLetter
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim oUsage As New clsUsage
        Literal1.Text = oUsage.Print_Conf_Letter(Request("UsageID"))
        oUsage = Nothing
    End Sub
End Class
