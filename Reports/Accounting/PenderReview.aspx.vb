
Partial Class Reports_Accounting_PenderReview
    Inherits System.Web.UI.Page
    Protected Sub Report()
        Dim rs As Object
        Dim cn As Object
        Dim bWritten As Boolean
        Dim dAmount As String
        Dim varVal As String

        cn = Server.CreateObject("ADODB.Connection")
        rs = Server.CreateObject("ADODB.Recordset")

        Lit1.Text = ""
        cn.open(Resources.resource.database, Resources.resource.username, Resources.resource.password)
        If rbList.SelectedValue = "Line" Then
            rs.open("select c.contractnumber as KCP, p.firstname + ' ' + p.lastname as Owner, " & _
                     "    cs.comboitem as Status, m.TotalFinanced, m.Terms, m.APR,  " & _
                     "    m.PaymentFee  as 'Mortgage Monthly Payment', " & _
                     "    ( " & _
                  "        select top 1 amount  " & _
                  "        from v_SchedPayments  " & _
                  "        where keyfield = 'MortgageDP' " & _
               "            and keyvalue = m.mortgageid " & _
                     "    ) as [Scheduled Payment Amount], " & _
                     "    c.contractdate as [Sale Date], " & _
                     "    dateadd(d,45,c.contractdate) as [45 Day Allowance], " & _
                     "    (select top 1 scheddate  " & _
                  "        from v_SchedPayments  " & _
                  "        where keyfield = 'MortgageDP'  " & _
               "            and keyvalue = m.mortgageid  " & _
                  "        order by scheddate desc " & _
                     "    ) as [Scheduled Final Payment], " & _
                     "    m.DPTotal as Amount, m.SalesVolume, bc.COmboItem As BillingCode, camp.Name as Campaign, pers.FirstName + ' ' + pers.LastName as SalesRep " & _
                        "from t_Contract c " & _
                     "    inner join t_Mortgage m on m.contractid = c.contractid " & _
                     "    inner join t_Comboitems cs on cs.comboitemid = c.statusid " & _
                     "    left outer join t_Comboitems bc on bc.comboitemid = c.billingcodeid " & _
                     "    inner join t_Prospect p on p.prospectid = c.prospectid " & _
                     "    left outer join t_Tour t on c.TourID = t.TourID " & _
                     "    left outer join t_ComboItems ts on t.SourceID = ts.ComboItemID " & _
                     "    left outer join t_Campaign camp on c.CampaignID = camp.CampaignID " & _
                     "    left outer join t_PersonnelTrans pt on c.ContractID = pt.KeyValue " & _
                     "    left outer join t_Personnel pers on pt.PersonnelID = pers.PersonnelID " & _
                     "    left outer join t_ComboItems title on pt.TitleID = title.ComboItemID " & _
                       "where (cs.comboitem = 'Pender' or cs.comboitem = 'Pender-Inv') and c.contractnumber not like 't%' and c.contractnumber not like 'u%' and (title.ComboItem is null or title.ComboItem = 'Sales Executive' and (pt.KeyField is null or pt.KeyField = 'ContractID')) " & _
                        "group by c.contractnumber,p.lastname, p.firstname, cs.comboitem,m.mortgageid, c.contractdate, m.apr, m.totalfinanced, m.terms,m.paymentfee,m.SalesVolume,bc.ComboItem, camp.Name, pers.FirstName, pers.LastName, m.dptotal " & _
                        "order by c.contractnumber", cn, 3, 3)
            '"and dpflag =1 and a.transactiondate < dateadd(d,1,'" & request("sdate") & "') and paid=1 and pm.comboitem <> 'Equity'

            Lit1.Text &= ("<table border='1px' style='border-collapse:collapse;'>")
            Lit1.Text &= ("<tr>")
            Lit1.Text &= ("<th colspan='" & rs.fields.count & "'><u>Pender Review</u></th>")
            Lit1.Text &= ("</tr>")
            Lit1.Text &= ("<tr>")
            For i = 0 To rs.fields.count - 1
                If rs.fields(i).name <> "APR" And rs.fields(i).name <> "TotalFinanced" And rs.fields(i).name <> "Terms" Then
                    Lit1.Text &= ("<th>" & rs.fields(i).name & "</th>")
                End If
            Next
            Lit1.Text &= ("</tr>")
            Do While Not rs.eof
                Lit1.Text &= ("<tr>")
                bWritten = False
                For i = 0 To rs.fields.count - 1
                    If rs.fields(i).name <> "APR" And rs.fields(i).name <> "TotalFinanced" And rs.fields(i).name <> "Terms" And rs.fields(i).name <> "Mortgage Monthly Payment" Then
                        If rs.fields(i).name = "Amount" Or rs.fields(i).name = "Scheduled Payment Amount" Or rs.fields(i).name = "SalesVolume" Then
                            Lit1.Text &= ("<td align='right'>")
                            dAmount = rs.fields(i).value & ""
                            If dAmount = "" Then dAmount = 0
                            Lit1.Text &= (FormatCurrency(dAmount))
                            Lit1.Text &= ("</td>")
                        Else
                            Lit1.Text &= ("<td>")
                            varVal = rs.fields(i).value & ""
                            If varVal = "" Then varVal = "&nbsp;"
                            Lit1.Text &= (varVal)
                            Lit1.Text &= ("</td>")
                        End If
                    ElseIf Not bWritten Then
                        Lit1.Text &= ("<td align='right'>")
                        dAmount = CDbl(PMT(rs.fields("APR").value / 1200, rs.fields("TotalFinanced").value, rs.fields("Terms").value, 0))
                        If dAmount > 0 Then
                            dAmount = dAmount + rs.fields("Mortgage Monthly Payment").value
                        End If
                        Lit1.Text &= (FormatCurrency(dAmount))
                        Lit1.Text &= ("</td>")
                        bWritten = True
                    End If

                Next
                Lit1.Text &= ("</tr>")
                rs.movenext()
            Loop
            rs.close()
            Lit1.Text &= ("</table>")
        Else
            rs.open("select c.contractnumber as KCP, p.firstname + ' ' + p.lastname as Owner, " & _
         "    cs.comboitem as Status, m.TotalFinanced, m.Terms, m.APR,  " & _
         "    m.PaymentFee  as 'Mortgage Monthly Payment', " & _
         "    ( " & _
      "        select top 1 amount  " & _
      "        from v_SchedPayments  " & _
      "        where keyfield = 'MortgageDP' " & _
   "            and keyvalue = m.mortgageid " & _
         "    ) as [Scheduled Payment Amount], " & _
         "    c.contractdate as [Sale Date], " & _
         "    dateadd(d,45,c.contractdate) as [45 Day Allowance], " & _
         "    (select top 1 scheddate  " & _
      "        from v_SchedPayments  " & _
      "        where keyfield = 'MortgageDP'  " & _
   "            and keyvalue = m.mortgageid  " & _
      "        order by scheddate desc " & _
         "    ) as [Scheduled Final Payment], " & _
         "    m.DPTotal as Amount, m.SalesVolume, bc.COmboItem As BillingCode, camp.Name as Campaign, pers.Firstname + ' ' + pers.LastName as SalesRep " & _
            "from t_Contract c " & _
         "    inner join t_Mortgage m on m.contractid = c.contractid " & _
         "    inner join t_Comboitems cs on cs.comboitemid = c.statusid " & _
         "    left outer join t_Comboitems bc on bc.comboitemid = c.billingcodeid " & _
         "    inner join t_Prospect p on p.prospectid = c.prospectid " & _
         "    left outer join t_Tour t on c.TourID = t.TourID " & _
         "    left outer join t_ComboItems ts on t.SourceID = ts.ComboItemID " & _
         "    left outer join t_Campaign camp on c.CampaignID = camp.CampaignID " & _
         "    left outer join t_PersonnelTrans pt on c.ContractID = pt.KeyValue " & _
         "    left outer join t_Personnel pers on pt.PersonnelID = pers.PersonnelID " & _
         "    left outer join t_ComboItems title on pt.TitleID = title.ComboItemID " & _
            "where (cs.comboitem = 'Pender' or cs.comboitem = 'Pender-Inv')  and (contractnumber like 't%' or contractnumber like 'u%') and ((title.ComboItem is null or title.ComboItem = 'Exit Sales Executive') and (pt.KeyField is null or pt.KeyField = 'ContractID'))" & _
            "group by c.contractnumber,p.lastname, p.firstname, cs.comboitem,m.mortgageid, c.contractdate, m.apr, m.totalfinanced, m.terms,m.paymentfee,m.SalesVolume,bc.ComboItem, camp.Name, pers.FirstName, pers.LastName, m.dptotal " & _
            "order by c.contractnumber", cn, 3, 3)


            '" and dpflag =1 and a.transactiondate < dateadd(d,1,'" & request("sdate") & "') and paid=1 and pm.comboitem <> 'Equity'


            Lit1.Text &= ("<table border='1px' style='border-collapse:collapse;'>")
            Lit1.Text &= ("<tr>")
            Lit1.Text &= ("<th colspan='" & rs.fields.count & "'><u>Exit Pender</u></th>")
            Lit1.Text &= ("</tr>")
            Lit1.Text &= ("<tr>")
            For i = 0 To rs.fields.count - 1
                If rs.fields(i).name <> "APR" And rs.fields(i).name <> "TotalFinanced" And rs.fields(i).name <> "Terms" Then
                    Lit1.Text &= ("<th>" & rs.fields(i).name & "</th>")
                End If
            Next
            Lit1.Text &= ("</tr>")
            Do While Not rs.eof
                Lit1.Text &= ("<tr>")
                bWritten = False
                For i = 0 To rs.fields.count - 1
                    If rs.fields(i).name <> "APR" And rs.fields(i).name <> "TotalFinanced" And rs.fields(i).name <> "Terms" And rs.fields(i).name <> "Mortgage Monthly Payment" Then
                        If rs.fields(i).name = "Amount" Or rs.fields(i).name = "Scheduled Payment Amount" Or rs.fields(i).name = "SalesVolume" Then
                            Lit1.Text &= ("<td align='right'>")
                            dAmount = rs.fields(i).value & ""
                            If dAmount = "" Then dAmount = 0
                            Lit1.Text &= (FormatCurrency(dAmount))
                            Lit1.Text &= ("</td>")
                        Else
                            Lit1.Text &= ("<td>")
                            varVal = rs.fields(i).value & ""
                            If varVal = "" Then varVal = "&nbsp;"
                            Lit1.Text &= (varVal)
                            Lit1.Text &= ("</td>")
                        End If
                    ElseIf Not bWritten Then
                        Lit1.Text &= ("<td align='right'>")
                        dAmount = CDbl(PMT(rs.fields("APR").value / 1200, rs.fields("TotalFinanced").value, rs.fields("Terms").value, 0))
                        If dAmount > 0 Then
                            dAmount = dAmount + rs.fields("Mortgage Monthly Payment").value
                        End If
                        Lit1.Text &= (FormatCurrency(dAmount))
                        Lit1.Text &= ("</td>")
                        bWritten = True
                    End If

                Next
                Lit1.Text &= ("</tr>")
                rs.movenext()
            Loop
            rs.close()
            Lit1.Text &= ("</table>")
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
        Response.AddHeader("Content-Disposition", "attachment; filename=Pender Review.xls")
        Response.Write(Lit1.Text)
        Response.End()
    End Sub
End Class
