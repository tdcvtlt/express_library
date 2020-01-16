'********************************************************
'   MultiView1 Indices
'   0: Landing Page
'   1: Create Batch
'   2: Upload Excel, csv, or insert contracts
'   3: View Existing Batch
'
'
'
'
'********************************************************
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports ClosedXML.Excel
Imports CrystalDecisions.CrystalReports.Engine
Imports DocumentFormat.OpenXml.Spreadsheet

Partial Class wizards_Accounting_CancellationWiz
    Inherits System.Web.UI.Page
    Dim dt As System.Data.DataTable
    Dim Report As New ReportDocument
    Dim Report2 As New ReportDocument
    Dim sReport As String = "Letters/1st Notice - Reverter.rpt"
    Dim sReport2 As String = "Letters/1st Notice - Reverter.rpt"

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Update_MV1_Index(0)
            'siType.Connection_String = Resources.Resource.cns
            'siType.ComboItem = "ContractSubStatus"
            'siType.Load_Items()
            Dim cn As New SqlConnection(Resources.Resource.cns)
            Dim cm As New SqlCommand("select * from ( select 0 as Comboitemid, '<Choose>' as Comboitem union select i.Comboitemid, i.comboitem from t_ComboItems i inner join t_Combos c on c.ComboID=i.ComboID where c.ComboName='ContractSubStatus' and (i.ComboItem like '%Dev%' or i.ComboItem like '%Rev%' or i.ComboItem like '%Fore%') and i.Active=1 ) a  order by a.Comboitem", cn)
            Dim da As New SqlDataAdapter(cm)
            Dim ds As New DataSet
            da.Fill(ds, "Types")
            ddBatchType.DataSource = ds.Tables("Types")
            ddBatchType.DataTextField = "Comboitem"
            ddBatchType.DataValueField = "ComboitemID"
            ddBatchType.DataBind()
            ddType.DataSource = ds.Tables("Types")
            ddType.DataTextField = "Comboitem"
            ddType.DataValueField = "ComboitemID"
            ddType.DataBind()
            'siBatchType.Connection_String = Resources.Resource.cns
            'siBatchType.ComboItem = "ContractSubStatus"
            'siBatchType.Load_Items()
            siBatchStatus.Connection_String = Resources.Resource.cns
            siBatchStatus.ComboItem = "CancellationBatchStatus"
            siBatchStatus.Load_Items()
            ds = Nothing
            da = Nothing
            cm = Nothing
            cn = Nothing
            hfShowReport.Value = 0
        Else
            lblMessage.Text = ""
        End If
        mvReport.ActiveViewIndex = hfShowReport.Value
        CrystalReportViewer2.Visible = False
        If hfShowReport.Value = 1 Then
            CrystalReportViewer1.ReportSource = Session("Report")
            If IsNothing(Session("Report2")) Then
                CrystalReportViewer2.Visible = False
            Else
                If DirectCast(Session("Report2"), ReportDocument).HasRecords Then
                    CrystalReportViewer2.ReportSource = Session("Report2")
                    CrystalReportViewer2.Visible = True
                Else
                    CrystalReportViewer2.Visible = False
                End If
            End If
        End If
    End Sub
    Protected Sub btnCreate_Click(sender As Object, e As EventArgs) Handles btnCreate.Click
        If dfHearingDate.Selected_Date <> "" And ddType.SelectedValue <> 0 Then ' siType.SelectedName <> "(empty)" Then
            Dim batch As New clsCancellationBatch
            batch.CreatedByID = Session("UserDBID")
            batch.DateCreated = Date.Now
            batch.HearingDate = dfHearingDate.Selected_Date
            batch.StatusDate = Date.Now
            batch.StatusID = (New clsComboItems).Lookup_ID("CancellationBatchStatus", "In Progress")
            batch.TypeID = ddType.SelectedValue ' siType.Selected_ID
            batch.Save()
            hfBatchID.Value = batch.BatchID
            lblMessage.Text = (New clsComboItems).Lookup_ComboItem(batch.TypeID) & " - " & batch.HearingDate
            batch = Nothing
            Update_MV1_Index(1)
        Else
            lblMessage.Text = "Please select both a Type and a Hearing Date"
        End If
    End Sub
    Protected Sub lbUpload_Click(sender As Object, e As EventArgs) Handles lbUpload.Click
        mvFiles.ActiveViewIndex = 0
    End Sub
    Protected Sub lbAddContracts_Click(sender As Object, e As EventArgs) Handles lbAddContracts.Click
        mvFiles.ActiveViewIndex = 1
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
        table.Columns.Add("PayOffAmount")
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
                    row("PayOffAmount") = li.PayoffAmount
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
    Protected Sub lbContinue_Click(sender As Object, e As EventArgs) Handles lbContinue.Click
        lblBatchName.Text = ""
        Update_MV1_Index(2)
        Update_mvContinue(0)
    End Sub
    Protected Sub lbStart_Click(sender As Object, e As EventArgs) Handles lbStart.Click
        lblBatchName.Text = ""
        Update_MV1_Index(0)
    End Sub

    Private Sub Update_mvContinue(index As Integer)
        hfShowReport.Value = 0
        mvReport.ActiveViewIndex = hfShowReport.Value
        mvContinue.ActiveViewIndex = index
        Select Case index
            Case 0
                Dim cn As New SqlConnection(Resources.Resource.cns)
                Dim cm As New SqlCommand("Select batchid as ID, bt.ComboItem + ' - ' + convert(varchar, b.hearingdate,110) as Batch, b.DateCreated, s.comboitem as Status, b.StatusDate from t_CancellationBatch b left outer join t_Comboitems s on s.comboitemid = b.statusid inner join t_Comboitems bt on bt.comboitemid=b.typeid order by batchid desc", cn)
                Dim da As New SqlDataAdapter(cm)
                Dim ds As New DataSet
                Try
                    da.Fill(ds, "Batches")
                    gvContinueContracts.DataSource = ds.Tables("Batches")
                    gvContinueContracts.DataBind()

                Catch ex As Exception
                    lblMessage.Text = ex.Message.ToString
                Finally
                    da = Nothing
                    ds = Nothing
                    cm = Nothing
                    cn = Nothing
                End Try
            Case 1

            Case Else

        End Select
    End Sub

    Private Sub Update_MV1_Index(index As Integer)
        hfShowReport.Value = 0
        mvReport.ActiveViewIndex = hfShowReport.Value
        MultiView1.ActiveViewIndex = index
        Select Case index
            Case 0

            Case 1

            Case 2

            Case Else

        End Select
    End Sub

    Protected Sub btnStart_Click(sender As Object, e As EventArgs) Handles btnStart.Click
        Dim kcp As Integer = 0
        Dim pm As Integer = 0
        Dim dpd As Integer = 0
        Dim pb As Integer = 0
        Dim poa As Integer = 0
        For i = 0 To gvBatch.HeaderRow.Cells.Count - 1
            If gvBatch.HeaderRow.Cells(i).Text.ToLower = "kcp" Then
                kcp = i
            ElseIf Trim(gvBatch.HeaderRow.Cells(i).Text.ToLower) = "principal balance" Then
                pb = i
            ElseIf gvBatch.HeaderRow.Cells(i).Text.ToLower = "daysdelinquent" Then
                dpd = i
            ElseIf gvBatch.HeaderRow.Cells(i).Text.ToLower = "paymentsmade" Then
                pm = i
            ElseIf gvBatch.HeaderRow.Cells(i).Text.ToLower = "payoffamount" Then
                poa = i
            End If
        Next
        For Each row As GridViewRow In gvBatch.Rows

            Dim oCon As New clsContract
            oCon.ContractNumber = row.Cells(kcp).Text
            oCon.Load()
            Dim oMort As New clsMortgage
            oMort.ContractID = oCon.ContractID
            oMort.Load()
            Dim ob2c As New clsCancellationBatch2Contract
            ob2c.AddedByID = Session("UserDBID")
            ob2c.BatchID = hfBatchID.Value
            ob2c.ContractID = oCon.ContractID
            ob2c.DateAdded = Date.Now
            ob2c.NextStep = (New clsComboItems).Lookup_ID("CancellationWizardSteps", "Step 2") 'Step 2
            ob2c.NextStepDate = Date.Today.AddDays(30)
            ob2c.PreviousStep = (New clsComboItems).Lookup_ID("CancellationWizardSteps", "Step 1") 'Step 1
            ob2c.PreviousStepDate = Date.Today
            ob2c.CurrentBalance = If(row.Cells(pb).Text.Trim.Replace("&nbsp;", "") <> "", row.Cells(pb).Text, "0")
            ob2c.DaysPastDueInitial = If(row.Cells(dpd).Text.Trim.Replace("&nbsp;", "") <> "", row.Cells(dpd).Text, "0")
            ob2c.PaymentsMade = If(row.Cells(pm).Text.Trim.Replace("&nbsp;", "") <> "", row.Cells(pm).Text, "0")
            ob2c.PayOffAmount = If(row.Cells(poa).Text.Trim.Replace("&nbsp;", "") <> "", row.Cells(poa).Text, "0")
            If oCon.ContractID <> 0 Then

                ob2c.ContractStatus = oCon.StatusID
                ob2c.ContractSubStatus = oCon.SubStatusID
                ob2c.MortgageStatus = oMort.StatusID
                ob2c.MortgageSubStatus = 0
                ob2c.MaintenanceFeeStatus = oCon.MaintenanceFeeStatusID
                ob2c.Save()

                oCon.StatusDate = Date.Today
                oCon.UserID = Session("UserDBID")
                oCon.StatusID = (New clsComboItems).Lookup_ID("ContractStatus", "On Hold")
                oCon.SubStatusID = ddType.SelectedValue ' siType.Selected_ID

                oCon.Save()
            End If
            oCon = Nothing
            oMort = Nothing
            ob2c = Nothing
            GC.Collect()
        Next
        Update_MV1_Index(2)
        Update_mvContinue(1)
        Update_Steps_View(0)
        Insert_Note(1)
    End Sub
    Protected Sub lbBatch_Click(sender As Object, e As EventArgs) Handles lbBatch.Click
        Update_Steps_View(0)
    End Sub

    Private Sub Update_Steps_View(index As Integer)
        mvSteps.ActiveViewIndex = index
        hfShowReport.Value = 0
        mvReport.ActiveViewIndex = hfShowReport.Value
        Select Case index
            Case 0
                Dim cn As New SqlConnection(Resources.Resource.cns)
                Dim cm As New SqlCommand("Select distinct c.comboitem, cb.NextStepDate, c.Description from t_CancellationBatch2Contract cb inner join t_Comboitems c on c.comboitemid=cb.nextstep where cb.batchid=" & hfBatchID.Value & " order by c.comboitem desc", cn)
                Dim da As New SqlDataAdapter(cm)
                Dim ds As New DataSet
                da.Fill(ds, "Status")

                Dim ob As New clsCancellationBatch

                ob.BatchID = hfBatchID.Value
                ob.Load()
                txtNextStep.Text = If(ds.Tables("Status").Rows.Count > 0, ds.Tables("Status").Rows(0)("Comboitem"), "Unknown")
                txtNextStepDate.Text = If(ds.Tables("Status").Rows.Count > 0, ds.Tables("Status").Rows(0)("NextStepDate"), "Unknown")
                lblNextStep.Text = If(ds.Tables("Status").Rows.Count > 0, "(" & ds.Tables("Status").Rows(0)("Description") & ")", "")
                ddBatchType.SelectedValue = ob.TypeID
                dfBatchHearingDate.Selected_Date = ob.HearingDate
                dfPublicationDate1.Selected_Date = ob.PublicationDate1
                dfPublicationDate2.Selected_Date = ob.PublicationDate2
                dfPublicationDate3.Selected_Date = ob.PublicationDate3
                dfPublicationDate4.Selected_Date = ob.PublicationDate4
                siBatchStatus.Selected_ID = ob.StatusID
                txtInstNum.Text = ob.LienNo
                ob = Nothing
                ds = Nothing
                da = Nothing
                cm = Nothing
                cn = Nothing
            Case 1
                Dim cn As New SqlConnection(Resources.Resource.cns)
                Dim cm As New SqlCommand("Select * From t_CancellationBatch2Contract where batchid=" & hfBatchID.Value, cn)
                Dim da As New SqlDataAdapter(cm)
                Dim ds As New DataSet
                Try
                    cm.CommandText = Get_CommandText(ddBatchType.SelectedItem.Text)
                    da.Fill(ds, "Contracts")
                    gvBatchContracts.DataSource = ds.Tables("Contracts")
                    gvBatchContracts.DataBind()

                Catch ex As Exception
                Finally
                    ds = Nothing
                    da = Nothing
                    cm = Nothing
                    cn = Nothing
                End Try
            Case 2
                Events.KeyField = "CancellationBatchID"
                Events.KeyValue = hfBatchID.Value
                Events.List()
            Case Else

        End Select

    End Sub

    Private Function Get_Note(index As Integer) As String
        Dim ret As String = ""
        Dim ob As New clsCancellationBatch
        ob.BatchID = hfBatchID.Value
        ob.Load()
        Select Case (New clsComboItems).Lookup_ComboItem(ob.TypeID)
            Case "Developer", "Dev-Default"
                Select Case index
                    Case 1
                    Case 2
                        ret = "Initial Developer Default mailed (final date " & ob.HearingDate & ")"
                    Case 3
                        ret = "Certified Developer Default mailed (final date " & ob.HearingDate & ")"
                    Case 4
                        ret = "CXL due to Developer Default (final date " & ob.HearingDate & ")"
                    Case 5
                    Case 6
                    Case Else

                End Select
            Case "Reverter"
                Select Case index
                    Case 1
                        ret = ""
                    Case 2
                        ret = "Initial Reverter mailed (final date " & ob.HearingDate & ")"
                    Case 3
                        ret = "Certified Reverter mailed (final date " & ob.HearingDate & ")"
                    Case 4
                        ret = "CXL due to Reverter (final date " & ob.HearingDate & ")"
                    Case 5
                    Case 6
                    Case Else

                End Select
            Case "Dev-Reverter"
                Select Case index
                    Case 1
                        ret = ""
                    Case 2
                        ret = "Initial Developer Reverter mailed (final date " & ob.HearingDate & ")"
                    Case 3
                        ret = "Certified Developer Reverter mailed (final date " & ob.HearingDate & ")"
                    Case 4
                        ret = "CXL due to Developer Reverter (final date " & ob.HearingDate & ")"
                    Case 5
                    Case 6
                    Case Else

                End Select
            Case "MF Foreclosure"
                Select Case index
                    Case 1
                        ret = "Initial MF Foreclosure mailed (final date " & ob.HearingDate & ")"
                    Case 2
                        ret = "Lien filed (" & txtInstNum.Text & ") and initial MF FC mailed (final date " & ob.HearingDate & ")"
                    Case 3
                        ret = "Certified MF FC mailed (final date " & ob.HearingDate & ")"
                    Case 4
                        ret = "CXL due to MF FC (final date " & ob.HearingDate & ")"
                    Case 5
                    Case 6
                    Case Else

                End Select
            Case "Foreclosure"
                Select Case index
                    Case 1
                    Case 2
                        ret = "Initial FC mailed (final date " & ob.HearingDate & ")"
                    Case 3
                        ret = "Certified FC mailed (final date " & ob.HearingDate & ")"
                    Case 4
                        ret = "CXL due to FC (final date " & ob.HearingDate & ")"
                    Case 5
                    Case 6
                    Case Else

                End Select
            Case "Dev-Foreclosure"
                Select Case index
                    Case 1
                    Case 2
                        ret = "Initial Developer FC mailed (final date " & ob.HearingDate & ")"
                    Case 3
                        ret = "Certified Developer FC mailed (final date " & ob.HearingDate & ")"
                    Case 4
                        ret = "CXL due to Developer FC (final date " & ob.HearingDate & ")"
                    Case 5
                    Case 6
                    Case Else

                End Select
        End Select
        Return ret
    End Function

    Private Sub Show_Report(letter As Integer)
        Dim ob As New clsCancellationBatch
        ob.BatchID = hfBatchID.Value
        ob.Load()
        Session("Report") = Nothing
        Select Case (New clsComboItems).Lookup_ComboItem(ob.TypeID)
            Case "Developer", "Dev-Default"
                Select Case letter
                    Case 1
                        sReport = "Letters/Developer/1st Notice - Developer Not Recorded.rpt"
                        sReport2 = "Letters/Reverter/2nd Notice - Reverter-Bankruptcy.rpt"
                    Case 2
                        sReport = "Letters/Developer/2nd Notice - Developer Not Recorded.rpt"
                        sReport2 = "Letters/Reverter/2nd Notice - Reverter-Bankruptcy.rpt"
                    Case 3
                        sReport = "Letters/Developer/Post Developer CXL Confirmation.rpt"
                        sReport2 = "Letters/Reverter/2nd Notice - Reverter-Bankruptcy.rpt"
                    Case 4
                        sReport = "Letters/Developer/Post Developer CXL Confirmation.rpt"
                        sReport2 = "Letters/Reverter/2nd Notice - Reverter-Bankruptcy.rpt"
                    Case Else
                        sReport = "Letters/Developer/1St Notice - Reverter.rpt"
                        sReport2 = "Letters/Reverter/2nd Notice - Reverter-Bankruptcy.rpt"
                End Select

            Case "Reverter", "Dev-Reverter"
                Select Case letter
                    Case 1
                        sReport = "Letters/Reverter/1St Notice - Reverter.rpt"
                        sReport2 = "Letters/Reverter/1St Notice - Reverter-Bankruptcy.rpt"
                    Case 2
                        sReport = "Letters/Reverter/2nd Notice - Reverter.rpt"
                        sReport2 = "Letters/Reverter/2nd Notice - Reverter-Bankruptcy.rpt"
                    Case 3
                        sReport = "Letters/Reverter/Form - Reverter Affidavit And Release.rpt"
                        sReport2 = "Letters/Reverter/2nd Notice - Reverter-Bankruptcy.rpt"
                    Case 4
                        sReport = "Letters/Reverter/Form - Reverter Affidavit And Release.rpt"
                        sReport2 = "Letters/Reverter/2nd Notice - Reverter-Bankruptcy.rpt"
                    Case Else
                        sReport = "Letters/1St Notice - Reverter.rpt"
                        sReport2 = "Letters/Reverter/2nd Notice - Reverter-Bankruptcy.rpt"
                End Select
            Case "MF Foreclosure"
                Select Case letter
                    Case 1
                        sReport = "Letters/MFForeclosure/Initial Letter 1st Owner.rpt"
                        sReport2 = "Letters/Foreclosure/KCP Initial Demand Letter-Bankruptcy.rpt"
                    Case 2
                        sReport = "Letters/MFForeclosure/KVF Demand 1St Person.rpt"
                        sReport2 = "Letters/MFForeclosure/KVF Demand 1St Person.rpt"
                    Case 3
                        sReport = "Letters/MFForeclosure/Trustee Deed - POA.rpt"
                        sReport2 = "Letters/Foreclosure/KVF Demand 2nd Person-Bankruptcy.rpt"
                    Case 4
                        sReport = "Letters/MFForeclosure/Legal Advertisement - MF FC.rpt"
                        sReport2 = "Letters/MFForeclosure/Legal Advertisement - MF FC.rpt"
                    Case Else
                        sReport = "Letters/Foreclosure/KCP Initial Demand Letter.rpt"
                        sReport2 = "Letters/Foreclosure/KCP Initial Demand Letter-Bankruptcy.rpt"
                End Select
            Case "Foreclosure", "Dev-Foreclosure"
                Select Case letter
                    Case 1
                        sReport = "Letters/Foreclosure/KCP Initial Demand Letter.rpt"
                        sReport2 = "Letters/Foreclosure/KCP Initial Demand Letter-Bankruptcy.rpt"
                    Case 2
                        sReport = "Letters/Foreclosure/KVF Demand 1St Person.rpt"
                        sReport2 = "Letters/Foreclosure/KVF Demand 1St Person-Bankruptcy.rpt"
                    Case 3
                        sReport = "Letters/Foreclosure/Trustee Deed-KVF.rpt"
                        sReport2 = "Letters/Foreclosure/KVF Demand 2nd Person-Bankruptcy.rpt"
                    Case 4
                        sReport = "Letters/Foreclosure/Legal Advertisement.rpt"
                        sReport2 = "Letters/Foreclosure/Legal Advertisement.rpt"
                    Case Else
                        sReport = "Letters/Foreclosure/KCP Initial Demand Letter.rpt"
                        sReport2 = "Letters/Foreclosure/KCP Initial Demand Letter-Bankruptcy.rpt"
                End Select
            Case Else
                Select Case letter
                    Case 1
                        sReport = "Letters/1st Notice - Reverter.rpt"
                        sReport2 = "Letters/Reverter/2nd Notice - Reverter-Bankruptcy.rpt"
                    Case 2
                        sReport = "Letters/1St Notice - Reverter.rpt"
                        sReport2 = "Letters/Reverter/2nd Notice - Reverter-Bankruptcy.rpt"
                    Case 3
                        sReport = "Letters/1St Notice - Reverter.rpt"
                        sReport2 = "Letters/Reverter/2nd Notice - Reverter-Bankruptcy.rpt"
                    Case 4
                        sReport = "Letters/1St Notice - Reverter.rpt"
                        sReport2 = "Letters/Reverter/2nd Notice - Reverter-Bankruptcy.rpt"
                    Case Else
                        sReport = "Letters/1St Notice - Reverter.rpt"
                        sReport2 = "Letters/Reverter/2nd Notice - Reverter-Bankruptcy.rpt"
                End Select
        End Select
        Session("Report") = Nothing
        Session("Report2") = Nothing
        Setup_Report()
        CrystalReportViewer1.ReportSource = Session("Report")
        CrystalReportViewer2.ReportSource = Session("Report2")
        hfShowReport.Value = 1
        mvReport.ActiveViewIndex = hfShowReport.Value
    End Sub

    Private Sub Setup_Report()
        If Session("Report") Is Nothing Then
            Report.Load(Server.MapPath(sReport))
            Report.FileName = Server.MapPath(sReport)
            Report2.Load(Server.MapPath(sReport2))
            Report2.FileName = Server.MapPath(sReport2)
            'Response.Write("HERE")
            'Dim categoryID As Integer = Convert.ToInt32(ddlCategory.SelectedValue)
            Report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
            Report2.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
            'Report.DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
            Set_Params()
            Session.Add("Report", Report)
            Session.Add("Report2", Report2)
        Else
            Report = Session("Report")
            If Report.FileName <> Server.MapPath(sReport) Then
                Session("Report") = Nothing
                Setup_Report()
            End If
        End If
    End Sub

    Protected Sub Set_Params()
        Report.SetParameterValue("BatchID", hfBatchID.Value)
        Report2.SetParameterValue("BatchID", hfBatchID.Value)
    End Sub

    Protected Sub lbContracts_Click(sender As Object, e As EventArgs) Handles lbContracts.Click
        Update_Steps_View(1)
    End Sub

    Protected Sub lbEvents_Click(sender As Object, e As EventArgs) Handles lbEvents.Click
        Update_Steps_View(2)
    End Sub

    Protected Sub gvContinueContracts_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvContinueContracts.SelectedIndexChanged
        hfBatchID.Value = gvContinueContracts.SelectedRow.Cells(1).Text
        Update_MV1_Index(2)
        Update_mvContinue(1)
        Update_Steps_View(0)
        lblBatchName.Text = gvContinueContracts.SelectedRow.Cells(2).Text
    End Sub
    Protected Sub btnUpdateBatch_Click(sender As Object, e As EventArgs) Handles btnUpdateBatch.Click
        If ddBatchType.SelectedValue > 0 And dfBatchHearingDate.Selected_Date <> "" Then
            Dim oB As New clsCancellationBatch
            oB.BatchID = hfBatchID.Value
            oB.Load()
            oB.HearingDate = dfBatchHearingDate.Selected_Date
            oB.PublicationDate1 = dfPublicationDate1.Selected_Date
            oB.PublicationDate2 = dfPublicationDate2.Selected_Date
            oB.PublicationDate3 = dfPublicationDate3.Selected_Date
            oB.PublicationDate4 = dfPublicationDate4.Selected_Date
            If oB.StatusID <> siBatchStatus.Selected_ID Then oB.StatusDate = Date.Now
            oB.StatusID = siBatchStatus.Selected_ID
            oB.TypeID = ddBatchType.SelectedValue
            oB.LienNo = txtInstNum.Text
            oB.Save()
            lblBatchName.Text = ddBatchType.SelectedItem.Text & " - " & dfBatchHearingDate.Selected_Date
            oB = Nothing
        End If
    End Sub
    Protected Sub lbAdd_Click(sender As Object, e As EventArgs) Handles lbAdd.Click
        Update_MV1_Index(1)
    End Sub
    Protected Sub btnStep1_Click(sender As Object, e As EventArgs) Handles btnStep1.Click
        Show_Report(1)
    End Sub
    Protected Sub btnCertified_Click(sender As Object, e As EventArgs) Handles btnCertified.Click
        Show_Report(2)
    End Sub

    Protected Sub btnAffidavit_Click(sender As Object, e As EventArgs) Handles btnAffidavit.Click
        Show_Report(3)
    End Sub

    Private Sub Insert_Note(index As Integer, Optional ContractID As Integer = 0)
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("insert into t_Note select 'ContractID' as Keyfield, contractid as keyvalue, '" & Get_Note(index) & "' as Note, GETDATE() as Datecreated, " & Session("UserDBID") & " as CreatedByID, '' as CreatedBy from t_CancellationBatch2Contract where dateremoved is null and BatchID = " & hfBatchID.Value & IIf(ContractID > 0, " and ContractID=" & ContractID, ""), cn)
        cn.Open()
        cm.ExecuteNonQuery()
        cn.Close()
        cm = Nothing
        cn = Nothing
    End Sub

    Protected Sub btnNextStep_Click(sender As Object, e As EventArgs) Handles btnNextStep.Click
        Select Case txtNextStep.Text
            'Step 1 = change status to on-hold and sub-status to match selected value
            Case "Step 2" '= Print 60 Day Letter
                Step2()
                Update_Steps_View(0)
                'Print 60 Day Letter
                Show_Report(2)
                Insert_Note(2)
            Case "Step 3" '= Print Certified Letter 
                Step3()
                Update_Steps_View(0)
                'Print Certified Letter
                Show_Report(3)
                Insert_Note(3)
            Case "Step 4" '= Cancel Accounts and Print Affidavit
                Step4()
                Update_Steps_View(0)
                'Print Affidavit
                Show_Report(4)
                Insert_Note(4)
            Case "Step 5"
                Update_Records(6)
                Update_Steps_View(0)
                Response.Redirect("UpdateContracts.aspx?id=" & hfBatchID.Value)
            Case "Step 6" '= Release inventory if recorded
                Step6()
                Update_Steps_View(0)
            Case Else

        End Select

    End Sub

    Private Sub Step6()
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("Select * from t_CancellationBatch2Contract where NextStep=" & (New clsComboItems).Lookup_ID("CancellationWizardSteps", "Step 6") & " And dateremoved Is null And batchid=" & hfBatchID.Value, cn)
        Dim da As New SqlDataAdapter(cm)
        Dim ds As New DataSet
        da.Fill(ds, "Contracts")
        For Each row In ds.Tables("Contracts").Rows
            cm.CommandText = "Select * from t_SoldInventory s inner join t_SalesInventory2ContractHist h On h.ContractID=s.ContractID And h.SalesInventoryID=s.SalesInventoryID where h.Active=1 And h.ContractID=" & row("ContractID")
            If Not (IsNothing(ds.Tables("Inv"))) Then ds.Tables("Inv").Clear()
            da.Fill(ds, "Inv")
            For Each item In ds.Tables("Inv").Rows
                Dim oHist As New clsSalesInventory2ContractHist
                oHist.SalesInventoryContractHistID = item("SalesInventoryContractHistID")
                oHist.UserID = Session("UserDBID")
                oHist.Load()
                oHist.HideFromContract = False
                oHist.Active = False
                oHist.DateRemoved = Date.Now
                oHist.Save()
                Dim osi As New clsSoldInventory
                osi.ContractID = oHist.ContractID
                osi.SalesInventoryID = oHist.SalesInventoryID
                osi.SoldInventoryID = 0
                osi.Load()
                'Response.Write(osi.Error_Message)
                'Response.Write("<br>")
                'Response.Write(oHist.ContractID)
                'Response.Write("<br>")
                'Response.Write(oHist.SalesInventoryID)
                'Response.Write("<br>")
                'Response.Write(osi.SoldInventoryID)
                osi.Delete()
                'Response.Write("<br>")
                'Response.Write(osi.Error_Message)
                'Response.Write("<br>")
                osi = Nothing
                oHist = Nothing
                GC.Collect()
            Next
        Next
        ds = Nothing
        da = Nothing
        cm = Nothing
        cn = Nothing
        'Update Records
        Update_Records(7)
    End Sub

    Private Sub Step4()
        'Cancel Accounts 
        Dim mortStatus As Integer = (New clsComboItems).Lookup_ID("MortgageStatus", ddBatchType.SelectedItem.Text)
        Dim conStatus As Integer = (New clsComboItems).Lookup_ID("ContractStatus", "Canceled")
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("Select * from t_CancellationBatch2Contract where NextStep=" & (New clsComboItems).Lookup_ID("CancellationWizardSteps", "Step 4") & " And dateremoved Is null And batchid=" & hfBatchID.Value, cn)
        Dim da As New SqlDataAdapter(cm)
        Dim ds As New DataSet
        da.Fill(ds, "Contracts")
        For Each row In ds.Tables("Contracts").Rows
            Dim oC As New clsContract
            Dim oM As New clsMortgage
            oC.ContractID = row("ContractID")
            oC.Load()
            oC.StatusID = conStatus
            oC.StatusDate = Date.Now
            oC.UserID = Session("UserDBID")
            oC.Save()
            oM.ContractID = row("ContractID")
            oM.Load()
            oM.UserID = Session("UserDBID")
            oM.StatusID = mortStatus
            oM.StatusDate = Date.Now
            oM.Save()
            oC = Nothing
            oM = Nothing
            GC.Collect()
        Next
        ds = Nothing
        da = Nothing
        cm = Nothing
        cn = Nothing
        'Update Records
        Update_Records(5)
    End Sub

    Private Sub Step3()
        'Update Records
        Dim oCont As clsContract
        Dim oMort As clsMortgage
        Dim oEQ As New clsEquiant

        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("Select * from t_CancellationBatch2Contract where batchid = " & hfBatchID.Value, cn)
        Dim da As New SqlDataAdapter(cm)
        Dim ds As New DataSet
        da.Fill(ds, "Contracts")
        For Each row In ds.Tables("Contracts").Rows
            If row(0) & "" <> "" Then
                oCont = New clsContract
                oMort = New clsMortgage
                oCont.ContractID = row("ContractID")
                oCont.Load()
                oMort.ContractID = oCont.ContractID
                oMort.Load()
                If Not oMort.Number.Contains(oCont.ContractID.ToString) Then
                    oMort.Number = oEQ.Get_Account(oCont.ContractID, False)
                    oMort.UserID = Session("UserDBID")
                    oMort.Save()
                End If
                Dim li = oEQ.LoanInformation(oMort.Number)

                If Not (IsNothing(li)) Then
                    Dim oCB As New clsCancellationBatch2Contract
                    oCB.Batch2ContractID = row("Batch2ContractID")
                    oCB.Load()
                    oCB.LenderCode = li.Lender & ""
                    oCB.CurrentBalance = li.PrincipalBalance & ""
                    oCB.DaysPastDueInitial = li.DaysDelinquent
                    oCB.PaymentsMade = li.Term - li.RemainingTerm
                    oCB.PayOffAmount = li.PayoffAmount
                    'li.
                    oCB.Save()
                    oCB = Nothing
                End If

                oCont = Nothing
                oMort = Nothing
                GC.Collect()
            End If
        Next
        ds = Nothing
        da = Nothing
        cm = Nothing
        cn = Nothing
        oEQ = Nothing
        Update_Records(4)
    End Sub

    Private Sub Step2()
        'Update Records
        Update_Records(3)
    End Sub

    Private Sub Update_Records(nextStep As Integer)
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("Update t_CancellationBatch2Contract Set NextStep= " & (New clsComboItems).Lookup_ID("CancellationWizardSteps", "Step " & nextStep) & ", PreviousStep =" & (New clsComboItems).Lookup_ID("CancellationWizardSteps", "Step " & nextStep - 1) & ", PreviousStepDate = getdate(), NextStepDate = DateAdd(dd, 30, getdate()) where dateremoved Is null And batchid=" & hfBatchID.Value & " And nextStep=" & (New clsComboItems).Lookup_ID("CancellationWizardSteps", "Step " & nextStep - 1), cn)
        cn.Open()
        cm.ExecuteNonQuery()
        If nextStep = 6 Then 'Update the status to complete
            Dim oB As New clsCancellationBatch
            oB.BatchID = hfBatchID.Value
            oB.Load()
            oB.StatusDate = Date.Now
            oB.StatusID = (New clsComboItems).Lookup_ID("CancellationBatchStatus", "Complete")
            oB.Save()
            oB = Nothing
            cm.CommandText = "Update t_CancellationBatch2Contract Set NextStep= " & (New clsComboItems).Lookup_ID("CancellationWizardSteps", "Complete") & ", PreviousStep =" & (New clsComboItems).Lookup_ID("CancellationWizardSteps", "Step " & nextStep - 1) & ", PreviousStepDate = getdate(), NextStepDate = DateAdd(dd, 30, getdate()) where dateremoved Is null And batchid=" & hfBatchID.Value & " And nextStep=" & (New clsComboItems).Lookup_ID("CancellationWizardSteps", "Step " & nextStep - 1)
            cm.ExecuteNonQuery()
        End If
        cn.Close()
        cm = Nothing
        cn = Nothing

    End Sub
    Protected Sub gvBatchContracts_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvBatchContracts.SelectedIndexChanged

    End Sub
    Protected Sub lbExcel_Click(sender As Object, e As EventArgs) Handles lbExcel.Click
        Dim xlWB As New XLWorkbook
        Dim res As HttpResponse = Response
        Loop_Records(gvBatchContracts, xlWB.Worksheets.Add("Charges"))
        res.Clear()
        res.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        res.AddHeader("content-disposition", "attachment;filename=""EquiantCharges.xlsx""")
        Using mem As New MemoryStream
            xlWB.SaveAs(mem)
            mem.WriteTo(res.OutputStream)
            mem.Close()
        End Using
        res.End()
    End Sub

    Private Sub Loop_Records(gv As GridView, ByRef ws As IXLWorksheet)
        Dim row As Integer = 1
        Dim col As Integer = 1
        For i = 1 To gv.HeaderRow.Cells.Count - 1
            ws.Cell(row, i).SetValue(gv.HeaderRow.Cells(i).Text)
        Next
        row += 1
        For Each kcp As GridViewRow In gv.Rows
            For i = 1 To kcp.Cells.Count - 1
                ws.Cell(row, i).SetValue(kcp.Cells(i).Text.Replace("&nbsp;", ""))
                If ddBatchType.SelectedItem.Text = "MF Foreclosure" Then
                    If i > 20 And i < 36 Then ws.Cell(row, i).SetDataType(XLDataType.Number)
                    If i = 38 Or i = 40 Or i = 45 Then ws.Cell(row, i).SetDataType(XLDataType.DateTime)
                Else
                    If i = 4 Or i = 6 Or i = 7 Then ws.Cell(row, i).SetDataType(XLDataType.Number)
                    If (i > 26 And i < 31) Or i = 33 Then ws.Cell(row, i).SetDataType(XLDataType.DateTime)

                End If
            Next
            row += 1
        Next

    End Sub
    Protected Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If txtKCP.Text <> "" Then
            Dim dt As New DataTable
            dt.Columns.Add("KCP")
            Dim row As DataRow = dt.NewRow
            row("KCP") = txtKCP.Text
            dt.Rows.Add(row)
            Get_Equiant_Information(dt)
            row = dt.Rows(0)
            Dim oCon As New clsContract
            oCon.ContractNumber = txtKCP.Text
            oCon.Load()
            Dim oMort As New clsMortgage
            oMort.ContractID = oCon.ContractID
            oMort.Load()
            Dim ob2c As New clsCancellationBatch2Contract
            ob2c.AddedByID = Session("UserDBID")
            ob2c.BatchID = hfBatchID.Value
            ob2c.ContractID = oCon.ContractID
            ob2c.DateAdded = Date.Now
            ob2c.NextStep = (New clsComboItems).Lookup_ID("CancellationWizardSteps", "Step 2") 'Step 2
            ob2c.NextStepDate = Date.Today.AddDays(30)
            ob2c.PreviousStep = (New clsComboItems).Lookup_ID("CancellationWizardSteps", "Step 1") 'Step 1
            ob2c.PreviousStepDate = Date.Today
            ob2c.LenderCode = row("Lender") & ""
            ob2c.CurrentBalance = If(row("Principal Balance") Is System.DBNull.Value, 0, row("Principal Balance"))
            ob2c.DaysPastDueInitial = If(row("DaysDelinquent") Is System.DBNull.Value, 0, row("DaysDelinquent"))
            ob2c.PaymentsMade = If(row("PaymentsMade") Is System.DBNull.Value, 0, row("PaymentsMade"))
            If oCon.ContractID <> 0 Then
                ob2c.ContractStatus = oCon.StatusID
                ob2c.ContractSubStatus = oCon.SubStatusID
                ob2c.MortgageStatus = oMort.StatusID
                ob2c.MaintenanceFeeStatus = oCon.MaintenanceFeeStatusID
                ob2c.Save()
                oCon.StatusDate = Date.Today
                oCon.UserID = Session("UserDBID")
                oCon.StatusID = (New clsComboItems).Lookup_ID("ContractStatus", "On Hold")
                oCon.SubStatusID = ddType.SelectedValue 'siType.Selected_ID
                oCon.Save()
                Insert_Note(2, oCon.ContractID)
            End If
            oCon = Nothing
            oMort = Nothing
            ob2c = Nothing
            GC.Collect()
        End If
    End Sub

    Private Function Get_CommandText(Optional sCommand As String = "") As String
        Dim ret As String = ""
        If sCommand = "MF Foreclosure" Then
            ret = "select Equiant, KCP, Name1, Name2, Name3, Name4, StreetAddress, Address, City, State, PostalCode, StreetAddress2, Address2, City2, State2, PostalCode2, Unit as [Time-Share Estate No.], Week, Freq as Frequency, " & _
                        "Interval,[Due as of 2/1/17], [Due as of 2/1/16], [Due as of 2/1/15], [Due as of 2/1/14], [Due as of 2/1/13], [Due as of 2/1/12], [Due as of 2/1/11], [Due as of 2/1/10], LF, [Return Check Fee], " & _
                        "Legal, Admin, Interest, [Special Assessment], " & _
                        "Convert(Varchar(50),cast([Due as of 2/1/17] as Money) + cast([Due as of 2/1/16] as Money) + cast([Due as of 2/1/15] as Money) + cast([Due as of 2/1/14] as Money) + cast([Due as of 2/1/13] as Money) + cast([Due as of 2/1/12] as Money) + cast([Due as of 2/1/11] as Money) + cast([Due as of 2/1/10] as Money) + cast(LF as Money) + cast([Return Check Fee] as Money) + cast(Legal as Money) + cast(Admin as Money) + cast(Interest as Money) + cast([Special Assessment] as Money),1) as [Lien Total], " & _
                        "Convert(Varchar(50),cast([Due as of 2/1/17] as Money) + cast([Due as of 2/1/16] as Money) + cast([Due as of 2/1/15] as Money) + cast([Due as of 2/1/14] as Money) + cast([Due as of 2/1/13] as Money) + cast([Due as of 2/1/12] as Money) + cast([Due as of 2/1/11] as Money) + cast([Due as of 2/1/10] as Money) + cast(LF as Money) + cast([Return Check Fee] as Money) + cast(Legal as Money) + cast(Admin as Money) + cast(Interest as Money) + cast([Special Assessment] as Money),1) as [Bid Price], " & _
                        "SalesPrice as [Sales Price], Dated, '' as [Deed Recordation No.],'' as [deed recorded date],'' as [Memo of Lien No.],'' as [Grantor tax (based on bid)],'' as [State Tax (base on value)],'' as [Local tax (base on value)],'' as [Memorandum of Lien Date],'' as [Lien Recorded],'' as [Lien Inst #],'' as [# Of certs],'' as [Clerk's Fee],'' as [Publication (Total $13,320.00)],'' as [$5.18 for each certified mail],'' as [Commish Fee],'' as [Trustee Fees],'' as [Title Search],'' as Comments from (" & _
                        "select  m.Number as Equiant, c.contractnumber as KCP, p.FirstName + ' ' + p.LastName as Name1, " &
                        "(select top 1 case when co.ProspectID=c.ProspectID then p1.SpouseFirstName + ' ' + p1.SpouseLastName else p1.FirstName + ' ' + p1.LastName end from t_Prospect p1 inner join t_ContractCoOwner co on co.ProspectID = p1.prospectid where co.contractid = c.contractid order by co.CoOwnerID) as Name2, " &
                        "(select top 1 case when co.ProspectID=c.ProspectID then p1.SpouseFirstName + ' ' + p1.SpouseLastName else p1.FirstName + ' ' + p1.LastName end from t_Prospect p1 inner join t_ContractCoOwner co on co.ProspectID = p1.prospectid where co.contractid = c.contractid  and co.ProspectID <> c.ProspectID and co.ProspectID not in (select top 1 p1.ProspectID from t_Prospect p1 inner join t_ContractCoOwner co on co.ProspectID = p1.prospectid where co.contractid = c.contractid order by co.CoOwnerID)  order by co.CoOwnerID) as Name3, " &
                        "(select top 1 case when co.ProspectID=c.ProspectID then p1.SpouseFirstName + ' ' + p1.SpouseLastName else p1.FirstName + ' ' + p1.LastName end from t_Prospect p1 inner join t_ContractCoOwner co on co.ProspectID = p1.ProspectID where co.contractid = c.contractid  and co.ProspectID <> c.ProspectID and co.ProspectID not in (select top 1 ProspectID from t_ContractCoOwner where ContractID=c.ContractID and ProspectID <> c.ProspectID order by CoOwnerID)  order by co.CoOwnerID) as Name4, " &
                        "(select top 1 Address1 from t_PRospectAddress where prospectid = c.prospectid and activeflag=1 order by AddressID) as StreetAddress, " &
                        "(select top 1 Address2 from t_PRospectAddress where prospectid = c.prospectid and activeflag=1 order by AddressID) as Address, " &
                        "(select top 1 City from t_PRospectAddress where prospectid = c.prospectid and activeflag=1 order by AddressID) as City, " &
                        "(select top 1 s.comboitem from t_PRospectAddress a left outer join t_ComboItems s on s.ComboItemID=a.StateID where prospectid = c.prospectid and activeflag=1 order by AddressID) as State, " &
                        "(select top 1 PostalCode from t_PRospectAddress where prospectid = c.prospectid and activeflag=1 order by AddressID) as PostalCode, " &
                        "(select top 1 Address1 from t_ProspectAddress where ActiveFlag=1 and prospectid <> c.prospectid and prospectid in (select prospectid from t_ContractCoOwner where contractid=c.contractid) order by AddressID) as StreetAddress2, " &
                        "(select top 1 Address2 from t_ProspectAddress where ActiveFlag=1 and prospectid <> c.prospectid and prospectid in (select prospectid from t_ContractCoOwner where contractid=c.contractid) order by AddressID) as Address2, " &
                        "(select top 1 City from t_ProspectAddress where ActiveFlag=1 and prospectid <> c.prospectid and prospectid in (select prospectid from t_ContractCoOwner where contractid=c.contractid) order by AddressID) as City2, " &
                        "(select top 1 s.comboitem from t_ProspectAddress a left outer join t_ComboItems s on s.ComboItemID=a.StateID where ActiveFlag=1 and prospectid <> c.prospectid and prospectid in (select prospectid from t_ContractCoOwner where contractid=c.contractid) order by AddressID) as State2, " &
                        "(select top 1 PostalCode from t_ProspectAddress where ActiveFlag=1 and prospectid <> c.prospectid and prospectid in (select prospectid from t_ContractCoOwner where contractid=c.contractid) order by AddressID) as PostalCode2, " &
                        "(select top 1 Name1 + case when coalesce(name2,'') <> '' then ', ' + name2 else '' end + case when coalesce(name3,'') <> '' then ', ' + name3 else '' end from ufn_ContractInventory(c.contractid)) as Unit,  " &
                        "(select top 1 Week from ufn_ContractInventory(c.contractid)) as Week, " &
                        "ci.Frequency As Freq,  " &
                        "case when ci.BD = 'Unknown' then " &
                        "    0 " &
                        "else " &
                        "    Case When ci.SaleType='Cottage' then   " &
                        "        Case when ci.Frequency='Annual' then 1 when ci.Frequency='Biennial' then .5 else 1/3 End * CAST(replace(ci.bd,'BD','') AS int)/3  " &
                        "    when ci.SaleType='Townes' then   " &
                        "        Case when ci.frequency = 'Annual' then 1 when ci.Frequency= 'Biennial' then .5 else 1/3 End * CAST(replace(ci.bd,'BD','') AS int)/2  " &
                        "    when ci.SaleType = 'Estates' then  " &
                        "        Case when ci.Frequency='Annual' then CAST(replace(ci.bd,'BD','') AS money)/4 when ci.Frequency='Biennial' then CAST(replace(ci.bd,'BD','') AS money)/8 else CAST(replace(ci.bd,'BD','') AS money)/12 End  " &
                        "    Else  " &
                        "        0  " &
                        "    End " &
                        "End As Interval, 	 " &
                        "Convert(Varchar(50),coalesce((select SUM(Balance) from v_Invoices where invoice = 'MF17' and keyfield='ContractID' and keyvalue = c.ContractID),0), 1) as [Due as of 2/1/17], " &
                        "Convert(Varchar(50),coalesce((select SUM(Balance) from v_Invoices where invoice = 'MF16' and keyfield='ContractID' and keyvalue = c.ContractID),0), 1) as [Due as of 2/1/16], " &
                        "Convert(Varchar(50),coalesce((Select SUM(Balance) from v_Invoices where invoice = 'MF15' and keyfield='ContractID' and keyvalue = c.ContractID),0), 1) as [Due as of 2/1/15], " &
                        "Convert(Varchar(50),coalesce((select SUM(Balance) from v_Invoices where invoice = 'MF14' and keyfield='ContractID' and keyvalue = c.ContractID),0), 1) as [Due as of 2/1/14], " &
                        "Convert(Varchar(50),coalesce((select SUM(Balance) from v_Invoices where invoice = 'MF13' and keyfield='ContractID' and keyvalue = c.ContractID),0), 1) as [Due as of 2/1/13], " &
                        "Convert(Varchar(50),coalesce((select SUM(Balance) from v_Invoices where invoice = 'MF12' and keyfield='ContractID' and keyvalue = c.ContractID),0), 1) as [Due as of 2/1/12], " &
                        "Convert(Varchar(50),coalesce((select SUM(Balance) from v_Invoices where invoice = 'MF11' and keyfield='ContractID' and keyvalue = c.ContractID),0), 1) as [Due as of 2/1/11], " &
                        "Convert(Varchar(50),coalesce((select SUM(Balance) from v_Invoices where invoice = 'MF10' and keyfield='ContractID' and keyvalue = c.ContractID),0), 1) as [Due as of 2/1/10], " &
                        "Convert(Varchar(50),coalesce((select SUM(i.Balance) from v_Invoices i inner join t_LF2MF t on t.LFInvoiceID=i.ID inner join v_Invoices m on m.ID=t.MFInvoiceID where i.invoice = 'Late Fee' and i.keyfield='ContractID' and i.keyvalue = c.ContractID and cast(RIGHT(m.Invoice,2) as int) <= RIGHT(YEAR(getdate()),2)),0), 1) as [LF], " &
                        "Convert(Varchar(50),coalesce((select SUM(Balance) from v_Invoices where invoice = 'NSF' and keyfield='ContractID' and keyvalue = c.ContractID),0), 1) as [Return Check Fee], " &
                        "Convert(Varchar(50),coalesce((select SUM(Balance) from v_Invoices where invoice = 'LegalFee' and keyfield='ContractID' and keyvalue = c.ContractID),0), 1) as [Legal], " &
                        "Convert(Varchar(50),coalesce((select SUM(Balance) from v_Invoices where invoice = 'AdminFee' and keyfield='ContractID' and keyvalue = c.ContractID),0), 1) as [Admin], " &
                        "Convert(Varchar(50),coalesce((select SUM(Balance) from v_Invoices where invoice = 'Interest' and keyfield='ContractID' and keyvalue = c.ContractID),0), 1) as [Interest], " &
                        "Convert(Varchar(50),coalesce((select SUM(Balance) from v_Invoices where invoice = 'SA14' and keyfield='ContractID' and keyvalue = c.ContractID),0), 1) as [Special Assessment], " &
                        "Convert(Varchar(50),m.SalesPrice, 1) as SalesPrice, " &
                        "convert(varchar(10),c.ContractDate,101) As Dated " &
                    "From t_CancellationBatch2Contract bc  " &
                        "inner join t_CancellationBatch cb On cb.BatchID=bc.BatchID  " &
                        "inner Join t_Contract c On c.ContractID = bc.ContractID  " &
                        "inner join t_Prospect p On p.ProspectID = c.ProspectID  " &
                        "inner Join t_Mortgage m On m.ContractID=c.ContractID  " &
                        "Left outer join t_Campaign ca On ca.CampaignID=c.CampaignID  " &
                        "Left outer Join v_ContractInventory ci On ci.ContractID = c.ContractID  " &
                        "Left outer join (Select distinct ContractID, ChargeBack, DeedInstrumentNumber,DeedOfTrustInstrumentNumber,DOTRecDate from v_ContractUserFields) uf On uf.ContractID=c.ContractID " &
                    "Where bc.batchid = " & hfBatchID.Value & " And bc.dateremoved Is null ) a"
        Else
            ret = "select  m.Number as Equiant, c.ContractNumber as KCP, p.FirstName + ' ' + p.LastName as Name1, " &
                                    "   bc.CurrentBalance, bc.LenderCode, m.SalesPrice, m.TotalFinanced As AmountFinanced, " &
                                    "	p.CreditScore As FICOScore, uf.ChargeBack As Chargeback, " &
                                    "	ca.Name As MKTGSource, " &
                                    "	(Select top 1 pe.FirstName + ' ' + pe.LastName from t_PersonnelTrans pt inner join t_Personnel pe on pe.PersonnelID=pt.PersonnelID inner join t_ComboItems t on t.ComboItemID = pt.TitleID where t.ComboItem='TO' and pt.KeyField='ContractID' and pt.KeyValue = c.contractid order by PersonnelTransID desc) as [T/O], " &
                                    "	(Select top 1 pe.FirstName + ' ' + pe.LastName from t_PersonnelTrans pt inner join t_Personnel pe on pe.PersonnelID=pt.PersonnelID inner join t_ComboItems t on t.ComboItemID = pt.TitleID where t.ComboItem='Sales Executive' and pt.KeyField='ContractID' and pt.KeyValue = c.contractid order by PersonnelTransID desc) as [SalesPerson], " &
                                    "	'' as FrontLine, " &
                                    "	'' as InHouse, " &
                                    "	'ST' as PaymentType, " &
                                    "	bc.DaysPastDueInitial, " &
                                    "	bc.PaymentsMade, " &
                                    "    bc.LastPayment, " &
                                    "	bc.InterestPaidThruDate, " &
                                    "    (select top 1 Name1 + case when coalesce(name2,'') <> '' then ', ' + name2 else '' end + case when coalesce(name3,'') <> '' then ', ' + name3 else '' end from ufn_ContractInventory(c.contractid)) as Unit, " &
                                    "   (select top 1 Week from ufn_ContractInventory(c.contractid)) as Week, " &
                                    "	ci.SaleType as Phase, " &
                                    "	REPLACE(ci.BD,'BD','') as BR, " &
                                    "	ci.Frequency as Freq, " &
                                    "   (select top 1 case when Week < 9 then 'White' else 'Red' end from ufn_ContractInventory(c.contractid)) as Season, " &
                                    "   case when ci.SaleType='Cottage' then  " &
                                    "		Case when ci.Frequency='Annual' then 1 when ci.Frequency='Biennial' then .5 else 1/3 End * CAST(replace(ci.bd,'BD','') AS int)/3 " &
                                    "	when ci.SaleType='Townes' then  " &
                                    "		Case when ci.frequency = 'Annual' then 1 when ci.Frequency= 'Biennial' then .5 else 1/3 End * CAST(replace(ci.bd,'BD','') AS int)/2 " &
                                    "	when ci.SaleType = 'Estates' then " &
                                    "		Case when ci.Frequency='Annual' then CAST(replace(ci.bd,'BD','') AS money)/4 when ci.Frequency='Biennial' then CAST(replace(ci.bd,'BD','') AS money)/8 else CAST(replace(ci.bd,'BD','') AS money)/12 End " &
                                    "	Else " &
                                    "		0 " &
                                    "	End As Interval, " &
                                    "	CONVERT(VARCHAR(10),c.ContractDate,101) as ContractDate, " &
                                    "	bc.FirstNotice, " &
                                    "	bc.SecondNotice, " &
                                    "	CONVERT(VARCHAR(10),cb.HearingDate,101) as HearingDate, " &
                                    "	uf.DeedInstrumentNumber As Deed#, " &
                                    "	uf.DeedOfTrustInstrumentNumber As DOTInst#, " &
                                    "	uf.DOTRecDate As DeedDate " &
                                    "from t_CancellationBatch2Contract bc " &
                                        "inner join t_CancellationBatch cb On cb.BatchID=bc.BatchID " &
                                        "inner join t_Contract c On c.ContractID = bc.ContractID " &
                                        "inner join t_Prospect p On p.ProspectID = c.ProspectID " &
                                        "inner join t_Mortgage m On m.ContractID=c.ContractID " &
                                        "left outer join t_Campaign ca On ca.CampaignID=c.CampaignID " &
                                        "left outer join v_ContractInventory2 ci On ci.ContractID = c.ContractID " &
                                        "left outer join (Select distinct ContractID, ChargeBack, DeedInstrumentNumber,DeedOfTrustInstrumentNumber,DOTRecDate from v_ContractUserFields) uf On uf.ContractID=c.ContractID " &
                                    "where bc.batchid = " & hfBatchID.Value & " and bc.dateremoved is null"
        End If
        Return ret
    End Function

    Private Sub btnStart_Command(sender As Object, e As CommandEventArgs) Handles btnStart.Command

    End Sub
    Protected Sub btnLegalAd_Click(sender As Object, e As EventArgs) Handles btnLegalAd.Click
        Show_Report(4)
    End Sub
    Protected Sub btnCertMailCSV_Click(sender As Object, e As EventArgs) Handles btnCertMailCSV.Click
        Generate_Report(0)

    End Sub


    Private Sub Loop_Records(ByRef dr As SqlDataReader, ByRef ws As IXLWorksheet)
        Dim row As Integer = 1
        Dim hasXref As Boolean = False
        If row = 1 Then
            For col = 1 To dr.VisibleFieldCount
                ws.Cell(row, col).SetValue(dr.GetName(col - 1))
            Next
            row += 1
        End If
        While dr.Read
            For col = 1 To dr.VisibleFieldCount

                ws.Cell(row, col).SetValue(dr.Item(col - 1))

            Next
            row += 1

            GC.Collect()
        End While
        dr.Close()
    End Sub


    Private Sub Generate_Report(sql As Integer)
        Dim res As HttpResponse = Response
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("exec sp_BatchDetails " & hfBatchID.Value, cn)
        Dim xlWB As New XLWorkbook
        cm.CommandTimeout = 10000
        cn.Open()

        cm.CommandText = "exec sp_BatchDetails " & hfBatchID.Value
        Loop_Records(cm.ExecuteReader, xlWB.Worksheets.Add("Funding"))


        cn.Close()
        cn = Nothing
        cm = Nothing

        res.Clear()
        res.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"

        res.AddHeader("content-disposition", "attachment;filename=""Details.xlsx""")

        Using mem As New MemoryStream
            xlWB.SaveAs(mem)
            mem.WriteTo(res.OutputStream)
            mem.Close()
        End Using
        res.End()
    End Sub


    Protected Sub btnUpdateContracts_Click(sender As Object, e As EventArgs) Handles btnUpdateContracts.Click
        Response.Redirect("updatecontracts.aspx?id=" & hfBatchID.Value)
    End Sub
End Class
