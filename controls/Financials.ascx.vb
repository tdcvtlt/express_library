Imports Microsoft.VisualBasic
Imports System
Partial Class controls_Financials
    Inherits System.Web.UI.UserControl

    Dim _KeyField As String = ""
    Dim _KeyValue As Integer = 0
    Dim _ProspectID As Integer = 0
    Dim _View As String = ""
    Dim oFinancials As clsFinancials

    Public Sub Display()
        Dim balance As Double
        Select Case LCase(_View)
            Case "mortgage"
                MultiView1.ActiveViewIndex = 0
            Case "mortgagedp"
                MultiView1.ActiveViewIndex = 1
                Load_Mortgage_DP()
            Case "reservation"
                MultiView1.ActiveViewIndex = 2
                balance = oFinancials.Get_Balance(_KeyField, _KeyValue, 0)
                lblResBalance.Text = FormatCurrency(balance, 2)
                If balance > 0 Then
                    lblResBalance.ForeColor = Drawing.Color.Red
                ElseIf balance < 0 Then
                    lblResBalance.ForeColor = Drawing.Color.Green
                End If
                Load_Reservation()
            Case "tour"
                MultiView1.ActiveViewIndex = 3
                balance = oFinancials.Get_Balance(_KeyField, _KeyValue, 0)
                lblTourBalance.Text = FormatCurrency(balance, 2)
                If balance > 0 Then
                    lblTourBalance.ForeColor = Drawing.Color.Red
                ElseIf balance < 0 Then
                    lblTourBalance.ForeColor = Drawing.Color.Green
                End If
                Load_Tour()
            Case "contract"
                MultiView1.ActiveViewIndex = 4
            Case "prospect"
                MultiView1.ActiveViewIndex = 5
                balance = oFinancials.Get_Balance(_KeyField, _KeyValue, _ProspectID)
                lblProsBalance.Text = FormatCurrency(balance, 2)
                If balance > 0 Then
                    lblProsBalance.ForeColor = Drawing.Color.Red
                ElseIf balance < 0 Then
                    lblProsBalance.ForeColor = Drawing.Color.Green
                End If
                Load_Prospect()
            Case "packageissued"
                MultiView1.ActiveViewIndex = 6
                balance = oFinancials.Get_Balance(_KeyField, _KeyValue, 0)
                lblPkgBalance.Text = FormatCurrency(balance, 2)
                If balance > 0 Then
                    lblPkgBalance.ForeColor = Drawing.Color.Red
                ElseIf balance < 0 Then
                    lblPkgBalance.ForeColor = Drawing.Color.Green
                End If
                Load_PackageIssued()
            Case Else
                MultiView1.ActiveViewIndex = 0
        End Select
        If Not (IsNumeric(hfProsID.Value)) Or hfProsID.Value = "" Then hfProsID.Value = 0
    End Sub

    Private Sub Load_Prospect()
        'If (_KeyField = "" And _KeyValue = 0) Or _ProspectID = 0 Then Exit Sub
        If ddContracts.SelectedValue = "0" Or ddContracts.SelectedValue = 0 Then
            oFinancials.ProspectID = _ProspectID
            oFinancials.KeyField = _KeyField
            oFinancials.KeyValue = _KeyValue
            gvProspect.DataSource = oFinancials.Prospect_Financials
            gvProspect.DataBind()

            'Response.Write("KeyField=" & oFinancials.KeyField & "<br />")
            'Response.Write("KeyValue=" & oFinancials.KeyValue)
            hfListFilter.Value = "prospecttrans"
            Fill_Contracts()
            Get_PendingPayments()
        Else
            Update_Prospect()
        End If
    End Sub

    Private Sub Load_Mortgage_DP()
        oFinancials.ProspectID = _ProspectID
        oFinancials.KeyField = _KeyField
        oFinancials.KeyValue = _KeyValue
        If _KeyField = "MortgageDP" Then
            hfListFilter.Value = "MortgageDP,MortgageCC,MortgageMem"
        ElseIf _KeyField = "ConversionDP" Then
            hfListFilter.Value = "ConversionDP,ConversionCC"
        Else
            hfListFilter.Value = "MortgageDP,MortgageCC,MortgageMem"
        End If
        gvMortgageDP.DataSource = oFinancials.Reservation_Financials
        gvMortgageDP.DataBind()
    End Sub

    Private Sub Get_PendingPayments()
        gvPendingPayments.DataSource = Nothing
        gvPendingPayments.DataBind()
        Dim dt As New System.Data.DataTable
        dt.Columns.Add("ContractID")
        dt.Columns.Add("PaymentType")
        dt.Columns.Add("Amount")
        dt.Columns.Add("PaymentDate")
        dt.Columns.Add("CreatedDate")
        dt.Columns.Add("Exception")
        Dim oEq As New clsEquiant
        If ddContracts.SelectedItem.Text <> "ALL" Then
            If ddContracts.SelectedItem.Text <> "ALL" Then
                Dim o As New clsEquiant2CRMS
                o.ContractID = ddContracts.SelectedValue
                o.Load()
                If o.EquiantBillingAccount <> "" Then
                    Dim res = oEq.PendingPayments(o.EquiantBillingAccount)
                    If IsNothing(res) Then
                    Else
                        For Each obj In res
                            If obj.accounttype.ToString <> "LOAN" Then
                                Dim row As Data.DataRow = dt.NewRow
                                row("ContractID") = o.ContractID
                                row("PaymentType") = obj.PaymentType
                                row("Amount") = FormatCurrency(obj.amount)
                                row("PaymentDate") = FormatDateTime(obj.paymentdate, DateFormat.ShortDate)
                                row("CreatedDate") = FormatDateTime(obj.entryDate, DateFormat.ShortDate)
                                row("Exception") = ""
                                dt.Rows.Add(row)
                                row = Nothing
                            End If
                        Next
                    End If
                    res = Nothing
                End If
                o = Nothing
            End If
        Else
            For i = 0 To ddContracts.Items.Count - 1
                If ddContracts.Items(i).Text <> "ALL" Then
                    Dim o As New clsEquiant2CRMS
                    o.ContractID = ddContracts.Items(i).Value
                    o.Load()
                    If o.EquiantBillingAccount <> "" Then
                        Dim res = oEq.PendingPayments(o.EquiantBillingAccount)
                        If Not IsNothing(res) Then
                            For Each obj In res
                                If obj.accounttype.ToString <> "LOAN" Then
                                    Dim row As Data.DataRow = dt.NewRow
                                    row("ContractID") = o.ContractID
                                    row("PaymentType") = obj.PaymentType
                                    row("Amount") = FormatCurrency(obj.amount)
                                    row("PaymentDate") = FormatDateTime(obj.paymentdate, DateFormat.ShortDate)
                                    row("CreatedDate") = FormatDateTime(obj.entryDate, DateFormat.ShortDate)
                                    row("Exception") = ""
                                    dt.Rows.Add(row)
                                    row = Nothing
                                End If
                            Next
                        End If
                        res = Nothing
                    End If
                    o = Nothing
                End If
            Next
        End If




        gvPendingPayments.DataSource = dt ' oFinancials.Get_Pending_Payments
        gvPendingPayments.DataBind()
    End Sub

    Private Sub Load_Tour()
        oFinancials.ProspectID = _ProspectID
        oFinancials.KeyField = _KeyField
        oFinancials.KeyValue = _KeyValue
        hfListFilter.Value = "TourTrans"
        hfProsID.value = _ProspectID
        gvTour.DataSource = oFinancials.Reservation_Financials
        gvTour.DataBind()
    End Sub

    Private Sub Load_Reservation()
        oFinancials.ProspectID = _ProspectID
        oFinancials.KeyField = _KeyField
        oFinancials.KeyValue = _KeyValue
        hfListFilter.Value = "ReservationTrans"
        gvReservation.DataSource = oFinancials.Reservation_Financials
        gvReservation.DataBind()
    End Sub

    Private Sub Load_PackageIssued()
        oFinancials.ProspectID = _ProspectID
        oFinancials.KeyField = _KeyField
        oFinancials.KeyValue = _KeyValue
        hfListFilter.Value = "PackageTrans"
        gvPackageIssued.DataSource = oFinancials.Reservation_Financials
        gvPackageIssued.DataBind()
    End Sub

    Protected Sub gv_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim gv As GridView = e.Row.FindControl("GridView2")
            Dim dbSrc As New SqlDataSource
            dbSrc.ConnectionString = Resources.Resource.cns ' ConfigurationManager.ConnectionStrings("NorthwindConnectionString2").ConnectionString
            dbSrc.SelectCommand = "SELECT * FROM  ufn_payments(" & e.Row.DataItem("ID").ToString & ")"
            Dim sKey(0) As String
            sKey(0) = "PaymentID"
            gv.DataKeyNames = sKey
            gv.DataSource = dbSrc
            gv.DataBind()

            Dim gv2 As GridView = e.Row.FindControl("GridView8")

            Dim dbSrc2 As New sqldatasource
            dbSrc2.connectionstring = resources.resource.cns
            dbSrc2.selectcommand = "Select * from ufn_InvoiceAdjustments(" & e.row.dataitem("ID").tostring & ")"
            gv2.DataSource = dbSrc2 'oFinancials.Prospect_Sub_Financials(e.Row.DataItem("ID").ToString)
            gv2.DataBind()


            Dim gv3 As GridView = e.Row.FindControl("GridView1")

            Dim dbSrc3 As New SqlDataSource
            dbSrc3.ConnectionString = Resources.Resource.cns
            dbSrc3.SelectCommand = "Select * from ufn_ScheduledPayments(" & e.Row.DataItem("ID").ToString & ")"
            gv3.DataSource = dbSrc3
            Dim sKeys(0) As String
            sKeys(0) = "SchedPayID"
            gv3.DataKeyNames = sKeys
            gv3.DataBind()


            dbSrc = Nothing
            dbSrc2 = Nothing
            dbSrc3 = Nothing

        End If
    End Sub

    Protected Sub GridView1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No History." Then
            e.Row.Cells(5).Visible = False
        End If
    End Sub
    Protected WithEvents GridView1 As GridView
    Protected Sub GridView1_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles GridView1.RowCommand
        Dim ID As Integer = 0
        If IsNumeric(e.CommandArgument) Then
            ID = Convert.ToInt32(e.CommandArgument)
        Else
            ID = -1
        End If
        If e.CommandName.CompareTo("Receipt") = 0 Then
            Page.ClientScript.RegisterClientScriptBlock(Me.Page.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/general/Receipt.aspx?ID=" & ID & "&Scheduled=True','win01',650,350);", True)
        ElseIf e.CommandName.CompareTo("Receive") = 0 Then
            Dim gvRow As GridViewRow = sender.Rows(ID)
            ID = gvRow.Cells(0).Text
            If CheckSecurity("Payments", "ReceiveScheduledPayments", , , Session("UserDBID")) Then
                If CDbl(gvRow.Cells(4).Text) > CDbl(CType(sender.Parent.parent, GridViewRow).Cells(6).Text) Then
                    Page.ClientScript.RegisterClientScriptBlock(Me.Page.GetType, "AjaxCall", "alert('Payment Being Received Exceeds Balance Due. Please Adjust Payment Amount. " & CType(sender.Parent.parent, GridViewRow).Cells(6).Text & "');", True)
                Else
                    Dim oSchedPay As New clsPaymentsScheduled
                    oSchedPay.SchedPayID = ID
                    oSchedPay.Load()
                    If oSchedPay.Cancelled = True Then
                        Page.ClientScript.RegisterClientScriptBlock(Me.Page.GetType, "AjaxCall", "alert('This Payment Has Already Been Cancelled.');", True)
                    ElseIf oSchedPay.Processed = True Then
                        Page.ClientScript.RegisterClientScriptBlock(Me.Page.GetType, "AjaxCall", "alert('Payment Is Already Received.');", True)
                    Else
                        oSchedPay.Processed = True
                        oSchedPay.ProcessedDate = System.DateTime.Now
                        oSchedPay.Save()
                        Dim paymentID As Integer = 0
                        Dim invID As Integer = 0
                        Dim oPayment As New clsPayments
                        Dim oPayment2Inv As New clsPayment2Invoice
                        Dim oSchedPay2Invoice As New clsPaymentSched2Invoice
                        Dim oFinTransCode As New clsFinancialTransactionCodes
                        Dim oCOmbo As New clsComboItems
                        invID = oSchedPay2Invoice.Get_Invoice_ID(oSchedPay.SchedPayID)
                        If oSchedPay.CreditCardID > 0 Then
                            Dim oInvoice As New clsInvoices
                            Dim oCCAcct As New clsCCMerchantAccount
                            Dim oCCTrans As New clsCCTrans
                            Dim oCCTransAPply As New clsCCTransApplyTo
                            Dim oC As New clsCreditCard
                            oC.CreditCardID = oSchedPay.CreditCardID
                            oC.Load()
                            Dim ccID As Integer = 0
                            oInvoice.InvoiceID = invID
                            oInvoice.Load()
                            oFinTransCode.FinTransID = oInvoice.FinTransID
                            oFinTransCode.Load()
                            oCCAcct.AccountID = oFinTransCode.MerchantAccountID
                            oCCAcct.Load()
                            If oCCAcct.CCApproval = False Then
                                oCCTrans.CCTransID = 0
                                oCCTrans.Load()
                                oCCTrans.AccountID = oCCAcct.AccountID
                                oCCTrans.Amount = oSchedPay.Amount
                                oCCTrans.TransTypeID = oCOmbo.Lookup_ID("CCTransType", "Charge")
                                oCCTrans.ClientIP = Request.ServerVariables("REMOTE_ADDR")
                                oCCTrans.CreditCardID = oSchedPay.CreditCardID
                                oCCTrans.CreatedByID = Session("UserDBID")
                                oCCTrans.DateCreated = System.DateTime.Now
                                oCCTrans.Token = oC.Token
                                oCCTrans.Save()
                                ccID = oCCTrans.CCTransID
                                oCCTransAPply.CCTransApplyToID = 0
                                oCCTransAPply.Load()
                                oCCTransAPply.CCTransID = ccID
                                oCCTransAPply.PaymentID = invID
                                oCCTransAPply.Amount = oSchedPay.Amount
                                oCCTransAPply.Save()
                                Page.ClientScript.RegisterClientScriptBlock(Me.Page.GetType, "AjaxCall", "alert('Payment Has Been Submitted For Final Approval.');", True)
                            Else
                                oPayment.PaymentID = 0
                                oPayment.Load()
                                oPayment.MethodID = oSchedPay.MethodID
                                oPayment.UserID = Session("UserDBID")
                                oPayment.Amount = oSchedPay.Amount
                                oPayment.PosNeg = True
                                oPayment.Adjustment = False
                                oPayment.TransDate = System.DateTime.Now.ToShortDateString
                                oPayment.Description = oSchedPay.Description
                                oPayment.Reference = oSchedPay.Reference
                                oPayment.Save()
                                paymentID = oPayment.PaymentID

                                oPayment2Inv.Inv2PayID = 0
                                oPayment2Inv.Load()
                                oPayment2Inv.PaymentID = paymentID
                                oPayment2Inv.InvoiceID = invID
                                oPayment2Inv.Amount = oSchedPay.Amount
                                oPayment2Inv.PosNeg = True
                                oPayment2Inv.Save()

                                oCCTrans.CCTransID = 0
                                oCCTrans.Load()
                                oCCTrans.AccountID = oCCAcct.AccountID
                                oCCTrans.Amount = oSchedPay.Amount
                                oCCTrans.TransTypeID = oCOmbo.Lookup_ID("CCTransType", "Charge")
                                oCCTrans.ClientIP = Request.ServerVariables("REMOTE_ADDR")
                                oCCTrans.CreditCardID = oSchedPay.CreditCardID
                                oCCTrans.DateCreated = System.DateTime.Now
                                oCCTrans.CreatedByID = Session("UserDBID")
                                oCCTrans.Token = oC.Token
                                oCCTrans.Save()
                                ccID = oCCTrans.CCTransID
                                oCCTransAPply.CCTransApplyToID = 0
                                oCCTransAPply.Load()
                                oCCTransAPply.CCTransID = ccID
                                oCCTransAPply.PaymentID = paymentID
                                oCCTransAPply.Amount = oSchedPay.Amount
                                oCCTransAPply.Save()
                                oCCTrans.Approved = 1
                                oCCTrans.ApprovedBy = Session("UserDBID")
                                oCCTrans.DateApproved = System.DateTime.Now
                                oCCTrans.Save()
                                Page.ClientScript.RegisterClientScriptBlock(Me.Page.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/general/ReceivePayment.aspx?ID=" & ccID & "','win01',650,350);", True)
                            End If
                            oInvoice = Nothing
                            oCCAcct = Nothing
                            oCCTrans = Nothing
                            oCCTransAPply = Nothing
                            oC = Nothing
                        Else
                            oPayment.PaymentID = 0
                            oPayment.Load()
                            oPayment.MethodID = oSchedPay.MethodID
                            oPayment.UserID = Session("UserDBID")
                            oPayment.Amount = oSchedPay.Amount
                            oPayment.PosNeg = True
                            oPayment.Adjustment = False
                            oPayment.TransDate = System.DateTime.Now.ToShortDateString
                            oPayment.Description = oSchedPay.Description
                            oPayment.Reference = oSchedPay.Reference
                            oPayment.Save()
                            paymentID = oPayment.PaymentID

                            oPayment2Inv.Inv2PayID = 0
                            oPayment2Inv.Load()
                            oPayment2Inv.PaymentID = paymentID
                            oPayment2Inv.InvoiceID = invID
                            oPayment2Inv.Amount = oSchedPay.Amount
                            oPayment2Inv.PosNeg = True
                            oPayment2Inv.Save()
                        End If

                        If _View = "" Then
                            _View = hfView.Value
                            _KeyField = hfKeyField.Value
                            _KeyValue = hfKeyValue.Value
                            _ProspectID = hfProsID.Value
                        End If
                        Display()

                        oPayment = Nothing
                        oPayment2Inv = Nothing
                        oSchedPay2Invoice = Nothing
                    End If
                    oSchedPay = Nothing
                End If
            Else
                Page.ClientScript.RegisterClientScriptBlock(Me.Page.GetType, "AjaxCall", "alert('You Do Not Have Permission To Receive Scheduled Payments.');", True)
            End If
        Else
            Page.ClientScript.RegisterClientScriptBlock(Me.Page.GetType, "AjaxCall", "alert('" & e.CommandArgument & " " & ID & "');", True)
        End If
    End Sub

    Protected Sub gv_PaymentAdjustments(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim gv As GridView = e.Row.FindControl("gvPaymentAdjustments")
            Dim dbSrc As New SqlDataSource
            dbSrc.ConnectionString = Resources.Resource.cns ' ConfigurationManager.ConnectionStrings("NorthwindConnectionString2").ConnectionString
            dbSrc.SelectCommand = "SELECT * FROM  ufn_paymentAdjustments(" & e.Row.DataItem("PaymentID").ToString & ")"

            gv.DataSource = dbSrc
            gv.DataBind()

            dbSrc = Nothing
        End If
    End Sub

#Region "Properties"
    Public Property KeyField As String
        Get
            Return _KeyField
        End Get
        Set(ByVal value As String)
            _KeyField = value
            hfKeyField.Value = _KeyField
        End Set
    End Property

    Public Property KeyValue As Integer
        Get
            Return _KeyValue
        End Get
        Set(ByVal value As Integer)
            _KeyValue = value
            hfKeyValue.Value = _KeyValue
        End Set
    End Property

    Public ReadOnly Property Selected_ContractID As Integer
        Get
            Return ddContracts.SelectedValue
        End Get
    End Property

    Public Property ProspectID As Integer
        Get
            Return _ProspectID
        End Get
        Set(ByVal value As Integer)
            _ProspectID = value
            hfProsID.Value = _ProspectID
        End Set
    End Property

    Public Property View As String
        Get
            Return _View
        End Get
        Set(ByVal value As String)
            _View = value
            hfView.Value = _View
        End Set
    End Property

#End Region

    Public Sub New()
        oFinancials = New clsFinancials
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If _ProspectID > 0 And Not (IsPostBack) Then Fill_Contracts()

    End Sub

    Private Sub Fill_Contracts()
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        ds.SelectCommand = "Select 0 as ID, 'ALL' as Contractnumber union Select ContractID as ID, ContractNumber from t_Contract where prospectid = " & _ProspectID & " order by Contractnumber"
        ddContracts.DataSource = ds
        ddContracts.DataTextField = "ContractNumber"
        ddContracts.DataValueField = "ID"
        'ddContracts.Items.Add(
        ddContracts.DataBind()
        For i = 0 To ddContracts.Items.Count - 1
            If ddContracts.Items(i).Value = 0 Then
                ddContracts.SelectedIndex = i
                Exit For
            End If
        Next
    End Sub

    Protected Sub Update_Prospect()
        hfKeyField.Value = IIf(ddContracts.SelectedValue = "0" Or ddContracts.SelectedValue = 0, "ProspectID", "ContractID")
        hfListFilter.Value = IIf(ddContracts.SelectedValue = "0" Or ddContracts.SelectedValue = 0, "ProspectTrans", "ContractTrans,mftrans")
        hfKeyValue.Value = ddContracts.SelectedValue
        'hfView.Value = IIf(ddContracts.SelectedValue = "0" Or ddContracts.SelectedValue = 0, "prospect", "contract")
        '_View = hfView.Value
        _View = IIf(ddContracts.SelectedValue = "0" Or ddContracts.SelectedValue = 0, "prospect", "contract")
        oFinancials.ProspectID = hfProsID.Value '_ProspectID
        oFinancials.KeyField = hfKeyField.Value
        oFinancials.KeyValue = hfKeyValue.Value
        gvProspect.DataSource = oFinancials.Prospect_Financials
        gvProspect.DataBind()
        'gvPendingPayments.DataSource = oFinancials.Get_Pending_Payments
        'gvPendingPayments.DataBind()
        Get_PendingPayments()
        Dim balance As Double
        balance = oFinancials.Get_Balance(hfKeyField.Value, hfKeyValue.Value, hfProsID.Value)
        lblProsBalance.Text = FormatCurrency(balance, 2)
        If balance > 0 Then
            lblProsBalance.ForeColor = Drawing.Color.Red
        ElseIf balance < 0 Then
            lblProsBalance.ForeColor = Drawing.Color.Green
        End If
    End Sub

    Protected Sub ddContracts_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddContracts.SelectedIndexChanged
        Update_Prospect()
    End Sub

    Protected Sub lbRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbRefresh.Click
        If _View = "" Then
            _View = hfView.Value
            _KeyField = hfKeyField.Value
            _KeyValue = hfKeyValue.Value
            _ProspectID = hfProsID.Value
        End If

        Display()
    End Sub

    Protected Sub btnArchives_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnArchives.Click
        If ddContracts.SelectedValue > 0 Then
            Page.ClientScript.RegisterClientScriptBlock(Me.Page.GetType, "Archives", "javascript:modal.mwindow.open('../mis/nextarchives.aspx?contract=" & ddContracts.SelectedValue & "','Edit',350,350);", True)
        End If
    End Sub
End Class
