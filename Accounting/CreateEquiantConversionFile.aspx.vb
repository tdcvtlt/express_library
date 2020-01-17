
Imports System.Data.SqlClient
Imports System.IO
Imports ClosedXML.Excel

Partial Class Accounting_CreateEquiantConversionFile
    Inherits System.Web.UI.Page

    Protected Sub GetWorkBook_Click(sender As Object, e As EventArgs) Handles GetWorkbook.Click
        lblErr.Text = ""
        If dfStart.Selected_Date <> "" And dfEnd.Selected_Date <> "" Then
            Generate_Report(1)
            'lblErr.Text = Get_SQL(1)
        ElseIf dfStart.Selected_Date = "" Then
            lblErr.Text = "Please select a Start Date"
        Else
            lblErr.Text = "Please select an End Date"
        End If
    End Sub

    Private Sub Loop_Records(ByRef dr As SqlDataReader, ByRef ws As IXLWorksheet)
        Dim row As Integer = 1
        Dim hasXref As Boolean = False
        Dim hasSalePrice As Boolean = False
        If row = 1 Then
            For col = 1 To dr.VisibleFieldCount
                ws.Cell(row, col).SetValue(dr.GetName(col - 1))
                If ws.Cell(row, col).Value = "Xref1" Then hasXref = True
                If ws.Cell(row, col).Value = "SalePrice" Then hasSalePrice = True
            Next
            row += 1
        End If
        While dr.Read
            If hasSalePrice Then
                If dr("SalePrice") - dr("DownPayment") = 0 Then
                Else
                    For col = 1 To dr.VisibleFieldCount
                        If dr.GetName(col - 1) = "CPA" Then
                            'ws.Cell(row, col).SetValue(oMort.PMT)
                            Dim Rate As Double = dr("InterestRate") / 1200
                            Dim mType As Integer = 0
                            Dim PV As Double = dr("SalePrice") - dr("DownPayment")
                            Dim _TotalFinanced As Double = PV
                            Dim _Terms As Double = dr("LoanTerm")
                            ws.Cell(row, col).SetValue(FormatCurrency(IIf(_TotalFinanced > 0, Math.Round((Rate * (mType + PV * (1 + Rate) ^ _Terms)) / ((1 - Rate * mType) * (1 - (1 + Rate) ^ _Terms)), 2), 0), 2) * -1)
                        ElseIf dr.GetName(col - 1) = "DownPayment" Then
                            ws.Cell(row, col).SetValue(dr.Item(col - 1))
                        ElseIf dr.GetName(col - 1) = "SSN1" Or dr.GetName(col - 1) = "SSN2" Or dr.GetName(col - 1) = "SSN3" Then
                            Dim val As String = dr.Item(col - 1) & ""
                            val = val.Replace("-", "")
                            If val.Length = 9 Then
                                val = val.Insert(5, "-").Insert(3, "-")
                            End If
                            ws.Cell(row, col).SetValue(val)
                        ElseIf dr.GetName(col - 1) = "ClosingCost" Then
                            ws.Cell(row, col).SetValue("")
                        ElseIf dr.GetName(col - 1) = "Country" Then
                            If dr.Item(col - 1) & "" = "USA" Then
                                ws.Cell(row, col).SetValue("")
                            Else
                                ws.Cell(row, col).SetValue(dr.Item(col - 1))
                            End If
                        ElseIf dr.GetName(col - 1) = "LoanAmount" Then
                            ws.Cell(row, col).SetValue(dr("SalePrice") - dr("DownPayment"))
                        Else
                            ws.Cell(row, col).SetValue(dr.Item(col - 1))
                        End If
                    Next
                    row += 1
                    GC.Collect()
                End If
            Else
                For col = 1 To dr.VisibleFieldCount
                    ws.Cell(row, col).SetValue(dr.Item(col - 1))
                Next
                row += 1
                GC.Collect()
            End If

        End While
        dr.Close()
    End Sub


    Private Sub Generate_Report(sql As Integer)
        Dim res As HttpResponse = Response
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand(Get_SQL(sql), cn)
        Dim xlWB As New XLWorkbook
        cm.CommandTimeout = 10000
        cn.Open()

        cm.CommandText = Get_SQL(sql)
        Loop_Records(cm.ExecuteReader, xlWB.Worksheets.Add("Funding"))


        cn.Close()
        cn = Nothing
        cm = Nothing

        res.Clear()
        res.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        If sql = 1 Then
            res.AddHeader("content-disposition", "attachment;filename=""Equiant.xlsx""")
        ElseIf sql = 2 Then
            res.AddHeader("content-disposition", "attachment;filename=""EquiantCancellations.xlsx""")
        Else
            res.AddHeader("content-disposition", "attachment;filename=""Equiant.xlsx""")
        End If
        Using mem As New MemoryStream
            xlWB.SaveAs(mem)
            mem.WriteTo(res.OutputStream)
            mem.Close()
        End Using
        res.End()
    End Sub

    Private Function Get_SQL(ByVal index As Integer) As String
        Dim ret As String = ""
        Select Case index
            Case 1

                ret = "select " &
                        "    case when i.Saletype = 'Estates' then '131000' + CAST(c.ContractID AS varchar(10))  " &
                        "		when i.SaleType = 'Townes' then '133000' + CAST(c.ContractID AS varchar(10)) " &
                        "		when i.SaleType in ('Cottage','Combo') then '132000' + CAST(c.ContractID AS varchar(10)) " &
                        "		Else '' " &
                        "                End As Account, " &
                        "	'KNGCRK' as Client, " &
                        "	'A' as Disposition, " &
                        "	'196' as Lender, " &
                        "	m.SalesPrice as SalePrice, " &
                        "	(select sum(amount-balance) from v_Invoices where keyfield='ConversionDP' and keyvalue = m.ConversionID and invoice='Closing Costs Conv') As ClosingCost, " &
                        "    (select case when sum(amount -balance) is null then 0 else sum(amount - balance) end from v_Invoices where keyfield='ConversionDP' and keyvalue = m.ConversionID and invoice='Down Payment Conv') As DownPayment, " &
                        "	(select SUM(amount * -1) from v_Payments where KeyField='ConversionDP' and KeyValue=m.Conversionid and Method='Equity') As EquityAmount, " &
                        "    m.TotalFinanced As LoanAmount, " &
                        " 'M' as PaymentFrequency, " &
                        "	'Payment' as CPA, " &
                        "	'15.9' as InterestRate, " &
                        "	'SA' as InterestCalculationMethod, " &
                        "	'36' as LoanTerm, " &
                        "	dateadd(dd,30,m.datecreated) As FirstPaymentDueDate, " &
                        "   m.datecreated as TermsDate, " &
                        "   '' as TermsAmortize, " &
                        "                'MA' as ImpoundCode, " &
                        "                '6.00' as ImpoundAmount, " &
                        "	m.datecreated As SaleDate, " &
                        "		dateadd(dd,30,m.datecreated)  As InterestDate, " &
                        "	'0.5P15' as LateChargeCode, " &
                        "   '' as UnitTract1, " &
                        "   '' as WeekLot1, " &
                        "   '' as UnitTract2, " &
                        "   '' as WeekLot2, " &
                        "   '' as PreviousServicerID1, " &
                        "	c.ProspectID as UniqueID1, " &
                        "   '' as PreviousServicerID2, " &
                        "   '' as UniqueID2, " &
                        "                '' as UpgradedFromID, " &
                        "                '' as EquityComputeDate, " &
                        "                '' as UpgradedFromID2, " &
                        "                '' as EquityComputeDate2, " &
                        "                '' as PaymentMethod, " &
                        "                p.LastName as LastName1, " &
                        "	p.FirstName As FirstName1, " &
                        "    p.MiddleInit As MiddleInit1, " &
                        "	p.SpouseLastName As LastName2, " &
                        "    p.SpouseFirstName As FirstName2, " &
                        "   '' as MiddleInit2, " &
                        "    p.SSN As SSN1, " &
                        "	p.SpouseSSN As SSN2, " &
                        "    p.BirthDate As DateOfBirth1, " &
                        "	p.SpouseBirthDate As DateOfBirth2, " &
                         "   p.CreditScore As FicoScore1, " &
                        "	C.ContractDate As FicoDate1, " &
                        "   '' as BNIScore1, " &
                        "   '' as BNIDate1, " &
                        "    p.SpouseCreditScore As FicoScore2, " &
                        "	c.contractdate As FicoDate2, " &
                        "   '' as BNIScore2, " &
                        "   '' as BNIDate2, " &
                         "   (select top 1 address1 from t_ProspectAddress where ProspectID=p.prospectid order by ActiveFlag desc) AddressLine1, " &
                        "	(select top 1 address2 from t_ProspectAddress where ProspectID=p.prospectid order by ActiveFlag desc) AddressLine2, " &
                        "	(select top 1 PostalCode from t_ProspectAddress where ProspectID=p.prospectid order by ActiveFlag desc) ZipCode, " &
                        "	(select top 1 City from t_ProspectAddress where ProspectID=p.prospectid order by ActiveFlag desc) City, " &
                        "	(select top 1 s.comboitem from t_ProspectAddress a left outer join t_ComboItems s on s.ComboItemID = a.StateID where ProspectID=p.prospectid order by ActiveFlag desc) State, " &
                        "	(select top 1 c.comboitem from t_ProspectAddress a left outer join t_ComboItems c on c.ComboItemID = a.CountryID where ProspectID=p.prospectid order by ActiveFlag desc) Country, " &
                        "	'Y' as GoodAddress, " &
                        "	(select top 1 pp.Number from t_ProspectPhone pp where pp.ProspectID = p.ProspectID order by pp.Active desc, pp.PhoneID desc) as PhoneNumber1, " &
                        "	(select top 1 c.comboitem from t_ProspectPhone pp left outer join t_ComboItems c on c.comboitemid = pp.typeid where pp.prospectid = p.prospectid order by pp.active desc, pp.PhoneID desc) as PhoneType1, " &
                        "	(select top 1 pp.Number from (select top 2 * from t_ProspectPhone pp where pp.ProspectID = p.ProspectID order by pp.Active desc, pp.PhoneID desc) pp order by pp.PhoneID asc) as PhoneNumber2, " &
                        "	(select top 1 c.comboitem from (select top 2 * from t_ProspectPhone pp where pp.ProspectID = p.ProspectID order by pp.Active desc, pp.PhoneID desc) pp left outer join t_ComboItems c on c.comboitemid = pp.typeid where pp.prospectid = p.prospectid order by pp.active desc, pp.PhoneID asc) as PhoneType2, " &
                        "	(select top 1 e.email from t_ProspectEmail e where e.ProspectID = p.ProspectID order by e.IsActive desc, e.IsPrimary desc) as EmailAddress1, " &
                        "	'' as EmailType1, " &
                        "	'' as ElectronicStatement1, " &
                        "	'' as EmailAddress2, " &
                        "	'' as EmailType2, " &
                        "	'' as ElectronicStatement2, " &
                        "	'' as ClassCode1, " &
                        "	'' as ClassCode2, " &
                        "	c.ContractID as Xref1, " &
                        "	c.ContractNumber as Xref2, " &
                        "	'' as Points, " &
                        "	'' as ActionCode1, " &
                        "	'' as ActionCode2, " &
                        "	'' as ActionCode3, " &
                        "	'' as RoutingNumber, " &
                        "	'' as BankAccount, " &
                        "	'' as IsChecking, " &
                        "	'' as SpNextDebitDate, " &
                        "	'' as CreditCardNumber, " &
                        "	'' as CreditCardExpMM, " &
                        "	'' as CreditCardExpYY, " &
                        "	'' as CreditCardNextDebitDate, " &
                        "	'' as OnSpCpa, " &
                        "	'' as OffSpCpa, " &
                        "	'' as OnSpInterest, " &
                        "	'' as OffSpInterest, " &
                        "	'' as OnCcCpa, " &
                        "	'' as OffCcCpa, " &
                        "	'' as OnCcInterest, " &
                        "	'' as OffCcInterest, " &
                        "	'' as TeaserRate, " &
                        "	'' as TeaserAmount, " &
                        "	'' as CustomCode1, " &
                        "	'' as CustomValue1, " &
                        "	'' as CustomCode2, " &
                        "	'' as CustomValue2, " &
                        "	'' as CustomCode3, " &
                        "	'' as CustomValue3, " &
                        "	'' as CustomCode4, " &
                        "	'' as CustomValue4, " &
                        "	'' as CustomCode5, " &
                        "	'' as CustomValue5 " &
                        "From t_Contract c  " &
                        "	inner join t_Conversion m on m.ContractID = c.ContractID " &
                        "	inner Join t_Prospect p on p.ProspectID = c.ProspectID " &
                        "	inner join v_ContractInventory i on i.ContractID = c.ContractID " &
                        "   inner join t_Comboitems cs on cs.comboitemid=m.statusid " &
                        "Where m.datecreated between '" & dfStart.Selected_Date & "' and '" & dfEnd.Selected_Date & "' and cs.comboitem='Active' and cs.ComboItem<>'PIF' and cs.comboitem <> 'Cancelled'"

            Case Else

        End Select
        Return ret
    End Function


End Class
