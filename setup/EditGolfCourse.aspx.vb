
Partial Class setup_EditGolfCourse
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not (IsPostBack) Then

            siState.Connection_String = Resources.Resource.cns
            siState.ComboItem = "State"
            siState.Label_Caption = ""
            siState.Load_Items()

            Dim oFC As New clsFinancialTransactionCodes
            ddInvoices.DataSource = oFC.List_Trans_Codes("ReservationTrans")
            ddInvoices.DataTextField = "TransCode"
            ddInvoices.DataValueField = "FinTransID"
            ddInvoices.DataBind()
            oFC = Nothing

            Dim oGolf As New clsGolfCourse
            oGolf.GolfID = Request("ID")
            oGolf.Load()
            txtID.Text = oGolf.GolfID
            txtCourse.Text = oGolf.Course
            txtAddress.Text = oGolf.Address1
            txtCity.Text = oGolf.City
            siState.Selected_ID = oGolf.StateID
            txtPostalCode.Text = oGolf.PostalCode
            txtPhone.Text = oGolf.PhoneNumber
            cbActive.Checked = oGolf.Active
            txtCost.Text = oGolf.Cost
            ddInvoices.SelectedValue = oGolf.NoShowInvoiceID
            txtInvAmount.Text = oGolf.NoShowInvoiceCost
            oGolf = Nothing
        End If
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim oGolf As New clsGolfCourse
        oGolf.GolfID = txtID.Text
        oGolf.UserID = Session("UserDBID")
        oGolf.Load()
        oGolf.Course = txtCourse.Text
        oGolf.Address1 = txtAddress.Text
        oGolf.City = txtCity.Text
        oGolf.StateID = siState.Selected_ID
        oGolf.PostalCode = txtPostalCode.Text
        oGolf.PhoneNumber = txtPhone.Text
        oGolf.Active = cbActive.Checked
        oGolf.Cost = txtCost.Text
        oGolf.NoShowInvoiceID = ddInvoices.SelectedValue
        oGolf.NoShowInvoiceCost = txtInvAmount.Text
        oGolf.Save()
        Response.Redirect("EditGolfCourse.aspx?ID=" & oGolf.GolfID)
        oGolf = Nothing
    End Sub

End Class
