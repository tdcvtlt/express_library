
Partial Class general_EditNote
    Inherits System.Web.UI.Page
    Dim oNT As New clsNoteTemplates

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Load_Note()
        End If
    End Sub

    Private Sub Load_Note()
        oNT.NoteTemplateID = Request("ID")
        oNT.Load()
        NoteTemplateID.Text = oNT.NoteTemplateID
        Title.Text = oNT.Title
        Active.Checked = oNT.Active
        txtNote.Text = oNT.Note
        oNT = Nothing
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        oNT.NoteTemplateID = Request("ID")
        oNT.Load()
        oNT.Title = Title.Text
        oNT.Active = Active.Checked
        oNT.Note = txtNote.Text
        oNT.UserDBID = Session("UserDBID")
        oNT.UserID = Session("UserDBID")
        oNT.Save()

        Label1.Text = oNT.Error_Message
        If oNT.Error_Message = "" Then
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "Close", "window.opener.__doPostBack('" & Request("linkid") & "','');window.close();", True)
        Else
            Label1.Text = oNT.Error_Message
        End If
        oNT = Nothing
    End Sub
End Class
