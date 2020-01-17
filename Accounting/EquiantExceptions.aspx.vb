
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports ClosedXML.Excel

Partial Class Accounting_EquiantExceptions
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim cn As New SqlConnection(Resources.Resource.cns)
            Dim cm As New SqlCommand("Select  case when m.transid is not null then 1 else 0 end as Mapped, m.id as MappedID,e.* from v_EquiantExceptions e left outer join (select * from t_EquiantCodeMapping where active = 1) m on m.transid=e.id  where e.transid not in (select TransID from v_EquiantDuplicates) order by e.Status desc, e.Code,e.SourceCode,e.Category", cn)
            Dim da As New SqlDataAdapter(cm)
            Dim ds As New DataSet
            Try
                da.Fill(ds, "Exceptions")
                cm.CommandText = "Select '' as Process, * from v_EquiantDuplicates order by TransactionID, TransID"
                da.Fill(ds, "Duplicates")
                Session("Exceptions") = ds.Tables("Exceptions")
                Session("Duplicates") = ds.Tables("Duplicates")
                gvExceptions.DataSource = Session("Exceptions")
                'gvExceptions.DataBind()
                gvDuplicates.DataSource = Session("Duplicates")
                'gvDuplicates.DataBind()
                Dim retryCounter As Integer = 0
                While Not (BindGVs()) And retryCounter < 15
                    BindGVs()
                    retryCounter += 1
                End While
            Catch ex As Exception
                If Session("UserName").ToString.ToLower() = "rhill" Then Response.Write(ex.ToString())
            Finally
                If cn.State <> Data.ConnectionState.Closed Then cn.Close()
                ds = Nothing
                da = Nothing
                cm = Nothing
                cn = Nothing
            End Try
        End If
    End Sub

    Private Function BindGVs() As Boolean
        Try
            gvExceptions.DataBind()
            gvDuplicates.DataBind()
            Return True
        Catch ex As SqlException
            If ex.Number = 1205 Then ' DeadLock
                Return False
            End If
            Return False
        End Try
    End Function

    Protected Sub btnExportDuplicates_Click(sender As Object, e As EventArgs) Handles btnExportDuplicates.Click
        GenerateDuplicates_Report()
    End Sub

    Protected Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        Generate_Report()
    End Sub

    Private Sub Loop_Records(ByRef dr As SqlDataReader, ByRef ws As IXLWorksheet)
        Dim row As Integer = 1
        Dim col As Integer = 1
        While dr.Read
            If row = 1 Then
                For col = 1 To dr.VisibleFieldCount
                    ws.Cell(row, col).SetValue(dr.GetName(col - 1))
                Next
                row += 1
            End If
            For col = 1 To dr.VisibleFieldCount
                If dr.GetName(col - 1) = "DueDate" Then
                    Dim val As String = (dr.Item(col - 1) & "")
                    If val.Contains(" ") Then
                        val = val.ToString.Split(" ")(0)
                    End If
                    If val = "" Then
                        val = "2/1/" & dr("TransactionCode").ToString.Substring(4, 4)
                    End If
                    ws.Cell(row, col).SetValue(val)
                ElseIf dr.GetName(col - 1) = "TransactionDate" Or dr.GetName(col - 1) = "StatementDate" Then
                    Dim val As String = (dr.Item(col - 1) & "").ToString.Split(" ")(0)
                    '    ws.Cell(row, col).SetValue(oMort.APR)
                    'ElseIf dr.GetName(col - 1) = "DownPayment" Then
                    '    If dr("Equity") Is System.DBNull.Value Then
                    '        ws.Cell(row, col).SetValue(dr.Item(col - 1))
                    '    Else
                    '        ws.Cell(row, col).SetValue(dr.Item(col - 1) - dr("Equity"))
                    '    End If

                    'ElseIf dr.GetName(col - 1) = "SSN1" Or dr.GetName(col - 1) = "SSN2" Or dr.GetName(col - 1) = "SSN3" Then
                    '    Dim val As String = dr.Item(col - 1) & ""
                    '    val = val.Replace("-", "")
                    '    If val.Length = 9 Then
                    '        val = val.Insert(5, "-").Insert(3, "-")
                    '    End If
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

    Private Sub GenerateDuplicates_Report()
        Dim res As HttpResponse = Response
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("", cn)
        Dim xlWB As New XLWorkbook
        cm.CommandTimeout = 10000
        cn.Open()

        cm.CommandText = "Select e.* from v_EquiantDuplicates e order by  transactionid,transid, Code,SourceCode,Category"
        Loop_Records(cm.ExecuteReader, xlWB.Worksheets.Add("Exceptions"))


        cn.Close()
        cn = Nothing
        cm = Nothing

        res.Clear()
        res.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        res.AddHeader("content-disposition", "attachment;filename=""EquiantExceptions.xlsx""")
        Using mem As New MemoryStream
            xlWB.SaveAs(mem)
            mem.WriteTo(res.OutputStream)
            mem.Close()
        End Using
        res.End()
    End Sub

    Private Sub Generate_Report()
        Dim res As HttpResponse = Response
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("", cn)
        Dim xlWB As New XLWorkbook
        cm.CommandTimeout = 10000
        cn.Open()

        cm.CommandText = "Select e.* from v_EquiantExceptions e order by Status desc, Code,SourceCode,Category"
        Loop_Records(cm.ExecuteReader, xlWB.Worksheets.Add("Exceptions"))


        cn.Close()
        cn = Nothing
        cm = Nothing

        res.Clear()
        res.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        res.AddHeader("content-disposition", "attachment;filename=""EquiantExceptions.xlsx""")
        Using mem As New MemoryStream
            xlWB.SaveAs(mem)
            mem.WriteTo(res.OutputStream)
            mem.Close()
        End Using
        res.End()
    End Sub

    Private Sub gvExceptions_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvExceptions.RowDataBound
        If e.Row.Cells(0).Text = "" Then
            e.Row.Cells(0).Text = "Map One Time"
        ElseIf e.Row.Cells(0).Text = "0" And e.Row.Cells(2).Text = "Not Processed" Then
            e.Row.Cells(0).Text = "<a href=""editequiantonetimemapping.aspx?transid=" & e.Row.Cells(10).Text & "&code=" & e.Row.Cells(5).Text & "&sourcecode=" & e.Row.Cells(6).Text & "&category=" & e.Row.Cells(12).Text & "&posneg=" & If(CDec(e.Row.Cells(13).Text) > 0, 0, 1) & """>Map This One</a>"
        ElseIf e.Row.Cells(0).Text = "0" And e.Row.Cells(2).Text = "Exception" Then
            e.Row.Cells(0).Text = "<a href=""editequiantonetimemapping.aspx?transid=" & e.Row.Cells(10).Text & "&code=" & e.Row.Cells(5).Text & "&sourcecode=" & e.Row.Cells(6).Text & "&category=" & e.Row.Cells(12).Text & "&posneg=" & If(CDec(e.Row.Cells(13).Text) > 0, 0, 1) & """>Map</a>"
            e.Row.Cells(0).Text &= " -- <a href=""editequiantonetimemapping.aspx?id=" & e.Row.Cells(10).Text & "&func=Ignore"">Ignore</a>"
        ElseIf e.Row.Cells(0).Text = "1" And e.Row.Cells(2).Text = "Not Processed" Then
            e.Row.Cells(0).Text = "Already Mapped"
        Else
            e.Row.Cells(0).Text = ""
        End If
        If e.Row.Cells(1).Text = "" Then
            e.Row.Cells(1).Text = "Always Map"
        ElseIf e.Row.Cells(2).Text = "Not Processed" Then
            e.Row.Cells(1).Text = "<a href=""editequiantmapping.aspx?ret=exc&code=" & e.Row.Cells(5).Text & "&sourcecode=" & e.Row.Cells(6).Text & "&category=" & e.Row.Cells(12).Text & "&posneg=" & If(CDec(e.Row.Cells(13).Text) > 0, 0, 1) & """>Map All</a>"
        Else
            e.Row.Cells(1).Text = ""
        End If
    End Sub

    Private Sub gvDuplicates_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvDuplicates.RowDataBound
        If e.Row.Cells(0).Text <> "Process" Then e.Row.Cells(0).Text = "<a href=""editequiantonetimemapping.aspx?id=" & e.Row.Cells(9).Text & "&func=ack"">Acknowledge</a>"
    End Sub

    Protected Sub btnAckAll_Click(sender As Object, e As EventArgs) Handles btnAckAll.Click
        Dim sIDs As String = ""
        For Each row As GridViewRow In gvDuplicates.Rows
            sIDs &= IIf(sIDs = "", "", ",") & row.Cells(9).Text
        Next
        Dim cn As New Data.SqlClient.SqlConnection(Resources.Resource.cns)
        Dim cm As New Data.SqlClient.SqlCommand("Update t_EquiantCategoryDescriptions set dupacknowledged=1 where id in (" & sIDs & ")", cn)
        Try
            cn.Open()
            cm.ExecuteNonQuery()
            cn.Close()
        Catch ex As Exception
        Finally
            If cn.State <> ConnectionState.Closed Then cn.Close()
            cm = Nothing
            cn = Nothing
        End Try
        Response.Redirect("EquiantExceptions.aspx")
    End Sub

    Protected Sub Exceptions_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs)

        'Retrieve the table from the session object.
        Dim dt = TryCast(Session("Exceptions"), DataTable)

        If dt IsNot Nothing Then

            'Sort the data.
            dt.DefaultView.Sort = e.SortExpression & " " & GetSortDirection(e.SortExpression)
            gvExceptions.DataSource = Session("Exceptions")
            gvExceptions.DataBind()

        End If

    End Sub


    Private Function GetSortDirection(ByVal column As String) As String

        ' By default, set the sort direction to ascending.
        Dim sortDirection = "ASC"

        ' Retrieve the last column that was sorted.
        Dim sortExpression = TryCast(ViewState("SortExpression"), String)

        If sortExpression IsNot Nothing Then
            ' Check if the same column is being sorted.
            ' Otherwise, the default value can be returned.
            If sortExpression = column Then
                Dim lastDirection = TryCast(ViewState("SortDirection"), String)
                If lastDirection IsNot Nothing _
                  AndAlso lastDirection = "ASC" Then

                    sortDirection = "DESC"

                End If
            End If
        End If

        ' Save new values in ViewState.
        ViewState("SortDirection") = sortDirection
        ViewState("SortExpression") = column

        Return sortDirection

    End Function
End Class
