
Partial Class Reports_CustomerService_BuschGardensInvoice
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        Dim cn As Object
        Dim rs As Object
        Dim rs2 As Object
        cn = Server.CreateObject("ADODB.Connection")
        rs = Server.CreateObject("ADODB.Recordset")
        rs2 = Server.CreateObject("ADODB.Recordset")
        Dim edate As String = Me.dteEDate.Selected_Date
        Dim sdate As String = Me.dteSDate.Selected_Date
        Dim sAns As String = ""
        Dim totalDue As Double = 0

        sAns = "<H2>Busch Gardens Invoice " & sdate & " - " & edate & "</H2>"
        sAns = sAns & "<table>"
        sAns = sAns & "<tr>"
        sAns = sAns & "<th><u>ReservationID</u></th>"
        sAns = sAns & "<th><u>Guest</u></th>"
        sAns = sAns & "<th><u>CheckIn</u></th>"
        sAns = sAns & "<th><u>Nights</u></th>"
        sAns = sAns & "<th><u>Reservation Status</u></th>"
        sAns = sAns & "<th><u>Balance Due</u></th>"
        sAns = sAns & "<th><u>Room Size</u></th>"
        sAns = sAns & "</tr>"
        Server.ScriptTimeout = 10000
        cn.open(Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
        cn.CommandTimeout = 0
        rs.Open("SELECT a.ReservationID, p.FirstName, p.LastName, a.CheckInDate, a.CheckOutDate, DateDiff(dd, a.CheckInDate, a.CheckOutDate) as Nights, c.ComboItem AS ResStatus, b.TourID, d.ComboItem AS TourStatus, s.ContractNumber, m.SalesVolume,(SELECT SUM(Cast(g.Rooms AS integer)) FROM (SELECT DISTINCT rm.RoomID, LEFT(rt.ComboItem, 1) AS Rooms FROM t_RoomAllocationMatrix rm INNER JOIN t_Room r ON rm.RoomID = r.RoomID INNER JOIN t_ComboItems rt ON r.TypeID = rt.ComboItemID WHERE ReservationID = a.ReservationID AND rm.DateAllocated = a.CheckInDate) g) AS RoomSize FROM t_Reservations a LEFT OUTER JOIN (SELECT * FROM t_Tour WHERE subTypeid <> (SELECT c.comboitemid FROM t_comboitems c inner join t_Combos co on c.ComboID = co.CombOID WHERE co.comboname = 'TourSubType' AND c.comboitem = 'Exit') AND locationid = (SELECT c.comboitemid FROM t_ComboItems c inner join t_Combos co on c.CombOID = co.CombOID WHERE co.comboname = 'TourLocation' AND c.comboitem = 'KCP')) b ON a.ProspectID = b.ProspectID LEFT OUTER JOIN t_Prospect p ON a.ProspectID = p.ProspectID LEFT OUTER JOIN t_ComboItems c ON a.StatusID = c.ComboItemID LEFT OUTER JOIN t_ComboItems d ON b.StatusID = d.ComboItemID LEFT OUTER JOIN t_Contract s ON s.TourID = b.TourID LEFT OUTER JOIN t_Mortgage m ON m.ContractID = s.ContractID WHERE (a.SourceID = (SELECT c.comboitemid FROM t_ComboItems c inner join t_Combos co on c.CombOID = co.COmboiD WHERE co.comboname = 'ReservationSource' AND c.comboitem = 'BG')) and a.CheckOutDate between '" & sdate & "' and '" & edate & "'", cn, 0, 1)
        If rs.EOF And rs.BOF Then
            sAns = sAns & "<tr><td colspan = '7'>No Reservations in Date Range</td></tr>"
        Else
            Do While Not rs.EOF
                sAns = sAns & "<tr>"
                sAns = sAns & "<td>" & rs.Fields("ReservationID").value & "</td>"
                sAns = sAns & "<td>" & rs.Fields("LastName").value & ", " & rs.Fields("Firstname").value & "</td>"
                sAns = sAns & "<td>" & rs.Fields("CheckInDate").value & "</td>"
                sAns = sAns & "<td>" & rs.Fields("Nights").value & "</td>"
                sAns = sAns & "<td>" & rs.Fields("ResStatus").value & "</td>"
                rs2.Open("Select Case when Sum(Balance) is null then 0 else Sum(balance) end as BalanceDue from UFN_Financials(0,'ReservationID'," & rs.Fields("ReservationID").value & ",0) where Invoice = 'CWorld'", cn, 0, 1)
                sAns = sAns & "<td align = 'center'>" & FormatCurrency(rs2.Fields("BalanceDue").value, 2) & "</td>"
                totalDue = totalDue + rs2.Fields("BalanceDue").value
                rs2.Close()
                sAns = sAns & "<td align = 'center'>" & rs.Fields("RoomSize").value & "</td>"
                sAns = sAns & "</tr>"
                rs.MoveNext()
            Loop
        End If
        rs.Close()
        sAns = sAns & "<tr><td colspan = '7' align = 'right'><B>Total Balance Due: " & FormatCurrency(totalDue, 2) & "</B></td></tr>"
        sAns = sAns & "</table>"

        cn.Close()
        rs = Nothing
        cn = Nothing
        litReport.Text = sAns
    End Sub

    Protected Sub Button3_Click(sender As Object, e As System.EventArgs) Handles Button3.Click
        Response.Clear()
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment; filename=BuschGardensInvoice.xls")
        Response.Write(litReport.Text)
        Response.End()
    End Sub
End Class
