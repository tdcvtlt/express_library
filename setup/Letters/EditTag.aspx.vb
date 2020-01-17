
Partial Class setup_Letters_EditTag
    Inherits System.Web.UI.Page

    Dim oTag As clsLetterTags

    Protected Sub ddView_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddView.SelectedIndexChanged
        Fill_Fields()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request("TagID") = "" Or Not (IsNumeric(Request("TagID"))) Then
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Close", "window.close();", True)
            Else
                oTag = New clsLetterTags
                oTag.TagID = Request("TAGID")
                oTag.Load()
                'Fill Types
                txtTagID.Text = oTag.TagID
                txtTag.Text = oTag.Tag
                txtDescription.Text = oTag.Description
                siType.Connection_String = Resources.Resource.cns
                siType.ComboItem = "LetterType"
                siType.Load_Items()
                siType.Selected_ID = IIf(oTag.TypeID = 0, Request("TypeID"), oTag.TypeID)
                ddStyle.DataSource = (New clsLetterTagStyles).List
                ddStyle.DataTextField = "Style"
                ddStyle.DataValueField = "TagStyleID"
                ddStyle.DataBind()
                ddStyle.SelectedValue = oTag.TagStyleID
                'Fill Views
                Fill_Views(oTag.ViewID)
                ddKeyFieldValue.DataSource = (New clsLetterViews).List_Source_View_Columns(IIf(oTag.TypeID = 0, Request("TypeID"), oTag.TypeID))
                ddKeyFieldValue.DataTextField = "COLUMN_NAME"
                ddKeyFieldValue.DataValueField = "COLUMN_NAME"
                ddKeyFieldValue.DataBind()
                ddKeyFieldValue.SelectedValue = oTag.KeyFieldValue

                ddFieldName.SelectedValue = oTag.FieldName
                ddKeyFieldName.SelectedValue = oTag.KeyFieldName

                oTag = Nothing
            End If
        End If
    End Sub

    Private Sub Fill_Views(ByVal value As Integer)
        ddView.DataSource = (New clsLetterViews).List_Views_By_Type(IIf(oTag.TypeID = 0, Request("TypeID"), oTag.TypeID))
        ddView.DataTextField = "View"
        ddView.DataValueField = "ViewID"
        ddView.DataBind()
        ddView.SelectedValue = value
        Fill_Fields()
    End Sub

    Private Sub Fill_Fields()
        ddFieldName.DataSource = (New clsLetterViews).List_View_Columns(ddView.SelectedItem.Text)
        ddFieldName.DataTextField = "COLUMN_NAME"
        ddFieldName.DataValueField = "COLUMN_NAME"
        ddFieldName.DataBind()

        ddKeyFieldName.DataSource = (New clsLetterViews).List_View_Columns(ddView.SelectedItem.Text)
        ddKeyFieldName.DataTextField = "COLUMN_NAME"
        ddKeyFieldName.DataValueField = "COLUMN_NAME"
        ddKeyFieldName.DataBind()
    End Sub
    Protected Sub ddFieldName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddFieldName.SelectedIndexChanged

    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        oTag = New clsLetterTags
        oTag.TagID = Request("TagID")
        oTag.Load()
        oTag.Tag = txtTag.Text
        oTag.TypeID = siType.Selected_ID
        oTag.ViewID = ddView.SelectedValue
        oTag.FieldName = ddFieldName.SelectedValue
        oTag.KeyFieldName = ddKeyFieldName.SelectedValue
        oTag.KeyFieldValue = ddKeyFieldValue.SelectedValue
        oTag.Description = txtDescription.Text
        oTag.TagStyleID = ddStyle.SelectedValue
        oTag.Save()

        'If oView.Error_Message = "" Then
        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "Close", "window.opener.__doPostBack('" & Request("linkid") & "','');window.close();", True)
        'Else
        '    Label1.Text = oNote.Error_Message
        'End If
    End Sub
End Class
