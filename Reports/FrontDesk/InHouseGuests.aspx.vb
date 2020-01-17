
Partial Class Reports_FrontDesk_InHouseGuests
    Inherits System.Web.UI.Page

    Protected Sub RadioButtonList1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblSort.SelectedIndexChanged
        Run_Report()
    End Sub

    Protected Sub Run_Report()
        Dim cn As Object
        Dim rs As Object
        Dim rs2 As Object
        Dim sAns As String = ""
        Dim sDate As Date


        cn = Server.CreateObject("ADODB.Connection")
        rs = Server.CreateObject("ADODB.Recordset")
        rs2 = Server.CreateObject("ADODB.Recordset")
        cn.open(Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)

        sAns = "<table>"
        If rblSort.SelectedValue = "By Guest" Then
            sANs = sAns & "<tr><th align = 'left' width = '150px'><u>Guest</u></th><th><u>State</u></th><th width = '150px' align = 'left'><u>Room(s)</u></th><th align = 'left' width = '150px'><u>Extension</u></th><th align = 'left' width = '150px'><u>InDate</u></th><th align = 'left' width = '150px'><u>OutDate</u></th><th><u>Type/SubType</u></th><th><u>Tour Campaign</u></th><th><u>ReservationID</u></th></tr>"
            rs.Open("Select e.*, f.COmboItem as ResSubTYpe from (Select c.*, d.COmboItem as ResType from (Select a.ReservationID, b.ProspectID, b.FirstName, b.LastName,'' as State, a.CheckInDate, a.CheckOutDate, a.TypeID, a.SubTypeID from t_Reservations a inner join t_Prospect b on a.ProspectID = b.ProspectID  where a.statusid in (Select comboitemid from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid where comboname = 'ReservationStatus' and comboitem = 'In-House') and reslocationid in (Select comboitemid from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid where comboname = 'ReservationLocation' and comboitem = 'KCP')) c left outer join t_ComboItems d on c.TypeID = d.comboitemid) e left outer join t_ComboItems f on e.subtypeid = f.comboitemid order by e.Lastname asc", cn, 3, 3)
            Do While Not rs.EOF
                sAns = sAns & "<tr style='border-top:thin solid black;'>"
                sAns = sAns & "<td style='border-top:thin solid black;'>" & rs.Fields("LastName").value & "" & ", " & rs.Fields("FirstName").value & "" & "</td>"
                sAns = sAns & "<td style='border-top:thin solid black;'>" & rs.fields("State").value & "" & "</td>"
                sAns = sAns & "<td style='border-top:thin solid black;'>"
                If CDate(rs.Fields("CheckOUtDate").value & "") = Date.Today Then
                    sDate = CDate(Date.Today.AddDays(-1))
                Else
                    sDate = Date.Today
                End If
                '***** Older QUery replace 4/1/2008 *******'
                'rs2.Open "Select RoomNumber, left(ut.comboitem, 1) as Unittype from t_Room r left outer join t_Unit u on u.unitid = r.unitid left outer join t_Comboitems ut on ut.comboitemid = u.typeid where roomid in (Select Distinct(RoomID) from t_RoomAllocationmatrix where reservationid = '" & rs.Fields("ReservationID") & "') order by CHARINDEX('-',RoomNumber), RoomNumber", cn, 3, 3
                rs2.Open("Select RoomNumber, left(ut.comboitem, 1) as Unittype from t_Room r left outer join t_Unit u on u.unitid = r.unitid left outer join t_Comboitems ut on ut.comboitemid = u.typeid where roomid in (Select Distinct(RoomID) from t_RoomAllocationmatrix where reservationid = '" & rs.Fields("ReservationID").value & "" & "' and DateALlocated = '" & sDate & "') order by CHARINDEX('-',RoomNumber), RoomNumber", cn, 3, 3)
                Do While Not rs2.EOF
                    sAns = sAns & rs2.Fields("RoomNumber").value & "" & " - " & rs2.fields("UnitType").value & "<br>"
                    rs2.MoveNext()
                Loop
                rs2.Close()
                sAns = sAns & "</td>"
                sAns = sAns & "<td style='border-top:thin solid black;'>"
                rs2.Open("Select Phone from t_Room where roomid in (Select Distinct(RoomID) from t_RoomAllocationmatrix where reservationid = '" & rs.Fields("ReservationID").value & "" & "' and dateallocated = '" & sDate & "') order by CHARINDEX('-',RoomNumber), RoomNumber", cn, 3, 3)
                Do While Not rs2.EOF
                    sAns = sAns & rs2.Fields("Phone").value & "" & "<br>"
                    rs2.MoveNext()
                Loop
                rs2.Close()
                sAns = sAns & "</td>"
                sAns = sAns & "<td style='border-top:thin solid black;'>" & rs.Fields("CheckInDate").value & "" & "</td>"
                sAns = sAns & "<td style='border-top:thin solid black;'>" & rs.Fields("CheckOutDate").value & "" & "</td>"
                sAns = sAns & "<td style='border-top:thin solid black;'>" & rs.Fields("ResType").value & "" & "/" & rs.Fields("ResSubType").value & "" & "</td>"
                rs2.Open("Select a.Name from t_Campaign a inner join (Select campaignID from t_Tour where prospectid = '" & rs.Fields("ProspectID").value & "" & "' and tourdate between '" & rs.Fields("CheckInDate").value & "" & "' and '" & rs.Fields("CheckOutDate").value & "" & "') b on a.campaignid = b.campaignID", cn, 3, 3)
                If rs2.EOF And rs2.BOF Then
                    sAns = sAns & "<td style='border-top:thin solid black;'>&nbsp</td>"
                Else
                    sAns = sAns & "<td style='border-top:thin solid black;'>" & rs2.Fields("Name").value & "" & "</td>"
                End If
                rs2.Close()
                sAns = sAns & "<td style='border-top:thin solid black;' align = right><a href = '../../marketing/editReservation.aspx?reservationid=" & rs.Fields("reservationID").value & "" & "'>" & rs.Fields("ReservationID").value & "" & "</a></td>"

                sAns = sAns & "</tr>"
                rs2.open("select * from t_Guest where guestid in (select guestid from t_Res2Guest where reservationid = '" & rs.fields("ReservationID").value & "')", cn, 0, 1)
                If rs2.eof And rs2.bof Then
                Else
                    sAns = sAns & "<tr>"
                    sAns = sAns & "<td colspan = 9>Additional Guests</td>"
                    sAns = sAns & "</tr>"
                    Do While Not rs2.eof
                        sAns = sAns & "<tr>"
                        sAns = sAns & "<td colspan = 9>" & rs2.fields("Lastname").value & ", " & rs2.fields("Firstname").value & "</td>"
                        sAns = sAns & "</tr>"
                        rs2.movenext()
                    Loop
                End If
                rs2.close()
                rs.MoveNext()
            Loop
            rs.Close()
        Else
            sAns = sAns & "<tr><th width = '100px' align = 'left'><u>Room</u></th><th align = 'left'><u>Guest</u></th><th><u>State</u></th><th width = '150px' align = 'left'><u>Phone</u></th><th width = '150px' align = 'left'><u>InDate</u></th><th width = '150px' align = 'left'><u>OutDate</u></th><th><u>Type/SubType</u></th><th><u>Tour Campaign</u></th><th><u>ReservationID</u></th></tr>"
            '******* Older Query replace 4/1/2008
            'rs.Open "Select i.*, j.ComboItem as ResSubType from (Select g.*, h.ComboItem as ResType from (Select e.*, f.FirstName, f.Lastname, state.comboitem as state  from (Select c.*, d.ProspectID, d.CheckInDate, d.CheckOutDate, d.TypeID, d.SubTypeID from (Select a.*, b.RoomNumber, b.Phone, left(ut.comboitem,1) as unittype from (Select Distinct(RoomID), ReservationID from t_RoomAllocationmatrix where dateallocated = '" & Date & "' and reservationid in (Select reservationid from t_Reservations where statusid in (Select comboitemid from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid where comboname = 'ReservationStatus' and comboitem = 'In-House'))) a inner join t_Room b on a.roomid = b.roomid left outer join t_Unit unit on unit.unitid = b.unitid left outer join t_Comboitems ut on ut.comboitemid = unit.typeid) c inner join t_Reservations d on c.reservationid = d.reservationid) e inner join t_Prospect f on e.prospectid = f.prospectid left outer join t_Comboitems state on state.comboitemid = f.stateid) g left outer join t_CombOItems h on g.TypeID = h.comboitemid) i left outer join t_ComboItems j on i.subtypeid = j.comboitemid Order by CHARINDEX('-', i.RoomNumber), i.RoomNumber", cn, 3, 3 
            rs.Open("Select z.* from (Select i.*, j.ComboItem as ResSubType from (Select g.*, h.ComboItem as ResType from (Select e.*, f.FirstName, f.Lastname, '' as state  from (Select c.*, d.ProspectID, d.CheckInDate, d.CheckOutDate, d.TypeID, d.SubTypeID from (Select a.*, b.RoomNumber, b.Phone, left(ut.comboitem,1) as unittype from (Select Distinct(RoomID), ReservationID from t_RoomAllocationmatrix where dateallocated = '" & Date.Today.ToShortDateString & "' and reservationid in (Select reservationid from t_Reservations where statusid in (Select comboitemid from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid where comboname = 'ReservationStatus' and comboitem = 'In-House'))) a inner join t_Room b on a.roomid = b.roomid left outer join t_Unit unit on unit.unitid = b.unitid left outer join t_Comboitems ut on ut.comboitemid = unit.typeid) c inner join t_Reservations d on c.reservationid = d.reservationid) e inner join t_Prospect f on e.prospectid = f.prospectid ) g left outer join t_CombOItems h on g.TypeID = h.comboitemid) i left outer join t_ComboItems j on i.subtypeid = j.comboitemid UNION Select i.*, j.ComboItem as ResSubType from (Select g.*, h.ComboItem as ResType from (Select e.*, f.FirstName, f.Lastname, '' as state  from (Select c.*, d.ProspectID, d.CheckInDate, d.CheckOutDate, d.TypeID, d.SubTypeID from (Select a.*, b.RoomNumber, b.Phone, left(ut.comboitem,1) as unittype from (Select Distinct(RoomID), ReservationID from t_RoomAllocationmatrix where dateallocated = '" & Date.Today.AddDays(-1).ToShortDateString & "' and reservationid in (Select reservationid from t_Reservations where statusid in (Select comboitemid from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid where comboname = 'ReservationStatus' and comboitem = 'In-House') and checkoutDate = '" & Date.Today & "')) a inner join t_Room b on a.roomid = b.roomid left outer join t_Unit unit on unit.unitid = b.unitid left outer join t_Comboitems ut on ut.comboitemid = unit.typeid) c inner join t_Reservations d on c.reservationid = d.reservationid) e inner join t_Prospect f on e.prospectid = f.prospectid ) g left outer join t_CombOItems h on g.TypeID = h.comboitemid) i left outer join t_ComboItems j on i.subtypeid = j.comboitemid) z Order by CHARINDEX('-', z.RoomNumber), z.RoomNumber", cn, 3, 3)
            Do While Not rs.EOF
                sAns = sAns & "<tr style='border-top:thin solid black;'>"
                sAns = sAns & "<td style='border-top:thin solid black;'>" & rs.Fields("RoomNumber").value & "" & " - " & rs.fields("Unittype").value & "</td>"
                sAns = sAns & "<td style='border-top:thin solid black;'>" & rs.Fields("LastName").value & "" & ", " & rs.Fields("FirstName").value & "" & "</td>"
                sAns = sAns & "<td style='border-top:thin solid black;'>" & rs.fields("State").value & "</td>"
                sAns = sAns & "<td style='border-top:thin solid black;'>" & rs.Fields("Phone").value & "" & "</td>"
                sAns = sAns & "<td style='border-top:thin solid black;'>" & rs.Fields("CheckInDate").value & "" & "</td>"
                sAns = sAns & "<td style='border-top:thin solid black;'>" & rs.Fields("CheckOutDate").value & "" & "</td>"
                sAns = sAns & "<td style='border-top:thin solid black;'>" & rs.Fields("ResType").value & "" & "/" & rs.Fields("ResSubType").value & "" & "</td>"
                rs2.Open("Select a.Name from t_Campaign a inner join (Select campaignID from t_Tour where prospectid = '" & rs.Fields("ProspectID").value & "" & "' and tourdate between '" & rs.Fields("CheckInDate").value & "" & "' and '" & rs.Fields("CheckOutDate").value & "" & "') b on a.campaignid = b.campaignID", cn, 3, 3)
                If rs2.EOF And rs2.BOF Then
                    sAns = sAns & "<td style='border-top:thin solid black;'>&nbsp</td>"
                Else
                    sAns = sAns & "<td style='border-top:thin solid black;'>" & rs2.Fields("Name").value & "" & "</td>"
                End If
                rs2.Close()
                sAns = sAns & "<td style='border-top:thin solid black;' align = right><a href = '../../marketing/editReservation.aspx?reservationid=" & rs.Fields("reservationID").value & "" & "'>" & rs.Fields("ReservationID").value & "" & "</a></td>"

                sAns = sAns & "</tr>"
                rs2.open("select * from t_Guest where guestid in (select guestid from t_Res2Guest where reservationid = '" & rs.fields("ReservationID").value & "')", cn, 0, 1)
                If rs2.eof And rs2.bof Then
                Else
                    sAns = sAns & "<tr>"
                    sAns = sAns & "<td colspan = 9>Additional Guests</td>"
                    sAns = sAns & "</tr>"
                    Do While Not rs2.eof
                        sAns = sAns & "<tr>"
                        sAns = sAns & "<td colspan = 9>" & rs2.fields("Lastname").value & ", " & rs2.fields("Firstname").value & "</td>"
                        sAns = sAns & "</tr>"
                        rs2.movenext()
                    Loop
                End If
                rs2.close()
                rs.MoveNext()
            Loop
            rs.Close()
        End If
        sAns = sAns & "</table>"
        Lit.text = sAns
    End Sub

    Protected Sub btnPrintable_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintable.Click
        ClientScript.RegisterClientScriptBlock(Me.GetType, "printable", "var mwin=window.open();mwin.document.write('" & Replace(Lit.Text, "'", "\'") & "');", True)
    End Sub
End Class
