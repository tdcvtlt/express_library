
Partial Class setup_EditScripts
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Set_Values()
        End If
        MultiView1.ActiveViewIndex = 0
    End Sub

    Protected Sub Set_Values()
        Dim oScripts As New clsScripts
        oScripts.ScriptID = Request("ScriptID")
        oScripts.Load()

        txtScript.Text = oScripts.ScriptName
        txtScriptID.Text = oScripts.ScriptID
    End Sub

    Protected Sub Note_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Note.Click
        If txtScriptID.Text > 0 Then
            MultiView1.ActiveViewIndex = 1
            Notes1.KeyField = "ScriptID"
            Notes1.KeyValue = txtScriptID.Text
            Notes1.Display()
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If txtScript.Text <> "" Then
            Dim oScript As New clsScripts
            oScript.ScriptID = txtScriptID.Text
            oScript.ScriptName = txtScript.Text
            oScript.Save()
            Response.Write(txtScript.Text & " " & oScript.ScriptName)
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('Please Enter a Script Name Value');", True)
        End If

    End Sub

    Protected Sub dooodooooo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles dooodooooo.Click
        If txtScriptID.Text > 0 Then
            MultiView1.ActiveViewIndex = 2
            Events1.KeyField = "ScriptID"
            Events1.KeyValue = txtScriptID.Text
            Events1.List()
        End If
    End Sub
End Class
