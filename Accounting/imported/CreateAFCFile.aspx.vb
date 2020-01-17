Imports System.Data
Imports System.IO
Imports ClosedXML.Excel
Imports System.Data.SqlClient

Partial Class Accounting_CreateAFCFile
    Inherits System.Web.UI.Page

    Protected Sub GetWorkBook_Click(sender As Object, e As EventArgs) Handles GetWorkBook.Click
        lblErr.Text = ""
        If ddMF.SelectedItem.Text <> "" And dfStart.Selected_Date <> "" And dfEnd.Selected_Date <> "" Then
            Generate_Report()
            'lblErr.Text = Get_SQL(1)
        Else
            If ddMF.SelectedItem.Text = "" Then
                lblErr.Text = "Please select a maintenance fee"
            ElseIf dfStart.Selected_Date = "" Then
                lblErr.Text = "Please select a start date"
            Else
                lblErr.Text = "Please select an end date"
            End If
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
                ws.Cell(row, col).SetValue(dr.Item(col - 1))
            Next
            row += 1
        End While
        dr.Close()
    End Sub


    Private Sub Generate_Report()
        Dim res As HttpResponse = Response
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand(Get_SQL(1), cn)
        Dim xlWB As New XLWorkbook
        cn.Open()

        cm.CommandText = Get_SQL(1)
        Loop_Records(cm.ExecuteReader, xlWB.Worksheets.Add("Owners"))
        
        cm.CommandText = Get_SQL(2)
        Loop_Records(cm.ExecuteReader, xlWB.Worksheets.Add("Contracts"))

        cm.CommandText = Get_SQL(3)
        Loop_Records(cm.ExecuteReader, xlWB.Worksheets.Add("Dues"))

        cn.Close()
        cn = Nothing
        cm = Nothing

        res.Clear()
        res.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        res.AddHeader("content-disposition", "attachment;filename=""AFC.xlsx""")
        Using mem As New MemoryStream
            xlWB.SaveAs(mem)
            mem.WriteTo(res.OutputStream)
            mem.Close()
        End Using
        res.End()
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            ddMF.DataSource = (New clsFinancialTransactionCodes).List_MF
            ddMF.DataTextField = "Code"
            ddMF.DataValueField = "Code"
            ddMF.DataBind()
        Else
            lblErr.Text = Get_SQL(1)
        End If
    End Sub

    Private Function Get_SQL(ByVal index As Integer) As String
        Dim ret As String = ""
        Select Case index
            Case 1
                ret = "select distinct p.prospectid as [Owner ID], " & _
                        "             '' as [Owner Number]," & _
                        "             p.Firstname, " & _
                        "             p.Lastname, " & _
                        " (select top 1 case when p.prospectid = co.prospectid then p.SpouseFirstName else co.Firstname end from t_Prospect co inner join t_ContractCoOwner cco on cco.prospectid = co.prospectid where cco.contractid = c.contractid)  as [Co-FirstName]," & _
                        " (select top 1 case when p.prospectid = co.prospectid then p.SpouseLastName else co.Lastname end from t_Prospect co inner join t_ContractCoOwner cco on cco.prospectid = co.prospectid where cco.contractid = c.contractid)  as [Co-LastName]," & _
                        " (select top 1 Address1 from t_ProspectAddress where prospectid = p.prospectid order by activeflag desc, contractaddress desc) as Address1," & _
                        " (select top 1 City from t_ProspectAddress where prospectid = p.prospectid order by activeflag desc, contractaddress desc) as City," & _
                        " (select top 1 Comboitem from t_ProspectAddress a left outer join t_Comboitems s on s.comboitemid= a.stateid where prospectid = p.prospectid order by a.activeflag desc, contractaddress desc) as State," & _
                        " (select top 1 PostalCode from t_ProspectAddress where prospectid = p.prospectid order by activeflag desc, contractaddress desc) as Zip," & _
                        " (select top 1 Comboitem from t_ProspectAddress a left outer join t_Comboitems b on b.comboitemid= a.countryid where prospectid = p.prospectid order by a.activeflag desc, contractaddress desc) as Country," & _
                        " (select top 1 LEFT(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(NUMBER,' ',''),'(',''),')',''),'-',''),'.',''),10) from t_ProspectPhone pp left outer join t_Comboitems pt on pt.comboitemid = pp.typeid where pt.comboitem = 'Home' and pp.prospectid = p.prospectid order by pp.active desc) as [Home Phone]," & _
                        " (select top 1 LEFT(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(NUMBER,' ',''),'(',''),')',''),'-',''),'.',''),10) from t_ProspectPhone pp left outer join t_Comboitems pt on pt.comboitemid = pp.typeid where pt.comboitem = 'Mobile' and pp.prospectid = p.prospectid order by pp.active desc) as [Mobile Phone]," & _
                        " (select top 1 LEFT(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(NUMBER,' ',''),'(',''),')',''),'-',''),'.',''),10) from t_ProspectPhone pp left outer join t_Comboitems pt on pt.comboitemid = pp.typeid where pt.comboitem = 'Work' and pp.prospectid = p.prospectid order by pp.active desc) as [Work Phone]," & _
                        " (select top 1 email from t_ProspectEmail where prospectid = p.prospectid order by isactive desc) as [Primary Email Address]," & _
                        " RIGHT(RTRIM(LTRIM(P.SSN)),4) AS [Last 4 SSN of 1st Owner]" & _
                    " from t_Prospect p" & _
                        " inner join t_Contract c on c.prospectid = p.prospectid" & _
                        " inner join v_ContractInventory ci on ci.contractid = c.contractid" & _
                    " where c.ContractID in (select  ContractID from (" & _
                                " select p.prospectid as [Owner ID], c.ContractID," & _
                                " 	(select sum(case when balance is null then 0 else balance end) from v_Invoices where keyfield = 'contractid' and keyvalue = c.contractid and invoice = '" & ddMF.SelectedItem.Text & "' AND TransDate BETWEEN '" & dfStart.Selected_Date & "' AND '" & dfEnd.Selected_Date & "') as [Current Balance Due] " & _
                                " from t_Contract c " & _
                                " 	inner join t_Prospect p on p.prospectid = c.prospectid " & _
                                " 	left join t_Comboitems wt on wt.comboitemid = c.weektypeid " & _
                                " 	inner join v_ContractInventory ci on ci.contractid = c.contractid " & _
                                " 	inner join t_Comboitems mfs on mfs.comboitemid = c.maintenancefeestatusid " & _
                                " 	inner join (select * from v_Invoices where keyfield = 'contractid' and invoice = '" & ddMF.SelectedItem.Text & "' AND TransDate BETWEEN '" & dfStart.Selected_Date & "' AND '" & dfEnd.Selected_Date & "') i on i.keyvalue = c.contractid " & _
                                " 	inner join t_Comboitems cs on cs.comboitemid = c.statusid " & _
                                " where c.contractid in (select distinct  contractid from t_Soldinventory) and mfs.comboitem like 'afc%' " & _
                                " 	and cs.comboitem in ('ACTIVE','SUSPENSE','DEVELOPER','REDEED') " & _
                                " ) a  " & _
                                " ) order by p.ProspectID"
            Case 2
                ret = "select distinct  * from ( " & _
                        " select p.prospectid as [Owner ID], " & _
                              "           '' as [Owner Number], " & _
                             "            c.ContractID, " & _
                            " c.ContractNumber as [Contract Number], " & _
                            " ci.Frequency as [Usage], " & _
                            " case when bc.Code IS null then " & _
                            " case when ci.SaleType = 'Cottage' then " & _
                            " 	case when c.contractdate < '11/18/05' then " & _
                             "            'KCP01' " & _
                            " 	else " & _
                            " 		case when ci.cottageinventory = 1 and c.contractdate < '2/25/08' then " & _
                             "            'KCP04' " & _
                            " 		when ci.cottageinventory = 1 and c.contractdate > '2/24/08' then " & _
                             "            'KCP11' " & _
                            " 		else " & _
                            "             'KCP20' " & _
                              "           End " & _
                             "            End " & _
                            " when ci.saletype = 'Townes' then " & _
                            " 	case when c.contractdate < '11/18/05' then " & _
                            " 		case when ci.townesinventory = 1 then " & _
                             "            'KCP02' " & _
                            " 		when ci.townesinventory = 2 then " & _
                             "            'KCP03' " & _
                            " 		else  " & _
                              "           'KCP00' " & _
                             "            End " & _
                            " 	when c.contractdate < '2/25/08' then " & _
                            " 		case when ci.townesinventory = 1 then " & _
                             "            'KCP05' " & _
                            " 		when ci.townesinventory = 2 then " & _
                             "            'KCP06' " & _
                            " 		else  " & _
                              "           'KCP00' " & _
                             "            End " & _
                            " 	else " & _
                            " 		case when ci.townesinventory = 1 then " & _
                             "            'KCP12' " & _
                            " 		when ci.townesinventory = 2 then " & _
                            " 			case when c.contractdate < '12/14/09' then " & _
                             "            'KCP10' " & _
                            " 			else " & _
                              "           'KCP13' " & _
                             "            End " & _
                            " 		when ci.townesinventory = 4 then " & _
                             "            'KCP22' " & _
                            " 		else " & _
                               "          'KCP00' " & _
                              "           End " & _
                             "            End " & _
                            " when ci.saletype = 'Combo' then " & _
                            " 	case when c.contractdate < '7/1/13' then " & _
                             "            'KCP18' " & _
                            " 	else " & _
                            "             'KCP19' " & _
                             "            End " & _
                            " when ci.saletype = 'Estates' then " & _
                            " 	case when c.contractdate between '11/18/05' and '2/25/08' then " & _
                            " 		case when estatesinventory2bd = 1 and estatesinventory1bddwn = 1 and estatesinventory1bdup = 1 then " & _
                             "            'KCP09' " & _
                            " 		when estatesinventory2bd = 1 and estatesinventory1bddwn = 1 and estatesinventory1bdup = 0 then " & _
                             "            'KCP08' " & _
                            " 		when estatesinventory2bd = 1 and estatesinventory1bddwn = 0 and estatesinventory1bdup = 0 then " & _
                             "            'KCP00' " & _
                            " 		when estatesinventory2bd = 0 and estatesinventory1bddwn = 1 and estatesinventory1bdup = 0 then " & _
                            "             'KCP07' " & _
                            " 		when estatesinventory2bd = 1 and estatesinventory1bddwn = 0 and estatesinventory1bdup = 1 then " & _
                             "            'KCP07' " & _
                            " 		when estatesinventory2bd = 0 and estatesinventory1bddwn = 0 and estatesinventory1bdup = 1 then " & _
                             "            'KCP07' " & _
                            " 		else  " & _
                              "           'KCP00' " & _
                             "            End " & _
                            " 	else " & _
                            " 		case when estatesinventory2bd = 1 and estatesinventory1bddwn = 1 and estatesinventory1bdup = 1 then " & _
                             "            'KCP17' " & _
                            " 		when estatesinventory2bd = 1 and estatesinventory1bddwn = 1 and estatesinventory1bdup = 0 then " & _
                             "            'KCP16' " & _
                            " 		when estatesinventory2bd = 1 and estatesinventory1bddwn = 0 and estatesinventory1bdup = 0 then " & _
                             "            'KCP15' " & _
                            " 		when estatesinventory2bd = 0 and estatesinventory1bddwn = 1 and estatesinventory1bdup = 0 then " & _
                             "            'KCP14' " & _
                            " 		when estatesinventory2bd = 1 and estatesinventory1bddwn = 0 and estatesinventory1bdup = 1 then " & _
                             "            'KCP14' " & _
                            " 		when estatesinventory2bd = 0 and estatesinventory1bddwn = 1 and estatesinventory1bdup = 1 then " & _
                             "            'KCP15' " & _
                            " 		when estatesinventory2bd = 0 and estatesinventory1bddwn = 0 and estatesinventory1bdup = 1 then " & _
                             "            'KCP14' " & _
                            " 		else  " & _
                            " 			case when estatesinventory2bd = 2 and estatesinventory1bddwn = 2 and estatesinventory1bdup = 2 then " & _
                             "            'KCP23' " & _
                            " 			when estatesinventory2bd = 0 and estatesinventory1bddwn + estatesinventory1bdup = 8 then " & _
                             "            'KCP24' " & _
                            " 			when estatesinventory2bd = 3 and estatesinventory1bddwn = 3 and estatesinventory1bdup = 3 then " & _
                            "             'KCP25' " & _
                            " 			else " & _
                            " 				case when (ci.cottageinventory * 3) + (ci.townesinventory * 2) + (ci.estatesinventory2bd * 2) + (ci.estatesinventory1bddwn * 1) + (ci.estatesinventory1bdup * 1) = 2 then " & _
                             "            'KCP26' " & _
                            " 				when (ci.cottageinventory * 3) + (ci.townesinventory * 2) + (ci.estatesinventory2bd * 2) + (ci.estatesinventory1bddwn * 1) + (ci.estatesinventory1bdup * 1) = 3 then " & _
                             "            'KCP27' " & _
                            " 				when (ci.cottageinventory * 3) + (ci.townesinventory * 2) + (ci.estatesinventory2bd * 2) + (ci.estatesinventory1bddwn * 1) + (ci.estatesinventory1bdup * 1) = 4 then " & _
                             "            'KCP28' " & _
                            " 				else " & _
                              "           'KCP00' " & _
                             "            End " & _
                            "             End " & _
                           "              End " & _
                          "               End " & _
                         "                End " & _
                        " 	else " & _
                         "                bc.Code " & _
                        " 	end as [Bill Code], " & _
                        " 	ci.SaleType as [Unit Week Number], " & _
                        " 	(ci.cottageinventory * 3) + (ci.townesinventory * 2) + (ci.estatesinventory2bd * 2) + (ci.estatesinventory1bddwn * 1) + (ci.estatesinventory1bdup * 1) as [Room Type], " & _
                        " 	wt.comboitem as [Week Type], " & _
                        " 	0 as [Balance Forward], " & _
                        " 	CASE WHEN (select sum(case when balance is null then 0 else balance end) from v_Invoices where keyfield = 'contractid' and keyvalue = c.contractid and invoice = '" & ddMF.SelectedItem.Text & "' AND TransDate BETWEEN '" & dfStart.Selected_Date & "' AND '" & dfEnd.Selected_Date & "') IS NULL THEN " & _
                        " 0 " & _
                        " 	ELSE " & _
                        " 		(select sum(case when balance is null then 0 else balance end) from v_Invoices where keyfield = 'contractid' and keyvalue = c.contractid and invoice = '" & ddMF.SelectedItem.Text & "' AND TransDate BETWEEN '" & dfStart.Selected_Date & "' AND '" & dfEnd.Selected_Date & "') " & _
                        " 	END as [Current Balance Due], " & _
                        " 	cs.comboitem as Status, " & _
                        " 	mfs.comboitem as [Maintenance Fee Status], " & _
                        " 	case when ci.frequency = 'biennial' then " & _
                        " 		case when year(c.occupancydate) % 2 = 0 then  " & _
                         "                '2/1/16' " & _
                        " 		else  " & _
                          "               '2/1/15' " & _
                         "                End " & _
                        " 	when ci.frequency = 'triennial' then " & _
                        " 		case when (2017 - year(c.occupancydate)) % 3 = 0 then " & _
                        "                 '2/1/17' " & _
                        " 		when (2015 - year(c.occupancydate)) % 3 = 0 then " & _
                         "                '2/1/15' " & _
                        " 		when (2016 - year(c.occupancydate)) % 3 = 0 then " & _
                         "                '2/1/16' " & _
                        " 		else " & _
                          "               '2/1/13' " & _
                         "                End " & _
                        " 	else " & _
                         "                '2/1/15' " & _
                        " 	end as [Next Due Date], " & _
                        " 	case when ci.frequency = 'biennial' then " & _
                        " 		case when year(c.occupancydate) % 2 = 0 then " & _
                         "                'EVEN' " & _
                        " 		else " & _
                          "               'ODD' " & _
                         "                End " & _
                        " 	else " & _
                         "                '' " & _
                        " 	end as [Odd or Even?], " & _
                        " 	case when (select max(origdate) from t_Mortgage where contractid = c.contractid) is null then c.contractdate else c.contractdate end as [Contract Start Date], " & _
                         "                'FIXED' as [Unit Type], " & _
                        " 	case when (select max(firstpaymentdate) from t_Mortgage where contractid = c.contractid) > getdate() then  " & _
                         "                'YES' " & _
                        " 	else  " & _
                         "                'NO' " & _
                        " 	end as [Close of Escrow Billing? Yes or No], " & _
                        " 	case when ci.frequency = 'biennial' then " & _
                        " 		case when year(c.occupancydate) % 2 = 0 then  " & _
                         "                '1/1/16' " & _
                        " 		else  " & _
                          "               '1/1/15' " & _
                         "                End " & _
                        " 	when ci.frequency = 'triennial' then " & _
                        " 		case when (2017 - year(c.occupancydate)) % 3 = 0 then " & _
                        "                 '1/1/17' " & _
                        " 		when (2015 - year(c.occupancydate)) % 3 = 0 then " & _
                        "                 '1/1/15' " & _
                        " 		when (2016 - year(c.occupancydate)) % 3 = 0 then " & _
                         "                '1/1/16' " & _
                        " 		else " & _
                          "               '1/1/13' " & _
                         "                End " & _
                        " 	else " & _
                         "                '1/1/15' " & _
                        " 	end as [Next Charge Date] " & _
                        " from t_Contract c " & _
                        " 	inner join t_Prospect p on p.prospectid = c.prospectid " & _
                        " 	left outer join t_Comboitems wt on wt.comboitemid = c.weektypeid " & _
                        " 	inner join v_ContractInventory ci on ci.contractid = c.contractid " & _
                        " 	left outer join t_Comboitems cs on cs.comboitemid = c.statusid " & _
                        " 	left outer join t_MaintenanceFeeCodes bc on bc.MaintenanceFeeCodeID = c.MaintenanceFeeCodeID " & _
                        " 	left outer join t_Comboitems mfs on mfs.comboitemid = c.maintenancefeestatusid " & _
                        " 	inner join (select * from v_Invoices where keyfield = 'contractid' and invoice = '" & ddMF.SelectedItem.Text & "' AND TransDate BETWEEN '" & dfStart.Selected_Date & "' AND '" & dfEnd.Selected_Date & "') i on i.keyvalue = c.contractid " & _
                         " where c.contractid in (select distinct  contractid from t_Soldinventory) and mfs.comboitem like 'afc%' " & _
                        " 	and cs.comboitem in ('ACTIVE','SUSPENSE','DEVELOPER','REDEED') " & _
                        " ) a  " & _
                            " order by [Owner ID] "

            Case 3
                ret = "select i.id as DuesID, " & _
                    " 	c.ContractID," & _
                    " 	c.ContractNumber as [Contract Number], " & _
                    " 	i.invoice as [Charge Code], " & _
                    " 	i.transdate as [Charge Date], " & _
                    " 	i.invoice_description as [Charge Description], " & _
                    " 	'Charge' as [Charge], " & _
                    " 	i.invoiceamount as [Charge Amount], " & _
                    " 	i.balance as [Current Balance], " & _
                    " 	i.invoiceamount - i.balance as [Total Payment/Credit Amount], " & _
                    " 	'' as [Payment Type] " & _
                    " from t_Contract c " & _
                    " 	inner join (select * from v_Invoices where keyfield = 'contractid' and invoice = '" & ddMF.SelectedItem.Text & "' AND TransDate BETWEEN '" & dfStart.Selected_Date & "' AND '" & dfEnd.Selected_Date & "') i on i.keyvalue = c.contractid " & _
                    " 	inner join t_Comboitems mfs on mfs.comboitemid = c.maintenancefeestatusid " & _
                    " where mfs.comboitem like 'afc%' and c.contractid in (select distinct contractid from t_Soldinventory) and c.prospectid in (select  [Owner ID] from ( " & _
                            " 	select p.prospectid as [Owner ID], " & _
                            " 		(select sum(case when balance is null then 0 else balance end) from v_Invoices where keyfield = 'contractid' and keyvalue = c.contractid and invoice = '" & ddMF.SelectedItem.Text & "' AND TransDate BETWEEN '" & dfStart.Selected_Date & "' AND '" & dfEnd.Selected_Date & "') as [Current Balance Due] " & _
                            " 	from t_Contract c " & _
                            " 		inner join t_Prospect p on p.prospectid = c.prospectid " & _
                            " 		left outer join t_Comboitems wt on wt.comboitemid = c.weektypeid " & _
                            " 		inner join v_ContractInventory ci on ci.contractid = c.contractid " & _
                            " 		inner join t_Comboitems mfs on mfs.comboitemid = c.maintenancefeestatusid " & _
                            " 		inner join (select * from v_Invoices where keyfield = 'contractid' and invoice = '" & ddMF.SelectedItem.Text & "' AND TransDate BETWEEN '" & dfStart.Selected_Date & "' AND '" & dfEnd.Selected_Date & "') i on i.keyvalue = c.contractid " & _
                            " 		inner join t_Comboitems cs on cs.comboitemid = c.statusid " & _
                            " 	where c.contractid in (select distinct  contractid from t_Soldinventory) and mfs.comboitem like 'afc%' " & _
                            " 		and cs.comboitem in ('ACTIVE','SUSPENSE','DEVELOPER','REDEED') " & _
                            " 	) a  " & _
                            " 	) "
            Case Else

        End Select
        Return ret
    End Function
End Class
