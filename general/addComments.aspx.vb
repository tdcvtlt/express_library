
Partial Class general_addComments
    Inherits System.Web.UI.Page

    Protected Sub Unnamed1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim oComments As New clsComments
        oComments.CommentID = 0
        oComments.Load()
        oComments.Comment = txtNote.Text
        oComments.KeyField = Request("KeyField")
        oComments.KeyValue = Request("KeyValue")
        oComments.CreatedByID = Session("UserDBID")
        oComments.CreatedDate = System.DateTime.Now
        oComments.Save()
        If Request("RunJS") = "NO" Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.close();", True)
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.opener.Refresh_Rpt();window.close();", True)
        End If
        oComments = Nothing
    End Sub
End Class
