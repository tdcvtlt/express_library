
Partial Class general_ReDeeds
    Inherits System.Web.UI.Page

    Protected Sub btnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Response.Redirect("~/wizards/contracts/redeedwiz.aspx")
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim oRe As New clsReDeeds
        GridView1.DataSource = oRe.List_Pending
        Dim sKeys(0) As String
        sKeys(0) = "ReDeedID"
        GridView1.DataKeyNames = sKeys
        GridView1.DataBind()
    End Sub

    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged
        Response.Redirect("~/wizards/contracts/redeedwiz.aspx?redeedid=" & GridView1.SelectedValue)
    End Sub
End Class
