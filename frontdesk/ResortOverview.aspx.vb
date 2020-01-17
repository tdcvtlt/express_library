Imports System.Data.Sql
Imports System.Data.SqlClient

Partial Class frontdesk_ResortOverview
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Set_Lookups()
            'Build_Report()
        End If
    End Sub

    Private Sub Set_Lookups()
        siRoomType.Connection_String = Resources.Resource.cns
        siRoomType.ComboItem = "UnitType"
        siRoomType.Label_Caption = "Room Type"
        siRoomType.Load_Items()
        siRoomSubType.Connection_String = Resources.Resource.cns
        siRoomSubType.ComboItem = "UnitSubType"
        siRoomSubType.Label_Caption = "Room Sub Type"
        siRoomSubType.Load_Items()
    End Sub

    Protected Sub btnRun_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRun.Click
        Build_Report()
    End Sub

    Private Sub Build_Report()
        Dim sql As String = "Select (select count(*) from t_Reservations res where res.reservationid = (select ReservationID from t_RoomAllocationMatrix where roomid=r.RoomID and dateallocated = '" & Date.Today & "') and (checkindate='" & Date.Today & "' or statusid in (select comboitemid from t_Comboitems i inner join t_Combos c on c.comboid=i.comboid where c.comboname = 'ReservationStatus' and i.comboitem = 'Booked'))) as Checkins, r.roomid,count(distinct req.requestid) as Requests, r.roomnumber,coalesce((select reservationid from t_Roomallocationmatrix where roomid=r.roomid and dateallocated = '" & Date.Today & "' ),0) as resid, rs.comboitem as status,rms.comboitem as maintstatus, rhs.comboitem as hkstatus, ut.comboitem as ut, ust.comboitem as ust from t_Room r  left outer join t_Comboitems rs on rs.comboitemid = r.statusid inner join t_Unit u on u.unitid = r.unitid inner join t_Comboitems ut on ut.comboitemid = u.typeid inner join t_comboitems ust on ust.comboitemid = u.subtypeid left outer join t_Comboitems rms on rms.comboitemid = r.maintenancestatusid left outer join t_Comboitems rhs on rhs.comboitemid = r.housekeepingstatusid left outer join (select * from t_Request where statusid not in (select comboitemid from t_Comboitems inner join t_Combos on t_Combos.comboid = t_Comboitems.comboid where comboname = 'workorderstatus' and comboitem in ('complete', 'completed','canceled','cancelled'))) req on req.roomid = r.roomid group by r.roomnumber,rs.comboitem,rms.comboitem , rhs.comboitem, ut.comboitem, ust.comboitem, r.roomid order by CharIndex('-',roomnumber), r.roomnumber asc, ut.comboitem"
        If siRoomType.Selected_ID > 0 And siRoomSubType.Selected_ID > 0 Then
            sql = "Select (select count(*) from t_Reservations res where res.reservationid = (select ReservationID from t_RoomAllocationMatrix where roomid=r.RoomID and dateallocated = '" & Date.Today & "') and (checkindate='" & Date.Today & "' or statusid in (select comboitemid from t_Comboitems i inner join t_Combos c on c.comboid=i.comboid where c.comboname = 'ReservationStatus' and i.comboitem = 'Booked'))) as Checkins,r.roomid,count(distinct req.requestid) as Requests, r.roomnumber,coalesce((select reservationid from t_Roomallocationmatrix where roomid=r.roomid and dateallocated = cast(datepart(mm,GETDATE()) as varchar(2)) + '/' + cast(DATEPART(dd,getdate()) as varchar(2)) + '/' + cast(DATEPART(yyyy,getdate()) as varchar(4)) ),0) as resid,rs.comboitem as status,rms.comboitem as maintstatus, rhs.comboitem as hkstatus, ut.comboitem as ut, ust.comboitem as ust from t_Room r  left outer join t_Comboitems rs on rs.comboitemid = r.statusid inner join t_Unit u on u.unitid = r.unitid inner join t_Comboitems ut on ut.comboitemid = u.typeid inner join t_comboitems ust on ust.comboitemid = u.subtypeid left outer join t_Comboitems rms on rms.comboitemid = r.maintenancestatusid left outer join t_Comboitems rhs on rhs.comboitemid = r.housekeepingstatusid left outer join (select * from t_Request where statusid not in (select comboitemid from t_Comboitems inner join t_Combos on t_Combos.comboid = t_Comboitems.comboid where comboname = 'workorderstatus' and comboitem in ('complete', 'completed','canceled','cancelled'))) req on req.roomid = r.roomid where ust.comboitemid in (" & siRoomSubType.Selected_ID & ") and ut.comboitemid in (" & siRoomType.Selected_ID & ") group by r.roomnumber,rs.comboitem,rms.comboitem , rhs.comboitem, ut.comboitem, ust.comboitem, r.roomid order by CharIndex('-',roomnumber), r.roomnumber asc, ut.comboitem"
        ElseIf siRoomSubType.Selected_ID > 0 Then
            sql = "Select (select count(*) from t_Reservations res where res.reservationid = (select ReservationID from t_RoomAllocationMatrix where roomid=r.RoomID and dateallocated = '" & Date.Today & "') and (checkindate='" & Date.Today & "' or statusid in (select comboitemid from t_Comboitems i inner join t_Combos c on c.comboid=i.comboid where c.comboname = 'ReservationStatus' and i.comboitem = 'Booked'))) as Checkins,r.roomid,count(distinct req.requestid) as Requests, r.roomnumber,coalesce((select reservationid from t_Roomallocationmatrix where roomid=r.roomid and dateallocated = cast(datepart(mm,GETDATE()) as varchar(2)) + '/' + cast(DATEPART(dd,getdate()) as varchar(2)) + '/' + cast(DATEPART(yyyy,getdate()) as varchar(4)) ),0) as resid,rs.comboitem as status,rms.comboitem as maintstatus, rhs.comboitem as hkstatus, ut.comboitem as ut, ust.comboitem as ust from t_Room r  left outer join t_Comboitems rs on rs.comboitemid = r.statusid inner join t_Unit u on u.unitid = r.unitid inner join t_Comboitems ut on ut.comboitemid = u.typeid inner join t_comboitems ust on ust.comboitemid = u.subtypeid left outer join t_Comboitems rms on rms.comboitemid = r.maintenancestatusid left outer join t_Comboitems rhs on rhs.comboitemid = r.housekeepingstatusid left outer join (select * from t_Request where statusid not in (select comboitemid from t_Comboitems inner join t_Combos on t_Combos.comboid = t_Comboitems.comboid where comboname = 'workorderstatus' and comboitem in ('complete', 'completed','canceled','cancelled'))) req on req.roomid = r.roomid where ust.comboitemid in (" & siRoomSubType.Selected_ID & ") group by r.roomnumber,rs.comboitem,rms.comboitem , rhs.comboitem, ut.comboitem, ust.comboitem, r.roomid order by CharIndex('-',roomnumber), r.roomnumber asc, ut.comboitem"
        ElseIf siRoomType.Selected_ID > 0 Then
            sql = "Select (select count(*) from t_Reservations res where res.reservationid = (select ReservationID from t_RoomAllocationMatrix where roomid=r.RoomID and dateallocated = '" & Date.Today & "') and (checkindate='" & Date.Today & "' or statusid in (select comboitemid from t_Comboitems i inner join t_Combos c on c.comboid=i.comboid where c.comboname = 'ReservationStatus' and i.comboitem = 'Booked'))) as Checkins,r.roomid,count(distinct req.requestid) as Requests, r.roomnumber,coalesce((select reservationid from t_Roomallocationmatrix where roomid=r.roomid and dateallocated = cast(datepart(mm,GETDATE()) as varchar(2)) + '/' + cast(DATEPART(dd,getdate()) as varchar(2)) + '/' + cast(DATEPART(yyyy,getdate()) as varchar(4)) ),0) as resid,rs.comboitem as status,rms.comboitem as maintstatus, rhs.comboitem as hkstatus, ut.comboitem as ut, ust.comboitem as ust from t_Room r  left outer join t_Comboitems rs on rs.comboitemid = r.statusid inner join t_Unit u on u.unitid = r.unitid inner join t_Comboitems ut on ut.comboitemid = u.typeid inner join t_comboitems ust on ust.comboitemid = u.subtypeid left outer join t_Comboitems rms on rms.comboitemid = r.maintenancestatusid left outer join t_Comboitems rhs on rhs.comboitemid = r.housekeepingstatusid left outer join (select * from t_Request where statusid not in (select comboitemid from t_Comboitems inner join t_Combos on t_Combos.comboid = t_Comboitems.comboid where comboname = 'workorderstatus' and comboitem in ('complete', 'completed','canceled','cancelled'))) req on req.roomid = r.roomid where ut.comboitemid in (" & siRoomType.Selected_ID & ") group by r.roomnumber,rs.comboitem,rms.comboitem , rhs.comboitem, ut.comboitem, ust.comboitem, r.roomid order by CharIndex('-',roomnumber), r.roomnumber asc, ut.comboitem"
        End If
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand(sql, cn)
        Dim rs As SqlDataReader
        cn.Open()
        rs = cm.ExecuteReader
        Timer1.Interval = IIf(IsNumeric(txtUpdate.Text) And txtUpdate.Text <> "", txtUpdate.Text * 1000, 5000)
        Dim lstType As String = ""
        Dim msc As String = ""
        Dim hksc As String = ""
        Dim rsc As String = ""

        lit1.Text = ""
        'lit1.Text &= siRoomType.Selected_ID.ToString
        'lit1.Text &= siRoomSubType.Selected_ID.ToString
        'lit1.Text &= sql
        lit1.Text &= ("<table>")
        If Not rs.HasRows Then

            lit1.Text &= ("<tr><td>NO RECORDS</td></tr>")
        Else
            Dim colcounter As Integer = 0

            Do While rs.Read
                If Not ckToday.Checked Or rs("CheckIns") > 0 Then



                    'HouseKeeping Status Color
                    Select Case Trim(rs("HKStatus") & "")
                        Case "Cleaned"
                            hksc = "#00FF00;"
                        Case "Occupied"
                            hksc = "#FFFF00;"
                        Case "Offline"
                            hksc = "#FF0000;"
                        Case "Dirty"
                            hksc = "#FFFF00;"
                        Case "Being Cleaned"
                            hksc = "#FFFF00;"
                        Case "Needs Inspected"
                            hksc = "#FFFF00;"
                        Case "Maintenance In"
                            hksc = "#FFFF00;"
                        Case "Maintenance Out"
                            hksc = "#FFFF00;"
                        Case Else
                            hksc = "#FFFF00;"
                    End Select

                    'Maintenance Status
                    Select Case Trim(rs("MaintStatus") & "")
                        Case "Cleaned", "Quick check complete"
                            msc = "#00FF00;"
                        Case "Occupied"
                            msc = "#FFFF00;"
                        Case "Offline"
                            msc = "#FF0000;"
                        Case "Dirty"
                            msc = "#FFFF00;"
                        Case "Being Cleaned"
                            msc = "#FFFF00;"
                        Case "Needs Inspected"
                            msc = "#FFFF00;"
                        Case "Maintenance In", "Painter In"
                            msc = "#FFFF00;"
                        Case "Maintenance Out", "Painter Out"
                            msc = "#FFFF00;"
                        Case Else
                            msc = "#FFFF00;"
                    End Select

                    If rs("resid") >= 0 Then
                        'Room Status
                        Select Case Trim(rs("Status") & "")
                            Case "Cleaned"
                                rsc = "#00FF00;"
                            Case "Occupied"
                                rsc = "#FF8C00;"
                            Case "Offline"
                                rsc = "#FFFF00;"
                            Case "Dirty"
                                rsc = "#FFFF00;"
                            Case "Being Cleaned"
                                rsc = "#FFFF00;"
                            Case "Needs Inspected"
                                rsc = "#FFFF00;"
                            Case "Maintenance In"
                                rsc = "#FFFF00;"
                            Case "Maintenance Out"
                                rsc = "#FFFF00;"
                            Case Else
                                rsc = "#FFFF00;"
                        End Select
                        If msc = "#00FF00;" And hksc = "#00FF00;" And rsc <> "#FF8C00;" Then
                            rsc = "#00FF00;"
                        Else
                            If rsc <> "#FF8C00;" Then rsc = "#FFFF00;"
                        End If
                    Else
                        'Room Status
                        rsc = "#FF0000;"
                    End If
                    If rsc = ddStatus.SelectedValue Or ddStatus.SelectedValue = "" Then
                        If colcounter Mod 10 = 0 Then
                            If colcounter = 0 Then
                                lit1.Text &= ("<tr>")
                            Else
                                lit1.Text &= ("</tr><tr>")
                            End If
                        End If
                        colcounter = colcounter + 1
                        'if lstType <> rs.fields("UT").value then
                        '	lstType = rs.fields("UT").value & ""
                        '	lit1.text &=  "<tr><td colspan = 10>" & lstType & "</td></tr>"
                        'end if
                        lit1.Text &= ("<td>")
                        lit1.Text &= ("<div id='" & rs("RoomID") & "' style='text-align:center;border:thin solid black;background-color:" & rsc & "'>")
                        lit1.Text &= ("<a href = '../marketing/editroom.aspx?roomid=" & rs("RoomID") & "'>" & rs("RoomNumber") & "</a>")
                        lit1.Text &= ("<br>Reqs: ")
                        If rs("Requests") & "" = "" Then
                            lit1.Text &= ("0")
                        Else
                            lit1.Text &= (rs("Requests"))
                        End If
                        lit1.Text &= ("<table width='100%'><tr>")
                        lit1.Text &= ("<td style='text-align:center;background-color:" & msc & "'>M</td><td style='text-align:center;background-color:" & hksc & "'>H</td>")
                        lit1.Text &= ("</tr></table>")
                        lit1.Text &= ("</div></td>")
                    End If
                    'style='position:relative;background-color:"

                End If

            Loop
        End If
        lit1.Text &= ("</div>")
        rs.Close()
        cn.Close()
    End Sub

    Protected Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Build_Report()
    End Sub
End Class
