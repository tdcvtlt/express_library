
Partial Class general_RequestRefund
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            ddRefMethod.Items.Add(New listitem("", "0"))
            ddRefmethod.Items.Add("Check Request")
            ddRefMethod.items.Add("Credit Card Refund")
            ddRefMethod.Items.Add("Cash Refund")
        End If
    End Sub

    Protected Sub ddRefMethod_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddRefMethod.SelectedIndexChanged
        If ddRefMethod.SelectedValue = "Credit Card Refund" Then
            MultiView1.ActiveViewIndex = 0
            Dim oCCTrans As New clsCCTrans
            If Request("KeyField") = "ProspectID" And (Request("KeyValue") = 0 Or Request("KeyValue") = "0") Then
                gvCCTrans.DataSource = oCCTrans.List_CC_Trans_For_Refund(Request("KeyField"), Request("ProspectID"))
            Else
                gvCCTrans.DataSource = oCCTrans.List_CC_Trans_For_Refund(Request("keyField"), Request("KeyValue"))
            End If
            gvCCTrans.DataBind()
            oCCTrans = Nothing
        ElseIf ddRefMethod.SelectedValue = "Cash Refund" Then
            MultiView1.ActiveViewIndex = 1
            Dim oPayments As New clsPayments
            gvCashPayments.DataSource = oPayments.List_Cash_Payments(Request("keyField"), Request("KeyValue"))
            gvCashPayments.DataBind()
            lblErr3.Text = oPayments.Error_Message
            oPayments = Nothing
        ElseIf ddRefMethod.SelectedValue = "Check Request" Then
            MultiView1.ActiveViewIndex = 2
            If CheckSecurity("Payments", "ProcessCheckRefunds", , , Session("UserDBID")) Then
                Dim oPayments As New clsPayments
                oPayments.KeyField = Request("KeyField")
                oPayments.KeyValue = Request("KeyValue")
                If Request("KeyField") = "ProspectID" And (Request("KeyValue") = 0 Or Request("KeyValue") = "0") Then
                    gvPayments.DataSource = oPayments.List_Check_Refund_Payments(Request("keyField"), Request("ProspectID"))
                Else
                    gvPayments.DataSource = oPayments.List_Check_Refund_Payments(Request("keyField"), Request("KeyValue"))
                End If
                gvPayments.DataBind()
                'lblErr4.Text = oPayments.Error_Message
                oPayments = Nothing
                Button2.Visible = True
            Else
                lblErr4.Text = "ACCESS DENIED"
                Button2.Visible = False
            End If
        End If
    End Sub

    Protected Sub gvCashPayments_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Cash Payments" Then
            e.Row.Cells(2).Visible = False
            If e.Row.RowIndex > -1 Then
                e.Row.Cells(3).Text = CDate(e.Row.Cells(3).Text).ToShortDateString
                Dim txtAmt As TextBox = e.Row.FindControl("txtAmount")
                txtAmt.Text = e.Row.Cells(5).Text
            End If
            e.Row.Cells(5).Visible = False
        End If
    End Sub

    Protected Sub gvPayments_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Payments" Then
            e.Row.Cells(2).Visible = False
            If e.Row.RowIndex > -1 Then
                e.Row.Cells(4).Text = CDate(e.Row.Cells(4).Text).ToShortDateString
                Dim txtAmt As TextBox = e.Row.FindControl("txtAmount")
                txtAmt.Text = e.Row.Cells(5).Text
            End If
            e.Row.Cells(5).Visible = False
            e.Row.Cells(6).Visible = False
        End If
    End Sub
    Protected Sub gvCCTrans_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Credit Card Transactions" Then
            e.Row.Cells(6).Visible = False
        End If
    End Sub

    Protected Sub gvCCApply_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then
            If e.Row.RowIndex > -1 Then
                Dim txtAmt As TextBox = e.Row.FindControl("txtAmount")
                txtAmt.Text = e.Row.Cells(4).Text
            End If
            e.Row.Cells(1).Visible = False
            e.Row.Cells(2).Visible = False
            e.Row.Cells(4).Visible = False
        End If
    End Sub

    Protected Sub gvCCTrans_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvCCTrans.SelectedIndexChanged
        Dim row As GridViewRow = gvCCTrans.SelectedRow
        Dim oCCTrans As New clsCCTransApplyTo
        gvCCApply.DataSource = oCCTrans.Get_Applied_To(row.Cells(1).Text)
        Dim sKeys(0) As String
        sKeys(0) = "PaymentID"
        gvCCApply.DataKeyNames = sKeys
        gvCCApply.DataBind()
        hfCCTransID.Value = row.Cells(1).Text
        hfRefRequest.Value = row.Cells(6).Text
        MultiView1.ActiveViewIndex = 3
    End Sub

    Protected Sub Unnamed2_Click(sender As Object, e As System.EventArgs)
        MultiView1.ActiveViewIndex = 0
    End Sub

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        '****Validate Amounts******'
        Dim totalRefAmt As Double = 0
        Dim refAmt As Double = 0
        Dim bProceed As Boolean = True
        Dim sErr As String = ""
        Dim orefApply As New clsRefundRequestApplyTo
        For Each row As GridViewRow In gvCCApply.Rows
            Dim cb As CheckBox = row.FindControl("cb")
            If cb.Checked Then
                Dim txtAmt As TextBox = row.FindControl("txtAmount")
                refAmt = CDbl(txtAmt.Text)
                If refAmt > CDbl(row.Cells(4).Text) Then
                    bProceed = False
                    sErr = "You Can Not Refund For More Than What Was Charged."
                Else
                    If CDbl(refAmt) > Math.Round(CDbl(row.Cells(4).Text) - orefApply.Get_Ref_Amount_Per_Payment(CInt(row.Cells(1).Text)), 2) Then
                        bProceed = False
                        sErr = "Amount Requested Exceeds Amount Available to Refund for Payment Selected. " '& CDbl(refAmt) & " - " & Math.Round(CDbl(row.Cells(4).Text) - orefApply.Get_Ref_Amount_Per_Payment(CInt(row.Cells(1).Text)), 2)
                    Else
                        totalRefAmt += refAmt
                    End If
                End If
            End If
        Next
        If bProceed Then
            Dim orefRequest As New clsRefundRequest
            Dim oCCTrans As New clsCCTrans
            Dim refundID As Integer = 0
            oCCTrans.CCTransID = hfCCTransID.Value
            oCCTrans.Load()
            orefRequest.RefundRequestID = 0
            orefRequest.Load()
            orefRequest.CreditCardID = oCCTrans.CreditCardID
            orefRequest.AccountID = oCCTrans.AccountID
            orefRequest.Amount = totalRefAmt
            orefRequest.RequestedByID = Session("UserDBID")
            orefRequest.DateRequested = System.DateTime.Now
            orefRequest.ProspectID = Request("ProspectID")
            If hfRefRequest.Value = "False" Then
                orefRequest.Approved = 1
                orefRequest.ApprovedByID = Session("UserDBID")
                orefRequest.DateApproved = System.DateTime.Now
            End If
            orefRequest.Save()
            refundID = orefRequest.RefundRequestID
            '*****Create RefundApplyTo Items
            For Each row As GridViewRow In gvCCApply.Rows
                Dim cb As CheckBox = row.FindControl("cb")
                If cb.Checked Then
                    Dim txtAmt As TextBox = row.FindControl("txtAmount")
                    orefApply.RefundApplyID = 0
                    orefApply.RefundRequestID = refundID
                    orefApply.PaymentID = row.Cells(1).Text
                    orefApply.Amount = CDbl(txtAmt.Text)
                    orefApply.Save()
                End If
            Next
            If hfRefRequest.Value = "False" Then
                If Create_Refund_Trans(refundID) Then
                    lblErr.Text = "Refund Trans Successful."
                Else
                    lblErr.Text = lblErr.Text & " Error Processing Refund Trans."
                End If
            Else
                lblErr.Text = IIf(hfRefRequest.Value = "True", "Request has been submitted", hfRefRequest.Value)
            End If
            orefRequest = Nothing
            oCCTrans = Nothing
        Else
            lblErr.Text = sErr
        End If
        orefApply = Nothing
    End Sub

    Private Function Create_Refund_Trans(ByVal refundID As Integer) As Boolean
        Dim bProcessed As Boolean = True
        Dim ccTransID As Integer = 0
        Dim oCCTrans As New clsCCTrans
        Dim oCCTransApply As New clsCCTransApplyTo
        Dim oRefRequest As New clsRefundRequest
        Dim oCombo As New clsComboItems
        oRefRequest.RefundRequestID = refundID
        oRefRequest.Load()
        oCCTrans.CCTransID = 0
        oCCTrans.Load()
        oCCTrans.TransTypeID = oCombo.Lookup_ID("CCTransType", "Refund")
        oCCTrans.AccountID = oRefRequest.AccountID
        oCCTrans.Amount = oRefRequest.Amount
        oCCTrans.CreditCardID = oRefRequest.CreditCardID
        oCCTrans.CreatedByID = Session("UserDBID")
        oCCTrans.DateCreated = System.DateTime.Now
        Dim oCC As New clsCreditCard
        oCC.CreditCardID = oRefRequest.CreditCardID
        oCC.Load()
        oCCTrans.Token = oCC.Token
        oCC = Nothing
        oCCTrans.Imported = 0
        oCCTrans.ClientIP = Request.ServerVariables("REMOTE_HOST")
        oCCTrans.BatchID = 0
        If oCCTrans.Save() Then
            ccTransID = oCCTrans.CCTransID
        Else
            Return False
            lblErr.Text = oCCTrans.Error_Message
        End If
        oCCTransApply.UserID = Session("UserDBID")
        If oCCTransApply.Create_Refund_Items(ccTransID, refundID) Then
            oCCTrans.Approved = 1
            oCCTrans.ApprovedBy = Session("UserDBID")
            oCCTrans.DateApproved = System.DateTime.Now
            oCCTrans.Save()
            oRefRequest.CCTransID = ccTransID
            oRefRequest.UserID = Session("UserDBID")
            oRefRequest.Save()
            bProcessed = True
        Else
            bProcessed = False
        End If
        oRefRequest = Nothing
        oCCTrans = Nothing
        oCCTransApply = Nothing
        oCombo = Nothing
        Return bProcessed
    End Function

    Protected Sub Unnamed1_Click1(sender As Object, e As System.EventArgs)
        Dim bProceed As Boolean = True
        Dim sErr As String = ""
        Dim refAmt As Double = 0
        For Each row As GridViewRow In gvCashPayments.Rows
            Dim cb As CheckBox = row.FindControl("cb")
            If cb.Checked Then
                Dim txtAmt As TextBox = row.FindControl("txtAmount")
                refAmt = CDbl(txtAmt.Text)
                If refAmt > CDbl(row.Cells(5).Text) Then
                    bProceed = False
                    sErr = "You Can Not Refund For More Than What Was Charged."
                End If
            End If
        Next

        If bProceed Then
            Dim oPayment As New clsPayments
            Dim oPmt2Invoice As New clsPayment2Invoice
            Dim oCombo As New clsComboItems
            Dim paymentID As Integer = 0
            For Each row As GridViewRow In gvCashPayments.Rows
                Dim cb As CheckBox = row.FindControl("cb")
                If cb.Checked Then
                    Dim txtAmt As TextBox = row.FindControl("txtAmount")
                    oPayment.PaymentID = 0
                    oPayment.Load()
                    oPayment.MethodID = oCombo.Lookup_ID("PaymentMethod", "Cash - Refund")
                    oPayment.Amount = CDbl(txtAmt.Text)
                    oPayment.UserID = Session("UserDBID")
                    oPayment.PosNeg = False
                    oPayment.TransDate = System.DateTime.Now
                    oPayment.Save()
                    paymentID = oPayment.PaymentID
                    oPmt2Invoice.Inv2PayID = 0
                    oPmt2Invoice.Load()
                    oPmt2Invoice.PaymentID = paymentID
                    oPmt2Invoice.InvoiceID = row.Cells(2).Text
                    oPmt2Invoice.Amount = CDbl(txtAmt.Text)
                    oPmt2Invoice.Save()
                End If
            Next
            oPayment = Nothing
            oPmt2Invoice = Nothing
            oCombo = Nothing
            ClientScript.RegisterClientScriptBlock(Me.GetType, "refresh", "window.opener.__doPostBack('" & Request("linkid") & "','');window.close();", True)
        Else
            lblErr3.Text = sErr
        End If
    End Sub

    Protected Sub Button2_Click(sender As Object, e As System.EventArgs) Handles Button2.Click
        Dim bProceed As Boolean = True
        Dim sErr As String = ""
        Dim refAmt As Double = 0
        Dim totalRefAmt As Double = 0
        Dim orefApply As New clsRefundRequestApplyTo
        Dim acctID As Integer = 0
        For Each row As GridViewRow In gvPayments.Rows
            Dim cb As CheckBox = row.FindControl("cb")
            If cb.Checked Then
                Dim txtAmt As TextBox = row.FindControl("txtAmount")
                refAmt = CDbl(txtAmt.Text)
                If refAmt > CDbl(row.Cells(5).Text) Then
                    bProceed = False
                    sErr = "You Can Not Refund For More Than What Was Charged."
                Else
                    Dim txtNumber As TextBox = row.FindControl("txtNumber")
                    If txtNumber.Text = "" Then
                        bProceed = False
                        sErr = "Please Enter a Check Number."
                    Else
                        If CDbl(refAmt) > Math.Round(CDbl(row.Cells(5).Text) - orefApply.Get_Ref_Amount_Per_Payment(CInt(row.Cells(1).Text)), 2) Then
                            bProceed = False
                            sErr = "Amount Requested Exceeds Amount Available to Refund for Payment Selected. " '& CDbl(refAmt) & " - " & Math.Round(CDbl(row.Cells(4).Text) - orefApply.Get_Ref_Amount_Per_Payment(CInt(row.Cells(1).Text)), 2)
                        Else
                            totalRefAmt += refAmt
                            acctID = row.Cells(6).Text
                        End If
                    End If

                End If
            End If
        Next

        If bProceed Then
            If totalRefAmt = 0 Then
                sErr = "You must select an Item to Refund"
                bProceed = False
            End If
        End If
        If bProceed Then
            Dim oPayment As New clsPayments
            Dim oPmt2Invoice As New clsPayment2Invoice
            Dim oCombo As New clsComboItems
            Dim paymentID As Integer = 0

            Dim orefRequest As New clsRefundRequest
            Dim refundID As Integer = 0
            orefRequest.RefundRequestID = 0
            orefRequest.Load()
            orefRequest.CreditCardID = 0
            orefRequest.AccountID = acctID
            orefRequest.Amount = totalRefAmt
            orefRequest.RequestedByID = Session("UserDBID")
            orefRequest.DateRequested = System.DateTime.Now
            orefRequest.ProspectID = Request("ProspectID")
            orefRequest.Approved = 1
            orefRequest.ApprovedByID = Session("UserDBID")
            orefRequest.DateApproved = System.DateTime.Now
            orefRequest.Save()
            refundID = orefRequest.RefundRequestID

            For Each row As GridViewRow In gvPayments.Rows
                Dim cb As CheckBox = row.FindControl("cb")
                If cb.Checked Then
                    Dim txtAmt As TextBox = row.FindControl("txtAmount")
                    Dim txtNumber As TextBox = row.FindControl("txtNumber")
                    oPayment.PaymentID = 0
                    oPayment.Load()
                    oPayment.MethodID = oCombo.Lookup_ID("PaymentMethod", "Check - Refund")
                    oPayment.Amount = CDbl(txtAmt.Text)
                    oPayment.UserID = Session("UserDBID")
                    oPayment.PosNeg = False
                    oPayment.TransDate = System.DateTime.Now
                    oPayment.Reference = "Check - " & txtNumber.Text
                    oPayment.Save()
                    paymentID = oPayment.PaymentID
                    oPmt2Invoice.Inv2PayID = 0
                    oPmt2Invoice.Load()
                    oPmt2Invoice.PaymentID = paymentID
                    oPmt2Invoice.InvoiceID = row.Cells(2).Text
                    oPmt2Invoice.Amount = CDbl(txtAmt.Text)
                    oPmt2Invoice.Save()
                    orefApply.RefundApplyID = 0
                    orefApply.RefundRequestID = refundID
                    orefApply.PaymentID = row.Cells(1).Text
                    orefApply.Amount = CDbl(txtAmt.Text)
                    orefApply.Save()
                End If
            Next
            oPayment = Nothing
            oPmt2Invoice = Nothing
            oCombo = Nothing
            orefApply = Nothing
            orefRequest = Nothing
            ClientScript.RegisterClientScriptBlock(Me.GetType, "refresh", "window.opener.__doPostBack('" & Request("linkid") & "','');window.close();", True)
        Else
            lblErr4.Text = sErr
        End If
    End Sub
End Class
