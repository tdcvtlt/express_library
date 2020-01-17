
Partial Class setup_EditPackageResFinTrans
    Inherits System.Web.UI.Page


    Protected Sub LinkFinTransPayment_Click(sender As Object, e As System.EventArgs) Handles LinkFinTransPayment.Click
        If txtID.Text > 0 Then
            Dim oPkgPayments As New clsPackageReservationPayment
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
            ddFinTrans.DataSource = oFin.List_Trans_Codes("ReservationTrans")
            ddFinTrans.DataTextField = "TransCode"
            ddFinTrans.DataValueField = "FinTransID"
            ddFinTrans.DataBind()
            oFin = Nothing
            Dim oPkgFinTrans As New clsPackageReservationFinTransCode
            oPkgFinTrans.PackageReservationFinTransCodeID = Request("PkgResFinTransID")
            oPkgFinTrans.Load()
            ddFinTrans.SelectedValue = oPkgFinTrans.FinTransID
            txtID.Text = Request("PkgResFinTransID")
            cbFormula.Checked = oPkgFinTrans.UseFormula
            txtAmount.Text = oPkgFinTrans.Amount
            txtFormula.Text = oPkgFinTrans.Formula
            Dim oPkg As New clsPackageReservation
            Dim oCombo As New clsComboItems
            If Request("PkgResFinTransID") = 0 Then
                oPkg.PackageReservationID = Request("PackageReservationID")
            Else
                oPkg.PackageReservationID = oPkgFinTrans.PackageReservationID
            End If
            oPkg.Load()
            lbPackage.Text = oCombo.Lookup_ComboItem(oPkg.LocationID)
            oPkg = Nothing
            oCombo = Nothing
            oPkgFinTrans = Nothing
            MultiViewPackage.ActiveViewIndex = 0
        End If
    End Sub

    Protected Sub LinkButton1_Click(sender As Object, e As System.EventArgs) Handles LinkButton1.Click
        Dim oPkgFinTrans As New clsPackageReservationFinTransCode
        oPkgFinTrans.PackageReservationFinTransCodeID = txtID.Text
        oPkgFinTrans.Load()
        oPkgFinTrans.UserID = Session("UserDBID")
        If txtID.Text = 0 Then
            oPkgFinTrans.PackageID = Request("PackageID")
            oPkgFinTrans.PackageReservationID = Request("PackageReservationID")
        End If
        oPkgFinTrans.FinTransID = ddFinTrans.SelectedValue
        oPkgFinTrans.Amount = txtAmount.Text
        oPkgFinTrans.UseFormula = cbFormula.Checked
        oPkgFinTrans.Formula = txtFormula.Text
        oPkgFinTrans.Save()
        Response.Redirect("EditPackageResFinTrans.aspx?PkgResFinTransID=" & oPkgFinTrans.PackageReservationFinTransCodeID)
        oPkgFinTrans = Nothing
    End Sub

    Protected Sub lbPackage_Click(sender As Object, e As System.EventArgs) Handles lbPackage.Click
        Dim oPkgReservation As New clsPackageReservation
        Dim oPkgResFin As New clsPackageReservationFinTransCode
        Dim oPkg As New clsPackage
        Dim oCombo As New clsComboItems
        If txtID.Text = 0 Then
            oPkgReservation.PackageReservationID = Request("PackageReservationID")
        Else
            oPkgResFin.PackageReservationFinTransCodeID = txtID.Text
            oPkgResFin.Load()
            oPkgReservation.PackageReservationID = oPkgResFin.PackageReservationID
        End If
        oPkgReservation.Load()

        oPkg.PackageID = oPkgReservation.PackageID
        oPkg.Load()
        Response.Redirect("EditPackageReservation.aspx?PackageReservationID=" & oPkgReservation.PackageReservationID & "&Package=" & oPkg.Package & "&PackageID=" & oPkg.PackageID & "&ReservationName=" & oCombo.Lookup_ComboItem(oPkgReservation.LocationID))
        oPkgResFin = Nothing
        oPkg = Nothing
        oPkgReservation = Nothing
        oCombo = Nothing
    End Sub

    Protected Sub LinkButton2_Click(sender As Object, e As System.EventArgs) Handles LinkButton2.Click
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/setup/EditPackageResPayment.aspx?PackagePaymentID=0&PackageResFinTransID=" & txtID.Text & "','win01',350,350);", True)
    End Sub

    Protected Sub LinkFinTrans_Click(sender As Object, e As System.EventArgs) Handles LinkFinTrans.Click
        MultiViewPackage.ActiveViewIndex = 0
    End Sub

    Protected Sub gvPayments_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvPayments.SelectedIndexChanged
        Dim row As GridViewRow = gvPayments.SelectedRow
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/setup/EditPackageResPayment.aspx?PackagePaymentID=" & row.Cells(1).Text & "','win01',350,350);", True)
    End Sub
End Class
