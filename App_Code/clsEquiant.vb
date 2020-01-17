Imports Microsoft.VisualBasic
Imports System.Data
Imports System.ComponentModel

Public Class clsEquiant
    Dim oEq As New Equiant.GatewaySoapClient
    Dim _Err As String = ""

    Public Function get_Auth() As Equiant.Credentials
        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12
        Dim x As New Equiant.Credentials
        x.UserName = "KcpWebServices"
        x.Password = "RichH6334"
        Return x
    End Function

    Public Function AccountClass(ByVal acctNumber As String) As Info 'Equiant.Info
        Dim i As Equiant.Info
        Dim iA As Info = Nothing
        Try
            i = oEq.AccountClass(get_Auth(), acctNumber)
            iA = New Info
            iA.AccountID = i.AccountID
            iA.BillClientID = i.BillClientID
            iA.BillDisposition = i.BillDisposition
            iA.ClassCodes = i.ClassCodes
            iA.DocClientID = i.DocClientID
            iA.DocDisposition = i.DocDisposition
            iA.DocLenderCode = i.DocLenderCode
            iA.Group = i.Group
            iA.LenderCode = i.LenderCode
            iA.LoanClientID = i.LoanClientID
            iA.LoanDisposition = i.LoanDisposition
            iA.Project = i.Project
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return iA
    End Function

    Public Function AccountCommunicationList(ByVal acctNumber As String, ByVal type As String) As List(Of Communication)
        Dim b As Equiant.Communication()
        Dim ret As New List(Of Communication)

        Try
            b = oEq.AccountCommunicationList(get_Auth(), acctNumber, IIf(type = "Phone", Equiant.CommunicationTypes.Phones, Equiant.CommunicationTypes.Emails))
            For Each r As Equiant.Communication In b
                Dim n As New Communication
                n.EmailStatements = r.EmailStatements
                n.Name = r.Name
                n.Sequence = r.Sequence
                n.Type = r.Type
                ret.Add(n)
                n = Nothing
            Next
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ret
    End Function

    Public Function AccountInformation(ByVal acctNumber As String) As PersonalInformation 'Equiant.PersonalInformation
        Dim t As Equiant.PersonalInformation
        Dim pi As PersonalInformation = Nothing
        Dim x As String = ""
        Try
            t = oEq.AccountInformation(get_Auth(), acctNumber)
            pi = New PersonalInformation
            pi.Address = t.Address.ToList
            pi.city = t.City
            pi.state = t.State
            pi.PostalCode = t.PostalCode
            pi.Country = t.Country
            pi.GoodAddress = t.GoodAddress
            For Each per As Equiant.Person In t.Person
                Dim p As New Person
                p.FirstName = per.FirstName
                p.LastName = per.LastName
                p.MiddleName = per.MiddleName
                p.Sequence = per.Sequence
                pi.person.Add(p)
                p = Nothing
            Next
            For Each p As Equiant.Communication In t.Phone
                Dim ph As New Communication
                ph.Name = p.Name
                ph.Type = p.Type
                ph.EmailStatements = p.EmailStatements
                ph.Sequence = p.Sequence
                pi.phone.Add(ph)
                ph = Nothing
            Next
            For Each p As Equiant.Communication In t.Email
                Dim em As New Communication
                em.Name = p.Name
                em.Type = p.Type
                em.EmailStatements = p.EmailStatements
                em.Sequence = p.Sequence
                pi.email.Add(em)
                em = Nothing
            Next
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return pi
    End Function

    Public Function BillingInformation(ByVal acctNumber As String) As Billing 'Equiant.Billing
        Dim b As Equiant.Billing
        Dim bA As Billing = Nothing
        Dim s As ServiceSettings
        Dim es As Equiant.ServiceSettings
        Dim f As Fee
        Dim ef As Equiant.Fee
        Try
            b = oEq.BillingInformation(get_Auth(), acctNumber)
            bA = New Billing
            bA.ActiveDisposition = b.ActiveDisposition
            bA.Client = b.Client
            bA.Currency = b.Currency
            bA.CurrentBilledDueDate = b.CurrentBilledDueDate
            bA.CurrentDue = b.CurrentDue
            bA.DaysDelinquent = b.DaysDelinquent
            bA.Description = b.Description
            bA.DispositionCode = b.DispositionCode
            bA.DispositionCodeDescription = b.DispositionCodeDescription
            bA.FinanceChargeDue = b.FinanceChargeDue
            bA.FinanceChargeRate = b.FinanceChargeRate
            bA.FutureRateAmount = b.FutureRateAmount
            bA.FutureRateDate = b.FutureRateDate
            bA.LastApplyAmount = b.LastApplyAmount
            bA.LastApplyCodeDesc = b.LastApplyCodeDesc
            bA.LateChargeCode = b.LateChargeCode
            bA.LateChargeDesc = b.LateChargeDesc
            bA.LateChargeDue = b.LateChargeDue
            bA.NextBillDueDate = b.NextBillDueDate
            bA.NextBillDueDate = b.NextStatementDueDate
            bA.PastDue = b.PastDue
            bA.PaymentAmount = b.PaymentAmount
            bA.PaymentFreq = b.PaymentFreq
            bA.PaymentFreqDesc = b.PaymentFreqDesc
            bA.PaymentMethod = b.PaymentMethod
            bA.PerDiem = b.PerDiem
            es = b.Settings
            s = New ServiceSettings
            s.AchTypes = es.AchTypes
            s.AllowACH = es.AllowACH
            s.AllowCheck = es.AllowCheck
            s.AllowCreditCard = es.AllowCreditCard
            s.AllowPayments = es.AllowPayments
            s.AllowPaymentsReason = es.AllowPaymentsReason
            s.AutoCCDaysOfMonth = es.AutoCCDaysOfMonth
            s.AutoCreditCard = es.AutoCreditCard
            s.AutoCreditCardOnAmount = es.AutoCreditCardOnAmount
            s.AutoPaidAhead = es.AutoPaidAhead
            s.CategoryCodes = es.CategoryCodes
            s.CreditCardAuthDays = es.CreditCardAuthDays
            s.CreditCardAuthsRequired = es.CreditCardAuthsRequired
            s.CreditCardBank = es.CreditCardBank
            s.CreditCardTypes = es.CreditCardTypes
            s.Currency = es.Currency
            s.ExtraPaymentType = es.ExtraPaymentType
            s.ExtraPaymentTypeDescription = es.ExtraPaymentTypeDescription
            s.ExtraPaymentTypeMaxAmount = es.ExtraPaymentTypeMaxAmount
            s.ExtraPaymentTypeMinAmount = es.ExtraPaymentTypeMinAmount
            s.MaxOTCCPayment = es.MaxOTCCPayment
            s.MaxOTSPpayment = es.MaxOTSPpayment
            s.MaxPRCRPayment = es.MaxPRCRPayment
            s.MaxRegularPayment = es.MaxRegularPayment
            s.merchantID = es.MerchantId
            s.MerchantMessage = es.MerchantMessage
            s.MinCreditCardPayment = es.MinCreditCardPayment
            s.MinSurePayPayment = es.MinSurePayPayment
            s.PostDateDays = es.PostDateDays
            s.ServiceFee = es.ServiceFee
            s.SurePay = es.SurePay
            s.SurePayDaysOfMonth = es.SurePayDaysOfMonth
            s.SurePayOnAmount = es.SurePayOnAmount
            ef = es.Fees
            f = New Fee
            f.Client = ef.Client
            f.Consumer = ef.Consumer
            f.IP = ef.IP
            f.IVR = ef.IVR
            f.OTCC = ef.OTCC
            f.OTSP = ef.OTSP
            s.Fees = f
            bA.Settings = s
            bA.TotalDue = b.TotalDue
            bA.Unit = b.Unit
            bA.Week = b.Week
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return bA
    End Function

    Public Function BillingSettings(ByVal acctNumber As String) As ServiceSettings
        Dim es As Equiant.ServiceSettings
        Dim ef As Equiant.Fee
        Dim s As ServiceSettings = Nothing
        Dim f As Fee
        Try
            es = oEq.BillingSettings(get_Auth(), acctNumber)
            s = New ServiceSettings
            s.AchTypes = es.AchTypes
            s.AllowACH = es.AllowACH
            s.AllowCheck = es.AllowCheck
            s.AllowCreditCard = es.AllowCreditCard
            s.AllowPayments = es.AllowPayments
            s.AllowPaymentsReason = es.AllowPaymentsReason
            s.AutoCCDaysOfMonth = es.AutoCCDaysOfMonth
            s.AutoCreditCard = es.AutoCreditCard
            s.AutoCreditCardOnAmount = es.AutoCreditCardOnAmount
            s.AutoPaidAhead = es.AutoPaidAhead
            s.CategoryCodes = es.CategoryCodes
            s.CreditCardAuthDays = es.CreditCardAuthDays
            s.CreditCardAuthsRequired = es.CreditCardAuthsRequired
            s.CreditCardBank = es.CreditCardBank
            s.CreditCardTypes = es.CreditCardTypes
            s.Currency = es.Currency
            s.ExtraPaymentType = es.ExtraPaymentType
            s.ExtraPaymentTypeDescription = es.ExtraPaymentTypeDescription
            s.ExtraPaymentTypeMaxAmount = es.ExtraPaymentTypeMaxAmount
            s.ExtraPaymentTypeMinAmount = es.ExtraPaymentTypeMinAmount
            s.MaxOTCCPayment = es.MaxOTCCPayment
            s.MaxOTSPpayment = es.MaxOTSPpayment
            s.MaxPRCRPayment = es.MaxPRCRPayment
            s.MaxRegularPayment = es.MaxRegularPayment
            s.merchantID = es.MerchantId
            s.MerchantMessage = es.MerchantMessage
            s.MinCreditCardPayment = es.MinCreditCardPayment
            s.MinSurePayPayment = es.MinSurePayPayment
            s.PostDateDays = es.PostDateDays
            s.ServiceFee = es.ServiceFee
            s.SurePay = es.SurePay
            s.SurePayDaysOfMonth = es.SurePayDaysOfMonth
            s.SurePayOnAmount = es.SurePayOnAmount
            ef = es.Fees
            f = New Fee
            f.Client = ef.Client
            f.Consumer = ef.Consumer
            f.IP = ef.IP
            f.IVR = ef.IVR
            f.OTCC = ef.OTCC
            f.OTSP = ef.OTSP
            s.Fees = f
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return s
    End Function

    Public Function BillingCategoryBalanceHeaders(ByVal acctNumber As String) As String()
        Dim s As String() = Nothing
        Try
            s = oEq.BillingCategoryBalanceHeaders(get_Auth(), acctNumber)
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return s
    End Function

    Public Function BillingCategoryBalanceHistory(ByVal acctNumber As String, ByVal year As Integer) As DataTable
        Dim dt As DataTable = Nothing
        Try
            dt = oEq.BillingCategoryBalanceHistory(get_Auth(), acctNumber, year)
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return dt
    End Function

    Public Function BillingTransactionDetails(ByVal acctNumber As String, ByVal year As Integer) As List(Of BillingTransaction) 'Equiant.BillingTransaction()
        Dim b As Equiant.BillingTransaction()
        Dim bD As List(Of BillingTransaction) = Nothing
        Try
            b = oEq.BillingTransactionDetails(get_Auth(), acctNumber, year)
            bD = New List(Of BillingTransaction)
            For Each item In b.ToList
                Dim n As New BillingTransaction
                n.CategoryDescriptions = item.CategoryDescriptions
                n.Code = item.Code
                n.CurrentBilledAmount = item.CurrentBilledAmount
                If Not (item.DueDate Is Nothing) Then n.DueDate = item.DueDate
                n.FinanceChargeBalance = item.FinanceChargeBalance
                n.LateChargeAmount = item.LateChargeAmount
                n.PastDueAmount = item.PastDueAmount
                n.PaymentID = item.PaymentID
                n.SourceCode = item.SourceCode
                n.SourceInformation = item.SourceInformation
                n.TotalDueAmount = item.TotalDueAmount
                n.TransactionAmount = item.TransactionAmount
                n.TransactionID = item.TransactionID
                n.TransDate = item.Date
                bD.Add(n)
                n = Nothing
            Next
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return bD
    End Function

    Public Function BillingTransactionsYearsList(ByVal acctNumber As String) As String()
        Dim b As String() = Nothing
        Try
            b = oEq.BillingTransactionsYearsList(get_Auth(), acctNumber)
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return b
    End Function

    Public Function EmailTypes(ByVal acctNumber As String) As List(Of Communication) 'Equiant.Communication()
        Dim b As List(Of Equiant.Communication)
        Dim bA As List(Of Communication) = Nothing
        Try
            b = oEq.EmailTypes(get_Auth(), acctNumber).ToList
            bA = New List(Of Communication)
            For Each item In b
                Dim n As New Communication
                n.EmailStatements = item.EmailStatements
                n.Name = item.Name
                n.Sequence = item.Sequence
                n.Type = item.Type
                bA.Add(n)
                n = Nothing
            Next
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return bA
    End Function

    Public Function GetStatementLink(ByVal acctNumber As String, ByVal RecordID As Integer, ByVal ClientID As Integer) As String
        Dim ret As String = Nothing
        Try
            ret = oEq.GetStatementLink(get_Auth, acctNumber, RecordID, ClientID)
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ret
    End Function

    Public Function GetStatements(ByVal acctNumber As String) As List(Of Statement)
        Dim ret As List(Of Statement) = Nothing
        Dim resp As Equiant.Statement()
        Try
            resp = oEq.GetStatements(get_Auth, acctNumber)
            ret = New List(Of Statement)
            For i = 0 To UBound(resp)
                Dim s As New Statement
                s.ClientID = resp(i).ClientID
                s.DataRecordID = resp(i).DataRecordID
                s.CancelBy = resp(i).CancelBy
                s.CancelDate = resp(i).CancelDate
                s.Dates = resp(i).Date
                s.Description = resp(i).Description
                s.ErrorCode = resp(i).ErrorCode
                s.ErrorDescription = resp(i).ErrorDescription
                s.Status = resp(i).Status
                s.Type = resp(i).Type
                s.User = resp(i).User
                s.VSN = resp(i).VSN
                ret.Add(s)
                s = Nothing
            Next

        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ret
    End Function

    Public Function InsertEmailAddress(ByVal acctNumber As String, ByVal Sequence As Integer, ByVal email As String, ByVal type As String, ByVal EmailStatements As Boolean) As Boolean
        Dim b As Boolean = False
        Try
            b = oEq.InsertEmailAddress(get_Auth(), acctNumber, Sequence, email, type, EmailStatements)
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return b
    End Function

    Public Function RemoveEmailAddress(ByVal acctNumber As String, ByVal Sequence As Integer, ByVal email As String, ByVal type As String) As Boolean
        Dim b As Boolean = False
        Try
            b = oEq.RemoveEmailAddress(get_Auth(), acctNumber, Sequence, email, type)
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return b
    End Function

    Public Function UpdateEmailAddress(ByVal acctNumber As String, ByVal Sequence As Integer, ByVal email As String, ByVal type As String, ByVal EmailStatements As Boolean) As Boolean
        Dim b As Boolean = False
        Try
            b = oEq.UpdateEmailAddress(get_Auth(), acctNumber, Sequence, email, type, EmailStatements)
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return b
    End Function

    Public Function PhoneTypes(ByVal acctNumber As String) As List(Of Communication) 'Equiant.Communication()
        Dim req As Equiant.Communication()
        Dim ret As List(Of Communication) = Nothing
        Try
            req = oEq.PhoneTypes(get_Auth(), acctNumber)
            For Each b In req
                Dim bA As New Communication
                bA.EmailStatements = b.EmailStatements
                bA.Name = b.Name
                bA.Sequence = b.Sequence
                bA.Type = b.Type
                ret.Add(bA)
                bA = Nothing
            Next
        Catch ex As Exception
            _Err = ex.Message.ToString '& " " & ex.InnerException.ToString
        End Try
        Return ret
    End Function

    Public Function InsertPhoneNumber(ByVal acctNumber As String, ByVal Sequence As Integer, ByVal phone As String, ByVal type As String) As Boolean
        Dim b As Boolean = False
        Try
            b = oEq.InsertPhoneNumber(get_Auth(), acctNumber, Sequence, phone, type)
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return b
    End Function

    Public Function RemovePhoneNumber(ByVal acctNumber As String, ByVal Sequence As Integer, ByVal phone As String, ByVal type As String) As Boolean
        Dim b As Boolean = False
        Try
            b = oEq.RemovePhoneNumber(get_Auth(), acctNumber, Sequence, phone, type)
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return b
    End Function

    Public Function UpdatePhoneNumber(ByVal acctNumber As String, ByVal Sequence As Integer, ByVal phone As String, ByVal type As String) As Boolean
        Dim b As Boolean = False
        Try
            b = oEq.UpdatePhoneNumber(get_Auth(), acctNumber, Sequence, phone, type)
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return b
    End Function

    Public Function IsValidAccount(ByVal acctNumber As String, ByVal phone As String) As Boolean
        Dim b As Boolean = False
        Try
            b = oEq.IsValidAccount(get_Auth(), acctNumber, phone)
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return b
    End Function

    Public Function IsValidIvrAccount(ByVal acctNumber As String) As Boolean
        Dim b As Boolean = False
        Try
            b = oEq.IsValidIvrAccount(get_Auth(), acctNumber)
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return b
    End Function

    Public Function LoanInformation(ByVal acctNumber As String) As Loan 'Equiant.Loan
        Dim l As Equiant.Loan
        Dim lA As Loan = Nothing
        Dim es As Equiant.ServiceSettings
        Dim s As ServiceSettings
        Dim f As Fee
        Dim eF As Equiant.Fee
        Try
            l = oEq.LoanInformation(get_Auth(), acctNumber)
            lA = New Loan
            lA.AccruedInterest = l.AccruedInterest
            lA.ActiveDisposition = l.ActiveDisposition
            lA.BringCurrent = l.BringCurrent
            lA.Client = l.Client
            lA.CPAT = l.CPAT
            lA.Currency = l.Currency
            lA.DaysDelinquent = l.DaysDelinquent
            lA.DispositionCode = l.DispositionCode
            lA.DispositionCodeDescription = l.DispositionCodeDescription
            lA.EquityBalance = l.EquityBalance
            lA.ImpoundAmount = l.ImpoundAmount
            lA.InterestBalance = l.InterestBalance
            lA.InterestCalcMethod = l.InterestCalcMethod
            lA.InterestCalcMethodDesc = l.InterestCalcMethodDesc
            lA.InterestRate = l.InterestRate
            lA.LastActiveYear = l.LastActiveYear
            lA.LastApplyAmount = l.LastApplyAmount
            lA.LastApplyCodeDesc = l.LastApplyCodeDesc
            lA.LastApplyDate = l.LastApplyDate
            lA.LateCharges = l.LateCharges
            lA.Lender = l.Lender
            lA.MaturityDate = l.MaturityDate
            lA.NextPaymentDueDate = l.NextPaymentDueDate
            lA.OriginalDownPayment = l.OriginalDownPayment
            lA.OriginalLoanAmount = l.OriginalLoanAmount
            lA.PartialPaymentAmount = l.PartialPaymentAmount
            lA.PaymentMethod = l.PaymentMethod
            lA.PayoffAmount = l.PayoffAmount
            lA.PayoffThroughDate = l.PayoffThroughDate
            lA.PerDiem = l.PerDiem
            lA.PrincipalBalance = l.PrincipalBalance
            lA.PriorActiveYear = l.PriorActiveYear
            lA.PriorYearToDateInterest = lA.PriorYearToDateInterest
            es = l.Settings
            s = New ServiceSettings
            s.AchTypes = es.AchTypes
            s.AllowACH = es.AllowACH
            s.AllowCheck = es.AllowCheck
            s.AllowCreditCard = es.AllowCreditCard
            s.AllowPayments = es.AllowPayments
            s.AllowPaymentsReason = es.AllowPaymentsReason
            s.AutoCCDaysOfMonth = es.AutoCCDaysOfMonth
            s.AutoCreditCard = es.AutoCreditCard
            s.AutoCreditCardOnAmount = es.AutoCreditCardOnAmount
            s.AutoPaidAhead = es.AutoPaidAhead
            s.CategoryCodes = es.CategoryCodes
            s.CreditCardAuthDays = es.CreditCardAuthDays
            s.CreditCardAuthsRequired = es.CreditCardAuthsRequired
            s.CreditCardBank = es.CreditCardBank
            s.CreditCardTypes = es.CreditCardTypes
            s.Currency = es.Currency
            s.ExtraPaymentType = es.ExtraPaymentType
            s.ExtraPaymentTypeDescription = es.ExtraPaymentTypeDescription
            s.ExtraPaymentTypeMaxAmount = es.ExtraPaymentTypeMaxAmount
            s.ExtraPaymentTypeMinAmount = es.ExtraPaymentTypeMinAmount
            s.MaxOTCCPayment = es.MaxOTCCPayment
            s.MaxOTSPpayment = es.MaxOTSPpayment
            s.MaxPRCRPayment = es.MaxPRCRPayment
            s.MaxRegularPayment = es.MaxRegularPayment
            s.merchantID = es.MerchantId
            s.MerchantMessage = es.MerchantMessage
            s.MinCreditCardPayment = es.MinCreditCardPayment
            s.MinSurePayPayment = es.MinSurePayPayment
            s.PostDateDays = es.PostDateDays
            s.ServiceFee = es.ServiceFee
            s.SurePay = es.SurePay
            s.SurePayDaysOfMonth = es.SurePayDaysOfMonth
            s.SurePayOnAmount = es.SurePayOnAmount
            eF = es.Fees
            f = New Fee
            f.Client = eF.Client
            f.Consumer = eF.Consumer
            f.IP = eF.IP
            f.IVR = eF.IVR
            f.OTCC = eF.OTCC
            f.OTSP = eF.OTSP
            s.Fees = f
            lA.Settings = s
            lA.Term = l.Term
            lA.RemainingTerm = l.RemainingTerm
            lA.UnappliedBalance = l.UnappliedBalance
            lA.Unit = l.Unit
            lA.Week = l.Week
            lA.YearToDateInterest = l.YearToDateInterest
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return lA
    End Function

    Public Function LoanSettings(ByVal acctNumber As String) As ServiceSettings
        Dim es As Equiant.ServiceSettings
        Dim s As ServiceSettings = Nothing
        Dim ef As Equiant.Fee
        Dim f As Fee
        Try
            es = oEq.LoanSettings(get_Auth(), acctNumber)
            s = New ServiceSettings
            s.AchTypes = es.AchTypes
            s.AllowACH = es.AllowACH
            s.AllowCheck = es.AllowCheck
            s.AllowCreditCard = es.AllowCreditCard
            s.AllowPayments = es.AllowPayments
            s.AllowPaymentsReason = es.AllowPaymentsReason
            s.AutoCCDaysOfMonth = es.AutoCCDaysOfMonth
            s.AutoCreditCard = es.AutoCreditCard
            s.AutoCreditCardOnAmount = es.AutoCreditCardOnAmount
            s.AutoPaidAhead = es.AutoPaidAhead
            s.CategoryCodes = es.CategoryCodes
            s.CreditCardAuthDays = es.CreditCardAuthDays
            s.CreditCardAuthsRequired = es.CreditCardAuthsRequired
            s.CreditCardBank = es.CreditCardBank
            s.CreditCardTypes = es.CreditCardTypes
            s.Currency = es.Currency
            s.ExtraPaymentType = es.ExtraPaymentType
            s.ExtraPaymentTypeDescription = es.ExtraPaymentTypeDescription
            s.ExtraPaymentTypeMaxAmount = es.ExtraPaymentTypeMaxAmount
            s.ExtraPaymentTypeMinAmount = es.ExtraPaymentTypeMinAmount
            s.MaxOTCCPayment = es.MaxOTCCPayment
            s.MaxOTSPpayment = es.MaxOTSPpayment
            s.MaxPRCRPayment = es.MaxPRCRPayment
            s.MaxRegularPayment = es.MaxRegularPayment
            s.merchantID = es.MerchantId
            s.MerchantMessage = es.MerchantMessage
            s.MinCreditCardPayment = es.MinCreditCardPayment
            s.MinSurePayPayment = es.MinSurePayPayment
            s.PostDateDays = es.PostDateDays
            s.ServiceFee = es.ServiceFee
            s.SurePay = es.SurePay
            s.SurePayDaysOfMonth = es.SurePayDaysOfMonth
            s.SurePayOnAmount = es.SurePayOnAmount
            ef = es.Fees
            f = New Fee
            f.Client = ef.Client
            f.Consumer = ef.Consumer
            f.IP = ef.IP
            f.IVR = ef.IVR
            f.OTCC = ef.OTCC
            f.OTSP = ef.OTSP
            s.Fees = f
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return s
    End Function

    Public Function LoanTransactionDetails(ByVal acctNumber As String, ByVal year As Integer) As List(Of LoanTransaction)
        Dim req As Equiant.LoanTransaction()
        Dim ret As List(Of LoanTransaction) = Nothing
        Try
            req = oEq.LoanTransactionDetails(get_Auth(), acctNumber, year)
            ret = New List(Of LoanTransaction)
            For Each l In req
                Dim lA As New LoanTransaction
                lA.Code = l.Code
                lA.ImpoundPaid = l.ImpoundPaid
                lA.LateChargeBalance = l.LateChargeBalance
                lA.NextDueDate = l.NextDueDate
                lA.NumberOfPayments = l.NumberOfPayments
                lA.OriginalAmount = l.OriginalAmount
                lA.OtherPaid = l.OtherPaid
                lA.PaidInterest = l.PaidInterest
                lA.PaidLateCharge = l.PaidLateCharge
                lA.PaidPrincipal = l.PaidPrincipal
                lA.PaymentID = l.PaymentID
                lA.PrincipalBalance = l.PrincipalBalance
                lA.Source = l.Source
                lA.SourceInformation = l.SourceInformation
                lA.TransDate = l.Date
                lA.Unapplied = l.Unapplied
                ret.Add(lA)
                lA = Nothing
            Next
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ret
    End Function

    Public Function LoanTransactionsYearsList(ByVal acctNumber As String) As String()
        Dim s As String() = Nothing
        Try
            s = oEq.LoanTransactionsYearsList(get_Auth(), acctNumber)
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return s
    End Function

    Public Function SearchByPhone(ByVal phone As String) As List(Of Object)
        Dim s As List(Of Object) = Nothing
        Try
            s = oEq.SearchByPhone(get_Auth(), phone).ToList
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return s
    End Function

    Public Function ProcessCreditCard(ByVal CC As CreditCard, ByVal recurring As Boolean) As Long
        Dim x As Long
        Try
            Dim eCC As New Equiant.CreditCard
            eCC.AccountNumber = CC.AccountNumber
            eCC.AccountType = CC.AccountType
            eCC.Address = CC.Address
            eCC.Amount = CC.Amount
            eCC.Authorization = CC.Authorization
            eCC.CanUnrestrict = CC.CanUnrestrict
            eCC.CardNumber = CC.CardNumber
            eCC.CardType = CC.CardType
            eCC.ConfirmationNumber = CC.ConfirmationNumber
            eCC.Currency = CC.Currency
            eCC.DisabledReason = CC.DisabledReason
            eCC.EntryDate = CC.EntryDate
            eCC.ExpirationMonth = CC.ExpirationMonth
            eCC.ExpirationYear = CC.ExpirationYear
            eCC.InternetCodeType = CC.InternetCodeType
            eCC.IsDebit = CC.isDebit
            eCC.LastDebitDate = CC.LastDebitDate
            eCC.LastDeclineDate = CC.LastDeclineDate
            eCC.LastDeclineReason = CC.LastDeclineReason
            eCC.Name = CC.Name
            eCC.OrderID = CC.OrderID
            eCC.PaymentDate = CC.PaymentDate
            If recurring Then
                eCC.PaymentID = "NEW"
                eCC.PaymentType = Equiant.PaymentTypes.CC
            Else
                eCC.PaymentID = CC.PaymentID
                eCC.PaymentType = CC.PaymentType
            End If
            eCC.PaymentStatus = CC.PaymentStatus
            eCC.PaymentTypeDescription = CC.PaymentTypeDescription
            eCC.PostalCode = CC.PostalCode
            eCC.RestrictReason = CC.RestrictReason
            eCC.ServiceType = CC.ServiceType
            eCC.SetupBy = CC.SetupBy
            eCC.SetupDate = CC.SetupDate
            eCC.TransactionReference = CC.TransactionReference
            eCC.User = CC.User
            If recurring Then
                x = oEq.ProcessAutoCreditCard(get_Auth(), eCC, True)
            Else
                x = oEq.ProcessCreditCard(get_Auth(), eCC, True)
            End If
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return x
    End Function

    Public Function ProcessCheck(ByVal c As Check, ByVal ach As Boolean) As Long
        Dim x As Long
        Dim ec As New Equiant.Check
        Try
            ec.AccountNumber = c.AccountNumber
            ec.AccountType = c.AccountType
            ec.ACHType = c.ACHType
            ec.Amount = c.Amount
            ec.BankAccountNumber = c.BankAccountNumber
            ec.CanUnrestrict = c.CanUnrestrict
            ec.CheckNumber = c.CheckNumber
            ec.CheckType = c.CheckType
            ec.ConfirmationNumber = c.ConfirmationNumber
            ec.Currency = c.Currency
            ec.DisabledReason = c.DisabledReason
            ec.EntryDate = c.EntryDate
            ec.InternetCodeType = c.InternetCodeType
            ec.isACH = c.isACH
            ec.LastDebitDate = c.LastDebitDate
            ec.LastDeclineDate = c.LastDeclineDate
            ec.LastDeclineReason = c.LastDeclineReason
            ec.Name = c.Name
            ec.OrderID = c.OrderID
            ec.PaymentDate = c.PaymentDate
            ec.PaymentID = c.PaymentID
            ec.PaymentStatus = c.PaymentStatus
            ec.PaymentType = c.PaymentType
            ec.PaymentTypeDescription = c.PaymentTypeDescription
            ec.RestrictReason = c.RestrictReason
            ec.RoutingNumber = c.RoutingNumber
            ec.ServiceType = c.ServiceType
            ec.SetupBy = c.SetupBy
            ec.SetupDate = c.SetupDate
            ec.User = c.User
            If ach Then
                x = oEq.ProcessACH(get_Auth(), ec, False)
            Else
                x = oEq.ProcessCheck(get_Auth(), ec, False)
            End If
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return x
    End Function

    Public Function Payment(ByVal acctNumber As String, ByVal PaymentID As Integer) As Object
        Dim p As Object = Nothing
        Try
            p = oEq.Payment(get_Auth(), acctNumber, PaymentID)
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return p
    End Function

    Public Function PendingPayments(ByVal acctNumber As String) As List(Of Object)
        Dim p As List(Of Object) = Nothing
        Try
            p = oEq.PendingPayments(get_Auth(), acctNumber).ToList
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return p
    End Function

    Public Function FindCreditCardType(ByVal ccNum As String) As CreditCardTypes
        Dim c As CreditCardTypes
        Try
            c = oEq.FindCreditCardType(get_Auth(), ccNum)
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return c
    End Function

    Public Function ValidateCreditCard(ByVal ccNum As String) As Boolean
        Dim b As Boolean = False
        Try
            b = oEq.ValidateCreditCard(get_Auth(), ccNum)
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return b
    End Function

    Public Function ValidateRoutingNumber(ByVal rNum As String) As Boolean
        Dim b As Boolean = False
        Try
            b = oEq.ValidateRoutingNumber(get_Auth(), rNum)
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return b
    End Function

    Public Function ManageAddress(ByVal acctNumber As String, ByVal address1 As String, ByVal address2 As String, ByVal City As String, ByVal State As String, ByVal Country As String, ByVal PostalCode As String) As String
        Dim b As Boolean = False
        Try
            b = oEq.ManageAddress(get_Auth(), acctNumber, address1, address2, "", City, State, Country, PostalCode)
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return b
    End Function

    Public Function InsertName(ByVal acctNumber As String, ByVal Sequence As Integer, ByVal first As String, ByVal last As String, ByVal middle As String) As Boolean
        Dim b As Boolean = False
        Try
            b = oEq.InsertName(get_Auth(), acctNumber, Sequence, first, middle, last)
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return b
    End Function

    Public Function RemoveName(ByVal acctNumber As String, ByVal Sequence As Integer, ByVal first As String, ByVal last As String, ByVal middle As String) As Boolean
        Dim b As Boolean = False
        Try
            b = oEq.RemoveName(get_Auth(), acctNumber, Sequence, first, middle, last)
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return b
    End Function

    Public Function UpdateName(ByVal acctNumber As String, ByVal Sequence As Integer, ByVal first As String, ByVal last As String, ByVal middle As String) As Boolean
        Dim b As Boolean
        Try
            b = oEq.UpdateName(get_Auth(), acctNumber, Sequence, first, middle, last)
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return b
    End Function

    Public Function NoteTypes() As String()
        Dim s As String() = Nothing
        Try
            s = oEq.NoteTypes(get_Auth())
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return s
    End Function

    Public Function NoteCount(ByVal acctNumber As String) As Integer
        Dim s As Integer
        Try
            s = oEq.NoteCount(get_Auth(), acctNumber)
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return s
    End Function

    Public Function Notes(ByVal acctNumber As String, ByVal position As String) As List(Of Note)
        Dim res As Equiant.Note()
        Dim ret As List(Of Note) = Nothing
        Try
            res = oEq.Notes(get_Auth(), acctNumber, position)
            ret = New List(Of Note)
            For Each es As Equiant.Note In res.ToList
                Dim s As New Note
                s.Code = es.Code
                s.Description = es.Description
                s.ID = es.ID
                s.Message = es.Message
                s.TransDate = es.Date
                s.UserName = es.UserName
                ret.Add(s)
                s = Nothing
            Next
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ret
    End Function

    Public Function InsertNote(ByVal acctNumber As String, ByVal note As String, ByVal type As String, ByVal user As String) As Boolean
        Dim b As Boolean
        Try
            b = oEq.InsertNote(get_Auth(), acctNumber, note, type, user)
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return b
    End Function

    Public Class PersonalInformation
        'Inherits Equiant.PersonalInformation
        Private _person As New List(Of Person)
        Public Property person() As List(Of Person)
            Get
                Return _person
            End Get
            Set(ByVal value As List(Of Person))
                _person = value
            End Set
        End Property
        Private _phone As New List(Of Communication)
        Public Property phone() As List(Of Communication)
            Get
                Return _phone
            End Get
            Set(ByVal value As List(Of Communication))
                _phone = value
            End Set
        End Property
        Private _email As New List(Of Communication)
        Public Property email() As List(Of Communication)
            Get
                Return _email
            End Get
            Set(ByVal value As List(Of Communication))
                _email = value
            End Set
        End Property
        Private _Address As New List(Of String)
        Public Property Address() As List(Of String)
            Get
                Return _Address
            End Get
            Set(ByVal value As List(Of String))
                _Address = value
            End Set
        End Property
        Private _city As String
        Public Property city() As String
            Get
                Return _city
            End Get
            Set(ByVal value As String)
                _city = value
            End Set
        End Property
        Private _state As String
        Public Property state() As String
            Get
                Return _state
            End Get
            Set(ByVal value As String)
                _state = value
            End Set
        End Property
        Private _Country As String
        Public Property Country() As String
            Get
                Return _Country
            End Get
            Set(ByVal value As String)
                _Country = value
            End Set
        End Property
        Private _PostalCode As String
        Public Property PostalCode() As String
            Get
                Return _PostalCode
            End Get
            Set(ByVal value As String)
                _PostalCode = value
            End Set
        End Property
        Private _GoodAddress As Boolean
        Public Property GoodAddress() As Boolean
            Get
                Return _GoodAddress
            End Get
            Set(ByVal value As Boolean)
                _GoodAddress = value
            End Set
        End Property
    End Class

    Public Class Person
        'Inherits Equiant.Person
        Private _FirstName As String
        Public Property FirstName() As String
            Get
                Return _FirstName
            End Get
            Set(ByVal value As String)
                _FirstName = value
            End Set
        End Property
        Private _LastName As String
        Public Property LastName() As String
            Get
                Return _LastName
            End Get
            Set(ByVal value As String)
                _LastName = value
            End Set
        End Property
        Private _MiddleName As String
        Public Property MiddleName() As String
            Get
                Return _MiddleName
            End Get
            Set(ByVal value As String)
                _MiddleName = value
            End Set
        End Property
        Private _Sequence As Integer
        Public Property Sequence() As Integer
            Get
                Return _Sequence
            End Get
            Set(ByVal value As Integer)
                _Sequence = value
            End Set
        End Property
    End Class

    Public Class Communication
        'Inherits Equiant.Communication
        Public Sub New()

        End Sub
        Private _Name As String
        Private _Type As String
        Private _Sequence As Integer
        Private _EmailStatements As Boolean
        Public Property Name As String
            Get
                Return _Name
            End Get
            Set(value As String)
                _Name = value
            End Set
        End Property
        Public Property Type As String
            Get
                Return _Type
            End Get
            Set(value As String)
                _Type = value
            End Set
        End Property
        Public Property Sequence As Integer
            Get
                Return _Sequence
            End Get
            Set(value As Integer)
                _Sequence = value
            End Set
        End Property
        Public Property EmailStatements As Boolean
            Get
                Return _EmailStatements
            End Get
            Set(value As Boolean)
                _EmailStatements = value
            End Set
        End Property
    End Class

    Public Class Billing
        'Inherits Equiant.Billing
        Private _ActiveDisposition As String
        Public Property ActiveDisposition() As String
            Get
                Return _ActiveDisposition
            End Get
            Set(ByVal value As String)
                _ActiveDisposition = value
            End Set
        End Property
        Private _Client As String
        Public Property Client() As String
            Get
                Return _Client
            End Get
            Set(ByVal value As String)
                _Client = value
            End Set
        End Property
        Private _Currency As String
        Public Property Currency() As String
            Get
                Return _Currency
            End Get
            Set(ByVal value As String)
                _Currency = value
            End Set
        End Property
        Private _CurrentBilledDueDate As DateTime
        Public Property CurrentBilledDueDate() As DateTime
            Get
                Return _CurrentBilledDueDate
            End Get
            Set(ByVal value As DateTime)
                _CurrentBilledDueDate = value
            End Set
        End Property
        Private _CurrentDue As Double
        Public Property CurrentDue() As Double
            Get
                Return _CurrentDue
            End Get
            Set(ByVal value As Double)
                _CurrentDue = value
            End Set
        End Property
        Private _DaysDelinquent As Double
        Public Property DaysDelinquent() As Double
            Get
                Return _DaysDelinquent
            End Get
            Set(ByVal value As Double)
                _DaysDelinquent = value
            End Set
        End Property
        Private _Description As String
        Public Property Description() As String
            Get
                Return _Description
            End Get
            Set(ByVal value As String)
                _Description = value
            End Set
        End Property
        Private _DispositionCode As String
        Public Property DispositionCode() As String
            Get
                Return _DispositionCode
            End Get
            Set(ByVal value As String)
                _DispositionCode = value
            End Set
        End Property
        Private _DispositionCodeDescription As String
        Public Property DispositionCodeDescription() As String
            Get
                Return _DispositionCodeDescription
            End Get
            Set(ByVal value As String)
                _DispositionCodeDescription = value
            End Set
        End Property
        Private _FinanceChargeDue As Double
        Public Property FinanceChargeDue() As Double
            Get
                Return _FinanceChargeDue
            End Get
            Set(ByVal value As Double)
                _FinanceChargeDue = value
            End Set
        End Property
        Private _FutureRateDate As DateTime
        Public Property FutureRateDate() As DateTime
            Get
                Return _FutureRateDate
            End Get
            Set(ByVal value As DateTime)
                _FutureRateDate = value
            End Set
        End Property
        Private _FinanceChargeRate As Double
        Public Property FinanceChargeRate() As Double
            Get
                Return _FinanceChargeRate
            End Get
            Set(ByVal value As Double)
                _FinanceChargeRate = value
            End Set
        End Property
        Private _FutureRateAmount As Double
        Public Property FutureRateAmount() As Double
            Get
                Return _FutureRateAmount
            End Get
            Set(ByVal value As Double)
                _FutureRateAmount = value
            End Set
        End Property
        Private _LastApplyAmount As Double
        Public Property LastApplyAmount() As Double
            Get
                Return _LastApplyAmount
            End Get
            Set(ByVal value As Double)
                _LastApplyAmount = value
            End Set
        End Property
        Private _LastApplyCodeDesc As String
        Public Property LastApplyCodeDesc() As String
            Get
                Return _LastApplyCodeDesc
            End Get
            Set(ByVal value As String)
                _LastApplyCodeDesc = value
            End Set
        End Property
        Private _LastApplyDate As DateTime
        Public Property LastApplyDate() As DateTime
            Get
                Return _LastApplyDate
            End Get
            Set(ByVal value As DateTime)
                _LastApplyDate = value
            End Set
        End Property
        Private _LateChargeCode As String
        Public Property LateChargeCode() As String
            Get
                Return _LateChargeCode
            End Get
            Set(ByVal value As String)
                _LateChargeCode = value
            End Set
        End Property
        Private _LateChargeDesc As String
        Public Property LateChargeDesc() As String
            Get
                Return _LateChargeDesc
            End Get
            Set(ByVal value As String)
                _LateChargeDesc = value
            End Set
        End Property
        Private _LateChargeDue As Double
        Public Property LateChargeDue() As Double
            Get
                Return _LateChargeDue
            End Get
            Set(ByVal value As Double)
                _LateChargeDue = value
            End Set
        End Property
        Private _NextBillDueDate As DateTime
        Public Property NextBillDueDate() As DateTime
            Get
                Return _NextBillDueDate
            End Get
            Set(ByVal value As DateTime)
                _NextBillDueDate = value
            End Set
        End Property
        Private _NextStatementDueDate As DateTime
        Public Property NextStatementDueDate() As DateTime
            Get
                Return _NextStatementDueDate
            End Get
            Set(ByVal value As DateTime)
                _NextStatementDueDate = value
            End Set
        End Property
        Private _PastDue As Double
        Public Property PastDue() As Double
            Get
                Return _PastDue
            End Get
            Set(ByVal value As Double)
                _PastDue = value
            End Set
        End Property
        Private _PaymentAmount As Double
        Public Property PaymentAmount() As Double
            Get
                Return _PaymentAmount
            End Get
            Set(ByVal value As Double)
                _PaymentAmount = value
            End Set
        End Property
        Private _PaymentFreq As String
        Public Property PaymentFreq() As String
            Get
                Return _PaymentFreq
            End Get
            Set(ByVal value As String)
                _PaymentFreq = value
            End Set
        End Property
        Private _PaymentFreqDesc As String
        Public Property PaymentFreqDesc() As String
            Get
                Return _PaymentFreqDesc
            End Get
            Set(ByVal value As String)
                _PaymentFreqDesc = value
            End Set
        End Property
        Private _PaymentMethod As String
        Public Property PaymentMethod() As String
            Get
                Return _PaymentMethod
            End Get
            Set(ByVal value As String)
                _PaymentMethod = value
            End Set
        End Property
        Private _PerDiem As Double
        Public Property PerDiem() As Double
            Get
                Return _PerDiem
            End Get
            Set(ByVal value As Double)
                _PerDiem = value
            End Set
        End Property
        Private _Settings As ServiceSettings
        Public Property Settings() As ServiceSettings
            Get
                Return _Settings
            End Get
            Set(ByVal value As ServiceSettings)
                _Settings = value
            End Set
        End Property
        Private _TotalDue As Double
        Public Property TotalDue() As Double
            Get
                Return _TotalDue
            End Get
            Set(ByVal value As Double)
                _TotalDue = value
            End Set
        End Property
        Private _Unit As String
        Public Property Unit() As String
            Get
                Return _Unit
            End Get
            Set(ByVal value As String)
                _Unit = value
            End Set
        End Property
        Private _Week As String
        Public Property Week() As String
            Get
                Return _Week
            End Get
            Set(ByVal value As String)
                _Week = value
            End Set
        End Property
    End Class

    Public Class LoanTransaction
        Private _TransDate As DateTime
        Private _Code As String
        Private _OriginalAmount As Double
        Private _PaidPrincipal As Double
        Private _PaidInterest As Double
        Private _PaidLateCharge As Double
        Private _ImpoundPaid As Double
        Private _OtherPaid As Double
        Private _Unapplied As Double
        Private _LateChargeBalance As Double
        Private _PrincipalBalance As Double
        Private _NumberOfPayments As Integer
        Private _NextDueDate As DateTime
        Private _Source As String
        Private _PaymentID As Long
        Private _SourceInformation As String

        Public Property SourceInformation As String
            Get
                Return _SourceInformation
            End Get
            Set(value As String)
                _SourceInformation = value
            End Set
        End Property

        Public Property PaymentID As Long
            Get
                Return _PaymentID
            End Get
            Set(value As Long)
                _PaymentID = value
            End Set
        End Property

        Public Property Source As String
            Get
                Return _Source
            End Get
            Set(value As String)
                _Source = value
            End Set
        End Property

        Public Property NextDueDate As DateTime
            Get
                Return _NextDueDate
            End Get
            Set(value As DateTime)
                _NextDueDate = value
            End Set
        End Property

        Public Property NumberOfPayments As Integer
            Get
                Return _NumberOfPayments
            End Get
            Set(value As Integer)
                _NumberOfPayments = value
            End Set
        End Property
        Public Property PrincipalBalance As Double
            Get
                Return _PrincipalBalance
            End Get
            Set(value As Double)
                _PrincipalBalance = value
            End Set
        End Property
        Public Property LateChargeBalance As Double
            Get
                Return _LateChargeBalance
            End Get
            Set(value As Double)
                _LateChargeBalance = value
            End Set
        End Property
        Public Property Unapplied As Double
            Get
                Return _Unapplied
            End Get
            Set(value As Double)
                _Unapplied = value
            End Set
        End Property
        Public Property OtherPaid As Double
            Get
                Return _OtherPaid
            End Get
            Set(value As Double)
                _OtherPaid = value
            End Set
        End Property
        Public Property ImpoundPaid As Double
            Get
                Return _ImpoundPaid
            End Get
            Set(value As Double)
                _ImpoundPaid = value
            End Set
        End Property
        Public Property PaidLateCharge As Double
            Get
                Return _PaidLateCharge
            End Get
            Set(value As Double)
                _PaidLateCharge = value
            End Set
        End Property
        Public Property PaidInterest As Double
            Get
                Return _PaidInterest
            End Get
            Set(value As Double)
                _PaidInterest = value
            End Set
        End Property

        Public Property PaidPrincipal As Double
            Get
                Return _PaidPrincipal
            End Get
            Set(value As Double)
                _PaidPrincipal = value
            End Set
        End Property

        Public Property TransDate As DateTime
            Get
                Return _TransDate
            End Get
            Set(value As DateTime)
                _TransDate = value
            End Set
        End Property
        Public Property Code As String
            Get
                Return _Code
            End Get
            Set(value As String)
                _Code = value
            End Set
        End Property
        Public Property OriginalAmount As Double
            Get
                Return _OriginalAmount
            End Get
            Set(value As Double)
                _OriginalAmount = value
            End Set
        End Property

    End Class

    Public Class Loan
        'Inherits Equiant.Loan
        Private _LastActiveYear As Integer
        Public Property LastActiveYear() As Integer
            Get
                Return _LastActiveYear
            End Get
            Set(ByVal value As Integer)
                _LastActiveYear = value
            End Set
        End Property
        Private _NextPaymentDueDate As DateTime
        Public Property NextPaymentDueDate() As DateTime
            Get
                Return _NextPaymentDueDate
            End Get
            Set(ByVal value As DateTime)
                _NextPaymentDueDate = value
            End Set
        End Property
        Private _BringCurrent As Double
        Public Property BringCurrent() As Double
            Get
                Return _BringCurrent
            End Get
            Set(ByVal value As Double)
                _BringCurrent = value
            End Set
        End Property
        Private _CPAT As Double
        Public Property CPAT() As Double
            Get
                Return _CPAT
            End Get
            Set(ByVal value As Double)
                _CPAT = value
            End Set
        End Property
        Private _AccruedInterest As Double
        Public Property AccruedInterest() As Double
            Get
                Return _AccruedInterest
            End Get
            Set(ByVal value As Double)
                _AccruedInterest = value
            End Set
        End Property
        Private _PerDiem As Double
        Public Property PerDiem() As Double
            Get
                Return _PerDiem
            End Get
            Set(ByVal value As Double)
                _PerDiem = value
            End Set
        End Property
        Private _LastApplyAmount As Double
        Public Property LastApplyAmount() As Double
            Get
                Return _LastApplyAmount
            End Get
            Set(ByVal value As Double)
                _LastApplyAmount = value
            End Set
        End Property
        Private _PriorActiveYear As Integer
        Public Property PriorActiveYear() As Integer
            Get
                Return _PriorActiveYear
            End Get
            Set(ByVal value As Integer)
                _PriorActiveYear = value
            End Set
        End Property
        Private _PrincipalBalance As Double
        Public Property PrincipalBalance() As Double
            Get
                Return _PrincipalBalance
            End Get
            Set(ByVal value As Double)
                _PrincipalBalance = value
            End Set
        End Property
        Private _LateCharges As Double
        Public Property LateCharges() As Double
            Get
                Return _LateCharges
            End Get
            Set(ByVal value As Double)
                _LateCharges = value
            End Set
        End Property
        Private _InterestCalcMethodDesc As String
        Public Property InterestCalcMethodDesc() As String
            Get
                Return _InterestCalcMethodDesc
            End Get
            Set(ByVal value As String)
                _InterestCalcMethodDesc = value
            End Set
        End Property
        Private _LastApplyDate As DateTime
        Public Property LastApplyDate() As DateTime
            Get
                Return _LastApplyDate
            End Get
            Set(ByVal value As DateTime)
                _LastApplyDate = value
            End Set
        End Property
        Private _LastApplyCodeDesc As String
        Public Property LastApplyCodeDesc() As String
            Get
                Return _LastApplyCodeDesc
            End Get
            Set(ByVal value As String)
                _LastApplyCodeDesc = value
            End Set
        End Property
        Private _YearToDateInterest As Double
        Public Property YearToDateInterest() As Double
            Get
                Return _YearToDateInterest
            End Get
            Set(ByVal value As Double)
                _YearToDateInterest = value
            End Set
        End Property
        Private _PriorYearToDateInterest As Double
        Public Property PriorYearToDateInterest() As Double
            Get
                Return _PriorYearToDateInterest
            End Get
            Set(ByVal value As Double)
                _PriorYearToDateInterest = value
            End Set
        End Property
        Private _InterestRate As Double
        Public Property InterestRate() As Double
            Get
                Return _InterestRate
            End Get
            Set(ByVal value As Double)
                _InterestRate = value
            End Set
        End Property
        Private _PayoffAmount As Double
        Public Property PayoffAmount() As Double
            Get
                Return _PayoffAmount
            End Get
            Set(ByVal value As Double)
                _PayoffAmount = value
            End Set
        End Property
        Private _PayoffThroughDate As DateTime
        Public Property PayoffThroughDate() As DateTime
            Get
                Return _PayoffThroughDate
            End Get
            Set(ByVal value As DateTime)
                _PayoffThroughDate = value
            End Set
        End Property
        Private _PartialPaymentAmount As Double
        Public Property PartialPaymentAmount() As Double
            Get
                Return _PartialPaymentAmount
            End Get
            Set(ByVal value As Double)
                _PartialPaymentAmount = value
            End Set
        End Property
        Private _ImpoundAmount As Double
        Public Property ImpoundAmount() As Double
            Get
                Return _ImpoundAmount
            End Get
            Set(ByVal value As Double)
                _ImpoundAmount = value
            End Set
        End Property
        Private _SalePrice As Double
        Public Property SalePrice() As Double
            Get
                Return _SalePrice
            End Get
            Set(ByVal value As Double)
                _SalePrice = value
            End Set
        End Property
        Private _OriginalDownPayment As Double
        Public Property OriginalDownPayment() As Double
            Get
                Return _OriginalDownPayment
            End Get
            Set(ByVal value As Double)
                _OriginalDownPayment = value
            End Set
        End Property
        Private _OriginalLoanAmount As Double
        Public Property OriginalLoanAmount() As Double
            Get
                Return _OriginalLoanAmount
            End Get
            Set(ByVal value As Double)
                _OriginalLoanAmount = value
            End Set
        End Property
        Private _EquityBalance As Double
        Public Property EquityBalance() As Double
            Get
                Return _EquityBalance
            End Get
            Set(ByVal value As Double)
                _EquityBalance = value
            End Set
        End Property
        Private _Term As Integer
        Public Property Term() As Integer
            Get
                Return _Term
            End Get
            Set(ByVal value As Integer)
                _Term = value
            End Set
        End Property
        Private _RemainingTerm As Integer
        Public Property RemainingTerm() As Integer
            Get
                Return _RemainingTerm
            End Get
            Set(ByVal value As Integer)
                _RemainingTerm = value
            End Set
        End Property
        Private _Lender As Integer
        Public Property Lender() As Integer
            Get
                Return _Lender
            End Get
            Set(ByVal value As Integer)
                _Lender = value
            End Set
        End Property
        Private _InterestCalcMethod As String
        Public Property InterestCalcMethod() As String
            Get
                Return _InterestCalcMethod
            End Get
            Set(ByVal value As String)
                _InterestCalcMethod = value
            End Set
        End Property
        Private _DaysDelinquent As Double
        Public Property DaysDelinquent() As Double
            Get
                Return _DaysDelinquent
            End Get
            Set(ByVal value As Double)
                _DaysDelinquent = value
            End Set
        End Property
        Private _InterestBalance As Double
        Public Property InterestBalance() As Double
            Get
                Return _InterestBalance
            End Get
            Set(ByVal value As Double)
                _InterestBalance = value
            End Set
        End Property
        Private _UnappliedBalance As Double
        Public Property UnappliedBalance() As Double
            Get
                Return _UnappliedBalance
            End Get
            Set(ByVal value As Double)
                _UnappliedBalance = value
            End Set
        End Property
        Private _MaturityDate As DateTime
        Public Property MaturityDate() As DateTime
            Get
                Return _MaturityDate
            End Get
            Set(ByVal value As DateTime)
                _MaturityDate = value
            End Set
        End Property
        Private _DispositionCode As String
        Public Property DispositionCode() As String
            Get
                Return _DispositionCode
            End Get
            Set(ByVal value As String)
                _DispositionCode = value
            End Set
        End Property
        Private _DispositionCodeDescription As String
        Public Property DispositionCodeDescription() As String
            Get
                Return _DispositionCodeDescription
            End Get
            Set(ByVal value As String)
                _DispositionCodeDescription = value
            End Set
        End Property
        Private _ActiveDisposition As String
        Public Property ActiveDisposition() As String
            Get
                Return _ActiveDisposition
            End Get
            Set(ByVal value As String)
                _ActiveDisposition = value
            End Set
        End Property
        Private _Client As String
        Public Property Client() As String
            Get
                Return _Client
            End Get
            Set(ByVal value As String)
                _Client = value
            End Set
        End Property
        Private _PaymentMethod As String
        Public Property PaymentMethod() As String
            Get
                Return _PaymentMethod
            End Get
            Set(ByVal value As String)
                _PaymentMethod = value
            End Set
        End Property
        Private _Unit As String
        Public Property Unit() As String
            Get
                Return _Unit
            End Get
            Set(ByVal value As String)
                _Unit = value
            End Set
        End Property
        Private _Week As String
        Public Property Week() As String
            Get
                Return _Week
            End Get
            Set(ByVal value As String)
                _Week = value
            End Set
        End Property
        Private _Currency As String
        Public Property Currency() As String
            Get
                Return _Currency
            End Get
            Set(ByVal value As String)
                _Currency = value
            End Set
        End Property
        Private _Settings As ServiceSettings
        Public Property Settings() As ServiceSettings
            Get
                Return _Settings
            End Get
            Set(ByVal value As ServiceSettings)
                _Settings = value
            End Set
        End Property
    End Class

    Public Class BillingTransaction
        'Inherits Equiant.BillingTransaction
        Private _TransactionID As String
        Public Property TransactionID() As String
            Get
                Return _TransactionID
            End Get
            Set(ByVal value As String)
                _TransactionID = value
            End Set
        End Property
        Private _SourceInformation As String
        Public Property SourceInformation() As String
            Get
                Return _SourceInformation
            End Get
            Set(ByVal value As String)
                _SourceInformation = value
            End Set
        End Property
        Private _PaymentID As Long
        Public Property PaymentID() As Long
            Get
                Return _PaymentID
            End Get
            Set(ByVal value As Long)
                _PaymentID = value
            End Set
        End Property
        Private _TransDate As DateTime
        Public Property TransDate() As DateTime
            Get
                Return _TransDate
            End Get
            Set(ByVal value As DateTime)
                _TransDate = value
            End Set
        End Property
        Private _Code As String
        Public Property Code() As String
            Get
                Return _Code
            End Get
            Set(ByVal value As String)
                _Code = value
            End Set
        End Property
        Private _DueDate As DateTime
        Public Property DueDate() As DateTime
            Get
                Return _DueDate
            End Get
            Set(ByVal value As DateTime)
                _DueDate = value
            End Set
        End Property
        Private _TransactionAmount As Double
        Public Property TransactionAmount() As Double
            Get
                Return _TransactionAmount
            End Get
            Set(ByVal value As Double)
                _TransactionAmount = value
            End Set
        End Property
        Private _TotalDueAmount As Double
        Public Property TotalDueAmount() As Double
            Get
                Return _TotalDueAmount
            End Get
            Set(ByVal value As Double)
                _TotalDueAmount = value
            End Set
        End Property
        Private _CurrentBilledAmount As Double
        Public Property CurrentBilledAmount() As Double
            Get
                Return _CurrentBilledAmount
            End Get
            Set(ByVal value As Double)
                _CurrentBilledAmount = value
            End Set
        End Property
        Private _PastDueAmount As Double
        Public Property PastDueAmount() As Double
            Get
                Return _PastDueAmount
            End Get
            Set(ByVal value As Double)
                _PastDueAmount = value
            End Set
        End Property
        Private _LateChargeAmount As Double
        Public Property LateChargeAmount() As Double
            Get
                Return _LateChargeAmount
            End Get
            Set(ByVal value As Double)
                _LateChargeAmount = value
            End Set
        End Property
        Private _FinanceChargeBalance As Double
        Public Property FinanceChargeBalance() As Double
            Get
                Return _FinanceChargeBalance
            End Get
            Set(ByVal value As Double)
                _FinanceChargeBalance = value
            End Set
        End Property
        Private _SourceCode As String
        Public Property SourceCode() As String
            Get
                Return _SourceCode
            End Get
            Set(ByVal value As String)
                _SourceCode = value
            End Set
        End Property
        Private _CategoryDescriptions As DataTable
        Public Property CategoryDescriptions() As DataTable
            Get
                Return _CategoryDescriptions
            End Get
            Set(ByVal value As DataTable)
                _CategoryDescriptions = value
            End Set
        End Property

    End Class

    Public Class ServiceSettings
        Private _Fees As Fee
        Public Property Fees() As Fee
            Get
                Return _Fees
            End Get
            Set(ByVal value As Fee)
                _Fees = value
            End Set
        End Property
        Private _CategoryCodes As String
        Public Property CategoryCodes() As String
            Get
                Return _CategoryCodes
            End Get
            Set(ByVal value As String)
                _CategoryCodes = value
            End Set
        End Property
        Private _Currency As String
        Public Property Currency() As String
            Get
                Return _Currency
            End Get
            Set(ByVal value As String)
                _Currency = value
            End Set
        End Property
        Private _AllowPaymentsReason As String
        Public Property AllowPaymentsReason() As String
            Get
                Return _AllowPaymentsReason
            End Get
            Set(ByVal value As String)
                _AllowPaymentsReason = value
            End Set
        End Property
        Private _MerchantMessage As String
        Public Property MerchantMessage() As String
            Get
                Return _MerchantMessage
            End Get
            Set(ByVal value As String)
                _MerchantMessage = value
            End Set
        End Property
        Private _AchTypes As String
        Public Property AchTypes() As String
            Get
                Return _AchTypes
            End Get
            Set(ByVal value As String)
                _AchTypes = value
            End Set
        End Property
        Private _CreditCardTypes As String
        Public Property CreditCardTypes() As String
            Get
                Return _CreditCardTypes
            End Get
            Set(ByVal value As String)
                _CreditCardTypes = value
            End Set
        End Property
        Private _AllowPayments As Boolean
        Public Property AllowPayments() As Boolean
            Get
                Return _AllowPayments
            End Get
            Set(ByVal value As Boolean)
                _AllowPayments = value
            End Set
        End Property
        Private _AllowACH As Boolean
        Public Property AllowACH() As Boolean
            Get
                Return _AllowACH
            End Get
            Set(ByVal value As Boolean)
                _AllowACH = value
            End Set
        End Property
        Private _AllowCheck As Boolean
        Public Property AllowCheck() As Boolean
            Get
                Return _AllowCheck
            End Get
            Set(ByVal value As Boolean)
                _AllowCheck = value
            End Set
        End Property
        Private _AllowCreditCard As Boolean
        Public Property AllowCreditCard() As Boolean
            Get
                Return _AllowCreditCard
            End Get
            Set(ByVal value As Boolean)
                _AllowCreditCard = value
            End Set
        End Property
        Private _CreditCardAuthsRequired As Boolean
        Public Property CreditCardAuthsRequired() As Boolean
            Get
                Return _CreditCardAuthsRequired
            End Get
            Set(ByVal value As Boolean)
                _CreditCardAuthsRequired = value
            End Set
        End Property
        Private _merchantID As String
        Public Property merchantID() As String
            Get
                Return _merchantID
            End Get
            Set(ByVal value As String)
                _merchantID = value
            End Set
        End Property
        Private _CreditCardAuthDays As Integer
        Public Property CreditCardAuthDays() As Integer
            Get
                Return _CreditCardAuthDays
            End Get
            Set(ByVal value As Integer)
                _CreditCardAuthDays = value
            End Set
        End Property
        Private _MaxRegularPayment As Double
        Public Property MaxRegularPayment() As Double
            Get
                Return _MaxRegularPayment
            End Get
            Set(ByVal value As Double)
                _MaxRegularPayment = value
            End Set
        End Property
        Private _MaxPRCRPayment As Double
        Public Property MaxPRCRPayment() As Double
            Get
                Return _MaxPRCRPayment
            End Get
            Set(ByVal value As Double)
                _MaxPRCRPayment = value
            End Set
        End Property
        Private _MaxOTCCPayment As Double
        Public Property MaxOTCCPayment() As Double
            Get
                Return _MaxOTCCPayment
            End Get
            Set(ByVal value As Double)
                _MaxOTCCPayment = value
            End Set
        End Property
        Private _MaxOTSPpayment As Double
        Public Property MaxOTSPpayment() As Double
            Get
                Return _MaxOTSPpayment
            End Get
            Set(ByVal value As Double)
                _MaxOTSPpayment = value
            End Set
        End Property
        Private _SurePay As Boolean
        Public Property SurePay() As Boolean
            Get
                Return _SurePay
            End Get
            Set(ByVal value As Boolean)
                _SurePay = value
            End Set
        End Property
        Private _ServiceFee As Decimal
        Public Property ServiceFee() As Decimal
            Get
                Return _ServiceFee
            End Get
            Set(ByVal value As Decimal)
                _ServiceFee = value
            End Set
        End Property
        Private _AutoCreditCard As Boolean
        Public Property AutoCreditCard() As Boolean
            Get
                Return _AutoCreditCard
            End Get
            Set(ByVal value As Boolean)
                _AutoCreditCard = value
            End Set
        End Property
        Private _SurePayOnAmount As Double
        Public Property SurePayOnAmount() As Double
            Get
                Return _SurePayOnAmount
            End Get
            Set(ByVal value As Double)
                _SurePayOnAmount = value
            End Set
        End Property
        Private _AutoCreditCardOnAmount As Double
        Public Property AutoCreditCardOnAmount() As Double
            Get
                Return _AutoCreditCardOnAmount
            End Get
            Set(ByVal value As Double)
                _AutoCreditCardOnAmount = value
            End Set
        End Property
        Private _MinSurePayPayment As Double
        Public Property MinSurePayPayment() As Double
            Get
                Return _MinSurePayPayment
            End Get
            Set(ByVal value As Double)
                _MinSurePayPayment = value
            End Set
        End Property
        Private _MinCreditCardPayment As Double
        Public Property MinCreditCardPayment() As Double
            Get
                Return _MinCreditCardPayment
            End Get
            Set(ByVal value As Double)
                _MinCreditCardPayment = value
            End Set
        End Property
        Private _AutoPaidAhead As Boolean
        Public Property AutoPaidAhead() As Boolean
            Get
                Return _AutoPaidAhead
            End Get
            Set(ByVal value As Boolean)
                _AutoPaidAhead = value
            End Set
        End Property
        Private _PostDateDays As Integer
        Public Property PostDateDays() As Integer
            Get
                Return _PostDateDays
            End Get
            Set(ByVal value As Integer)
                _PostDateDays = value
            End Set
        End Property
        Private _AutoCCDaysOfMonth As String
        Public Property AutoCCDaysOfMonth() As String
            Get
                Return _AutoCCDaysOfMonth
            End Get
            Set(ByVal value As String)
                _AutoCCDaysOfMonth = value
            End Set
        End Property
        Private _SurePayDaysOfMonth As String
        Public Property SurePayDaysOfMonth() As String
            Get
                Return _SurePayDaysOfMonth
            End Get
            Set(ByVal value As String)
                _SurePayDaysOfMonth = value
            End Set
        End Property
        Private _CreditCardBank As String
        Public Property CreditCardBank() As String
            Get
                Return _CreditCardBank
            End Get
            Set(ByVal value As String)
                _CreditCardBank = value
            End Set
        End Property
        Private _ExtraPaymentType As String()
        Public Property ExtraPaymentType() As String()
            Get
                Return _ExtraPaymentType
            End Get
            Set(ByVal value As String())
                _ExtraPaymentType = value
            End Set
        End Property
        Private _ExtraPaymentTypeDescription As String()
        Public Property ExtraPaymentTypeDescription() As String()
            Get
                Return _ExtraPaymentTypeDescription
            End Get
            Set(ByVal value As String())
                _ExtraPaymentTypeDescription = value
            End Set
        End Property
        Private _ExtraPaymentTypeMinAmount As Double()
        Public Property ExtraPaymentTypeMinAmount() As Double()
            Get
                Return _ExtraPaymentTypeMinAmount
            End Get
            Set(ByVal value As Double())
                _ExtraPaymentTypeMinAmount = value
            End Set
        End Property
        Private _ExtraPaymentTypeMaxAmount As Double()
        Public Property ExtraPaymentTypeMaxAmount As Double()
            Get
                Return _ExtraPaymentTypeMaxAmount
            End Get
            Set(ByVal value As Double())
                _ExtraPaymentTypeMaxAmount = value
            End Set
        End Property
    End Class

    Public Class Fee
        Private _Consumer As String
        Public Property Consumer() As String
            Get
                Return _Consumer
            End Get
            Set(ByVal value As String)
                _Consumer = value
            End Set
        End Property
        Private _IVR As Double
        Public Property IVR() As Double
            Get
                Return _IVR
            End Get
            Set(ByVal value As Double)
                _IVR = value
            End Set
        End Property
        Private _OTCC As Double
        Public Property OTCC() As Double
            Get
                Return _OTCC
            End Get
            Set(ByVal value As Double)
                _OTCC = value
            End Set
        End Property
        Private _OTSP As Double
        Public Property OTSP() As Double
            Get
                Return _OTSP
            End Get
            Set(ByVal value As Double)
                _OTSP = value
            End Set
        End Property
        Private _IP As Double
        Public Property IP() As Double
            Get
                Return _IP
            End Get
            Set(ByVal value As Double)
                _IP = value
            End Set
        End Property
        Private _Client As Double
        Public Property Client() As Double
            Get
                Return _Client
            End Get
            Set(ByVal value As Double)
                _Client = value
            End Set
        End Property
    End Class

    Public Class Info
        Private _AccountID As String
        Public Property AccountID() As String
            Get
                Return _AccountID
            End Get
            Set(ByVal value As String)
                _AccountID = value
            End Set
        End Property
        Private _LoanClientID As String
        Public Property LoanClientID() As String
            Get
                Return _LoanClientID
            End Get
            Set(ByVal value As String)
                _LoanClientID = value
            End Set
        End Property
        Private _BillClientID As String
        Public Property BillClientID() As String
            Get
                Return _BillClientID
            End Get
            Set(ByVal value As String)
                _BillClientID = value
            End Set
        End Property
        Private _Group As String
        Public Property Group() As String
            Get
                Return _Group
            End Get
            Set(ByVal value As String)
                _Group = value
            End Set
        End Property
        Private _Project As String
        Public Property Project() As String
            Get
                Return _Project
            End Get
            Set(ByVal value As String)
                _Project = value
            End Set
        End Property
        Private _LenderCode As String
        Public Property LenderCode() As String
            Get
                Return _LenderCode
            End Get
            Set(ByVal value As String)
                _LenderCode = value
            End Set
        End Property
        Private _LoanDisposition As String
        Public Property LoanDisposition() As String
            Get
                Return _LoanDisposition
            End Get
            Set(ByVal value As String)
                _LoanDisposition = value
            End Set
        End Property
        Private _BillDisposition As String
        Public Property BillDisposition() As String
            Get
                Return _BillDisposition
            End Get
            Set(ByVal value As String)
                _BillDisposition = value
            End Set
        End Property
        Private _ClassCodes As String
        Public Property ClassCodes() As String
            Get
                Return _ClassCodes
            End Get
            Set(ByVal value As String)
                _ClassCodes = value
            End Set
        End Property
        Private _DocClientID As String
        Public Property DocClientID() As String
            Get
                Return _DocClientID
            End Get
            Set(ByVal value As String)
                _DocClientID = value
            End Set
        End Property
        Private _DocLenderCode As String
        Public Property DocLenderCode() As String
            Get
                Return _DocLenderCode
            End Get
            Set(ByVal value As String)
                _DocLenderCode = value
            End Set
        End Property
        Private _DocDisposition As String
        Public Property DocDisposition() As String
            Get
                Return _DocDisposition
            End Get
            Set(ByVal value As String)
                _DocDisposition = value
            End Set
        End Property
    End Class

    Public Class Common
        Private _PaymentTypeDescription As String
        Public Property PaymentTypeDescription() As String
            Get
                Return _PaymentTypeDescription
            End Get
            Set(ByVal value As String)
                _PaymentTypeDescription = value
            End Set
        End Property
        Private _PaymentType As PaymentTypes
        Public Property PaymentType() As PaymentTypes
            Get
                Return _PaymentType
            End Get
            Set(ByVal value As PaymentTypes)
                _PaymentType = value
            End Set
        End Property
        Private _PaymentStatus As PaymentStatus
        Public Property PaymentStatus() As PaymentStatus
            Get
                Return _PaymentStatus
            End Get
            Set(ByVal value As PaymentStatus)
                _PaymentStatus = value
            End Set
        End Property
        Private _User As String
        Public Property User() As String
            Get
                Return _User
            End Get
            Set(ByVal value As String)
                _User = value
            End Set
        End Property
        Private _AccountNumber As String
        Public Property AccountNumber() As String
            Get
                Return _AccountNumber
            End Get
            Set(ByVal value As String)
                _AccountNumber = value
            End Set
        End Property
        Private _OrderID As String
        Public Property OrderID() As String
            Get
                Return _OrderID
            End Get
            Set(ByVal value As String)
                _OrderID = value
            End Set
        End Property
        Private _PaymentID As String
        Public Property PaymentID() As String
            Get
                Return _PaymentID
            End Get
            Set(ByVal value As String)
                _PaymentID = value
            End Set
        End Property
        Private _Amount As Double
        Public Property Amount() As Double
            Get
                Return _Amount
            End Get
            Set(ByVal value As Double)
                _Amount = value
            End Set
        End Property
        Private _AccountType As AccountTypes
        Public Property AccountType() As AccountTypes
            Get
                Return _AccountType
            End Get
            Set(ByVal value As AccountTypes)
                _AccountType = value
            End Set
        End Property
        Private _ServiceType As String
        Public Property ServiceType() As String
            Get
                Return _ServiceType
            End Get
            Set(ByVal value As String)
                _ServiceType = value
            End Set
        End Property
        Private _Currency As Currency
        Public Property Currency() As Currency
            Get
                Return _Currency
            End Get
            Set(ByVal value As Currency)
                _Currency = value
            End Set
        End Property
        Private _PaymentDate As DateTime
        Public Property PaymentDate() As DateTime
            Get
                Return _PaymentDate
            End Get
            Set(ByVal value As DateTime)
                _PaymentDate = value
            End Set
        End Property
        Private _EntryDate As DateTime
        Public Property EntryDate() As DateTime
            Get
                Return _EntryDate
            End Get
            Set(ByVal value As DateTime)
                _EntryDate = value
            End Set
        End Property
        Private _Name As String
        Public Property Name() As String
            Get
                Return _Name
            End Get
            Set(ByVal value As String)
                _Name = value
            End Set
        End Property
        Private _InternetCodeType As InternetCodes
        Public Property InternetCodeType() As InternetCodes
            Get
                Return _InternetCodeType
            End Get
            Set(ByVal value As InternetCodes)
                _InternetCodeType = value
            End Set
        End Property
        Private _SetupDate As DateTime
        Public Property SetupDate() As DateTime
            Get
                Return _SetupDate
            End Get
            Set(ByVal value As DateTime)
                _SetupDate = value
            End Set
        End Property
        Private _SetupBy As String
        Public Property SetupBy() As String
            Get
                Return _SetupBy
            End Get
            Set(ByVal value As String)
                _SetupBy = value
            End Set
        End Property
        Private _LastDebitDate As DateTime
        Public Property LastDebitDate() As DateTime
            Get
                Return _LastDebitDate
            End Get
            Set(ByVal value As DateTime)
                _LastDebitDate = value
            End Set
        End Property
        Private _LastDeclineDate As DateTime
        Public Property LastDeclineDate() As DateTime
            Get
                Return _LastDeclineDate
            End Get
            Set(ByVal value As DateTime)
                _LastDeclineDate = value
            End Set
        End Property
        Private _LastDeclineReason As String
        Public Property LastDeclineReason() As String
            Get
                Return _LastDeclineReason
            End Get
            Set(ByVal value As String)
                _LastDeclineReason = value
            End Set
        End Property
        Private _DisabledReason As String
        Public Property DisabledReason() As String
            Get
                Return _DisabledReason
            End Get
            Set(ByVal value As String)
                _DisabledReason = value
            End Set
        End Property
        Private _RestrictReason As String
        Public Property RestrictReason() As String
            Get
                Return _RestrictReason
            End Get
            Set(ByVal value As String)
                _RestrictReason = value
            End Set
        End Property
        Private _CanUnrestrict As Boolean
        Public Property CanUnrestrict() As Boolean
            Get
                Return _CanUnrestrict
            End Get
            Set(ByVal value As Boolean)
                _CanUnrestrict = value
            End Set
        End Property
    End Class

    Public Class CreditCard
        Inherits Common
        Private _isDebit As Boolean
        Public Property isDebit() As Boolean
            Get
                Return _isDebit
            End Get
            Set(ByVal value As Boolean)
                _isDebit = value
            End Set
        End Property
        Private _Authorization As String
        Public Property Authorization() As String
            Get
                Return _Authorization
            End Get
            Set(ByVal value As String)
                _Authorization = value
            End Set
        End Property
        Private _ConfirmationNumber As String
        Public Property ConfirmationNumber() As String
            Get
                Return _ConfirmationNumber
            End Get
            Set(ByVal value As String)
                _ConfirmationNumber = value
            End Set
        End Property
        Private _CardNumber As String
        Public Property CardNumber() As String
            Get
                Return _CardNumber
            End Get
            Set(ByVal value As String)
                _CardNumber = value
            End Set
        End Property
        Private _Address As String
        Public Property Address() As String
            Get
                Return _Address
            End Get
            Set(ByVal value As String)
                _Address = value
            End Set
        End Property
        Private _PostalCode As String
        Public Property PostalCode() As String
            Get
                Return _PostalCode
            End Get
            Set(ByVal value As String)
                _PostalCode = value
            End Set
        End Property
        Private _ExpirationMonth As Integer
        Public Property ExpirationMonth() As Integer
            Get
                Return _ExpirationMonth
            End Get
            Set(ByVal value As Integer)
                _ExpirationMonth = value
            End Set
        End Property
        Private _ExpirationYear As Integer
        Public Property ExpirationYear() As Integer
            Get
                Return _ExpirationYear
            End Get
            Set(ByVal value As Integer)
                _ExpirationYear = value
            End Set
        End Property
        Private _CardType As CreditCardTypes
        Public Property CardType() As CreditCardTypes
            Get
                Return _CardType
            End Get
            Set(ByVal value As CreditCardTypes)
                _CardType = value
            End Set
        End Property
        Private _TransactionReference As String
        Public Property TransactionReference() As String
            Get
                Return _TransactionReference
            End Get
            Set(ByVal value As String)
                _TransactionReference = value
            End Set
        End Property
    End Class

    Public Class Check
        Inherits Common
        Private _BankAccountNumber As String
        Public Property BankAccountNumber() As String
            Get
                Return _BankAccountNumber
            End Get
            Set(ByVal value As String)
                _BankAccountNumber = value
            End Set
        End Property
        Private _RoutingNumber As String
        Public Property RoutingNumber() As String
            Get
                Return _RoutingNumber
            End Get
            Set(ByVal value As String)
                _RoutingNumber = value
            End Set
        End Property
        Private _ConfirmationNumber As String
        Public Property ConfirmationNumber() As String
            Get
                Return _ConfirmationNumber
            End Get
            Set(ByVal value As String)
                _ConfirmationNumber = value
            End Set
        End Property
        Private _CheckNumber As Integer
        Public Property CheckNumber() As Integer
            Get
                Return _CheckNumber
            End Get
            Set(ByVal value As Integer)
                _CheckNumber = value
            End Set
        End Property
        Private _isACH As Boolean
        Public Property isACH() As Boolean
            Get
                Return _isACH
            End Get
            Set(ByVal value As Boolean)
                _isACH = value
            End Set
        End Property
        Private _ACHType As ACHTypes
        Public Property ACHType() As ACHTypes
            Get
                Return _ACHType
            End Get
            Set(ByVal value As ACHTypes)
                _ACHType = value
            End Set
        End Property
        Private _CheckType As CheckTypes
        Public Property CheckType() As CheckTypes
            Get
                Return _CheckType
            End Get
            Set(ByVal value As CheckTypes)
                _CheckType = value
            End Set
        End Property
    End Class

    Public Class Note
        Private _ID As Integer
        Public Property ID() As Integer
            Get
                Return _ID
            End Get
            Set(ByVal value As Integer)
                _ID = value
            End Set
        End Property
        Private _TransDate As DateTime
        Public Property TransDate() As DateTime
            Get
                Return _TransDate
            End Get
            Set(ByVal value As DateTime)
                _TransDate = value
            End Set
        End Property
        Private _Code As String
        Public Property Code() As String
            Get
                Return _Code
            End Get
            Set(ByVal value As String)
                _Code = value
            End Set
        End Property
        Private _Description As String
        Public Property Description() As String
            Get
                Return _Description
            End Get
            Set(ByVal value As String)
                _Description = value
            End Set
        End Property
        Private _UserName As String
        Public Property UserName() As String
            Get
                Return _UserName
            End Get
            Set(ByVal value As String)
                _UserName = value
            End Set
        End Property
        Private _Message As String
        Public Property Message() As String
            Get
                Return _Message
            End Get
            Set(ByVal value As String)
                _Message = value
            End Set
        End Property
    End Class

    Public Class Statement
        Private _DataRecord As String
        Public Property DataRecordID() As String
            Get
                Return _DataRecord
            End Get
            Set(ByVal value As String)
                _DataRecord = value
            End Set
        End Property
        Private _ClientID As Integer
        Public Property ClientID() As Integer
            Get
                Return _ClientID
            End Get
            Set(ByVal value As Integer)
                _ClientID = value
            End Set
        End Property
        Private _CancelBy As String
        Public Property CancelBy() As String
            Get
                Return _CancelBy
            End Get
            Set(ByVal value As String)
                _CancelBy = value
            End Set
        End Property
        Private _CancelDate As Date
        Public Property CancelDate() As Date
            Get
                Return _CancelDate
            End Get
            Set(ByVal value As Date)
                _CancelDate = value
            End Set
        End Property
        Private _Date As Date
        Public Property Dates() As Date
            Get
                Return _Date
            End Get
            Set(ByVal value As Date)
                _Date = value
            End Set
        End Property
        Private _Description As String
        Public Property Description() As String
            Get
                Return _Description
            End Get
            Set(ByVal value As String)
                _Description = value
            End Set
        End Property
        Private _ErrorCode As String
        Public Property ErrorCode() As String
            Get
                Return _ErrorCode
            End Get
            Set(ByVal value As String)
                _ErrorCode = value
            End Set
        End Property
        Private _ErrorDescription As String
        Public Property ErrorDescription() As String
            Get
                Return _ErrorDescription
            End Get
            Set(ByVal value As String)
                _ErrorDescription = value
            End Set
        End Property
        Private _Status As String
        Public Property Status() As String
            Get
                Return _Status
            End Get
            Set(ByVal value As String)
                _Status = value
            End Set
        End Property
        Private _Type As String
        Public Property Type() As String
            Get
                Return _Type
            End Get
            Set(ByVal value As String)
                _Type = value
            End Set
        End Property
        Private _User As String
        Public Property User() As String
            Get
                Return _User
            End Get
            Set(ByVal value As String)
                _User = value
            End Set
        End Property
        Private _VSN As String
        Public Property VSN() As String
            Get
                Return _VSN
            End Get
            Set(ByVal value As String)
                _VSN = value
            End Set
        End Property
    End Class

    Public Enum CreditCardTypes
        Visa = 0
        MasterCard = 1
        AmericanExpress = 2
        Discover = 3
        JCB = 4
        Invalid = 5
    End Enum

    Public Enum PaymentTypes
        'SP = 0
        SP = Equiant.PaymentTypes.SP
        OTSP = Equiant.PaymentTypes.OTSP ' 1
        IP = Equiant.PaymentTypes.IP '2
        PP = Equiant.PaymentTypes.PP ' 3
        CC = Equiant.PaymentTypes.CC ' 4
        OTCC = Equiant.PaymentTypes.OTCC ' 5
        PTP = Equiant.PaymentTypes.PTP '6
    End Enum

    Public Enum PaymentStatus
        A = 0
        H = 1
        X = 2
        C = 3
        D = 4
        I = 5
        M = 6
        R = 7
        S = 8
    End Enum

    Public Enum AccountTypes
        LOAN = 0
        PBS = 1
    End Enum

    Public Enum Currency
        USD = 0
        CDN = 1
    End Enum

    Public Enum InternetCodes
        CNS = 0
        CLI = 1
        IVR = 2
        OTHER = 3
        NCL = 4
    End Enum

    Public Enum ACHTypes
        WEB = 0
        TELS = 1
        TELR = 2
        PPD = 3
        NA = 4
    End Enum

    Public Enum CheckTypes
        C = 0
        S = 1
    End Enum

    Public Property Err() As String
        Get
            Return _Err
        End Get
        Set(ByVal value As String)
            _Err = value
        End Set
    End Property


    Public Function Get_Account(ByVal ContractID As Integer, Conversion As Boolean) As String
        Dim cn As New System.Data.SqlClient.SqlConnection(Resources.Resource.cns)
        Dim cm As New System.Data.SqlClient.SqlCommand("Select * from v_ContractInventory where contractid = " & ContractID, cn)
        Dim dr As Data.SqlClient.SqlDataReader
        Dim _Account As String = ""
        Try
            cn.Open()
            dr = cm.ExecuteReader
            If Not dr.HasRows Then
                dr.Close()
                cm.CommandText = "Select * from v_ContractInventoryHistory where contractid = " & ContractID
                dr = cm.ExecuteReader
            End If
            If dr.HasRows Then
                dr.Read()
                Select Case dr("SaleType")
                    Case "Estates"
                        _Account = "131" & If(Conversion, "000", "100") & ContractID.ToString
                    Case "Townes"
                        _Account = "133" & If(Conversion, "000", "100") & ContractID.ToString
                    Case "Cottage"
                        _Account = "132" & If(Conversion, "000", "100") & ContractID.ToString
                    Case "Combo"
                        _Account = "132" & If(Conversion, "000", "100") & ContractID.ToString
                    Case Else
                        _Account = ""
                End Select
            End If
        Catch ex As Exception
        Finally
            If cn.State <> Data.ConnectionState.Closed Then cn.Close()
            dr = Nothing
            cm = Nothing
            cn = Nothing
        End Try
        Return _Account
    End Function

End Class
