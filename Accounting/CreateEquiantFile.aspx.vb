Imports System.Data
Imports System.IO
Imports ClosedXML.Excel
Imports System.Data.SqlClient

Partial Class Accounting_CreateEquiantFile
    Inherits System.Web.UI.Page

    Protected Sub GetWorkBook_Click(sender As Object, e As EventArgs) Handles GetWorkBook.Click
        lblErr.Text = ""
        If lstFunding.Items.Count > 0 And ddLender.selectedValue <> 0 Then
            Generate_Report(1)
            'lblErr.Text = Get_SQL(1)
        ElseIf ddLender.selectedvalue = 0 Then
            lblErr.Text = "Please select a Lender"
        Else
            lblErr.Text = "Please select a Funding"
        End If
    End Sub

    Protected Sub GetCancellations_Click(sender As Object, e As EventArgs) Handles GetCancellations.Click
        lblErr.Text = ""
        If lstFunding.Items.Count > 0 And ddLender.SelectedValue <> 0 Then
            Generate_Report(2)
            'lblErr.Text = Get_SQL(1)
        ElseIf ddLender.SelectedValue = 0 Then
            lblErr.Text = "Please select a Lender"
        Else
            lblErr.Text = "Please select a Funding"
        End If
    End Sub

    Private Sub Loop_Records(ByRef dr As SqlDataReader, ByRef ws As IXLWorksheet)
        Dim row As Integer = 1
        Dim hasXref As Boolean = False
        If row = 1 Then
            For col = 1 To dr.VisibleFieldCount
                ws.Cell(row, col).SetValue(dr.GetName(col - 1))
                If ws.Cell(row, col).Value = "Xref1" Then hasXref = True
            Next
            row += 1
        End If
        While dr.Read

            Dim oMort As New clsMortgage
            If hasXref Then
                oMort.ContractID = dr("Xref1")
                oMort.Load()
            End If
            For col = 1 To dr.VisibleFieldCount
                If dr.GetName(col - 1) = "CPA" Or dr.GetName(col - 1) = "OffSpCpa" Then
                    ws.Cell(row, col).SetValue(oMort.PMT)
                ElseIf dr.GetName(col - 1) = "OnSpCpa" Then
                    oMort.APR = oMort.APR - 1.0
                    ws.Cell(row, col).SetValue(oMort.PMT)
                    oMort.APR = oMort.APR + 1.0
                ElseIf dr.GetName(col - 1) = "InterestRate" Or dr.GetName(col - 1) = "OffSpInterest" Then
                    ws.Cell(row, col).SetValue(oMort.APR)
                ElseIf dr.GetName(col - 1) = "OnSpInterest" Then
                    ws.Cell(row, col).SetValue(oMort.APR - 1.0)
                ElseIf dr.GetName(col - 1) = "DownPayment" Then
                    If dr("EquityAmount") Is System.DBNull.Value Then
                        ws.Cell(row, col).SetValue(dr.Item(col - 1))
                    Else
                        ws.Cell(row, col).SetValue(dr.Item(col - 1) - dr("EquityAmount"))
                    End If
                ElseIf dr.GetName(col - 1) = "Lender" Then
                    If dr("LoanAmount") = 0 Then
                        ws.Cell(row, col).SetValue(dr.Item(col - 1))
                    Else
                        ws.Cell(row, col).SetValue(ddLender.SelectedValue)
                    End If
                ElseIf dr.GetName(col - 1) = "SSN1" Or dr.GetName(col - 1) = "SSN2" Or dr.GetName(col - 1) = "SSN3" Then
                    Dim val As String = dr.Item(col - 1) & ""
                    val = val.Replace("-", "")
                    If val.Length = 9 Then
                        val = val.Insert(5, "-").Insert(3, "-")
                    End If
                    ws.Cell(row, col).SetValue(val)
                Else
                    ws.Cell(row, col).SetValue(dr.Item(col - 1))
                End If
            Next
            row += 1
            oMort = Nothing
            GC.Collect()
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

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        lblErr.Text = ""
        If Not (IsPostBack) Then
            ddFunding.DataSource = (New SqlDataSource(Resources.Resource.cns, "Select FundingID, Name from t_Funding order by fundingid desc"))
            ddFunding.DataTextField = "Name"
            ddFunding.DataValueField = "FundingID"
            ddFunding.DataBind()
        Else
            'lblErr.Text = Get_SQL(1)
        End If
    End Sub

    Private Function Get_SQL(ByVal index As Integer) As String
        Dim ret As String = ""
        Select Case index
            Case 1
                Dim fundings As String = ""
                For Each item As ListItem In lstFunding.Items
                    fundings &= IIf(fundings = "", item.Value, "," & item.Value)
                Next
                ret = "select Case when m.TotalFinanced = 0 then 'B' else 'ALL' end as System, " & _
                        "    case when i.Saletype = 'Estates' then '131100' + CAST(c.ContractID AS varchar(10))  " & _
                        "		when i.SaleType = 'Townes' then '133100' + CAST(c.ContractID AS varchar(10)) " & _
                        "		when i.SaleType in ('Cottage','Combo') then '132100' + CAST(c.ContractID AS varchar(10)) " & _
                        "		Else '' " & _
                        "                End As LoanAccount, " & _
                        "	Case When i.Saletype = 'Estates' then '131100' + CAST(c.ContractID AS varchar(10))  " & _
                        "		When i.SaleType = 'Townes' then '133100' + CAST(c.ContractID AS varchar(10)) " & _
                        "		when i.SaleType in ('Cottage','Combo') then '132100' + CAST(c.ContractID AS varchar(10)) " & _
                        "		Else '' " & _
                        "                End As BillAccount, " & _
                        "	'KNGCRK' as LoanClient, " & _
                        "	'KNGCRK' as BillClient, " & _
                        "	case when c.contractnumber like 'A%' then 'T' when c.contractnumber like 'F%' then 'T' else 'A' end as LoanDisposition, " & _
                        "	'A' as BillDisposition, " & _
                        "	'' as Lender, " & _
                        "	m.SalesPrice as SalePrice, " & _
                        "	m.CCTotal * -1 As ClosingCost, " & _
                        "    m.DPTotal As DownPayment, " & _
                        "	(select SUM(amount * -1) from v_Payments where KeyField='MortgageDP' and KeyValue=m.MortgageID and Method='Equity') As EquityAmount, " & _
                        "    m.TotalFinanced As LoanAmount, " & _
                        " 'M' as LoanPaymentFrequency, " & _
                         "   case when i.Frequency='Annual' then 'A' " & _
                        "		when i.Frequency = 'Biennial' then  " & _
                        "			Case when YEAR(c.OccupancyDate) % 2 = 0 then " & _
                        "				'E' " & _
                        "			else  " & _
                        "				'O' " & _
                        "			end " & _
                        "		when i.Frequency = 'Triennial' then " & _
                        "			'FTA' " & _
                        "		Else  " & _
                        "			'' " & _
                        "		End As BillPaymentFrequency, " & _
                        "	'Payment' as CPA, " & _
                        "	'17.9' as InterestRate, " & _
                        "	'SA' as InterestCalculationMethod, " & _
                        "	m.Terms as LoanTerm, " & _
                        "	m.FirstPaymentDate As FirstPaymentDueDate, " & _
                        "                'MA' as ImpoundCode, " & _
                        "                m.PaymentFee as ImpoundAmount, " & _
                        "	c.contractdate As LoanSaleDate, " & _
                        "    c.contractdate As BillSaleDate, " & _
                        "	Case When datediff(dd,c.ContractDate,m.FirstPaymentDate) > 30 then " & _
                        "        DateAdd(DD, -30, m.FirstPaymentDate) " & _
                        "	else	 " & _
                        "		c.contractdate  " & _
                        "	End As InterestDate, " & _
                        "	'5P15' as LoanLateChargeCode, " & _
                        "	c.ProspectID as LoanUniqueID1, " & _
                        "	c.ProspectID As BillUniqueID1, " & _
                        "                '' as UpgradedFromID, " & _
                        "                '' as EquityComputeDate, " & _
                        "                '' as UpgradedFromID2, " & _
                        "                '' as EquityComputeDate2, " & _
                        "                '' as LoanPaymentMethod, " & _
                        "                'ST' as BillPaymentMethod, " & _
                        "                p.LastName as LastName1, " & _
                        "	p.FirstName As FirstName1, " & _
                        "    p.MiddleInit As MiddleInit1, " & _
                        "	p.SpouseLastName As LastName2, " & _
                        "    p.SpouseFirstName As FirstName2, " & _
                        "   '' as MiddleInit2, " & _
                        "    p.SSN As SSN1, " & _
                        "	p.SpouseSSN As SSN2, " & _
                        "    p.BirthDate As DateOfBirth1, " & _
                        "	p.SpouseBirthDate As DateOfBirth2, " & _
                         "   p.CreditScore As FicoScore1, " & _
                        "	C.ContractDate As FicoDate1, " & _
                        "    p.SpouseCreditScore As FicoScore2, " & _
                        "	c.contractdate As FicoDate2, " & _
                         "   (select top 1 address1 from t_ProspectAddress where ProspectID=p.prospectid order by ActiveFlag desc) AddressLine1, " & _
                        "	(select top 1 address2 from t_ProspectAddress where ProspectID=p.prospectid order by ActiveFlag desc) AddressLine2, " & _
                        "	(select top 1 City from t_ProspectAddress where ProspectID=p.prospectid order by ActiveFlag desc) City, " & _
                        "	(select top 1 s.comboitem from t_ProspectAddress a left outer join t_ComboItems s on s.ComboItemID = a.StateID where ProspectID=p.prospectid order by ActiveFlag desc) State, " & _
                        "	(select top 1 PostalCode from t_ProspectAddress where ProspectID=p.prospectid order by ActiveFlag desc) ZipCode, " & _
                        "	(select top 1 c.comboitem from t_ProspectAddress a left outer join t_ComboItems c on c.ComboItemID = a.CountryID where ProspectID=p.prospectid order by ActiveFlag desc) Country, " & _
                        "	'Y' as GoodAddress, " & _
                        "	(select top 1 pp.Number from t_ProspectPhone pp where pp.ProspectID = p.ProspectID order by pp.Active desc, pp.PhoneID desc) as PhoneNumber1, " & _
                        "	(select top 1 c.comboitem from t_ProspectPhone pp left outer join t_ComboItems c on c.comboitemid = pp.typeid where pp.prospectid = p.prospectid order by pp.active desc, pp.PhoneID desc) as PhoneType1, " & _
                        "	(select top 1 pp.Number from (select top 2 * from t_ProspectPhone pp where pp.ProspectID = p.ProspectID order by pp.Active desc, pp.PhoneID desc) pp order by pp.PhoneID asc) as PhoneNumber2, " & _
                        "	(select top 1 c.comboitem from (select top 2 * from t_ProspectPhone pp where pp.ProspectID = p.ProspectID order by pp.Active desc, pp.PhoneID desc) pp left outer join t_ComboItems c on c.comboitemid = pp.typeid where pp.prospectid = p.prospectid order by pp.active desc, pp.PhoneID asc) as PhoneType2, " & _
                        "	(select top 1 e.email from t_ProspectEmail e where e.ProspectID = p.ProspectID order by e.IsActive desc, e.IsPrimary desc) as EmailAddress1, " & _
                        "	'' as EmailAddress2, " & _
                        "	c.ContractID as Xref1, " & _
                        "	c.ContractNumber as Xref2, " & _
                        "	'' as LoanRoutingNumber, " & _
                        "	'' as LoanBankAccount, " & _
                        "	'' as LoanIsChecking, " & _
                        "	'' as BillActionCode2, " & _
                        "	'' as BillRoutingNumber, " & _
                        "	'' as BillBankAccount, " & _
                        "	'' as BillIsChecking, " & _
                        "	'' as SpNextDebitDate, " & _
                        "   '' as OnSpCpa, " & _
                        "   '' as OffSpCpa, " & _
                        "   '16.9' as OnSpInterest, " & _
                        "   '17.9' as OffSpInterest, " & _
                        " c.ContractNumber as LoanUnitTract1, " & _
                        " c.ContractNumber as BillUnitTract1, " & _
                        " case when i.Frequency='Annual' then 'A' " & _
                        "		when i.Frequency = 'Biennial' then  " & _
                        "			Case when YEAR(c.OccupancyDate) % 2 = 0 then " & _
                        "				'E' " & _
                        "			else  " & _
                        "				'O' " & _
                        "			end " & _
                        "		when i.Frequency = 'Triennial' then " & _
                        "			'FTA' " & _
                        "		Else  " & _
                        "			'' " & _
                        "		End as LoanWeekLot1, " & _
                        "case when year(c.contractdate) < year(c.occupancydate) then " & _
                        "           '2/1/' + cast(year(c.occupancydate) as varchar(4)) " & _
                        "       when year(c.contractdate) = year(c.occupancydate) then " & _
                        "           cast(month(c.contractdate) + 1 as varchar(4)) + '/1/' + cast(year(c.occupancydate) as varchar(4)) " & _
                        "       when year(c.contractdate) > year(c.occupancydate) then " & _
                        "           cast(month(c.contractdate) + 1 as varchar(4)) + '/1/' + cast(year(c.contractdate) as varchar(4)) " & _
                        "       else " & _
                        "           '' " & _
                        "       end as MiscClientDate " & _
                        "From t_Contract c  " & _
                        "	inner join t_Mortgage m on m.ContractID = c.ContractID " & _
                        "	inner Join t_Prospect p on p.ProspectID = c.ProspectID " & _
                        "	inner join v_ContractInventory i on i.ContractID = c.ContractID " & _
                        "   inner join t_ComboItems cs on c.StatusID = cs.ComboItemID " & _
                        "Where cs.comboitem not in ('Rescinded','Canceled') and c.ContractID In (Select ContractID from t_FundingItems i inner join t_Funding f On f.FundingID = i.FundingID where f.FundingID In (" & fundings & "))"
            Case 2
                Dim fundings As String = ""
                For Each item As ListItem In lstFunding.Items
                    fundings &= IIf(fundings = "", item.Value, "," & item.Value)
                Next
                ret = "Select Case When i.Saletype = 'Estates' then '131100' + CAST(c.ContractID AS varchar(10))  " & _
                        "		When i.SaleType = 'Townes' then '133100' + CAST(c.ContractID AS varchar(10)) " & _
                        "		when i.SaleType in ('Cottage','Combo') then '132100' + CAST(c.ContractID AS varchar(10)) " & _
                        "		Else '' " & _
                        "                End As Account, " & _
                        "   'DISP' as Type, 'LOAN' as System, '' as Amount, '' as Note, '' As Date,'' as SourceCode, " & _
                        "   Case when cs.Comboitem = 'Rescinded' then 'X' " & _
                        "        when cs.comboitem = 'Canceled' and css.comboitem = 'Upgrade' then 'Q' " & _
                        "        else 'Q' end as DispCode " & _
                        "From t_Contract c  " & _
                        "	inner join t_Mortgage m on m.ContractID = c.ContractID " & _
                        "	inner Join t_Prospect p on p.ProspectID = c.ProspectID " & _
                        "	inner join v_ContractInventory i on i.ContractID = c.ContractID " & _
                        "   left outer join t_Comboitems cs on cs.comboitemid=c.Statusid " & _
                        "   left outer join t_Comboitems css on css.comboitemid=c.SubStatusid " & _
                        "Where cs.comboitem in ('Rescinded','Canceled') and c.ContractID In (Select ContractID from t_FundingItems i inner join t_Funding f On f.FundingID = i.FundingID where f.FundingID In (" & fundings & "))"
            Case Else

        End Select
        Return ret
    End Function
    Protected Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        lstFunding.SelectedIndex = -1
        lstFunding.Items.Add(ddFunding.SelectedItem)
        ddFunding.Items.Remove(ddFunding.SelectedItem)
    End Sub
    Protected Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
        If Not (IsNothing(lstFunding.SelectedItem)) Then
            ddFunding.SelectedIndex = -1
            ddFunding.Items.Add(lstFunding.SelectedItem)
            lstFunding.Items.Remove(lstFunding.SelectedItem)
        End If
    End Sub

    Private Sub ReOrderDropDown(ByRef dd As DropDownList)
        'Dim listCopy As New List(Of ListItem)
        'For Each item As ListItem In dd.Items
        '    listCopy.Add(item)
        'Next
        'dd.Items.Clear()
        'For Each item In listCopy.Sort()
        '    dd.Items.Add(item)
        'Next
    End Sub

End Class
