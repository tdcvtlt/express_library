
Partial Class general_EditSpecialNeed
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Load_DD()
            If Request("SpecialNeedID") > 0 Then
                Dim osNeed As New clsSpecialNeeds
                osNeed.SpecialNeedID = Request("SpecialNeedID")
                osNeed.Load()
                ddNeeds.SelectedValue = osNeed.NeedID
                If ddNeeds.SelectedItem.Text = "Other" Then
                    txtNeed.Visible = True
                    lblNeed.Visible = True
                    txtNeed.Text = osNeed.NeedText
                End If
                osNeed = Nothing
            End If
        End If
    End Sub

    Protected Sub Load_DD()
        Dim osNeed As New clsSpecialNeeds
        ddNeeds.DataSource = osNeed.List_SpecialNeeds()
        ddNeeds.DataTextField = "ComboItem"
        ddNeeds.DataValueField = "ComboItemID"
        ddNeeds.DataBind()
        osNeed = Nothing
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim osNeed As New clsSpecialNeeds
        osNeed.SpecialNeedID = Request("SpecialNeedID")
        osNeed.Load()
        osNeed.KeyField = Request("KeyField")
        osNeed.KeyValue = Request("KeyValue")
        osNeed.NeedID = ddNeeds.SelectedValue
        If ddNeeds.SelectedItem.Text = "Other" Then
            osNeed.NeedText = txtNeed.Text
        End If
        osNeed.Save()
        Label1.Text = osNeed.Err
        If osNeed.Err = "" Then
            'ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "Close", "window.opener.__doPostBack('" & Request("linkid") & "','');window.close();", True)
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "Close", "window.opener.Refresh_SpecialNeeds();window.close();", True)
        End If
        osNeed = Nothing
    End Sub

    Protected Sub ddNeeds_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddNeeds.SelectedIndexChanged
        If ddNeeds.SelectedItem.Text = "Other" Then
            txtNeed.Visible = True
            txtNeed.Text = ""
            lblNeed.Visible = True
        Else
            txtNeed.Visible = False
            lblNeed.Visible = False
        End If
    End Sub
End Class
