
Partial Class setup_Letters_EditView
    Inherits System.Web.UI.Page

    Dim oView As clsLetterViews

    Protected Sub ddView_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddView.SelectedIndexChanged
        ddKeyFieldName.DataSource = (New clsLetterViews).List_View_Columns(ddView.SelectedValue)
        ddKeyFieldName.DataTextField = "COLUMN_NAME"
        ddKeyFieldName.DataValueField = "COLUMN_NAME"
        ddKeyFieldName.DataBind()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request("ViewID") = "" Or Not (IsNumeric(Request("ViewID"))) Then
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Close", "window.close();", True)
            Else
                'Get View Object
                oView = New clsLetterViews
                oView.ViewID = Request("ViewID")
                oView.Load()
                'Fill Types
                siType.Connection_String = Resources.Resource.cns
                siType.ComboItem = "LetterType"
                siType.Load_Items()
                siType.Selected_ID = IIf(oView.TypeID = 0, Request("TypeID"), oView.TypeID)
                'Fill Views
                ddView.DataSource = (New clsLetterViews).List_Views
                ddView.DataTextField = "Name"
                ddView.DataValueField = "Name"
                ddView.DataBind()
                ddView.SelectedValue = oView.View
                ddKeyFieldName.DataSource = (New clsLetterViews).List_View_Columns(oView.View)
                ddKeyFieldName.DataTextField = "COLUMN_NAME"
                ddKeyFieldName.DataValueField = "COLUMN_NAME"
                ddKeyFieldName.DataBind()
                ddKeyFieldName.SelectedValue = oView.KeyField
                ckSource.Checked = oView.Source
                oView = Nothing
            End If

        End If

    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        oView = New clsLetterViews
        oView.ViewID = Request("ViewID")
        oView.Load()
        oView.View = ddView.SelectedValue
        oView.Source = ckSource.Checked
        oView.KeyField = ddKeyFieldName.SelectedValue
        oView.TypeID = siType.Selected_ID
        oView.Save()

        'If oView.Error_Message = "" Then
        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "Close", "window.opener.__doPostBack('" & Request("linkid") & "','');window.close();", True)
        'Else
        '    Label1.Text = oNote.Error_Message
        'End If
    End Sub
End Class
