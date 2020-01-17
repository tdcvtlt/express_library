
Partial Class marketing_mortgages
    Inherits System.Web.UI.Page

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim oMort As New clsMortgage
        gvMortgages.DataSource = oMort.Query(100, ddFilter.SelectedValue, txtFilter.Text, "")
        gvMortgages.DataBind()
        oMort = Nothing
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then
            btnSearch_Click(sender, e)
        End If
    End Sub
End Class
