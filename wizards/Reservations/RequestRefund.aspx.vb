Imports System.Data
Imports System.Reflection
Imports System.Data.SqlClient
Imports System.Web.Services
Imports System.Collections.Generic
Imports System.Web.Script.Serialization
Imports System.Web.Script.Services

Imports clsReservationWizard

Partial Class Wizards_Reservations_RequestRefund
    Inherits System.Web.UI.Page

    Private package_base As New Base_Package
    Private wiz As New Wizard

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        Dim ph = CType(Me.Master.Master.FindControl("ContentPlaceHolder1"), ContentPlaceHolder)
        Dim wiz_data = CType(ph.FindControl("LB_WIZ_DATA"), Label)

        If Session("wizData" + Session("wizGuid")) <> Nothing Then
            wiz = New JavaScriptSerializer().Deserialize(Of Wizard)(Session("wizData" + Session("wizGuid")))
        Else
            wiz = New JavaScriptSerializer().Deserialize(Of Wizard)(wiz_data.Text)
        End If
        Session("wizData" + wiz.GUID_TIMESTAMP) = New JavaScriptSerializer().Serialize(wiz)
        Session("wizGuid") = wiz.GUID_TIMESTAMP

        If Not IsPostBack Then
            ddRefMethod.Items.Add(New ListItem("", "0"))
            'ddRefMethod.Items.Add("Check Request")
            ddRefMethod.Items.Add("Credit Card Refund")
            'ddRefMethod.Items.Add("Cash Refund")
        End If

        lb_Refund_Balance.Text = New clsFinancials().Get_Balance("ReservationID", wiz.Reservation.ReservationID, 0)
        lblErr.Text = String.Empty
        Session("wizData" + wiz.GUID_TIMESTAMP) = New JavaScriptSerializer().Serialize(wiz)
    End Sub

    Protected Sub ddRefMethod_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddRefMethod.SelectedIndexChanged

        Dim oPayments As New clsPayments
        Dim oCCTrans As New clsCCTrans

        Dim reservation_id = wiz.Reservation.ReservationID

        If ddRefMethod.SelectedValue = "Credit Card Refund" Then
            MultiView1.ActiveViewIndex = 0
            gvCCTrans.DataSource = oCCTrans.List_CC_Trans_For_Refund("reservationid", reservation_id)
            gvCCTrans.DataBind()

        ElseIf ddRefMethod.SelectedValue = "Cash Refund" Then

            MultiView1.ActiveViewIndex = 1

            gvCashPayments.DataSource = oPayments.List_Cash_Payments("reservationid", reservation_id)
            gvCashPayments.DataBind()
            lblErr3.Text = oPayments.Error_Message

        ElseIf ddRefMethod.SelectedValue = "Check Request" Then

            MultiView1.ActiveViewIndex = 2

            If CheckSecurity("Payments", "ProcessCheckRefunds", , , wiz.USER_ID) = False Then
                lblErr4.Text = "ACCESS DENIED"
                btSubmit_Check_Refund.Visible = False
            Else
                oPayments.KeyField = "reservationid"
                oPayments.KeyValue = reservation_id

                gvPayments.DataSource = oPayments.List_Check_Refund_Payments("reservationid", reservation_id)
                gvPayments.DataBind()
            End If
            btSubmit_Check_Refund.Visible = IIf(gvPayments.Rows.Count > 0, True, False)
        End If
        oCCTrans = Nothing
        oPayments = Nothing
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

    Protected Sub btCancel_Click(sender As Object, e As System.EventArgs) Handles btCancel.Click
        MultiView1.ActiveViewIndex = 0
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
        oCCTrans.CreatedByID = wiz.USER_ID ' Session("UserDBID")
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
            lblErr.Text = oCCTrans.Error_Message
            Return False
        End If
        oCCTransApply.UserID = wiz.USER_ID 'Session("UserDBID")
        If oCCTransApply.Create_Refund_Items(ccTransID, refundID) Then
            oCCTrans.Approved = 1
            oCCTrans.ApprovedBy = wiz.USER_ID ' Session("UserDBID")
            oCCTrans.DateApproved = System.DateTime.Now
            oCCTrans.Save()
            oRefRequest.CCTransID = ccTransID
            oRefRequest.UserID = wiz.USER_ID ' Session("UserDBID")
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

    Protected Sub btNext_Click(sender As Object, e As System.EventArgs) Handles btNext.Click
        Dim bt = CType(sender, Button)
        bt.Attributes.Add("nav", 1)
        Navigate(bt)
    End Sub

#Region "Subs/Functions"

    Private Sub Navigate(bt As Button)
        Dim gt = Me.Master.GetType()
        Dim mi As MethodInfo = gt.GetMethod("Navigate")
        Dim parameters = mi.GetParameters()

        Dim dir = Convert.ToInt32(bt.Attributes("nav"))
        Dim parametersArray() As Object = New Object() {Request.PhysicalPath.Substring(Request.PhysicalPath.LastIndexOf("\") + 1).ToLower(), dir}
        Dim r = mi.Invoke(Me.Master, parametersArray)
        Response.Redirect(r.ToString())
    End Sub

#End Region
    Protected Sub btSubmit_Cash_Refund_Click(sender As Object, e As System.EventArgs) Handles btSubmit_Cash_Refund.Click
        Dim bProceed As Boolean = True
        Dim sErr As String = ""
        Dim refAmt As Double = 0
        Dim reservation_id = wiz.Reservation.ReservationID

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
                    oPayment.UserID = wiz.USER_ID ' Session("UserDBID")
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

            Session("wizData" + wiz.GUID_TIMESTAMP) = New JavaScriptSerializer().Serialize(wiz)

            If New clsFinancials().Get_Balance("ReservationID", reservation_id, 0).ToString("c") < 0 Then
                ddRefMethod_SelectedIndexChanged(Nothing, EventArgs.Empty)
            Else
                btNext_Click(btNext, EventArgs.Empty)
            End If
        Else
            lblErr3.Text = sErr
        End If
    End Sub
    Protected Sub btSubmit_Credit_Refund_Click(sender As Object, e As System.EventArgs) Handles btSubmit_Credit_Refund.Click      
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

        Dim balance_amt = New clsFinancials().Get_Balance("ReservationID", wiz.Reservation.ReservationID, 0)
        If totalRefAmt > Math.Abs(balance_amt) Then
            lblErr.Text = String.Format("Amount to refund can not be greater than ${0}", Math.Abs(balance_amt))
            Return
        End If
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
            orefRequest.RequestedByID = wiz.USER_ID 'Session("UserDBID")
            orefRequest.DateRequested = System.DateTime.Now
            orefRequest.ProspectID = wiz.Prospect.Prospect_ID

            If hfRefRequest.Value = "False" Then
                orefRequest.Approved = 1
                orefRequest.ApprovedByID = wiz.USER_ID 'Session("UserDBID")
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
                    lb_Refund_Balance.Text = New clsFinancials().Get_Balance("ReservationID", wiz.Reservation.ReservationID, 0)
                    MultiView1.SetActiveView(CCView)
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
    Protected Sub btSubmit_Check_Refund_Click(sender As Object, e As System.EventArgs) Handles btSubmit_Check_Refund.Click

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
            orefRequest.RequestedByID = wiz.USER_ID ' Session("UserDBID")
            orefRequest.DateRequested = System.DateTime.Now
            orefRequest.ProspectID = wiz.Prospect.Prospect_ID
            orefRequest.Approved = 1
            orefRequest.ApprovedByID = wiz.USER_ID ' Session("UserDBID")
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
                    oPayment.UserID = wiz.USER_ID ' Session("UserDBID")
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
            If New clsFinancials().Get_Balance("ReservationID", wiz.Reservation.ReservationID, 0).ToString("c") < 0 Then
                ddRefMethod_SelectedIndexChanged(Nothing, EventArgs.Empty)
            Else
                btNext_Click(btNext, EventArgs.Empty)
            End If
        Else
            lblErr4.Text = sErr
        End If
    End Sub
    Protected Sub tmrCheck_Tick(sender As Object, e As System.EventArgs) Handles tmrCheck.Tick
        MultiView1.SetActiveView(CCProgress)
        tmrCheck.Enabled = False
        If String.IsNullOrEmpty(hfTickCounter.Value) Then
            hfTickCounter.Value = 0
        End If
        hfTickCounter.Value += 1
        lblWaiting.Text = "Processing ... please wait <br/>"
        lblWaiting.Text += hfTickCounter.Value & " seconds"
      
        Dim check_icv_response As Func(Of Int32, Boolean) _
            = Function(cc_id As Int32)
                  Dim tf As Boolean = False
                  With New clsCCTrans
                      .CCTransID = cc_id
                      .Load()
                      If String.IsNullOrEmpty(.ICVResponse) Then
                          tf = False
                      Else
                          If .ICVResponse.Substring(0, 1) <> "N" Then
                              tf = True
                          Else
                              tf = False
                          End If
                      End If
                  End With
                  Return tf
              End Function

        If hfTickCounter.Value >= 50 Then
            Using cn = New SqlConnection(Resources.Resource.cns)
                Using cmd = New SqlCommand(String.Format("update t_CCTrans set icvResponse='NDecline' where CCTransID={0};" &
                        "insert into t_Payments " &
                        "select (select ci.ComboItemID from t_ComboItems ci inner join t_Combos c on ci.ComboID = c.ComboID " &
                        "where c.ComboName = 'PaymentMethod' and ci.ComboItem = 'Card Declined'), 0, 0, " &
                        "(select amount from t_CCTrans where CCTransID={0}), " &
                        "(select PaymentID from t_CCTransApplyTo where CCTransID={0}), 0, GETDATE(), null, 'Card Declined', 'Card Declined'",
                        hfCCTransID.Value, wiz.USER_ID), cn)
                    Try
                        cn.Open()
                        Dim ra = cmd.ExecuteNonQuery()
                    Catch ex As Exception
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using
            hfTickCounter.Value = 1
            MultiView1.SetActiveView(CCView)
        ElseIf check_icv_response(hfCCTransID.Value) Then

            hfTickCounter.Value = 1
            lb_Refund_Balance.Text = New clsFinancials().Get_Balance("ReservationID", wiz.Reservation.ReservationID, 0)

            Dim b = 0
            Dim textbox_amount As TextBox = Nothing

            For Each row As GridViewRow In gvCCApply.Rows
                Dim cb As CheckBox = row.FindControl("cb")
                If cb.Checked Then
                    textbox_amount = row.FindControl("txtAmount")
                End If
            Next

            lblErr.Text = "Refund Trans Successful."

            'there are still credit to refund
            If Decimal.Parse(lb_Refund_Balance.Text) < 0 Then

                'update the credit balance
                If b > 0 Then
                    textbox_amount.Text = b
                Else    'select different refund method other than credit card by clicking the dropdown
                    'hide Process Refund button
                    btSubmit_Credit_Refund.Visible = False
                    'hide Cancel button
                    btCancel.Visible = False
                End If
            Else
                'hide Process Refund button
                btSubmit_Credit_Refund.Visible = False
                'hide Cancel button
                btCancel.Visible = False
            End If
        Else
            tmrCheck.Enabled = True
        End If
    End Sub
End Class
