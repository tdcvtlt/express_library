
Partial Class Accounting_maintenancefeecode2fintrans
    Inherits System.Web.UI.Page

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "Close", "window.opener.__doPostBack('" & Request("linkid") & "','');window.close();", True)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            If Request("CodeID") = "" Then
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "Close", "window.close();", True)
            Else
                Dim oFTC As New clsFinancialTransactionCodes
                ddFinTrans.DataSource = oFTC.List_MF
                ddFinTrans.DataTextField = "Code"
                ddFinTrans.DataValueField = "ID"
                ddFinTrans.DataBind()
                oFTC = Nothing

                Dim oMF As New clsMaintenanceFeeCodes
                oMF.MaintenanceFeeCodeID = Request("CodeID")
                If Request("mapid") = "" Then
                    oMF.Load()
                    txtCode.Text = oMF.Code
                Else
                    Dim oMFM As New clsMaintenanceFeeCode2FinTrans
                    oMFM.MaintenanceFeeCode2FinTransID = Request("mapid")
                    oMFM.Load()
                    oMF.MaintenanceFeeCodeID = oMFM.MaintenanceFeeCodeID
                    oMF.Load()
                    txtCode.Text = oMF.Code
                    ddFinTrans.SelectedValue = oMFM.FinTransID
                    txtAmount.Text = oMFM.Amount
                    oMFM = Nothing
                End If
                oMF = Nothing
                
            End If
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim oMFM As New clsMaintenanceFeeCode2FinTrans
        oMFM.MaintenanceFeeCode2FinTransID = IIf(Request("mapid") > 0, Request("Mapid"), 0)
        oMFM.Load()
        oMFM.FinTransID = ddFinTrans.SelectedValue
        oMFM.Amount = txtAmount.Text
        oMFM.MaintenanceFeeCodeID = IIf(oMFM.MaintenanceFeeCode2FinTransID > 0, oMFM.MaintenanceFeeCodeID, Request("codeid"))
        oMFM.Save()
        oMFM = Nothing
        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "Close", "window.opener.__doPostBack('" & Request("linkid") & "','');window.close();", True)
    End Sub
End Class
