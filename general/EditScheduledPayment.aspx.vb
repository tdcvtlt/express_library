
Partial Class general_EditScheduledPayment
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oSchedPay As New clsPaymentsScheduled
            Dim oCombo As New clsComboItems
            Dim oPersonnel As New clsPersonnel
            oSchedPay.SchedPayID = Request("ID")
            oSchedPay.Load()
            lblPM.Text = oCombo.Lookup_ComboItem(oSchedPay.SchedPayID)
            txtRef.Text = oSchedPay.Reference
            txtDesc.Text = oSchedPay.Description
            txtAmount.Text = oSchedPay.Amount
            oPersonnel.PersonnelID = oSchedPay.UserID
            oPersonnel.Load()
            txtUserName.Text = oPersonnel.UserName
            dteScheduledDate.Selected_Date = oSchedPay.SchedDate
            oCombo = Nothing
            oSchedPay = Nothing
            oPersonnel = Nothing
        End If
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        Dim oSchedPay As New clsPaymentsScheduled
        Dim bProceed As Boolean = True
        oSchedPay.SchedPayID = Request("ID")
        oSchedPay.Load()
        If DateDiff(DateInterval.Minute, CDate(oSchedPay.CreatedDate), Date.Now) <= 1440 And oSchedPay.UserID = Session("UserDBID") Then
            bProceed = True
        ElseIf CheckSecurity("Payments", "EditScheduled", , , Session("UserDBID")) Then
            bProceed = True
        Else
            bProceed = False
        End If

        If bProceed Then
            oSchedPay.Reference = txtRef.Text
            oSchedPay.Amount = txtAmount.Text
            oSchedPay.Description = txtDesc.Text
            oSchedPay.SchedDate = dteScheduledDate.Selected_Date
            If oSchedPay.Save Then
                Dim oPayment2Inv As New clsPaymentSched2Invoice
                oPayment2Inv.Update_Pmt_Amt(oSchedPay.SchedPayID, txtAmount.Text)
                oPayment2Inv = Nothing
                ClientScript.RegisterClientScriptBlock(Me.GetType, "refresh", "window.opener.__doPostBack('" & Request("linkid") & "','');window.close();", True)
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Sorry", "alert('" & oSchedPay.Err & "');", True)
            End If
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Sorry", "alert('This payment is not editable.');", True)
        End If
        oSchedPay = Nothing
    End Sub
End Class
