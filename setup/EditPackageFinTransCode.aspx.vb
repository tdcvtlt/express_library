
Partial Class setup_EditPackageFinTransCode
    Inherits System.Web.UI.Page


    Protected Sub LinkFinTransPayment_Click(sender As Object, e As System.EventArgs) Handles LinkFinTransPayment.Click
        If txtID.Text > 0 Then
            Dim oPkgPayments As New clsPackagePayment
            gvPayments.DataSource = oPkgPayments.List_Payments(txtID.Text)
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvPayments.DataKeyNames = sKeys
            gvPayments.DataBind()
            oPkgPayments = Nothing
            MultiViewPackage.ActiveViewIndex = 1
        End If
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim oFin As New clsFinancialTransactionCodes
            ddFinTrans.DataSource = oFin.List_Trans_Codes("PackageTrans")
            ddFinTrans.DataTextField = "TransCode"
            ddFinTrans.DataValueField = "FinTransID"
            ddFinTrans.DataBind()
            oFin = Nothing
            Dim oPkgFinTrans As New clsPackageFinTransCode
            oPkgFinTrans.PackageFinTransCodeID = Request("PkgFinTransID")
            oPkgFinTrans.Load()
            ddFinTrans.SelectedValue = oPkgFinTrans.FinTransCodeID
            txtID.Text = Request("PkgFinTransID")
            cbFormula.Checked = oPkgFinTrans.UseFormula
            txtAmount.Text = oPkgFinTrans.Amount
            txtFormula.Text = oPkgFinTrans.Formula
            Dim oPkg As New clsPackage
            If Request("PkgFinTransID") = 0 Then
                oPkg.PackageID = Request("PackageID")
            Else
                oPkg.PackageID = oPkgFinTrans.PackageID
            End If
            oPkg.Load()
            lbPackage.Text = oPkg.Package
            oPkg = Nothing
            oPkgFinTrans = Nothing
            MultiViewPackage.ActiveViewIndex = 0
        End If
    End Sub

    Protected Sub LinkButton1_Click(sender As Object, e As System.EventArgs) Handles LinkButton1.Click
        Dim oPkgFinTrans As New clsPackageFinTransCode
        oPkgFinTrans.PackageFinTransCodeID = txtID.Text
        oPkgFinTrans.Load()
        oPkgFinTrans.UserID = Session("UserDBID")
        If txtID.Text = 0 Then
            oPkgFinTrans.PackageID = Request("PackageID")
        End If
        oPkgFinTrans.FinTransCodeID = ddFinTrans.SelectedValue
        oPkgFinTrans.Amount = txtAmount.Text
        oPkgFinTrans.UseFormula = cbFormula.Checked
        oPkgFinTrans.Formula = txtFormula.Text
        oPkgFinTrans.Save()
        Response.Redirect("EditPackageFinTransCode.aspx?PkgFinTransID=" & oPkgFinTrans.PackageFinTransCodeID)
        oPkgFinTrans = Nothing
    End Sub

    Protected Sub lbPackage_Click(sender As Object, e As System.EventArgs) Handles lbPackage.Click
        If txtID.Text = 0 Then
            Response.Redirect("EditPackage.aspx?PackageID=" & Request("PackageID"))
        Else
            Dim oPkgFinTrans As New clsPackageFinTransCode
            oPkgFinTrans.PackageFinTransCodeID = txtID.Text
            oPkgFinTrans.Load()
            Response.Redirect("EditPackage.aspx?PackageID=" & oPkgFinTrans.PackageID)
            oPkgFinTrans = Nothing
        End If
    End Sub

    Protected Sub LinkButton2_Click(sender As Object, e As System.EventArgs) Handles LinkButton2.Click
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/setup/EditPackagePayment.aspx?PackagePaymentID=0&PackageFinTransID=" & txtID.Text & "','win01',350,350);", True)
    End Sub

    Protected Sub LinkFinTrans_Click(sender As Object, e As System.EventArgs) Handles LinkFinTrans.Click
        MultiViewPackage.ActiveViewIndex = 0
    End Sub

    Protected Sub gvPayments_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvPayments.SelectedIndexChanged
        Dim row As GridViewRow = gvPayments.SelectedRow
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/setup/EditPackagePayment.aspx?PackagePaymentID=" & row.Cells(1).Text & "','win01',350,350);", True)
    End Sub
End Class
