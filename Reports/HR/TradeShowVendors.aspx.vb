Imports System.Data
Imports System.Data.SqlClient

Partial Class Reports_HR_TradeShowVendors
    Inherits System.Web.UI.Page

    Protected Sub btnReport_Click(sender As Object, e As System.EventArgs) Handles btnReport.Click
        Dim html = "<table>"
        Dim total = 0D
        Dim acct = 4
        Dim edate As String = Me.dteEDate.Selected_Date
        Dim sdate As String = Me.dteSDate.Selected_Date        

        Dim sql = "select i.transdate as [Trans Date], ftc.comboitem as [Trans Code], case when Upper(i.Keyfield) = 'PACKAGEISSUEDID' then 'PkgID:' + cast(i.Keyvalue as varchar) when Upper(i.KeyField) = 'TOURID' then 'TourID:' + cast(i.keyvalue as varchar) when Upper(i.KeyField) = 'RESERVATIONID' then 'ResID:' + cast(i.Keyvalue as varchar) when UPPER(i.KeyField) = 'CONTRACTID' then 'KCP#:' + (Select xx.ContractNumber from t_Contract xx where xx.ContractID = i.KeyValue) when UPPER(i.KeyField) = 'MORTGAGEDP' then 'MortID:' + cast(i.KeyValue as varchar) when UPPER(i.KeyField) = 'CONVERSIONDP' then 'ConversionID:' + cast(i.KeyValue as varchar) when UPPER(i.KeyField) = 'PROSPECTID' then 'ProspectID:' + cast(i.KeyValue as varchar) else i.KeyField + ' Unknown' end  as Field, p.Firstname + ' ' + p.Lastname as Prospect, i.Amount, per.username as Personnel, sol.lastname + ', ' + sol.firstname as Solicitor, sl.Location as [Sales Location], (select top 1 Email from t_prospectEmail where prospectid = p.prospectId and isPrimary = 1 and isActive = 1 order by emailID desc) Email from t_Invoices i inner join t_Prospect p on p.prospectid = i.prospectid inner join t_Fintranscodes f on f.fintransid = i.fintransid inner join t_Comboitems ftc on ftc.comboitemid = f.transcodeid left outer join t_Personnel per on per.personnelid = i.UserID left outer join (select p.lastname, p.firstname, pt.*, ti.comboitem from t_Personneltrans pt inner join t_Personnel p on p.personnelid = pt.personnelid inner join t_Comboitems ti on ti.comboitemid = pt.titleid where ti.comboitem = 'Tradeshow Solicitor' and pt.keyfield='packageissuedid') sol on sol.keyvalue = i.keyvalue left outer join (select vsl.Location, t.packageissuedid from t_VendorSalesLocations vsl inner join t_VendorRep2Tour vrt on vrt.salelocid = vsl.saleslocationid inner join t_Tour t on t.tourid = vrt.tourid) sl on sl.packageissuedid = i.keyvalue where i.transdate between '" & sdate & "' and '" & edate & "' and f.MerchantAccountID = '" & acct & "' and i.ApplyToID =0 and i.fintransid in (485, 490, 519) " & _
            "union " & _
                "select ia.transdate as [Trans Date], ftc.comboitem as [Trans Code], case when Upper(i.Keyfield) = 'PACKAGEISSUEDID' then 'PkgID:' + cast(i.Keyvalue as varchar) when Upper(i.KeyField) = 'TOURID' then 'TourID:' + cast(i.keyvalue as varchar) when Upper(i.KeyField) = 'RESERVATIONID' then 'ResID:' + cast(i.Keyvalue as varchar) when UPPER(i.KeyField) = 'CONTRACTID' then 'KCP#:' + (Select xx.ContractNumber from t_Contract xx where xx.ContractID = i.KeyValue) when UPPER(i.KeyField) = 'MORTGAGEDP' then 'MortID:' + cast(i.KeyValue as varchar) when UPPER(i.KeyField) = 'CONVERSIONDP' then 'ConversionID:' + cast(i.KeyValue as varchar) when UPPER(i.KeyField) = 'PROSPECTID' then 'ProspectID:' + cast(i.KeyValue as varchar) else i.KeyField + ' Unknown' end  as Field, p.Firstname + ' ' + p.Lastname as Prospect, case when ia.posneg = 0 then ia.Amount else ia.Amount * -1 end as Amount, per.username as Personnel, sol.lastname + ', ' + sol.firstname as Solicitor, sl.Location as [Sales Location], (select top 1 Email from t_prospectEmail where prospectid = p.prospectId and isPrimary = 1 and isActive = 1 order by emailID desc) Email from t_Invoices i inner join t_Invoices ia on ia.applytoid = i.invoiceid inner join t_Prospect p on p.prospectid = i.prospectid inner join t_Fintranscodes f on f.fintransid = i.fintransid inner join t_Comboitems ftc on ftc.comboitemid = f.transcodeid left outer join t_Personnel per on per.personnelid = ia.UserID  left outer join (select  p.lastname, p.firstname, pt.*, ti.comboitem from t_Personneltrans pt inner join t_Personnel p on p.personnelid = pt.personnelid inner join t_Comboitems ti on ti.comboitemid = pt.titleid where ti.comboitem = 'Tradeshow Solicitor' and pt.keyfield='packageissuedid') sol on sol.keyvalue = i.keyvalue left outer join (select vsl.Location, t.packageissuedid from t_VendorSalesLocations vsl inner join t_VendorRep2Tour vrt on vrt.salelocid = vsl.saleslocationid inner join t_Tour t on t.tourid = vrt.tourid) sl on sl.packageissuedid = i.keyvalue   where ia.transdate between '" & sdate & "' and '" & edate & "' and f.MerchantAccountID = '" & acct & "' and i.ApplyToID =0 and i.fintransid in (485, 490, 519) order by [Trans Code], [Trans Date], Prospect"


        Using cnn = New SqlConnection(Resources.Resource.cns)
            Using cmd = New SqlCommand(sql, cnn)

                Try
                    cnn.Open()
                    Dim reader = cmd.ExecuteReader()

                    html += "<tr>"
                    If reader.HasRows = False Then
                        html += "<td>No rows.</td>"
                    Else
                        For i = 0 To reader.FieldCount - 1
                            html += String.Format("<td>{0}</td>", reader.GetName(i))
                        Next
                    End If
                    html += "</tr>"

                    While reader.Read()
                        html += "<tr>"

                        For i = 0 To reader.FieldCount - 1

                            If reader.Item(i).Equals(DBNull.Value) Then
                                html += String.Format("<td>{0}</td>", "N/A")
                            Else

                                If reader.GetName(i).Equals("Trans Date") Then

                                    Try
                                        html += String.Format("<td>{0}</td>", DateTime.Parse(reader.Item(i).ToString()).ToShortDateString())
                                    Catch ex As Exception
                                        html += String.Format("<td>{0}</td>", "")
                                    End Try

                                ElseIf reader.GetName(i).Equals("Amount") Then

                                    Try
                                        html += String.Format("<td>{0:C}</td>", Decimal.Parse(reader.Item(i).ToString()))
                                        total += Decimal.Parse(reader.Item(i).ToString())
                                    Catch ex As Exception
                                        html += String.Format("<td>{0}</td>", "0.00")
                                    End Try

                                Else
                                    html += String.Format("<td>{0}</td>", reader.Item(i))
                                End If
                            End If
                        Next

                        html += "</tr>"
                    End While

                    If reader.HasRows Then
                        html += String.Format("<tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td><td align='right'>Total:</td><td>" & FormatCurrency(total) & "</td><td>&nbsp;</td><td>&nbsp;</td></tr>")
                    End If

                Catch ex As Exception
                    Response.Write(ex.Message)
                Finally
                    cnn.Close()
                End Try

            End Using
        End Using

        html += String.Format("</table>")
        litReport.Text = html

    End Sub

    Protected Sub btnExcel_Click(sender As Object, e As System.EventArgs) Handles btnExcel.Click
        Response.Clear()
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment; filename=Trade Show Vendors Report.xls")
        Response.Write(litReport.Text)
        Response.End()
    End Sub
End Class
