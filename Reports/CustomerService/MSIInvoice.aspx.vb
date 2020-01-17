
Partial Class Reports_CustomerService_MSIInvoice
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
        Dim sANs As String = ""
        Dim premCost As Double = 0
        Dim grandTotal As Double = 0
        Dim totalTourPay As Double = 0
        Dim tourPay As Double = 0
        totalTourPay = 0
        tourPay = 0
        premCost = 0
        sANs = "<H2>MSI Invoice " & sdate & " - " & edate & "</H2>"
        sAns = sAns & "<table>"
        sAns = sAns & "<tr>"
        sAns = sAns & "<th><u>TourID</u></th>"
        sAns = sAns & "<th><u>Prospect</u></th>"
        sAns = sAns & "<th><u>TourDate</u></th>"
        sAns = sAns & "<th><u>TourStatus</u></th>"
        sAns = sAns & "<th><u>Premiums</u></th>"
        sAns = sANs & "<th><u>Premium Cost</u></th>"
        sAns = sAns & "<th><u>Tour Pay</u></th>"
        sAns = sAns & "</tr>"

        Server.ScriptTimeout = 10000
        cn.open(Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
        cn.CommandTimeout = 0
        rs.Open("SELECT a.TourID, a.TourDate, b.FirstName, b.LastName, c.ComboItem AS TourStatus FROM t_Tour a INNER JOIN t_Prospect b ON a.ProspectID = b.ProspectID LEFT OUTER JOIN t_ComboItems c ON a.StatusID = c.ComboItemID where campaignid = (Select campaignid from t_Campaign where name = 'MSI') and tourdate between '" & sdate & "' and '" & edate & "'", cn, 3, 3)
        If rs.EOF And rs.BOF Then
            sAns = sAns & "<tr><td colspan = '8'>No Tours In This Date Range</td></tr>"
        Else
            Do While Not rs.EOF
                sAns = sAns & "<tr>"
                sANs = sANs & "<td>" & rs.Fields("TourID").value & "</td>"
                sANs = sANs & "<td>" & rs.Fields("FirstName").value & " " & rs.Fields("LastName").value & "</td>"
                sANs = sANs & "<td>" & rs.Fields("TourDate").value & "</td>"
                sANs = sANs & "<td>" & rs.Fields("TourStatus").value & "</td>"
                sAns = sAns & "<td><table>"
                rs2.Open("Select b.PremiumName, a.CostEA, a.QtyIssued, a.TotalCost from t_PremiumIssued a inner join t_Premium b on a.premiumid = b.premiumid where a.KeyField = 'TourID' and a.KeyValue = '" & rs.Fields("TourID").Value & "' and a.statusid = (Select c.comboitemid from t_Comboitems c inner join t_COmbos co on c.ComboID = co.COmboID where co.comboname = 'PremiumStatus' and c.comboitem = 'Issued')", cn, 3, 3)
                If rs2.EOF And rs2.BOF Then
                Else
                    Do While Not rs2.EOF
                        sAns = sAns & "<tr>"
                        sANs = sANs & "<td>" & rs2.Fields("PremiumName").value & "</td>"
                        sANs = sANs & "<td>" & rs2.Fields("QtyIssued").value & "</td>"
                        sANs = sANs & "<td>" & rs2.Fields("TotalCost").value & "</td>"
                        sAns = sANs & "</tr>"
                        premCost = premCost + rs2.Fields("TotalCost").value
                        rs2.MoveNext()
                    Loop
                End If
                rs2.Close()
                sAns = sAns & "</table></td>"
                If rs.Fields("ComboItem").value = "Showed" Then
                    tourPay = 325
                Else
                    tourPay = 0
                End If
                totalTourPay = totalTourPay + (tourPay - premCost)
                sAns = sAns & "<td>" & FormatCurrency((tourPay - premCost), 2) & "</td>"
                sAns = sAns & "</tr>"
                premCost = 0
                tourPay = 0
                rs.MoveNExt()
            Loop
        End If
        rs.Close()
        sAns = sAns & "</table>"
        cn.Close()
        rs = Nothing
        rs2 = Nothing
        cn = Nothing
        litReport.Text = sANs
    End Sub

    Protected Sub Button3_Click(sender As Object, e As System.EventArgs) Handles Button3.Click
        Response.Clear()
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment; filename=MSIInvoice.xls")
        Response.Write(litReport.Text)
        Response.End()
    End Sub
End Class
