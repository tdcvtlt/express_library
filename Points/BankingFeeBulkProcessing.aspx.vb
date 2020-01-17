
Imports System.Data.SqlClient

Partial Class Points_BankingFeeBulkProcessing
    Inherits System.Web.UI.Page
    Dim dt As System.Data.DataTable
    Dim rowID As Integer = 0
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        lblMessage.Text = ""
        If Not (IsPostBack) Then
            lblMessage.Text = "Please make sure that the Contract/KCP Number column in the spreadsheet is formated as Text and not as General.<br />If formated as General, contracts beginning with a letter will not be included."
        End If
    End Sub

    Protected Sub btnUpload_Click(sender As Object, e As EventArgs) Handles btnUpload.Click
        If xlsUpload.HasFile Then
            Dim upload As Boolean = True
            Dim Position As String = ""
            Dim fileUpload As String = System.IO.Path.GetExtension(xlsUpload.FileName.ToString())
            If fileUpload.Trim().ToLower = ".xls" Or fileUpload.Trim.ToLower = ".xlsx" Then
                xlsUpload.SaveAs("\\rs-fs-01\UploadedContracts\misprojects\" + xlsUpload.FileName.ToString)
                Dim uploadedFile As String = "\\rs-fs-01\UploadedContracts\misprojects\" + xlsUpload.FileName.ToString
                Dim strColumnList() As String = {"kcp", "contractid", "contractnumber", "contract_ref_num"}
                Dim strInvoices() As String = {"bank", "rebank", "bank fee", "rebank fee"}
                Dim strCCColumnList() As String = {"pymt_card_num"}
                Try
                    Position = "xlsInsert"
                    dt = xlsInsert(uploadedFile)
                    Dim ds As New Data.DataTable
                    Dim dsReject As New Data.DataTable
                    Position = "Creating Columns"
                    For i = 0 To dt.Columns.Count - 1
                        'If strColumnList.Contains(dt.Columns(i).ColumnName.ToLower) Then
                        ds.Columns.Add(dt.Columns(i).ColumnName)
                        dsReject.Columns.Add(dt.Columns(i).ColumnName)
                        'End If
                    Next
                    Position = "Adding Columns"
                    ds.Columns.Add("FullCC")
                    ds.Columns.Add("Active Points")
                    ds.Columns.Add("Current Mortgages")
                    ds.Columns.Add("Current Conversions")
                    ds.Columns.Add("Owner Financials")
                    ds.Columns.Add("Contract Financials")
                    ds.Columns.Add("Points Avail")
                    ds.Columns.Add("Token")
                    dsReject.Columns.Add("FullCC")
                    dsReject.Columns.Add("Active Points")
                    dsReject.Columns.Add("Current Mortgages")
                    dsReject.Columns.Add("Current Conversions")
                    dsReject.Columns.Add("Owner Financials")
                    dsReject.Columns.Add("Contract Financials")
                    dsReject.Columns.Add("Points Avail")
                    dsReject.Columns.Add("Token")
                    For Each row As Data.DataRow In dt.Rows
                        Dim newRow As Data.DataRow = ds.NewRow
                        Position = "Getting CC for " & newRow("contract_ref_num")
                        For i = 0 To ds.Columns.Count - 1
                            If ds.Columns(i).ColumnName = "FullCC" Then
                                newRow(ds.Columns(i).ColumnName) = GetNumber(row(strCCColumnList(0)))
                                Exit For
                                'ElseIf ds.Columns(i).ColumnName = "expiration_dt" Then
                                '    newRow(ds.Columns(i).ColumnName) = Month(row(ds.Columns(i).ColumnName)) & "/" & Right(Year(row(ds.Columns(i).ColumnName)).ToString(), 2)
                            Else
                                newRow(ds.Columns(i).ColumnName) = row(ds.Columns(i).ColumnName)
                            End If

                        Next
                        If newRow(0) & "" <> "" Then
                            Position = "Sending " & newRow("contract_ref_num") & " to Qualified"
                            Qualified(newRow("contract_ref_num") & "", newRow)
                            If (newRow("Active Points") & "" = "" Or newRow("Current Mortgages") & "" = "" Or newRow("Current Conversions") & "" = "" Or newRow("Owner Financials") & "" = "" Or newRow("Contract Financials") & "" = "" Or newRow("Points Avail") & "" = "" Or newRow("Expiration_Dt") & "" = "") And strInvoices.Contains(newRow("Invoice").ToString.ToLower) Then
                                Dim newRJ As Data.DataRow = dsReject.NewRow
                                For r = 0 To newRJ.ItemArray.Count - 1
                                    newRJ.Item(r) = newRow.Item(r)
                                Next
                                dsReject.Rows.Add(newRJ)
                                newRow = Nothing
                                newRJ = Nothing
                            Else
                                If newRow("Active Points") = "True" And newRow("Current Mortgages") = "True" And newRow("Current Conversions") = "True" And newRow("Owner Financials") = 0 And newRow("Contract Financials") = 0 And newRow("Points Avail") > 0 And CDate(newRow("expiration_dt")) > Date.Today And strInvoices.Contains(newRow("Invoice").ToString.ToLower) Then
                                    ds.Rows.Add(newRow)
                                    newRow = Nothing
                                Else
                                    Dim newRJ As Data.DataRow = dsReject.NewRow
                                    For r = 0 To newRJ.ItemArray.Count - 1
                                        newRJ.Item(r) = newRow.Item(r)
                                    Next
                                    dsReject.Rows.Add(newRJ)
                                    newRow = Nothing
                                    newRJ = Nothing
                                End If
                            End If
                        Else
                            Dim newRJ As Data.DataRow = dsReject.NewRow
                            For r = 0 To newRJ.ItemArray.Count - 1
                                newRJ.Item(r) = newRow.Item(r)
                            Next
                            dsReject.Rows.Add(newRJ)
                            newRow = Nothing
                            newRJ = Nothing
                        End If
                    Next


                    Dim start As String = lblMessage.Text
                    'lblMessage.Text = DateDiff(DateInterval.Second, CDate(start), Date.Now) & " seconds "
                    gvBatch.DataSource = ds
                    gvBatch.DataBind()
                    gvRejects.DataSource = dsReject
                    gvRejects.DataBind()

                    If ds.Rows.Count > 0 Then
                        'btnStart.Enabled = True
                    End If
                    If lblMessage.Text = "" Then lblMessage.Text = gvBatch.Rows.Count & " Qualified Items Loaded<br />" & gvRejects.Rows.Count & " Non-Qualified Items Loaded."
                Catch ex As Exception
                    lblMessage.Text = ex.Message.ToString & " In Uploading() " & Position
                End Try

            End If
        End If
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
                    lblMessage.Text = ex.Message.ToString
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

    Function GetNumber(cc As String) As String
        Dim ret As String = ""
        Dim newNum As String
        Dim tempSum As Integer
        Dim lastDigit As Integer
        Dim bQuit As Boolean
        cc = Replace(cc, ".", "")
        cc = Replace(cc, "E+15", "")
        newNum = cc
        cc = cc & "0"
        While Len(cc) < 16 And CInt(Left(cc, 1)) <> 3
            cc = cc & "0"
        End While
        tempSum = 0

        For x = Len(cc) - 1 To 0 Step -2
            tempSum = tempSum + IIf((CInt(Right(Left(cc, x), 1)) * 2) > 9, CInt(Left((Right(Left(cc, x), 1) * 2), 1)) + CInt(Right(CInt(Right(Left(cc, x), 1)) * 2, 1)), CInt(Right(Left(cc, x), 1)) * 2)
            If x > 1 Then
                tempSum = tempSum + CInt(Right(Left(cc, x - 1), 1))
            End If
        Next

        lastDigit = 0
        bQuit = False
        While (tempSum + lastDigit) Mod 10 <> 0 And Not (bQuit)
            lastDigit = lastDigit + 1
            If lastDigit > 9 Then bQuit = True
        End While
        If Left(cc, 1) = "3" Then
            ret = newNum
        Else
            ret = Left(cc, Len(cc) - 1) & lastDigit
        End If
        Return ret
    End Function

    Private Function Qualified(kcp As String, mdr As Data.DataRow) As Boolean
        Dim ret As Boolean = False
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("", cn)
        Dim da As New SqlDataAdapter(cm)
        Dim ds As New DataSet
        Dim cids As String = ""
        Try
            'Has Active points contract
            cm.CommandText = "Select distinct c.contractid from t_Contract c inner join t_Comboitems cs on cs.comboitemid= c.statusid inner join t_Comboitems cst on cst.comboitemid=c.subtypeid where prospectid in (select prospectid from t_Contract where contractnumber = '" & kcp & "') and cst.comboitem='Points' and cs.comboitem = 'Active'"
            da.Fill(ds, "Contracts")
            For Each row In ds.Tables("Contracts").Rows
                cids &= IIf(cids = "", row("ContractID"), "," & row("ContractID"))
            Next
            mdr("Active Points") = IIf(cids <> "", True, False)

            'Mortgage status in ('Active', 'PIF', 'N/A')
            If cids <> "" Then
                cm.CommandText = "select distinct c.contractid from t_Contract c inner join t_Comboitems cs on cs.comboitemid=c.statusid inner join t_Mortgage m on m.contractid = c.contractid inner join t_Comboitems ms on ms.comboitemid = m.statusid where c.contractid in (" & cids & ") and ms.comboitem in ('Active', 'PIF','N/A')"
                da.Fill(ds, "Mortgages")
                mdr("Current Mortgages") = IIf(ds.Tables("Mortgages").Rows.Count = ds.Tables("Contracts").Rows.Count, True, False)


                'Conversion Mortgage Status in ('Active','PIF')
                cm.CommandText = "select c.contractid from t_Contract c inner join t_Comboitems cs on cs.comboitemid=c.statusid inner join t_Conversion m on m.contractid = c.contractid inner join t_Comboitems ms on ms.comboitemid = m.statusid where c.contractid in (" & cids & ") and ms.comboitem not in ('Active', 'PIF','N/A')"
                da.Fill(ds, "Conversions")
                mdr("Current Conversions") = IIf(ds.Tables("Conversions").Rows.Count > 0, False, True)
            Else
                mdr("Current Mortgages") = False
                mdr("Current Conversions") = True
            End If
            'No Past Due Owner Financials
            cm.CommandText = "Select sum(balance) as Due from v_Invoices where keyfield='ProspectID' and keyvalue in (select prospectid from t_Contract where contractnumber = '" & kcp & "') and duedate <= getdate()"
            da.Fill(ds, "ProsBalance")
            mdr("Owner Financials") = If(ds.Tables("ProsBalance").Rows(0)(0).ToString() & "" = "", FormatCurrency(0), FormatCurrency(ds.Tables("ProsBalance").Rows(0)(0)))

            'No Past Due Contract Financials
            cm.CommandText = "Select sum(balance) as Due from v_Invoices where keyfield='ContractID' and keyvalue in (select contractid from t_Contract where prospectid in (select prospectid from t_Contract where contractnumber = '" & kcp & "')) and duedate <= getdate()"
            da.Fill(ds, "ConBalance")
            mdr("Contract Financials") = If(ds.Tables("ConBalance").Rows(0)(0).ToString() & "" = "", FormatCurrency(0), FormatCurrency(ds.Tables("ConBalance").Rows(0)(0)))

            'Has Points to Bank or Rebank (based on field) from current year
            cm.CommandText = "select sum(balance) from v_PointsTracking p where p.Balance > 0 and TransType in ('Deposit','Bank') and year(ExpirationDate) = YEAR(getdate()) and contractid in (select contractid from t_Contract where prospectid in  (select prospectid from t_Contract where contractnumber = '" & kcp & "'))"
            da.Fill(ds, "PointsBalance")
            mdr("Points Avail") = If(ds.Tables("PointsBalance").Rows(0)(0).ToString() & "" = "", 0, ds.Tables("PointsBalance").Rows(0)(0))
        Catch ex As Exception
            lblMessage.Text = ex.Message.ToString & " In Qualified() for KCP " & kcp
        End Try
        Return ret
    End Function
    Private Sub gvBatch_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvBatch.RowDataBound
        If e.Row.RowType <> DataControlRowType.Header Then
            Dim ccCell As Integer = 0
            For i = 0 To gvBatch.HeaderRow.Cells.Count - 1
                If gvBatch.HeaderRow.Cells(i).Text = "FullCC" Then
                    ccCell = i
                    Exit For
                End If
            Next
            If e.Row.Cells(ccCell).Controls.Count < 1 Then
                Dim sText As String = e.Row.Cells(ccCell).Text
                'e.Row.Cells(ccCell).Text = "Hidden"
                e.Row.Cells(ccCell).Controls.Add(New HiddenField With {.Value = sText, .ID = "hf" & rowID})
                rowID += 1
            End If
        End If
    End Sub
    Protected Sub btnProcess_Click(sender As Object, e As EventArgs) Handles btnProcess.Click
        'Response.Write(CType(gvBatch.Rows(5).Cells(7).FindControl("hf5"), HiddenField).Value)
        Dim ccRow As Integer = 0
        Dim expRow As Integer = 0
        For i = 0 To gvBatch.HeaderRow.Cells.Count - 1
            If gvBatch.HeaderRow.Cells(i).Text = "FullCC" Then
                ccRow = i
                Exit For
            End If
        Next
        For i = 0 To gvBatch.HeaderRow.Cells.Count - 1
            If gvBatch.HeaderRow.Cells(i).Text = "expiration_dt" Then
                expRow = i
                Exit For
            End If
        Next

        Dim scriptText As String = "function proc() {"
        scriptText &= "var cards = ["
        For Each row As GridViewRow In gvBatch.Rows
            scriptText &= "'" & row.Cells(ccRow).Text & "',"
            If row.Cells(expRow).Text <> "" Then
                scriptText &= "'" & If(CDate(row.Cells(expRow).Text).Month < 10, "0" & CDate(row.Cells(expRow).Text).Month.ToString, CDate(row.Cells(expRow).Text).Month.ToString) & "-" & CDate(row.Cells(expRow).Text).Year & "',"
            Else
                scriptText &= "'',"
            End If
        Next
        scriptText = Left(scriptText, Len(scriptText) - 1) & "];"
        scriptText &= "var item=1;"
        scriptText &= "for (i=0;i<cards.length;i+=2){"
        scriptText &= "var item=item+1;var box='';var value='';"
        scriptText &= "if (i>-1){if(cards[i] == cards[i+2]){value='Duplicate';}else{value= cards[i] + ',' + cards[i+1];}}else{value=cards[i] + ',' + cards[i+1];};"
        scriptText &= "if (item<10) {"
        scriptText &= "box = '#ctl00_ContentPlaceHolder1_gvBatch_ctl0' + item + '_txtCC';"
        scriptText &= "}else{"
        scriptText &= "box='#ctl00_ContentPlaceHolder1_gvBatch_ctl' + item + '_txtCC';"
        scriptText &= "};"
        scriptText &= "$(box).val(value);if (value !='Duplicate' && i==0){"
        scriptText &= "var vals = value.split(',');"
        scriptText &= "var exps = vals[1].split('-');"
        scriptText &= ""
        scriptText &= "Tokenize_CC(vals[0], 123, exps[0], exps[1],box);"
        'scriptText &= "Tokenize_CC(value, 123, 12, 16,box);"
        scriptText &= "};"
        scriptText &= "}"
        scriptText &= "}"

        ClientScript.RegisterClientScriptBlock(Me.GetType, "update", scriptText, True)

        Response.Write("<div id='Results'></div>")
        ClientScript.RegisterClientScriptBlock(Me.GetType, "runupdate", "$(document).ready(function(){proc();});", True)
    End Sub

    Protected Sub btnStep2_Click(sender As Object, e As EventArgs) Handles btnStep2.Click
        For Each row As GridViewRow In gvBatch.Rows
            Dim tb As TextBox = row.FindControl("txtCC")
            If Not (IsNothing(tb)) Then
                Response.Write("Found the control with a value of: " & tb.Text & " for contract # : " & row.Cells(1).Text)
                Dim cn As New SqlConnection(Resources.Resource.cns)
                Dim cm As New SqlCommand("Select * from t_CreditCard where number = '" & tb.Text.Split("|")(2) & "' and prospectid = (select prospectid from t_Contract where contractnumber = '" & row.Cells(1).Text & "') and token <> '' and token not like 'supt%'", cn)
                Dim da As New SqlDataAdapter(cm)
                Dim ds As New DataSet
                da.Fill(ds, "CC")
                cm.CommandText = "Select * From t_Prospect where prospectid in (select prospectid from t_Contract where contractnumber = '" & row.Cells(1).Text & "') "
                da.Fill(ds, "Prospect")
                Dim ccID As Integer = 0
                Dim oCont As New clsContract
                Dim oCC As New clsCreditCard
                If ds.Tables("CC").Rows.Count > 0 Then
                    ccID = ds.Tables("CC").Rows(0)("CreditCardID")
                    oCC.CreditCardID = ccID
                    oCC.Load()
                    oCC.Expiration = If(CDate(row.Cells(6).Text).Month < 10, "0" & CDate(row.Cells(6).Text).Month.ToString, CDate(row.Cells(6).Text).Month.ToString) & Right(CDate(row.Cells(6).Text).Year.ToString, 2)
                    oCC.UserID = Session("UserDBID")
                    oCC.Save()
                Else
                    'Add new
                    oCC.CreditCardID = ccID
                    oCC.Load()
                    oCC.Expiration = If(CDate(row.Cells(6).Text).Month < 10, "0" & CDate(row.Cells(6).Text).Month.ToString, CDate(row.Cells(6).Text).Month.ToString) & Right(CDate(row.Cells(6).Text).Year.ToString, 2)
                    oCC.ProspectID = ds.Tables("Prospect").Rows(0)("ProspectID")
                    oCC.Number = tb.Text.Split("|")(2)
                    oCC.CCString = ""
                    oCC.NameOnCard = row.Cells(2).Text
                    oCC.Address = row.Cells(3).Text
                    oCC.PostalCode = If(row.Cells(4).Text.Length = 4, "0" & row.Cells(4).Text, row.Cells(4).Text)
                    oCC.StateID = 0
                    oCC.City = ""
                    oCC.TypeID = (New clsComboItems).Lookup_ID("CreditCardType", tb.Text.Split("|")(1))
                    oCC.ReadyToImport = 1
                    oCC.ImportedID = 0
                    oCC.CRMSID = 0
                    oCC.UpdateCard = 0
                    oCC.Token = tb.Text.Split("|")(0)
                    oCC.UserID = Session("UserDBID")
                    oCC.Save()
                    ccID = oCC.CreditCardID
                End If
                oCont = Nothing
                oCC = Nothing
                ds = Nothing
                da = Nothing
                cm = Nothing
                cn = Nothing
                Response.Write(ccID)
                Exit For
            End If
        Next
    End Sub
End Class
