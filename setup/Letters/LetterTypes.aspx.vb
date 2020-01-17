
Partial Class setup_Letters_LetterTypes
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        GridView1.DataSource = (New clsLetters).ListTypes(filter.Text)
        GridView1.DataBind()
    End Sub

    Protected Sub btnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Response.Redirect("EditLetterType.aspx?id=0")
    End Sub
End Class
