Imports System.Data.SqlClient
Imports ClosedXML.Excel
Imports System.IO

Partial Class Reports_OwnerServices_RoomTypeAllocation
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'Fill Dropdowns
            Fill()
        End If
        lblError.Text = ""
    End Sub

    Private Sub Fill()
        Using ds As New SqlDataSource(Resources.Resource.cns, "select 'All' as Value, 'All' as Text union (select i.comboitem as Value, i.Comboitem as Text from t_Comboitems i inner join t_Combos c on c.comboid=i.comboid where c.comboname='UnitType' and i.active = 1)")
            ddUnitType.DataSource = ds
            ddUnitType.DataValueField = "value"
            ddUnitType.DataTextField = "Text"
            ddUnitType.DataBind()
        End Using
    End Sub

    Protected Sub btnCreateExcel_Click(sender As Object, e As EventArgs) Handles btnCreateExcel.Click
        If dfSDate.selected_Date <> "" And dfEDate.selected_Date <> "" And dfFirstDate.selected_Date <> "" Then
            Generate_Report()

        Else
            lblError.Text = "All Fields are required."
            Exit Sub
        End If
    End Sub


    Private Sub Loop_Records(ByRef dr As SqlDataReader, ByRef ws As IXLWorksheet)
        Dim row As Integer = 1

        While dr.Read
            Dim fd As Date = dr("StartDate") 'dfFirstDate.selected_date
            If row = 1 Then
                For col = 1 To dr.VisibleFieldCount
                    ws.Cell(row, col).SetValue(dr.GetName(col - 1))
                Next
                row += 1
                For col = 8 To dr.VisibleFieldCount
                    ws.Cell(row, col).SetValue(fd.AddDays((dr.GetName(col - 1) - 1) * 7))
                    ws.Column(col).Cells.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                Next
                row += 1
            End If
            If ddUnitType.SelectedValue = "All" Or ddUnitType.SelectedValue = dr("UnitType") Then
                For col = 1 To dr.VisibleFieldCount
                    ws.Cell(row, col).Style.Fill.SetBackgroundColor(Set_Color(dr.Item(col - 1) & ""))
                    If dr.Item(col - 1) & "" <> "Owner" Then
                        ws.Cell(row, col).SetValue(dr.Item(col - 1))
                        ws.Column(col).Cells.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center
                    End If
                Next
                row += 1
            End If
        End While
        dr.Close()
        ws.Columns.AdjustToContents()
    End Sub

    Private Function Set_Color(Val As String) As XLColor
        Dim ret As XLColor = XLColor.White
        Select Case Val.ToLower
            Case "offline"
                ret = XLColor.FromIndex(46)
                ret = XLColor.FromArgb(255, 102, 0)
            Case "rental"
                ret = XLColor.FromIndex(38)
                ret = XLColor.FromArgb(255, 153, 204)
            Case "set rent"
                ret = XLColor.FromIndex(43)
                ret = XLColor.FromArgb(153, 204, 0)
            Case "kcp rent"
                ret = XLColor.FromIndex(33)
                ret = XLColor.FromArgb(0, 204, 255)
            Case "pool rent"
                ret = XLColor.FromIndex(35)
                ret = XLColor.FromArgb(204, 255, 204)
            Case "mkt/mkt"
                ret = XLColor.FromIndex(39)
                ret = XLColor.FromArgb(204, 153, 255)
            Case "marketing"
                ret = XLColor.FromIndex(16)
                ret = XLColor.FromArgb(128, 128, 128)
            Case "pointsexchange"
                ret = XLColor.FromIndex(25)
                ret = XLColor.FromArgb(0, 0, 128)
            Case "fixed"
                ret = XLColor.FromIndex(6)
                ret = XLColor.FromArgb(255, 255, 0)
            Case "float"
                ret = XLColor.FromIndex(7)
                ret = XLColor.FromArgb(255, 0, 255)
            Case "waitlist"
                ret = XLColor.FromIndex(36)
                ret = XLColor.FromArgb(255, 255, 153)
            Case "exchange"
                ret = XLColor.FromIndex(3)
                ret = XLColor.FromArgb(255, 0, 0)
            Case "rci"
                ret = XLColor.FromIndex(40)
                ret = XLColor.FromArgb(255, 204, 153)
            Case "ii"
                ret = XLColor.FromIndex(8)
                ret = XLColor.FromArgb(0, 255, 255)
            Case "ice"
                ret = XLColor.FromIndex(4)
                ret = XLColor.FromArgb(0, 255, 0)
            Case "non-owner"
                ret = XLColor.FromIndex(22)
                ret = XLColor.FromArgb(255, 128, 128)
            Case "pointsu"
                ret = XLColor.FromIndex(54)
                ret = XLColor.FromArgb(153, 51, 102)
            Case "spare"
                ret = XLColor.FromIndex(50)
                ret = XLColor.FromArgb(51, 153, 102)
            Case "developer"
                ret = XLColor.FromIndex(5)
                ret = XLColor.FromArgb(0, 0, 255)
            Case "srental"
                ret = XLColor.FromIndex(36)
                ret = XLColor.FromArgb(255, 255, 153)
            Case "trialowner"
                ret = XLColor.FromIndex(41)
                ret = XLColor.FromArgb(51, 102, 255)
            Case "points"
                ret = XLColor.FromIndex(12)
                ret = XLColor.FromArgb(128, 128, 0)
            Case "pointsexchangeu"
                ret = XLColor.FromIndex(48)
                ret = XLColor.FromArgb(150, 150, 150)
            Case "vendor"
                ret = XLColor.FromIndex(18)
                ret = XLColor.FromArgb(128, 128, 128)
            Case Else
                ret = XLColor.White

        End Select
        Return ret
    End Function

    Private Sub Generate_Report()
        Dim res As HttpResponse = Response
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("exec sp_AllocationMatrix '" & dfSDate.Selected_Date & "', '" & dfEDate.Selected_Date & "', '" & dfFirstDate.Selected_Date & "'", cn)
        Dim xlWB As New XLWorkbook
        cm.CommandTimeout = 10000
        cn.Open()

        Loop_Records(cm.ExecuteReader, xlWB.Worksheets.Add("Owners"))

        cn.Close()
        cn = Nothing
        cm = Nothing

        res.Clear()
        res.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        res.AddHeader("content-disposition", "attachment;filename=""RoomTypeAllocationMatrix.xlsx""")
        Using mem As New MemoryStream
            xlWB.SaveAs(mem)
            mem.WriteTo(res.OutputStream)
            mem.Close()
        End Using
        res.End()
    End Sub
End Class
