Imports System.Data
Imports System.IO
Imports ClosedXML.Excel
Imports System.Data.SqlClient

Partial Class Accounting_CreateEquiantFile
    Inherits System.Web.UI.Page

    Protected Sub GetWorkBook_Click(sender As Object, e As EventArgs) Handles GetWorkBook.Click
        lblErr.Text = ""
        If dfStart.Selected_Date <> "" And dfEnd.Selected_Date <> "" Then
            Generate_Report(1)
            'lblErr.Text = Get_SQL(1)
        Else
            lblErr.Text = "Please select a date range"
        End If
    End Sub

    Protected Sub GetCancellations_Click(sender As Object, e As EventArgs) Handles GetCancellations.Click
        lblErr.Text = ""
        If dfStart.Selected_Date <> "" And dfEnd.Selected_Date <> "" Then
            Generate_Report(2)
            'lblErr.Text = Get_SQL(1)
        ElseIf dfStart.Selected_Date <> "" Then
            lblErr.Text = "Please select an Ending Date"
        Else
            lblErr.Text = "Please select a Starting Date"
        End If
    End Sub

    Private Sub Loop_Records(ByRef dr As SqlDataReader, ByRef ws As IXLWorksheet)
        Dim row As Integer = 1
        While dr.Read
            If row = 1 Then
                For col = 1 To dr.VisibleFieldCount
                    ws.Cell(row, col).SetValue(dr.GetName(col - 1))
                Next
                row += 1
            End If
            For col = 1 To dr.VisibleFieldCount
                If dr.GetName(col - 1) = "SSN1" Or dr.GetName(col - 1) = "SSN2" Or dr.GetName(col - 1) = "SSN3" Then
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
        If sql = 1 Then
            Loop_Records(cm.ExecuteReader, xlWB.Worksheets.Add("Funding"))
        ElseIf sql = 2 Then
            Loop_Records(cm.ExecuteReader, xlWB.Worksheets.Add("ReDeed"))
        End If


        cn.Close()
        cn = Nothing
        cm = Nothing

        res.Clear()
        res.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        res.AddHeader("content-disposition", "attachment;filename=""Equiant.xlsx""")
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

        Else
            'lblErr.Text = Get_SQL(1)
        End If
    End Sub

    Private Function Get_SQL(ByVal index As Integer) As String
        Dim ret As String = ""
        Select Case index
            Case 1
                ret = "select Case When i.Saletype = 'Estates' then '131100' + CAST(c.ContractID AS varchar(10))  " & _
                        "		When i.SaleType = 'Townes' then '133100' + CAST(c.ContractID AS varchar(10)) " & _
                        "		when i.SaleType in ('Cottage','Combo') then '132100' + CAST(c.ContractID AS varchar(10)) " & _
                        "		Else '' " & _
                        "                End As Account, " & _
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
                        "	(select top 1 pp.Number from t_ProspectPhone pp where pp.ProspectID = p.ProspectID order by pp.Active desc, pp.PhoneID desc) as PhoneNumber1, " & _
                        "	(select top 1 c.comboitem from t_ProspectPhone pp left outer join t_ComboItems c on c.comboitemid = pp.typeid where pp.prospectid = p.prospectid order by pp.active desc, pp.PhoneID desc) as PhoneType1, " & _
                        "	(select top 1 pp.Number from (select top 2 * from t_ProspectPhone pp where pp.ProspectID = p.ProspectID order by pp.Active desc, pp.PhoneID desc) pp order by pp.PhoneID asc) as PhoneNumber2, " & _
                        "	(select top 1 c.comboitem from (select top 2 * from t_ProspectPhone pp where pp.ProspectID = p.ProspectID order by pp.Active desc, pp.PhoneID desc) pp left outer join t_ComboItems c on c.comboitemid = pp.typeid where pp.prospectid = p.prospectid order by pp.active desc, pp.PhoneID asc) as PhoneType2, " & _
                        "	(select top 1 e.email from t_ProspectEmail e where e.ProspectID = p.ProspectID order by e.IsActive desc, e.IsPrimary desc) as EmailAddress1, " & _
                        "	'' as EmailAddress2, " & _
                        "	'KNGCRK' as BillClient, " & _
                        "   'N' as EmailDocuments1, " & _
                        "   (select top 1 address1 from t_ProspectAddress where ProspectID=p.prospectid order by ActiveFlag desc) AddressLine1, " & _
                        "	(select top 1 address2 from t_ProspectAddress where ProspectID=p.prospectid order by ActiveFlag desc) AddressLine2, " & _
                        "	(select top 1 City from t_ProspectAddress where ProspectID=p.prospectid order by ActiveFlag desc) City, " & _
                        "	(select top 1 s.comboitem from t_ProspectAddress a left outer join t_ComboItems s on s.ComboItemID = a.StateID where ProspectID=p.prospectid order by ActiveFlag desc) State, " & _
                        "	(select top 1 PostalCode from t_ProspectAddress where ProspectID=p.prospectid order by ActiveFlag desc) ZipCode, " & _
                        "	(select top 1 c.comboitem from t_ProspectAddress a left outer join t_ComboItems c on c.ComboItemID = a.CountryID where ProspectID=p.prospectid order by ActiveFlag desc) Country, " & _
                        "	'Y' as GoodAddress, " & _
                        "   '' as OldID, " & _
                         "   '' As FicoScore1, " & _
                        "	'' As FicoDate1, " & _
                        "	'A' as Disposition, " & _
                         "   case when i.Frequency='Annual' then 'A' " & _
                        "		when i.Frequency = 'Biennial' then  " & _
                        "			Case when YEAR(c.ContractDate) % 2 = 0 then " & _
                        "				'E' " & _
                        "			else  " & _
                        "				'O' " & _
                        "			end " & _
                        "		when i.Frequency = 'Triennial' then " & _
                        "			'TAS' " & _
                        "		Else  " & _
                        "			'' " & _
                        "		End As PaymentFrequency, " & _
                        "      'ST' as PaymentMethod, " & _
                        "    year(c.occupancydate) as UseYear, " & _
                        "	'' as MiscClientDate, '' as Points,c.contractid as UniqueID,'' as RoutingNumber, '' as BankAccount, '' as IsChecking, '' as CreditCardNumber,'' as CreditCardExpMM, '' as CreditCardExpYY, " & _
                        " c.StatusDate as SaleDate, c.ContractNumber as Unit, '' as Week, '' as LateChargeCode, '' as SetupDate,''as MemberID,'' as TransferFlag, '' as ActionCode1, '' as ActionCode2, '' as ActionCode3, '' as CustomCode1, '' as CustomValue1, " & _
                        " '' as CustomCode2, '' as CustomValue2, " & _
                        " '' as CustomCode3, '' as CustomValue3, " & _
                        " '' as CustomCode4, '' as CustomValue4, " & _
                        " '' as CustomCode5, '' as CustomValue5 " & _
                        "From t_Contract c  " & _
                        "	inner join t_Mortgage m on m.ContractID = c.ContractID " & _
                        "	inner Join t_Prospect p on p.ProspectID = c.ProspectID " & _
                        "	inner join v_ContractInventory i on i.ContractID = c.ContractID " & _
                        "   inner join t_Comboitems css on css.comboitemid=c.substatusid " & _
                        "Where p.firstname<>'Resort' and p.firstname <> 'King''s Creek Plantation' and c.contractnumber like '%R%' and c.ContractID In (Select keyvalue from t_Event where keyfield = 'ContractID' and fieldname='DateCreated' and datecreated between '" & dfStart.Selected_Date & "' and '" & dfEnd.Selected_Date & "' )"

            Case 2
                ret = "Select Case When i.Saletype = 'Estates' then '131100' + CAST(c.ContractID AS varchar(10))   " & _
                      "         When i.SaleType = 'Townes' then '133100' + CAST(c.ContractID AS varchar(10))  " & _
                      "  		when i.SaleType in ('Cottage','Combo') then '132100' + CAST(c.ContractID AS varchar(10))  " & _
                      "  		Else ''  " & _
                      "         End As Account,  " & _
                      "    'DISP' as Type, 'PBS' as System, '' as Amount, '' as Note, '' As Date,'' as SourceCode,  " & _
                      "    Case When cs.Comboitem = 'Rescinded' then 'X'  " & _
                      "     when cs.comboitem = 'Canceled' and css.comboitem = 'Upgrade' then 'X'  " & _
                      "     Else 'X' end as DispCode  " & _
                      "From t_Contract c   " & _
                      "  	inner join t_Mortgage m on m.ContractID = c.ContractID  " & _
                      "  	inner Join t_Prospect p on p.ProspectID = c.ProspectID  " & _
                      "  	inner join v_ContractInventoryHistory i on i.ContractID = c.ContractID  " & _
                      "     left outer Join t_Comboitems cs on cs.comboitemid=c.Statusid  " & _
                      "     left outer join t_Comboitems css on css.comboitemid=c.SubStatusid  " & _
                      "Where cs.comboitem In ('Canceled') and css.ComboItem in ('ReDeed') " & _
                      "     and c.statusdate between '" & dfStart.Selected_Date & "' and '" & dfEnd.Selected_Date & "'"
            Case Else

        End Select
        Return ret
    End Function


End Class
