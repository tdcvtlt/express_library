
Partial Class EditMaintenanceFeeCode
    Inherits System.Web.UI.Page

    Protected Sub Main_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Main_Link.Click
        MultiView1.ActiveViewIndex = 0
    End Sub

    Protected Sub Mapping_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Mapping_Link.Click
        MultiView1.ActiveViewIndex = 1
        Dim oMFM As New clsMaintenanceFeeCode2FinTrans
        gvMapping.DataSource = oMFM.List("mfc.MaintenanceFeeCodeID", txtID.Text)
        gvMapping.DataBind()
        oMFM = Nothing
    End Sub

    Protected Sub Events_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Events_Link.Click
        MultiView1.ActiveViewIndex = 2
        ucEvents.KeyField = "MaintenanceFeeCodeID"
        ucEvents.KeyValue = txtID.Text
        If ucEvents.KeyValue > 0 Then
            ucEvents.List()
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            MultiView1.ActiveViewIndex = 0
            Set_Values()
        End If
        If Not (CheckSecurity("MaintenanceFeeCodes", "View", , , Session("UserDBID"), )) Then
            MultiView1.ActiveViewIndex = 3
        End If
    End Sub

    Private Sub Set_Values()
        Dim oMFC As New clsMaintenanceFeeCodes
        oMFC.MaintenanceFeeCodeID = IIf(Request("MaintenanceFeeCodeID") <> "", Request("MaintenanceFeeCodeID"), 0)
        oMFC.Load()
        txtID.Text = oMFC.MaintenanceFeeCodeID
        txtCode.Text = oMFC.Code
        txtDescription.Text = oMFC.Description
        ddPhase.SelectedValue = oMFC.Phase
        ddSize.SelectedValue = oMFC.Size
        oMFC = Nothing
    End Sub

    Protected Sub lbSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSave.Click
        Dim oMFC As New clsMaintenanceFeeCodes
        oMFC.MaintenanceFeeCodeID = IIf(txtID.Text <> "", txtID.Text, 0)
        oMFC.Load()
        oMFC.Code = txtCode.Text
        oMFC.Description = txtDescription.Text
        oMFC.Phase = ddPhase.SelectedValue
        oMFC.Size = ddSize.SelectedValue
        oMFC.UserID = Session("UserDBID")
        oMFC.Save()

    End Sub

    Protected Sub lbRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbRefresh.Click
        Mapping_Link_Click(sender, e)
    End Sub
End Class
