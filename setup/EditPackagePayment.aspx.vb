
Partial Class setup_EditPackagePayment
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim oPkgPayment As New clsPackagePayment
            oPkgPayment.PackagePaymentID = Request("PackagePaymentID")
            oPkgPayment.Load()
            siMethod.Connection_String = Resources.Resource.cns
            siMethod.ComboItem = "PaymentMethod"
            siMethod.Label_Caption = ""
            siMethod.Selected_ID = oPkgPayment.PaymentMethodID
            siMethod.Load_Items()
            txtID.Text = Request("PackagePaymentID")
            txtAmount.Text = oPkgPayment.Amount
            siMethod.Selected_ID = oPkgPayment.PaymentMethodID
            cbAdjustment.Checked = oPkgPayment.Adjustment
            cbPosNeg.Checked = oPkgPayment.PosNeg
            cbFixedAmount.Checked = oPkgPayment.FixedAmount
            oPkgPayment = Nothing
        End If
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        Dim oPkgPayment As New clsPackagePayment
        Dim oPkgFinTrans As New clsPackageFinTransCode
        oPkgPayment.PackagePaymentID = txtID.Text
        oPkgPayment.Load()
        oPkgPayment.UserID = Session("UserDBID")
        If txtID.Text = 0 Then
            oPkgFinTrans.PackageFinTransCodeID = Request("PackageFinTransID")
            oPkgFinTrans.Load()
            oPkgPayment.PackageID = oPkgFinTrans.PackageID
            oPkgPayment.PackageFinTransID = Request("PackageFinTransID")
        End If
        oPkgPayment.PaymentMethodID = siMethod.Selected_ID
        oPkgPayment.Adjustment = cbAdjustment.Checked
        oPkgPayment.PosNeg = cbPosNeg.Checked
        oPkgPayment.FixedAmount = cbFixedAmount.Checked
        txtAmount.Text = oPkgPayment.Amount
        oPkgPayment.Save()
        oPkgPayment = Nothing
        oPkgFinTrans = Nothing
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.opener.refreshPayments();window.close();", True)
    End Sub
End Class
