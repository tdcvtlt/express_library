
Partial Class general_addComment
    Inherits System.Web.UI.Page
    Dim oComment As New clsComments

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Load_Comment()
            If Request("CommentID") > 0 And Not (oComment.CreatedByID = CInt(Session("UserDBID")) And Math.Abs(DateDiff(DateInterval.Hour, Date.Now, CDate(IIf(IsDate(oComment.CreatedDate), oComment.CreatedDate, Date.Now)))) < 24) Then
                txtComment.ReadOnly = True
                btnSave.Enabled = False
            End If
            ' Response.Write(oNote.CreatedByID & ":" & Session("UserDBID") & "<br />" & Math.Abs(DateDiff(DateInterval.Hour, Date.Now, CDate(oNote.DateCreated))))
        End If
    End Sub

    Private Sub Load_Comment()
        oComment.CommentID = Request("CommentID")
        oComment.Load()

        txtComment.Text = oComment.Comment
        txtUser.Text = oComment.CreatedBy
        txtDate.Text = oComment.CreatedDate
        Label1.Text = oComment.Error_Message
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        oComment.CommentID = Request("CommentID")
        oComment.Comment = txtComment.Text
        oComment.CreatedByID = Session("UserDBID")
        oComment.KeyField = Request("KeyField")
        oComment.KeyValue = Request("KeyValue")
        oComment.CreatedDate = Date.Now
        oComment.Save()
        
        Label1.Text = oComment.Error_Message
        If oComment.Error_Message = "" Then
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "Close", "window.opener.__doPostBack('" & Request("linkid") & "','');window.close();", True)
            'ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "Close", "window.opener.Refresh_Notes();window.close();", True)
        End If
        oComment = Nothing
    End Sub
End Class
