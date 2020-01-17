
Partial Class Reports_Accounting_CCTrans
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click
        Dim cn As Object
        Dim rst As Object
        Dim rst2 As Object
        Dim rst3 As Object
        cn = Server.CreateObject("ADODB.Connection")
        rst = Server.CreateObject("ADODB.Recordset")
        rst2 = Server.CreateObject("ADODB.Recordset")
        rst3 = Server.CreateObject("ADODB.Recordset")
        Dim edate As String = Me.dteEDate.Selected_Date
        Dim sdate As String = Me.dteSDate.Selected_Date
        Dim acct As Integer = Me.ddAccounts.SelectedValue
        Dim sAns As String = ""

        Dim dForceAmt As Double = 0
        Dim dRefAmt As Double = 0
        Dim tRefAmt As Double = 0
        Dim tForceAmt As Double = 0

        Server.ScriptTimeout = 10000
        cn.open(Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
        cn.CommandTimeout = 0

        litReport.Text = "<table>"
        Do While CDate(sDate) <= CDate(eDate)
            dRefAmt = 0
            dForceAmt = 0
            litReport.Text &= "<tr><td colspan = 10><H3>" & sdate & "</H3></td></tr>"
            rst.open("Select c.CCTransID, cc.Number, cc.Expiration, c.Amount, tt.ComboItem as transType, ct.ComboItem as CardType from t_CCTrans c inner join t_CreditCard cc on c.CreditCardID = cc.CreditCardID inner join t_CombOitems tt on c.TranstypeID = tt.ComboItemID left outer join t_CombOItems ct on cc.typeID = ct.ComboitemID where tt.ComboItem in ('Charge','Force','Refund') and c.DateImported Between '" & CDate(sdate) & "' and '" & CDate(edate).AddDays(1) & "'  and accountid = " & acct & " and c.ICVResponse not like 'N%'", cn, 3, 3)
            '*** SKIPPING THIS rst.Open("get_CCTrans '" & CStr(sdate) & "', '" & CStr(sdate) & "', '" & CStr(acct & "") & "'", cn, 3, 3)
            'rst.Open "Select * from v_CCTrans where dateimported between '" & cDate(sDate) & " 00:00:01 AM' and '" & CDate(sDate) & " 11:59:59 PM' and account = '" & CSTR(acct & "") & "' and responsestring not like 'N%' order by Vendor asc", cn, 3, 3
            If rst.EOF And rst.BOF Then
                litReport.Text &= "<tr><td colspan = 10>No transactions in this date range.</td></tr>"
                litReport.Text &= "<tr><td><br></td></tr>"
            Else
                'response.write "<script>alert('rstVendr: " & rst.Fields("Vendor") & "');</script>"
                Do While Not rst.EOF
                    'If ccTransID <> rst.Fields("CCTransID") Then
                    'ccTransID = rst.Fields("CCTransID")
                    'If IsNull(rst.Fields("Vendor").value) Then
                    'If vendor <> "Vendor N/A" And vendor <> "" Then
                    'litReport.text &= "<tr><td colspan = 10><B>&nbsp&nbsp&nbsp&nbsp" & vendor & " Totals:  Refunds: $" & FormatNumber(vRefAmt, 2) & "    Charges: $" & FormatNumber(vForceAMt, 2) & "</td></tr>"
                    'litReport.text &= "<tr><td colspan = 10><br></td></tr>"
                    'litReport.text &= "<tr><td colspan = 10><B>&nbsp&nbsp&nbsp&nbspVendor N/A</B></td></tr>"
                    'vRefAmt = 0
                    'vForceAMt = 0
                    'End If

                    'If vendor = "" Then
                    'vendor = "Vendor N/A"
                    'litReport.text &= "<tr><td colspan = 10><B>&nbsp&nbsp&nbsp&nbsp" & vendor & "</B></td></tr>"
                    'Else
                    'vendor = "Vendor N/A"
                    'End If




                    'ElseIf CStr(vendor & "") <> CStr(Trim(rst.Fields("Vendor").value & "")) Then
                    '   If vendor <> "" Then
                    '          litReport.text &= "<tr><td colspan = 10><B>&nbsp&nbsp&nbsp&nbsp" & vendor & " Totals:  Refunds: $" & FormatNumber(vRefAmt, 2) & "    Charges: $" & FormatNumber(vForceAMt, 2) & "</td></tr>")
                    '         litReport.text &= "<tr><td colspan = 10><br></td></tr>")
                    'End If

                    'vendor = CStr(Trim(rst.Fields("Vendor").value & ""))

                    'vRefAmt = 0
                    'vForceAMt = 0
                    'litReport.text &= "<tr><td colspan = 10><B>&nbsp&nbsp&nbsp&nbsp&nbsp" & vendor & "</B></td></tr>")
                    'End If

                    litReport.Text &= "<tr>"
                    rst2.Open("Select i.KeyField, i.KeyValue, cca.Amount, tc.ComboItem as TransCode from t_CCTransApplyTo cca inner join t_Payments p on cca.paymentid = p.PaymentID inner join t_Payment2Invoice pi on p.PaymentID = pi.PaymentID inner join t_Invoices i on pi.InvoiceID = i.InvoiceID inner join t_FinTransCodes f on i.FintransID = f.FinTransID inner join t_ComboItems tc on f.TransCodeID = tc.ComboItemID where cca.CCTransID = " & rst.Fields("CCTransID").Value, cn, 3, 3)
                    If UCase(rst2.Fields("KeyField").value) = "PACKAGEISSUEDID" Then
                        rst3.Open("Select p.packageIssuedID, ps.ComboItem as PkgStatus from t_packageIssued p left outer join t_ComboItems ps on p.StatusID = ps.ComboItemID where p.PackageIssuedID = " & rst2.Fields("KeyValue").value, cn, 3, 3)
                        litReport.Text &= "<td><font size = '2'>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbspPkgID:" & rst3.Fields("PackageIssuedID").Value & "</td>"
                        litReport.Text &= "<td><font size = '2'>&nbsp&nbsp&nbsp&nbsp" & rst3.Fields("PkgStatus").Value & "" & "</td>"
                        rst3.Close()
                    ElseIf UCase(rst2.Fields("KeyField").Value) = "RESERVATIONID" Then
                        rst3.Open("Select r.ReservationID, rs.ComboItem as resStatus from t_Reservations r left outer join t_ComboItems rs on r.StatusID = rs.ComboItemID where r.ReservationID = " & rst2.Fields("KeyValue").value, cn, 3, 3)
                        litReport.Text &= "<td><font size = '2'>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbspResID:" & rst3.Fields("ReservationID").Value & "</td>"
                        litReport.Text &= "<td><font size = '2'>&nbsp&nbsp&nbsp&nbsp" & rst3.Fields("ResStatus").Value & "" & "</td>"
                        rst3.Close()
                    ElseIf UCase(rst2.Fields("KeyField").Value) = "TOURID" Then
                        rst3.Open("Select t.TourID, ts.ComboItem as TourStatus from t_Tour t left outer join t_ComboItems ts on t.StatusID = ts.ComboItemID where t.TourID = " & rst2.Fields("KeyValue").value, cn, 3, 3)
                        litReport.Text &= "<td><font size = '2'>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbspTourID:" & rst3.Fields("TourID").Value & "</td>"
                        litReport.Text &= "<td><font size = '2'>&nbsp&nbsp&nbsp&nbsp" & rst3.Fields("TourStatus").Value & "" & "</td>"
                        rst3.Close()
                    ElseIf UCase(rst2.Fields("KeyField").Value) = "CONTRACTID" Then
                        rst3.Open("Select c.ContractNumber, cs.ComboItem as ContractStatus from t_Contract c left outer join t_ComboItems cs on c.StatusID = cs.ComboItemID where c.ContractID = " & rst2.Fields("KeyValue").Value, cn, 3, 3)
                        litReport.Text &= "<td><font size = '2'>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbspKCP#:" & rst3.Fields("ContractNumber").Value & "</td>"
                        litReport.Text &= "<td><font size = '2'>&nbsp&nbsp&nbsp&nbsp" & rst3.Fields("ContractStatus").Value & "" & "</td>"
                        rst3.Close()
                    ElseIf UCase(rst2.fields("KeyField").Value) = "MORTGAGEDP" Or rst2.Fields("KeyField").Value = "CONVERSIONDP" Then
                        litReport.Text &= "<td><font size = '2'>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp:" & rst2.fields("KeyField").value & " " & rst2.Fields("KeyValue").Value & "</td>"
                        litReport.Text &= "<td><font size = '2'>&nbsp&nbsp&nbsp&nbspN/A</td>"
                    ElseIf UCase(rst2.Fields("KeyField").Value) = "PROSPECTID" Then
                        rst3.open("Select ProspectID, Lastname + ', ' + Firstname as prospect from t_prospect where prospectid = " & rst2.fields("KeyValue"), cn, 3, 3)
                        litReport.Text &= "<td><font size = '2'>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbspProspectID: " & rst3.Fields("ProspectID").value & " " & rst3.Fields("Prospect") & "</td>"
                        litReport.Text &= "<td><font size = '2'>&nbsp&nbsp&nbsp&nbsp</td>"
                        rst3.Close()
                    Else
                        litReport.Text &= "<td>" & rst2.fields("KeyField").value & "</td>"
                    End If
                    litReport.Text &= "<td><font size = '2'>&nbsp&nbsp&nbsp" & rst.Fields("CardType").Value & "" & "</td>"
                    litReport.Text &= "<td><font size = '2'>&nbsp&nbsp&nbspXXXX" & Right(Trim(rst.Fields("Number").Value), 4) & "</td>"
                    litReport.Text &= "<td><font size = '2'>&nbsp&nbsp&nbsp" & CStr(Left(Trim(rst.Fields("Expiration").value & ""), 2)) & "/" & Right(Trim(rst.Fields("Expiration").value & ""), 2) & "</td>"
                    litReport.Text &= "<td><font size = '2'>&nbsp&nbsp&nbsp" & rst.Fields("TransType").Value & "</td>"
                    litReport.Text &= "<td><font size = '2'>&nbsp&nbsp&nbsp" & FormatCurrency(rst.Fields("Amount").Value, 2) & "</td>"


                    If Trim(rst.Fields("TransType").value) = "Refund" Then
                        tRefAmt = tRefAmt + rst.Fields("Amount").Value
                        dRefAmt = dRefAmt + rst.Fields("Amount").value
                    Else
                        tForceAmt = tForceAmt + rst.Fields("Amount").Value
                        dForceAmt = dForceAmt + rst.Fields("Amount").value
                    End If

                    litReport.Text &= "<td>"
                    Do While Not rst2.EOF
                        litReport.Text &= "<font size = '2'>&nbsp&nbsp&nbsp" & rst2.FIelds("TransCode").Value & " - $" & rst2.FIelds("Amount").Value & "<br>"
                        rst2.MoveNext()
                    Loop
                    litReport.Text &= "</td>"
                    rst2.Close()
                    rst.MoveNext()
                Loop
            End If
            rst.Close()

            'If CStr(vendor & "") <> "" Then
            '   litReport.text &= "<tr><td colspan = '10'><B>&nbsp&nbsp&nbsp&nbsp" & vendor & " Totals: &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp Refunds: $" & FormatNumber(vRefAmt, 2) & "  &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp  Charges: $" & FormatNumber(vForceAMt, 2) & "</td></tr>")
            '  litReport.text &= "<tr><td><br></td></tr>")
            'vRefAmt = 0
            'vForceAMt = 0
            'vendor = ""
            'End If
            'vendor = ""
            litReport.Text &= "<tr><td colspan = '10'><B>" & sdate & " Totals: &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp Refunds: $" & FormatNumber(dRefAmt, 2) & "    &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbspCharges: $" & FormatNumber(dForceAmt, 2) & "</td></tr>"
            litReport.Text &= "<tr><td><br></td></tr>"
            litReport.Text &= "<tr><td><P style = 'page-break-after: always'>&nbsp</P></td></tr>"
            sdate = CDate(sdate).AddDays(1)
        Loop

        litReport.Text &= "<tr><td><br></td></tr>"
        litReport.Text &= "<tr><td colspan = 10><H2>Grand Totals: &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp  Refunds: $" & FormatNumber(tRefAmt, 2) & "  &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp   Charges: $" & FormatNumber(tForceAmt, 2) & "</td></tr>"
        litReport.Text &= "</table>"

        cn.Close()
        rst2 = Nothing
        rst3 = Nothing
        rst = Nothing
        cn = Nothing

        'litReport.text = sAns
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oCCAcct As New clsCCMerchantAccount
            ddAccounts.dATAsOURCE = oCCAcct.List_Accounts()
            ddAccounts.DataTextField = "Description"
            ddAccounts.DataValueField = "AccountID"
            ddAccounts.DataBind()
            oCCAcct = Nothing
        End If
    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        Response.Clear()
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment; filename=CCTrans.xls")
        Response.Write(litReport.Text)
        Response.End()

    End Sub
End Class
