
Partial Class setup_PremiumBundles
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click
        Dim oPremBundles As New clsPremiumBundles
        gvBundles.DataSource = oPremBundles.List_Premium_Bundles(ddFilter.SelectedValue, filter.Text)
        Dim sKeys(0) As String
        sKeys(0) = "PremiumBundleID"
        gvBundles.DataKeyNames = sKeys
        gvBundles.DataBind()
        oPremBundles = Nothing
    End Sub

    Protected Sub gvBundles_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvBundles.SelectedIndexChanged
        Dim row As GridViewRow = gvBundles.SelectedRow
        Response.Redirect("EditPremiumBundle.aspx?PremiumBundleID=" & row.Cells(1).Text)
    End Sub

    Protected Sub btnNew_Click(sender As Object, e As System.EventArgs) Handles btnNew.Click
        Response.Redirect("EditPremiumBundle.aspx?PremiumBundleID=0")
    End Sub
End Class
