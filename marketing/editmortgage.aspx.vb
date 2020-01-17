
Partial Class marketing_editmortgage
    Inherits System.Web.UI.Page
    Dim oMort As New clsMortgage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            If CheckSecurity("Mortgage", "View", , , Session("UserDBID")) Then
                MultiView1.ActiveViewIndex = 0
                '*** Create view events *** '
                If IsNumeric(Request("MortgageID")) Then
                    If CInt(Request("MortgageID")) > 0 Then
                        Dim oE As New clsEvents
                        Dim sErr As String = ""

                        If Not (oE.Find_View_Event("MortgageID", Request("MortgageID"), Resources.Resource.ViewEventTime, Session("UserDBID"), sErr)) Then
                            If sErr <> "" Then Response.Write(sErr)
                            sErr = ""
                            If Not (oE.Create_View_Event("MortgageID", Request("MortgageID"), Resources.Resource.ViewEventTime, Session("UserDBID"), sErr)) Then
                                If sErr <> "" Then Response.Write("<br />" & sErr)
                            End If
                        End If
                        oE = Nothing
                    End If
                End If

                '*** End View Events *** '
                Load_Lookups()
                oMort.MortgageID = IIf(IsNumeric(Request("MortgageID")), Request("MortgageID"), 0)
                oMort.Load()
                If oMort.MortgageID > 0 Then
                    Dim sProsLink() As String = Split(oMort.Get_Owner_Link, "||")
                    Dim iProsID As Integer = sProsLink(0)
                    Dim sProsName As String = sProsLink(1)
                    lbProspect.Text = sProsName
                    lbProspect.Attributes.Add("pid", iProsID)
                    EquiantLoanInformation1.Set_Account(oMort.ContractID, False)
                    If oMort.Number <> EquiantLoanInformation1.EquiantAccount Then
                        oMort.Number = EquiantLoanInformation1.EquiantAccount
                        oMort.Save()
                    End If
                End If
                'Dim oPros As New clsProspect
                'oPros.Prospect_ID = IIf(IsNumeric(Request("ProspectID")), Request("ProspectID"), oMort)
                'oPros.Load()
                'lbProspect.Text = oPros.Last_Name & ", " & oPros.First_Name
                'oPros = Nothing
                'If oMort.ProspectID = 0 And IsNumeric(Request("ProspectID")) Then oMort.ProspectID = Request("ProspectID")

                'Dim newItem As New System.Web.UI.WebControls.ListItem("", 0)
                'ddOccupanyYear.Items.Add(newItem)
                'For i = 1998 To Date.Now.Year + 8
                ' newItem = New System.Web.UI.WebControls.ListItem(i, i)
                ' ddOccupanyYear.Items.Add(newItem)
                ' Next
                Set_Fields()
                lblMortError.Text = oMort.Error_Message
            Else
                MultiView1.ActiveViewIndex = 8
                txtMortgageID.text = -1
            End If
        End If
    End Sub

    Private Sub Set_Fields()
        txtMortgageID.Text = oMort.MortgageID
        siStatus.Selected_ID = oMort.StatusID
        siTitleType.Selected_ID = oMort.TitleTypeID
        txtNumber.Text = oMort.Number
        dfStatus.Selected_Date = oMort.StatusDate
        txtSalesVolume.Text = FormatCurrency(oMort.SalesVolume, 2)
        txtSalesPrice.Text = FormatCurrency(oMort.SalesPrice, 2)
        txtCommissionVolume.Text = FormatCurrency(oMort.CommVolume, 2)
        txtOrigPurchasePrice.Text = FormatCurrency(oMort.OrigSellingPrice, 2)
        cbFinanceCC.Checked = IIf(oMort.CCFinanced > 0, True, False)
        'txtDPTotal.Text = FormatCurrency(oMort.DPTotal, 2)
        'txtDPReceived.Text = FormatCurrency(0, 2) 'oMort.dpreceived
        'txtDPScheduled.Text = FormatCurrency(0, 2) 'oMort.dpscheduled
        'txtDPBalance.Text = FormatCurrency(oMort.DPBalance, 2)
        'txtCCTotal.Text = FormatCurrency(oMort.CCTotal, 2)
        'txtCCReceived.Text = FormatCurrency(0, 2) 'oMort.ccreceived
        'txtCCScheduled.Text = FormatCurrency(0, 2) 'oMort.ccscheduled
        'txtCCFinanced.Text = FormatCurrency(oMort.CCFinanced, 2)
        'txtCCBalance.Text = FormatCurrency(0, 2) 'oMort.ccbalance
        'txtMemTotal.Text = FormatCurrency(0, 2) 'oMort.memtotal
        'txtMemReceived.Text = FormatCurrency(0, 2) 'omort.memreceived
        'txtMemScheduled.Text = FormatCurrency(0, 2) 'omort.memschedule
        'txtMemBalance.Text = FormatCurrency(0, 2) 'omort.membalance
        dfOrigDate.Selected_Date = oMort.OrigDate
        dfFirstPaymentDate.Selected_Date = oMort.FirstPaymentDate
        'dfNextPaymentDate.Selected_Date=oMort.
        txtBalanceDue.Text = FormatCurrency(0, 2) 'oMort.BalanceDue
        siFrequency.Selected_ID = oMort.FrequencyID
        siInterestType.Selected_ID = oMort.InterestTypeID
        siPaymentType.Selected_ID = oMort.PaymentTypeID
        txtAPR.Text = oMort.APR
        txtTerms.Text = oMort.Terms
        txtPayProcFee.Text = FormatCurrency(oMort.PaymentFee, 2)
        txtPaymentProcessingFee.Text = FormatCurrency(oMort.PaymentFee, 2)
        txtOneTimeFee.Text = FormatCurrency(oMort.OneTimeFee, 2)
        txtTotalFin.Text = FormatCurrency(oMort.TotalFinanced, 2)
        txtTotalFinanced.Text = FormatCurrency(oMort.TotalFinanced, 2)
        'siGrace.Selected_ID 
        txtActualAmount.Text = FormatCurrency(oMort.PMT + oMort.PaymentFee, 2) 'oMort.actualamount
        txtFinanceCharges.Text = FormatCurrency((oMort.PMT * oMort.Terms) - oMort.TotalFinanced, 2) 'oMort.financecharges
        txtTotalPayments.Text = FormatCurrency(oMort.PMT * oMort.Terms, 2) 'omort.totalpayments
        txtTotalFees.Text = FormatCurrency(oMort.PaymentFee * oMort.Terms, 2) 'omort.totalfees
        txtPaidToday.Text = FormatCurrency(0, 2) 'omort.paidtoday
        txtTotalCost.Text = FormatCurrency((oMort.PMT * oMort.Terms), 2) 'omort.totalcost
        txtLoanPayment.Text = FormatCurrency(oMort.PMT, 2) 'oMort.loanpayment
        DP_Financials.KeyField = "MortgageDP"
        DP_Financials.KeyValue = oMort.MortgageID
        DP_Financials.View = "mortgagedp"
        Dim oContract As New clsContract
        oContract.ContractID = oMort.ContractID
        oContract.Load()
        DP_Financials.ProspectID = oContract.ProspectID
        oContract = Nothing
        DP_Financials.Display()
    End Sub

    

    Private Sub Load_Lookups()
        Dim sErr As String = ""
        siStatus.Connection_String = Resources.Resource.cns
        siStatus.Label_Caption = ""
        siStatus.ComboItem = "MortgageStatus"
        siStatus.Load_Items()
        siTitleType.Connection_String = Resources.Resource.cns
        siTitleType.Label_Caption = ""
        siTitleType.ComboItem = "MortgageTitleType"
        siTitleType.Load_Items()
        siFrequency.Connection_String = Resources.Resource.cns
        siFrequency.Label_Caption = ""
        siFrequency.ComboItem = "PaymentFrequency"
        siFrequency.Load_Items()
        siPaymentType.Connection_String = Resources.Resource.cns
        siPaymentType.Label_Caption = ""
        siPaymentType.ComboItem = "MortPmtType"
        siPaymentType.Load_Items()
        siInterestType.Connection_String = Resources.Resource.cns
        siInterestType.Label_Caption = ""
        siInterestType.ComboItem = "MortgageIntestType"
        siInterestType.Load_Items()
        siGrace.Connection_String = Resources.Resource.cns
        siGrace.Label_Caption = ""
        siGrace.ComboItem = "Grace"
        siGrace.Load_Items()

    End Sub

    Protected Sub Mortgage_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Mortgage_Link.Click
        If txtMortgageID.Text > 0 Then
            MultiView1.ActiveViewIndex = 0
        End If
    End Sub

    Protected Sub Purchase_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Purchase_Link.Click
        If txtMortgageID.Text > 0 Then
            MultiView1.ActiveViewIndex = 1
        End If
    End Sub

    Protected Sub Financing_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Financing_Link.Click
        If txtMortgageID.text > 0 Then
            MultiView1.ActiveViewIndex = 2
        End If
    End Sub

    Protected Sub Amortization_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Amortization_Link.Click
        If txtMortgageID.Text > 0 Then
            MultiView1.ActiveViewIndex = 3
            Recreate_Object()
            gvAmort.DataSource = oMort.Calc_Amortization
            gvAmort.DataBind()
        End If
    End Sub

    Protected Sub Events_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Events_Link.Click
        If txtMortgageID.Text > 0 Then
            MultiView1.ActiveViewIndex = 4
            ucEvents.KeyField = "MortgageID"
            ucEvents.KeyValue = txtMortgageID.Text
            ucEvents.List()
        End If
    End Sub

    Protected Sub Notes_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Notes_Link.Click
        If txtMortgageID.text > 0 Then
            MultiView1.ActiveViewIndex = 5
            Notes1.KeyValue = txtMortgageID.Text
            Notes1.Display()
        End If
    End Sub

    Protected Sub Payments_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Payments_Link.Click
        If txtMortgageID.Text > 0 Then
            Dim cn As New System.Data.SqlClient.SqlConnection(Resources.Resource.cns)
            Dim cm As New System.Data.SqlClient.SqlCommand("Select LoanTransID as ID, PricipalBalance as [Prin Balance],LateChargeBalance,OriginalAmount, TransDate,PaidPrincipal as [Paid Prin],PaidInterest as [Paid Int], Unapplied,ImpoundPaid, OtherPaid, NextDueDate from t_EquiantLoanTransactions l where l.contractid = (select contractid from t_Mortgage where mortgageid = " & txtMortgageID.Text & ")", cn)
            Dim dr As System.Data.SqlClient.SqlDataReader
            Try
                cn.Open()
                dr = cm.ExecuteReader
                gvPayments.DataSource = dr
                gvPayments.DataBind()
                cn.Close()
            Catch ex As Exception

            Finally
                If cn.State <> Data.ConnectionState.Closed Then cn.Close()
                cn = Nothing
                cm = Nothing
                dr = Nothing
            End Try
            MultiView1.ActiveViewIndex = 6
        End If
    End Sub


    Protected Sub UserFields_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UserFields_Link.Click
        If txtMortgageID.Text > 0 Then
            MultiView1.ActiveViewIndex = 7
            UF.KeyField = "Mortgage"
            UF.KeyValue = CInt(txtMortgageID.Text)
            UF.Load_List()
        End If
    End Sub

    Protected Sub Recreate_Object()
        oMort.MortgageID = IIf(IsNumeric(Request("MortgageID")), Request("MortgageID"), 0)
        oMort.Load()
        Dim oInv As New clsInvoices
        oInv.InvoiceID = oMort.DPInvoiceID
        oInv.Load()
        oMort.DPTotal = IIf(oInv.Amount > 0, oInv.Amount, oMort.DPTotal)
        oInv.InvoiceID = oMort.CCInvoiceID
        oInv.Load()
        oMort.CCTotal = IIf(oInv.Amount > 0, oInv.Amount, oMort.CCTotal)
        oMort.CCFinanced = IIf(cbFinanceCC.Checked, IIf(oInv.Amount > 0, oInv.Amount, oMort.CCFinanced), 0)
        oInv = Nothing
        oMort.StatusID = siStatus.Selected_ID
        oMort.TitleTypeID = siTitleType.Selected_ID
        oMort.Number = txtNumber.Text
        oMort.StatusDate = dfStatus.Selected_Date
        oMort.SalesVolume = txtSalesVolume.Text
        oMort.SalesPrice = txtSalesPrice.Text
        oMort.CommVolume = txtCommissionVolume.Text
        oMort.OrigSellingPrice = txtOrigPurchasePrice.Text
        oMort.TotalFinanced = oMort.SalesPrice + oMort.CCFinanced - oMort.DPTotal
        'oMort.DPTotal = oMort.
        oMort.OrigDate = dfOrigDate.Selected_Date
        oMort.FirstPaymentDate = dfFirstPaymentDate.Selected_Date
        'dfNextPaymentDate.Selected_Date=oMort.
        'txtBalanceDue.Text = FormatCurrency(0, 2) 'oMort.BalanceDue
        oMort.FrequencyID = siFrequency.Selected_ID
        oMort.InterestTypeID = siInterestType.Selected_ID
        oMort.PaymentTypeID = siPaymentType.Selected_ID
        oMort.APR = txtAPR.Text
        oMort.Terms = txtTerms.Text
        oMort.PaymentFee = txtPayProcFee.Text
        'oMort.PaymentFee = txtPaymentProcessingFee.Text
        oMort.OneTimeFee = txtOneTimeFee.Text
        'oMort.TotalFinanced = txtTotalFin.Text
        'oMort.TotalFinanced = txtTotalFinanced.Text
        'siGrace.Selected_ID 
        'txtLoanPayment.Text = FormatCurrency(0, 2) 'oMort.loanpayment
        'txtActualAmount.Text = FormatCurrency(0, 2) 'oMort.actualamount
        'txtFinanceCharges.Text = FormatCurrency(0, 2) 'oMort.financecharges
        'txtTotalPayments.Text = FormatCurrency(0, 2) 'omort.totalpayments
        'txtTotalFees.Text = FormatCurrency(0, 2) 'omort.totalfees
        'txtPaidToday.Text = FormatCurrency(0, 2) 'omort.paidtoday
        'txtTotalCost.Text = FormatCurrency(0, 2) 'omort.totalcost
        oMort.UserID = Session("UserDBID")
        Set_Fields()
    End Sub

 
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If CheckSecurity("Mortgage", "Edit", , , Session("UserDBID")) Then
            Recreate_Object()
            oMort.Save()
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "close", "alert('You Do Not Have Persmission To Edit A Mortgage Record.');", True)
        End If
    End Sub


    Protected Sub txtSalesVolume_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSalesVolume.TextChanged

    End Sub

    Protected Sub txtSalesPrice_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSalesPrice.TextChanged
        If IsNumeric(txtSalesPrice.Text) Then
            txtCommissionVolume.Text = FormatCurrency(CDbl(txtSalesPrice.Text) - 300, 2)
        Else
            txtCommissionVolume.Text = FormatCurrency(0, 2)
        End If

    End Sub

    Protected Sub lbCC_BreakOut_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCC_BreakOut.Click
        ClientScript.RegisterClientScriptBlock(Me.GetType, "none", "javascript:modal.mwindow.open('../general/mortgageclosingcosts.aspx?mortgageid=" & txtMortgageID.Text & "','win01',350,350);", True)
    End Sub

    Protected Sub lbUpdate_Costs_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbUpdate_Costs.Click
        Recreate_Object()
        oMort.Save()
        oMort.CCTotal = oMort.Update_CC_Defaults(oMort.SalesPrice, oMort.MortgageID, oMort.DPTotal)
        oMort.TotalFinanced = oMort.Update_Total_Financed()
        oMort.Save()
        Dim oCCInvoice As New clsInvoices
        oCCInvoice.InvoiceID = oMort.CCInvoiceID
        oCCInvoice.Load()
        oCCInvoice.Amount = oMort.CCTotal
        oCCInvoice.Save()
        oCCInvoice.InvoiceID = oMort.DPInvoiceID
        oCCInvoice.Load()
        oCCInvoice.Amount = oMort.DPTotal
        oCCInvoice.Save()
        Refresh_Invoices()
        oCCInvoice = Nothing
    End Sub

    Protected Sub lbProspect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbProspect.Click
        If lbProspect.Attributes.Item("pid") <> "" Then Response.Redirect("~/marketing/editprospect.aspx?prospectid=" & lbProspect.Attributes.Item("pid"))
    End Sub

    Private Sub Refresh_Invoices()
        Dim oCon As New clsContract
        oCon.ContractID = oMort.ContractID
        oCon.Load()
        DP_Financials.KeyField = "MortgageDP"
        DP_Financials.KeyValue = oMort.MortgageID
        DP_Financials.View = "mortgagedp"
        DP_Financials.ProspectID = oCon.ProspectID
        DP_Financials.Display()
        oCon = Nothing
    End Sub


    Private Sub gvPayments_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvPayments.RowDataBound
        If e.Row.Cells(0).Text <> "ID" Then
            e.Row.Cells(1).Text = FormatCurrency(e.Row.Cells(1).Text)
            e.Row.Cells(2).Text = FormatCurrency(e.Row.Cells(2).Text)
            e.Row.Cells(3).Text = FormatCurrency(e.Row.Cells(3).Text)
            e.Row.Cells(4).Text = FormatDateTime(e.Row.Cells(4).Text, DateFormat.ShortDate)
            e.Row.Cells(5).Text = FormatCurrency(e.Row.Cells(5).Text)
            e.Row.Cells(6).Text = FormatCurrency(e.Row.Cells(6).Text)
            e.Row.Cells(7).Text = FormatCurrency(e.Row.Cells(7).Text)
            e.Row.Cells(8).Text = FormatCurrency(e.Row.Cells(8).Text)
            e.Row.Cells(9).Text = FormatCurrency(e.Row.Cells(9).Text)
            e.Row.Cells(10).Text = FormatDateTime(e.Row.Cells(10).Text, DateFormat.ShortDate)
        End If
    End Sub
End Class
