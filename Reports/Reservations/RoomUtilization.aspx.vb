Imports Microsoft.VisualBasic
Partial Class Reports_Reservations_RoomUtilization
    Inherits System.Web.UI.Page

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        Dim cn As Object
        Dim rs As Object
        Dim rs2 As Object
        Dim edate As String = Me.dteEDate.Selected_Date
        Dim sdate As String = Me.dteSDate.Selected_Date
        Dim sAns As String = ""
        cn = Server.CreateObject("ADODB.Connection")
        rs = Server.CreateObject("ADODB.Recordset")
        rs2 = Server.CreateObject("ADODB.Recordset")

        Server.ScriptTimeout = 10000
        cn.open(Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
        cn.CommandTimeout = 0

        sAns = "<table>"
        sANs = sAns & "<tr><th>Room Number</th>"
        rs.Open("Select c.comboitem from t_ComboItems c inner join t_Combos co on c.ComboID = co.CombOID where co.comboname = 'reservationtype' order by c.comboitem asc", cn, 3, 3)
        Do While Not rs.EOF
            sAns = sAns & "<th>" & rs.Fields("ComboItem").Value & "</th>"
            rs.MoveNext()
        Loop
        rs.Close()
        sANs = sANs & "<th>No Usage</th>"
        sAns = sAns & "</tr>"


        rs.Open("Select RoomNumber, RoomID from t_Room order by CharIndex('-', roomnumber), RoomNumber asc", cn, 3, 3)
        Do While Not rs.EOF
            sAns = sAns & "<tr><td>" & rs.Fields("RoomNumber").value & "</td>"
            rs2.Open("Select y.ComboItem as ResType, z.Days, z.PCT from t_ComboItems y left outer join (SELECT DISTINCT UsageType, COUNT(AllocationID) AS Days, CAST(COUNT(AllocationID) AS money) / CAST(DATEDIFF(d, '" & sdate & "', '" & edate & "') + 1 AS money) * 100 AS PCT FROM (Select g.*, h.ComboItem as ResStatus from (Select e.*, f.StatusID from (SELECT c.*, d .ComboItem AS UsageType FROM (SELECT a.*, b.TypeID AS UsageTypeID FROM t_RoomAllocationMatrix a LEFT OUTER JOIN t_Usage b ON a.UsageID = b.UsageID WHERE (a.RoomID = '" & rs.Fields("RoomID").Value & "') AND (a.DateAllocated BETWEEN '" & sdate & "' AND '" & edate & "')) c LEFT OUTER JOIN t_ComboItems d ON c.UsageTypeID = d .ComboItemID) e left outer join t_Reservations f on e.reservationid = f.reservationid) g left outer join t_ComboItems h on g.statusid = h.comboitemid where h.comboitem is null or h.comboitem <> 'No Show') DERIVEDTBL GROUP BY UsageType) z on y.comboitem = z.UsageTYpe where y.comboid = (Select comboid from t_Combos where comboname = 'ReservationType') Order By y.ComboItem", cn, 3, 3)
            Do While Not rs2.EOF
                If IsDBNull(rs2.Fields("PCT").Value) Then
                    sAns = sAns & "<td align = 'center'>0%</td>"
                Else
                    sAns = sAns & "<td align = 'center'>" & FormatNumber(rs2.Fields("PCT").Value, 2) & "%</td>"
                End If
                rs2.MoveNext()
            Loop
            rs2.Close()

            rs2.Open("SELECT COUNT(AllocationID) AS Days, CAST(COUNT(AllocationID) AS money) / CAST(DATEDIFF(d, '" & sdate & "', '" & edate & "') + 1 AS money) * 100 AS PCT FROM (Select g.*, h.ComboItem as ResStatus from (Select e.*, f.StatusID from (SELECT c.*, d .ComboItem AS UsageType FROM (SELECT a.*, b.TypeID AS UsageTypeID FROM t_RoomAllocationMatrix a LEFT OUTER JOIN t_Usage b ON a.UsageID = b.UsageID WHERE (a.RoomID = '" & rs.Fields("RoomID").value & "') AND (a.DateAllocated BETWEEN '" & sdate & "' AND '" & edate & "')) c LEFT OUTER JOIN t_ComboItems d ON c.UsageTypeID = d .ComboItemID) e left outer join t_Reservations f on e.reservationid = f.reservationid) g left outer join t_ComboItems h on g.statusid = h.comboitemid where (g.UsageID is null or g.usageid = '0') or h.comboitem = 'No Show') DERIVEDTBL", cn, 3, 3)
            If rs2.EOf And rs2.BOF Or IsDBNull(rs2.Fields("PCT").value) Then
                sAns = sAns & "<td align = 'center'>0%</td>"
            Else
                sAns = sAns & "<td align = 'center'>" & FormatNumber(rs2.Fields("PCT").value, 2) & "%</td>"
            End If
            rs2.Close()
            sAns = sAns & "</tr>"
            rs.MoveNext()
        Loop
        rs.Close()
        sAns = sAns & "</table>"
        
        cn.Close()
        rs = Nothing
        rs2 = Nothing
        cn = Nothing

        litReport.Text = sAns
    End Sub
End Class
