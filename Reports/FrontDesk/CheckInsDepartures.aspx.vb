Imports CrystalDecisions.CrystalReports.Engine

Partial Class Reports_FrontDesk_CheckInsDepartures
    Inherits System.Web.UI.Page


    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/CheckinsDepartures.rpt"

    Protected Sub btnRun_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRun.Click
        ReportInHtml()
    End Sub

    'Following complete code was moved from the Run button's event handler
    Private Sub ReportInHtml()

        Dim cn As Object
        Dim rs As Object
        Dim rs2 As Object
        Dim rsTour As Object
        Dim edate As String = Me.EDate.Selected_Date
        Dim sdate As String = Me.SDate.Selected_Date
        Dim endate As String = Me.EDate.Selected_Date
        Dim stdate As String = Me.SDate.Selected_Date
        Dim sAns As String
        Dim sCount As String
        Dim sSQL2 As String
        Dim sSQL3 As String

        cn = Server.CreateObject("ADODB.Connection")
        rs = Server.CreateObject("ADODB.Recordset")
        rs2 = Server.CreateObject("ADODB.Recordset")

        cn.open(Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
        Server.ScriptTimeout = 10000

        cn.commandtimeout = 26000
        'stdate = Request("stdate")
        'endate = Request("endate")
        'sdate = Request("sdate")
        'edate = Request("edate")

        sAns = ""
        sCount = ""
        Lit.Text = ""
        If rblType.SelectedValue = "checkins" Then
            Lit.Text &= "Check-Ins from " & edate & " to " & sdate
            sSQL2 = "SELECT distinct charIndex('-',r.roomnumber)as a, res.ReservationID, r.RoomNumber,t.comboitem as 'Room Type', s.comboitem as 'Room Status', p.LastName + ', ' + p.FirstName as Guest, res.checkInDate, res.checkOutDate, rt.comboitem as 'Type', rst.comboitem as 'SubType', rs.comboitem as 'ReservationStatus', source.comboitem as ReservationSource, r.RoomID  " &
             "FROM   t_Reservations res " &
             "left outer join t_comboitems rt on rt.comboitemid = res.typeid " &
             "left outer join t_comboitems rst on rst.comboitemid = res.subtypeid " &
             "left outer join t_prospect p on p.prospectid = res.prospectid " &
             "left outer join t_roomallocationmatrix ram on ram.reservationid = res.reservationid " &
             "left outer join t_room r on r.roomid = ram.roomid " &
             "left outer join t_comboitems t on t.comboitemid = r.typeid " &
             "left outer join t_comboitems rs on rs.comboitemid = res.statusid " &
             "left outer join t_comboitems s on s.comboitemid = r.statusid " &
             "left outer join t_comboitems source on source.comboitemid = res.sourceid " &
             "where rs.comboitem in ('Booked','Pending Payment') and res.checkindate between '" & sdate & "' and '" & edate & "' and res.reslocationid in (select comboitemid from t_comboitems where comboitem = 'KCP') and res.ReservationID <> 324936 "
            If cbRoom.Checked Then
                sSQL2 = sSQL2 & "order by charindex('-',r.roomnumber),r.roomnumber,guest asc"
            Else
                sSQL2 = sSQL2 & "order by guest asc"
            End If
            rs.open(sSQL2, cn, 3, 3)


            Lit.Text = "<TABLE>"
            If rs.bof And rs.eof Then
                Lit.Text &= "<tr><td><b>No Records</b></td></tr></table>"
            Else
                Lit.Text &= "<table><tr align = center>"
                For i = 1 To rs.fields.count - 1
                    Lit.Text &= "<th>" & rs.fields(i).name & "</th>"
                Next


                '*** Added fields to report per workorder 15241		

                Lit.Text &= "<th>Last Tour</th>"
                Lit.Text &= "<th>Pre-Booked</th>"

                '*** second recordset for tour lookup

                rsTour = Server.CreateObject("ADODB.Recordset")

                '*** variables 
                Dim PreBooked As String = ""
                Dim LastTourDate As String = ""

                '*** End field addition title


                Lit.Text &= "</tr>"

                Do While Not rs.eof
                    Lit.Text &= "<tr>"
                    sSQL3 = "Select rm.RoomNumber, coalesce(r.Description, '') as Description from t_Request r inner join t_ComboItems rs on r.StatusID = rs.ComboItemID inner join t_Room rm on r.KeyValue = rm.RoomID where r.KeyField = 'RoomID' and keyvalue = '" & rs.fields("roomID").value & "' and rs.ComboItem not in ('Cancelled','Complete') order by r.requestid"
                    rs2.Open(sSQL3, cn, 3, 3)
                    Dim att As String = ""
                    Do While Not rs2.EOF
                        If att = "" Then
                            att = rs2.Fields("Description").value
                        Else
                            att = att & "~" & rs2.Fields("Description").value
                        End If
                        rs2.MoveNext
                    Loop
                    rs2.Close
                    Lit.Text &= "<td align = center><a href='../../marketing/editreservation.aspx?reservationid=" & rs.fields("reservationid").value & "'>" & rs.fields("reservationid").value & "</a></td>"
                    If att = "" Then
                        Lit.Text &= "<td align = center spacing = 8>" & rs.fields("roomnumber").value & "</td>"
                    Else
                        Lit.Text &= "<td align = center spacing = 8><a href='#' class='roomNumber' data-desc=""" & att & """ data-room='" & rs.Fields("RoomNumber").value & "'>" & rs.fields("roomnumber").value & "</a></td>"
                    End If
                    Lit.Text &= "<td align = center spacing = 8>" & rs.fields("room type").value & "</td>"
                    Lit.Text &= "<td align = center spacing = 8>" & rs.fields("room status").value & "</td>"
                    Lit.Text &= "<td align = center spacing = 8>" & rs.fields("guest").value & "</td>"
                    Lit.Text &= "<td align = center spacing = 8>" & rs.fields("checkindate").value & "</td>"
                    Lit.Text &= "<td align = center spacing = 8>" & rs.fields("checkoutdate").value & "</td>"
                    Lit.Text &= "<td align = center spacing = 8>" & rs.fields("type").value & "</td>"
                    Lit.Text &= "<td align = center spacing = 8>" & rs.fields("subtype").value & "</td>"
                    Lit.Text &= "<td align = center spacing = 8>" & rs.fields("reservationstatus").value & "</td>"

                    '*** added by Request Susan Davis
                    Lit.Text &= "<td align = center spacing = 8>" & rs.fields("ReservationSource").value & "</td>"

                    '*** Query field addition WOID 15241
                    rsTour.open("Select tourdate from t_Tour where prospectid in (select prospectid from t_Reservations where reservationid = " & rs.fields("ReservationID").value & ") and tourdate is not null order by tourdate desc", cn, 0, 1)
                    If rsTour.eof And rsTour.bof Then
                        PreBooked = "NO"
                        LastTourDate = ""
                    Else
                        If CDate(rsTour.fields("TourDate").value) >= CDate(rs.fields("checkindate").value) Then
                            PreBooked = "YES"
                            LastTourDate = CDate(rsTour.fields("TourDate").value)
                        Else
                            PreBooked = "NO"
                            LastTourDate = CDate(rsTour.fields("TourDate").value)
                        End If
                    End If
                    rsTour.Close()
                    Lit.Text &= "<td align= center spacing = 8>" & LastTourDate & "</td>"
                    Lit.Text &= "<td align= center spacing = 8>" & PreBooked & "</td>"



                    '*** End field addition
                    Lit.Text &= "</tr>"
                    rs.movenext()
                Loop
                Lit.Text &= "<tr><td><input type='button' name='label' value='Preview Key Labels' onclick=window.open('../../label.asp?sdate=" & sdate & "&edate=" & edate & "');></td></tr>"
                Lit.Text &= "</TABLE>"

                sCount = rs.recordcount
                rs.close()
                Lit.Text &= sAns & "<br>" & "<b># of Check-Ins " & "<u>" & sCount & "</u></b>"
            End If
        ElseIf rblType.SelectedValue = "departures" Then

            sSQL2 = "SELECT distinct charindex('-',r.roomnumber),res.reservationid, r.RoomNumber,t.comboitem as 'Room Type', p.LastName + ', ' + p.FirstName as Guest, res.checkInDate, res.checkOutDate, rt.comboitem as 'Type', rst.comboitem as 'SubType', rs.comboitem as 'ReservationStatus'" &
             "FROM   t_Reservations res " &
             "left outer join t_comboitems rt on rt.comboitemid = res.typeid " &
             "left outer join t_comboitems rst on rst.comboitemid = res.subtypeid " &
             "left outer join t_prospect p on p.prospectid = res.prospectid " &
             "left outer join t_roomallocationmatrix ram on ram.reservationid = res.reservationid " &
             "left outer join t_room r on r.roomid = ram.roomid " &
             "left outer join t_comboitems t on t.comboitemid = res.typeid " &
             "left outer join t_comboitems rs on rs.comboitemid = res.statusid " &
             "where rs.comboitem = 'In-House' and res.checkoutdate between '" & stdate & "' and '" & endate & "' and res.reslocationid in (select comboitemid from t_comboitems where comboitem = 'KCP') "
            If cbRoom.Checked Then
                sSQL2 = sSQL2 & "order by charindex('-',r.roomnumber), r.roomnumber"
            Else
                sSQL2 = sSQL2 & "order by guest asc"
            End If
            rs.open(sSQL2, cn, 3, 3)

            sAns = "<table>"
            Lit.Text &= "<tr align = left><td><u><b>" & " Expected Check-Outs " & "</b></u></td></tr>"
            If rs.bof And rs.eof Then
                Lit.Text &= "<tr><td><b>No Records</b></td></tr></table>"
            Else
                Lit.Text &= "<TABLE>"
                Lit.Text &= "<tr align = center>"
                For i = 1 To rs.fields.count - 1
                    Lit.Text &= "<th>" & rs.fields(i).name & "</th>"
                Next
                Lit.Text &= "</tr>"

                Do While Not rs.eof
                    Lit.Text &= "<tr>"


                    Lit.Text &= "<td align = center><a href='../../marketing/editreservation.aspx?reservationid=" & rs.fields("reservationid").value & "'>" & rs.fields("reservationid").value & "</a></td>"
                    Lit.Text &= "<td align = center spacing = 10>" & rs.fields("roomnumber").value & "</td>"
                    Lit.Text &= "<td align = center spacing = 10>" & rs.fields("room type").value & "</td>"
                    Lit.Text &= "<td align = center spacing = 10>" & rs.fields("guest").value & "</td>"
                    Lit.Text &= "<td align = center spacing = 10>" & rs.fields("checkindate").value & "</td>"
                    Lit.Text &= "<td align = center spacing = 10>" & rs.fields("checkoutdate").value & "</td>"
                    Lit.Text &= "<td align = center spacing = 10>" & rs.fields("type").value & "</td>"
                    Lit.Text &= "<td align = center spacing = 10>" & rs.fields("subtype").value & "</td>"
                    Lit.Text &= "<td align = center spacing = 10>" & rs.fields("reservationstatus").value & "</td>"

                    Lit.Text &= "</tr>"
                    rs.movenext()
                Loop
                Lit.Text &= "</TABLE>"
            End If
            sCount = rs.recordcount
            rs.close()
            Lit.Text &= sAns & "<br>" & "<b> # of Departures " & "<u>" & sCount & "</u></b>"
            Lit.Text &= "<hr>"


            sSQL2 = "SELECT distinct charindex('-',r.roomnumber),res.reservationid, r.RoomNumber,t.comboitem as 'Room Type', p.LastName + ', ' + p.FirstName as Guest, res.checkInDate, res.checkOutDate, rt.comboitem as 'Type', rst.comboitem as 'SubType', rs.comboitem as 'ReservationStatus'" &
             "FROM   t_Reservations res " &
             "left outer join t_comboitems rt on rt.comboitemid = res.typeid " &
             "left outer join t_comboitems rst on rst.comboitemid = res.subtypeid " &
             "left outer join t_prospect p on p.prospectid = res.prospectid " &
             "left outer join t_roomallocationmatrix ram on ram.reservationid = res.reservationid " &
             "left outer join t_room r on r.roomid = ram.roomid " &
             "left outer join t_comboitems t on t.comboitemid = res.typeid " &
             "left outer join t_comboitems rs on rs.comboitemid = res.statusid " &
             "where rs.comboitem = 'Completed' and res.checkoutdate between '" & stdate & "' and '" & endate & "' and res.reslocationid in (select comboitemid from t_comboitems where comboitem = 'KCP') "
            If cbRoom.Checked Then
                sSQL2 = sSQL2 & "order by charindex('-',r.roomnumber), r.roomnumber"
            Else
                sSQL2 = sSQL2 & "order by guest asc"
            End If
            rs.open(sSQL2, cn, 3, 3)

            sAns = "<table>"
            Lit.Text &= "<tr><td align=left><u><b>" & " Already Checked Out " & "</b></u></td></tr>"
            If rs.bof And rs.eof Then
                Lit.Text &= "<tr><td align=left><b>No Records</b></td></tr></table>"
            Else
                Lit.Text &= "<TABLE>"
                Lit.Text &= "<tr align = center>"
                For i = 1 To rs.fields.count - 1
                    Lit.Text &= "<th>" & rs.fields(i).name & "</th>"
                Next

                Lit.Text &= "</tr>"

                Do While Not rs.eof
                    Lit.Text &= "<tr>"


                    Lit.Text &= "<td align = center><a href='../../marketing/editreservation.aspx?reservationid=" & rs.fields("reservationid").value & "'>" & rs.fields("reservationid").value & "</a></td>"
                    Lit.Text &= "<td align = center spacing = 10><a href='#' class='roomNumber'>" & rs.fields("roomnumber").value & "</a></td>"
                    Lit.Text &= "<td align = center spacing = 10>" & rs.fields("room type").value & "</td>"
                    Lit.Text &= "<td align = center spacing = 10>" & rs.fields("guest").value & "</td>"
                    Lit.Text &= "<td align = center spacing = 10>" & rs.fields("checkindate").value & "</td>"
                    Lit.Text &= "<td align = center spacing = 10>" & rs.fields("checkoutdate").value & "</td>"
                    Lit.Text &= "<td align = center spacing = 10>" & rs.fields("type").value & "</td>"
                    Lit.Text &= "<td align = center spacing = 10>" & rs.fields("subtype").value & "</td>"
                    Lit.Text &= "<td align = center spacing = 10>" & rs.fields("reservationstatus").value & "</td>"



                    Lit.Text &= "</tr>"
                    rs.movenext()
                Loop

                Lit.Text &= "</TABLE>"
            End If
            sCount = rs.recordcount
            rs.close()
            Lit.Text &= "<br>" & "<b> # Already Checked Out " & "<u>" & sCount & "</u></b>"
        Else
            Lit.Text &= "You Did Not Select a Function"
        End If
        cn.close()
        rs = Nothing
        rsTour = Nothing
        cn = Nothing


    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub


    Protected Sub btnPrintable_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintable.Click
        '        ClientScript.RegisterClientScriptBlock(Me.GetType, "Export", "var mwin = window.open();mwin.document.write('" & Replace(Lit.Text, "'", "\'") & "');", True)
        ClientScript.RegisterClientScriptBlock(Me.GetType, "printable", "var mwin=window.open();mwin.document.write('" & Replace(Lit.Text, "'", "\'") & "');", True)
    End Sub

    Protected Sub btnExcel_Click(sender As Object, e As System.EventArgs) Handles btnExcel.Click
        If Lit.Text <> "" Then
            Response.Clear()
            Response.ContentType = "application/vnd.ms-excel"
            Response.AddHeader("Content-Disposition", "attachment; filename=checkindepartures.xls")
            Response.Write(Lit.Text)
            Response.End()
        End If
    End Sub
End Class
