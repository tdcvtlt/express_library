
Partial Class general_EditVoiceStamp
    Inherits System.Web.UI.Page
    Dim ovStamp As New clsVoiceStamps

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            If Request("VSID") = 0 Then
                If CheckSecurity("Voicestamps", "Add", , , Session("UserDBID")) Then
                    Load_VS()
                Else
                    Label1.Text = "ACCESS DENIED"
                End If
            Else
                If CheckSecurity("Voicestamps", "Edit", , , Session("UserDBID")) Then
                    Load_VS()
                Else
                    Label1.Text = "ACCESS DENIED"
                End If
            End If
        End If
    End Sub

    Private Sub Load_VS()
        ovStamp.VSID = Request("VSID")
        ovStamp.Load()
        txtVSNumber.Text = ovStamp.VSNumber
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        ovStamp.VSID = Request("VSID")
        ovStamp.VSNumber = txtVSNumber.Text
        ovStamp.KeyField = Request("KeyField")
        ovStamp.KeyValue = Request("KeyValue")
        ovStamp.Save()
        Label1.Text = ovStamp.Err
        If ovStamp.Err = "" Then
            'ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "Close", "window.opener.__doPostBack('" & Request("linkid") & "','');window.close();", True)
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "Close", "window.opener.Refresh_VS();window.close();", True)
        End If
    End Sub
End Class
