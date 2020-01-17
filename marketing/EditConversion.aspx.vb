
Partial Class marketing_EditConversion
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then

            ddlYear.Items.Clear()
            'Club First Occupancy dropdown control
            'Go back 2 years, forward 3 years plus current year
            Dim year_int As Integer = DateTime.Now.Year
            Dim year_array() As String = {"(empty)", year_int - 2, year_int - 1, year_int, year_int + 1, year_int + 2, year_int + 3}

            ddlYear.DataSource = year_array
            ddlYear.DataBind()
            '
            'End of Club First Occupancy
            '

            Dim oConversion As New clsConversion
            MultiView1.ActiveViewIndex = 0
            Load_Lookups()
            oConversion.ConversionID = IIf(IsNumeric(Request("ConversionID")), Request("ConversionID"), 0)
            oConversion.Load()
            Set_Fields()
            DP_Financials.View = "MortgageDP"
            DP_Financials.KeyField = "ConversionDP"
            DP_Financials.KeyValue = oConversion.ConversionID
            DP_Financials.ProspectID = oConversion.ProspectID
            DP_Financials.Display()
            Dim oPros As New clsProspect
            If Request("ConversionID") = 0 Then
                Dim oCOn As New clsContract
                oCOn.ContractID = Request("ContractID")
                oCOn.Load()
                oPros.Prospect_ID = oCOn.ProspectID
                oCOn = Nothing
            Else
                oPros.Prospect_ID = oConversion.ProspectID
                EquiantLoanInformation1.Set_Account(oConversion.ContractID, True)
                If oConversion.ConversionNumber <> EquiantLoanInformation1.EquiantAccount Then
                    oConversion.ConversionNumber = EquiantLoanInformation1.EquiantAccount
                    oConversion.Save()
                End If
            End If

            oPros.Load()
            lbProspect.Text = oPros.Last_Name & ", " & oPros.First_Name '& " .. " & oPros.Prospect_ID & " .. " & oContract.ProspectID
            txtProspectID.Text = oPros.Prospect_ID
            oPros = Nothing
            oConversion = Nothing

     
        End If
    End Sub

    Private Sub Set_Fields()
        Dim oConversion As New clsConversion
        oConversion.ConversionID = Request("ConversionID")
        oConversion.Load()
        txtConversionID.Text = oConversion.ConversionID
        siStatus.Selected_ID = oConversion.StatusID
        siTitleType.Selected_ID = oConversion.TitleTypeID
        txtNumber.Text = oConversion.ConversionNumber
        dfStatus.Selected_Date = oConversion.StatusDate
        If Request("ConversionID") = 0 Then
            txtSalesVolume.Text = FormatCurrency(CDbl(2995), 2)
            txtSalesPrice.Text = FormatCurrency(CDbl(2995), 2)
            txtCommissionVolume.Text = FormatCurrency(CDbl(2995), 2)
        Else
            txtSalesVolume.Text = FormatCurrency(oConversion.SalesVolume, 2)
            txtSalesPrice.Text = FormatCurrency(oConversion.SalesPrice, 2)
            txtCommissionVolume.Text = FormatCurrency(oConversion.CommissionVolume, 2)
        End If
        txtOrigPurchasePrice.Text = FormatCurrency(oConversion.OrigSellingPrice, 2)
        dfOrigDate.Selected_Date = oConversion.OriginationDate
        dfFirstPaymentDate.Selected_Date = oConversion.FirstPaymentDate
        txtBalanceDue.Text = FormatCurrency(0, 2) 'oConversion.BalanceDue
        siFrequency.Selected_ID = oConversion.FrequencyID
        siInterestType.Selected_ID = oConversion.InterestTypeID
        siPaymentType.Selected_ID = oConversion.PaymentTypeID
        txtAPR.Text = oConversion.APR
        txtTerms.Text = oConversion.Terms
        txtPayProcFee.Text = FormatCurrency(oConversion.PaymentFee, 2)
        txtPaymentProcessingFee.Text = FormatCurrency(oConversion.PaymentFee, 2)
        txtOneTimeFee.Text = FormatCurrency(oConversion.OneTimeFee, 2)
        txtTotalFin.Text = FormatCurrency(oConversion.TotalFinanced, 2)
        txtTotalFinanced.Text = FormatCurrency(oConversion.TotalFinanced, 2)
        'siGrace.Selected_ID 
        txtActualAmount.Text = FormatCurrency(oConversion.PMT + oConversion.PaymentFee, 2) 'oConversion.actualamount
        txtFinanceCharges.Text = FormatCurrency((oConversion.PMT * oConversion.Terms) - oConversion.TotalFinanced, 2) 'oConversion.financecharges
        txtTotalPayments.Text = FormatCurrency(oConversion.PMT * oConversion.Terms, 2) 'oConversion.totalpayments
        txtTotalFees.Text = FormatCurrency(oConversion.PaymentFee * oConversion.Terms, 2) 'oConversion.totalfees
        txtPaidToday.Text = FormatCurrency(0, 2) 'oConversion.paidtoday
        txtTotalCost.Text = FormatCurrency((oConversion.PMT * oConversion.Terms), 2) 'oConversion.totalcost
        txtLoanPayment.Text = FormatCurrency(oConversion.PMT, 2) 'oConversion.loanpayment

        Try
            ddlYear.SelectedValue = oConversion.OccupancyYear.ToString()
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try


        oConversion = Nothing
    End Sub

    Private Sub Load_Lookups()
        Dim sErr As String = ""
        siStatus.Connection_String = Resources.Resource.cns
        siStatus.Label_Caption = ""
        siStatus.ComboItem = "ConversionStatus"
        siStatus.Load_Items()
        siTitleType.Connection_String = Resources.Resource.cns
        siTitleType.Label_Caption = ""
        siTitleType.ComboItem = "ConversionTitleType"
        siTitleType.Load_Items()
        siFrequency.Connection_String = Resources.Resource.cns
        siFrequency.Label_Caption = ""
        siFrequency.ComboItem = "ConversionPaymentFrequency"
        siFrequency.Load_Items()
        siPaymentType.Connection_String = Resources.Resource.cns
        siPaymentType.Label_Caption = ""
        siPaymentType.ComboItem = "MortPmtType"
        siPaymentType.Load_Items()
        siInterestType.Connection_String = Resources.Resource.cns
        siInterestType.Label_Caption = ""
        siInterestType.ComboItem = "ConversionIntrestType"
        siInterestType.Load_Items()
        siGrace.Connection_String = Resources.Resource.cns
        siGrace.Label_Caption = ""
        siGrace.ComboItem = "Grace"
        siGrace.Load_Items()
    End Sub
    Protected Sub Events_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Events_Link.Click
        If txtConversionID.Text > 0 Then
            MultiView1.ActiveViewIndex = 4
            Dim oEvents As New clsEvents
            With oEvents
                .KeyField = "ConversionID"
                .KeyValue = txtConversionID.Text
                gvEvents.DataSource = oEvents.List
                gvEvents.DataBind()
            End With
            oEvents = Nothing
        End If
    End Sub

    Protected Sub Notes_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Notes_Link.Click
        If txtConversionID.Text > 0 Then
            MultiView1.ActiveViewIndex = 5
            Notes1.KeyValue = txtConversionID.Text
            Notes1.Display()
        End If
    End Sub

    Protected Sub UserFields_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UserFields_Link.Click
        If txtConversionID.Text > 0 Then
            MultiView1.ActiveViewIndex = 7
            UF.KeyField = "Conversion"
            UF.KeyValue = CInt(txtConversionID.Text)
            UF.Load_List()
        End If
    End Sub

    Protected Sub Financing_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Financing_Link.Click
        If txtConversionID.Text > 0 Then
            MultiView1.ActiveViewIndex = 2
        End If
    End Sub

    Protected Sub Purchase_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Purchase_Link.Click
        If txtConversionID.Text > 0 Then
            MultiView1.ActiveViewIndex = 1
        End If
    End Sub

    Protected Sub Payments_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Payments_Link.Click
        If txtConversionID.Text > 0 Then
            MultiView1.ActiveViewIndex = 6
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim oConversion As New clsConversion
        If Request("ConversionID") = 0 Then
            oConversion.ConversionID = 0
            oConversion.ContractID = Request("ContractID")
            Dim oConTract As New clsContract
            oConTract.ContractID = Request("ContractID")
            oConTract.Load()
            oConversion.ProspectID = oConTract.ProspectID
            oConTract = Nothing
        Else
            oConversion.ConversionID = txtConversionID.Text
        End If
        oConversion.Load()
        oConversion.StatusID = siStatus.Selected_ID
        oConversion.TitleTypeID = siTitleType.Selected_ID
        oConversion.ConversionNumber = txtNumber.Text
        oConversion.StatusDate = dfStatus.Selected_Date
        oConversion.SalesVolume = txtSalesVolume.Text
        oConversion.SalesPrice = txtSalesPrice.Text
        oConversion.CommissionVolume = txtCommissionVolume.Text
        oConversion.OrigSellingPrice = txtOrigPurchasePrice.Text
        oConversion.OriginationDate = dfOrigDate.Selected_Date
        oConversion.FirstPaymentDate = dfFirstPaymentDate.Selected_Date
        oConversion.FrequencyID = siFrequency.Selected_ID
        oConversion.InterestTypeID = siInterestType.Selected_ID
        oConversion.PaymentTypeID = siPaymentType.Selected_ID
        oConversion.APR = txtAPR.Text
        oConversion.Terms = txtTerms.Text
        oConversion.PaymentFee = txtPayProcFee.Text
        oConversion.PaymentFee = txtPaymentProcessingFee.Text
        oConversion.OneTimeFee = txtOneTimeFee.Text
        oConversion.TotalFinanced = txtTotalFin.Text
        oConversion.TotalFinanced = txtTotalFinanced.Text

        If ddlYear.SelectedValue.Equals("(empty)") = False Then
            oConversion.OccupancyYear = Int16.Parse(ddlYear.SelectedValue)
        Else
            oConversion.OccupancyYear = -1
        End If


        oConversion.Save()
        Dim id As Integer = oConversion.ConversionID
        Response.Redirect("editConversion.aspx?ConversionID=" & id)
    End Sub

    Protected Sub Conversion_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Conversion_Link.Click
        MultiView1.ActiveViewIndex = 0
    End Sub

    Protected Sub Amortization_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Amortization_Link.Click
        If txtConversionID.Text > 0 Then
            MultiView1.ActiveViewIndex = 3
        End If
    End Sub

    Protected Sub txtSalesPrice_TextChanged(sender As Object, e As System.EventArgs) Handles txtSalesPrice.TextChanged
        If IsNumeric(txtSalesPrice.Text) Then
            txtSalesVolume.Text = FormatCurrency(CDbl(txtSalesPrice.Text), 2)
        Else
            txtSalesVolume.Text = FormatCurrency(0, 2)
        End If
    End Sub

    Protected Sub lbProspect_Click(sender As Object, e As System.EventArgs) Handles lbProspect.Click
        Response.Redirect(Request.ApplicationPath & "/marketing/editprospect.aspx?prospectid=" & txtProspectID.Text)
    End Sub
End Class
