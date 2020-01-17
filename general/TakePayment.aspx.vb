Imports Microsoft.VisualBasic
Imports System.Data

Partial Class general_TakePayment
    Inherits System.Web.UI.Page
    Dim oInv As New clsInvoices
    Public payments As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not (IsPostBack) Then
            If Not (CheckSecurity("Payments", "ReceiveAFCPayments", , , Session("UserDBID"))) Then
                ddPayMethod.Items.RemoveAt(15)
                ddPayMethod.Items.RemoveAt(14)
            End If
            If Request("Schedule") = "True" Then
                btnProcess_Check_ACH_MO.Visible = False
                btnProcess_Cash_Equity_ExitEquity.Visible = False
                btnProcess_Concord.Visible = False
                btnScheduleConcord.Visible = True
                btnScheduleCash.Visible = True
                btnScheduleCheck.Visible = True
            End If

            Select Case Request("KeyField")
                Case "ReservationID"
                    If Not (CheckSecurity("Payments", "PositiveReservationAdjustments", , , Session("UserDBID"))) Then
                        rbPos.Enabled = False
                    End If
                Case "ContractID"
                    If Not (CheckSecurity("Payments", "PositiveContractAdjustments", , , Session("UserDBID"))) Then
                        rbPos.Enabled = False
                    End If
                Case "ConversionDP", "ConversionCC"
                    If Not (CheckSecurity("Payments", "PositiveConversionAdjustments", , , Session("UserDBID"))) Then
                        rbPos.Enabled = False
                    End If
                Case "MortgageDP", "MortgageCC"
                    If Not (CheckSecurity("Payments", "PositiveMortgageAdjustments", , , Session("UserDBID"))) Then
                        rbPos.Enabled = False
                    End If
                Case "PackageIssuedID"
                    If Not (CheckSecurity("Payments", "PositivePackageAdjustments", , , Session("UserDBID"))) Then
                        rbPos.Enabled = False
                    End If
                Case "ProspectID"
                    If Not (CheckSecurity("Payments", "PositiveProspectAdjustments", , , Session("UserDBID"))) Then
                        rbPos.Enabled = False
                    End If
                Case "TourID"
                    If Not (CheckSecurity("Payments", "PositiveTourAdjustments", , , Session("UserDBID"))) Then
                        rbPos.Enabled = False
                    End If

            End Select
            MultiView2.ActiveViewIndex = 0
            MultiView1.ActiveViewIndex = 0
            Load_Invoices(True)
            siState_CC.Connection_String = Resources.Resource.cns
            siState_CC.Load_Items()
            
            'ScriptManager1.RegisterDispose(lblWaiting, "if(document.getElementById('lblWaiting').text == 'Approved'){alert('Approved');}else{alert('Denied');}")
        End If
        Dim sds As SqlDataSource = (New clsCCMerchantAccount).List_Accounts()
        Dim dv As DataView = CType(sds.Select(DataSourceSelectArguments.Empty), DataView)
        Dim code As String = "function Get_Key(index){switch (index){ "
        For Each dvr As DataRowView In dv
            code &= "case """ & dvr("AccountID") & """: pkey='" & dvr("publictoken") & "'; break; "
        Next
        code &= "default: pkey=''; break; };};"
        sds = Nothing
        dv = Nothing
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "pk", code, True)
        lblErr.Text = ""
    End Sub


    Protected Sub ddPayMethod_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddPayMethod.SelectedIndexChanged
        Select Case ddPayMethod.SelectedValue
            Case "Check", "ACH Payment", "MoneyOrder"
                MultiView1.ActiveViewIndex = 1
                Load_Invoices(True)
            Case "Credit Card"
                MultiView1.ActiveViewIndex = 0
                Load_Invoices(True)
            Case "Cash", "Equity", "Exit Equity", "Leisure Link"
                MultiView1.ActiveViewIndex = 2
                Load_Invoices(True)
            Case "Concord Payment", "Meridian", "Aspen", "AFC Payment", "Meridian - AFC", "Equiant Payment"
                MultiView1.ActiveViewIndex = 3
                Load_Invoices(True)
            Case "Chargeback Employee"
                MultiView1.ActiveViewIndex = 4
                Load_Invoices(True)
            Case "Adjustment"
                MultiView1.ActiveViewIndex = 5
                Load_Adjustments()
            Case "Online"
                MultiView1.ActiveViewIndex = 6
                Load_Invoices(True)
            Case Else

        End Select
    End Sub

    Protected Sub Load_Payments()
        gvpayments.visible = True
        gvInvoices.visible = False
        Dim oPay As clsPayments = New clsPayments
        oPay.KeyField = Request("KeyField")
        oPay.KeyValue = Request("KeyValue")
        oPay.ProspectID = Request("ProspectID")
        gvPayments.DataSource = oPay.List_Payments
        gvPayments.DataBind()
        oPay = Nothing
        Dim oAdjustments As New clsPaymentMethod2Adjustment
        ddAdjustments.DataSource = oAdjustments.Sort_Adjustments(0)
        ddAdjustments.DataTextField = "Adjustment"
        ddAdjustments.DataValueField = "ID"
        ddAdjustments.DataBind()
        oAdjustments = Nothing
	rbPos.enabled = true
    End Sub

    Protected Sub Load_Invoices(ByVal balance As Boolean)
        gvpayments.visible = False
        gvInvoices.visible = True
        oInv = New clsInvoices
        oInv.KeyField = Request("KeyField")
        oInv.KeyValue = Request("KeyValue")
        oInv.ProspectID = Request("ProspectID")

        If balance = True Then

            If CheckSecurity("Payments", "AFC", , , Session("UserDBID"), "") = False And CheckSecurity("Payments", "AFC - In Collection", , , Session("UserDBID"), "") = False Then

                'Dim args As DataSourceSelectArguments = New DataSourceSelectArguments()
                'Dim view As DataView = DirectCast(oInv.List_Invoices_With_Balance.Select(args), DataView)
                'Dim dt As New DataTable
                'dt = view.ToTable()

                'Dim rows = dt.AsEnumerable().Where(Function(x) x("AFC").ToString().CompareTo("AFC") = 0).ToArray()

                'For Each row As DataRow In rows
                '    dt.Rows.Remove(row)
                'Next

                'rows = dt.AsEnumerable().Where(Function(x) x("AFC").ToString().CompareTo("AFC - In Collections") = 0).ToArray()
                'For Each row As DataRow In rows
                '    dt.Rows.Remove(row)
                'Next
                Dim fields() As String = {"AFC", "AFC"}
                Dim vals() As String = {"AFC", "AFC - In Collections"}
                gvInvoices.DataSource = Get_Invoices(fields, vals, False)
            ElseIf CheckSecurity("Payments", "AFC", , , Session("UserDBID"), "") = False And CheckSecurity("Payments", "AFC - In Collection", , , Session("UserDBID"), "") Then
                'Dim args As DataSourceSelectArguments = New DataSourceSelectArguments()
                'Dim view As DataView = DirectCast(oInv.List_Invoices_With_Balance.Select(args), DataView)
                'Dim dt As New DataTable
                'dt = view.ToTable()

                'Dim rows = dt.AsEnumerable().Where(Function(x) x("AFC").ToString().CompareTo("AFC") = 0).ToArray()
                'For Each row As DataRow In rows
                '    dt.Rows.Remove(row)
                'Next
                Dim fields() As String = {"AFC"}
                Dim vals() As String = {"AFC"}
                gvInvoices.DataSource = Get_Invoices(fields, vals, False)
            ElseIf CheckSecurity("Payments", "AFC", , , Session("UserDBID"), "") And CheckSecurity("Payments", "AFC - In Collection", , , Session("UserDBID"), "") = False Then
                'Dim args As DataSourceSelectArguments = New DataSourceSelectArguments()
                'Dim view As DataView = DirectCast(oInv.List_Invoices_With_Balance.Select(args), DataView)
                'Dim dt As New DataTable
                'dt = view.ToTable()

                'Dim rows = dt.AsEnumerable().Where(Function(x) x("AFC").ToString().CompareTo("AFC - In Collections") = 0).ToArray()
                'For Each row As DataRow In rows
                '    dt.Rows.Remove(row)
                'Next
                Dim fields() As String = {"AFC"}
                Dim vals() As String = {"AFC - In Collections"}
                gvInvoices.DataSource = Get_Invoices(fields, vals, False) 'dt
            Else
                Dim fields() As String = {""}
                Dim vals() As String = {""}
                gvInvoices.DataSource = Get_Invoices(fields, vals, False) ' oInv.List_Invoices_With_Balance

            End If
        Else

            If CheckSecurity("Payments", "AFC", , , Session("UserDBID"), "") = False And CheckSecurity("Payments", "AFC - In Collection", , , Session("UserDBID"), "") = False Then

                'Dim args As DataSourceSelectArguments = New DataSourceSelectArguments()
                'Dim view As DataView = DirectCast(oInv.List_Invoices_Oldest_First.Select(args), DataView)
                'Dim dt As New DataTable
                'dt = view.ToTable()

                'Dim rows = dt.AsEnumerable().Where(Function(x) x("AFC").ToString().CompareTo("AFC") = 0).ToArray()

                'For Each row As DataRow In rows
                '    dt.Rows.Remove(row)
                'Next

                'rows = dt.AsEnumerable().Where(Function(x) x("AFC").ToString().CompareTo("AFC - In Collections") = 0).ToArray()
                'For Each row As DataRow In rows
                '    dt.Rows.Remove(row)
                'Next
                Dim fields() As String = {"AFC", "AFC"}
                Dim vals() As String = {"AFC", "AFC - In Collections"}
                gvInvoices.DataSource = Get_Invoices(fields, vals, True) 'dt
            ElseIf CheckSecurity("Payments", "AFC", , , Session("UserDBID"), "") = False And CheckSecurity("Payments", "AFC - In Collection", , , Session("UserDBID"), "") Then
                'Dim args As DataSourceSelectArguments = New DataSourceSelectArguments()
                'Dim view As DataView = DirectCast(oInv.List_Invoices_Oldest_First.Select(args), DataView)
                'Dim dt As New DataTable
                'dt = view.ToTable()

                'Dim rows = dt.AsEnumerable().Where(Function(x) x("AFC").ToString().CompareTo("AFC") = 0).ToArray()
                'For Each row As DataRow In rows
                '    dt.Rows.Remove(row)
                'Next
                Dim fields() As String = {"AFC"}
                Dim vals() As String = {"AFC"}
                gvInvoices.DataSource = Get_Invoices(fields, vals, True)
            ElseIf CheckSecurity("Payments", "AFC", , , Session("UserDBID"), "") And CheckSecurity("Payments", "AFC - In Collection", , , Session("UserDBID"), "") = False Then
                'Dim args As DataSourceSelectArguments = New DataSourceSelectArguments()
                'Dim view As DataView = DirectCast(oInv.List_Invoices_Oldest_First.Select(args), DataView)
                'Dim dt As New DataTable
                'dt = view.ToTable()

                'Dim rows = dt.AsEnumerable().Where(Function(x) x("AFC").ToString().CompareTo("AFC - In Collections") = 0).ToArray()
                'For Each row As DataRow In rows
                '    dt.Rows.Remove(row)
                'Next
                Dim fields() As String = {"AFC"}
                Dim vals() As String = {"AFC - In Collections"}
                gvInvoices.DataSource = Get_Invoices(fields, vals, True)
            Else
                Dim fields() As String = {""}
                Dim vals() As String = {""}
                gvInvoices.DataSource = Get_Invoices(fields, vals, True) 'oInv.List_Invoices_Oldest_First

            End If

        End If


        gvInvoices.DataBind()
        oInv = Nothing
        'gvInvoices.Sort("TransDate", SortDirection.Ascending)
        Dim oAdjustments As New clsPaymentMethod2Adjustment
        ddAdjustments.DataSource = oAdjustments.Sort_Adjustments(1)
        ddAdjustments.DataTextField = "Adjustment"
        ddAdjustments.DataValueField = "ID"
        ddAdjustments.DataBind()
        oAdjustments = Nothing
    End Sub

    Protected Function Get_Invoices(fields() As String, removeValues() As String, oldestFirst As Boolean) As DataTable
        Dim args As DataSourceSelectArguments = New DataSourceSelectArguments()
        Dim view As DataView
        Dim dt As New DataTable
        If oldestFirst Then
            view = DirectCast(oInv.List_Invoices_Oldest_First.Select(args), DataView)
        Else
            view = DirectCast(oInv.List_Invoices_With_Balance.Select(args), DataView)
        End If
        dt = view.ToTable
        If Not (IsNothing(fields)) Then
            If fields(0) <> "" Then
                For i = 0 To fields.Count - 1
                    Dim field As String = fields(i)
                    Dim val As String = removeValues(i)
                    Dim rows = dt.AsEnumerable().Where(Function(x) x(field).ToString().CompareTo(val) = 0).ToArray()
                    For Each row As DataRow In rows
                        dt.Rows.Remove(row)
                    Next
                Next
            End If
        End If
        Dim con As New clsContract
        If oInv.KeyField = "ContractID" Then
            con.ContractID = oInv.KeyValue
        End If
        con.Load()
        If CheckSecurity("Payments", "MF Account", , , Session("UserDBID"), "") = False And (New clsComboItems).Lookup_ComboItem(con.StatusID) <> "Canceled" Then
            Dim MFAccount = dt.AsEnumerable().Where(Function(x) x("Acct").ToString().CompareTo("1") = 0).ToArray()
            For Each row As DataRow In MFAccount
                If ddPayMethod.SelectedValue = "Adjustment" Then
                    If Not (CheckSecurity("Payments", "MF Account Adjust", , , Session("UserDBID"), "")) Then
                        dt.Rows.Remove(row)
                    End If
                Else
                    dt.Rows.Remove(row)
                End If

            Next
        End If
        con = Nothing

        Return dt
    End Function

    Protected Sub gvInvoices_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvInvoices.RowDataBound
        e.Row.Cells(7).Visible = False
        e.Row.Cells(8).Visible = False
        If e.Row.RowIndex > -1 Then
            e.Row.Cells(5).Text = FormatNumber(CDbl(e.Row.Cells(5).Text), 2)
            e.Row.Cells(6).Text = FormatNumber(CDbl(e.Row.Cells(6).Text), 2)
        End If
    End Sub
    Private Function Verify_Credit_Card_Trans() As Boolean
        If txtCVV2_CC.Text = "" Then txtCVV2_CC.Text = "123"
        If Not (IsNumeric(txtAmount_CC.Text)) Or txtAmount_CC.Text = "" Then
            lblErr.Text = "Please enter an Amount"
            Return False
        ElseIf txtCardNumber_CC.Text = "" Then
            lblErr.Text = "Please enter a Credit Card Number"
            Return False
            'ElseIf txtCVV2_CC.Text = "" Then
            'lblErr.Text = "Please enter a CVV code"
            'Return False
        ElseIf txtExpiration_CC.Text = "" Then
            lblErr.Text = "Please enter an Expiration Date"
            Return False
        ElseIf hfTokenValue.Value = "0" Or hfTokenValue.Value = "" Then
            lblErr.Text = "Unable to read Credit Card Token Value."
            Return False
        ElseIf txtName_CC.Text = "" Then
            lblErr.Text = "Please enter the Name on Card"
            Return False
        ElseIf txtBillingAddress_CC.Text = "" Then
            lblErr.Text = "Please enter a Billing Address "
            Return False
        ElseIf txtCity_CC.Text = "" Then
            lblErr.Text = "Please enter a City"
            Return False
        ElseIf siState_CC.SelectedName = "" Then
            lblErr.Text = "Please select a State"
            Return False
        ElseIf txtZip_CC.Text = "" Then
            lblErr.Text = "Please enter a Zip"
            Return False
        ElseIf txtDescription_CC.Text = "" Then
            lblErr.Text = "Please enter a Description"
            Return False
        Else
            Return True
        End If
    End Function
    Private Function Verify_Adjustments(ByRef amt_Selected As Decimal, ByVal amt_Entered As String) As Boolean
        For i = 0 To gvPayments.Rows.Count - 1
            If CType(gvPayments.Rows(i).FindControl("cb"), CheckBox).Checked Then
                amt_Selected += CDec(gvPayments.Rows(i).Cells(4).Text)
            End If
        Next
        If amt_Selected < CDec(amt_Entered) Then
            lblErr.Text = "Please lower the amount to adjust or select more payments."
            Return False
        Else
            Return True
        End If
    End Function
    Private Function Verify_Amounts(ByRef strMA As String, ByRef amt_Selected As Decimal, ByVal amt_Entered As String, ByRef autoApprove As Boolean) As Boolean
        'Get_Amount selected and the ids
        For i = 0 To gvInvoices.Rows.Count - 1
            If CType(gvInvoices.Rows(i).FindControl("cb"), CheckBox).Checked Then
                If strMA = "0" Then
                    strMA = gvInvoices.Rows(i).Cells(2).Text
                    autoApprove = CBool(gvInvoices.Rows(i).Cells(7).Text)
                End If
                If strMA <> gvInvoices.Rows(i).Cells(2).Text Then
                    lblErr.Text = "Please select invoices on the same merchant account."
                    Return False
                    Exit Function
                End If
                amt_Selected += CDec(gvInvoices.Rows(i).Cells(6).Text)
            End If
        Next

        If amt_Selected <= 0 Then
            lblErr.Text = "Please select an Invoice with a balance greater than $0"
            Return False
        ElseIf amt_Selected < CDec(amt_Entered) Then
            lblErr.Text = "Please lower the amount to charge or select more invoices"
            Return False
        ElseIf amt_Selected < (CDec(amt_Entered) * CDec(txtPayments.Text)) And Not (cbAutoSplit.Checked) Then
            lblErr.Text = "Please lower the amount of each payment."
            Return False
        Else
            Return True
        End If
    End Function

    Private Function Get_Credit_Card_Record() As Integer
        'Get Credit Card Record
        Dim oFin As New clsFinancials
        Return oFin.Get_CreditCard(txtCardNumber_CC.Text, txtExpiration_CC.Text, txtCVV2_CC.Text, txtBillingAddress_CC.Text, txtCity_CC.Text, siState_CC.Selected_ID, txtZip_CC.Text, txtName_CC.Text, txtSwipe_CC.Text, Request("ProspectID"), hfTokenValue.Value, hfCardType.Value)
        oFin = Nothing
    End Function

    Public Function Get_LookupID(ByVal Combo As String, ByVal Item As String) As Integer
        Dim oCombo As New clsComboItems
        Return oCombo.Lookup_ID(Combo, Item)
        oCombo = Nothing
    End Function

    Protected Sub btnProcess_CC_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcess_CC.Click
        If Process_Payment() Then Receipt(0, "CC")
    End Sub

    Private Function Verify_PreAuth(ByVal CCID As Integer) As Integer
        Dim iRet As Integer = 0
        Dim cn As New System.Data.SqlClient.SqlConnection(Resources.Resource.cns)
        Dim cm As New System.Data.SqlClient.SqlCommand("", cn)
        Dim dr As System.Data.SqlClient.SqlDataReader
        'System.Threading.Thread.Sleep(10000)
        Try
            cm.CommandText = "select case when sum(ID) is null then 0 else sum(ID) end as ID, case when sum(Auth) is null then 0 else sum(Auth) end as Auth, case when sum(Force) is null then 0 else sum(Force) end as Force, " & _
                                    "case when Sum(Auth) is null then 0 else Sum(Auth) end - case when Sum(Force) is null then 0 else Sum(Force) end as Remaining  " & _
                                    "from ( " & _
                                    "select CCTransID as ID, amount as Auth, 0 as Force " & _
                                    "from t_CCTrans  " & _
                                    "where creditcardid IN (Select CreditCardID from t_CreditCard where Right(Number, 4) = '" & Right(txtCardNumber_CC.Text, 4) & "' and Expiration = '" & txtExpiration_CC.Text & "') and amount >= " & CDbl(txtAmount_CC.Text) & " and transtypeid = " & Get_LookupID("CCTransType", "PreAuth") & " and ICVResponse like '%" & txtAuthorization_CC.Text & "%' " & _
                                    "union  " & _
                                    "select 0 as ID,0 as Auth, Amount as Force " & _
                                    "from t_CCTrans  " & _
                                    "where creditcardid IN (Select CreditCardID from t_CreditCard where Right(Number, 4) = '" & Right(txtCardNumber_CC.Text, 4) & "' and Expiration = '" & txtExpiration_CC.Text & "') and Preauthid in ( " & _
                                    "	select CCTransID  " & _
                                    "	from t_CCTrans  " & _
                                    "	where creditcardid IN (Select CreditCardID from t_CreditCard where Right(Number, 4) = '" & Right(txtCardNumber_CC.Text, 4) & "' and Expiration = '" & txtExpiration_CC.Text & "') and amount >= " & CDbl(txtAmount_CC.Text) & " and transtypeid = " & Get_LookupID("CCTransType", "PreAuth") & " and ICVResponse like '%" & txtAuthorization_CC.Text & "%' " & _
                                    ") and ICVResponse like 'Y" & txtAuthorization_CC.Text & "%' " & _
                                    ") a"
            If cn.State <> Data.ConnectionState.Open Then cn.Open()
            dr = cm.ExecuteReader
            If dr.HasRows Then
                dr.Read()
                If dr("Remaining") >= CDbl(txtAmount_CC.Text) Then
                    iRet = dr("ID")
                Else
                    lblErr.Text = FormatCurrency(txtAmount_CC.Text) & " " & CCID & "is not available on that PreAuthorization. X"
                End If
            Else
                lblErr.Text = FormatCurrency(txtAmount_CC.Text) & " " & CCID & "is not available on that PreAuthorization."
            End If
            dr.Close()
            If cn.State <> Data.ConnectionState.Closed Then cn.Close()
        Catch ex As Exception
            lblErr.Text = ex.ToString
        End Try

        dr = Nothing
        cm = Nothing
        cn = Nothing

        Return iRet
    End Function

    Private Function Create_CCTrans_Record(ByVal TransTypeID As Integer, ByVal AcctID As Integer, ByVal ApplyTo() As String, ByVal Amount() As String, ByVal CreditCardID As Integer, ByVal PreAuthID As Integer, ByVal ICVResponse As String, ByVal Imported As Boolean, ByVal autoApprove As Boolean) As Integer
        Dim iRet As Integer = 0
        Dim oCombo As New clsComboItems
        Dim oCCTrans As New clsCCTrans
        Dim oCC As New clsCreditCard
        Try
            oCCTrans.Load()
            oCCTrans.TransTypeID = TransTypeID
            oCCTrans.Amount = txtAmount_CC.Text
            oCCTrans.AccountID = AcctID
            oCCTrans.Imported = 0
            oCCTrans.Token = hfTokenValue.Value
            If autoApprove Then
                oCCTrans.Approved = 1
                oCCTrans.ApprovedBy = "Auto"
                oCCTrans.DateApproved = Date.Now
            End If
            If oCombo.Lookup_ComboItem(TransTypeID) = "Force" Then
                Dim occPA As New clsCCTrans
                occPA.CCTransID = PreAuthID
                occPA.Load()
                oCC.CreditCardID = occPA.CreditCardID
                oCC.Load()
                oCC.ProspectID = Request("ProspectID")
                oCC.Save()
                oCCTrans.CreditCardID = occPA.CreditCardID
                occPA = Nothing
            Else
                oCCTrans.CreditCardID = CreditCardID
            End If
            oCCTrans.ClientIP = Request.ServerVariables("REMOTE_HOST")
            oCCTrans.DateCreated = Date.Now
            oCCTrans.CreatedByID = Session("UserDBID")
            oCCTrans.PreAuthID = PreAuthID
            oCCTrans.ICVResponse = ICVResponse
            oCCTrans.UserID = Session("UserDBID")
            oCCTrans.Imported = Imported
            If Imported Then
                oCCTrans.DateImported = System.DateTime.Now
                oCCTrans.ICVResponse = "Y" & ICVResponse
                oCCTrans.Approved = 1
                oCCTrans.ApprovedBy = "Manual Charge"
                oCCTrans.DateApproved = System.DateTime.Now
                oCCTrans.Applied = 1
            End If
            oCCTrans.Save()
            iRet = oCCTrans.CCTransID

            If oCombo.Lookup_ComboItem(TransTypeID) <> "PreAuth" Then
                'Apply CCTrans to Payments
                Dim i As Integer = 0
                Dim oCCTransApply As New clsCCTransApplyTo
                For i = 0 To UBound(ApplyTo) - 1
                    oCCTransApply.CCTransApplyToID = 0
                    oCCTransApply.Load()
                    oCCTransApply.CCTransID = iRet
                    oCCTransApply.Amount = Amount(i)
                    oCCTransApply.PaymentID = ApplyTo(i)
                    oCCTransApply.UserID = Session("UserDBID")
                    oCCTransApply.Save()
                Next
                oCCTransApply = Nothing
            End If

            oCC.CreditCardID = CreditCardID
            oCC.Load()
            oCC.ReadyToImport = 1
            oCC.UserID = Session("UserDBID")
            oCC.Save()
            'lblErr.Text = oCC.Save()
            oCC = Nothing
        Catch ex As Exception
            lblErr.Text = ex.ToString
        End Try
        oCCTrans = Nothing
        oCombo = Nothing
        Return iRet
    End Function

    Private Function Create_Payment(ByVal sMethod As String, ByVal sAmount As String, ByVal bAdjustment As Boolean, ByVal bPosNeg As Boolean, Optional ByVal applyTo As Integer = 0, Optional ByVal desc As String = "", Optional ByVal ref As String = "") As Integer
        Dim iRet As Integer = 0
        Dim oPay As New clsPayments
        Dim oPay2 As New clsPayments
        Try
            oPay.PaymentID = 0
            oPay.Load()
            oPay.MethodID = Get_LookupID("PaymentMethod", sMethod)
            If sMethod = "NSF" Then
                oPay2.PaymentID = applyTo
                oPay2.Load()
                oPay.Reference = oPay2.Reference
                oPay2 = Nothing
            Else
                oPay.Reference = ref
            End If
            oPay.Adjustment = bAdjustment
            oPay.PosNeg = bPosNeg
            oPay.Description = desc
            oPay.Amount = sAmount
            oPay.ApplyToID = applyTo
            oPay.UserID = Session("UserDBID")
            oPay.TransDate = Date.Now
            oPay.Save()
            iRet = oPay.PaymentID
            If payments = "" Then
                payments = iRet
            Else
                payments = payments & "," & iRet
            End If
        Catch ex As Exception
            lblErr.Text = ex.ToString
        End Try
        oPay = Nothing
        Return iRet
    End Function
    Private Function Create_Payment_Tie(ByVal pmtID As Integer, ByVal invID As Integer, ByVal amt As Double) As Boolean
        Try
            Dim oP2I As New clsPayment2Invoice
            Dim oPmt As New clsPayments
            oPmt.PaymentID = pmtID
            oPmt.Load()

            oP2I.Load()
            oP2I.InvoiceID = invID
            oP2I.PaymentID = pmtID
            oP2I.Amount = amt
            'oP2I.PosNeg = True
            oP2I.PosNeg = oPmt.PosNeg
            oP2I.Save()
            oP2I = Nothing
            Return True
        Catch ex As Exception
            lblErr.Text = ex.ToString
            Return False
        End Try
    End Function

    Protected Sub tmrCheck_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tmrCheck.Tick
        tmrCheck.Enabled = False
        MultiView2.ActiveViewIndex = 1
        lblWaiting.Text = "Processing ... Please Wait <br />"
        lblWaiting.Text += hfTickCounter.Value + 1 & " Seconds"
        hfTickCounter.Value += 1
        'If hfTickCounter.Value > 1 Then
        '    Dim test As New clsHeartland
        '    test.Token = hfTokenValue.Value
        '    test.Zip = txtZip_CC.Text
        '    test.Address = txtBillingAddress_CC.Text
        '    test.City = txtCity_CC.Text
        '    test.State = siState_CC.SelectedName
        '    test.CCTransID = CCTransID.Value
        '    test.FirstName = (txtName_CC.Text.Split(" "))(0) 
        '    'test.LastName = (txtName_CC.Text.Split(" "))(1)
        '    lblWaiting.Text = test.Charge(txtAmount_CC.Text)
        '    hfTokenValue.Value = IIf(test.Token <> hfTokenValue.Value, test.Token, hfTokenValue.Value)
        '    'Exit Sub
        'End If
        If Not (Check_Status()) Or hfTickCounter.Value >= 50 Then
            MultiView2.ActiveViewIndex = 0
            If hfTickCounter.Value >= 10 And txtAuthorization_CC.Text = "" Then txtAuthorization_CC.Text = "Timer Expired"
            lblWaiting.Text = "Approved"
        Else
            'lblWaiting.Text = "Denied"
            tmrCheck.Enabled = True
        End If
    End Sub

    Private Function Check_Status() As Boolean
        Dim cn As New System.Data.SqlClient.SqlConnection(Resources.Resource.cns)
        Dim cm As New System.Data.SqlClient.SqlCommand("Select * from t_CCTrans where cctransid= " & CCTransID.Value, cn)
        Dim dr As System.Data.SqlClient.SqlDataReader
        Try
            cn.Open()
            dr = cm.ExecuteReader
            If dr.HasRows Then
                dr.Read()
                If dr("ICVResponse") & "" <> "" Then
                    txtAuthorization_CC.Text = dr("ICVResponse")
                    If Left(txtAuthorization_CC.Text, 1) <> "N" Then
                        lblWaiting.Text = "Approved"
                        hfReceiptURL.Value = "receipt.aspx?id=" & hfpayments.Value & "&sched=" '& CCTransID.Value
                    Else
                        lblWaiting.Text = "Denied"
                        hfReceiptURL.Value = 0
                    End If
                    Return False
                Else
                    Return True
                End If
            Else
                lblErr.Text = "Unable to locate transaction record with ID: " & CCTransID.Value
                Return False
            End If
            dr.Close()
            cn.Close()
        Catch ex As Exception
            lblWaiting.Text = ex.InnerException.Message
            Return False
        Finally
            If cn.State <> Data.ConnectionState.Closed Then cn.Close()
        End Try

        cn = Nothing
        cm = Nothing
        dr = Nothing
    End Function

 
    Protected Sub txtAuthorization_CC_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAuthorization_CC.TextChanged
        If lblWaiting.Text = "Approved" Then
            Receipt(CCTransID.Value, "CC")
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Receipt", "alert('Declined');", True)
        End If
    End Sub

    Protected Sub Receipt(ByVal id As Integer, ByVal sType As String)
        If sType = "adjustment" Then
            hfReceiptURL.value = "-1"
        Else
            If payments = "" Then
                hfReceiptURL.Value = "-1" '"receipt.aspx?id=" & payments & "&type=" & sType & "&Scheduled=" & Request("Schedule")
            Else
                hfReceiptURL.Value = "receipt.aspx?id=" & payments & "&type=" & sType & "&Scheduled=" & Request("Schedule")
            End If
            'Response.Redirect(hfReceiptURL.value)
        End If

    End Sub

    Protected Sub btnProcess_Check_ACH_MO_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcess_Check_ACH_MO.Click
        If Process_Payment() Then Receipt(0, "check")
    End Sub

    Protected Sub btnProcess_Cash_Equity_ExitEquity_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcess_Cash_Equity_ExitEquity.Click
        If Process_Payment() Then Receipt(0, "cash")
    End Sub

    Protected Sub btnProcess_Concord_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcess_Concord.Click
        If Process_Payment() Then Receipt(0, "concord")
    End Sub

    Protected Sub btnProcess_ChargeBackEmployee_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcess_ChargeBackEmployee.Click
        If Process_Payment() Then Receipt(0, "chargebackemployee")
    End Sub

    Protected Sub btnProcess_Adjustment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcess_Adjustment.Click
        If ddItemType.SelectedValue = "Invoice" Then
            If Process_Payment() Then Receipt(0, "adjustment")
        Else
            If Process_Adjustment() Then Receipt(0, "adjustment")
        End If
    End Sub

    Private Function Process_Adjustment() As Boolean
        Dim amt_Selected As Decimal = 0
        Dim amt_Entered As String = ""
        amt_Entered = txtAmount_Adjustment.Text
        If Not (Verify_Trans(6)) Then
            Return False
            Exit Function
        End If
        If amt_Entered = "" Or Not (IsNumeric(amt_Entered)) Then
            lblErr.Text = "Please enter a valid amount"
            Return False
            Exit Function
        End If
        Dim oPmt2Adj As New clsPaymentMethod2Adjustment
        Dim oPerm As Boolean = True
        Dim oInvAdj As Integer = 1
        If ddItemType.SelectedValue <> "Invoice" Then
            oInvAdj = 0
        End If
        If oPmt2Adj.Require_Permissions(ddAdjustments.SelectedValue, oInvAdj) Then
            If CheckSecurity("Payments", "Receive" & Replace(ddAdjustments.SelectedItem.Text, " ", "") & "Payments", , , Session("UserDBID")) Then
                lblErr.Text = "You Do Not Have Permission To Process This Adjustment Type."
                Return False
                Exit Function
            End If
        End If
        oPmt2Adj = Nothing

        If Verify_Adjustments(amt_Selected:=amt_Selected, amt_Entered:=amt_Entered) Then
            Dim tempAmt As Double = CDbl(amt_Entered)
            Dim bPmt As Boolean = False
            Dim paymentID As Integer = 0
            For i = 0 To gvPayments.Rows.Count - 1
                If CType(gvPayments.Rows(i).FindControl("cb"), CheckBox).Checked Then
                    If tempAmt > 0 Then
                        If tempAmt > CDbl(gvPayments.Rows(i).Cells(4).Text) Then
                            paymentID = Create_Payment(ddAdjustments.SelectedItem.Text, CDbl(gvPayments.Rows(i).Cells(4).Text), True, rbNeg.Checked, CInt(gvPayments.Rows(i).Cells(1).Text), txtDescription_Adjustment.Text, "Adjustment")
                        Else
                            paymentID = Create_Payment(ddAdjustments.SelectedItem.Text, tempAmt, True, rbNeg.Checked, CInt(gvPayments.Rows(i).Cells(1).text), txtDescription_Adjustment.Text, "Adjustment")
                        End If
                        tempAmt = tempAmt - CDbl(gvPayments.Rows(i).Cells(4).Text)
                    Else
                        Exit For
                    End If
                End If
            Next
            Return True
        Else
            Return False
        End If
    End Function

    Private Function Process_Payment() As Boolean

        If Request("Schedule") = "True" Then
            Return Process_Scheduled_Payment()
        Else
            Dim strMA As String = "0"
            Dim autoApprove As Boolean = True
            Dim amt_Selected As Decimal = 0
            Dim amt_Entered As String = ""
            Dim iPM As Integer = 0
            Dim oCombo As New clsComboItems
            Dim ccPMethod As String = ""
            Dim reference As String = ""
            Dim desc As String = ""
            Select Case ddPayMethod.SelectedValue
                Case "Credit Card" 'Method 1
                    amt_Entered = txtAmount_CC.Text
                    iPM = 1
                    desc = txtDescription_CC.Text
                Case "Check", "ACH Payment", "MoneyOrder" 'Method 2
                    amt_Entered = txtAmount_Check_ACH_MO.Text
                    If ddPayMethod.SelectedValue = "Check" Then
                        reference = "Check-" & txtNumber_Check_ACH_MO.Text
                    ElseIf ddPayMethod.SelectedValue = "ACH Payment" Then
                        reference = "ACH-" & txtNumber_Check_ACH_MO.Text
                    Else
                        reference = "MO-" & txtNumber_Check_ACH_MO.Text
                    End If
                    iPM = 2
                Case "Cash", "Equity", "Exit Equity", "Leisure Link" 'Method 3
                    amt_Entered = txtAmount_Cash_Equity_ExitEquity.Text
                    'reference = ddPayMethod.SelectedValue
                    iPM = 3
                Case "Concord Payment", "Aspen", "Meridian", "AFC Payment", "Meridian - AFC 'Method 4", "Equiant Payment"
                    amt_Entered = txtAmount_Concord.Text
                    iPM = 4
                    reference = IIf(txtReference_Concord.Text = "", ddPayMethod.SelectedValue & " - " & dtTransDate_Concord.Selected_Date, txtReference_Concord.Text) 'ddPaymentMethod.SelectedValue
                Case "ChargeBack Employee" 'Method 5
                    amt_Entered = txtAmount_ChargeBackEmployee.Text
                    iPM = 5
                Case "Adjustment" 'Method 6
                    desc = txtDescription_Adjustment.Text
                    amt_Entered = txtAmount_Adjustment.Text
                    iPM = 6
                Case "Online" 'Method 7
                    amt_Entered = ""
                    iPM = 7
                Case Else
                    amt_Entered = ""
            End Select
            If Not (Verify_Trans(iPM)) Then
                'lblErr.Text = "NO"
                Return False
                Exit Function
            End If
            If amt_Entered = "" Or Not (IsNumeric(amt_Entered)) Then
                lblErr.Text = "Please enter a valid amount"
                Return False
                Exit Function
            End If
            If iPM = 6 Then
                Dim oPmt2Adj As New clsPaymentMethod2Adjustment
                Dim oPerm As Boolean = True
                Dim oInvAdj As Integer = 1
                If ddItemType.SelectedValue <> "Invoice" Then
                    oInvAdj = 0
                End If
                If oPmt2Adj.Require_Permissions(ddAdjustments.SelectedValue, oInvAdj) Then
                    If Not (CheckSecurity("Payments", "Receive" & Replace(ddAdjustments.SelectedItem.Text, " ", "") & "Payments", , , Session("UserDBID"))) Then
                        lblErr.Text = "You Do Not Have Permission To Process This Adjustment Type."
                        Return False
                        Exit Function
                    End If
                End If
                oPmt2Adj = Nothing
            End If


            If Verify_Amounts(strMA:=strMA, amt_Selected:=amt_Selected, amt_Entered:=amt_Entered, autoApprove:=autoApprove) Then
                Dim bImported As Boolean = False
                Dim TransType As String = ""
                Dim iCCID As Integer = 0
                Dim iPreauthID As Integer = 0

                If iPM = 1 Then
                    'Get Trans Type
                    TransType = IIf(Charge.Checked, "Charge", IIf(Force.Checked, "Force", IIf(Manual.Checked, "Manual", IIf(VoiceAuth.Checked, "VoiceAuth", ""))))

                    'Get Credit Card Record
                    If hfCCID.Value <> "0" Then ' And Left(1, txtCardNumber_CC.Text) = "X" Then
                        iCCID = CInt(hfCCID.Value)
                        Dim oCCard As New clsCreditCard
                        oCCard.CreditCardID = iCCID
                        oCCard.Load()
                        oCCard.PostalCode = txtZip_CC.Text
                        oCCard.UserID = Session("UserDBID")
                        oCCard.Save()
                        oCCard = Nothing
                        '**** Get Payment Method ****'
                        Dim oCreditCard As New clsFinancials
                        ccPMethod = oCreditCard.Get_CCPaymentMethod(iCCID)
                        oCreditCard = Nothing
                    Else
                        If TransType = "Force" Then
                            iCCID = 0
                            ccPMethod = hfCardType.Value
                        Else
                            iCCID = Get_Credit_Card_Record()
                            If iCCID = 0 Then
                                lblErr.Text = "Unable to locate Credit Card Record"
                                Return False
                                Exit Function
                            End If
                            '**** Get Payment Method ****'
                            Dim oCreditCard As New clsFinancials
                            ccPMethod = oCreditCard.Get_CCPaymentMethod(iCCID)
                            oCreditCard = Nothing
                        End If
                    End If



                    'Charge, PreAuth and Force as normal
                    If TransType <> "" Then
                        If TransType = "Force" Then
                            'Verify Preauth availability and get the ID before continuing

                            iPreauthID = Verify_PreAuth(iCCID)

                            If iPreauthID = 0 Then Exit Function

                        ElseIf TransType = "Manual" Then
                            'If manual then insert a charge and mark as already imported
                            If txtAuthorization_CC.Text = "" Or txtAuthorization_CC.Text.Length < 6 Then
                                lblErr.Text = "Manual transactions require an authorization code."
                                Return False
                                Exit Function
                            End If
                            bImported = True
                            autoApprove = True
                            TransType = "Charge"
                            reference = txtAuthorization_CC.Text
                        ElseIf TransType = "VoiceAuth" Then
                            'If Voice Auth then run a preauth marked as imported and then a force to process
                            'Insert a PreAuth first of charge then continue as a Force
                            If txtAuthorization_CC.Text = "" Or txtAuthorization_CC.Text.Length < 6 Then
                                lblErr.Text = "Voice Authorized transactions require an authorization code."
                                Return False
                                Exit Function
                            End If
                            Dim paAmt(0) As String
                            Dim paApply(0) As String
                            paApply(0) = 0
                            paAmt(0) = amt_Entered
                            iPreauthID = Create_CCTrans_Record(Get_LookupID("CCTransType", "PreAuth"), strMA, paApply, paAmt, iCCID, 0, txtAuthorization_CC.Text, True, True)

                            If iPreauthID = 0 Then Exit Function

                            TransType = "Force"

                        ElseIf TransType = "Charge" Then
                            'Normal Processing

                        Else
                            lblErr.Text = "Unable to determine the type of transaction requested"
                            Return False
                            Exit Function
                        End If
                    End If

                End If

                'Create payment record
                Dim tempAmt As Double = CDbl(amt_Entered)
                Dim bPmt As Boolean = False
                Dim paymentID As Integer = 0
                Dim sPmts() As String = Nothing
                Dim sAmts() As String = Nothing
                Dim k As Integer = 0
                For i = 0 To gvInvoices.Rows.Count - 1
                    If CType(gvInvoices.Rows(i).FindControl("cb"), CheckBox).Checked Then
                        If tempAmt > 0 Then
                            If tempAmt > CDbl(gvInvoices.Rows(i).Cells(6).Text) Then
                                If iPM = 6 Then
                                    paymentID = Create_Payment(ddAdjustments.SelectedItem.Text, CDbl(gvInvoices.Rows(i).Cells(6).Text), IIf(iPM = 6, True, False), IIf(iPM = 6, rbNeg.Checked, True), , desc, reference)
                                ElseIf iPM = 1 Then
                                    If autoApprove Then
                                        paymentID = Create_Payment(ccPMethod, CDbl(gvInvoices.Rows(i).Cells(6).Text), IIf(iPM = 6, True, False), IIf(iPM = 6, rbNeg.Checked, True), , desc, reference)
                                    Else
                                        paymentID = gvInvoices.Rows(i).Cells(1).Text
                                    End If
                                Else
                                    paymentID = Create_Payment(ddPayMethod.SelectedValue, CDbl(gvInvoices.Rows(i).Cells(6).Text), IIf(iPM = 6, True, False), IIf(iPM = 6, rbNeg.Checked, True), , desc, reference)
                                End If
                                ReDim Preserve sPmts(k + 1)
                                ReDim Preserve sAmts(k + 1)
                                sAmts(k) = CDbl(gvInvoices.Rows(i).Cells(6).Text)
                                sPmts(k) = paymentID
                                If iPM = 1 Then
                                    If autoApprove Then
                                        If Not (Create_Payment_Tie(paymentID, gvInvoices.Rows(i).Cells(1).Text, CDbl(gvInvoices.Rows(i).Cells(6).Text))) Then
                                            Return False
                                        End If
                                    End If
                                Else
                                    If Not (Create_Payment_Tie(paymentID, gvInvoices.Rows(i).Cells(1).Text, CDbl(gvInvoices.Rows(i).Cells(6).Text))) Then
                                        Return False
                                    End If
                                End If
                                'If Not (iPM = 1 And Not (autoApprove)) Then
                                ' If Not (Create_Payment_Tie(paymentID, gvInvoices.Rows(i).Cells(1).text, CDbl(gvInvoices.Rows(i).Cells(6).Text))) Then
                                '      Return False
                                '   End If
                                'End If
                            Else
                                If iPM = 6 Then
                                    paymentID = Create_Payment(ddAdjustments.SelectedItem.Text, tempAmt, IIf(iPM = 6, True, False), IIf(iPM = 6, rbNeg.Checked, True), , desc, reference)
                                ElseIf iPM = 1 Then
                                    If autoApprove Then
                                        paymentID = Create_Payment(ccPMethod, tempAmt, IIf(iPM = 6, True, False), IIf(iPM = 6, rbNeg.Checked, True), , desc, reference)
                                    Else
                                        paymentID = gvInvoices.Rows(i).Cells(1).Text
                                    End If
                                Else
                                    paymentID = Create_Payment(ddPayMethod.SelectedValue, tempAmt, IIf(iPM = 6, True, False), IIf(iPM = 6, rbNeg.Checked, True), , desc, reference)
                                End If
                                ReDim Preserve sPmts(k + 1)
                                ReDim Preserve sAmts(k + 1)
                                sAmts(k) = CDbl(tempAmt)
                                sPmts(k) = paymentID
                                'If Not (iPM = 1 And Not (autoApprove)) Then
                                '   If Not (Create_Payment_Tie(paymentID, gvInvoices.Rows(i).Cells(1).text, tempAmt)) Then
                                '       Return False
                                '   End If
                                'End If
                                If iPM = 1 Then
                                    If autoApprove Then
                                        If Not (Create_Payment_Tie(paymentID, gvInvoices.Rows(i).Cells(1).Text, CDbl(tempAmt))) Then
                                            Return False
                                        End If
                                    End If
                                Else
                                    If Not (Create_Payment_Tie(paymentID, gvInvoices.Rows(i).Cells(1).Text, CDbl(tempAmt))) Then
                                        Return False
                                    End If
                                End If
                            End If
                            tempAmt = tempAmt - CDbl(gvInvoices.Rows(i).Cells(6).Text)
                        Else
                            Exit For
                        End If
                        k = k + 1
                    End If
                Next

                If iPM = 1 Then
                    'Create Credit Card Record
                    If bImported Then
                        CCTransID.Value = Create_CCTrans_Record(Get_LookupID("CCTransType", TransType), strMA, sPmts, sAmts, iCCID, iPreauthID, txtAuthorization_CC.Text, bImported, autoApprove)
                    Else
                        CCTransID.Value = Create_CCTrans_Record(Get_LookupID("CCTransType", TransType), strMA, sPmts, sAmts, iCCID, iPreauthID, "", bImported, autoApprove)
                    End If
                    If CCTransID.Value = 0 Then Exit Function
                    If autoApprove Then
                        If bImported Then
                            Receipt(CCTransID.Value, "CC")
                        Else
                            hfpayments.Value = payments
                            lblErr.Text = "Begin watching for CCTrans " & CCTransID.Value & " for process completion"
                            MultiView2.ActiveViewIndex = 1
                            tmrCheck.Interval = 2000
                            tmrCheck.Enabled = True
                        End If
                    Else
                        lblErr.Text = "CCTrans Submitted For Approval"
                    End If
                End If
                'lblErr.Text = paymentID
                Return True
            Else
                lblErr.Text = "Please select a Valid Invoice"
                Return False
            End If
        End If
    End Function

    Private Function Verify_Trans(ByVal iPM As Integer, Optional ByVal sched As Boolean = False) As Boolean
        Select Case iPM
            Case 1 '"Credit Card"
                Return Verify_Credit_Card_Trans()
            Case 2 '"Check", "ACH Payment", "Money Order"
                If Not (IsNumeric(txtNumber_Check_ACH_MO.Text)) Or txtNumber_Check_ACH_MO.Text = "" Then
                    lblErr.Text = "Please enter a numeric check number"
                    Return False
                ElseIf Not (IsNumeric(txtAmount_Check_ACH_MO.Text)) Or txtAmount_Check_ACH_MO.Text = "" Then
                    lblErr.Text = "Please enter an Amount"
                    Return False
                Else
                    If sched Then
                        If dteScheduledCheck.Selected_Date = "" Then
                            lblErr.Text = "Please Enter as Scheduled Date."
                            Return False
                        ElseIf Date.Compare(System.DateTime.Now, CDate(dteScheduledCheck.Selected_Date)) > 0 Then
                            lblErr.Text = "Please Enter a Valid Scheduled Date."
                            Return False
                        Else
                            Return True
                        End If
                    Else
                        Return True
                    End If
                End If
            Case 3 '"Cash", "Equity", "Exit Equity"
                If Not (IsNumeric(txtAmount_Cash_Equity_ExitEquity.Text)) Or txtAmount_Cash_Equity_ExitEquity.Text = "" Then
                    lblErr.Text = "Please enter an Amount"
                    Return False
                Else
                    If sched Then
                        If dteScheduledCash.Selected_Date = "" Then
                            lblErr.Text = "Please Enter as Scheduled Date."
                            Return False
                        ElseIf Date.Compare(System.DateTime.Now, CDate(dteScheduledCash.Selected_Date)) > 0 Then
                            lblErr.Text = "Please Enter a Valid Scheduled Date."
                            Return False
                        Else
                            Return True
                        End If
                    Else
                        Return True
                    End If
                End If
            Case 4 '"Concord Payment"
                If Not (IsNumeric(txtAmount_Concord.Text)) Or txtAmount_Concord.Text = "" Then
                    lblErr.Text = "Please enter an Amount"
                    Return False
                Else
                    If sched Then
                        If dteScheduledConcord.Selected_Date = "" Then
                            lblErr.Text = "Please Enter as Scheduled Date."
                            Return False
                        ElseIf Date.Compare(System.DateTime.Now, CDate(dteScheduledConcord.Selected_Date)) > 0 Then
                            lblErr.Text = "Please Enter a Valid Scheduled Date."
                            Return False
                        Else
                            Return True
                        End If
                    Else
                        Return True
                    End If
                End If
            Case 5 '"ChargeBack Employee"
                lblErr.Text = "This function has been disabled"
                Return False
            Case 6 '"Adjustment"
                If Not (rbNeg.Checked Or rbPos.Checked) Then
                    lblErr.Text = "Please select either Positive or Negative"
                    Return False
                ElseIf Not (IsNumeric(txtAmount_Adjustment.Text)) Or txtAmount_Adjustment.Text = "" Then
                    lblErr.Text = "Please enter an Amount"
                    Return False
                ElseIf txtDescription_Adjustment.Text = "" Then
                    lblErr.Text = "Please enter a description"
                    Return False
                Else
                    Return True
                End If
            Case 7 '"Online"

                lblErr.Text = "This function has been disabled"
                Return False
            Case Else '
                Return False
        End Select

    End Function

    Protected Sub Load_Adjustments()
        If ddItemType.SelectedValue = "Invoice" Then
            Load_Invoices(False)
        Else
            Load_Payments()
        End If

    End Sub

    Protected Sub ddItemType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddItemType.SelectedIndexChanged
        Load_Adjustments()
    End Sub

    Protected Sub ddAdjustments_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddAdjustments.SelectedIndexChanged
        If ddAdjustments.SelectedItem.Text = "NSF" Then
            Dim oPay As clsPayments = New clsPayments
            oPay.KeyField = Request("KeyField")
            If Request("KeyField") = "ProspectID" Then
                oPay.KeyValue = Request("ProspectID")
            Else
                oPay.KeyValue = Request("KeyValue")
            End If
            oPay.ProspectID = Request("ProspectID")
            gvPayments.DataSource = oPay.List_Checks
            gvPayments.DataBind()
            oPay = Nothing
            rbNeg.Checked = False
            rbPos.Checked = True
            rbNeg.Enabled = False
        Else
            Dim oPay As clsPayments = New clsPayments
            oPay.KeyField = Request("KeyField")
            oPay.KeyValue = Request("KeyValue")
            oPay.ProspectID = Request("ProspectID")
            gvPayments.DataSource = oPay.List_Payments
            gvPayments.DataBind()
            oPay = Nothing
            'If ddAdjustments.SelectedItem.Text = "HPS Dispute" Then
            '    rbNeg.Checked = False
            '    rbPos.Checked = True
            '    rbNeg.Enabled = False
            'Else
            rbNeg.Checked = True
            rbNeg.Enabled = True
            rbPos.Enabled = True
            'End If
        End If
    End Sub
    Protected Sub btnScheduleCash_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnScheduleCash.Click
        If Process_Scheduled_Payment() Then
            Receipt(0, "cash")
        End If
    End Sub


    Private Function Process_Scheduled_Payment() As Boolean
        Dim strMA As String = "0"
        Dim amt_Selected As Decimal = 0
        Dim amt_Entered As String = ""
        Dim iPM As Integer = 0
        Dim schedDate As Date
        Dim ccPMethod As String = ""
        Dim reference As String = ""
        Dim desc As String = ""
        Select Case ddPayMethod.SelectedValue
            Case "Credit Card" 'Method 1
                amt_Entered = txtAmount_CC.Text
                schedDate = dteScheduledCC.Selected_Date
                desc = txtDescription_CC.Text
                iPM = 1
            Case "Check", "ACH Payment", "MoneyOrder" 'Method 2
                amt_Entered = txtAmount_Check_ACH_MO.Text
                schedDate = dteScheduledCheck.Selected_Date
                If ddPayMethod.SelectedValue = "Check" Then
                    reference = "Check-" & txtNumber_Check_ACH_MO.Text
                ElseIf ddPayMethod.SelectedValue = "ACH Payment" Then
                    reference = "ACH-" & txtNumber_Check_ACH_MO.Text
                Else
                    reference = "MO-" & txtNumber_Check_ACH_MO.Text
                End If
                iPM = 2
            Case "Cash", "Equity", "Exit Equity" 'Method 3
                amt_Entered = txtAmount_Cash_Equity_ExitEquity.Text
                schedDate = dteScheduledCash.Selected_Date
                iPM = 3
            Case "Concord Payment" 'Method 4
                amt_Entered = txtAmount_Concord.Text
                schedDate = dteScheduledConcord.Selected_Date
                iPM = 4
            Case "ChargeBack Employee" 'Method 5
                amt_Entered = txtAmount_ChargeBackEmployee.Text
                iPM = 5
            Case "Adjustment" 'Method 6
                amt_Entered = txtAmount_Adjustment.Text
                iPM = 6
            Case "Online" 'Method 7
                amt_Entered = ""
                iPM = 7
            Case Else
                amt_Entered = ""
        End Select
        If Not (Verify_Trans(iPM, True)) Then
            Return False
            Exit Function
        End If
        If amt_Entered = "" Or Not (IsNumeric(amt_Entered)) Then
            lblErr.Text = "Please enter a valid amount"
            Return False
            Exit Function
        End If
        If Verify_Amounts(strMA:=strMA, amt_Selected:=amt_Selected, amt_Entered:=amt_Entered, autoApprove:=False) Then
            Dim iCCID As Integer = 0

            If iPM = 1 Then
                'Get Credit Card Record
                If hfCCID.Value <> "0" Then ' And Left(1, txtCardNumber_CC.Text) = "X" Then
                    iCCID = CInt(hfCCID.Value)
                    Dim oCCard As New clsCreditCard
                    oCCard.CreditCardID = iCCID
                    oCCard.Load()
                    oCCard.PostalCode = txtZip_CC.Text
                    'oCCard.Token = hfTokenValue.Value
                    oCCard.Save()
                    oCCard = Nothing
                Else
                    
                    iCCID = Get_Credit_Card_Record()
                    If iCCID = 0 Then Exit Function

                End If
            End If
            '**** Get Payment Method ****'
            ccPMethod = (New clsFinancials).Get_CCPaymentMethod(iCCID)

            Dim Total_Amount As Double = CDbl(amt_Entered)
            Dim iDay As Integer = Day(schedDate)
            Dim iMonth As Integer = Month(schedDate)
            Dim iYear As Integer = Year(schedDate)
            For z = 0 To CInt(txtPayments.Text) - 1
                If cbAutoSplit.Checked Then
                    If z = CInt(txtPayments.Text) - 1 Then
                        amt_Entered = Total_Amount - (Math.Round(Total_Amount / CInt(txtPayments.Text), 2) * (CInt(txtPayments.Text) - 1))
                    Else
                        amt_Entered = Math.Round(Total_Amount / CInt(txtPayments.Text), 2)
                    End If
                End If
                'Modify the scheddate
                If iMonth = 12 And z > 0 Then
                    iMonth = 1
                    iYear += 1
                Else
                    iMonth += IIf(z > 0, 1, 0)
                End If

                If IsDate(iMonth & "/" & iDay & "/" & iYear) Then
                    schedDate = CDate(iMonth & "/" & iDay & "/" & iYear)
                Else
                    Dim x As Integer = 0
                    Do While Not (IsDate(iMonth & "/" & iDay - x & "/" & iYear))
                        x += 1
                        If x > 30 Then Exit Do
                    Loop
                    schedDate = CDate(iMonth & "/" & iDay - x & "/" & iYear)
                End If

                'Create payment records
                Dim paymentID As Integer = 0
                Dim tempAmt As Double = CDbl(amt_Entered) 'Changed from cdbl(amt_Selected) to cdbl(amt_Entered) 
                Dim bPmt As Boolean = False
                For i = 0 To gvInvoices.Rows.Count - 1
                    If CType(gvInvoices.Rows(i).FindControl("cb"), CheckBox).Checked Then

                        If tempAmt > 0 Then
                            If tempAmt > CDbl(gvInvoices.Rows(i).Cells(6).Text) Then
                                If CDbl(gvInvoices.Rows(i).Cells(6).Text) > 0 Then
                                    If iPM = 1 Then
                                        paymentID = Create_Scheduled_Payment(ccPMethod, CDbl(gvInvoices.Rows(i).Cells(6).Text), schedDate, iCCID, desc, reference)
                                    Else
                                        paymentID = Create_Scheduled_Payment(ddPayMethod.SelectedValue, CDbl(gvInvoices.Rows(i).Cells(6).Text), schedDate, iCCID, desc, reference)
                                    End If
                                    If Not (Create_Tie_Scheduled(paymentID, gvInvoices.Rows(i).Cells(1).Text, CDbl(gvInvoices.Rows(i).Cells(6).Text))) Then
                                        Return False
                                    End If
                                End If
                                tempAmt = tempAmt - CDbl(gvInvoices.Rows(i).Cells(6).Text)
                                gvInvoices.Rows(i).Cells(6).Text = "0"
                            Else
                                If iPM = 1 Then
                                    paymentID = Create_Scheduled_Payment(ccPMethod, tempAmt, schedDate, iCCID, desc, reference)
                                Else
                                    paymentID = Create_Scheduled_Payment(ddPayMethod.SelectedValue, tempAmt, schedDate, iCCID, desc, reference)
                                End If
                                If Not (Create_Tie_Scheduled(paymentID, gvInvoices.Rows(i).Cells(1).Text, tempAmt)) Then
                                    Return False
                                End If
                                gvInvoices.Rows(i).Cells(6).Text = CDbl(gvInvoices.Rows(i).Cells(6).Text) - tempAmt
                                tempAmt = 0
                            End If

                        Else
                            Exit For
                        End If
                    End If
                Next
            Next
            If iPM = 1 Then
                Dim oCC As New clsCreditCard
                oCC.CreditCardID = iCCID
                oCC.Load()
                oCC.UserID = Session("UserDBID")
                If oCC.ReadyToImport = False Then
                    oCC.ReadyToImport = True
                    oCC.ImportedID = -100
                    oCC.Save()
                End If
                oCC = Nothing
            End If
            'lblErr.Text = paymentID
            Return True
        Else
            Return False
        End If
    End Function

    Private Function Create_Scheduled_Payment(ByVal sMethod As String, ByVal sAmount As String, ByVal schedDate As Date, ByVal ccID As Integer, ByVal desc As String, ByVal reference As String) As Integer
        Dim iRet As Integer = 0
        Dim oPay As New clsPaymentsScheduled
        oPay.SchedPayID = 0
        oPay.Load()
        oPay.MethodID = Get_LookupID("PaymentMethod", sMethod)
        oPay.Amount = sAmount
        oPay.UserID = Session("UserDBID")
        oPay.TransDate = Date.Now
        oPay.CreditCardID = ccID
        oPay.SchedDate = schedDate
        oPay.CreatedDate = System.DateTime.Now
        oPay.Reference = reference
        oPay.Description = desc
        If oPay.Save() Then
            iRet = oPay.SchedPayID
            If payments = "" Then
                payments = iRet
            Else
                payments = payments & "," & iRet
            End If
        Else
            lblErr.Text = oPay.Err
        End If
        oPay = Nothing
        Return iRet
    End Function

    Private Function Create_Tie_Scheduled(ByVal PayID As Integer, ByVal InvID As Integer, ByVal amt As Double) As Boolean
        Try
            Dim oP2I As New clsPaymentSched2Invoice
            oP2I.Load()
            oP2I.InvoiceID = InvID
            oP2I.SchedPaymentID = PayID
            oP2I.Amount = amt
            oP2I.Save()
            oP2I = Nothing
            Return True
        Catch ex As Exception
            lblErr.Text = ex.ToString
            Return False
        End Try
    End Function

    Protected Sub btnRetAddress_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRetAddress.Click
        Dim oProsAddress As New clsAddress
        Dim addID As Integer = 0
        addID = oProsAddress.Retrieve_Address(CInt(Request("ProspectID")))
        If addID > 0 Then
            oProsAddress.AddressID = addID
            oProsAddress.Load()
            txtBillingAddress_CC.Text = oProsAddress.Address1
            txtCity_CC.Text = oProsAddress.City
            txtZip_CC.Text = oProsAddress.PostalCode
            siState_CC.Selected_ID = oProsAddress.StateID
        End If
        oProsAddress = Nothing
    End Sub


    Protected Sub btnScheduleConcord_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnScheduleConcord.Click
        If Process_Scheduled_Payment() Then
            Receipt(0, "concord")
        End If
    End Sub

    Protected Sub Unnamed2_Click(sender As Object, e As System.EventArgs)
        MultiView1.ActiveViewIndex = 0
        gvInvoices.visible = True
    End Sub

    Protected Sub Unnamed2_Click1(sender As Object, e As System.EventArgs)
        MultiView1.ActiveViewIndex = 7
        gvInvoices.visible = False
        Dim oCC As New clsCreditCard
        gvCardOnFile.DataSource = oCC.Get_Card_OnFile(Request("ProspectID"))
        gvCardOnFile.DataBind()
        oCC = Nothing
    End Sub

    Protected Sub gvCardOnFile_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvCardOnFile.SelectedIndexChanged
        Dim row As GridViewRow = gvCardOnFile.SelectedRow
        hfCCID.Value = row.Cells(1).Text
        txtCardNumber_CC.Text = row.Cells(2).Text & ""
        txtCVV2_CC.Text = Replace(row.Cells(4).Text, "&nbsp;", "") & ""
        txtExpiration_CC.Text = row.Cells(3).Text & ""
        txtName_CC.Text = Replace(row.Cells(5).Text, "&nbsp;", "") & ""
        txtBillingAddress_CC.Text = Replace(row.Cells(6).Text, "&nbsp;", "") & ""
        txtCity_CC.Text = Replace(row.Cells(7).Text, "&nbsp;", "") & ""
        siState_CC.Selected_ID = row.Cells(8).Text
        txtZip_CC.Text = Replace(row.Cells(9).Text, "&nbsp;", "") & ""
        hfTokenValue.Value = row.Cells(10).Text & ""
        If hfTokenValue.Value = "" Then hfTokenValue.Value = "0"
        MultiView1.ActiveViewIndex = 0
        gvInvoices.visible = True
    End Sub

    Protected Sub gvCardOnFile_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Cards On File" Then
            e.Row.Cells(1).Visible = False
            e.Row.Cells(6).Visible = False
            e.Row.Cells(7).Visible = False
            e.Row.Cells(8).Visible = False
            e.Row.Cells(9).Visible = False
        End If
    End Sub

    Protected Sub btnScheduleCheck_Click(sender As Object, e As System.EventArgs) Handles btnScheduleCheck.Click
        If Process_Scheduled_Payment() Then
            Receipt(0, "check")
        End If
    End Sub




    Protected Sub hfTokenValue_ValueChanged(sender As Object, e As EventArgs) Handles hfTokenValue.ValueChanged

    End Sub
    Protected Sub hfCardType_ValueChanged(sender As Object, e As EventArgs) Handles hfCardType.ValueChanged

    End Sub
End Class
