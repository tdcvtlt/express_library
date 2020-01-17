Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports System.Data.SqlClient
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports System.Web.Security
Imports System.Web
Imports System.Configuration
Imports System.Data
Imports System

Partial Class Reports_Tours_TourCountsByWave1
    Inherits System.Web.UI.Page
    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/TourCountsByWave.rpt"

    Private Sub BindData()
        Dim cnn As New SqlConnection(Resources.Resource.cns)
        Dim cmd As New SqlCommand("select b.COMBOITEM, b.COMBOITEMID from t_combos a inner join t_comboitems b on a.comboid = b.comboid where comboname = 'CampaignType' and b.active = 1 order by b.Comboitem", cnn)
        Dim da As New SqlDataAdapter(cmd)
        Dim ds As New DataSet
        da.Fill(ds, "CampaignType")
        'ListBox1.Items.Add(New ListItem("All", "1"))
        ListBox1.DataSource = ds.Tables("CampaignType")
        ListBox1.DataTextField = "ComboItem"
        ListBox1.DataValueField = "ComboItemID"
        ListBox1.AppendDataBoundItems = True
        ListBox1.DataBind()
        cmd.CommandText = "select b.COMBOITEM, b.COMBOITEMID from t_combos a inner join t_comboitems b on a.comboid = b.comboid where comboname = 'TourTime' and b.active = 1 order by cast(b.Comboitem as int)"
        da.Fill(ds, "Waves")
        lbWave.DataSource = ds.Tables("Waves")
        lbWave.DataTextField = "ComboItem"
        lbWave.DataValueField = "ComboItemID"
        lbWave.AppendDataBoundItems = True
        lbWave.DataBind()
        If cnn.State <> ConnectionState.Closed Then cnn.Close()
        cnn.Dispose()
        cmd = Nothing
        da = Nothing
        ds = Nothing
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then

        Else
            Session("Report") = Nothing
            BindData()
            Setup_Report()
        End If

        If hfShowReport.Value = 1 Then CrystalReportViewer1.ReportSource = Session("Report")


    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Try
            'Report.Close()
            'Report.Dispose()
        Catch ex As Exception

        End Try


    End Sub



    Private Sub Setup_Report()
        If Session("Report") Is Nothing Then
            Report.Load(Server.MapPath(sReport))
            Report.FileName = Server.MapPath(sReport)
            'Response.Write("HERE")
            'Dim categoryID As Integer = Convert.ToInt32(ddlCategory.SelectedValue)
            Report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
            'Report.DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
            Set_Params()
            Session.Add("Report", Report)
        Else
            Report = Session("Report")
            If Report.FileName <> Server.MapPath(sReport) Then
                Session("Report") = Nothing
                Setup_Report()
            End If
        End If
    End Sub

    Protected Sub btnRun_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRunReport.Click
        If dfStartDate.Selected_Date <> "" And dfEndDate.Selected_Date <> "" And ListBox2.Items.Count > 0 And lbWaves.Items.Count > 0 Then
            List_Report()
            Exit Sub
            hfShowReport.Value = 1
            Setup_Report()

            CrystalReportViewer1.ReportSource = Session("Report")
        Else
            hfShowReport.Value = 0
        End If
    End Sub

    Protected Sub Set_Params()

        Report.SetParameterValue("SDate", CDate(IIf(dfStartDate.Selected_Date <> "", dfStartDate.Selected_Date, Date.Today)))
        Report.SetParameterValue("EDate", CDate(IIf(dfEndDate.Selected_Date <> "", dfEndDate.Selected_Date, Date.Today)))
        Dim sItems As String = ""
        For i = 0 To ListBox2.Items.Count - 1
            sItems &= IIf(sItems = "", ListBox2.Items(i).Value, "," & ListBox2.Items(i).Value)
        Next
        Report.SetParameterValue("sType", sItems)

    End Sub


    Protected Sub Button4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button4.Click
        '<<<
        For i = ListBox2.Items.Count - 1 To 0 Step -1
            ListBox1.ClearSelection()
            ListBox1.Items.Add(ListBox2.Items(i))
            ListBox2.Items.Remove(ListBox2.Items(i))
        Next
        hfShowReport.Value = 0
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        '<<
        If ListBox2.SelectedIndex >= 0 Then
            ListBox1.ClearSelection()
            ListBox1.Items.Add(ListBox2.SelectedItem)
            ListBox2.Items.Remove(ListBox2.SelectedItem)
            hfShowReport.Value = 0
        End If
    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        '>>
        If ListBox1.SelectedIndex >= 0 Then
            ListBox2.ClearSelection()
            ListBox2.Items.Add(ListBox1.SelectedItem)
            ListBox1.Items.Remove(ListBox1.SelectedItem)
            ListBox2.SelectedIndex = 0
            hfShowReport.Value = 0
        End If
    End Sub

    Protected Sub Button5_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button5.Click
        '>>>
        For i = ListBox1.Items.Count - 1 To 0 Step -1
            ListBox2.ClearSelection()
            ListBox2.Items.Add(ListBox1.Items(i))
            ListBox1.Items.Remove(ListBox1.Items(i))
            ListBox2.SelectedIndex = 0
            hfShowReport.Value = 0
        Next
    End Sub
    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        For i = lbWaves.Items.Count - 1 To 0 Step -1
            lbWave.ClearSelection()
            lbWave.Items.Add(lbWaves.Items(i))
            lbWaves.Items.Remove(lbWaves.Items(i))
        Next
        SortWaves(lbWave)
        hfShowReport.Value = 0
    End Sub
    Protected Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        If lbWaves.SelectedIndex >= 0 Then
            lbWave.ClearSelection()
            lbWave.Items.Add(lbWaves.SelectedItem)
            lbWaves.Items.Remove(lbWaves.SelectedItem)
            hfShowReport.Value = 0
            SortWaves(lbWave)
        End If
    End Sub
    Protected Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        If lbWave.SelectedIndex >= 0 Then
            lbWaves.ClearSelection()
            lbWaves.Items.Add(lbWave.SelectedItem)
            lbWave.Items.Remove(lbWave.SelectedItem)
            lbWaves.SelectedIndex = 0
            hfShowReport.Value = 0
            SortWaves(lbWaves)
        End If
    End Sub
    Protected Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        For i = lbWave.Items.Count - 1 To 0 Step -1
            lbWaves.ClearSelection()
            lbWaves.Items.Add(lbWave.Items(i))
            lbWave.Items.Remove(lbWave.Items(i))
            lbWaves.SelectedIndex = 0
            hfShowReport.Value = 0
        Next
        SortWaves(lbWaves)
    End Sub

    Private Sub SortWaves(ByRef lb As ListBox)
        If lb.Items.Count < 2 Then Exit Sub
        Dim lbTemp As New List(Of ListItem)
        For i = 0 To lb.Items.Count - 1
            lbTemp.Add(lb.Items(i))
        Next
        For x = 1 To lbTemp.Count - 1
            For i = 0 To x
                If CInt(lbTemp.Item(x).Text) < CInt(lbTemp.Item(i).Text) Then
                    Dim item As ListItem = lbTemp.Item(x)
                    lbTemp.Item(x) = lbTemp.Item(i)
                    lbTemp.Item(i) = item
                End If
            Next
        Next
        lb.Items.Clear()
        For Each item As ListItem In lbTemp
            lb.Items.Add(item)
        Next
    End Sub

    Protected Sub List_Report()
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("", cn)
        Dim da As New SqlDataAdapter(cm)
        Dim ds As New DataSet
        Dim result As New DataTable
        Dim endResult As New DataTable
        result.Columns.Add(New DataColumn With {.ColumnName = "Wave", .DataType = System.Type.GetType("System.String")})
        result.Columns.Add(New DataColumn With {.ColumnName = "TourDate", .DataType = System.Type.GetType("System.DateTime")})
        result.Columns.Add(New DataColumn With {.ColumnName = "Tours", .DataType = System.Type.GetType("System.Int32")})
        result.Columns.Add(New DataColumn With {.ColumnName = "WeekDay", .DataType = System.Type.GetType("System.String")})

        cm.CommandText = Command_Text()
        da.Fill(ds, "tours")
        If ds.Tables("tours").Rows.Count > 0 Then
            For Each row As DataRow In ds.Tables("tours").Rows
                Dim newRow As DataRow = result.NewRow
                newRow("Wave") = row("Wave")
                newRow("TourDate") = row("TourDate")
                newRow("Tours") = row("Tours")
                newRow("WeekDay") = CDate(row("TourDate")).ToString("ddd")
                result.Rows.Add(newRow)
            Next
        End If

        endResult.Columns.Add(New DataColumn With {.ColumnName = "Wave", .DataType = System.Type.GetType("System.String")})
        endResult.Columns.Add(New DataColumn With {.ColumnName = "Day1Wave", .DataType = System.Type.GetType("System.Int32")})
        endResult.Columns.Add(New DataColumn With {.ColumnName = "Day1", .DataType = System.Type.GetType("System.DateTime")})
        endResult.Columns.Add(New DataColumn With {.ColumnName = "Day2Wave", .DataType = System.Type.GetType("System.Int32")})
        endResult.Columns.Add(New DataColumn With {.ColumnName = "Day2", .DataType = System.Type.GetType("System.DateTime")})
        endResult.Columns.Add(New DataColumn With {.ColumnName = "Day3Wave", .DataType = System.Type.GetType("System.Int32")})
        endResult.Columns.Add(New DataColumn With {.ColumnName = "Day3", .DataType = System.Type.GetType("System.DateTime")})
        endResult.Columns.Add(New DataColumn With {.ColumnName = "Day4Wave", .DataType = System.Type.GetType("System.Int32")})
        endResult.Columns.Add(New DataColumn With {.ColumnName = "Day4", .DataType = System.Type.GetType("System.DateTime")})
        endResult.Columns.Add(New DataColumn With {.ColumnName = "Day5Wave", .DataType = System.Type.GetType("System.Int32")})
        endResult.Columns.Add(New DataColumn With {.ColumnName = "Day5", .DataType = System.Type.GetType("System.DateTime")})
        endResult.Columns.Add(New DataColumn With {.ColumnName = "Day6Wave", .DataType = System.Type.GetType("System.Int32")})
        endResult.Columns.Add(New DataColumn With {.ColumnName = "Day6", .DataType = System.Type.GetType("System.DateTime")})
        endResult.Columns.Add(New DataColumn With {.ColumnName = "Day7Wave", .DataType = System.Type.GetType("System.Int32")})
        endResult.Columns.Add(New DataColumn With {.ColumnName = "Day7", .DataType = System.Type.GetType("System.DateTime")})
        Dim i As Date = CDate(dfStartDate.Selected_Date)
        Dim foundRows() As DataRow
        While i <= CDate(dfEndDate.Selected_Date)
            For Each x As ListItem In lbWaves.Items
                Dim endRow As DataRow = endResult.NewRow
                Dim t As Integer = CInt(x.Text)
                Dim tours As Integer = 0
                endRow("Wave") = IIf(t \ 100 < 13, t \ 100, (t \ 100) - 12) & ":" & (t - (t \ 100) * 100) & IIf(Len((t - (t \ 100) * 100).ToString()) = 1, "0", "") & IIf(t \ 100 < 12, " AM", " PM")
                foundRows = result.Select("wave = '" & x.Text & "' and TourDate='" & i & "'")
                If foundRows.Length > 0 Then
                    tours = foundRows(0)("Tours")
                Else
                    tours = 0
                End If
                endRow("Day1Wave") = tours
                endRow("Day1") = i
                foundRows = result.Select("wave = '" & x.Text & "' and TourDate='" & i.AddDays(1) & "'")
                If foundRows.Length > 0 Then
                    tours = foundRows(0)("Tours")
                Else
                    tours = 0
                End If
                endRow("Day2Wave") = tours
                endRow("Day2") = i.AddDays(1)
                foundRows = result.Select("wave = '" & x.Text & "' and TourDate='" & i.AddDays(2) & "'")
                If foundRows.Length > 0 Then
                    tours = foundRows(0)("Tours")
                Else
                    tours = 0
                End If
                endRow("Day3Wave") = tours
                endRow("Day3") = i.AddDays(2)
                foundRows = result.Select("wave = '" & x.Text & "' and TourDate='" & i.AddDays(3) & "'")
                If foundRows.Length > 0 Then
                    tours = foundRows(0)("Tours")
                Else
                    tours = 0
                End If
                endRow("Day4Wave") = tours
                endRow("Day4") = i.AddDays(3)
                foundRows = result.Select("wave = '" & x.Text & "' and TourDate='" & i.AddDays(4) & "'")
                If foundRows.Length > 0 Then
                    tours = foundRows(0)("Tours")
                Else
                    tours = 0
                End If
                endRow("Day5Wave") = tours
                endRow("Day5") = i.AddDays(4)
                foundRows = result.Select("wave = '" & x.Text & "' and TourDate='" & i.AddDays(5) & "'")
                If foundRows.Length > 0 Then
                    tours = foundRows(0)("Tours")
                Else
                    tours = 0
                End If
                endRow("Day6Wave") = tours
                endRow("Day6") = i.AddDays(5)
                foundRows = result.Select("wave = '" & x.Text & "' and TourDate='" & i.AddDays(6) & "'")
                If foundRows.Length > 0 Then
                    tours = foundRows(0)("Tours")
                Else
                    tours = 0
                End If
                endRow("Day7Wave") = tours
                endRow("Day7") = i.AddDays(6)
                endResult.Rows.Add(endRow)
            Next
            i = i.AddDays(7)
        End While

        endResult.DefaultView.Sort = "Day1 asc"
        lvReport.DataSource = endResult
        lvReport.DataBind()

        ds = Nothing
        da = Nothing
        cm = Nothing
        cn = Nothing
    End Sub

    Dim lastGroupDate As String = ""
    Protected Function AddGroupingRowIfChanged() As String
        Dim ret As String = ""
        If lastGroupDate <> Eval("Day1").ToString() Then
            If lastGroupDate <> "" Then ret = "<tr><td colspan=""8"">&nbsp;</td></tr>"
            lastGroupDate = Eval("Day1").ToString()
            ret &= "<tr style='border:thin solid;border-color:black;'><td></td>"
            ret &= "<td style=""border: thin solid #000000;" & IIf(CDate(Eval("Day1")).ToString("ddd").ToLower() = "sat" Or CDate(Eval("Day1")).ToString("ddd").ToLower() = "sun", "background:lightblue;""", "") & """>" & CDate(Eval("Day1")).ToString("ddd") & " " & Month(Eval("Day1")) & "/" & Day(Eval("Day1")) & "</td>"
            ret &= "<td style=""border: thin solid #000000;" & IIf(CDate(Eval("Day2")).ToString("ddd").ToLower() = "sat" Or CDate(Eval("Day2")).ToString("ddd").ToLower() = "sun", "background:lightblue;""", "") & """>" & CDate(Eval("Day2")).ToString("ddd") & " " & Month(Eval("Day2")) & "/" & Day(Eval("Day2")) & "</td>"
            ret &= "<td style=""border: thin solid #000000;" & IIf(CDate(Eval("Day3")).ToString("ddd").ToLower() = "sat" Or CDate(Eval("Day3")).ToString("ddd").ToLower() = "sun", "background:lightblue;""", "") & """>" & CDate(Eval("Day3")).ToString("ddd") & " " & Month(Eval("Day3")) & "/" & Day(Eval("Day3")) & "</td>"
            ret &= "<td style=""border: thin solid #000000;" & IIf(CDate(Eval("Day4")).ToString("ddd").ToLower() = "sat" Or CDate(Eval("Day4")).ToString("ddd").ToLower() = "sun", "background:lightblue;""", "") & """>" & CDate(Eval("Day4")).ToString("ddd") & " " & Month(Eval("Day4")) & "/" & Day(Eval("Day4")) & "</td>"
            ret &= "<td style=""border: thin solid #000000;" & IIf(CDate(Eval("Day5")).ToString("ddd").ToLower() = "sat" Or CDate(Eval("Day5")).ToString("ddd").ToLower() = "sun", "background:lightblue;""", "") & """>" & CDate(Eval("Day5")).ToString("ddd") & " " & Month(Eval("Day5")) & "/" & Day(Eval("Day5")) & "</td>"
            ret &= "<td style=""border: thin solid #000000;" & IIf(CDate(Eval("Day6")).ToString("ddd").ToLower() = "sat" Or CDate(Eval("Day6")).ToString("ddd").ToLower() = "sun", "background:lightblue;""", "") & """>" & CDate(Eval("Day6")).ToString("ddd") & " " & Month(Eval("Day6")) & "/" & Day(Eval("Day6")) & "</td>"
            ret &= "<td style=""border: thin solid #000000;" & IIf(CDate(Eval("Day7")).ToString("ddd").ToLower() = "sat" Or CDate(Eval("Day7")).ToString("ddd").ToLower() = "sun", "background:lightblue;""", "") & """>" & CDate(Eval("Day7")).ToString("ddd") & " " & Month(Eval("Day7")) & "/" & Day(Eval("Day7")) & "</td>"
            ret &= "</tr>"
        End If
        Return ret
    End Function

    Private Function Command_Text() As String
        Dim waves As String = ""
        Dim types As String = ""
        For Each item As ListItem In lbWaves.Items
            waves &= IIf(waves = "", "'" & item.Text & "'", ",'" & item.Text & "'")
        Next
        For Each item As ListItem In ListBox2.Items
            types &= IIf(types = "", "'" & item.Text & "'", ",'" & item.Text & "'")
        Next
        Dim ret As String = "Select  Distinct (tt.comboitem) As Wave, t.TourDate, Count(*) As Tours, lim.Quota, DATEPART(DW,t.tourdate) As WeekDay " &
                "From t_Tour t  " &
                "	Left outer join t_Comboitems tt on tt.comboitemid = t.tourtime  " &
                "    Left outer Join ( " &
                "            Select  Distinct (WaveTimeID), EffectiveStartDate, TourLocationID, SUM(MaxLimit) As Quota  " &
                "            From t_TourWaveLimits " &
                "            Where CampaignTypeID In ( " &
                "				Select comboitemid " &
                "                From t_Comboitems i  " &
                "					inner Join t_Combos c on c.comboid = i.comboid  " &
                "                Where c.comboname = 'TourType'  " &
                "                    And ComboItem in (" & types & ") " &
                "				) " &
                "				And effectivestartdate >= '" & dfStartDate.Selected_Date & "' and EffectiveStartDate <= '" & dfEndDate.Selected_Date & "'  " &
                "			Group by WaveTimeID, EffectiveStartDate, tourlocationid " &
                "		) lim on t.TourDate = lim.EffectiveStartDate  " &
                "		And t.TourTime = lim.WaveTimeID And t.TourLocationID = lim.TourLocationID " &
                "where tourdate >= '" & dfStartDate.Selected_Date & "' and tourdate <= '" & dfEndDate.Selected_Date & "'  " &
                "    And subtypeid Not in  " &
                "	( " &
                "		Select comboitemid " &
                "        From t_ComboItems i  " &
                "			inner Join t_combos c on c.comboid = i.comboid  " &
                "        where comboname = 'TourSubType' and comboitem = 'Exit'  " &
                "	) And t.tourLocationID in  " &
                "	( " &
                "		Select comboitemid " &
                "        From t_ComboItems i  " &
                "			inner Join t_combos c on c.comboid = i.comboid  " &
                "        where comboname = 'TourLocation' and comboitem = 'KCP'  " &
                "	) And statusid in  " &
                "	( " &
                "		Select comboitemid " &
                "        From t_ComboItems i  " &
                "			inner Join t_combos c on c.comboid = i.comboid  " &
                "        where comboname = 'TourStatus' and comboitem = 'Booked'  " &
                "	) And campaignid in  " &
                "	( " &
                "		Select c.campaignid " &
                "        From t_Campaign c " &
                "			inner Join t_comboitems i on i.comboitemid = c.typeid " &
                "        where i.comboitem in (" & types & ") " &
                "	) And tt.ComboItem in (" & waves & ") " &
                "Group by tourdate, tt.comboitem, lim.Quota  " &
                "order by tourdate asc, tt.Comboitem"
        Return ret
    End Function
End Class


