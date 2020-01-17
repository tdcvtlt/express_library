
Partial Class Accounting_vendorFilter
    Inherits System.Web.UI.Page

    Protected Sub Unnamed1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim pRequest As New clsPurchaseRequest
        gvVendors.DataSource = pRequest.List_Vendors(txtVendor.Text)
        Dim sKeys(0) As String
        sKeys(0) = "VENDORID"
        gvVendors.DataKeyNames = sKeys
        gvVendors.DataBind()
    End Sub

    Protected Sub gvVendors_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvVendors.SelectedIndexChanged
        Dim row As GridViewRow = gvVendors.SelectedRow
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.opener.assign_Vendor(" & Chr(34) & Trim(row.Cells(1).Text) & Chr(34) & ");window.close();", True)
    End Sub

    Protected Sub Unnamed2_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("vendorRequest.aspx")
    End Sub
End Class
