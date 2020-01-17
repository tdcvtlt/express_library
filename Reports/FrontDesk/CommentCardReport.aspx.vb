Imports System.Data.SqlClient

Partial Class Reports_FrontDesk_CommentCardReport
    Inherits System.Web.UI.Page
    Dim cn As Object
    Dim rs As Object
    Dim x As Integer
    Dim y As Integer
    Dim i As Integer
    Dim sLink As String
    Dim sql As String
    Dim sFilename As String
    Dim xlApp As Object
    Dim wb As Object
    Dim wsActiveSheet As Object
    Dim aTypes() As String
    Dim aFields() As String
    Dim row As Integer
    Dim column As Integer

    Protected Sub btnRun_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRun.Click
       
        cn = Server.CreateObject("ADODB.Connection")
        rs = Server.CreateObject("ADODB.Recordset")

        cn.open(Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)


        'Dim aTypes() As String
        ReDim aTypes(0)

        'Get unit types
        If siRoomType.SelectedName = "(empty)" And ddRooms.SelectedValue = 0 Then
            sql = "Select distinct comboitem from t_Comboitems i inner join t_Combos co on co.comboid = i.comboid where comboname = 'unittype' and active = 1"
        ElseIf ddRooms.SelectedValue = 0 Then
            sql = "Select distinct comboitem from t_Comboitems i inner join t_Combos co on co.comboid = i.comboid where comboname = 'unittype' and active = 1 and comboitem = '" & siRoomType.SelectedName & "'"
        ElseIf siRoomType.SelectedName = "(empty)" Then
            sql = "Select distinct comboitem from t_Comboitems c inner join t_Combos co on co.comboid = c.comboid inner join t_Unit u on u.typeid = c.comboitemid inner join t_Room r on r.unitid = u.unitid where comboname = 'unittype' and active = 1 and  r.roomnumber = '" & ddRooms.SelectedItem.Text & "'"
        Else
            sql = "Select distinct comboitem from t_Comboitems c inner join t_Combos co on co.comboid = c.comboid inner join t_Unit u on u.typeid = c.comboitemid inner join t_Room r on r.unitid = u.unitid where comboname = 'unittype' and active = 1 and comboitem = '" & siRoomType.SelectedName & "' and r.roomnumber = '" & ddRooms.SelectedItem.Text & "'"
        End If
        rs.open(sql, cn, 0, 1)
        i = 0
        Do While Not rs.eof
            If i > UBound(aTypes) Then ReDim Preserve aTypes(i)
            aTypes(i) = rs.fields("Comboitem").value & ""
            i = i + 1
            rs.movenext()
        Loop
        rs.close()

        'Get Field List 
        'Dim aFields() As String
        ReDim aFields(0)
        i = 0

        sql = "Select distinct fieldname from t_Commentcardfields order by fieldname"
        rs.open(sql, cn, 0, 1)
        Do While Not rs.eof
            If i > UBound(aFields) Then ReDim Preserve aFields(i)
            aFields(i) = rs.fields("Fieldname").value & ""
            i = i + 1
            rs.movenext()
        Loop
        rs.close()

        'Hold top ten for each type
        Dim aCTop(9) As String
        Dim aTTop(9) As String
        Dim aETop(9) As String

        If Request("excel") = "1" Then
            sLink = Replace(Replace("CommentCards-" & Session.SessionID & "-" & Date.Today.ToShortDateString & "-" & Date.Now.ToShortTimeString & ".xls", "/", "-"), ":", "-")
            sFilename = "C:\inetpub\wwwroot\crms\reports\contracts\" & sLink
            xlApp = Server.CreateObject("Excel.Application")
            wb = xlApp.Workbooks.open("c:\inetpub\wwwroot\crms\reports\contracts\commentcard.xls")
            wb.SaveAs(sFilename)
            wb.close()
            wb = xlApp.Workbooks.Open(sFilename)
        End If

        For Me.i = 0 To UBound(aTypes)
            'if i>0 then 
            Lit.Text &= ("<p style='page-break-before:always;'>")
            'else
            '	lit.text &=  "<p style='page-break-after:always;'>"
            'end if
            If Request("excel") = "1" Then
                Write_Excel()
            Else
                Write_Web()
            End If
        Next

        If Request("excel") = "1" Then
            Write_Excel_Top10()
        Else
            Write_Web_Top10()
        End If

        If Request("excel") = "1" Then
            wb.Save()
            wb.Close()
            xlApp = Nothing
            wb = Nothing
            wsActiveSheet = Nothing
            Lit.Text &= ("<a href='../contracts/" & sLink & "'>Open Excel File By Clicking Here</a>")
        End If

        '     ElseIf Request("f") = "getrooms" Then
        '         If Request("ut") = "" Then
        '             sql = "Select roomnumber from t_Room order by roomnumber"
        '         Else
        'sql = "Select roomnumber from t_Room r inner join t_Units u on u.unitid = r.unitid inner join t_Comboitems ut on ut.comboitemid = u.typeid where ut.comboitem = '" & request("ut") & "' order by r.roomnumber"		end if		

        '             rs.open(sql, cn, 0, 1)
        '             If rs.eof And rs.bof Then
        '                 lit.text &= ("NO")
        '             Else
        '                 i = 0
        '                 Do While Not rs.eof
        '                     If i > 0 Then lit.text &= ("|")
        '                     i = 1
        '                     lit.text &= (rs.fields("Roomnumber").value)
        '                     rs.movenext()
        '                 Loop
        '             End If
        '             rs.close()

        '         End If

        cn.close()

        cn = Nothing
        rs = Nothing





    End Sub


    Sub Write_Web()
        Lit.Text &= ("<table style='border:thin solid black;' cellpadding='2'>")
        Lit.Text &= ("<tr>")
        Lit.Text &= ("<td colspan='6' align='center' style='border-bottom:thin solid black;'><b>" & aTypes(i))
        If ddRooms.SelectedValue <> 0 Then Lit.Text &= ("<br>Room: " & ddRooms.SelectedItem.Text)
        Lit.Text &= ("<br>For " & SDate.Selected_Date & " to " & EDate.Selected_Date & "</b></td>")
        Lit.Text &= ("</tr>")
        Lit.Text &= ("<tr>")
        Lit.Text &= ("<th style='border-bottom:thin solid black;'>Area</th>")
        Lit.Text &= ("<th style='border-bottom:thin solid black;border-right:thin solid black;'>#</th>")
        Lit.Text &= ("<th style='border-bottom:thin solid black;'>Area</th>")
        Lit.Text &= ("<th style='border-bottom:thin solid black;border-right:thin solid black;'>#</th>")
        Lit.Text &= ("<th style='border-bottom:thin solid black;'>Area</th>")
        Lit.Text &= ("<th style='border-bottom:thin solid black;border-right:thin solid black;'>#</th>")
        Lit.Text &= ("</tr>")

        For x As Integer = 0 To UBound(aFields)
            Get_Records()


            rs.open(sql, cn, 0, 1)

            If x Mod 3 = 0 Then
                If x > 0 Then
                    Lit.Text &= ("</tr>")
                End If
                Lit.Text &= ("<tr>")
            End If

            Lit.Text &= ("<td style='border-bottom:thin solid black;'>" & aFields(x) & "</td>")
            Lit.Text &= ("<td style='border-bottom:thin solid black;border-right:thin solid black;' align = 'right'>" & rs.fields("Issues").value & "</td>")

            rs.close()

        Next
        If x Mod 3 <> 0 Then
            For y As Integer = 1 To 3 - (x Mod 3)
                Lit.Text &= ("<td style='border-bottom:thin solid black;'>&nbsp;</td>")
                Lit.Text &= ("<td style='border-bottom:thin solid black;border-right:thin solid black;' align = 'right'>&nbsp;</td>")
            Next
        End If

        Lit.Text &= ("</tr>")
        Lit.Text &= ("</table>")


        'if i > 0 then 
        Lit.Text &= ("</p>")
    End Sub


    Sub Write_Web_Top10()
        'Top 10 Issues
        Lit.Text &= ("<p style='page-break-before:always;'>")
        Lit.Text &= ("<table style='border:thin solid black;' cellpadding='2'>")
        Lit.Text &= ("<tr>")
        Lit.Text &= ("<td colspan='6' align='center' style='border-bottom:thin solid black;'><b>Top 10 Issues")
        Lit.Text &= ("<br>For " & SDate.Selected_Date & " to " & EDate.Selected_Date & "</b></td>")
        Lit.Text &= ("</tr>")
        Lit.Text &= ("<tr>")
        Lit.Text &= ("<th style='border-bottom:thin solid black;'>Area</th>")
        Lit.Text &= ("<th style='border-bottom:thin solid black;border-right:thin solid black;'>#</th>")
        Lit.Text &= ("<th style='border-bottom:thin solid black;'>Area</th>")
        Lit.Text &= ("<th style='border-bottom:thin solid black;border-right:thin solid black;'>#</th>")
        Lit.Text &= ("<th style='border-bottom:thin solid black;'>Area</th>")
        Lit.Text &= ("<th style='border-bottom:thin solid black;border-right:thin solid black;'>#</th>")
        Lit.Text &= ("</tr>")
        Get_Top10()
        If rs.eof And rs.bof Then
        Else
            i = 0
            Lit.Text &= ("<tr>")
            Do While Not rs.eof
                Lit.Text &= ("<td style='border-bottom:thin solid black;'>")
                Lit.Text &= (rs.fields("Fieldname").value & "")
                Lit.Text &= ("</td>")
                Lit.Text &= ("<td style='border-bottom:thin solid black;border-right:thin solid black;'>")
                Lit.Text &= (rs.fields("Issues").value)
                Lit.Text &= ("</td>")
                i = i + 1
                If i = 10 Then Exit Do
                If i Mod 3 = 0 Then Lit.Text &= ("</tr><tr>")
                rs.movenext()
            Loop

        End If
        Lit.Text &= ("<td style='border-bottom:thin solid black;'>&nbsp;</td>")
        Lit.Text &= ("<td style='border-bottom:thin solid black;border-right:thin solid black;'>&nbsp;</td>")
        Lit.Text &= ("<td style='border-bottom:thin solid black;'>&nbsp;</td>")
        Lit.Text &= ("<td style='border-bottom:thin solid black;border-right:thin solid black;'>&nbsp;</td>")
        Lit.Text &= ("</tr>")
        Lit.Text &= ("</table>")
        Lit.Text &= ("</p>")
        rs.close()
    End Sub

    Sub Get_Top10()
        sql = "select count(distinct valueid) as Issues, f.fieldname from t_CommentCardFields f " & _
          "	left outer join t_Commentcardfieldvalues v on v.fieldid = f.fieldid " & _
          "	left outer join t_CommentCards c on c.cardid = v.cardid " & _
          "	left outer join t_Room r on r.roomid = c.roomid " & _
          "	left outer join t_Unit u on u.unitid = r.unitid " & _
          "	left outer join t_Comboitems ut on ut.comboitemid = u.typeid " & _
          "	where (c.carddate between '" & SDate.Selected_Date & "' and '" & EDate.Selected_Date & "' or c.carddate is null) " & _
          "group by f.fieldname  " & _
          "order by count(distinct valueid) desc"

        rs.open(sql, cn, 0, 1)

    End Sub

    Sub Get_Records()
        If ddRooms.SelectedValue <> 0 Then
            sql = "select count(distinct valueid) as Issues from t_CommentCardFields f " & _
              "left outer join t_Commentcardfieldvalues v on v.fieldid = f.fieldid " & _
              "left outer join t_CommentCards c on c.cardid = v.cardid " & _
              "left outer join t_Room r on r.roomid = c.roomid " & _
              "left outer join t_Unit u on u.unitid = r.unitid " & _
              "left outer join t_Comboitems ut on ut.comboitemid = u.typeid " & _
              "where (c.carddate between '" & SDate.Selected_Date & "' and '" & CDate(EDate.Selected_Date).AddDays(1).ToShortDateString & "' or c.carddate is null) " & _
              "and (ut.comboitem is null or ut.comboitem = '" & aTypes(i) & "') " & _
              "and f.fieldname = '" & aFields(x) & "' " & _
              "and r.roomnumber = '" & ddRooms.SelectedItem.Text & "' "
        Else
            sql = "select count(distinct valueid) as Issues from t_CommentCardFields f " & _
              "left outer join t_Commentcardfieldvalues v on v.fieldid = f.fieldid " & _
              "left outer join t_CommentCards c on c.cardid = v.cardid " & _
              "left outer join t_Room r on r.roomid = c.roomid " & _
              "left outer join t_Unit u on u.unitid = r.unitid " & _
              "left outer join t_Comboitems ut on ut.comboitemid = u.typeid " & _
              "where (c.carddate between '" & SDate.Selected_Date & "' and '" & CDate(EDate.Selected_Date).AddDays(1).ToShortDateString & "' or c.carddate is null) " & _
              "and (ut.comboitem is null or ut.comboitem = '" & aTypes(i) & "') " & _
              "and f.fieldname = '" & aFields(x) & "' "
        End If
    End Sub

    Sub Write_Excel()


        wsActiveSheet = wb.Worksheets(aTypes(i))
        Format_Sheet()

        row = 4
        column = 1


        For x As Integer = 0 To UBound(aFields)
            Get_Records()

            rs.open(sql, cn, 0, 1)

            If x Mod 3 = 0 Then
                row = row + 1
                column = 1
            End If

            wsActiveSheet.cells(row, column).formulaR1C1 = aFields(x)
            column = column + 1
            wsActiveSheet.cells(row, column).formulaR1C1 = rs.fields("Issues").value
            column = column + 1

            rs.close()

        Next
        wsactivesheet.Cells.EntireColumn.AutoFit()
    End Sub

    Sub Format_Sheet()

        wsactivesheet.Range("A1:F1").HorizontalAlignment = -4108
        wsactivesheet.Range("A1:F1").VerticalAlignment = -4107
        wsactivesheet.Range("A1:F1").WrapText = False
        wsactivesheet.Range("A1:F1").Orientation = 0
        wsactivesheet.Range("A1:F1").AddIndent = False
        wsactivesheet.Range("A1:F1").IndentLevel = 0
        wsactivesheet.Range("A1:F1").ShrinkToFit = False
        wsactivesheet.Range("A1:F1").ReadingOrder = -5002
        wsactivesheet.Range("A1:F1").MergeCells = False

        wsactivesheet.Range("A1:F1").Merge()

        wsactivesheet.Range("A1:F1").FormulaR1C1 = aTypes(i)

        wsactivesheet.Range("A1:F1").FormulaR1C1 = aTypes(i)


        wsactivesheet.Range("A2:F2").HorizontalAlignment = -4108
        wsactivesheet.Range("A2:F2").VerticalAlignment = -4107
        wsactivesheet.Range("A2:F2").WrapText = False
        wsactivesheet.Range("A2:F2").Orientation = 0
        wsactivesheet.Range("A2:F2").AddIndent = False
        wsactivesheet.Range("A2:F2").IndentLevel = 0
        wsactivesheet.Range("A2:F2").ShrinkToFit = False
        wsactivesheet.Range("A2:F2").ReadingOrder = -5002
        wsactivesheet.Range("A2:F2").MergeCells = False

        wsactivesheet.Range("A2:F2").Merge()

        wsActiveSheet.Range("A2:F2").FormulaR1C1 = "For " & SDate.Selected_Date & " to " & EDate.Selected_Date
        If ddRooms.SelectedValue <> 0 Then wsActiveSheet.Range("A2:F2").formulaR1C1 = "Room: " & ddRooms.SelectedItem.Text & " : For " & SDate.Selected_Date & " to " & EDate.Selected_Date
        wsactivesheet.Range("A3").FormulaR1C1 = "Area"

        wsactivesheet.Range("B3").FormulaR1C1 = "#"

        wsactivesheet.Range("C3").FormulaR1C1 = "Area #"

        wsactivesheet.Range("D3").FormulaR1C1 = "Area "

        wsactivesheet.Range("E3").FormulaR1C1 = ""

        wsactivesheet.Range("C3").FormulaR1C1 = "Area"

        wsactivesheet.Range("D3").FormulaR1C1 = "#"

        wsactivesheet.Range("E3").FormulaR1C1 = "Area"

        wsactivesheet.Range("F3").FormulaR1C1 = "#"
        wsactivesheet.Range("A40").Borders(5).LineStyle = -4142
        wsactivesheet.Range("A40").Borders(6).LineStyle = -4142

        wsactivesheet.Range("A40").Borders(7).LineStyle = 1
        wsactivesheet.Range("A40").Borders(7).Weight = 2
        wsactivesheet.Range("A40").Borders(7).ColorIndex = -4105
        wsactivesheet.Range("A40").Borders(8).LineStyle = 1
        wsactivesheet.Range("A40").Borders(8).Weight = 2
        wsactivesheet.Range("A40").Borders(8).ColorIndex = -4105
        wsactivesheet.Range("A40").Borders(9).LineStyle = 1
        wsactivesheet.Range("A40").Borders(9).Weight = 2
        wsactivesheet.Range("A40").Borders(9).ColorIndex = -4105


        wsactivesheet.Range("A40").Borders(10).LineStyle = 1
        wsactivesheet.Range("A40").Borders(10).Weight = 2
        wsactivesheet.Range("A40").Borders(10).ColorIndex = -4105

        wsactivesheet.Range("A40").Borders(11).LineStyle = -4142
        wsactivesheet.Range("A40").Borders(12).LineStyle = -4142
        wsactivesheet.Range("F40").Borders(5).LineStyle = -4142
        wsactivesheet.Range("F40").Borders(6).LineStyle = -4142
        With wsactivesheet.Range("F40").Borders(7)
            .LineStyle = 1
            .Weight = 2
            .ColorIndex = -4105
        End With
        With wsactivesheet.Range("F40").Borders(8)
            .LineStyle = 1
            .Weight = 2
            .ColorIndex = -4105
        End With
        With wsactivesheet.Range("F40").Borders(9)
            .LineStyle = 1
            .Weight = 2
            .ColorIndex = -4105
        End With
        With wsactivesheet.Range("F40").Borders(10)
            .LineStyle = 1
            .Weight = 2
            .ColorIndex = -4105
        End With
        'With wsactivesheet.Range("F40").Borders(11)
        '    .LineStyle = 1
        '    .Weight = 2
        '    .ColorIndex = -4105
        'End With
        'With wsactivesheet.Range("F40").Borders(12)
        '    .LineStyle = 1
        '    .Weight = 2
        '    .ColorIndex = -4105
        'End With

        With wsactivesheet.Range("A3:F40")
            .HorizontalAlignment = -4108
            .VerticalAlignment = -4107
            .WrapText = False
            .Orientation = 0
            .AddIndent = False
            .IndentLevel = 0
            .ShrinkToFit = False
            .ReadingOrder = -5002
            .MergeCells = False
        End With

        With wsactivesheet.Range("A4:A40")
            .HorizontalAlignment = -4131
            .VerticalAlignment = -4107
            .WrapText = False
            .Orientation = 0
            .AddIndent = False
            .IndentLevel = 0
            .ShrinkToFit = False
            .ReadingOrder = -5002
            .MergeCells = False
        End With

        With wsactivesheet.Range("C4:C40")
            .HorizontalAlignment = -4131
            .VerticalAlignment = -4107
            .WrapText = False
            .Orientation = 0
            .AddIndent = False
            .IndentLevel = 0
            .ShrinkToFit = False
            .ReadingOrder = -5002
            .MergeCells = False
        End With

        With wsactivesheet.Range("E4:E40")
            .HorizontalAlignment = -4131
            .VerticalAlignment = -4107
            .WrapText = False
            .Orientation = 0
            .AddIndent = False
            .IndentLevel = 0
            .ShrinkToFit = False
            .ReadingOrder = -5002
            .MergeCells = False
        End With

        With wsactivesheet.Range("F4:F40")
            .HorizontalAlignment = -4152
            .VerticalAlignment = -4107
            .WrapText = False
            .Orientation = 0
            .AddIndent = False
            .IndentLevel = 0
            .ShrinkToFit = False
            .ReadingOrder = -5002
            .MergeCells = False
        End With

        With wsactivesheet.Range("D4:D40")
            .HorizontalAlignment = -4152
            .VerticalAlignment = -4107
            .WrapText = False
            .Orientation = 0
            .AddIndent = False
            .IndentLevel = 0
            .ShrinkToFit = False
            .ReadingOrder = -5002
            .MergeCells = False
        End With

        With wsactivesheet.Range("B4:B40")
            .HorizontalAlignment = -4152
            .VerticalAlignment = -4107
            .WrapText = False
            .Orientation = 0
            .AddIndent = False
            .IndentLevel = 0
            .ShrinkToFit = False
            .ReadingOrder = -5002
            .MergeCells = False
        End With
        wsactivesheet.Cells.Font.Bold = True
        wsactivesheet.Cells.EntireColumn.AutoFit()
    End Sub


    Sub Write_Excel_Top10()
        'Top 10 Issues
        wsactivesheet = wb.worksheets("TOP 10")
        wsactivesheet.Range("A1:F1").HorizontalAlignment = -4108
        wsactivesheet.Range("A1:F1").VerticalAlignment = -4107
        wsactivesheet.Range("A1:F1").WrapText = False
        wsactivesheet.Range("A1:F1").Orientation = 0
        wsactivesheet.Range("A1:F1").AddIndent = False
        wsactivesheet.Range("A1:F1").IndentLevel = 0
        wsactivesheet.Range("A1:F1").ShrinkToFit = False
        wsactivesheet.Range("A1:F1").ReadingOrder = -5002
        wsactivesheet.Range("A1:F1").MergeCells = False

        wsactivesheet.Range("A1:F1").Merge()

        wsactivesheet.Range("A1:F1").FormulaR1C1 = "TOP 10 ISSUES"


        wsactivesheet.Range("A2:F2").HorizontalAlignment = -4108
        wsactivesheet.Range("A2:F2").VerticalAlignment = -4107
        wsactivesheet.Range("A2:F2").WrapText = False
        wsactivesheet.Range("A2:F2").Orientation = 0
        wsactivesheet.Range("A2:F2").AddIndent = False
        wsactivesheet.Range("A2:F2").IndentLevel = 0
        wsactivesheet.Range("A2:F2").ShrinkToFit = False
        wsactivesheet.Range("A2:F2").ReadingOrder = -5002
        wsactivesheet.Range("A2:F2").MergeCells = False

        wsactivesheet.Range("A2:F2").Merge()

        wsActiveSheet.Range("A2:F2").FormulaR1C1 = "For " & SDate.Selected_Date & " to " & EDate.Selected_Date
        If ddRooms.SelectedValue <> 0 Then wsActiveSheet.Range("A2:F2").formulaR1C1 = "Room: " & ddRooms.SelectedItem.Text & " : For " & SDate.Selected_Date & " to " & EDate.Selected_Date
        wsactivesheet.Range("A3").FormulaR1C1 = "Area"

        wsactivesheet.Range("B3").FormulaR1C1 = "#"

        wsactivesheet.Range("C3").FormulaR1C1 = "Area"

        wsactivesheet.Range("D3").FormulaR1C1 = "#"

        wsactivesheet.Range("E3").FormulaR1C1 = "Area"

        wsactivesheet.Range("F3").FormulaR1C1 = "#"

        Get_Top10()

        row = 4
        column = 1
        If rs.eof And rs.bof Then
        Else
            i = 0
            'Response.Write("<tr>")
            Do While Not rs.eof
                wsactivesheet.cells(row, column).formulaR1C1 = rs.fields("Fieldname").value & ""
                column = column + 1
                wsactivesheet.cells(row, column).formulaR1C1 = rs.fields("Issues").value
                column = column + 1
                i = i + 1
                If i = 10 Then Exit Do
                If i Mod 3 = 0 Then
                    row = row + 1
                    column = 1
                End If
                rs.movenext()
            Loop

        End If
        rs.close()
        wsactivesheet.Cells.Font.Bold = True
        wsactivesheet.Cells.EntireColumn.AutoFit()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            siRoomType.Connection_String = Resources.Resource.cns
            siRoomType.Label_Caption = ""
            siRoomType.ComboItem = "UnitType"
            siRoomType.Load_Items()
            Dim ds As New SqlDataSource
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "select 0 as RoomID, 'ALL' as RoomNumber union Select RoomID, RoomNumber from t_Room"
            ddRooms.DataSource = ds
            ddRooms.DataValueField = "RoomID"
            ddRooms.DataTextField = "RoomNumber"
            ddRooms.DataBind()
            ds = Nothing
        Else
            Lit.Text = ""
        End If
    End Sub
End Class
