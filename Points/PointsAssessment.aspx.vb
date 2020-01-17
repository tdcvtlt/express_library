
Imports System.Data
Imports System.IO
Imports ClosedXML.Excel

Partial Class Points_PointsAssessment
    Inherits System.Web.UI.Page

    Protected Sub btnPreview_Click(sender As Object, e As EventArgs) Handles btnPreview.Click
        gvPoints.DataSource = Get_Records()
        gvPoints.DataBind()
        lblErr.Text = gvPoints.Rows.Count & " Contracts Found"
        Toggle_Buttons()
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        lblErr.Text = ""
        If Not (IsPostBack) Then
            Load_Years()
            ddUsageYear.SelectedValue = Year(Date.Today)
            Toggle_Buttons()
        End If
    End Sub

    Private Function Get_Records() As DataTable
        Dim dt As DataTable
        Dim ds As New SqlDataSource(Resources.Resource.cns, Get_SQL(1))
        dt = CType(ds.Select(New DataSourceSelectArguments), DataView).ToTable
        Return dt
    End Function

    Private Function Get_SQL(index As Integer) As String
        Dim SQL As String = ""
        Select Case index
            Case 1
                SQL = "select a.ContractID, a.ContractNumber, coalesce(b.Points,0) as [Previously Assessed], a.Points as [Points Allowed], a.points-coalesce(b.points,0) as [To Be Assessed]  " & _
                        "from ( " & _
                        "   Select c.ContractID, c.ContractNumber, SUM(s.points) points " & _
                        "   From t_Contract c  " & _
                        "	    inner Join t_SoldInventory s on s.ContractID=c.ContractID  " & _
                        "       inner Join t_ComboItems cst on cst.ComboItemID=c.SubTypeID " & _
                        "       inner Join t_ComboItems cs on cs.ComboItemID=c.StatusID " & _
                        "   Where (" & ddUsageYear.SelectedValue & " - s.OccupancyYear) % s.FrequencyID = 0 and year(c.occupancydate)<=" & ddUsageYear.SelectedValue & " And cst.ComboItem='Points' and cs.ComboItem='Active' " & _
                        "   Group by c.contractid, c.ContractNumber " & _
                        ") a left outer join ( " & _
                        "   Select ContractID, sum(points) as Points " & _
                        "   from t_PointsTracking t " & _
                        "       inner join t_ComboItems tt On tt.ComboItemID=t.TransTypeID " & _
                        "   where UsageYear = " & ddUsageYear.SelectedValue & " And tt.ComboItem='Deposit' " & _
                        "   group by contractid " & _
                        ") b on b.ContractID=a.ContractID " ' & _
                ' "where (b.ContractID Is null Or b.Points <> a.points) " 'and a.points>0 "
            Case 2
                SQL = "select 0 as ApplyToID, " & ddUsageYear.SelectedValue & " as AvailYear, '' as Comments, '' as ConfirmationNumber, a.ContractID, a.ContractNumber, coalesce(b.Points,0) as [Previously Assessed], a.Points as [Points Allowed], a.points-coalesce(b.points,0) as [To Be Assessed]  " & _
                        "from ( " & _
                        "   Select c.ContractID, c.ContractNumber, SUM(s.points) points " & _
                        "   From t_Contract c  " & _
                        "	    inner Join t_SoldInventory s on s.ContractID=c.ContractID  " & _
                        "       inner Join t_ComboItems cst on cst.ComboItemID=c.SubTypeID " & _
                        "       inner Join t_ComboItems cs on cs.ComboItemID=c.StatusID " & _
                        "   Where (" & ddUsageYear.SelectedValue & " - s.OccupancyYear) % s.FrequencyID = 0 and year(c.occupancydate)<=" & ddUsageYear.SelectedValue & " And cst.ComboItem='Points' and cs.ComboItem='Active' " & _
                        "   Group by c.contractid, c.ContractNumber " & _
                        ") a left outer join ( " & _
                        "   Select ContractID, sum(points) as Points " & _
                        "   from t_PointsTracking t " & _
                        "       inner join t_ComboItems tt On tt.ComboItemID=t.TransTypeID " & _
                        "   where UsageYear = " & ddUsageYear.SelectedValue & " And tt.ComboItem='Deposit' " & _
                        "   group by contractid " & _
                        ") b on b.ContractID=a.ContractID " & _
                        "where (b.ContractID Is null Or b.Points <> a.points) and a.points > 0 "
            Case Else
        End Select
        Return SQL
    End Function

    Private Sub Load_Years()
        For i = Year(DateAdd(DateInterval.Year, -2, Date.Today)) To Year(DateAdd(DateInterval.Year, 2, Date.Today))
            ddUsageYear.Items.Add(New ListItem With {.Text = i, .Value = i})
        Next
    End Sub

    Private Sub Toggle_Buttons()
        Dim bEnabled As Boolean = gvPoints.Rows.Count > 0
        btnAssess.Enabled = bEnabled
        btnExport.Enabled = bEnabled
    End Sub

    Protected Sub btnAssess_Click(sender As Object, e As EventArgs) Handles btnAssess.Click
        Dim dt As DataTable = Get_Records()
        Try
            dt.Columns.Add("Assessed")
            For Each row As DataRow In dt.Rows
                If row("To Be Assessed") > 0 And row("Assessed") & "" <> "1" Then
                    Dim oPA As New clsPointsTracking
                    Dim oC As New clsContract
                    oC.ContractID = row("ContractID")
                    oC.Load()

                    oPA.ApplyToID = 0
                    oPA.AvailYear = ddUsageYear.SelectedValue
                    oPA.Comments = "Deposit"
                    oPA.ConfirmationNumber = ""
                    oPA.ContractID = row("ContractID")
                    oPA.CreatedByID = Session("UserDBID")
                    oPA.ExpirationDate = "12/31/" & ddUsageYear.SelectedValue
                    oPA.Points = row("To Be Assessed")
                    oPA.PosNeg = 0
                    oPA.ProspectID = oC.ProspectID
                    oPA.StayLocID = 0
                    oPA.TransDate = Date.Now
                    oPA.TransTypeID = (New clsComboItems).Lookup_ID("PointsTransactionType", "Deposit")
                    oPA.UsageYear = ddUsageYear.SelectedValue
                    oPA.UserID = Session("UserDBID")
                    oPA.Save()

                    If oPA.ID > 0 Then row("Assessed") = 1 Else row("assessed") = 0

                    oPA = Nothing
                    oC = Nothing
                Else
                    row("Assessed") = 0
                End If
                dt.AcceptChanges()
                GC.Collect()
            Next
        Catch ex As Exception
            lblErr.Text = ex.Message
        End Try
        gvPoints.DataSource = dt
        gvPoints.DataBind()
        dt = Nothing
    End Sub
    Protected Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        Generate_Report()
    End Sub

    Private Sub Loop_Records(ByRef ws As IXLWorksheet)
        Dim row As Integer = 1
        For Each gvRow As GridViewRow In gvPoints.Rows
            If row = 1 Then
                For col = 0 To gvPoints.HeaderRow.Cells.Count - 1
                    ws.Cell(row, col + 1).SetValue(gvPoints.HeaderRow.Cells(col).Text)
                Next
                row += 1
            End If
            For col = 0 To gvRow.Cells.Count - 1
                ws.Cell(row, col + 1).SetValue(gvRow.Cells(col).Text)
            Next
            row += 1
        Next
    End Sub

    Private Sub Generate_Report()
        Dim res As HttpResponse = Response
        Dim xlWB As New XLWorkbook

        Loop_Records(xlWB.Worksheets.Add("Owners - Points"))


        res.Clear()
        res.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        res.AddHeader("content-disposition", "attachment;filename=""PointsAssessment.xlsx""")
        Using mem As New MemoryStream
            xlWB.SaveAs(mem)
            mem.WriteTo(res.OutputStream)
            mem.Close()
        End Using
        res.End()
    End Sub
End Class
