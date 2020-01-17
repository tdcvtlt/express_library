
Partial Class Reports_FrontDesk_CheckInSheets
    Inherits System.Web.UI.Page

    Protected Sub btnRun_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRun.Click
        Dim cn As Object
        Dim rs As Object
        Dim sAns As String = ""
        Dim curRoom As String = ""
        Dim edate As String = Me.EDate.Selected_Date
        Dim sdate As String = Me.SDate.Selected_Date
        cn = Server.CreateObject("ADODB.Connection")
        rs = Server.CreateObject("ADODB.Recordset")

        cn.Open(Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)

        sAns = "<div id='printable'>"
        Dim sql As String = "select * from (" & _
                                "SELECT distinct b.CheckInDate, " & _
                                "    b.CheckOutDate, d.RoomNumber, d.Phone,  " & _
                                "	f.LastName + ', ' + f.FirstName AS Prospect, h.COmboItem AS ResType, j.ComboItem AS ResSubType, " & _
                                "	coalesce(hs.ComboItem,'') As HKS, coalesce(ms.ComboItem,'') As MS " & _
                                "FROM t_RoomAllocationMatrix a  " & _
                                "	INNER JOIN t_Reservations b On a.ReservationID = b.ReservationID  " & _
                                "	INNER JOIN t_Room d On a.RoomID = d.RoomID " & _
                                "	LEFT OUTER JOIN t_Prospect f On b.ProspectID = f.ProspectID " & _
                                "	LEFT OUTER JOIN t_CombOItems h On b.TypeID = h.ComboItemID " & _
                                "	LEFT OUTER JOIN t_ComboItems j On b.SubTypeID = j.ComboItemID  " & _
                                "	left outer join t_ComboItems hs On hs.ComboItemID=d.HouseKeepingStatusID " & _
                                "	left outer join t_ComboItems ms On ms.ComboItemID=d.MaintenanceStatusID " & _
                                "WHERE (b.CheckInDate BETWEEN '" & sdate & "' and '" & edate & "') " & _
                            ")a ORDER BY CHARINDEX('-',a.RoomNumber), a.RoomNumber, a.CheckInDate"
        'rs.Open("Select i.CheckInDate, i.CheckOutDate, i.RoomNumber, i.Phone, i.Prospect, i.ResType, j.ComboItem As ResSubType FROM (Select g.*, h.COmboItem As ResType FROM (Select e.*, f.LastName + ', ' + f.FirstName AS Prospect FROM (SELECT c.*, d .RoomNumber AS RoomNumber, d .Phone AS Phone FROM (SELECT DISTINCT a.ReservationID, a.RoomID, b.CheckInDate, b.CheckOutDate, b.ProspectID, b.TypeID, b.SubTypeID FROM t_RoomAllocationMatrix a INNER JOIN t_Reservations b ON a.ReservationID = b.ReservationID WHERE (b.CheckInDate BETWEEN '" & sdate & "' and '" & edate & "')) c INNER JOIN t_Room d ON c.RoomID = d .RoomID) e LEFT OUTER JOIN t_Prospect f ON e.ProspectID = f.ProspectID) g LEFT OUTER JOIN t_CombOItems h ON g.TypeID = h.ComboItemID) i LEFT OUTER JOIN t_ComboItems j ON i.SubTypeID = j.ComboItemID ORDER BY CHARINDEX('-',i.RoomNumber), i.RoomNumber, i.CheckInDate", cn, 3, 3)
        rs.Open(sql, cn, 3, 3)
        If rs.EOF And rs.BOF Then
            sAns = sAns & "No Data to Report"
        Else
            sAns = sAns & "<table><tr><th>Room #</th><th>MC</th><th>KCP</th><th>Guest Name</th><th>Ext</th><th>In</th><th>Out</th><th>ChkIn</th><th>ChkOut</th><th>Res Type</th></tr>"
            Do While Not rs.EOF
                If curRoom = "" Then
                    curRoom = rs.FIelds("RoomNumber").value & ""
                End If

                If curRoom <> rs.Fields("RoomNumber").value & "" Then
                    sAns = sAns & "<tr><td><BR></td></tr>"
                    curRoom = rs.Fields("RoomNumber").value & ""
                End If

                sAns = sAns & "<tr>"
                sAns = sAns & "<td><input type = 'text' size = '5' value = '" & rs.Fields("RoomNumber").value & "" & "' style = 'background-color:transparent;border-style:none;font:bolder arial;font-size:12pt'></td>"
                sAns = sAns & "<td><input type = 'text' size = '2' value = '"
                If rs.fields("HKS").value = "Cleaned" Then
                    sAns = sAns & "Ready"
                End If
                sAns = sAns & "'></td>"

                sAns = sAns & "<td><input type = 'text' size = '2' value = '"
                If rs.fields("MS").value = "Quick check complete" Then
                    sAns = sAns & "Ready"
                End If
                sAns = sAns & "'></td>"
                sAns = sAns & "<td align = 'center'>" & Trim(rs.Fields("Prospect").value & "") & "</td>"
                sAns = sAns & "<td>" & rs.Fields("Phone").value & "" & "</td>"
                sAns = sAns & "<td>" & rs.Fields("CheckInDate").value & "" & "</td>"
                sAns = sAns & "<td>" & rs.Fields("CheckOutDate").value & "" & "</td>"
                sAns = sAns & "<td><input type = 'text' size = '3'></td>"
                sAns = sAns & "<td><input type = 'text' size = '3'></td>"
                sAns = sAns & "<td>" & rs.Fields("ResType").value & "" & " / " & rs.Fields("ResSubType").value & "" & "</td>"
                sAns = sAns & "</tr>"

                rs.MoveNExt()
            Loop
            sAns = sAns & "</table>"
        End If
        rs.Close()

        sAns = sAns & "</div>"
        Lit.Text = sAns

        cn.Close()
        rs = Nothing
        cn = Nothing
    End Sub
End Class
