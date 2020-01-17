
Partial Class LeadManagement_editleadtemplatemapping
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            If Request("MappingID") & "" = "" Or Request("linkid") & "" = "" Or Request("LeadTemplateID") & "" = "" Or Request("LeadTemplateID") & "" = "0" Or Request("LeadTemplateID") & "" = 0 Then
                Close()
            Else
                Load_Page()
            End If
        End If
    End Sub

    Private Function List_Lead_Columns() As SqlDataSource
        Return New SqlDataSource(Resources.Resource.cns, "Select * from information_schema.columns WHERE table_name = 't_Leads'")
    End Function

    Private Sub Load_Page()
        hfID.Value = Request("mappingid")
        hfLTID.Value = Request("LeadTemplateID")
        For i = 1 To 30
            ddColNum.Items.Add(New ListItem(i, i))
        Next
        ddColumnName.DataSource = List_Lead_Columns()
        ddColumnName.DataValueField = "COLUMN_NAME"
        ddColumnName.DataTextField = "COLUMN_NAME"
        ddColumnName.DataBind()
        If hfID.Value <> 0 And hfID.Value <> "0" Then
            Dim oLTM As New clsLeadTemplateMapping
            With oLTM
                .LeadFieldMappingID = hfID.Value
                .Load()
                hfLTID.Value = .LeadTemplateID
                ddColumnName.SelectedValue = .ColumnName
                ddColNum.SelectedValue = .ColumnNumber
                ckConvert.Checked = .ConvertValue
                txtOld.Text = .Value
                txtNew.Text = .ConvertedValue
            End With
            oLTM = Nothing
        End If
        Load_Grid()
    End Sub

    Private Sub Load_Grid()
        gvItems.DataSource = (New clsLeadTemplateMappingItems).List(IIf(hfID.Value & "" = "", 0, hfID.Value))
        gvItems.DataBind()
    End Sub

    Private Sub Close()
        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "Close", "window.opener.__doPostBack('" & Request("linkid") & "','');window.close();", True)
    End Sub

    Protected Sub lbSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSave.Click
        If Not (CheckSecurity("LeadTemplate", "Edit", , , Session("UserDBID"))) Then
            Dim oLTM As New clsLeadTemplateMapping
            With oLTM
                .LeadFieldMappingID = hfID.Value
                .Load()
                .LeadTemplateID = hfLTID.Value
                .ColumnName = ddColumnName.SelectedValue
                .ColumnNumber = ddColNum.SelectedValue
                .ConvertValue = ckConvert.Checked
                .Value = txtOld.Text
                .ConvertedValue = txtNew.Text
                .CheckDuplicates = False
                .Save()
            End With
            oLTM = Nothing
            Close()
        End If
    End Sub

    Protected Sub lbClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbClose.Click
        Close()
    End Sub

    Protected Sub btnAdd_Click(sender As Object, e As System.EventArgs) Handles btnAdd.Click
        Dim oItem As New clsLeadTemplateMappingItems
        oItem.MapItemID = 0
        oItem.Load()
        oItem.LookupValue = txtOld.Text
        oItem.ConvertedValue = txtNew.Text
        oItem.LeadFieldMappingID = hfID.Value
        oItem.Save()
        oItem = Nothing
        Load_Grid()
    End Sub

    Protected Sub btnUpdate_Click(sender As Object, e As System.EventArgs) Handles btnUpdate.Click
        If gvItems.SelectedIndex > -1 Then
            Dim oItem As New clsLeadTemplateMappingItems
            oItem.MapItemID = gvItems.Rows(gvItems.SelectedIndex).Cells(1).Text
            oItem.Load()
            oItem.LookupValue = txtOld.Text
            oItem.ConvertedValue = txtNew.Text
            oItem.LeadFieldMappingID = hfID.Value
            oItem.Save()
            oItem = Nothing
            Load_Grid()
        End If
    End Sub

    Protected Sub btnRemove_Click(sender As Object, e As System.EventArgs) Handles btnRemove.Click
        If gvItems.SelectedIndex > -1 Then
            Dim o As New clsLeadTemplateMappingItems
            o.Remove(gvItems.Rows(gvItems.SelectedIndex).Cells(1).Text)
            o = Nothing
            Load_Grid()
        End If
    End Sub

    Protected Sub gvItems_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvItems.SelectedIndexChanged
        If gvItems.SelectedIndex > -1 Then
            txtOld.Text = Trim(gvItems.Rows(gvItems.SelectedIndex).Cells(3).Text).Replace("&nbsp;", "")
            txtNew.Text = Trim(gvItems.Rows(gvItems.SelectedIndex).Cells(4).Text).Replace("&nbsp;", "")
        End If
    End Sub
End Class
