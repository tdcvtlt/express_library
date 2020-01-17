
Partial Class Reports_CustomerService_LocationInvoice
    Inherits System.Web.UI.Page

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click
        Dim cn As Object
        Dim rs As Object
        cn = Server.CreateObject("ADODB.Connection")
        rs = Server.CreateObject("ADODB.Recordset")
        Dim edate As String = Me.dteEDate.Selected_Date
        Dim sdate As String = Me.dteSDate.Selected_Date
        Dim sANs As String = ""
        Dim tourFee As Double = 0
        Dim tourCost As Double = 0
        Dim grandTotal As Double = 0

        sANs = "<H2>Location Invoice " & sdate & " - " & edate & "</H2>"
        sANs = sANs & "<table>"
        sANs = sANs & "<tr>"
        sANs = sANs & "<th><u>TourID</u></th>"
        sANs = sANs & "<th><u>TourDate</u></th>"
        sANs = sANs & "<th><u>TourStatus</u></th>"
        sANs = sANs & "<th><u>Prospect</u></th>"
        sANs = sANs & "<th><u>Tour Total</u></th>"
        sANs = sANs & "</tr>"

        Server.ScriptTimeout = 10000
        cn.open(Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
        cn.CommandTimeout = 0
        rs.Open("Select t.tourid, t.tourdate, ts.ComboItem as TourStatus, p.FirstName, p.LastName from t_Tour t inner join t_Prospect p on t.prospectid = p.prospectid inner join t_VendorRep2Tour vt on t.tourid = vt.tourid inner join t_VendorSalesLocations sl on vt.SaleLocID = sl.SalesLocationID left outer join t_ComboItems ts on ts.ComboItemID = t.StatusID where (sl.Location = 'Days Inn' or sl.Location = 'Old Mill Pancake') and t.subtypeid not in (Select c.comboitemid from t_ComboItems c inner join t_Combos co on c.ComboID = co.ComboID where co.comboname = 'TourSubType' and c.comboitem LIKE '%Exit%') and t.tourdate between '" & sdate & "' and '" & edate & "'", cn, 3, 3)
        If rs.EOF And rs.BOF Then
            sANs = sANs & "<tr><td colspan = '5'>No Tours In This Date Range</td></tr>"
        Else
            Do While Not rs.EOF
                sANs = sANs & "<tr>"
                sANs = sANs & "<td>" & rs.Fields("TourID").Value & "</td>"
                sANs = sANs & "<td>" & rs.Fields("TourDate").Value & "</td>"
                sANs = sANs & "<td>" & rs.Fields("TourStatus").Value & "</td>"
                sANs = sANs & "<td>" & rs.Fields("FirstName").Value & " " & rs.Fields("LastName").Value & "</td>"
                If rs.Fields("TourStatus").Value = "Showed" Then
                    sANs = sANs & "<td align = right>$25.00</td>"
                    grandTotal = grandTotal + 25
                Else
                    sANs = sANs & "<td align = right>$0.00</td>"
                End If
                sANs = sANs & "</tr>"
                rs.MoveNext()
            Loop
            sANs = sANs & "<tr>"
            sANs = sANs & "<td colspan = 5 align = right><B>" & Replace(Replace(FormatCurrency(grandTotal), "(", "-"), ")", "") & "</B></td>"
            sANs = sANs & "</tr>"
        End If
        sANs = sANs & "</table>"
        rs.Close()
        cn.Close()
        litReport.Text = sANs
    End Sub

    Protected Sub Button2_Click(sender As Object, e As System.EventArgs) Handles Button2.Click
        Response.Clear()
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment; filename=LocationInvoice.xls")
        Response.Write(litReport.Text)
        Response.End()
    End Sub
End Class
