
Imports System.Data.SqlClient
Imports System.IO
Imports ClosedXML.Excel

Partial Class wizards_Accounting_EditCancellationBatch
    Inherits System.Web.UI.Page
    Dim dt As System.Data.DataTable
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        lblMessage.Text = ""
        If Not IsPostBack Then
            mvSteps.ActiveViewIndex = 0
            hfBatchID.Value = If(Request("bid") <> "" And Request("bid") <> "0", Request("bid"), "0")
            Dim cn As New SqlConnection(Resources.Resource.cns)
            Dim cm As New SqlCommand("select * from ( select 0 as Comboitemid, '<Choose>' as Comboitem union select i.Comboitemid, i.comboitem from t_ComboItems i inner join t_Combos c on c.ComboID=i.ComboID where c.ComboName='ContractSubStatus' and (i.ComboItem like '%Dev%' or i.ComboItem like '%Rev%' or i.ComboItem like '%Fore%') and i.Active=1 ) a  order by a.Comboitem", cn)
            Dim da As New SqlDataAdapter(cm)
            Dim ds As New DataSet
            da.Fill(ds, "Types")
            ddBatchType.DataSource = ds.Tables("Types")
            ddBatchType.DataTextField = "Comboitem"
            ddBatchType.DataValueField = "ComboitemID"
            ddBatchType.DataBind()
            siBatchStatus.Connection_String = Resources.Resource.cns
            siBatchStatus.ComboItem = "CancellationBatchStatus"
            siBatchStatus.Load_Items()
            ds = Nothing
            da = Nothing
            cm = Nothing
            cn = Nothing
            If Request("bid") <> "" And Request("bid") <> "0" Then
                Load_Batch()
                trStart.Visible = False
            Else
                trInstNum.Visible = False
                trUpdate.Visible = False
                trNextStep.Visible = False
                trNextStepDate.Visible = False
                trStatus.Visible = False
            End If
            'hfShowReport.Value = 0
        Else
            'lblMessage.Text = ""
        End If
        'mvReport.ActiveViewIndex = hfShowReport.Value
        'CrystalReportViewer2.Visible = False
        'If hfShowReport.Value = 1 Then
        '    CrystalReportViewer1.ReportSource = Session("Report")
        '    If IsNothing(Session("Report2")) Then
        '        CrystalReportViewer2.Visible = False
        '    Else
        '        If DirectCast(Session("Report2"), ReportDocument).HasRecords Then
        '            CrystalReportViewer2.ReportSource = Session("Report2")
        '            CrystalReportViewer2.Visible = True
        '        Else
        '            CrystalReportViewer2.Visible = False
        '        End If
        '    End If
        'End If
    End Sub

    Private Sub Load_Batch()
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("Select distinct c.comboitem, cb.NextStepDate, c.Description from t_CancellationBatch2Contract cb inner join t_Comboitems c on c.comboitemid=cb.nextstep where cb.batchid=" & hfBatchID.Value & " order by c.comboitem desc", cn)
        Dim da As New SqlDataAdapter(cm)
        Dim ds As New DataSet
        da.Fill(ds, "Status")

        Dim ob As New clsCancellationBatch
        hfBatchID.Value = If(Request("bid") = "" Or Request("bid") = "0", "0", Request("bid"))
        ob.BatchID = hfBatchID.Value
        ob.Load()
        txtNextStep.Text = If(ds.Tables("Status").Rows.Count > 0, ds.Tables("Status").Rows(0)("Comboitem"), "Unknown")
        txtNextStepDate.Text = If(ds.Tables("Status").Rows.Count > 0, ds.Tables("Status").Rows(0)("NextStepDate"), "Unknown")
        lblNextStep.Text = If(ds.Tables("Status").Rows.Count > 0, "(" & ds.Tables("Status").Rows(0)("Description") & ")", "")
        ddBatchType.SelectedValue = ob.TypeID
        trInstNum.Visible = If(ddBatchType.SelectedItem.Text = "MF Foreclosure", True, False)
        trPublicationDate1.Visible = True
        trPublicationDate2.Visible = True
        dfBatchHearingDate.Selected_Date = ob.HearingDate
        dfPublicationDate1.Selected_Date = ob.PublicationDate1
        dfPublicationDate2.Selected_Date = ob.PublicationDate2
        siBatchStatus.Selected_ID = ob.StatusID
        ob = Nothing
        ds = Nothing
        da = Nothing
        cm = Nothing
        cn = Nothing
    End Sub

    Protected Sub btnGenInstRequest_Click(sender As Object, e As EventArgs) Handles btnGenInstRequest.Click
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("Select * from t_CancellationBatch2Contract where dateremoved is null and batchid=" & hfBatchID.Value, cn)
        cn.Open()
        cm.CommandText = GET_COMMANDTEXT("InstNum")
        mvSteps.ActiveViewIndex = 1
        gvBatchContracts.DataSource = cm.ExecuteReader
        gvBatchContracts.DataBind()
        cn.Close()
        cn = Nothing
        cm = Nothing
        hfTitle.Value = ddBatchType.SelectedItem.Text & " - " & dfBatchHearingDate.Selected_Date.ToString.Replace("/", "-")
    End Sub

    Private Sub Load_Contracts()
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("Select * from t_CancellationBatch2Contract where dateremoved is null and batchid=" & hfBatchID.Value, cn)
        cn.Open()
        cm.CommandText = GET_COMMANDTEXT("")
        gvBatchContracts.DataSource = cm.ExecuteReader
        gvBatchContracts.DataBind()
        cn.Close()
        cn = Nothing
        cm = Nothing
    End Sub

    Protected Sub lbBatch_Click(sender As Object, e As EventArgs) Handles lbBatch.Click
        mvSteps.ActiveViewIndex = 0
    End Sub
    Protected Sub lbContracts_Click(sender As Object, e As EventArgs) Handles lbContracts.Click
        mvSteps.ActiveViewIndex = If(hfBatchID.Value = "0" Or hfBatchID.Value = "", 0, 1)
        If mvSteps.ActiveViewIndex > 0 Then Load_Contracts()
        hfTitle.Value = ddBatchType.SelectedItem.Text & " - " & dfBatchHearingDate.Selected_Date.ToString.Replace("/", "-") ' & " - " & "Contracts"
    End Sub
    Protected Sub lbAddContracts_Click(sender As Object, e As EventArgs) Handles lbAddContracts.Click
        mvSteps.ActiveViewIndex = If(hfBatchID.Value = "0" Or hfBatchID.Value = "", 0, 2)
        mvAdd.ActiveViewIndex = 0
    End Sub
    Protected Sub lbEvents_Click(sender As Object, e As EventArgs) Handles lbEvents.Click
        mvSteps.ActiveViewIndex = If(hfBatchID.Value = "0" Or hfBatchID.Value = "", 0, 3)
    End Sub

    Private Function GET_COMMANDTEXT(Optional req As String = "") As String
        Dim ret As String = ""
        ret = "Select  m.Number As Equiant, c.contractid As KCP, p.FirstName + ' ' + p.LastName as Name1, " & _
                "   Convert(Varchar(50),coalesce(bc.CurrentBalance,0),1) as CurrentBalance, bc.LenderCode, " & _
                "   Convert(Varchar(50),coalesce(m.SalesPrice,0),1) as SalesPrice, Convert(Varchar(50),coalesce(m.TotalFinanced,0),1) As AmountFinanced, " & _
                "	p.CreditScore As FICOScore, uf.ChargeBack As Chargeback, " & _
                "	ca.Name As MKTGSource, " & _
                "	(Select top 1 pe.FirstName + ' ' + pe.LastName from t_PersonnelTrans pt inner join t_Personnel pe on pe.PersonnelID=pt.PersonnelID inner join t_ComboItems t on t.ComboItemID = pt.TitleID where t.ComboItem='TO' and pt.KeyField='ContractID' and pt.KeyValue = c.contractid order by PersonnelTransID desc) as [T/O], " & _
                "	(Select top 1 pe.FirstName + ' ' + pe.LastName from t_PersonnelTrans pt inner join t_Personnel pe on pe.PersonnelID=pt.PersonnelID inner join t_ComboItems t on t.ComboItemID = pt.TitleID where t.ComboItem='Sales Executive' and pt.KeyField='ContractID' and pt.KeyValue = c.contractid order by PersonnelTransID desc) as [SalesPerson], " & _
                "	'' as FrontLine, " & _
                "	'' as InHouse, " & _
                "	'ST' as PaymentType, " & _
                "	bc.DaysPastDueInitial, " & _
                "	bc.PaymentsMade, " & _
                "    bc.LastPayment, " & _
                "	bc.InterestPaidThruDate, " & _
                "    (select top 1 Name1 + case when coalesce(name2,'') <> '' then ', ' + name2 else '' end + case when coalesce(name3,'') <> '' then ', ' + name3 else '' end from ufn_ContractInventory(c.contractid)) as Unit, " & _
                "   (select top 1 Week from ufn_ContractInventory(c.contractid)) as Week, " & _
                "	ci.SaleType as Phase, " & _
                "	REPLACE(ci.BD,'BD','') as BR, " & _
                "	ci.Frequency as Freq, " & _
                "   (select top 1 case when Week < 9 then 'White' else 'Red' end from ufn_ContractInventory(c.contractid)) as Season, " & _
                "   Convert(Varchar(50),convert(money,coalesce(case when ci.BD='Unknown' then 0 else case when ci.SaleType='Cottage' then  " & _
                "		Case when ci.Frequency='Annual' then 1 when ci.Frequency='Biennial' then .5 else 1/3 End * CAST(replace(ci.bd,'BD','') AS int)/3 " & _
                "	when ci.SaleType='Townes' then  " & _
                "		Case when ci.frequency = 'Annual' then 1 when ci.Frequency= 'Biennial' then .5 else 1/3 End * CAST(replace(ci.bd,'BD','') AS int)/2 " & _
                "	when ci.SaleType = 'Estates' then " & _
                "		Case when ci.Frequency='Annual' then CAST(replace(ci.bd,'BD','') AS money)/4 when ci.Frequency='Biennial' then CAST(replace(ci.bd,'BD','') AS money)/8 else CAST(replace(ci.bd,'BD','') AS money)/12 End " & _
                "	Else " & _
                "		0 " & _
                "	End end,0)),1) As Interval, " & _
                "	c.ContractDate, " & _
                "	bc.FirstNotice, " & _
                "	bc.SecondNotice, " & _
                "	cb.HearingDate, " & _
                "	uf.DeedInstrumentNumber As Deed#, " & _
                "	uf.DeedOfTrustInstrumentNumber As DOTInst#, " & _
                "	uf.DOTRecDate As DeedDate " & _
                "from t_CancellationBatch2Contract bc " & _
                    "inner join t_CancellationBatch cb On cb.BatchID=bc.BatchID " & _
                    "inner join t_Contract c On c.ContractID = bc.ContractID " & _
                    "inner join t_Prospect p On p.ProspectID = c.ProspectID " & _
                    "inner join t_Mortgage m On m.ContractID=c.ContractID " & _
                    "left outer join t_Campaign ca On ca.CampaignID=c.CampaignID " & _
                    "left outer join v_ContractInventory ci On ci.ContractID = c.ContractID " & _
                    "left outer join (Select distinct ContractID, ChargeBack, DeedInstrumentNumber,DeedOfTrustInstrumentNumber,DOTRecDate from v_ContractUserFields) uf On uf.ContractID=c.ContractID " & _
                "where bc.batchid = " & hfBatchID.Value & " and bc.dateremoved is null"

        If ddBatchType.SelectedItem.Text = "MF Foreclosure" Then
            If req = "InstNum" Then
                ret = "select  m.Number as Equiant, c.contractnumber as KCP, p.FirstName + ' ' + p.LastName as Name1, " & _
                        "(select top 1 case when co.ProspectID=c.ProspectID then p1.SpouseFirstName + ' ' + p1.SpouseLastName else p1.FirstName + ' ' + p1.LastName end from t_Prospect p1 inner join t_ContractCoOwner co on co.ProspectID = p1.prospectid where co.contractid = c.contractid order by co.CoOwnerID) as Name2, " & _
                        "(select top 1 case when co.ProspectID=c.ProspectID then p1.SpouseFirstName + ' ' + p1.SpouseLastName else p1.FirstName + ' ' + p1.LastName end from t_Prospect p1 inner join t_ContractCoOwner co on co.ProspectID = p1.prospectid where co.contractid = c.contractid  and co.ProspectID <> c.ProspectID and co.ProspectID not in (select top 1 p1.ProspectID from t_Prospect p1 inner join t_ContractCoOwner co on co.ProspectID = p1.prospectid where co.contractid = c.contractid order by co.CoOwnerID)  order by co.CoOwnerID) as Name3, " & _
                        "(select top 1 case when co.ProspectID=c.ProspectID then p1.SpouseFirstName + ' ' + p1.SpouseLastName else p1.FirstName + ' ' + p1.LastName end from t_Prospect p1 inner join t_ContractCoOwner co on co.ProspectID = p1.ProspectID where co.contractid = c.contractid  and co.ProspectID <> c.ProspectID and co.ProspectID not in (select top 1 ProspectID from t_ContractCoOwner where ContractID=c.ContractID and ProspectID <> c.ProspectID order by CoOwnerID)  order by co.CoOwnerID) as Name4, " & _
                        "(select top 1 Address1 from t_PRospectAddress where prospectid = c.prospectid and activeflag=1 order by AddressID) as StreetAddress2, " & _
                        "(select top 1 Address2 from t_PRospectAddress where prospectid = c.prospectid and activeflag=1 order by AddressID) as Address, " & _
                        "(select top 1 City from t_PRospectAddress where prospectid = c.prospectid and activeflag=1 order by AddressID) as City, " & _
                        "(select top 1 s.comboitem from t_PRospectAddress a left outer join t_ComboItems s on s.ComboItemID=a.StateID where prospectid = c.prospectid and activeflag=1 order by AddressID) as State, " & _
                        "(select top 1 PostalCode from t_PRospectAddress where prospectid = c.prospectid and activeflag=1 order by AddressID) as PostalCode, " & _
                        "(select top 1 Address1 from t_ProspectAddress where ActiveFlag=1 and prospectid <> c.prospectid and prospectid in (select prospectid from t_ContractCoOwner where contractid=c.contractid) order by AddressID) as StreetAddress2, " & _
                        "(select top 1 Address2 from t_ProspectAddress where ActiveFlag=1 and prospectid <> c.prospectid and prospectid in (select prospectid from t_ContractCoOwner where contractid=c.contractid) order by AddressID) as Address2, " & _
                        "(select top 1 City from t_ProspectAddress where ActiveFlag=1 and prospectid <> c.prospectid and prospectid in (select prospectid from t_ContractCoOwner where contractid=c.contractid) order by AddressID) as City2, " & _
                        "(select top 1 s.comboitem from t_ProspectAddress a left outer join t_ComboItems s on s.ComboItemID=a.StateID where ActiveFlag=1 and prospectid <> c.prospectid and prospectid in (select prospectid from t_ContractCoOwner where contractid=c.contractid) order by AddressID) as State2, " & _
                        "(select top 1 PostalCode from t_ProspectAddress where ActiveFlag=1 and prospectid <> c.prospectid and prospectid in (select prospectid from t_ContractCoOwner where contractid=c.contractid) order by AddressID) as PostalCode2, " & _
                        "(select top 1 Name1 + case when coalesce(name2,'') <> '' then ', ' + name2 else '' end + case when coalesce(name3,'') <> '' then ', ' + name3 else '' end from ufn_ContractInventory(c.contractid)) as Unit,  " & _
                        "(select top 1 Week from ufn_ContractInventory(c.contractid)) as Week, " & _
                        "	ci.Frequency As Freq,  " & _
                        "    Convert(Varchar(50),convert(money,coalesce(Case When ci.BD = 'Unknown' then " & _
                        "		0 " & _
                        "    Else " & _
                        "		Case When ci.SaleType='Cottage' then   " & _
                        "			Case When ci.Frequency='Annual' then 1 when ci.Frequency='Biennial' then .5 else 1/3 End * CAST(replace(ci.bd,'BD','') AS int)/3  " & _
                        "		When ci.SaleType='Townes' then   " & _
                        "			Case When ci.frequency = 'Annual' then 1 when ci.Frequency= 'Biennial' then .5 else 1/3 End * CAST(replace(ci.bd,'BD','') AS int)/2  " & _
                        "		When ci.SaleType = 'Estates' then  " & _
                        "			Case When ci.Frequency='Annual' then CAST(replace(ci.bd,'BD','') AS money)/4 when ci.Frequency='Biennial' then CAST(replace(ci.bd,'BD','') AS money)/8 else CAST(replace(ci.bd,'BD','') AS money)/12 End  " & _
                        "		Else  " & _
                        "			0  " & _
                        "		End " & _
                        "	End,0)),1) As Interval, 	 " & _
                        "Convert(Varchar(50),coalesce((select SUM(Balance) from v_Invoices where invoice = 'MF16' and keyfield='ContractID' and keyvalue = c.ContractID),0),1) as [Due as of 2/1/16], " & _
                        "Convert(Varchar(50),coalesce((select SUM(Balance) from v_Invoices where invoice = 'MF15' and keyfield='ContractID' and keyvalue = c.ContractID),0),1) as [Due as of 2/1/15], " & _
                        "Convert(Varchar(50),coalesce((select SUM(Balance) from v_Invoices where invoice = 'MF14' and keyfield='ContractID' and keyvalue = c.ContractID),0),1) as [Due as of 2/1/14], " & _
                        "Convert(Varchar(50),coalesce((select SUM(Balance) from v_Invoices where invoice = 'MF13' and keyfield='ContractID' and keyvalue = c.ContractID),0),1) as [Due as of 2/1/13], " & _
                        "Convert(Varchar(50),coalesce((select SUM(Balance) from v_Invoices where invoice = 'MF12' and keyfield='ContractID' and keyvalue = c.ContractID),0),1) as [Due as of 2/1/12], " & _
                        "Convert(Varchar(50),coalesce((select SUM(Balance) from v_Invoices where invoice = 'MF11' and keyfield='ContractID' and keyvalue = c.ContractID),0),1) as [Due as of 2/1/11], " & _
                        "Convert(Varchar(50),coalesce((select SUM(Balance) from v_Invoices where invoice = 'MF10' and keyfield='ContractID' and keyvalue = c.ContractID),0),1) as [Due as of 2/1/10], " & _
                        "Convert(Varchar(50),coalesce((select SUM(Balance) from v_Invoices where invoice = 'Late Fee' and keyfield='ContractID' and keyvalue = c.ContractID),0),1) as [LF], " & _
                        "Convert(Varchar(50),coalesce((select SUM(Balance) from v_Invoices where invoice = 'NSF' and keyfield='ContractID' and keyvalue = c.ContractID),0),1) as [Return Check Fee], " & _
                        "Convert(Varchar(50),coalesce((select SUM(Balance) from v_Invoices where invoice = 'LegalFee' and keyfield='ContractID' and keyvalue = c.ContractID),0),1) as [Legal], " & _
                        "Convert(Varchar(50),coalesce((select SUM(Balance) from v_Invoices where invoice = 'AdminFee' and keyfield='ContractID' and keyvalue = c.ContractID),0),1) as [Admin], " & _
                        "Convert(Varchar(50),coalesce((select SUM(Balance) from v_Invoices where invoice = 'Interest' and keyfield='ContractID' and keyvalue = c.ContractID),0),1) as [Interest], " & _
                        "Convert(Varchar(50),coalesce((select SUM(Balance) from v_Invoices where invoice = 'SA14' and keyfield='ContractID' and keyvalue = c.ContractID),0),1) as [Special Assessment], " & _
                        "	Convert(Varchar(50),coalesce(m.SalesPrice,0),1) as SalesPrice, " & _
                        "    c.ContractDate As Dated " & _
                        "from t_CancellationBatch2Contract bc  " & _
                        "    inner join t_CancellationBatch cb On cb.BatchID=bc.BatchID  " & _
                        "    inner join t_Contract c On c.ContractID = bc.ContractID  " & _
                        "    inner join t_Prospect p On p.ProspectID = c.ProspectID  " & _
                        "    inner join t_Mortgage m On m.ContractID=c.ContractID  " & _
                        "    left outer join t_Campaign ca On ca.CampaignID=c.CampaignID " & _
                        "    left outer join v_ContractInventory ci On ci.ContractID = c.ContractID  " & _
                        "    left outer join (Select distinct ContractID, ChargeBack, DeedInstrumentNumber,DeedOfTrustInstrumentNumber,DOTRecDate from v_ContractUserFields) uf On uf.ContractID=c.ContractID  " & _
                        "where bc.batchid =  " & hfBatchID.Value & " And bc.dateremoved Is null"
            End If
        End If
        Return ret
    End Function
    Protected Sub lbUpload_Click(sender As Object, e As EventArgs) Handles lbUpload.Click
        mvAdd.ActiveViewIndex = 0
    End Sub
    Protected Sub lbIndividual_Click(sender As Object, e As EventArgs) Handles lbIndividual.Click
        mvAdd.ActiveViewIndex = 1
    End Sub
    Protected Sub lbExcel_Click(sender As Object, e As EventArgs) Handles lbExcel.Click
        Generate_Report(hfTitle.Value)
    End Sub

    Private Sub Loop_Records(ByRef dr As GridView, ByRef ws As IXLWorksheet)
        Dim row As Integer = 1
        Dim col As Integer = 1
        For Each cell In dr.HeaderRow.Cells
            ws.Cell(row, col).SetValue(cell.Text)
            ws.Cell(row, col).Style.Fill.SetBackgroundColor(IIf(cell.BackColor = Drawing.Color.Empty, XLColor.FromColor(Drawing.Color.White), XLColor.FromColor(cell.BackColor)))
            'ws.Cell(row, col).AddConditionalFormat().WhenNotBlank().Fill.SetBackgroundColor(IIf(cell.BackColor = Drawing.Color.Empty, XLColor.FromColor(Drawing.Color.White), XLColor.FromColor(cell.BackColor)))
            col += 1
        Next
        row += 1
        For Each gRow As GridViewRow In dr.Rows
            If gRow.Cells(0).Text <> "First Name" Then
                col = 1
                For Each cell As TableCell In gRow.Cells
                    ws.Cell(row, col).SetValue(Replace(cell.Text, "&nbsp;", ""))
                    ws.Cell(row, col).Style.Fill.SetBackgroundColor(IIf(cell.BackColor = Drawing.Color.Empty, XLColor.FromColor(Drawing.Color.White), XLColor.FromColor(cell.BackColor)))
                    'ws.Cell(row, col).AddConditionalFormat().WhenNotBlank().Fill.SetBackgroundColor(IIf(cell.BackColor = Drawing.Color.Empty, XLColor.FromColor(Drawing.Color.White), XLColor.FromColor(cell.BackColor)))
                    col += 1
                Next
                row += 1
            End If
        Next

    End Sub

    Private Sub Generate_Report(sTitle As String)
        Dim res As HttpResponse = Response
        Dim xlWB As New XLWorkbook

        Loop_Records(gvBatchContracts, xlWB.Worksheets.Add(sTitle))

        res.Clear()
        res.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        res.AddHeader("content-disposition", "attachment;filename= """ & hfTitle.Value & ".xlsx""")
        Using mem As New MemoryStream
            xlWB.SaveAs(mem)
            mem.WriteTo(res.OutputStream)
            mem.Close()
        End Using
        res.End()
    End Sub
    Protected Sub btnUpload_Click(sender As Object, e As EventArgs) Handles btnUpload.Click
        If xlsUpload.HasFile Then
            Dim upload As Boolean = True
            Dim strStep As String = "Upload file"
            Dim fileUpload As String = System.IO.Path.GetExtension(xlsUpload.FileName.ToString())
            If fileUpload.Trim().ToLower = ".xls" Or fileUpload.Trim.ToLower = ".xlsx" Then
                xlsUpload.SaveAs("\\rs-fs-01\UploadedContracts\misprojects\" + xlsUpload.FileName.ToString)
                Dim uploadedFile As String = "\\rs-fs-01\UploadedContracts\misprojects\" + xlsUpload.FileName.ToString
                Dim strColumnList() As String = {"kcp", "contractid", "contractnumber", "contract id", "contract number"}
                Try
                    strStep = "xlsInsert"
                    dt = xlsInsert(uploadedFile)
                    Dim ds As New Data.DataTable
                    For i = 0 To dt.Columns.Count - 1
                        If strColumnList.Contains(dt.Columns(i).ColumnName.ToLower) Then ds.Columns.Add(dt.Columns(i).ColumnName)
                    Next
                    For Each row As Data.DataRow In dt.Rows
                        Dim newRow As Data.DataRow = ds.NewRow
                        For i = 0 To ds.Columns.Count - 1
                            newRow(ds.Columns(i).ColumnName) = row(ds.Columns(i).ColumnName)
                        Next
                        If newRow(0) & "" <> "" Then
                            ds.Rows.Add(newRow)
                        Else
                            newRow = Nothing
                        End If
                    Next
                    strStep = "Get Equiant Information"
                    Get_Equiant_Information(ds)
                    Dim start As String = lblMessage.Text
                    'lblMessage.Text = DateDiff(DateInterval.Second, CDate(start), Date.Now) & " seconds "
                    gvBatch.DataSource = ds
                    gvBatch.DataBind()
                    If ds.Rows.Count > 0 Then
                        btnStart.Enabled = True
                    End If
                Catch ex As Exception
                    lblMessage.Text = ex.Message.ToString & " - " & strStep
                End Try

            End If
        End If
    End Sub

    Private Sub Get_Equiant_Information(ByRef table As Data.DataTable)
        Dim oCont As clsContract
        Dim oMort As clsMortgage
        Dim oEQ As New clsEquiant
        table.Columns.Add("AccountNumber")
        table.Columns.Add("Lender")
        table.Columns.Add("Principal Balance")
        table.Columns.Add("DaysDelinquent")
        table.Columns.Add("PaymentsMade")
        For Each row In table.Rows
            If row(0) & "" <> "" Then
                oCont = New clsContract
                oMort = New clsMortgage
                oCont.ContractNumber = row("KCP") & ""
                oCont.Load()
                oMort.ContractID = oCont.ContractID
                oMort.Load()
                If Not oMort.Number.Contains(oCont.ContractID.ToString) Then
                    oMort.Number = oEQ.Get_Account(oCont.ContractID, False)
                    oMort.UserID = Session("UserDBID")
                    oMort.Save()
                End If
                Dim li = oEQ.LoanInformation(oMort.Number)
                row("AccountNumber") = oMort.Number
                If Not (IsNothing(li)) Then
                    row("Lender") = li.Lender & ""
                    row("Principal Balance") = li.PrincipalBalance & ""
                    row("DaysDelinquent") = li.DaysDelinquent
                    row("PaymentsMade") = li.Term - li.RemainingTerm
                    'li.
                End If
                oCont = Nothing
                oMort = Nothing
                GC.Collect()
            End If
        Next
        oEQ = Nothing

    End Sub

    Private Function xlsInsert(pth As String) As System.Data.DataTable
        Dim strCon As String = ""
        If System.IO.Path.GetExtension(pth).ToLower.Equals(".xlsx") Or System.IO.Path.GetExtension(pth).ToLower.Equals(".xls") Then
            strCon = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & pth & ";Extended Properties=""Excel 12.0;HDR=YES;"""
            Dim strSelect As String = "Select * from " '[Sheet1$]"
            Dim exDT As New System.Data.DataTable
            Using excelCon As New System.Data.OleDb.OleDbConnection(strCon)
                Try
                    excelCon.Open()
                    strSelect &= "[" & excelCon.GetSchema("Tables").Rows(0)("Table_Name") & "]"
                    Using exDA As New System.Data.OleDb.OleDbDataAdapter(strSelect, excelCon)
                        exDA.Fill(exDT)
                    End Using
                Catch ex As Exception
                    lblMessage.Text = ex.Message.ToString & " - In xlsInsert"
                Finally
                    If excelCon.State <> Data.ConnectionState.Closed Then excelCon.Close()
                End Try
                For i = 0 To exDT.Rows.Count - 1
                    If exDT.Rows(i)(0).ToString = String.Empty Or exDT.Rows(i)(0).ToString = "" Then
                        exDT.Rows(i).Delete()
                    End If
                Next
                exDT.AcceptChanges()
                Return exDT
            End Using
        Else
            Return dt
        End If
    End Function

    Private Sub btnUpdateBatch_Click(sender As Object, e As EventArgs) Handles btnUpdateBatch.Click
        If ddBatchType.SelectedValue > 0 And dfBatchHearingDate.Selected_Date <> "" Then
            Dim oB As New clsCancellationBatch
            oB.BatchID = hfBatchID.Value
            oB.Load()
            oB.HearingDate = dfBatchHearingDate.Selected_Date
            oB.PublicationDate1 = dfPublicationDate1.Selected_Date
            oB.PublicationDate2 = dfPublicationDate2.Selected_Date
            If oB.StatusID <> siBatchStatus.Selected_ID Then oB.StatusDate = Date.Now
            oB.StatusID = siBatchStatus.Selected_ID
            oB.TypeID = ddBatchType.SelectedValue
            oB.LienNo = txtInstNum.Text
            oB.Save()
            'lblBatchName.Text = ddBatchType.SelectedItem.Text & " - " & dfBatchHearingDate.Selected_Date
            oB = Nothing
        End If
    End Sub
End Class
