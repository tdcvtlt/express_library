
Partial Class setup_EditPackageTourFinTrans
    Inherits System.Web.UI.Page

    Protected Sub LinkFinTransPayment_Click(sender As Object, e As System.EventArgs) Handles LinkFinTransPayment.Click
        If txtID.Text > 0 Then
            Dim oPkgPayments As New clsPackageTourPayment
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
            ddFinTrans.DataSource = oFin.List_Trans_Codes("TourTrans")
            ddFinTrans.DataTextField = "TransCode"
            ddFinTrans.DataValueField = "FinTransID"
            ddFinTrans.DataBind()
            oFin = Nothing
            Dim oPkgFinTrans As New clsPackageTourFinTransCode
            oPkgFinTrans.PackageTourFinTransCodeID = Request("PkgTourFinTransID")
            oPkgFinTrans.Load()
            ddFinTrans.SelectedValue = oPkgFinTrans.FinTransID
            txtID.Text = Request("PkgTourFinTransID")
            cbFormula.Checked = oPkgFinTrans.UseFormula
            txtAmount.Text = oPkgFinTrans.Amount
            txtFormula.Text = oPkgFinTrans.Formula
            Dim oPkgTour As New clsPackageTour
            Dim oCombo As New clsComboItems
            If Request("PkgTourFinTransID") = 0 Then
                oPkgTour.PackageTourID = Request("PackageTourID")
            Else
                oPkgTour.PackageTourID = oPkgFinTrans.PackageTourID
            End If
            oPkgTour.Load()
            lbPackage.Text = oCombo.Lookup_ComboItem(oPkgTour.TourLocationID)
            oPkgTour = Nothing
            oPkgFinTrans = Nothing
            MultiViewPackage.ActiveViewIndex = 0
        End If
    End Sub

    Protected Sub LinkButton1_Click(sender As Object, e As System.EventArgs) Handles LinkButton1.Click
        Dim oPkgFinTrans As New clsPackageTourFinTransCode
        oPkgFinTrans.PackageTourFinTransCodeID = txtID.Text
        oPkgFinTrans.Load()
        oPkgFinTrans.UserID = Session("UserDBID")
        If txtID.Text = 0 Then
            oPkgFinTrans.PackageID = Request("PackageID")
            oPkgFinTrans.PackageTourID = Request("packageTourID")
        End If
        oPkgFinTrans.FinTransID = ddFinTrans.SelectedValue
        oPkgFinTrans.Amount = txtAmount.Text
        oPkgFinTrans.UseFormula = cbFormula.Checked
        oPkgFinTrans.Formula = txtFormula.Text
        oPkgFinTrans.Save()
        Response.Redirect("EditPackageTourFinTrans.aspx?PkgTourFinTransID=" & oPkgFinTrans.PackageTourFinTransCodeID)
        oPkgFinTrans = Nothing
    End Sub

    Protected Sub lbPackage_Click(sender As Object, e As System.EventArgs) Handles lbPackage.Click
        Dim oPkgTour As New clsPackageTour
        Dim oPkgReservation As New clsPackageReservation
        Dim oPkg As New clsPackage
        Dim oCombo As New clsComboItems
        Dim oPkgTourFinTrans As New clsPackageTourFinTransCode
        If txtID.Text = 0 Then
            oPkgTour.PackageTourID = Request("PackageTourID")
        Else
            oPkgTourFinTrans.PackageTourFinTransCodeID = txtID.Text
            oPkgTourFinTrans.Load()
            oPkgTour.PackageTourID = oPkgTourFinTrans.PackageTourID
        End If
        oPkgTour.Load()
        oPkgReservation.PackageReservationID = oPkgTour.PackageReservationID
        oPkgReservation.Load()
        oPkg.PackageID = oPkgReservation.PackageID
        oPkg.Load()
        Response.Redirect("EditPackageReservationTour.aspx?PackageID=" & oPkg.PackageID & "&PackageName=" & oPkg.Package & "&PackageReservationID=" & oPkgReservation.PackageReservationID & "&Reservation=" & oCombo.Lookup_ComboItem(oPkgReservation.LocationID) & "&PackageTourID=" & oPkgTour.PackageTourID)
        oPkg = Nothing
        oPkgReservation = Nothing
        oCombo = Nothing
    End Sub

    Protected Sub LinkButton2_Click(sender As Object, e As System.EventArgs) Handles LinkButton2.Click
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/setup/EditPackageTourPayment.aspx?PackagePaymentID=0&PackageTourFinTransID=" & txtID.Text & "','win01',350,350);", True)
    End Sub

    Protected Sub LinkFinTrans_Click(sender As Object, e As System.EventArgs) Handles LinkFinTrans.Click
        MultiViewPackage.ActiveViewIndex = 0
    End Sub

    Protected Sub gvPayments_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvPayments.SelectedIndexChanged
        Dim row As GridViewRow = gvPayments.SelectedRow
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/setup/EditPackageTourPayment.aspx?PackagePaymentID=" & row.Cells(1).Text & "','win01',350,350);", True)
    End Sub
End Class
