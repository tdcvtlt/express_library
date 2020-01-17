
Partial Class setup_Letters_LetterPreview
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim oLetters As New clsLetters
        litPreview.Text = oLetters.Get_Letter_Text(Request("ID"), Request("Key"))
        oLetters = Nothing
    End Sub
End Class
