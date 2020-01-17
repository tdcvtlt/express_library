
Partial Class Reports_CustomerService_UP4ORCEInvoice
    Inherits System.Web.UI.Page
    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        Dim cn As Object
        Dim rs As Object
        Dim rs2 As Object
        cn = Server.CreateObject("ADODB.Connection")
        rs = Server.CreateObject("ADODB.Recordset")
        rs2 = Server.CreateObject("ADODB.Recordset")
        Dim edate As String = Me.dteEDate.Selected_Date
        Dim sdate As String = Me.dteSDate.Selected_Date
        Dim AccomCost As Double = 0
        Dim sANs As String = ""
        Dim counter As Integer = 0
        Dim premCost As Double = 0
        Dim tourFee As Double = 0
        Dim tourCost As Double = 0
        Dim grandTotal As Double = 0
        Dim prevID As Integer = 0
        
        sAns = "<H2>UP4ORCE Invoice " & sDate & " - " & eDate & "</H2>"
        sAns = sAns & "<table>"
        sANs = sANs & "<tr>"
        sANs = sANs & "<th><u>Tour Location</u></th>"
        sAns = sAns & "<th><u>TourID</u></th>"
        sAns = sAns & "<th><u>TourDate</u></th>"
        sANs = sANs & "<th><u>TourStatus</u></th>"
        sANs = sANs & "<th><u>Tour Time</u></th>"
        sAns = sAns & "<th><u>Prospect</u></th>"
        sAns = sAns & "<th><u>Gifts</u></th>"
        sAns = sAns & "<th><u>Gift Cost</u></th>"
        sAns = sAns & "<th><u>Tour Fee</u></th>"
        sAns = sAns & "<th><u>Tour Total</u></th>"
        sAns = sAns & "</tr>"


        Server.ScriptTimeout = 10000
        Dim cs As String = ""
        cn.open(Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
        cn.CommandTimeout = 0
        'rs.Open("Select t.tourid, t.tourdate, ts.ComboItem as TourStatus, p.FirstName, p.LastName, cs.UserFieldValue as CreditScore, c.ContractNumber from t_Tour t inner join t_Prospect p on t.prospectid = p.prospectid left outer join t_Contract c on t.tourid = c.tourid left outer join t_ComboItems ts on ts.ComboItemID = t.TourStatusID left outer join (Select UserFieldValue, RecordID from t_UserFieldsValue where UserFieldID = (Select UserFieldID from t_UserFields where FieldName = 'Equifax')) cs on t.tourid = cs.RecordID where t.CampaignID = (Select campaignid from t_Campaign where campaignname = 'UP4ORCE') and t.toursubtypeid not in (Select comboitemid from t_ComboItems where comboname = 'TourSubType' and comboitem = 'Exit') and t.tourdate between '" & Request("sDate") & "' and '" & Request("eDate") & "'", cn, 3, 3)
        rs.Open("Select tl.comboItem as TourLocation, t.tourid, t.tourdate, ts.ComboItem as TourStatus, (select COMBOITEM from t_comboitems where comboitemid = t.tourtime) TourTime, p.FirstName, p.LastName, c.ContractNumber from t_Tour t inner join t_Prospect p on t.prospectid = p.prospectid left outer join t_Contract c on t.tourid = c.tourid left outer join t_ComboItems ts on ts.ComboItemID = t.StatusID left outer join t_ComboItems tl on tl.comboItemID = t.TourLocationID where t.CampaignID = (Select campaignid from t_Campaign where name = 'UP4ORCE-DYD') and t.subtypeid not in (Select c.comboitemid from t_ComboItems c inner join t_Combos co on c.ComboID = co.ComboID where co.comboname = 'TourSubType' and c.comboitem = 'Exit') and t.tourdate between '" & sdate & "' and '" & edate & "'", cn, 3, 3)

        'Response.Write(String.Format("Select tl.comboItem as TourLocation, t.tourid, t.tourdate, ts.ComboItem as TourStatus, p.FirstName, p.LastName, c.ContractNumber from t_Tour t inner join t_Prospect p on t.prospectid = p.prospectid left outer join t_Contract c on t.tourid = c.tourid left outer join t_ComboItems ts on ts.ComboItemID = t.StatusID left outer join t_ComboItems tl on tl.comboItemID = t.TourLocationID where t.CampaignID = (Select campaignid from t_Campaign where name = 'UP4ORCE-DYD') and t.subtypeid not in (Select c.comboitemid from t_ComboItems c inner join t_Combos co on c.ComboID = co.ComboID where co.comboname = 'TourSubType' and c.comboitem = 'Exit') and t.tourdate between '{0}' and '{1}'", sdate, edate))
        If rs.EOF And rs.BOF Then
            sAns = sAns & "<tr><td colspan = '10'>No Tours In This Date Range</td></tr>"
        Else
            Do While Not rs.EOF
                If CInt(rs.Fields("TourID").Value) <> prevID Then
                    sANs = sANs & "<tr>"
                    sANs = sANs & "<td>" & rs.Fields("TourLocation").value & "</td>"
                    sANs = sANs & "<td>" & rs.Fields("TourID").value & "</td>"
                    sANs = sANs & "<td>" & rs.Fields("TourDate").value & "</td>"
                    sANs = sANs & "<td>" & rs.Fields("TourStatus").value & "</td>"
                    sANs = sANs & "<td>" & rs.Fields("TourTime").value & "</td>"
                    sANs = sANs & "<td>" & rs.Fields("FirstName").value & " " & rs.Fields("LastName").value & "</td>"
                    sANs = sANs & "<td>"
                    counter = 0
                    premCost = 0
                    rs2.Open("Select a.TotalCost, b.PremiumName from t_PremiumIssued a inner join t_Premium b on a.premiumid = b.premiumid where a.KeyField = 'TourID' and a.keyvalue = '" & rs.Fields("TourID").value & "' and a.statusid in (Select c.comboitemid from t_ComboItems c inner join t_Combos co on c.COmboID = co.ComboID where co.comboname = 'PremiumStatus' and (c.comboitem = 'Issued' or c.comboitem = 'Prepared'))", cn, 3, 3)
                    If rs2.EOF And rs2.BOF Then
                        premCost = 0
                    Else
                        counter = 0
                        Do While Not rs2.EOF
                            If counter = 0 Then
                                sANs = sANs & rs2.Fields("PremiumName").Value
                            Else
                                sANs = sANs & "<br>" & rs2.Fields("PremiumName").Value
                            End If
                            premCost = premCost + rs2.Fields("TotalCost").Value
                            counter = counter + 1
                            rs2.MoveNext()
                        Loop
                    End If
                    rs2.Close()
                    sANs = sANs & "</td>"
                    sANs = sANs & "<td>" & FormatCurrency(premCost, 2) & "</td>"

                    If (Not rs.Fields("TourStatus").Value Is DBNull.Value) Then
                        If rs.Fields("TourStatus").Value = "No Tour - Overage" Or rs.Fields("TourStatus").Value = "Showed" _
                            Or rs.Fields("TourStatus").Value = "OnTour" Or (rs.Fields("TourStatus").Value = "NQ - Toured" And Not rs.Fields("ContractNumber").Value Is DBNull.Value) Then
                            tourFee = 400
                        Else
                            tourFee = 0
                        End If
                    Else
                        tourFee = 0
                    End If

                    sANs = sANs & "<td align = right>" & FormatCurrency(tourFee) & "</td>"
                    tourCost = tourFee - premCost
                    sANs = sANs & "<td align = right>" & Replace(Replace(FormatCurrency(tourCost), "(", "-"), ")", "") & "</td>"
                    grandTotal = grandTotal + tourCost
                    sANs = sANs & "</tr>"
                    tourFee = 0
                    AccomCost = 0
                    premCost = 0
                    prevID = rs.Fields("TourID").Value
                End If
                rs.MoveNext()
            Loop
            sAns = sAns & "<tr>"
            sAns = sAns & "<td colspan = '8' align = right><B>Grand Total:</B></td>"
            sAns = sAns & "<td align = right><B>" & Replace(Replace(FormatCurrency(grandTotal), "(", "-"), ")", "") & "</B></td>"
            sAns = sAns & "</tr>"
        End If
        rs.Close()
        sAns = sAns & "</table>"
        cn.Close()
        litReport.Text = sANs

    End Sub

    Protected Sub Button3_Click(sender As Object, e As System.EventArgs) Handles Button3.Click
        Response.Clear()
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment; filename=UP4ORCEInvoice.xls")
        Response.Write(litReport.Text)
        Response.End()
    End Sub
End Class
