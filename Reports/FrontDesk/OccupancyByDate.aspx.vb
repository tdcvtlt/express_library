
Partial Class Reports_FrontDesk_OccupancyByDate
    Inherits System.Web.UI.Page
    Dim cn As Object
    Dim rs As Object
    Dim sLink As String
    Dim sFileName As String
    Dim xlApp As Object
    Dim wb As Object
    Dim ws As Object
    Dim sql As String
    Dim Val As Decimal
    Dim row As Integer
    Dim column As Integer
    Dim sFormulas() As String
    Dim sFormula As String
    Dim sRows() As Integer
    Dim sSheets() As String
    Dim total(8) As Decimal
    Dim ctotal(8) As Decimal
    Dim ttotal(8) As Decimal
    Dim etotal(8) As Decimal
    Dim stotal(8) As Decimal
    Dim sctotal(8) As Decimal
    Dim sttotal(8) As Decimal
    Dim setotal(8) As Decimal

    Protected Sub btnRun_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRun.Click
        cn = Server.CreateObject("ADODB.Connection")
        rs = Server.CreateObject("ADODB.Recordset")

        Server.ScriptTimeout = 10000

        cn.open(Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
        Lit.Text = ""

        If CDate(SDate.Selected_Date) > CDate(EDate.Selected_Date) Then
            Lit.Text &= ("Invalid Date Parameters.")
        Else


            If Request("opt") = "1" Then
                'Create the Excel workbook
                sLink = Replace(Replace("Occupancy-" & Session.SessionID & "-" & Date.Today.ToShortDateString & "-" & Date.Today.ToShortTimeString & ".xls", "/", "-"), ":", "-")
                sFileName = "C:\inetpub\wwwroot\crms\reports\contracts\" & sLink

                xlApp = Server.CreateObject("Excel.Application")
                wb = xlApp.Workbooks.open("c:\inetpub\wwwroot\crms\reports\contracts\occupancytemplate.xls")
                wb.SaveAs(sFileName)
                wb = xlApp.Workbooks.Open(sFileName)
                For z = wb.worksheets.count To 1
                    wb.worksheets(z).delete()
                Next
            End If

            sql = "select * from ( " & _
                   "select month(m.dateallocated) as Month,ut.comboitem as UnitType, " & _
                   "	r.roomnumber,count(distinct m.dateallocated) nights,  " & _
                   "	case when sn.nights is null then " & _
                   "		0 " & _
                   "	else " & _
                   "		sn.nights " & _
                   "	end  as UsedNights, " & _
                   "	case when mn.nights is null then " & _
                   "		0 " & _
                   "	else " & _
                   "		mn.nights  " & _
                   "	end as Marketing, " & _
                   "	case when rn.nights is null then " & _
                   "		0 " & _
                   "	else " & _
                   "		rn.nights  " & _
                   "	end as Rental, " & _
                   "	case when dn.nights is null then " & _
                   "		0 " & _
                   "	else " & _
                   "		dn.nights  " & _
                   "	end as Developer, " & _
                   "	case when en.nights is null then " & _
                   "		0 " & _
                   "	else " & _
                   "		en.nights  " & _
                   "	end as Exchange, " & _
                   "	case when ow.nights is null then " & _
                   "		0 " & _
                   "	else " & _
                   "		ow.nights " & _
                   "	end as Owner, " & _
                   "	case when sp.nights is null then " & _
                   "		0 " & _
                   "	else " & _
                   "		sp.nights " & _
                   "	end as Spare, " & _
                   "	case when oos.nights is null then " & _
                   "		0 " & _
                   "	else " & _
                   "		oos.nights " & _
                   "	end as OutOfService " & _
                   "from t_Room r " & _
                   "	inner join t_RoomAllocationMatrix m on m.roomid=r.roomid  " & _
                   "	inner join t_Unit u on u.unitid = r.unitid  " & _
                   "	inner join t_Comboitems ut on ut.comboitemid = u.typeid " & _
                   "	left outer join  " & _
                   "		(select count(ms.dateallocated) as nights, ms.roomid, month(ms.dateallocated) as Mth from t_Roomallocationmatrix ms inner join t_Reservations r on r.reservationid = ms.reservationid inner join t_Comboitems rs on rs.comboitemid = r.statusid where ms.dateallocated between '" & SDate.Selected_Date & "' and '" & EDate.Selected_Date & "' and ms.reservationid >0 and rs.comboitem in ('completed', 'inhouse', 'in-house', 'in house') group by ms.roomid,month(ms.dateallocated)) as sn on sn.roomid = r.roomid and sn.mth = month(m.dateallocated) " & _
                   "	left outer join  " & _
                   "		(select count(ms.dateallocated) as nights, ms.roomid, month(ms.dateallocated) as Mth from t_Roomallocationmatrix ms inner join t_Reservations r on r.reservationid = ms.reservationid inner join t_Comboitems rs on rs.comboitemid = r.statusid left outer join t_Comboitems at on at.comboitemid = ms.typeid where ms.dateallocated between '" & SDate.Selected_Date & "' and '" & EDate.Selected_Date & "' and ms.reservationid >0 and rs.comboitem in ('completed', 'inhouse', 'in-house', 'in house') and at.Comboitem = 'Marketing' group by ms.roomid,month(ms.dateallocated)) as mn on mn.roomid = r.roomid and mn.mth = month(m.dateallocated) " & _
                   "	left outer join  " & _
                   "		(select count(ms.dateallocated) as nights, ms.roomid, month(ms.dateallocated) as Mth from t_Roomallocationmatrix ms inner join t_Reservations r on r.reservationid = ms.reservationid inner join t_Comboitems rs on rs.comboitemid = r.statusid left outer join t_Comboitems at on at.comboitemid = ms.typeid where ms.dateallocated between '" & SDate.Selected_Date & "' and '" & EDate.Selected_Date & "' and ms.reservationid >0 and rs.comboitem in ('completed', 'inhouse', 'in-house', 'in house') and at.Comboitem = 'Rental' group by ms.roomid,month(ms.dateallocated)) as rn on rn.roomid = r.roomid and rn.mth = month(m.dateallocated) " & _
                   "	left outer join  " & _
                   "       (select count(ms.dateallocated) as nights, ms.roomid, month(ms.dateallocated) as Mth from t_Roomallocationmatrix ms left outer join t_Reservations r on r.reservationid = ms.reservationid left outer join t_Comboitems rs on rs.comboitemid = r.statusid left outer join t_Comboitems at on at.comboitemid = ms.typeid where ms.dateallocated between '" & SDate.Selected_Date & "' and '" & EDate.Selected_Date & "' and (ms.reservationid >0 or ms.reservationid = -1) and (rs.comboitem is null or rs.comboitem in ('completed', 'inhouse', 'in-house', 'in house')) and at.Comboitem = 'Developer' group by ms.roomid,month(ms.dateallocated)) as dn on dn.roomid = r.roomid and dn.mth = month(m.dateallocated) " '						"		(select count(ms.dateallocated) as nights, ms.roomid, month(ms.dateallocated) as Mth from t_Roomallocationmatrix ms inner join t_Reservations r on r.reservationid = ms.reservationid inner join t_Comboitems rs on rs.comboitemid = r.statusid left outer join t_Comboitems at on at.comboitemid = ms.typeid where ms.dateallocated between '" & sdate & "' and '" & edate & "' and ms.reservationid >0 and rs.comboitem in ('completed', 'inhouse', 'in-house', 'in house') and at.Comboitem = 'Developer' group by ms.roomid,month(ms.dateallocated)) as dn on dn.roomid = r.roomid and dn.mth = month(m.dateallocated)) " & _
            sql = sql & "	left outer join  " & _
                  "		(select count(ms.dateallocated) as nights, ms.roomid, month(ms.dateallocated) as Mth from t_Roomallocationmatrix ms inner join t_Reservations r on r.reservationid = ms.reservationid inner join t_Comboitems rs on rs.comboitemid = r.statusid left outer join t_Comboitems at on at.comboitemid = ms.typeid where ms.dateallocated between '" & SDate.Selected_Date & "' and '" & EDate.Selected_Date & "' and ms.reservationid >0 and rs.comboitem in ('completed', 'inhouse', 'in-house', 'in house') and at.Comboitem = 'Exchange' group by ms.roomid,month(ms.dateallocated)) as en on en.roomid = r.roomid and en.mth = month(m.dateallocated) " & _
                  "	left outer join  " & _
                  "		(select count(ms.dateallocated) as nights, ms.roomid, month(ms.dateallocated) as Mth from t_Roomallocationmatrix ms inner join t_Reservations r on r.reservationid = ms.reservationid inner join t_Comboitems rs on rs.comboitemid = r.statusid left outer join t_Comboitems at on at.comboitemid = ms.typeid where ms.dateallocated between '" & SDate.Selected_Date & "' and '" & EDate.Selected_Date & "' and ms.reservationid >0 and rs.comboitem in ('completed', 'inhouse', 'in-house', 'in house') and at.Comboitem = 'Owner' group by ms.roomid,month(ms.dateallocated)) as ow on ow.roomid = r.roomid and ow.mth = month(m.dateallocated) " & _
                  "	left outer join  " & _
                  "		(select count(ms.dateallocated) as nights, ms.roomid, month(ms.dateallocated) as Mth from t_Roomallocationmatrix ms inner join t_Reservations r on r.reservationid = ms.reservationid inner join t_Comboitems rs on rs.comboitemid = r.statusid left outer join t_Comboitems at on at.comboitemid = ms.typeid where ms.dateallocated between '" & SDate.Selected_Date & "' and '" & EDate.Selected_Date & "' and ms.reservationid >0 and rs.comboitem in ('completed', 'inhouse', 'in-house', 'in house') and at.Comboitem = 'Spare' group by ms.roomid,month(ms.dateallocated)) as sp on sp.roomid = r.roomid and sp.mth = month(m.dateallocated) " & _
                  "	left outer join  " & _
                  "		(select count(ms.dateallocated) as nights, ms.roomid, month(ms.dateallocated) as Mth from t_Roomallocationmatrix ms left outer join t_Comboitems at on at.comboitemid = ms.typeid where ms.dateallocated between '" & SDate.Selected_Date & "' and '" & EDate.Selected_Date & "' and ms.reservationid =-1  group by ms.roomid,month(ms.dateallocated)) as oos on oos.roomid = r.roomid and oos.mth = month(m.dateallocated) " & _
                  "where m.dateallocated between '" & SDate.Selected_Date & "' and '" & EDate.Selected_Date & "' " & _
                  "group by month(m.dateallocated),ut.comboitem,r.roomnumber,sn.nights, mn.nights,dn.nights,en.nights, ow.nights,sp.nights,oos.nights,rn.nights " & _
                  ") a "
            Dim lastmonth As String = "0"
            Dim sqlWhere As String
            'Dim sSheets(), sRows()
            ReDim sSheets(0)
            sSheets(0) = ""
            ReDim sRows(0)
            sRows(0) = 0
            Dim i As Integer
            For i = 0 To DateDiff(DateInterval.Day, CDate(SDate.Selected_Date), CDate(EDate.Selected_Date))
                For z = 0 To 8
                    total(z) = 0
                    ctotal(z) = 0
                    ttotal(z) = 0
                    etotal(z) = 0
                Next

                sqlWhere = " where a.month = " & Month(CDate(SDate.Selected_Date).AddDays(i)) & " order by cast(left(RoomNumber,charindex('-',roomnumber)-1) as int), roomnumber "
                If lastmonth <> Month(CDate(SDate.Selected_Date).AddDays(i)) Then
                    lastmonth = Month(CDate(SDate.Selected_Date).AddDays(i))

                    'Open Recordset for the month
                    rs.open(sql & sqlWhere, cn, 0, 1)

                    If Request("opt") = "1" Then
                        'Create the Worksheet
                        'on error resume next
                        ws = wb.worksheets.add

                        'Rename the Worksheet 
                        ws.name = MonthName(lastmonth) & " " & Year(CDate(SDate.Selected_Date).AddDays(i))

                        'Create Headers
                        Create_WorkSheet_Titles()


                        'Fill in Values
                        'Dim row, column
                        row = 3
                        column = 1
                        Do While Not rs.eof
                            column = 1
                            row = row + 1
                            Write_Excel_Cell(MonthName(rs.fields("Month").value))
                            For x = 1 To 3
                                Write_Excel_Cell(rs.fields(x).value)
                            Next
                            For x = 4 To 11
                                Write_Excel_Cell(rs.fields(x).value)
                                'val = round(rs.fields(x).value / rs.fields(3).value,2)
                                'write_Excel_cell left(val,instr(val,".")-1) & "%"
                                Write_Excel_Cell("=(RC[-1]/RC[-" & (2 + (2 * (x - 4))) & "])")
                                ws.cells(row, column - 1).style = "Percent"
                            Next
                            rs.movenext()
                        Loop

                        'Build Totals
                        row = row + 1
                        column = 4

                        Write_Excel_Cell("=Sum(R[-" & row - 4 & "]C:R[-1]C)")
                        Format_Total(row, column - 1)
                        For x = 1 To 8
                            Write_Excel_Cell("=Sum(R[-" & row - 4 & "]C:R[-1]C)")
                            Format_Total(row, column - 1)
                            Write_Excel_Cell("=(RC[-1]/RC[-" & (2 + (2 * (x - 1))) & "])")
                            Format_Total(row, column - 1)
                            ws.cells(row, column - 1).style = "Percent"
                        Next

                        'Remember reference of Totals for Summary Page
                        Write_Totals()
                    Else
                        'Create Table
                        Lit.Text &= ("<table style = 'border:thin solid black;'>")

                        'Create Header
                        Lit.Text &= ("<tr>")
                        Lit.Text &= ("<th colspan = '20' align='center' style='border-bottom:thin solid black;'>")
                        Lit.Text &= (MonthName(Month(CDate(SDate.Selected_Date).AddDays(i))) & "&nbsp;" & Year(CDate(SDate.Selected_Date).AddDays(i)) & "<br />(" & SDate.Selected_Date & " to " & EDate.Selected_Date & ")")
                        Lit.Text &= ("</th>")
                        Lit.Text &= ("</tr>")

                        'Create Column Headers
                        'Top Row
                        Lit.Text &= ("<tr>")
                        Lit.Text &= ("<th style='border-right:thin solid black;'>&nbsp;</th>")
                        Lit.Text &= ("<th style='border-right:thin solid black;'>&nbsp;</th>")
                        Lit.Text &= ("<th style='border-right:thin solid black;'>&nbsp;</th>")
                        Lit.Text &= ("<th style='border-right:thin solid black;'>Nights</th>")
                        Lit.Text &= ("<th style='border-right:thin solid black;'>Nights</th>")
                        Lit.Text &= ("<th style='border-right:thin solid black;'>&nbsp;</th>")
                        Lit.Text &= ("<th colspan=2 style='border-right:thin solid black;border-bottom:thin solid black;'>Marketing</th>")
                        Lit.Text &= ("<th colspan=2 style='border-right:thin solid black;border-bottom:thin solid black;'>Rental</th>")
                        Lit.Text &= ("<th colspan=2 style='border-right:thin solid black;border-bottom:thin solid black;'>Developer</th>")
                        Lit.Text &= ("<th colspan=2 style='border-right:thin solid black;border-bottom:thin solid black;'>Exchange</th>")
                        Lit.Text &= ("<th colspan=2 style='border-right:thin solid black;border-bottom:thin solid black;'>Owner</th>")
                        Lit.Text &= ("<th colspan=2 style='border-right:thin solid black;border-bottom:thin solid black;'>Spare</th>")
                        Lit.Text &= ("<th colspan=2 style='border-bottom:thin solid black;'>Out Of Service</th>")
                        Lit.Text &= ("</tr>")
                        'Second Row
                        Lit.Text &= ("<tr>")
                        Lit.Text &= ("<th style='border-right:thin solid black;border-bottom:thin solid black;'>Month</th>")
                        Lit.Text &= ("<th style='border-right:thin solid black;border-bottom:thin solid black;'>Unit Type</th>")
                        Lit.Text &= ("<th style='border-right:thin solid black;border-bottom:thin solid black;'>Room</th>")
                        Lit.Text &= ("<th style='border-right:thin solid black;border-bottom:thin solid black;'>Allocated</th>")
                        Lit.Text &= ("<th style='border-right:thin solid black;border-bottom:thin solid black;'>Used</th>")
                        Lit.Text &= ("<th style='border-right:thin solid black;border-bottom:thin solid black;'>Occ %</th>")
                        Lit.Text &= ("<th style='border-right:thin solid black;border-bottom:thin solid black;'>Occ</th>")
                        Lit.Text &= ("<th style='border-right:thin solid black;border-bottom:thin solid black;'>%</th>")
                        Lit.Text &= ("<th style='border-right:thin solid black;border-bottom:thin solid black;'>Occ</th>")
                        Lit.Text &= ("<th style='border-right:thin solid black;border-bottom:thin solid black;'>%</th>")
                        Lit.Text &= ("<th style='border-right:thin solid black;border-bottom:thin solid black;'>Occ</th>")
                        Lit.Text &= ("<th style='border-right:thin solid black;border-bottom:thin solid black;'>%</th>")
                        Lit.Text &= ("<th style='border-right:thin solid black;border-bottom:thin solid black;'>Occ</th>")
                        Lit.Text &= ("<th style='border-right:thin solid black;border-bottom:thin solid black;'>%</th>")
                        Lit.Text &= ("<th style='border-right:thin solid black;border-bottom:thin solid black;'>Occ</th>")
                        Lit.Text &= ("<th style='border-right:thin solid black;border-bottom:thin solid black;'>%</th>")
                        Lit.Text &= ("<th style='border-right:thin solid black;border-bottom:thin solid black;'>Occ</th>")
                        Lit.Text &= ("<th style='border-right:thin solid black;border-bottom:thin solid black;'>%</th>")
                        Lit.Text &= ("<th style='border-right:thin solid black;border-bottom:thin solid black;'>Occ</th>")
                        Lit.Text &= ("<th style='border-bottom:thin solid black;'>%</th>")
                        Lit.Text &= ("</tr>")

                        'Fill in Values and Formulae

                        Do While Not rs.eof
                            Lit.Text &= ("<tr>")
                            Write_Cell(MonthName(rs.fields("Month").value))
                            For x = 1 To 3
                                Write_Cell(rs.fields(x).value)
                            Next
                            For x = 4 To 11
                                Write_Cell(rs.fields(x).value)
                                Val = FormatCurrency(Math.Round(rs.fields(x).value / rs.fields(3).value, 2))
                                Write_Cell(Left(Val, InStr(Val, ".") - 1) & "%")
                            Next


                            total(0) = total(0) + rs.fields(3).value
                            total(1) = total(1) + rs.fields(4).value
                            total(2) = total(2) + rs.fields(5).value
                            total(3) = total(3) + rs.fields(6).value
                            total(4) = total(4) + rs.fields(7).value
                            total(5) = total(5) + rs.fields(8).value
                            total(6) = total(6) + rs.fields(9).value
                            total(7) = total(7) + rs.fields(10).value
                            total(8) = total(8) + rs.fields(11).value

                            If rs.fields("Unittype").value = "Cottage" Then
                                For z = 0 To 8
                                    ctotal(z) = ctotal(z) + rs.fields(z + 3).value
                                    sctotal(z) = sctotal(z) + rs.fields(z + 3).value
                                Next
                            ElseIf rs.fields("Unittype").value = "Townes" Then
                                For z = 0 To 8
                                    ttotal(z) = ttotal(z) + rs.fields(z + 3).value
                                    sttotal(z) = sttotal(z) + rs.fields(z + 3).value
                                Next
                            ElseIf rs.fields("UnitType").value = "Estates" Then
                                For z = 0 To 8
                                    etotal(z) = etotal(z) + rs.fields(z + 3).value
                                    setotal(z) = setotal(z) + rs.fields(z + 3).value
                                Next
                            End If
                            For z = 0 To 8
                                stotal(z) = stotal(z) + rs.fields(z + 3).value
                            Next


                            rs.movenext()
                        Loop

                        'Build Totals
                        Lit.Text &= ("<tr>")
                        Write_Header("&nbsp;")
                        Write_Header("Totals:")
                        Write_Header("&nbsp;")
                        Write_Header(total(0))

                        For z = 1 To 8
                            Write_Header(total(z))

                            Val = FormatCurrency(Math.Round(total(z) / total(0), 2))

                            Write_Header(Left(Val, InStr(Val, ".") - 1) & "%")
                        Next


                        Lit.Text &= ("</tr>")

                        'Blank Row
                        Lit.Text &= ("<tr>")
                        For z = 0 To 19
                            Write_Cell("&nbsp;")
                        Next
                        Lit.Text &= ("</tr>")

                        'Cottage
                        Lit.Text &= ("<tr>")
                        Write_Total_Row(ctotal, "Cottage")
                        Lit.Text &= ("</tr>")

                        'Townes
                        Lit.Text &= ("<tr>")
                        Write_Total_Row(ttotal, "Townes")
                        Lit.Text &= ("</tr>")

                        'Estates
                        Lit.Text &= ("<tr>")
                        Write_Total_Row(etotal, "Estates")
                        Lit.Text &= ("</tr>")

                        'Blank Row
                        Lit.Text &= ("<tr>")
                        For z = 0 To 19
                            Write_Cell("&nbsp;")
                        Next
                        Lit.Text &= ("</tr>")

                        'Close Table
                        Lit.Text &= ("</table>")
                    End If

                    rs.close()
                End If
            Next

            If Request("opt") = "1" Then
                Create_Summary()
                wb.save()
                wb.close()
                wb = Nothing
                ws = Nothing
                xlApp = Nothing
                Lit.Text &= ("Your file is complete. Click on the link below to open it.<br>")
                Lit.Text &= ("<a href='../contracts/" & sLink & "'>" & sLink & "</a>")
            Else
                'Summary
                Lit.Text &= ("<table style='border:thin solid black;'>")

                'Create Header
                Lit.Text &= ("<tr>")
                Lit.Text &= ("<th colspan = '20' align='center' style='border-bottom:thin solid black;'>")
                Lit.Text &= ("SUMMARY<br />(" & SDate.Selected_Date & " to " & EDate.Selected_Date & ")")
                Lit.Text &= ("</th>")
                Lit.Text &= ("</tr>")

                'Create Column Headers
                'Top Row
                Lit.Text &= ("<tr>")
                Lit.Text &= ("<th style='border-right:thin solid black;'>&nbsp;</th>")
                Lit.Text &= ("<th style='border-right:thin solid black;'>&nbsp;</th>")
                Lit.Text &= ("<th style='border-right:thin solid black;'>&nbsp;</th>")
                Lit.Text &= ("<th style='border-right:thin solid black;'>Nights</th>")
                Lit.Text &= ("<th style='border-right:thin solid black;'>Nights</th>")
                Lit.Text &= ("<th style='border-right:thin solid black;'>&nbsp;</th>")
                Lit.Text &= ("<th colspan=2 style='border-right:thin solid black;border-bottom:thin solid black;'>Marketing</th>")
                Lit.Text &= ("<th colspan=2 style='border-right:thin solid black;border-bottom:thin solid black;'>Rental</th>")
                Lit.Text &= ("<th colspan=2 style='border-right:thin solid black;border-bottom:thin solid black;'>Developer</th>")
                Lit.Text &= ("<th colspan=2 style='border-right:thin solid black;border-bottom:thin solid black;'>Exchange</th>")
                Lit.Text &= ("<th colspan=2 style='border-right:thin solid black;border-bottom:thin solid black;'>Owner</th>")
                Lit.Text &= ("<th colspan=2 style='border-right:thin solid black;border-bottom:thin solid black;'>Spare</th>")
                Lit.Text &= ("<th colspan=2 style='border-bottom:thin solid black;'>Out Of Service</th>")
                Lit.Text &= ("</tr>")
                'Second Row
                Lit.Text &= ("<tr>")
                Lit.Text &= ("<th style='border-right:thin solid black;border-bottom:thin solid black;'>Month</th>")
                Lit.Text &= ("<th style='border-right:thin solid black;border-bottom:thin solid black;'>Unit Type</th>")
                Lit.Text &= ("<th style='border-right:thin solid black;border-bottom:thin solid black;'>Room</th>")
                Lit.Text &= ("<th style='border-right:thin solid black;border-bottom:thin solid black;'>Allocated</th>")
                Lit.Text &= ("<th style='border-right:thin solid black;border-bottom:thin solid black;'>Used</th>")
                Lit.Text &= ("<th style='border-right:thin solid black;border-bottom:thin solid black;'>Occ %</th>")
                Lit.Text &= ("<th style='border-right:thin solid black;border-bottom:thin solid black;'>Occ</th>")
                Lit.Text &= ("<th style='border-right:thin solid black;border-bottom:thin solid black;'>%</th>")
                Lit.Text &= ("<th style='border-right:thin solid black;border-bottom:thin solid black;'>Occ</th>")
                Lit.Text &= ("<th style='border-right:thin solid black;border-bottom:thin solid black;'>%</th>")
                Lit.Text &= ("<th style='border-right:thin solid black;border-bottom:thin solid black;'>Occ</th>")
                Lit.Text &= ("<th style='border-right:thin solid black;border-bottom:thin solid black;'>%</th>")
                Lit.Text &= ("<th style='border-right:thin solid black;border-bottom:thin solid black;'>Occ</th>")
                Lit.Text &= ("<th style='border-right:thin solid black;border-bottom:thin solid black;'>%</th>")
                Lit.Text &= ("<th style='border-right:thin solid black;border-bottom:thin solid black;'>Occ</th>")
                Lit.Text &= ("<th style='border-right:thin solid black;border-bottom:thin solid black;'>%</th>")
                Lit.Text &= ("<th style='border-right:thin solid black;border-bottom:thin solid black;'>Occ</th>")
                Lit.Text &= ("<th style='border-right:thin solid black;border-bottom:thin solid black;'>%</th>")
                Lit.Text &= ("<th style='border-right:thin solid black;border-bottom:thin solid black;'>Occ</th>")
                Lit.Text &= ("<th style='border-bottom:thin solid black;'>%</th>")
                Lit.Text &= ("</tr>")

                Lit.Text &= ("<tr>")

                Lit.Text &= ("<th style='border-bottom:thin solid black;border-right:thin solid black;'>Summary:</th>")
                For z = 1 To 19
                    Write_Cell("&nbsp;")
                Next
                Lit.Text &= ("</tr>")

                'Cottage
                Lit.Text &= ("<tr>")
                Write_Total_Row(sctotal, "Cottage")
                Lit.Text &= ("</tr>")

                'Townes
                Lit.Text &= ("<tr>")
                Write_Total_Row(sttotal, "Townes")
                Lit.Text &= ("</tr>")

                'Estates
                Lit.Text &= ("<tr>")
                Write_Total_Row(setotal, "Estates")
                Lit.Text &= ("</tr>")

                'Total
                Lit.Text &= ("<tr>")
                Write_Total_Row(stotal, "Total")
                Lit.Text &= ("</tr>")

                Lit.Text &= ("</table>")
            End If
        End If



        cn.close()

        cn = Nothing
        rs = Nothing

    End Sub
    

    Sub Write_Cell(ByVal value As String)
        Lit.Text &= ("<td align = 'center' style='border-bottom:thin solid black;border-right:thin solid black;'>")
        Lit.Text &= (value)
        Lit.Text &= ("</td>")
    End Sub

    Sub Write_Header(ByVal value As String)
        Lit.Text &= ("<th align = 'center' style='border-bottom:thin solid black;border-right:thin solid black;'>" & value & "</th>")
    End Sub

    Sub Write_Total_Row(ByVal arr() As Decimal, ByVal title As String)
        Write_Header("&nbsp;")
        Write_Header(title)
        Write_Header("&nbsp;")
        Write_Header(arr(0))

        For y = 1 To 8
            Write_Header(arr(y))

            Val = FormatCurrency(Math.Round(arr(y) / arr(0), 2))

            Write_Header(Left(Val, InStr(Val, ".") - 1) & "%")
        Next
    End Sub

    Sub Create_WorkSheet_Titles()
        ws.cells(3, "A") = "Month"
        ws.cells(3, "B") = "Unit Type"
        ws.cells(3, "C") = "Room"
        ws.cells(3, "A").font.bold = True
        ws.cells(3, "A").style.horizontalalignment = -4108
        ws.cells(3, "B").font.bold = True
        ws.cells(3, "B").style.horizontalalignment = -4108
        ws.cells(3, "C").font.bold = True
        ws.cells(3, "C").style.horizontalalignment = -4108

        Merge_Fields("D2:D3")
        ws.cells(2, "D") = "Nights Allocated"
        ws.cells(2, "D").font.bold = True
        ws.cells(2, "D").style.horizontalalignment = -4108
        ws.cells(2, "D").wraptext = True

        Merge_Fields("E2:E3")
        ws.cells(2, "E") = "Nights Used"
        ws.cells(2, "E").font.bold = True
        ws.cells(2, "E").style.horizontalalignment = -4108
        ws.cells(2, "E").wraptext = True

        Merge_Fields("F2:F3")
        ws.cells(2, "F") = "Occ %"
        ws.cells(2, "F").font.bold = True
        ws.cells(2, "F").style.horizontalalignment = -4108

        Merge_Fields("G1:T1")
        ws.cells(1, "G") = "Occupancy By Type"
        ws.cells(1, "G").font.bold = True
        ws.cells(1, "G").style.horizontalalignment = -4108

        Merge_Fields("G2:H2")
        ws.cells(2, "G") = "Marketing"
        ws.cells(3, "G") = "Occ"
        ws.cells(3, "H") = "%"
        ws.cells(2, "G").font.bold = True
        ws.cells(2, "G").style.horizontalalignment = -4108
        ws.cells(3, "G").font.bold = True
        ws.cells(3, "G").style.horizontalalignment = -4108
        ws.cells(3, "H").font.bold = True
        ws.cells(3, "H").style.horizontalalignment = -4108

        Merge_Fields("I2:J2")
        ws.cells(2, "I") = "Rental"
        ws.cells(3, "I") = "Occ"
        ws.cells(3, "J") = "%"
        ws.cells(2, "I").font.bold = True
        ws.cells(2, "I").style.horizontalalignment = -4108
        ws.cells(3, "I").font.bold = True
        ws.cells(3, "I").style.horizontalalignment = -4108
        ws.cells(3, "J").font.bold = True
        ws.cells(3, "J").style.horizontalalignment = -4108

        Merge_Fields("K2:L2")
        ws.cells(2, "K") = "Developer"
        ws.cells(3, "K") = "Occ"
        ws.cells(3, "L") = "%"
        ws.cells(2, "K").font.bold = True
        ws.cells(2, "K").style.horizontalalignment = -4108
        ws.cells(3, "K").font.bold = True
        ws.cells(3, "K").style.horizontalalignment = -4108
        ws.cells(3, "L").font.bold = True
        ws.cells(3, "L").style.horizontalalignment = -4108

        Merge_Fields("M2:N2")
        ws.cells(2, "M") = "Exchange"
        ws.cells(3, "M") = "Occ"
        ws.cells(3, "N") = "%"
        ws.cells(2, "M").font.bold = True
        ws.cells(2, "M").style.horizontalalignment = -4108
        ws.cells(3, "M").font.bold = True
        ws.cells(3, "M").style.horizontalalignment = -4108
        ws.cells(3, "N").font.bold = True
        ws.cells(3, "N").style.horizontalalignment = -4108

        Merge_Fields("O2:P2")
        ws.cells(2, "O") = "Owner"
        ws.cells(3, "O") = "Occ"
        ws.cells(3, "P") = "%"
        ws.cells(2, "O").font.bold = True
        ws.cells(2, "O").style.horizontalalignment = -4108
        ws.cells(3, "O").font.bold = True
        ws.cells(3, "O").style.horizontalalignment = -4108
        ws.cells(3, "P").font.bold = True
        ws.cells(3, "P").style.horizontalalignment = -4108

        Merge_Fields("Q2:R2")
        ws.cells(2, "Q") = "Spare"
        ws.cells(3, "Q") = "Occ"
        ws.cells(3, "R") = "%"
        ws.cells(2, "Q").font.bold = True
        ws.cells(2, "Q").style.horizontalalignment = -4108
        ws.cells(3, "Q").font.bold = True
        ws.cells(3, "Q").style.horizontalalignment = -4108
        ws.cells(3, "R").font.bold = True
        ws.cells(3, "R").style.horizontalalignment = -4108

        Merge_Fields("S2:T2")
        ws.cells(2, "S") = "Out Of Service"
        ws.cells(3, "S") = "Occ"
        ws.cells(3, "T") = "%"
        ws.cells(2, "S").font.bold = True
        ws.cells(2, "S").style.horizontalalignment = -4108
        ws.cells(3, "S").font.bold = True
        ws.cells(3, "S").style.horizontalalignment = -4108
        ws.cells(3, "T").font.bold = True
        ws.cells(3, "T").style.horizontalalignment = -4108


        'Range("B7").Select
        'Selection.Style = "Percent"
        'ActiveCell.FormulaR1C1 = "=SUM(R[-2]C:R[-1]C)"

    End Sub


    Sub Merge_Fields(ByVal Ranges As String)
        ws.range(Ranges).select()
        ws.range(Ranges).Merge()
    End Sub

    Sub Write_Excel_Cell(ByVal value As String)
        ws.cells(row, column).formulaR1C1 = value
        ws.cells(row, column).HorizontalAlignment = -4108
        ws.cells(row, column).font.bold = True
        column = column + 1
    End Sub

    Sub Write_Totals()
        row = row + 2
        If sSheets(ubound(sSheets)) <> "" Then
            ReDim Preserve sSheets(ubound(sSheets) + 1)
            ReDim Preserve sRows(ubound(sRows) + 1)
        End If
        sSheets(ubound(sSheets)) = "'" & ws.name & "'"
        sRows(ubound(sRows)) = row

        column = 1
        Write_Excel_Cell("Totals:")

        Write_Excel_Cell("Cottage")
        column = column + 1
        Write_Excel_Cell("=sumif(R[-" & row - 4 & "]C[-2]:R[-3]C[-2]," & chr(34) & "=Cottage" & chr(34) & ",R[-" & row - 4 & "]C:R[-3]C)")
        Write_Excel_Cell("=sumif(R[-" & row - 4 & "]C[-3]:R[-3]C[-3]," & chr(34) & "=Cottage" & chr(34) & ",R[-" & row - 4 & "]C:R[-3]C)")
        Write_Excel_Cell("=RC[-1]/RC[-2]")
        ws.cells(row, column - 1).style = "Percent"
        Write_Excel_Cell("=sumif(R[-" & row - 4 & "]C[-5]:R[-3]C[-5]," & chr(34) & "=Cottage" & chr(34) & ",R[-" & row - 4 & "]C:R[-3]C)")
        Write_Excel_Cell("=RC[-1]/RC[-4]")
        ws.cells(row, column - 1).style = "Percent"
        Write_Excel_Cell("=sumif(R[-" & row - 4 & "]C[-7]:R[-3]C[-7]," & chr(34) & "=Cottage" & chr(34) & ",R[-" & row - 4 & "]C:R[-3]C)")
        Write_Excel_Cell("=RC[-1]/RC[-6]")
        ws.cells(row, column - 1).style = "Percent"
        Write_Excel_Cell("=sumif(R[-" & row - 4 & "]C[-9]:R[-3]C[-9]," & chr(34) & "=Cottage" & chr(34) & ",R[-" & row - 4 & "]C:R[-3]C)")
        Write_Excel_Cell("=RC[-1]/RC[-8]")
        ws.cells(row, column - 1).style = "Percent"
        Write_Excel_Cell("=sumif(R[-" & row - 4 & "]C[-11]:R[-3]C[-11]," & chr(34) & "=Cottage" & chr(34) & ",R[-" & row - 4 & "]C:R[-3]C)")
        Write_Excel_Cell("=RC[-1]/RC[-10]")
        ws.cells(row, column - 1).style = "Percent"
        Write_Excel_Cell("=sumif(R[-" & row - 4 & "]C[-13]:R[-3]C[-13]," & chr(34) & "=Cottage" & chr(34) & ",R[-" & row - 4 & "]C:R[-3]C)")
        Write_Excel_Cell("=RC[-1]/RC[-12]")
        ws.cells(row, column - 1).style = "Percent"
        Write_Excel_Cell("=sumif(R[-" & row - 4 & "]C[-15]:R[-3]C[-15]," & chr(34) & "=Cottage" & chr(34) & ",R[-" & row - 4 & "]C:R[-3]C)")
        Write_Excel_Cell("=RC[-1]/RC[-14]")
        ws.cells(row, column - 1).style = "Percent"
        Write_Excel_Cell("=sumif(R[-" & row - 4 & "]C[-17]:R[-3]C[-17]," & chr(34) & "=Cottage" & chr(34) & ",R[-" & row - 4 & "]C:R[-3]C)")
        Write_Excel_Cell("=RC[-1]/RC[-16]")
        ws.cells(row, column - 1).style = "Percent"
        column = 2
        row = row + 1
        Write_Excel_Cell("Townes")
        column = 4
        Write_Excel_Cell("=sumif(R[-" & row - 4 & "]C[-2]:R[-4]C[-2]," & chr(34) & "=Townes" & chr(34) & ",R[-" & row - 4 & "]C:R[-4]C)")
        Write_Excel_Cell("=sumif(R[-" & row - 4 & "]C[-3]:R[-4]C[-3]," & chr(34) & "=Townes" & chr(34) & ",R[-" & row - 4 & "]C:R[-4]C)")
        Write_Excel_Cell("=RC[-1]/RC[-2]")
        ws.cells(row, column - 1).style = "Percent"
        Write_Excel_Cell("=sumif(R[-" & row - 4 & "]C[-5]:R[-4]C[-5]," & chr(34) & "=Townes" & chr(34) & ",R[-" & row - 4 & "]C:R[-4]C)")
        Write_Excel_Cell("=RC[-1]/RC[-4]")
        ws.cells(row, column - 1).style = "Percent"
        Write_Excel_Cell("=sumif(R[-" & row - 4 & "]C[-7]:R[-4]C[-7]," & chr(34) & "=Townes" & chr(34) & ",R[-" & row - 4 & "]C:R[-4]C)")
        Write_Excel_Cell("=RC[-1]/RC[-6]")
        ws.cells(row, column - 1).style = "Percent"
        Write_Excel_Cell("=sumif(R[-" & row - 4 & "]C[-9]:R[-4]C[-9]," & chr(34) & "=Townes" & chr(34) & ",R[-" & row - 4 & "]C:R[-4]C)")
        Write_Excel_Cell("=RC[-1]/RC[-8]")
        ws.cells(row, column - 1).style = "Percent"
        Write_Excel_Cell("=sumif(R[-" & row - 4 & "]C[-11]:R[-4]C[-11]," & chr(34) & "=Townes" & chr(34) & ",R[-" & row - 4 & "]C:R[-4]C)")
        Write_Excel_Cell("=RC[-1]/RC[-10]")
        ws.cells(row, column - 1).style = "Percent"
        Write_Excel_Cell("=sumif(R[-" & row - 4 & "]C[-13]:R[-4]C[-13]," & chr(34) & "=Townes" & chr(34) & ",R[-" & row - 4 & "]C:R[-4]C)")
        Write_Excel_Cell("=RC[-1]/RC[-12]")
        ws.cells(row, column - 1).style = "Percent"
        Write_Excel_Cell("=sumif(R[-" & row - 4 & "]C[-15]:R[-4]C[-15]," & chr(34) & "=Townes" & chr(34) & ",R[-" & row - 4 & "]C:R[-4]C)")
        Write_Excel_Cell("=RC[-1]/RC[-14]")
        ws.cells(row, column - 1).style = "Percent"
        Write_Excel_Cell("=sumif(R[-" & row - 4 & "]C[-17]:R[-4]C[-17]," & chr(34) & "=Townes" & chr(34) & ",R[-" & row - 4 & "]C:R[-4]C)")
        Write_Excel_Cell("=RC[-1]/RC[-16]")
        ws.cells(row, column - 1).style = "Percent"
        column = 2
        row = row + 1
        Write_Excel_Cell("Estates")
        column = 4
        Write_Excel_Cell("=sumif(R[-" & row - 4 & "]C[-2]:R[-5]C[-2]," & chr(34) & "=Estates" & chr(34) & ",R[-" & row - 4 & "]C:R[-5]C)")
        Write_Excel_Cell("=sumif(R[-" & row - 4 & "]C[-3]:R[-5]C[-3]," & chr(34) & "=Estates" & chr(34) & ",R[-" & row - 4 & "]C:R[-5]C)")
        Write_Excel_Cell("=RC[-1]/RC[-2]")
        ws.cells(row, column - 1).style = "Percent"
        Write_Excel_Cell("=sumif(R[-" & row - 4 & "]C[-5]:R[-5]C[-5]," & chr(34) & "=Estates" & chr(34) & ",R[-" & row - 4 & "]C:R[-5]C)")
        Write_Excel_Cell("=RC[-1]/RC[-4]")
        ws.cells(row, column - 1).style = "Percent"
        Write_Excel_Cell("=sumif(R[-" & row - 4 & "]C[-7]:R[-5]C[-7]," & chr(34) & "=Estates" & chr(34) & ",R[-" & row - 4 & "]C:R[-5]C)")
        Write_Excel_Cell("=RC[-1]/RC[-6]")
        ws.cells(row, column - 1).style = "Percent"
        Write_Excel_Cell("=sumif(R[-" & row - 4 & "]C[-9]:R[-5]C[-9]," & chr(34) & "=Estates" & chr(34) & ",R[-" & row - 4 & "]C:R[-5]C)")
        Write_Excel_Cell("=RC[-1]/RC[-8]")
        ws.cells(row, column - 1).style = "Percent"
        Write_Excel_Cell("=sumif(R[-" & row - 4 & "]C[-11]:R[-5]C[-11]," & chr(34) & "=Estates" & chr(34) & ",R[-" & row - 4 & "]C:R[-5]C)")
        Write_Excel_Cell("=RC[-1]/RC[-10]")
        ws.cells(row, column - 1).style = "Percent"
        Write_Excel_Cell("=sumif(R[-" & row - 4 & "]C[-13]:R[-5]C[-13]," & chr(34) & "=Estates" & chr(34) & ",R[-" & row - 4 & "]C:R[-5]C)")
        Write_Excel_Cell("=RC[-1]/RC[-12]")
        ws.cells(row, column - 1).style = "Percent"
        Write_Excel_Cell("=sumif(R[-" & row - 4 & "]C[-15]:R[-5]C[-15]," & chr(34) & "=Estates" & chr(34) & ",R[-" & row - 4 & "]C:R[-5]C)")
        Write_Excel_Cell("=RC[-1]/RC[-14]")
        ws.cells(row, column - 1).style = "Percent"
        Write_Excel_Cell("=sumif(R[-" & row - 4 & "]C[-17]:R[-5]C[-17]," & chr(34) & "=Estates" & chr(34) & ",R[-" & row - 4 & "]C:R[-5]C)")
        Write_Excel_Cell("=RC[-1]/RC[-16]")
        ws.cells(row, column - 1).style = "Percent"
        row = row + 1
        column = 4
        Write_Excel_Cell("=sum(R[-3]C:R[-1]C)")
        Format_Total(row, column - 1)
        For x = 1 To 8
            Write_Excel_Cell("=sum(R[-3]C:R[-1]C)")
            Write_Excel_Cell("=(RC[-1]/RC[-" & (2 + (2 * (x - 1))) & "])")
            ws.cells(row, column - 1).style = "Percent"
            format_Total(row, column - 2)
            format_Total(row, column - 1)
        Next
        Page_Setup()
    End Sub

    Sub Page_Setup()
        ws.Range("G1:R1").Select()
        ws.Range("G1:R1").Borders(5).LineStyle = -4142
        ws.Range("G1:R1").Borders(6).LineStyle = -4142
        ws.Range("G1:R1").Borders(7).LineStyle = -4142
        ws.Range("G1:R1").Borders(8).LineStyle = -4142
        With ws.Range("G1:R1").Borders(9)
            .LineStyle = 1
            .Weight = 2
            .ColorIndex = -4105
        End With
        ws.Range("G1:R1").Borders(10).LineStyle = -4142
        ws.Range("G1:R1").Borders(11).LineStyle = -4142
        ws.Rows("3:3").Select()
        ws.Rows("3:3").Borders(5).LineStyle = -4142
        ws.Rows("3:3").Borders(6).LineStyle = -4142
        ws.Rows("3:3").Borders(7).LineStyle = -4142
        ws.Rows("3:3").Borders(8).LineStyle = -4142
        With ws.Rows("3:3").Borders(9)
            .LineStyle = 1
            .Weight = 2
            .ColorIndex = -4105
        End With
        ws.Rows("3:3").Borders(10).LineStyle = -4142
        ws.Rows("3:3").Borders(11).LineStyle = -4142
        ws.Range("C5").Select()
    End Sub

    Sub Format_Total(ByVal r As Integer, ByVal c As Integer)
        ws.cells(r, c).borders(5).linestyle = -4142
        ws.cells(r, c).borders(6).linestyle = -4142
        ws.cells(r, c).borders(7).linestyle = -4142
        ws.cells(r, c).borders(8).linestyle = 1
        ws.cells(r, c).borders(8).Weight = 2
        ws.cells(r, c).borders(8).ColorIndex = -4105
        ws.cells(r, c).borders(9).linestyle = -4119
        ws.cells(r, c).borders(9).Weight = 4
        ws.cells(r, c).borders(9).ColorIndex = -4105
        ws.cells(r, c).borders(10).linestyle = -4142
    End Sub

    Sub Create_Summary()
        'Exit sub
        ws = wb.worksheets.add
        ws.name = "Summary"
        Create_Worksheet_Titles()
        row = 4
        column = 1
        write_excel_cell("Totals:")
        write_excel_cell("Cottage")
        column = column + 1

        sFormula = "="
        For i = 0 To ubound(sSheets)
            If i > 0 Then sFormula = sFormula & "+"
            sFormula = sFormula & sSheets(i) & "!R[" & sRows(i) - 4 & "]C"
        Next
        write_excel_cell(sformula)
        For x = 1 To 8
            write_excel_cell(sFormula)
            Write_Excel_Cell("=(RC[-1]/RC[-" & (2 + (2 * (x - 1))) & "])")
            ws.cells(row, column - 1).style = "Percent"
            'format_Total row,column-2
            'format_Total row,column-1
        Next
        row = row + 1
        column = 2
        write_excel_cell("Townes")
        column = 4
        sFormula = "="
        For i = 0 To ubound(sSheets)
            If i > 0 Then sFormula = sFormula & "+"
            sFormula = sFormula & sSheets(i) & "!R[" & sRows(i) - 4 & "]C"
        Next
        write_excel_cell(sformula)
        For x = 1 To 8
            write_excel_cell(sFormula)
            Write_Excel_Cell("=(RC[-1]/RC[-" & (2 + (2 * (x - 1))) & "])")
            ws.cells(row, column - 1).style = "Percent"
            'format_Total row,column-2
            'format_Total row,column-1
        Next
        row = row + 1
        column = 2

        write_excel_cell("Estates")
        column = 4
        sFormula = "="
        For i = 0 To ubound(sSheets)
            If i > 0 Then sFormula = sFormula & "+"
            sFormula = sFormula & sSheets(i) & "!R[" & sRows(i) - 4 & "]C"
        Next
        write_excel_cell(sformula)
        For x = 1 To 8
            write_excel_cell(sFormula)
            Write_Excel_Cell("=(RC[-1]/RC[-" & (2 + (2 * (x - 1))) & "])")
            ws.cells(row, column - 1).style = "Percent"
            'format_Total row,column-2
            'format_Total row,column-1
        Next
        row = row + 1
        column = 4
        Write_Excel_Cell("=sum(R[-3]C:R[-1]C)")
        Format_Total(row, column - 1)
        For x = 1 To 8
            Write_Excel_Cell("=sum(R[-3]C:R[-1]C)")
            Write_Excel_Cell("=(RC[-1]/RC[-" & (2 + (2 * (x - 1))) & "])")
            ws.cells(row, column - 1).style = "Percent"
            format_Total(row, column - 2)
            format_Total(row, column - 1)
        Next


        ws.cells(3, 1) = ""
        ws.cells(3, 3) = ""
        Page_Setup()
    End Sub
End Class
