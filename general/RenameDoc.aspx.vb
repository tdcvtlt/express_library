
Partial Class general_RenameDoc
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oUpload As New clsUploadedDocs
            oUpload.FileID = Request("ID")
            oUpload.Load()
            txtOldName.Text = oUpload.Name
            oUpload = Nothing
        End If
    End Sub

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click
        Dim oUpload As New clsUploadedDocs
        oUpload.FileID = Request("ID")
        oUpload.Load()
        oUpload.UserID = Session("UserDBID")
        oUpload.Name = txtNewName.Text
        oUpload.Save()
        oUpload = Nothing
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.opener.Refresh_Docs();window.close();", True)
    End Sub
End Class
