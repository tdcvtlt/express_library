Imports Microsoft.VisualBasic
Imports System.Data
Partial Class marketing_confirmResCO
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oRes As New clsReservations
            Dim oPros As New clsProspect
            oRes.ReservationID = Request("ReservationID")
            oRes.Load()
            oPros.Prospect_ID = oRes.ProspectID
            oPros.Load()
            If Date.Compare(oRes.CheckOutDate, System.DateTime.Now) > 0 Then
                COlbl.Text = "This Reservation is Not Scheduled to Check Out Until " & oRes.CheckOutDate & ". <br /><br />Do you wish to Check Out ReservationID " & oRes.ReservationID & " - " & oPros.First_Name & " " & oPros.Last_Name & "?"
            Else
                COlbl.Text = "Do you wish to Check Out ReservationID " & oRes.ReservationID & " - " & oPros.First_Name & " " & oPros.Last_Name & "?"
            End If
            oPros = Nothing
            oRes = Nothing
            MultiView1.ActiveViewIndex = 0
        End If
    End Sub

    Protected Sub Unnamed1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If CheckSecurity("Reservations", "CheckOut", , , Session("UserDBID")) Then
            Dim oRes As New clsReservations
            Dim oPros As New clsProspect
            Dim oCombo As New clsComboItems
            Dim prosName As String = ""
            Dim taxes As Boolean = True
            Dim oCCMA As New clsCCMerchantAccount
            oRes.ReservationID = Request("ReservationID")
            oRes.UserID = Session("UserDBID")
            oRes.Load()
            oPros.Prospect_ID = oRes.ProspectID
            oPros.Load()
            prosName = Left(oPros.First_Name, 10) & " " & Left(oPros.Last_Name, 10)
            oPros = Nothing
            If Date.Compare(oRes.CheckOutDate, System.DateTime.Now) > 0 Then
                oRes.Check_Out(Request("ReservationID"), prosName, CDate(System.DateTime.Now).AddDays(-1).ToShortDateString, True)
                oRes.CheckOutDate = System.DateTime.Now.ToShortDateString
            Else
                oRes.Check_Out(Request("ReservationID"), prosName, CDate(oRes.CheckOutDate).AddDays(-1), False)
            End If
            oRes.StatusID = oCombo.Lookup_ID("ReservationStatus", "Completed")
            taxes = oRes.Check_Taxes(oRes.ReservationID)
            oRes.Save()
            Dim resBalance As Double = oRes.Get_Total_Balance_ByAcct(Request("ReservationID"), oCCMA.Lookup_By_AcctName("~0013~"))
            oRes = Nothing
            Dim oInv As New clsInvoices
            Dim oCCT As New clsCCTrans
            oInv.KeyField = "ReservationID"
            oInv.KeyValue = Request("ReservationID")
            oInv.ProspectID = 0
            oInv.Load()
            'Find PreAuth
            Dim ccTID = oCCT.Find_PA_By_ResID(Request("ReservationID"), 75, oCCMA.Lookup_By_AcctName("~0013~"))
            If taxes Then
                If resBalance > 0 Then
                    If ccTID > 0 Then
                        COLbl2.Text = "Taxes Have Been Assessed. <br><br> The Reservation Has An Open Balance of " & FormatCurrency(resBalance, 2) & ". <br><br> Would You Like to Use the Card On File to Pay the Balance?"
                        MultiView1.ActiveViewIndex = 1
                    Else
                        ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Taxes Have Been Assessed.');window.close();", True)
                    End If
                Else
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "window.opener.navigate('" & Request.ApplicationPath & "/marketing/editReservation.aspx?ReservationID=" & Request("ReservationID") & "');window.close();", True)
                End If
            Else
                If resBalance > 0 Then
                    If ccTID > 0 Then
                        COLbl2.Text = "The Reservation Has An Open Balance of " & FormatCurrency(resBalance, 2) & ". <br><br> Would You Like to Use the Card On File to Pay the Balance?"
                        MultiView1.ActiveViewIndex = 1
                    Else
                        ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Reservation Has A Balance. Please View Financials.');window.opener.Refresh_Financials();window.close();", True)
                    End If
                Else
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "window.opener.navigate('" & Request.ApplicationPath & "/marketing/editReservation.aspx?ReservationID=" & Request("ReservationID") & "');window.close();", True)
                End If
            End If
            oInv = Nothing
            oCCT = Nothing
            oRes = Nothing
            oCombo = Nothing
            oPros = Nothing
            oCCMA = Nothing
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('You Do Not Have Persmission to Check Out Reservations.');", True)
        End If
    End Sub

    Protected Sub Unnamed2_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "window.close();", True)
    End Sub
    Protected Sub CCNoBtn_Click(sender As Object, e As EventArgs) Handles CCNoBtn.Click
        ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "window.opener.navigate('" & Request.ApplicationPath & "/marketing/editReservation.aspx?ReservationID=" & Request("ReservationID") & "');window.close();", True)
    End Sub
    Protected Sub CCYesBtn_Click(sender As Object, e As EventArgs) Handles CCYesBtn.Click
        Dim oRes As New clsReservations
        Dim oInv As New clsInvoices
        Dim oCCMA As New clsCCMerchantAccount
        Dim oCCT As New clsCCTrans
        Dim oCCTPA As New clsCCTrans
        Dim oCombo As New clsComboItems
        Dim resBalance As Double = oRes.Get_Total_Balance_ByAcct(Request("ReservationID"), oCCMA.Lookup_By_AcctName("~0013~"))
        oInv.KeyField = "ReservationID"
        oInv.KeyValue = Request("ReservationID")
        oInv.ProspectID = 0
        oInv.Load()
        'Find PreAuth
        Dim ccTID = oCCT.Find_PA_By_ResID(Request("ReservationID"), 75, oCCMA.Lookup_By_AcctName("~0013~"))

        If resBalance > 75 Then
            oCCTPA.CCTransID = ccTID
            oCCTPA.Load()
            oCCT.CCTransID = 0
            oCCT.Load()
            oCCT.AccountID = oCCTPA.AccountID
            oCCT.Amount = resBalance
            oCCT.DateCreated = System.DateTime.Now
            oCCT.AccountID = oCCTPA.AccountID
            oCCT.TransTypeID = oCombo.Lookup_ID("CCTransType", "PreAuth")
            oCCT.CreditCardID = oCCTPA.CreditCardID
            oCCT.ClientIP = Request.ServerVariables("REMOTE_HOST")
            oCCT.CreatedByID = Session("UserDBID")
            oCCT.PreAuthID = 0
            oCCT.Token = oCCTPA.Token
            oCCT.Approved = 1
            oCCT.ApprovedBy = "AutoApprove"
            oCCT.DateApproved = System.DateTime.Now
            oCCT.Imported = False
            oCCT.Save()
            hfCCTransID.Value = oCCT.CCTransID
            Timer1.Interval = 1000
            Timer1.Enabled = True
            MultiView1.ActiveViewIndex = 2
            '            Dim dt As DataTable = oInv.List_Invoices_With_Balance_ByAcct(oCCMA.Lookup_By_AcctName("~0013~"))

        Else
            If ccTID > 0 Then
                'Make Sure PreAuth hasnt been used
                If oCCT.Check_PA(ccTID) Then
                    'Create CCTrans Record
                    oCCTPA.CCTransID = ccTID
                    oCCTPA.Load()
                    oCCT.CCTransID = 0
                    oCCT.Load()
                    oCCT.AccountID = oCCTPA.AccountID
                    oCCT.Amount = resBalance
                    oCCT.DateCreated = System.DateTime.Now
                    oCCT.AccountID = oCCTPA.AccountID
                    oCCT.TransTypeID = oCombo.Lookup_ID("CCTransType", "Force")
                    oCCT.CreditCardID = oCCTPA.CreditCardID
                    oCCT.ClientIP = Request.ServerVariables("REMOTE_HOST")
                    oCCT.CreatedByID = Session("UserDBID")
                    oCCT.PreAuthID = ccTID
                    oCCT.Token = "Token On PreAuth"
                    oCCT.Approved = 0
                    oCCT.Save()
                    hfCCTransID.Value = oCCT.CCTransID
                    'Create Payments and Apply To Invoice
                    Dim dt As DataTable = oInv.List_Invoices_With_Balance_ByAcct(oCCMA.Lookup_By_AcctName("~0013~"))
                    Dim oPmt As New clsPayments
                    Dim opmt2Inv As New clsPayment2Invoice
                    Dim oCC As New clsCreditCard
                    Dim oCCTApply As New clsCCTransApplyTo
                    oCC.CreditCardID = oCCT.CreditCardID
                    oCC.Load()
                    For i = 0 To dt.Rows.Count - 1
                        oPmt.PaymentID = 0
                        oPmt.Load()
                        oPmt.MethodID = oCombo.Lookup_ID("PaymentMethod", oCombo.Lookup_ComboItem(oCC.TypeID))
                        oPmt.Amount = dt.Rows(i).Item("Balance")
                        oPmt.ApplyToID = 0
                        oPmt.Adjustment = 0
                        oPmt.PosNeg = True
                        oPmt.UserID = Session("UserDBID")
                        oPmt.TransDate = System.DateTime.Now
                        oPmt.Save()
                        opmt2Inv.Inv2PayID = 0
                        opmt2Inv.Load()
                        opmt2Inv.PaymentID = oPmt.PaymentID
                        opmt2Inv.InvoiceID = dt.Rows(i).Item("ID")
                        opmt2Inv.Amount = oPmt.Amount
                        opmt2Inv.PosNeg = True
                        opmt2Inv.Save()
                        oCCTApply.CCTransApplyToID = 0
                        oCCTApply.Load()
                        oCCTApply.CCTransID = oCCT.CCTransID
                        oCCTApply.PaymentID = oPmt.PaymentID
                        oCCTApply.Amount = oPmt.Amount
                        oCCTApply.Save()
                    Next
                    'Approve CCTrans and fire timer
                    oCCT.Approved = 1
                    oCCT.ApprovedBy = "AutoApprove"
                    oCCT.DateApproved = System.DateTime.Now
                    oCCT.Save()
                    'Fire Timer
                    Timer1.Interval = 1000
                    Timer1.Enabled = True
                    MultiView1.ActiveViewIndex = 2
                    oCCTApply = Nothing
                    oPmt = Nothing
                    opmt2Inv = Nothing
                    oCC = Nothing
                Else
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Credit Card Has Been Charged. Please Return to Financials to Verify.');window.opener.Refresh_Financials();window.close();", True)
                End If
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Unable to Find Card On File. Please Return to Financials to Process Payment.');window.opener.Refresh_Financials();window.close();", True)
            End If
        End If
        oCCT = Nothing
        oCCTPA = Nothing
        oRes = Nothing
        oInv = Nothing
        oCCMA = Nothing
        oCombo = Nothing
    End Sub

    Protected Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        'Timer1.Enabled = False
        Timer1.Interval = 2000
        Dim resp As String = ""
        resp = Check_Status()
        'ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('" & hfCCTransID.Value & ": " & resp & "');", True)

        If Left(resp, 1) <> "" Then
            Timer1.Enabled = False
            If Left(resp, 1) = "N" Then
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Credit Card Declined. Please Return to Financials to Collect Balance.');window.opener.Refresh_Financials();window.close();", True)
            Else
                Dim oCCT As New clsCCTrans
                Dim oCombo As New clsComboItems
                Dim oCCTPA As New clsCCTrans
                Dim oInv As New clsInvoices
                Dim oPmt As New clsPayments
                Dim oPmt2Inv As New clsPayment2Invoice
                Dim oCC As New clsCreditCard
                Dim oCCTApply As New clsCCTransApplyTo
                Dim oCCMA As New clsCCMerchantAccount
                oInv.KeyField = "ReservationID"
                oInv.KeyValue = Request("ReservationID")
                oInv.ProspectID = 0
                oInv.Load()
                oCCT.CCTransID = hfCCTransID.Value
                oCCT.Load()
                If oCombo.Lookup_ComboItem(oCCT.TransTypeID) = "PreAuth" Then
                    'Create Force
                    If oCCT.Check_PA(hfCCTransID.Value) Then
                        'Create CCTrans Record
                        oCCTPA.CCTransID = hfCCTransID.Value
                        oCCTPA.Load()
                        oCCT.CCTransID = 0
                        oCCT.Load()
                        oCCT.AccountID = oCCTPA.AccountID
                        oCCT.Amount = oCCTPA.Amount
                        oCCT.DateCreated = System.DateTime.Now
                        oCCT.AccountID = oCCTPA.AccountID
                        oCCT.TransTypeID = oCombo.Lookup_ID("CCTransType", "Force")
                        oCCT.CreditCardID = oCCTPA.CreditCardID
                        oCCT.ClientIP = Request.ServerVariables("REMOTE_HOST")
                        oCCT.CreatedByID = Session("UserDBID")
                        oCCT.PreAuthID = hfCCTransID.Value
                        oCCT.Token = "Token On PreAuth"
                        oCCT.Imported = False
                        oCCT.ICVResponse = ""
                        oCCT.Approved = 0
                        oCCT.Save()
                        hfCCTransID.Value = oCCT.CCTransID
                        'Create Payments and Apply To Invoice
                        Dim dt As DataTable = oInv.List_Invoices_With_Balance_ByAcct(oCCMA.Lookup_By_AcctName("~0013~"))
                        oCC.CreditCardID = oCCT.CreditCardID
                        oCC.Load()
                        For i = 0 To dt.Rows.Count - 1
                            oPmt.PaymentID = 0
                            oPmt.Load()
                            oPmt.MethodID = oCombo.Lookup_ID("PaymentMethod", oCombo.Lookup_ComboItem(oCC.TypeID))
                            oPmt.Amount = dt.Rows(i).Item("Balance")
                            oPmt.ApplyToID = 0
                            oPmt.Adjustment = 0
                            oPmt.PosNeg = True
                            oPmt.UserID = Session("UserDBID")
                            oPmt.TransDate = System.DateTime.Now
                            oPmt.Save()
                            oPmt2Inv.Inv2PayID = 0
                            oPmt2Inv.Load()
                            oPmt2Inv.PaymentID = oPmt.PaymentID
                            oPmt2Inv.InvoiceID = dt.Rows(i).Item("ID")
                            oPmt2Inv.Amount = oPmt.Amount
                            oPmt2Inv.PosNeg = True
                            oPmt2Inv.Save()
                            oCCTApply.CCTransApplyToID = 0
                            oCCTApply.Load()
                            oCCTApply.CCTransID = hfCCTransID.Value ' oCCT.CCTransID
                            oCCTApply.PaymentID = oPmt.PaymentID
                            oCCTApply.Amount = oPmt.Amount
                            oCCTApply.Save()
                        Next
                        'Approve CCTrans and fire timer
                        oCCT.Approved = 1
                        oCCT.ApprovedBy = "AutoApprove"
                        oCCT.DateApproved = System.DateTime.Now
                        oCCT.Save()
                        'Fire Timer
                        Timer1.Interval = 2000
                        Timer1.Enabled = True
                        oCCTApply = Nothing
                        oPmt = Nothing
                        opmt2Inv = Nothing
                        oCC = Nothing
                    End If
                Else
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "alert('Credit Card Processed. Please See Financials for Receipt.');window.opener.Refresh_Financials();window.close();", True)
                End If
                oCCT = Nothing
                oCCTPA = Nothing
                oCombo = Nothing
            End If
        End If
    End Sub
    Private Function Check_Status() As String
        Dim response As String = ""
        Dim oCCTrans As New clsCCTrans
        oCCTrans.CCTransID = hfCCTransID.Value
        oCCTrans.Load()
        response = oCCTrans.ICVResponse & ""
        oCCTrans = Nothing
        Return response
    End Function
End Class
