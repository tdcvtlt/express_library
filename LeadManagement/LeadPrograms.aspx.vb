
Partial Class LeadManagement_LeadPrograms
    Inherits System.Web.UI.Page

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Response.Redirect("EditLeadProgram.aspx?ID=0")
    End Sub

    Protected Sub btnList_Click(sender As Object, e As EventArgs) Handles btnList.Click
        Dim oLP As New clsLeadProgram
        gvLP.DataSource = oLP.List_Programs()
        Dim sKeys(0) As String
        sKeys(0) = "ID"
        gvLP.DataKeyNames = sKeys
        gvLP.DataBind()
        oLP = Nothing
    End Sub

    Protected Sub gvLP_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvLP.SelectedIndexChanged
        Dim row As GridViewRow = gvLP.SelectedRow
        Response.Redirect("EditLeadProgram.aspx?ID=" & row.Cells(1).Text)
    End Sub
End Class
