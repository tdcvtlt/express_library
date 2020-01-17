
Partial Class Reports_CustomerService_GoodBuyInvoice
    Inherits System.Web.UI.Page

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click
        Dim cn As Object
        Dim rs As Object
        Dim rs2 As Object
        cn = Server.CreateObject("ADODB.Connection")
        rs = Server.CreateObject("ADODB.Recordset")
        rs2 = Server.CreateObject("ADODB.Recordset")
        Dim edate As String = Me.dteEDate.Selected_Date
        Dim sdate As String = Me.dteSDate.Selected_Date
        Dim sANs As String = ""
        Dim BD As Integer = 0
        Dim totalAmt As Double = 0
        sANs = "<H2>GoodBuy Vacations Invoice " & sdate & " - " & edate & "</H2>"
        sANs = sANs & "<table>"
        sANs = sANs & "<tr>"
        sANs = sANs & "<th><u>ReservationID</u></th>"
        sANs = sANs & "<th><u>Prospect</u></th>"
        sANs = sANs & "<th><u>HomePhone</u></th>"
        sANs = sANs & "<th><u>Check In Date</u></th>"
        sANs = sANs & "<th><u>Check Out Date</u></th>"
        sANs = sANs & "<th><u>Status</u></th>"
        sANs = sANs & "<th><u>Accommodations</u></th>"
        sANs = sANs & "<th><u>Amount</u></th>"
        sANs = sANs & "</tr>"

        Server.ScriptTimeout = 10000
        cn.open(Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
        cn.CommandTimeout = 0
        rs.Open("Select a.ReservationID, a.ProspectID, p.FirstName, p.LastName, a.CheckInDate, a.CheckOutDate, i.Amount, rs.ComboItem as Status, (Select Top 1 Number from t_ProspectPhone where active = 1 and prospectid = a.ProspectID) as HomePhone from t_Reservations a inner join t_Prospect p on a.ProspectID = p.ProspectID left outer join t_ComboItems rs on a.StatusID = rs.ComboItemID inner join t_invoices i on a.ReservationID = i.KeyValue inner join t_FinTransCodes f on i.FinTransID = f.FinTransID inner join t_ComboItems tc on f.TransCodeID = tc.ComboItemID where a.CheckInDate between '" & sdate & "' and '" & edate & "' and tc.ComboItem = 'GoodBuy' and i.KeyField = 'ReservationID' order by a.CheckInDate asc", cn, 3, 3)
        If rs.EOF And rs.BOF Then
            sANs = sANs & "<tr><td colspan = '6'>No Tours In This Date Range</td></tr>"
        Else
            Do While Not rs.EOF
                sANs = sANs & "<tr>"
                sANs = sANs & "<td>" & rs.FIelds("ReservationID").Value & "</td>"
                sANs = sANs & "<td>" & rs.FIelds("LastName").Value & ", " & rs.Fields("FirstName").Value & "</td>"
                sANs = sANs & "<td>" & rs.Fields("HomePhone").Value & "</td>"
                sANs = sANs & "<td>" & rs.FIelds("CheckInDate").Value & "</td>"
                sANs = sANs & "<td>" & rs.Fields("CheckOutDate").Value & "</td>"
                sANs = sANs & "<td>" & rs.Fields("Status").Value & "</td>"
                rs2.Open("Select (Left(b.comboitem, 1)) As BD from t_Room a inner join t_ComboItems b on a.typeid = b.comboitemid where a.roomid in (Select Distinct(RoomID) from t_RoomAllocationMatrix where reservationid = '" & rs.FIelds("ReservationID").Value & "' and dateallocated = '" & rs.Fields("CheckIndate").Value & "')", cn, 3, 3)
                If rs2.EOF And rs2.BOF Then
                    sANs = sANs & "<td></td>"
                Else
                    Do While Not rs2.EOF
                        BD = CDbl(rs2.Fields("BD").Value) + BD
                        rs2.MoveNext()
                    Loop
                    sANs = sANs & "<td>" & BD & "BD</td>"
                End If
                rs2.Close()
                sANs = sANs & "<td>" & FormatCurrency(rs.Fields("Amount").Value, 2) & "</td>"
                sANs = sANs & "</tr>"
                totalAmt = totalAmt + rs.Fields("Amount").Value
                rs.MoveNext()
                BD = 0
            Loop
            sANs = sANs & "<tr><td colspan = '7' align = 'right'>Grand Total:</td><td>" & FormatCurrency(totalAmt, 2) & "</td></tr>"
        End If
        rs.Close()
        sANs = sANs & "</table>"
        cn.Close()
        rs = Nothing
        rs2 = Nothing
        cn = Nothing
        litReport.Text = sANs
    End Sub

    Protected Sub Button3_Click(sender As Object, e As System.EventArgs) Handles Button3.Click
        Response.Clear()
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment; filename=GoodBuyInvoice.xls")
        Response.Write(litReport.Text)
        Response.End()
    End Sub
End Class
