
Partial Class Add_Ins_MultiReservationInvoice
    Inherits System.Web.UI.Page

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        Dim oRes As New clsReservations
        If txtReservation.Text = "" Or Not (isNumeric(txtReservation.Text)) Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Please Enter a Valid Reservation.');", True)
        ElseIf Not (oRes.val_ResID(CInt(txtReservation.Text))) Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Please Enter a Valid Reservation.');", True)
        Else
            Dim bProceed As Boolean = True
            For i = 0 To lbReservation.Items.Count - 1
                If lbReservation.Items(i).Value = txtReservation.Text Then
                    bProceed = False
                    Exit For
                End If
            Next
            If bProceed Then
                lbReservation.Items.Add(txtReservation.Text)
                txtReservation.Text = ""
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('This Reservation Has Already Been Added.');", True)
            End If
        End If
        oRes = Nothing
    End Sub

    Protected Sub Unnamed2_Click(sender As Object, e As System.EventArgs)
        If lbReservation.SelectedValue <> "" Then
            lbReservation.Items.Remove(lbReservation.SelectedItem)
        End If
    End Sub

    Protected Sub Unnamed3_Click(sender As Object, e As System.EventArgs)
        If lbReservation.Items.Count = 0 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Please Enter a Valid Reservation');", True)
        ElseIf txtAmount.Text = "" Or Not (isNumeric(txtAmount.Text)) Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Please Enter a Valid Amount');", True)
        ElseIf txtReference.Text = "" Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Please Enter a Reference');", True)
        ElseIf txtDesc.Text = "" Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Please Enter a Description');", True)
        Else
            Dim oInvoice As New clsInvoices
            Dim oRes As New clsReservations
            For i = 0 To lbReservation.Items.Count - 1
                oInvoice.InvoiceID = 0
                oInvoice.Load()
                oInvoice.KeyField = "ReservationID"
                oInvoice.KeyValue = lbReservation.items(i).Value
                oRes.ReservationID = lbReservation.items(i).Value
                oRes.Load()
                oInvoice.ProspectID = oRes.ProspectID
                oInvoice.FinTransID = ddInvoice.SelectedValue
                oInvoice.Amount = txtAmount.Text
                oInvoice.Reference = txtReference.Text
                oInvoice.Description = txtDesc.Text
                oInvoice.UserID = Session("UserDBID")
                oInvoice.TransDate = System.DateTime.Now
                oInvoice.DueDate = System.DateTime.Now.AddDays(30)
                oInvoice.Save()
            Next
            oInvoice = Nothing
            oRes = Nothing
            lbReservation.Items.Clear()
            txtAmount.Text = ""
            txtDesc.Text = ""
            txtReference.Text = ""
            txtReservation.text = ""
        End If

    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim ofTrans As New clsFinancialTransactionCodes
            ddInvoice.DataSource = ofTrans.List_Trans_Codes("ReservationTrans")
            ddInvoice.DataTextField = "TransCode"
            ddInvoice.DataValueField = "FinTransID"
            ddInvoice.DataBind()
            ofTrans = Nothing
        End If
    End Sub
End Class
