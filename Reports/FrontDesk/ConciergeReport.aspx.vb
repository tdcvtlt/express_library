
Partial Class Reports_FrontDesk_ConciergeReport
    Inherits System.Web.UI.Page

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        Lit.Text = ""
        Run_Report()
    End Sub

    Private Sub Run_Report()
        Dim cn As Object
        Dim rs As Object
        Dim rsC As Object
        Dim rsT As Object
        Dim sUnit As String
        Dim sPhone As String
        Dim tDate As String

        cn = Server.CreateObject("ADODB.Connection")
        rs = Server.CreateObject("ADODB.Recordset")
        rsC = Server.CreateObject("ADODB.Recordset")
        rsT = Server.CreateObject("ADODB.Recordset")

        cn.open(Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)

        rs.open("select p.Lastname + ', ' + p.firstname as Guest, p.ProspectID, st.comboitem as SType, r.checkindate, r.checkoutdate, r.reservationid, rs.comboitem as Source, t.comboitem as rType, t.comboitem as eType from t_reservations r inner join t_Prospect p on p.prospectid = r.prospectid inner join t_comboitems t on t.comboitemid = r.typeid left outer join t_Comboitems st on st.comboitemid = r.subtypeid left outer join t_comboitems rs on rs.comboitemid = r.sourceid where r.checkindate between '" & SDate.Selected_Date & "' and '" & EDate.Selected_Date & "' and r.statusid in (select comboitemid from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid where comboname = 'reservationstatus' and comboitem in ('Booked', 'In-House')) order by p.lastname asc ", cn, 3, 3)

        If rs.eof And rs.bof Then
            Lit.Text &= ("No CheckIns for this Date Range")
        Else
            'With Response
            Lit.Text &= ("<table>")
            Lit.Text &= ("<tr>")
            Lit.Text &= ("<td><b>Guest Name</b></td>")
            Lit.Text &= ("<td><b>Owner (Y/N)</b></td>")
            Lit.Text &= ("<td><b>Reservation ID</b></td>")
            Lit.Text &= ("<td><b>D-Paper/Resort Finance</b></td>")
            Lit.Text &= ("<td><b>Source</b></td>")
            Lit.Text &= ("<td><b>Type</b></td>")
            Lit.Text &= ("<td><b>Sub-Type</b></td>")
            Lit.Text &= ("<td><b>Phone</b></td>")
            Lit.Text &= ("<td><b>Unit</b></td>")
            Lit.Text &= ("<td><b>CheckIn</b></td>")
            Lit.Text &= ("<td><b>CheckOut</b></td>")
            Lit.Text &= ("<td><b>Last Tour</b></td>")
            Lit.Text &= ("<td><b>Phone</b></td>")            
            Lit.Text &= ("<td><b>Address</b></td>")
            Lit.Text &= ("<td colspan='2'><b>Book</b></td>")
            Lit.Text &= ("<td><b>Comments</b></td>")
            Lit.Text &= ("</tr>")

            Do While Not rs.eof
                Lit.Text &= ("<tr>")
                Lit.Text &= ("<td>" & rs.fields("Guest").Value & "</td>")
                rsT.open("Select c.ContractNumber, cs.ComboItem as Status from t_Contract c left outer join t_ComboItems cs on c.statusid = cs.comboitemid where c.contractid in (select contractid from t_Soldinventory) and c.prospectid = '" & rs.Fields("ProspectID").value & "'", cn, 3, 3)
                If rsT.EOF And rsT.BOF Then
                    Lit.Text &= ("<td>N</td>")
                Else
                    Lit.Text &= ("<td>Y</td>")
                End If
                rsT.Close()
                Lit.Text &= ("<td>" & rs.fields("ReservationID").Value & "</td>")
                rsT.open("Select c.ContractNumber, cs.ComboItem as Status from t_Contract c left outer join t_ComboItems cs on c.statusid = cs.comboitemid where (cs.comboitem = 'Developer' or c.contractnumber like '%X%') and c.prospectid = '" & rs.Fields("ProspectID").value & "'", cn, 3, 3)
                If rsT.EOF And rsT.BOF Then
                    Lit.Text &= ("<td></td>")
                Else
                    If InStr(rsT.Fields("ContractNumber"), "X") > 0 Then
                        Lit.Text &= ("<td>Resort Finance</td>")
                    Else
                        If rsT.Fields("Status") & "" = "Developer" Then
                            Lit.Text &= ("<td>D-Paper</td>")
                        Else
                            Lit.Text &= ("<td></td>")
                        End If
                    End If
                End If
                rsT.Close()
                Lit.Text &= ("<td>" & rs.fields("Source").Value & "</td>")
                Lit.Text &= ("<td>" & rs.fields("rType").Value & "</td>")
                Lit.Text &= ("<td>" & rs.fields("sType").Value & "</td>")
                rsT.open("Select * from t_Room where roomid in (select roomid from t_Roomallocationmatrix where reservationid = '" & rs.fields("ReservationID").value & "')", cn, 0, 1)
                If rsT.eof And rsT.bof Then
                    sUnit = ""
                    sPhone = ""
                Else
                    sUnit = rsT.fields("RoomNumber").value & ""
                    sPhone = rsT.fields("Phone").value & ""
                End If
                rsT.close()
                Lit.Text &= ("<td>" & sPhone & "</td>")
                Lit.Text &= ("<td>" & sUnit & "</td>")
                Lit.Text &= ("<td>" & rs.fields("CheckInDate").Value & "</td>")
                Lit.Text &= ("<td>" & rs.fields("CheckOutDate").Value & "</td>")




                rsT.open("select top 1 TourDate from t_tour where prospectid in (select prospectid from t_reservations where reservationid = '" & rs.fields("ReservationID").value & "') order by tourdate desc", cn, 3, 3)
                If rsT.eof And rsT.bof Then
                    tDate = "<N/A>"
                Else
                    If rsT.fields("TourDate").value & "" <> "" Then
                        tDate = FormatDateTime(rsT.fields("TourDate").value, DateFormat.ShortDate)
                    Else
                        tDate = ""
                    End If
                End If
                rsT.close()
                Lit.Text &= ("<td>" & tDate & "</td>")

                Dim sGPhone As String = ""
                Dim sEmail As String = ""
                Dim sAddress As String = ""

                rsT.open("select * from t_ProspectPhone where prospectid in (select prospectid from t_reservations where reservationid = '" & rs.fields("ReservationID").value & "') and active = 1", cn, 3, 3)
                If rsT.eof And rsT.bof Then
                    sGPhone = "<N/A>"
                Else
                    Do While Not rsT.eof
                        sGPhone &= IIf(sPhone = "", rsT.fields("Number").value & "", ", " & rsT.fields("Number").value)
                        rsT.movenext()
                    Loop
                End If
                rsT.close()
                Lit.Text &= ("<td>" & sGPhone & "</td>")

                rsT.open("select * from t_ProspectEmail where prospectid in (select prospectid from t_reservations where reservationid = '" & rs.fields("ReservationID").value & "') and isactive = 1", cn, 3, 3)
                If rsT.eof And rsT.bof Then
                    sEmail = "<N/A>"
                Else
                    Do While Not rsT.eof
                        sEmail &= IIf(sPhone = "", rsT.fields("Email").value & "", ", " & rsT.fields("Email").value)
                        rsT.movenext()
                    Loop
                End If
                rsT.close()
                Lit.Text &= ("<td>" & sEmail & "</td>")


                Dim res_id = rs.fields("ReservationID").value.ToString




                Lit.Text &= "<td><input type='button' name='book' value='BOOK' onclick=""javascript:modal.mwindow.open('../../general/addtour.aspx?ReservationID=" & rs.fields("ReservationID").value & "','win01',700,700);""></td>"                
                Lit.Text &= "<td><input type='button' name='book' value='Extra Tour' onclick='Book_Tour2('" & rs.fields("reservationid").value & "');'></td>"

                rsC.open("select * from t_Comments where KeyField = 'Reservation' and KeyValue = '" & rs.fields("reservationid").value & "' ", cn, 3, 3)
                If Not (rsC.eof) Then

                    Lit.Text &= "<td><input type=button value='View' onclick='View_Note('" & rs.fields("reservationid").value & "');'> </td>"

                Else

                    Lit.Text &= "<td><input type=button value='ADD' onclick=""javascript:modal.mwindow.open('../../general/editcomment.aspx?commentid=0&KeyField=ReservationID&keyvalue=" & rs.fields("ReservationID").value & "&linkid=','win01',350,350);""> </td>"

                End If
                rsC.close()
                Lit.Text &= ("</tr>")
                rs.movenext()
            Loop
            rs.close()
            Lit.Text &= ("</table>")
            'End With
        End If
        cn = Nothing
        rs = Nothing
        rsC = Nothing
        rsT = Nothing
        sUnit = Nothing
        sPhone = Nothing
        tDate = Nothing

    End Sub
End Class
