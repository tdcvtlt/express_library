
Partial Class setup_Letters_letters
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        GridView1.DataSource = (New clsLetters).List(IIf(filter.Text = "" Or Not (IsNumeric(filter.Text)), 0, filter.Text))
        GridView1.DataBind()
    End Sub

    Protected Sub btnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Response.Redirect("EditLetter.aspx?letterid=0")
    End Sub
End Class
