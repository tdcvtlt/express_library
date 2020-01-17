
Partial Class Reports_Accounting_PenderSales
    Inherits System.Web.UI.Page
    Protected Sub Report()
        Dim rs As Object
        Dim cn As Object
        ' convert(char(12),transactiondate,101)

        cn = Server.CreateObject("ADODB.Connection")
        rs = Server.CreateObject("ADODB.Recordset")

        Lit1.Text = ""
        cn.open(Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
        If rbList.SelectedValue = "Line" Then
            rs.open("select c.contractnumber as KCP,p.firstname + ' ' + p.lastname as Owner, " & _
       "      s.comboitem as Status, " & _
       "	      convert(char(12),(select max(transdate) from v_Payments where keyfield='mortgagedp' and keyvalue = m.mortgageid and amount + adjustment < 0),101) as LastPaymentDate, " & _
       "	sum(i.amount + i.adjustment) as Amount " & _
       "from t_contract c " & _
       "      inner join t_prospect p on p.prospectid = c.prospectid " & _
       "      inner join t_comboitems s on s.comboitemid = c.statusid " & _
       "      inner join t_mortgage m on m.contractid = c.contractid " & _
       "      inner join (select * from v_payments where keyfield = 'mortgagedp' and invoice like 'down payment%') i on  i.keyvalue = m.mortgageid " & _
       "where (s.comboitem = 'Pender' or s.comboitem = 'Pender-Inv') and i.transdate < dateadd(d,1,'" & dfDate.Selected_Date & "') " & _
       "   and contractnumber not like 't%' and contractnumber not like 'u%' and i.method not like 'Equity%' " & _
       "group by c.contractnumber,p.lastname, p.firstname, s.comboitem, m.mortgageid " & _
       "order by c.contractnumber ", cn, 3, 3)


            lit1.text &= ("<table border=1>")
            lit1.text &= ("<tr>")
            lit1.text &= ("<th colspan='" & rs.fields.count & "'><u>Pender Sales</u></th>")
            lit1.text &= ("</tr>")
            lit1.text &= ("<tr>")
            For i = 0 To rs.fields.count - 1
                lit1.text &= ("<th>" & rs.fields(i).name & "</th>")
            Next
            lit1.text &= ("</tr>")
            Do While Not rs.eof
                lit1.text &= ("<tr>")
                For i = 0 To rs.fields.count - 1
                    If rs.fields(i).name = "Amount" Then
                        lit1.text &= ("<td>" & FormatCurrency(rs.fields(i).value) & "</td>")
                    Else
                        lit1.text &= ("<td>" & rs.fields(i).value & "</td>")
                    End If
                Next
                lit1.text &= ("</tr>")
                rs.movenext()
            Loop
            rs.close()
            lit1.text &= ("</table>")
        Else
            rs.open("select c.contractnumber as KCP,p.firstname + ' ' + p.lastname as Owner, " & _
       "      s.comboitem as Status, " & _
       "	      convert(char(12),(select max(transdate) from v_Payments where keyfield='mortgagedp' and keyvalue = m.mortgageid and amount + adjustment < 0),101) as LastPaymentDate, " & _
       "	sum(i.amount + i.adjustment) as Amount " & _
       "from t_contract c " & _
       "      inner join t_prospect p on p.prospectid = c.prospectid " & _
       "      inner join t_comboitems s on s.comboitemid = c.statusid " & _
       "      inner join t_mortgage m on m.contractid = c.contractid " & _
       "      inner join (select * from v_payments where keyfield = 'mortgagedp' and invoice like 'down payment%') i on  i.keyvalue = m.mortgageid " & _
       "where (s.comboitem = 'Pender' or s.comboitem = 'Pender-Inv') and i.transdate < dateadd(d,1,'" & dfDate.Selected_Date & "') " & _
       "   and (contractnumber like 't%' or contractnumber like 'u%') and i.method not like 'Equity%' " & _
       "group by c.contractnumber,p.lastname, p.firstname, s.comboitem, m.mortgageid " & _
       "order by c.contractnumber ", cn, 3, 3)

            lit1.text &= ("<table border=1>")
            lit1.text &= ("<tr>")
            lit1.text &= ("<th colspan='" & rs.fields.count & "'><u>Exit Pender</u></th>")
            lit1.text &= ("</tr>")
            lit1.text &= ("<tr>")
            For i = 0 To rs.fields.count - 1
                lit1.text &= ("<th>" & rs.fields(i).name & "</th>")
            Next
            lit1.text &= ("</tr>")
            Do While Not rs.eof
                lit1.text &= ("<tr>")
                For i = 0 To rs.fields.count - 1
                    If rs.fields(i).name = "Amount" Then
                        lit1.text &= ("<td>" & FormatCurrency(rs.fields(i).value) & "</td>")
                    Else
                        lit1.text &= ("<td>" & rs.fields(i).value & "</td>")
                    End If
                Next
                lit1.text &= ("</tr>")
                rs.movenext()
            Loop
            rs.close()
            lit1.text &= ("</table>")
        End If
        cn.close()
    End Sub
    Private Function PMT(ByVal Rate As Double, ByVal PV As Double, ByVal Terms As Double, ByVal mType As Double) As Double
        Dim ans As Double
        If Rate & "" = "" Or Rate = "0" Or PV & "" = "" Or PV = "0" Or Terms & "" = "" Or Terms = "0" Then
            ans = 0
            Return ans
            Exit Function
        End If
        ans = CDbl(Math.Round((Rate * (mType + PV * (1 + Rate) ^ Terms)) / ((1 - Rate * mType) * (1 - (1 + Rate) ^ Terms)), 2)) * -1
        If Len(CStr(ans)) - InStr(CStr(ans), ".") = 1 Then
            ans = CStr(ans) & "0"
        End If
        Return Math.Round(ans, 2)
    End Function

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        Report()
    End Sub

    Protected Sub btnExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcel.Click
        Response.Clear()
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment; filename=Pender Sales.xls")
        Response.Write(Lit1.Text)
        Response.End()
    End Sub
End Class
