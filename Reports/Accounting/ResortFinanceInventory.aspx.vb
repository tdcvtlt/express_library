
Partial Class Reports_Accounting_ResortFinanceInventory
    Inherits System.Web.UI.Page
    Dim COTTAGE(3, 12) As String
    Dim TOWNES(7, 12) As String
    Dim ESTATES(21, 12) As String
    Dim COMBOS(1, 12) As String
    Dim FOOTER(3, 12) As String
    Protected Sub btnScreen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnScreen.Click
        Report()
    End Sub

    Private Sub Report()
        Dim cn As Object
        Dim rs As Object

        cn = Server.CreateObject("ADODB.Connection")
        rs = Server.CreateObject("ADODB.Recordset")

        'ARRAY FORMATS

        'COMBOS(X,Y)
        '   X = TYPE
        '   0 = COMBO RED ANNUAL
        '   1 = COMBO WHITE ANNUAL

        'COTTAGES(X,Y)
        '	X = TYPE 
        '		0 = 3 BR RED ANNUAL
        '		1 = 3 BR RED BIENNIAL
        '		2 = 3 BR WHITE ANNUAL
        '		3 = 3 BR WHITE BIENNIAL


        'TOWNES (X, Y)
        '	X = TYPE
        '		0 = 2 BR RED ANNUAL
        '		1 = 2 BR RED BIENNIAL
        '		2 = 4 BR RED ANNUAL
        '		3 = 4 BR RED BIENNIAL
        '		4 = 2 BR WHITE ANNUAL
        '		5 = 2 BR WHITE BIENNIAL
        '		6 = 4 BR WHITE ANNUAL
        '		7 = 4 BR WHITE BIENNIAL
        
        'ESTATES (X, Y)
        '	X = TYPE
        '		0 = 1 BR RED ANNUAL
        '		1 = 1 BR RED BIENNIAL
        '		2 = 2 BR RED ANNUAL
        '		3 = 2 BR RED BIENNIAL
        '		4 = 3 BR RED ANNUAL
        '		5 = 3 BR RED BIENNIAL
        '		6 = 4 BR RED ANNUAL
        '		7 = 4 BR RED BIENNIAL
        '		8 = 4 BR RED TRIENNIAL
        '		9 = 1 BR WHITE ANNUAL
        '		10 = 1 BR WHITE BIENNIAL
        '		11 = 2 BR WHITE ANNUAL
        '		12 = 2 BR WHITE BIENNIAL
        '		13 = 3 BR WHITE ANNUAL
        '		14 = 3 BR WHITE BIENNIAL
        '		15 = 4 BR WHITE ANNUAL
        '		16 = 4 BR WHITE BIENNIAL
        '		17 = 4 BR WHITE TRIENNIAL
        '       18 = 1 BR RED TRIENNIAL
        '       19 = 2 BR RED TRIENNIAL
        '       20 = 1 BR WHITE TRIENNIAL
        '       21 = 2 BR WHITE TRIENNIAL
        

        'Y = VALUES
        '		0 = SIZE
        '		1 = SEASON
        '		2 = FREQUENCY
        '		3 = NUM AVAILABLE
        '		4 = UNSOLD VOLUME
        '		5 = NUM SOLD
        '		6 = SOLD VOLUME
        '		7 = PENDER COUNT
        '		8 = PENDER VOLUME
        '		9 = TOTAL VOLUME
        '		10 = CURRENT SALES PRICE
        '       11 = CURRENT MAINTENANCE FEE
        '       12 = TOTAL MAINTENANCE FEE

        

        'SET DEFAULTS TO 0
        SET_DEFAULTS()

        'SET SIZES
        For I = 0 To UBound(COMBOS)
            COMBOS(I, 0) = "5"
            COMBOS(I, 2) = "ANNUAL"
            If I < 1 Then
                COMBOS(I, 1) = "RED"
            Else
                COMBOS(I, 1) = "WHITE"
            End If
            COMBOS(I, 11) = 730.0
        Next

        For I = 0 To UBound(COTTAGE)
            COTTAGE(I, 0) = "3"
            If I < 2 Then
                COTTAGE(I, 1) = "RED"
            Else
                COTTAGE(I, 1) = "WHITE"
            End If
            If I Mod 2 = 0 Then
                COTTAGE(I, 2) = "ANNUAL"
            Else
                COTTAGE(I, 2) = "BIENNIAL"
            End If
            COTTAGE(I, 11) = 730.0 'MAINTENANCE FEE
        Next

        For I = 0 To UBound(TOWNES)
            If I = 0 Or I = 1 Or I = 4 Or I = 5 Then
                TOWNES(I, 0) = "2"
                TOWNES(I, 11) = 585.0 'MAINTENANCE FEE
            Else
                TOWNES(I, 0) = "4"
                TOWNES(I, 11) = 1040.0 'MAINTENANCE FEE
            End If
            If I < 4 Then
                TOWNES(I, 1) = "RED"
            Else
                TOWNES(I, 1) = "WHITE"
            End If
            If I Mod 2 = 0 Then
                TOWNES(I, 2) = "ANNUAL"
            Else
                TOWNES(I, 2) = "BIENNIAL"
            End If
        Next

        For I = 0 To UBound(ESTATES)
            If I = 0 Or I = 1 Or I = 9 Or I = 10 Or I = 18 Or I = 20 Then
                ESTATES(I, 0) = "1"
                ESTATES(I, 11) = 355.0 'MAINTENANCE FEE
            ElseIf I = 2 Or I = 3 Or I = 11 Or I = 12 Or I = 19 Or I = 21 Then
                ESTATES(I, 0) = "2"
                ESTATES(I, 11) = 615.0 'MAINTENANCE FEE
            ElseIf I = 4 Or I = 5 Or I = 13 Or I = 14 Then
                ESTATES(I, 0) = "3"
                ESTATES(I, 11) = 695.0 'MAINTENANCE FEE
            Else
                ESTATES(I, 0) = "4"
                ESTATES(I, 11) = 1040.0 'MAINTENANCE FEE
            End If
            If I < 9 Or I = 18 Or I = 19 Then
                ESTATES(I, 1) = "RED"
            Else
                ESTATES(I, 1) = "WHITE"
            End If
            If I = 8 Or I >= 17 Then
                ESTATES(I, 2) = "TRIENNIAL"
            ElseIf (I Mod 2 = 0 And I < 9) Or (I Mod 2 <> 0 And I > 8) Then
                ESTATES(I, 2) = "ANNUAL"
            ElseIf (I Mod 2 = 0 And I > 9) Or (I Mod 2 <> 0 And I < 10) Then
                ESTATES(I, 2) = "BIENNIAL"
            Else
                ESTATES(I, 2) = "BIENNIAL"
            End If
        Next

        'SET DEFAULT SALES PRICES
        COMBOS(0, 10) = 0
        COMBOS(1, 10) = 0

        COTTAGE(0, 10) = 19900
        COTTAGE(1, 10) = 11900
        COTTAGE(2, 10) = 8900
        COTTAGE(3, 10) = 7900


        TOWNES(0, 10) = 14900
        TOWNES(1, 10) = 9900
        TOWNES(2, 10) = 27900
        TOWNES(3, 10) = 16900
        TOWNES(4, 10) = 8900
        TOWNES(5, 10) = 7900
        TOWNES(6, 10) = 12900
        TOWNES(7, 10) = 8900

        ESTATES(0, 10) = 13900
        ESTATES(1, 10) = 8900
        ESTATES(2, 10) = 0
        ESTATES(3, 10) = 0
        ESTATES(4, 10) = 32900
        ESTATES(5, 10) = 18900
        ESTATES(6, 10) = 44900
        ESTATES(7, 10) = 26900
        ESTATES(8, 10) = 17900
        ESTATES(9, 10) = 5900
        ESTATES(10, 10) = 4900
        ESTATES(11, 10) = 0
        ESTATES(12, 10) = 0
        ESTATES(13, 10) = 10900
        ESTATES(14, 10) = 7900
        ESTATES(15, 10) = 12900
        ESTATES(16, 10) = 8900
        ESTATES(17, 10) = 0
        ESTATES(18, 10) = 0
        ESTATES(19, 10) = 0
        ESTATES(20, 10) = 0
        ESTATES(21, 10) = 0

        Dim sql As String
        cn.open(Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)

        


        '****** AVAILABLE ******

        sql = "select SaleType,case when i.week < 9 then 'White' else 'Red' end  as season, BD, frequency, count(distinct c.contractid) as Available " & _
          "from v_Contractinventory ci " & _
          "	inner join t_Contract c on c.contractid = ci.contractid " & _
          "	inner join t_Prospect p on p.prospectid = c.prospectid " & _
          "	inner join t_Soldinventory si on si.contractid = c.contractid " & _
          "	inner join t_Salesinventory i on i.salesinventoryid = si.salesinventoryid  " & _
          "where p.prospectid in (select prospectid from t_Prospect where FirstName = 'Resort' and LastName like 'finance%') " & _
          "group by SaleType, case when i.week < 9 then 'White' else 'Red' end , BD, frequency " & _
          "order by SaleType, case when i.week < 9 then 'White' else 'Red' end , BD, frequency "

        rs.OPEN(SQL, cn, 0, 1)
        Do While Not rs.EOF
            If UCase(rs.FIELDS("SALETYPE").VALUE & "") = "COTTAGE" Then
                For I = 0 To UBound(COTTAGE)
                    If COTTAGE(I, 1) = UCase(Trim(rs.fields("Season").value & "")) And COTTAGE(I, 2) = UCase(Trim(rs.FIELDS("FREQUENCY").VALUE & "")) Then
                        COTTAGE(I, 3) = rs.FIELDS("AVAILABLE").VALUE
                        COTTAGE(I, 4) = CDbl(COTTAGE(I, 3)) * CDbl(COTTAGE(I, 10))
                        FOOTER(0, 3) = CDbl(FOOTER(0, 3)) + CDbl(COTTAGE(I, 3))
                        FOOTER(0, 4) = CDbl(FOOTER(0, 4)) + CDbl(COTTAGE(I, 4))
                        Exit For
                    End If
                Next
            ElseIf UCase(rs.FIELDS("SALETYPE").VALUE & "") = "TOWNES" Then
                For I = 0 To UBound(TOWNES)
                    If TOWNES(I, 1) = UCase(Trim(rs.fields("Season").value & "")) And TOWNES(I, 2) = UCase(Trim(rs.FIELDS("FREQUENCY").VALUE & "")) And TOWNES(I, 0) & "BD" = rs.FIELDS("BD").VALUE & "" Then
                        TOWNES(I, 3) = rs.FIELDS("AVAILABLE").VALUE
                        TOWNES(I, 4) = CDbl(TOWNES(I, 3)) * CDbl(TOWNES(I, 10))
                        FOOTER(1, 3) = CDbl(FOOTER(1, 3)) + CDbl(TOWNES(I, 3))
                        FOOTER(1, 4) = CDbl(FOOTER(1, 4)) + CDbl(TOWNES(I, 4))
                        Exit For
                    End If
                Next
            ElseIf UCase(rs.FIELDS("SALETYPE").VALUE & "") = "ESTATES" Then
                For I = 0 To UBound(ESTATES)
                    If ESTATES(I, 1) = UCase(Trim(rs.fields("Season").value & "")) And ESTATES(I, 2) = UCase(Trim(rs.FIELDS("FREQUENCY").VALUE & "")) And ESTATES(I, 0) & "BD" = rs.FIELDS("BD").VALUE & "" Then
                        ESTATES(I, 3) = rs.FIELDS("AVAILABLE").VALUE
                        ESTATES(I, 4) = CDbl(ESTATES(I, 3)) * CDbl(ESTATES(I, 10))
                        FOOTER(2, 3) = CDbl(FOOTER(2, 3)) + CDbl(ESTATES(I, 3))
                        FOOTER(2, 4) = CDbl(FOOTER(2, 4)) + CDbl(ESTATES(I, 4))
                        Exit For
                    End If
                Next
            ElseIf UCase(rs.FIELDS("SALETYPE").VALUE & "") = "COMBO" Then
                For I = 0 To UBound(COMBOS)
                    If COMBOS(I, 1) = UCase(Trim(rs.fields("Season").value & "")) And COMBOS(I, 2) = UCase(Trim(rs.FIELDS("FREQUENCY").VALUE & "")) And "Combo" = rs.FIELDS("BD").VALUE & "" Then
                        COMBOS(I, 3) = rs.FIELDS("AVAILABLE").VALUE
                        COMBOS(I, 4) = CDbl(COMBOS(I, 3)) * CDbl(COMBOS(I, 10))
                        FOOTER(3, 3) = CDbl(FOOTER(3, 3)) + CDbl(COMBOS(I, 3))
                        FOOTER(3, 4) = CDbl(FOOTER(3, 4)) + CDbl(COMBOS(I, 4))
                        Exit For
                    End If
                Next
            Else
            End If
            rs.MOVENEXT()
        Loop
        rs.CLOSE()

        '****** SOLD *******

        SQL = "select SaleType,case when i.week < 9 then 'White' else 'Red' end  as season, BD, ci.frequency, count(distinct c.contractid) as Available, sum(m.salesvolume) as Volume " & _
          "from v_Contractinventory ci " & _
          "	inner join t_Contract c on c.contractid = ci.contractid " & _
          "	inner join t_Mortgage m on m.contractid = c.contractid " & _
          "	inner join t_Prospect p on p.prospectid = c.prospectid " & _
          "	inner join t_Soldinventory si on si.contractid = c.contractid " & _
          "	inner join t_Salesinventory i on i.salesinventoryid = si.salesinventoryid  " & _
          "	left outer join t_Comboitems cs on cs.comboitemid = c.statusid " & _
          "where c.contractnumber like 'x%' and (cs.comboitem is null or cs.comboitem not like  'Pen%') " & _
          "group by SaleType, case when i.week < 9 then 'White' else 'Red' end , BD, ci.frequency " & _
          "order by SaleType, case when i.week < 9 then 'White' else 'Red' end , BD, ci.frequency "

        rs.OPEN(SQL, cn, 0, 1)
        Do While Not rs.EOF
            If UCase(rs.FIELDS("SALETYPE").VALUE & "") = "COTTAGE" Then
                For I = 0 To UBound(COTTAGE)
                    If COTTAGE(I, 1) = UCase(Trim(rs.fields("Season").value & "")) And COTTAGE(I, 2) = UCase(Trim(rs.FIELDS("FREQUENCY").VALUE & "")) Then
                        COTTAGE(I, 5) = rs.FIELDS("AVAILABLE").VALUE
                        COTTAGE(I, 6) = rs.FIELDS("VOLUME").VALUE
                        FOOTER(0, 5) = CDbl(FOOTER(0, 5)) + CDbl(COTTAGE(I, 5))
                        FOOTER(0, 6) = CDbl(FOOTER(0, 6)) + CDbl(COTTAGE(I, 6))
                        Exit For
                    End If
                Next
            ElseIf UCase(rs.FIELDS("SALETYPE").VALUE & "") = "TOWNES" Then
                For I = 0 To UBound(TOWNES)
                    If TOWNES(I, 1) = UCase(Trim(rs.fields("Season").value & "")) And TOWNES(I, 2) = UCase(Trim(rs.FIELDS("FREQUENCY").VALUE & "")) And TOWNES(I, 0) & "BD" = rs.FIELDS("BD").VALUE & "" Then
                        TOWNES(I, 5) = rs.FIELDS("AVAILABLE").VALUE
                        TOWNES(I, 6) = rs.FIELDS("VOLUME").VALUE
                        FOOTER(1, 5) = CDbl(FOOTER(1, 5)) + CDbl(TOWNES(I, 5))
                        FOOTER(1, 6) = CDbl(FOOTER(1, 6)) + CDbl(TOWNES(I, 6))
                        Exit For
                    End If
                Next
            ElseIf UCase(rs.FIELDS("SALETYPE").VALUE & "") = "ESTATES" Then
                For I = 0 To UBound(ESTATES)
                    If ESTATES(I, 1) = UCase(Trim(rs.fields("Season").value & "")) And ESTATES(I, 2) = UCase(Trim(rs.FIELDS("FREQUENCY").VALUE & "")) And ESTATES(I, 0) & "BD" = rs.FIELDS("BD").VALUE & "" Then
                        ESTATES(I, 5) = rs.FIELDS("AVAILABLE").VALUE
                        ESTATES(I, 6) = rs.FIELDS("VOLUME").VALUE
                        FOOTER(2, 5) = CDbl(FOOTER(2, 5)) + CDbl(ESTATES(I, 5))
                        FOOTER(2, 6) = CDbl(FOOTER(2, 6)) + CDbl(ESTATES(I, 6))
                        Exit For
                    End If
                Next
            ElseIf UCase(rs.FIELDS("SALETYPE").VALUE & "") = "COMBO" Then
                For I = 0 To UBound(COMBOS)
                    If COMBOS(I, 1) = UCase(Trim(rs.fields("Season").value & "")) And COMBOS(I, 2) = UCase(Trim(rs.FIELDS("FREQUENCY").VALUE & "")) And COMBOS(I, 0) & "BD" = rs.FIELDS("BD").VALUE & "" Then
                        COMBOS(I, 5) = rs.FIELDS("AVAILABLE").VALUE
                        COMBOS(I, 6) = rs.FIELDS("VOLUME").VALUE
                        FOOTER(3, 5) = CDbl(FOOTER(3, 5)) + CDbl(COMBOS(I, 5))
                        FOOTER(3, 6) = CDbl(FOOTER(3, 6)) + CDbl(COMBOS(I, 6))
                        Exit For
                    End If
                Next
            Else
            End If
            rs.MOVENEXT()
        Loop
        rs.CLOSE()

        '****** PENDERS ******

        SQL = "select SaleType,case when i.week < 9 then 'White' else 'Red' end  as season, BD, ci.frequency, count(distinct c.contractid) as Available, sum(m.salesvolume) as Volume " & _
          "from v_Contractinventory ci " & _
          "	inner join t_Contract c on c.contractid = ci.contractid " & _
          "	inner join t_Mortgage m on m.contractid = c.contractid " & _
          "	inner join t_Prospect p on p.prospectid = c.prospectid " & _
          "	inner join t_Soldinventory si on si.contractid = c.contractid " & _
          "	inner join t_Salesinventory i on i.salesinventoryid = si.salesinventoryid  " & _
          "	left outer join t_Comboitems cs on cs.comboitemid = c.statusid " & _
          "where c.contractnumber like 'x%' and (cs.comboitem like 'Pen%') " & _
          "group by SaleType, case when i.week < 9 then 'White' else 'Red' end , BD, ci.frequency " & _
          "order by SaleType, case when i.week < 9 then 'White' else 'Red' end , BD, ci.frequency "


        rs.OPEN(SQL, cn, 0, 1)
        Do While Not rs.EOF
            If UCase(rs.FIELDS("SALETYPE").VALUE & "") = "COTTAGE" Then
                For I = 0 To UBound(COTTAGE)
                    If COTTAGE(I, 1) = UCase(Trim(rs.fields("Season").value & "")) And COTTAGE(I, 2) = UCase(Trim(rs.FIELDS("FREQUENCY").VALUE & "")) Then
                        COTTAGE(I, 7) = rs.FIELDS("AVAILABLE").VALUE
                        COTTAGE(I, 8) = rs.FIELDS("VOLUME").VALUE
                        FOOTER(0, 7) = CDbl(FOOTER(0, 7)) + CDbl(COTTAGE(I, 7))
                        FOOTER(0, 8) = CDbl(FOOTER(0, 8)) + CDbl(COTTAGE(I, 8))
                        Exit For
                    End If
                Next
            ElseIf UCase(rs.FIELDS("SALETYPE").VALUE & "") = "TOWNES" Then
                For I = 0 To UBound(TOWNES)
                    If TOWNES(I, 1) = UCase(Trim(rs.fields("Season").value & "")) And TOWNES(I, 2) = UCase(Trim(rs.FIELDS("FREQUENCY").VALUE & "")) And TOWNES(I, 0) & "BD" = rs.FIELDS("BD").VALUE & "" Then
                        TOWNES(I, 7) = rs.FIELDS("AVAILABLE").VALUE
                        TOWNES(I, 8) = rs.FIELDS("VOLUME").VALUE
                        FOOTER(1, 7) = CDbl(FOOTER(1, 7)) + CDbl(TOWNES(I, 7))
                        FOOTER(1, 8) = CDbl(FOOTER(1, 8)) + CDbl(TOWNES(I, 8))
                        Exit For
                    End If
                Next
            ElseIf UCase(rs.FIELDS("SALETYPE").VALUE & "") = "ESTATES" Then
                For I = 0 To UBound(ESTATES)
                    If ESTATES(I, 1) = UCase(Trim(rs.fields("Season").value & "")) And ESTATES(I, 2) = UCase(Trim(rs.FIELDS("FREQUENCY").VALUE & "")) And ESTATES(I, 0) & "BD" = rs.FIELDS("BD").VALUE & "" Then
                        ESTATES(I, 7) = rs.FIELDS("AVAILABLE").VALUE
                        ESTATES(I, 8) = rs.FIELDS("VOLUME").VALUE
                        FOOTER(2, 7) = CDbl(FOOTER(2, 7)) + CDbl(ESTATES(I, 7))
                        FOOTER(2, 8) = CDbl(FOOTER(2, 8)) + CDbl(ESTATES(I, 8))
                        Exit For
                    End If
                Next
            ElseIf UCase(rs.FIELDS("SALETYPE").VALUE & "") = "COMBO" Then
                For I = 0 To UBound(COMBOS)
                    If COMBOS(I, 1) = UCase(Trim(rs.fields("Season").value & "")) And COMBOS(I, 2) = UCase(Trim(rs.FIELDS("FREQUENCY").VALUE & "")) And COMBOS(I, 0) & "BD" = rs.FIELDS("BD").VALUE & "" Then
                        COMBOS(I, 7) = rs.FIELDS("AVAILABLE").VALUE
                        COMBOS(I, 8) = rs.FIELDS("VOLUME").VALUE
                        FOOTER(3, 7) = CDbl(FOOTER(3, 7)) + CDbl(COMBOS(I, 7))
                        FOOTER(3, 8) = CDbl(FOOTER(3, 8)) + CDbl(COMBOS(I, 8))
                        Exit For
                    End If
                Next
            Else
            End If
            rs.MOVENEXT()
        Loop
        rs.CLOSE()

        '****** SET TOTALS ******
        For I = 0 To UBound(COTTAGE)
            COTTAGE(I, 9) = CDbl(COTTAGE(I, 4)) + CDbl(COTTAGE(I, 6)) + CDbl(COTTAGE(I, 8))
            COTTAGE(I, 12) = CDbl(COTTAGE(I, 11)) * CDbl(COTTAGE(I, 3))
            FOOTER(0, 9) = CDbl(FOOTER(0, 9)) + CDbl(COTTAGE(I, 9))
            FOOTER(0, 11) = "&nbsp;"
            FOOTER(0, 12) = CDbl(FOOTER(0, 12)) + CDbl(COTTAGE(I, 12))
        Next
        For I = 0 To UBound(TOWNES)
            TOWNES(I, 9) = CDbl(TOWNES(I, 4)) + CDbl(TOWNES(I, 6)) + CDbl(TOWNES(I, 8))
            TOWNES(I, 12) = CDbl(TOWNES(I, 11)) * CDbl(TOWNES(I, 3))
            FOOTER(1, 9) = CDbl(FOOTER(1, 9)) + CDbl(TOWNES(I, 9))
            FOOTER(1, 12) = CDbl(FOOTER(1, 12)) + CDbl(TOWNES(I, 12))
            FOOTER(1, 11) = "&nbsp;"
        Next
        For I = 0 To UBound(ESTATES)
            ESTATES(I, 9) = CDbl(ESTATES(I, 4)) + CDbl(ESTATES(I, 6)) + CDbl(ESTATES(I, 8))
            ESTATES(I, 12) = CDbl(ESTATES(I, 11)) * CDbl(ESTATES(I, 3))
            FOOTER(2, 9) = CDbl(FOOTER(2, 9)) + CDbl(ESTATES(I, 9))
            FOOTER(2, 12) = CDbl(FOOTER(2, 12)) + CDbl(ESTATES(I, 12))
            FOOTER(2, 11) = "&nbsp;"
        Next
        For I = 0 To UBound(COMBOS)
            COMBOS(I, 9) = CDbl(COMBOS(I, 4)) + CDbl(COMBOS(I, 6)) + CDbl(COMBOS(I, 8))
            COMBOS(I, 12) = CDbl(COMBOS(I, 11)) * CDbl(COMBOS(I, 3))
            FOOTER(3, 9) = CDbl(FOOTER(3, 9)) + CDbl(COMBOS(I, 9))
            FOOTER(3, 12) = CDbl(FOOTER(3, 12)) + CDbl(COMBOS(I, 12))
            FOOTER(3, 11) = "&nbsp;"
        Next
        Lit1.Visible = True
        Lit1.Text = ""
        '****** DISPLAY ******
        Lit1.Text &= ("<TABLE BORDER=1>")

        DISPLAY_TITLE()
        DISPLAY_HEADER("COTTAGE")
        For I = 0 To UBound(COTTAGE)
            Lit1.Text &= ("<TR>")
            For X = 0 To UBound(COTTAGE, 2)
                If X = 4 Or X = 6 Or X > 7 Then
                    Lit1.Text &= ("<TD ALIGN='RIGHT'>" & FormatCurrency(COTTAGE(I, X)) & "</TD>")
                ElseIf X > 2 Then
                    Lit1.Text &= ("<TD ALIGN='RIGHT'>" & COTTAGE(I, X) & "</TD>")
                Else
                    Lit1.Text &= ("<TD ALIGN='LEFT'>" & COTTAGE(I, X) & "</TD>")
                End If
            Next
            Lit1.Text &= ("</TR>")
        Next
        DISPLAY_FOOTER(0)

        DISPLAY_HEADER("TOWNES")
        For I = 0 To UBound(TOWNES)
            Lit1.Text &= ("<TR>")
            For X = 0 To UBound(TOWNES, 2)
                If X = 4 Or X = 6 Or X > 7 Then
                    Lit1.Text &= ("<TD ALIGN='RIGHT'>" & FormatCurrency(TOWNES(I, X)) & "</TD>")
                ElseIf X > 2 Then
                    Lit1.Text &= ("<TD ALIGN='RIGHT'>" & TOWNES(I, X) & "</TD>")
                Else
                    Lit1.Text &= ("<TD ALIGN='LEFT'>" & TOWNES(I, X) & "</TD>")
                End If
            Next
            Lit1.Text &= ("</TR>")
        Next
        DISPLAY_FOOTER(1)

        DISPLAY_HEADER("ESTATES")
        For I = 0 To UBound(ESTATES)
            Lit1.Text &= ("<TR>")
            For X = 0 To UBound(ESTATES, 2)
                If X = 4 Or X = 6 Or X > 7 Then
                    Lit1.Text &= ("<TD ALIGN='RIGHT'>" & FormatCurrency(ESTATES(I, X)) & "</TD>")
                ElseIf X > 2 Then
                    Lit1.Text &= ("<TD ALIGN='RIGHT'>" & ESTATES(I, X) & "</TD>")
                Else
                    Lit1.Text &= ("<TD ALIGN='LEFT'>" & ESTATES(I, X) & "</TD>")
                End If
            Next
            Lit1.Text &= ("</TR>")
        Next
        DISPLAY_FOOTER(2)

        DISPLAY_HEADER("COMBOS")
        For I = 0 To UBound(COMBOS)
            Lit1.Text &= ("<TR>")
            For X = 0 To UBound(COMBOS, 2)
                If X = 4 Or X = 6 Or X > 7 Then
                    Lit1.Text &= ("<TD ALIGN='RIGHT'>" & FormatCurrency(COMBOS(I, X)) & "</TD>")
                ElseIf X > 2 Then
                    Lit1.Text &= ("<TD ALIGN='RIGHT'>" & COMBOS(I, X) & "</TD>")
                Else
                    Lit1.Text &= ("<TD ALIGN='LEFT'>" & COMBOS(I, X) & "</TD>")
                End If
            Next
            Lit1.Text &= ("</TR>")
        Next
        DISPLAY_FOOTER(3)

        Lit1.Text &= ("<TR>")
        For I = 0 To UBound(FOOTER, 2)
            If I = 0 Then
                Lit1.Text &= ("<TH>TOTAL</TH>")
            ElseIf I < 3 Or I = 10 Or I = 11 Then
                Lit1.Text &= ("<TH>&nbsp;</TH>")
            ElseIf I < 10 And (I = 3 Or I = 5 Or I = 7) Then
                Lit1.Text &= ("<TH ALIGN='RIGHT'>" & CDbl(FOOTER(0, I)) + CDbl(FOOTER(1, I)) + CDbl(FOOTER(2, I)) & "</TH>")
            Else
                Lit1.Text &= ("<TH ALIGN='RIGHT'>" & FormatCurrency(CDbl(FOOTER(0, I)) + CDbl(FOOTER(1, I)) + CDbl(FOOTER(2, I))) & "</TH>")
            End If
        Next
        Lit1.Text &= ("</TR>")

        Lit1.Text &= ("</TABLE>")

        'FOR I = 0 TO UBOUND(COTTAGE)
        '	FOR X = 0 TO UBOUND(COTTAGE,2)
        '		lit1.text &= "COTTAGE(" & I & ", " & X & ")=" & COTTAGE(I,X) & "<BR />"
        '	NEXT
        'NEXT

        'FOR I = 0 TO UBOUND(TOWNES)
        '	FOR X = 0 TO UBOUND(TOWNES,2)
        '		lit1.text &= "TOWNES(" & I & ", " & X & ")=" & TOWNES(I,X) & "<BR />"
        '	NEXT
        'NEXT	

        'FOR I = 0 TO UBOUND(ESTATES)
        '	FOR X = 0 TO UBOUND(ESTATES,2)
        '		lit1.text &= "ESTATES(" & I & ", " & X & ")=" & ESTATES(I,X) & "<BR />"
        '	NEXT
        'NEXT	







        cn.CLOSE()
        rs = Nothing
        cn = Nothing
    End Sub

    Sub SET_DEFAULTS()
        For I = 0 To UBOUND(COTTAGE)
            For X = 0 To UBOUND(COTTAGE, 2)
                COTTAGE(I, X) = 0
            Next
        Next
        For I = 0 To UBOUND(TOWNES)
            For X = 0 To UBOUND(TOWNES, 2)
                TOWNES(I, X) = 0
            Next
        Next
        For I = 0 To UBound(ESTATES)
            For X = 0 To UBound(ESTATES, 2)
                ESTATES(I, X) = 0
            Next
        Next
        For I = 0 To UBound(COMBOS)
            For X = 0 To UBound(COMBOS, 2)
                COMBOS(I, X) = 0
            Next
        Next
        For I = 0 To UBOUND(FOOTER)
            For X = 0 To UBOUND(FOOTER, 2)
                If X < 3 Or X = 10 Or X = 11 Then
                    FOOTER(I, X) = "&nbsp;"
                Else
                    FOOTER(I, X) = 0
                End If
            Next
        Next
    End Sub

    Sub DISPLAY_HEADER(ByVal SVALUE As String)
        Lit1.Text &= ("<TR><TH COLSPAN='13' ALIGN='LEFT'>" & SVALUE & " INVENTORY</TH></TR>")
        Lit1.Text &= ("<TR>")
        Lit1.Text &= ("<TH>SIZE</TH>")
        Lit1.Text &= ("<TH>SEASON</TH>")
        Lit1.Text &= ("<TH>FREQUENCY</TH>")
        Lit1.Text &= ("<TH>NUM AVAILABLE</TH>")
        Lit1.Text &= ("<TH>UNSOLD VOLUME</TH>")
        Lit1.Text &= ("<TH>NUM SOLD</TD>")
        Lit1.Text &= ("<TH>SOLD VOLUME</TH>")
        Lit1.Text &= ("<TH>PENDERS</TH>")
        Lit1.Text &= ("<TH>PENDER VOLUME</TH>")
        Lit1.Text &= ("<TH>TOTAL VOLUME</TH>")
        Lit1.Text &= ("<TH>CURRENT SALES PRICE</TH>")
        Lit1.Text &= ("<TH>MF / Unit</TH>")
        Lit1.Text &= ("<TH>MFUNSOLD</TH>")
        Lit1.Text &= ("</TR>")
    End Sub

    Sub DISPLAY_TITLE()
        Lit1.Text &= ("<TR><TH COLSPAN='13' ALIGN='CENTER'>RESORT FINANCE</TH></TR>")
        Lit1.Text &= ("<TR><TH COLSPAN='13' ALIGN='CENTER'>INVENTORY REPORT</TH></TR>")
        Lit1.Text &= ("<TR><TH COLSPAN='13' ALIGN='CENTER'>AS OF " & Date.Today & "</TH></TR>")
    End Sub

    Sub DISPLAY_FOOTER(ByVal IVALUE As Integer)
        Lit1.Text &= ("<TR>")
        For I = 0 To UBound(FOOTER, 2)
            If I = 4 Or I = 6 Or (I > 7 And I < 10) Or I = 12 Then
                Lit1.Text &= ("<TH ALIGN='RIGHT'>" & FormatCurrency(FOOTER(IVALUE, I)) & "</TH>")
            Else
                Lit1.Text &= ("<TH ALIGN='RIGHT'>" & FOOTER(IVALUE, I) & "</TH>")
            End If
        Next
        Lit1.Text &= ("</TR>")
        Lit1.Text &= ("<TR><TD COLSPAN='13' ALIGN='CENTER'>&nbsp;</TD></TR>")
        Lit1.Text &= ("<TR><TD COLSPAN='13' ALIGN='CENTER'>&nbsp;</TD></TR>")
    End Sub

    Protected Sub btnExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcel.Click
        Response.Clear()
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment; filename=RESORT FINANCE INVENTORY.xls")
        Report()
        Response.Write(Lit1.Text)
        Response.End()
    End Sub
End Class
