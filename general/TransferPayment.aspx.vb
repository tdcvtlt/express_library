Imports Microsoft.VisualBasic
Partial Class general_TransferPayment
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oPayments As New clsPayments
            If Request("KeyField") = "ProspectID" Then
                gvPayments.DataSource = oPayments.List_Transferable_Payments(Request("KeyField"), Request("ProspectID"))
            Else
                gvPayments.DataSource = oPayments.List_Transferable_Payments(Request("KeyField"), Request("KeyValue"))
            End If
            gvPayments.DataBind()
            oPayments = Nothing
        End If
    End Sub

    Protected Sub cb_CheckedChanged(sender As Object, e As System.EventArgs)
        hfPaymentID.Value = CType(sender.Parent.parent, GridViewRow).Cells(1).Text
        hfPaymentAmt.Value = CType(sender.Parent.parent, GridViewRow).Cells(5).Text
        hfInvoiceID.Value = CType(sender.Parent.parent, GridViewRow).Cells(7).Text
        Dim pmtMethod As String = CType(sender.Parent.parent, GridViewRow).Cells(2).Text
        For Each i As Control In Me.Controls
            If TypeOf i Is HtmlForm Then
                For Each x As Control In i.Controls
                    If TypeOf x Is GridView Then
                        For Each y As Control In x.Controls
                            For Each z As Control In y.Controls
                                For Each f As Control In z.Controls
                                    If f.HasControls Then
                                        For Each r As Control In f.Controls
                                            If TypeOf r Is RadioButton And r IsNot sender Then
                                                If r.ID = "cb" Then
                                                    CType(r, RadioButton).Checked = False
                                                End If
                                            ElseIf r Is sender Then
                                                'lblErr.Text = CType(sender.Parent.parent, GridViewRow).Cells(1).Text
                                            End If
                                        Next
                                    End If
                                Next
                            Next
                        Next
                    End If
                Next
            End If
        Next
        If pmtMethod = "Equiant Payment" Then
            If CheckSecurity("Payments", "Transfer Equiant", , , Session("UserDBID")) Then
                If rbTransfer.Visible = False Then
                    rbTransfer.Visible = True
                    If Request("keyField") = "ContractID" Then
                        lblTransfer.Text = "Transfer To KCP#: "
                        rbTransfer.Items(0).Selected = True
                    ElseIf Request("KeyField") = "ReservationID" Then
                        lblTransfer.Text = "Transfer To ReservationID: "
                        rbTransfer.Items(1).Selected = True
                    ElseIf Request("KeyField") = "TourID" Then
                        lblTransfer.Text = "Trasfer To TourID: "
                        rbTransfer.Items(2).Selected = True
                    ElseIf Request("KeyField") = "PackageIssuedID" Then
                        lblTransfer.Text = "Transfer To PackageID: "
                        rbTransfer.Items(3).Selected = True
                    ElseIf Request("KeyField") = "MortgageID" Or Request("KeyField") = "MortgageDP" Then
                        lblTransfer.Text = "Tranfer To KCP#: "
                        rbTransfer.Items(4).Selected = True
                    ElseIf Request("KeyField") = "ProspectID" Then
                        lblTransfer.Text = "Transfer to ProspectID: "
                        rbTransfer.Items(6).Selected = True
                    Else
                        lblTransfer.Text = "Transfer To ConversionID: "
                        rbTransfer.Items(5).Selected = True
                    End If
                End If
                gvInvoices.DataSource = ""
                gvInvoices.DataBind()
                gvInvoices.Visible = False
                txtFilter.Text = ""
                txtFilter.Visible = True
                btnsearch.Visible = True
                lblErr.Text = ""
                txtFilter.Focus()
            Else
                lblErr.ForeColor = Drawing.Color.Red
                lblErr.Text = "You Do Not Have Permission to Transfer Equiant Payments."
                gvInvoices.DataSource = ""
                gvInvoices.DataBind()
                gvInvoices.Visible = False
                txtFilter.Text = ""
                txtFilter.Visible = False
                btnsearch.Visible = False
                rbTransfer.Visible = False
                lblTransfer.Visible = False

            End If
        Else
                If rbTransfer.Visible = False Then
                rbTransfer.Visible = True
                If Request("keyField") = "ContractID" Then
                    lblTransfer.Text = "Transfer To KCP#: "
                    rbTransfer.Items(0).Selected = True
                ElseIf Request("KeyField") = "ReservationID" Then
                    lblTransfer.Text = "Transfer To ReservationID: "
                    rbTransfer.Items(1).Selected = True
                ElseIf Request("KeyField") = "TourID" Then
                    lblTransfer.Text = "Trasfer To TourID: "
                    rbTransfer.Items(2).Selected = True
                ElseIf Request("KeyField") = "PackageIssuedID" Then
                    lblTransfer.Text = "Transfer To PackageID: "
                    rbTransfer.Items(3).Selected = True
                ElseIf Request("KeyField") = "MortgageID" Or Request("KeyField") = "MortgageDP" Then
                    lblTransfer.Text = "Tranfer To KCP#: "
                    rbTransfer.Items(4).Selected = True
                ElseIf Request("KeyField") = "ProspectID" Then
                    lblTransfer.Text = "Transfer to ProspectID: "
                    rbTransfer.Items(6).Selected = True
                Else
                    lblTransfer.Text = "Transfer To ConversionID: "
                    rbTransfer.Items(5).Selected = True
                End If
            End If
            gvInvoices.DataSource = ""
            gvInvoices.DataBind()
            gvInvoices.Visible = False
            txtFilter.Text = ""
            txtFilter.Visible = True
            btnsearch.Visible = True
            lblErr.Text = ""
            txtFilter.Focus()
        End If
    End Sub

    Protected Sub rbTransfer_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles rbTransfer.SelectedIndexChanged
        Select Case rbTransfer.SelectedIndex
            Case 0, 4
                lblTransfer.Text = "Transfer To KCP#: "
            Case 1
                lblTransfer.Text = "Transfer To ReservationID: "
            Case 2
                lblTransfer.Text = "Transfer To TourID: "
            Case 3
                lblTransfer.Text = "Transfer To PackageID: "
            Case 5
                lblTransfer.Text = "Transfer To ConversionID: "
            Case 6
                lblTransfer.Text = "Transfer to ProspectID: "
        End Select
        gvInvoices.DataSource = ""
        gvInvoices.DataBind()
        gvInvoices.Visible = False
        txtFilter.Text = ""
        txtFilter.Focus()
    End Sub

    Protected Sub btnsearch_Click(sender As Object, e As System.EventArgs) Handles btnsearch.Click
        lblErr.Text = ""
        Dim bValid As Boolean = True
        Dim keyField As String = ""
        Dim keyValue As Integer = 0
        If rbTransfer.SelectedIndex <> 4 And rbTransfer.SelectedIndex <> 0 And (txtFilter.Text = "" Or Not (IsNumeric(txtFilter.Text))) Then
            bValid = False
        Else
            Select Case rbTransfer.SelectedIndex
                Case 0, 4
                    Dim oContract As New clsContract
                    bValid = oContract.Verify_Contract(txtFilter.Text)
                    If bValid Then
                        oContract.ContractID = oContract.Get_Contract_ID(txtFilter.Text)
                        oContract.Load()
                        hfProsID.Value = oContract.ProspectID
                        If rbTransfer.SelectedIndex = 0 Then
                            keyValue = oContract.Get_Contract_ID(txtFilter.Text)
                            keyField = "ContractID"
                        Else
                            Dim oMortgage As New clsMortgage
                            keyValue = oMortgage.Get_Mortgage_ID(oContract.Get_Contract_ID(txtFilter.Text))
                            keyField = "MortgageID"
                            lblErr.Text = keyField & " " & keyValue
                            oMortgage = Nothing
                        End If
                    End If
                    oContract = Nothing
                Case 1
                    Dim oReservation As New clsReservations
                    bValid = oReservation.val_ResID(txtFilter.Text)
                    If bValid Then
                        oReservation.ReservationID = txtFilter.Text
                        oReservation.Load()
                        hfProsID.Value = oReservation.ProspectID
                    End If
                    keyField = "ReservationID"
                    keyValue = CInt(txtFilter.Text)
                    oReservation = Nothing
                Case 2
                    Dim oTour As New clsTour
                    bValid = oTour.val_TourID(txtFilter.Text)
                    If bValid Then
                        oTour.TourID = txtFilter.Text
                        oTour.Load()
                        hfProsID.Value = oTour.ProspectID
                    End If
                    keyField = "TourID"
                    keyValue = CInt(txtFilter.Text)
                    oTour = Nothing
                Case 3
                    Dim oPkg As New clsPackageIssued
                    bValid = oPkg.val_PkgID(txtFilter.Text)
                    If bValid Then
                        oPkg.PackageIssuedID = txtFilter.Text
                        oPkg.Load()
                        hfProsID.Value = oPkg.ProspectID
                    End If
                    keyField = "PackageIssuedID"
                    keyValue = CInt(txtFilter.Text)
                    oPkg = Nothing
                Case 5
                    Dim oConv As New clsConversion
                    bValid = oConv.val_ConversionID(txtFilter.Text)
                    If bValid Then
                        oConv.ConversionID = txtFilter.Text
                        oConv.Load()
                        hfProsID.Value = oConv.ProspectID
                    End If
                    keyField = "ConversionID"
                    keyValue = CInt(txtFilter.Text)
                    oConv = Nothing
                Case 6
                    Dim oPros As New clsProspect
                    bValid = oPros.val_ProspectID(txtFilter.Text)
                    If bValid Then
                        hfProsID.Value = CInt(txtFilter.Text)
                    End If
                    keyField = "ProspectID"
                    keyValue = CInt(txtFilter.Text)
                    oPros = Nothing
            End Select
        End If
        If bValid Then
            Dim extraRow As Boolean = True
            If keyField = "ConversionID" Then
            ElseIf keyField = "MortgageID" Then
                If Request("KeyField") <> "MortgageDP" And Request("KeyField") <> "MortgageCC" Then
                    extraRow = False
                End If
            ElseIf keyField = "ContractID" Then
                If Request("KeyField") <> "ContractID" And Request("KeyField") <> "ProspectID" Then
                    extraRow = False
                End If
            Else
                If keyField <> Request("KeyField") Then
                    extraRow = False
                End If
            End If
            Dim oInvoice As New clsInvoices
            gvInvoices.DataSource = oInvoice.List_Transfer_Invoices(keyField, keyValue, extraRow)
            gvInvoices.DataBind()
            gvInvoices.Visible = True
            btnTransfer.Visible = True
            txtFilter.Focus()
            hfKeyField.value = keyField
            hfkeyValue.Value = keyValue
        Else
            gvInvoices.DataSource = ""
            gvInvoices.DataBind()
            gvInvoices.Visible = False
            txtFilter.Focus()
            btnTransfer.Visible = False
            hfKeyField.value = 0
            hfkeyValue.Value = 0
            lblErr.Text = "Not Found"
        End If
    End Sub

    Protected Sub gvInvoices_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Invoices For This Account" Then
            If e.Row.RowIndex > 0 Then
                e.Row.Cells(3).Text = CDate(e.Row.Cells(3).Text).ToShortDateString
                'e.Row.Cells(4).Text = FormatCurrency(e.Row.Cells(4).Text, 2)
                'e.Row.Cells(5).Text = FormatCurrency(e.Row.Cells(5).Text, 2)
            End If
            e.Row.Cells(1).Visible = False
        End If
    End Sub
    Protected Sub gvPayments_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Payments To Transfer" Then
            If e.Row.RowIndex > -1 Then
                e.Row.Cells(4).Text = CDate(e.Row.Cells(4).Text).ToShortDateString
            End If
            e.Row.Cells(7).Visible = False
        End If
    End Sub
    Protected Sub btnTransfer_Click(sender As Object, e As System.EventArgs) Handles btnTransfer.Click
        If CheckSecurity("Payments", "TransferPayments", , , Session("UserDBID")) Then
            Dim bProceed As Boolean = True
            Dim sErr As String = ""
            lblErr.Text = sErr
            Dim totalMove As Double = 0
            For Each row As GridViewRow In gvInvoices.Rows
                If CType(row.FindControl("cb"), CheckBox).Checked Then
                    Dim txtAmt As TextBox = row.FindControl("txtAmt")
                    If txtAmt.Text = "" Or Not (IsNumeric(txtAmt.Text)) Then
                        bProceed = False
                        sErr = "Please Enter a Valid Amount."
                    ElseIf row.Cells(2).Text = "Create Same Invoice" Then
                        totalMove = totalMove + CDbl(txtAmt.Text)
                    ElseIf CDbl(txtAmt.Text) > CDbl(row.Cells(5).Text) Then
                        bProceed = False
                        sErr = "You Can Not Transfer More Than What is Owed."
                    Else
                        totalMove = totalMove + CDbl(txtAmt.Text)
                    End If
                End If
            Next
            If bProceed Then
                If totalMove = 0 Then
                    bProceed = False
                    sErr = "Please Enter a Transfer Amount."
                ElseIf totalMove > CDbl(hfPaymentAmt.Value) Then
                    bProceed = False
                    sErr = "You Can Not Move More Than The Payment Amount."
                End If
            End If

            If bProceed Then
                Dim oPayment As New clsPayments
                Dim movableAmt As Double = oPayment.Get_Moved_Amount(hfPaymentID.Value)
                If CDbl(hfPaymentAmt.Value) - movableAmt < totalMove Then
                    bProceed = False
                    sErr = "This Payment Only Has " & CDbl(hfPaymentAmt.Value) - movableAmt & " Left to Transfer."
                End If
            End If

            If bProceed Then
                Dim oInvoiceA As New clsInvoices
                Dim oInvoiceB As New clsInvoices
                Dim oFinTrans As New clsFinancialTransactionCodes
                Dim oCombo As New clsComboItems
                Dim oPayment As New clsPayments
                Dim oPmt2Inv As New clsPayment2Invoice
                Dim invID As Integer = 0
                Dim pmtID As Integer = 0
                For Each row As GridViewRow In gvInvoices.Rows
                    If CType(row.FindControl("cb"), CheckBox).Checked Then
                        Dim txtAmt As TextBox = row.FindControl("txtAmt")
                        If row.Cells(1).Text = "0" Then
                            'Create Destination Invoice
                            oInvoiceA.InvoiceID = hfInvoiceID.Value
                            oInvoiceA.Load()
                            oInvoiceB.InvoiceID = 0
                            oInvoiceB.Load()
                            oInvoiceB.ProspectID = hfProsID.Value
                            oInvoiceB.FinTransID = oInvoiceA.FinTransID
                            oInvoiceB.KeyValue = hfKeyValue.Value
                            oInvoiceB.TransDate = System.DateTime.Now
                            oInvoiceB.Amount = txtAmt.Text
                            oInvoiceB.UserID = Session("UserDBID")
                            If hfKeyField.Value = "MortgageID" Or hfKeyField.Value = "ConversionID" Then
                                oFinTrans.FinTransID = oInvoiceA.FinTransID
                                oFinTrans.Load()
                                oInvoiceB.KeyField = oCombo.Lookup_ComboItem(oFinTrans.TransTypeID)
                            Else
                                oInvoiceB.KeyField = hfKeyField.Value
                            End If
                            oInvoiceB.Save()
                            invID = oInvoiceB.InvoiceID
                        Else
                            'Destination Invoice
                            oInvoiceA.InvoiceID = hfInvoiceID.Value
                            oInvoiceA.Load()
                            invID = row.Cells(1).Text
                        End If

                        'Create Destination Payment
                        oPayment.PaymentID = 0
                        oPayment.Load()
                        oPayment.Amount = txtAmt.Text
                        oPayment.UserID = Session("UserDBID")
                        oPayment.MethodID = oCombo.Lookup_ID("PaymentMethod", "Payment Move")
                        oPayment.PosNeg = True
                        oPayment.ApplyToID = 0
                        oPayment.TransDate = System.DateTime.Now
                        oPayment.Description = "Moved From PaymentID " & hfPaymentID.Value
                        If Request("KeyField") = "ContractID" Then
                            Dim oKCP As New clsContract
                            oKCP.ContractID = Request("KeyValue")
                            oKCP.Load()
                            oPayment.Reference = "Moved From KCP " & oKCP.ContractNumber
                            oKCP = Nothing
                        Else
                            If Request("KeyValue") = 0 Then
                                oPayment.Reference = "Moved From " & oInvoiceA.KeyField & " " & oInvoiceA.KeyValue
                            Else
                                oPayment.Reference = "Moved From " & Request("KeyField") & " " & Request("KeyValue")
                            End If
                        End If
                        oPayment.Save()
                        pmtID = oPayment.PaymentID()


                        'Create Destination Tie
                        oPmt2Inv.Inv2PayID = 0
                        oPmt2Inv.PaymentID = pmtID
                        oPmt2Inv.InvoiceID = invID
                        oPmt2Inv.Amount = txtAmt.Text
                        oPmt2Inv.PosNeg = True
                        oPmt2Inv.Save()

                        'Create PaymentMove For Originating Payment
                        oPayment.PaymentID = 0
                        oPayment.Load()
                        oPayment.Amount = txtAmt.Text
                        oPayment.UserID = Session("UserDBID")
                        oPayment.MethodID = oCombo.Lookup_ID("PaymentMethod", "Payment Move")
                        oPayment.PosNeg = False
                        oPayment.Adjustment = False
                        oPayment.Description = "Moved To PaymentID " & pmtID
                        oPayment.Reference = "Moved To " & rbTransfer.SelectedItem.Text & " " & txtFilter.Text
                        oPayment.ApplyToID = hfPaymentID.Value
                        oPayment.Save()

                        'Check for Adjusting Off Balance of Origination Invoice
                        If chkAdjust.Checked Then
                            Dim oinv As New clsInvoices
                            oinv.InvoiceID = 0
                            oinv.Load()
                            oinv.ProspectID = oInvoiceA.ProspectID
                            oinv.KeyField = oInvoiceA.KeyField
                            oinv.KeyValue = oInvoiceA.KeyValue
                            oinv.Amount = txtAmt.Text
                            oinv.UserID = Session("UserDBID")
                            oinv.TransDate = System.DateTime.Now
                            oinv.PaymentMethodID = oCombo.Lookup_ID("InvoiceAdjustments", "Adjustment")
                            oinv.ApplyToID = oInvoiceA.InvoiceID
                            oinv.Adjustment = True
                            oinv.PosNeg = True
                            oinv.Description = "Adjusting Balance Off After Payment Move."
                            oinv.Reference = "Payment Move Adjustment"
                            oinv.Save()
                            oinv = Nothing

                            '**** Replace Adjustment Payment with invoice adjustment on 8/8/12
                            'oPayment.PaymentID = 0
                            'oPayment.Load()
                            'oPayment.Amount = txtAmt.Text
                            'oPayment.MethodID = oCombo.Lookup_ID("PaymentMethod", "Adjustment")
                            'oPayment.UserID = Session("UserDBID")
                            'oPayment.TransDate = System.DateTime.Now
                            'oPayment.ApplyToID = 0
                            'oPayment.Adjustment = True
                            'oPayment.PosNeg = True
                            'oPayment.Save()
                            'pmtID = oPayment.PaymentID

                            'Create Tie
                            'oPmt2Inv.Inv2PayID = 0
                            'oPmt2Inv.PaymentID = pmtID
                            'oPmt2Inv.InvoiceID = hfInvoiceID.Value
                            'oPmt2Inv.Amount = txtAmt.Text
                            'oPmt2Inv.PosNeg = True
                            'oPmt2Inv.Save()
                        End If
                    End If
                Next
                ClientScript.RegisterClientScriptBlock(Me.GetType, "refresh", "window.opener.__doPostBack('" & Request("linkid") & "','');window.close();", True)
                oInvoiceA = Nothing
                oInvoiceB = Nothing
                oPayment = Nothing
                oPmt2Inv = Nothing
                oFinTrans = Nothing
                oCombo = Nothing
            Else
                lblErr.Text = sErr
                btnTransfer.Focus()
            End If
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "refresh", "alert('You Do Not Have Persmission To Transfer Payments.');", True)
        End If

    End Sub
End Class
