
Partial Class Accounting_MaintenanceFeeCodes
    Inherits System.Web.UI.Page

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If CheckSecurity("MaintenanceFeeCodes", "List", , , Session("UserDBID")) Then
            Dim res As New clsMaintenanceFeeCodes
            gvCodes.DataSource = res.Search(txtFilter.Text, ddfilter.SelectedValue)
            gvCodes.DataBind()
            res = Nothing
        Else
            lblErr.Text = "ACESS DENIED"
        End If
    End Sub

    Protected Sub gvCodes_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvCodes.SelectedIndexChanged
        Dim row As GridViewRow = gvCodes.SelectedRow
        Response.Redirect("editmaintenancefeecode.aspx?maintenancefeecodeid=" & row.Cells(1).Text)
    End Sub
End Class
