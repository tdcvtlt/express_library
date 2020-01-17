
Partial Class marketing_editUsageOwner
    Inherits System.Web.UI.Page

    Protected Sub Unnamed1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim oPros As New clsProspect
        Dim filter() As String
        If InStr(filterTxt.Text, ",") <> 0 Then
            filter = filterTxt.Text.Split(",")
            gvOwners.DataSource = oPros.List_Owners(filter(0), Trim(filter(1)))
        Else
            gvOwners.DataSource = oPros.List_Owners(filterTxt.Text, "")
        End If
        Dim sKeys(0) As String
        sKeys(0) = "ProspectID"
        gvOwners.DataKeynames = sKeys
        gvOwners.DataBind()
    End Sub
End Class
