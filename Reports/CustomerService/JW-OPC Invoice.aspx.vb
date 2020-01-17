Imports System.Data
Imports System.Data.SqlClient

Partial Class Reports_CustomerService_JW_OPC_Invoice
    Inherits System.Web.UI.Page

  
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

    End Sub


    Protected Sub btnReport_Click(sender As Object, e As System.EventArgs) Handles btnReport.Click

        Dim sd As String = dteSDate.Selected_Date, ed As String = dteEDate.Selected_Date                       
        Dim sb = New StringBuilder()
        Dim sql = String.Format("Select RowNumber = row_number() over (order by t.tourid), t.tourid, t.tourdate, ts.ComboItem as TourStatus, p.FirstName, p.LastName, sl.Location as SalesLocation, pers.FirstName + ' ' + pers.LastName as Solicitor from t_Tour t inner join t_Prospect p on t.prospectid = p.prospectid left outer join t_VendorRep2Tour vt on t.tourid = vt.tourid left outer join t_Personnel pers on vt.UserID = pers.PersonnelID left outer join t_VendorSalesLocations sl on vt.SaleLocID = sl.SalesLocationID left outer join t_ComboItems ts on ts.ComboItemID = t.StatusID where t.CampaignID = (Select top 1 campaignid from t_Campaign where name LIKE 'jw-opc' order by CampaignID ASC) and t.subtypeid not in (Select c.comboitemid from t_ComboItems c inner join t_Combos co on c.ComboID = co.ComboID where co.comboname = 'TourSubType' and c.comboitem LIKE '%Exit%') and t.tourdate between '{0}' and '{1}'", sd, ed)
        Dim gt As Decimal

        Using cnn = New SqlConnection(Resources.Resource.cns)
            Using ada = New SqlDataAdapter(sql, cnn)

                Dim dt = New DataTable
                ada.Fill(dt)

                If dt.Rows.Count = 0 Then
                    litReport.Text = "No rows."
                    Return
                Else
                    sb.Append("<H2>JW-OPC Invoice " & sd & " - " & ed & "</H2><table border=1 style=border-collapse:collapse;><tr>")

                    For Each s As String In New String() {"Row No.", "TourID", "TourDate", "TourStatus", "Prospect", "Solicitor", "Sales Location", "Gifts", "Gift Cost", "Tour Fee", "Tour Total"}
                        If s.CompareTo("Gift Cost") = 0 Or s.CompareTo("Tour Fee") = 0 Or s.CompareTo("Tour Total") = 0 Then
                            sb.AppendFormat("<th align=right><u>{0}</u></th>", s)
                        Else
                            sb.AppendFormat("<th><u>{0}</u></th>", s)
                        End If
                    Next
                    sb.Append("</tr>")
                End If

                For Each row As DataRow In dt.Rows

                    sql = String.Format("Select a.TotalCost, b.PremiumName, a.CBCostEA, a.QtyIssued, a.CostEA from t_PremiumIssued a inner join t_Premium b on a.premiumid = b.premiumid where a.keyField = 'TourID' and a.KeyValue = '{0}' and a.qtyIssued > 0 and a.statusid in (Select c.comboitemid from t_ComboItems c inner join t_Combos co on c.COmboID = co.ComboID where co.comboname = 'PremiumStatus' and (c.comboitem = 'Issued' or c.comboitem = 'Prepared'))", row.Item("tourid").ToString())
                    Using cmd = New SqlCommand(sql, cnn)

                        Try
                            cnn.Open()
                            Dim tbTemp As New DataTable()

                            tbTemp.Load(cmd.ExecuteReader())
                            Dim ar() As String = New String() {}

                            For Each r As DataRow In tbTemp.Rows
                                Array.Resize(ar, ar.Length + 1)
                                ar(ar.Length - 1) = String.Format("({0}) @ {1:N2} - {2}", r.Item("QtyIssued").ToString(), Decimal.Parse(r.Item("CostEA").ToString()), r.Item("PremiumName").ToString())
                            Next

                            sb.AppendFormat("<tr><td>{0}</td>", row.Item("RowNumber").ToString())
                            sb.AppendFormat("<td>{0}</td>", row.Item("TourID").ToString())
                            sb.AppendFormat("<td>{0}</td>", DateTime.Parse(row.Item("TourDate").ToString()).ToShortDateString())
                            sb.AppendFormat("<td>{0}</td>", row.Item("TourStatus").ToString())
                            sb.AppendFormat("<td>{0} {1}</td>", row.Item("FirstName").ToString(), row.Item("LastName").ToString())
                            sb.AppendFormat("<td>{0}</td>", row.Item("Solicitor").ToString())
                            sb.AppendFormat("<td>{0}</td>", row.Item("SalesLocation").ToString())



                            sb.AppendFormat("<td>{0}</td>", String.Join("<br/>", ar))

                            Dim tc As Decimal = IIf(row.Item("TourStatus").ToString() = "Showed", "300.00", "0.00")

                            gt += tc - tbTemp.Rows.OfType(Of DataRow).Sum(Function(x) Decimal.Parse(x.Item("CostEA").ToString()))

                            sb.AppendFormat("<td align=right>{0:N2}</td>", tbTemp.Rows.OfType(Of DataRow).Sum(Function(x) Decimal.Parse(x.Item("CostEA").ToString()) * Int16.Parse(x.Item("QtyIssued"))))
                            sb.AppendFormat("<td align=right>{0:N2}</td>", tc)

                            sb.AppendFormat("<td align=right>{0:N2}</td></tr>", tc - tbTemp.Rows.OfType(Of DataRow).Sum(Function(x) Decimal.Parse(x.Item("CostEA").ToString()) * Int16.Parse(x.Item("QtyIssued"))))

                        Catch ex As Exception
                            Response.Write(String.Format("<br/>{0} TourID {1}", ex.Message, row.Item("tourid").ToString()))
                        Finally
                            cnn.Close()
                        End Try
                    End Using

                Next
                litReport.Text = sb.ToString() + String.Format("<tr><td colspan=10>&nbsp;</td><td align=right><b>Grand Total: {0:C2}</b></td></tr></table>", gt)
            End Using
        End Using
    End Sub

    Protected Sub btnExcel_Click(sender As Object, e As System.EventArgs) Handles btnExcel.Click
        Response.Clear()
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment; filename=4KInvoice.xls")
        Response.Write(litReport.Text)
        Response.End()
    End Sub


End Class
