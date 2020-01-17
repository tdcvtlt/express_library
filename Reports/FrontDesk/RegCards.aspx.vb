
Partial Class Reports_FrontDesk_RegCards
    Inherits System.Web.UI.Page

    Protected Sub btnRun_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRun.Click
        Dim cn As Object
        Dim rs As Object
        Dim rs1 As Object
        Dim rs2 As Object
        Dim sResID As String
        Dim sRoom As String = ""
        Dim sTable As String = ""
        Dim sAns As String = ""


        cn = Server.CreateObject("ADODB.connection")
        rs = Server.CreateObject("ADODB.recordset")
        rs1 = Server.CreateObject("ADODB.recordset")
        rs2 = Server.CreateObject("ADODB.recordset")

        cn.open(Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
        rs.open("SELECT * FROM (SELECT DISTINCT(r.ReservationID), p.LastName + ',' + ' ' + p.FirstName as ProspectName, pa.Address1, pa.City, c.ComboItem as State, pa.PostalCode, r.CheckInDate, r.CheckOutDate, rest.ComboItem as ResType, resst.ComboItem as ResSubType " & _
              "FROM t_Prospect p " & _
              "LEFT OUTER JOIN T_PROSPECTADDRESS PA ON PA.PROSPECTID = P.PROSPECTID " & _
              "LEFT OUTER JOIN t_comboitems c on c.comboitemid = pA.stateID " & _
              "INNER JOIN t_Reservations r on r.ProspectID = p.ProspectID " & _
              "INNER JOIN t_ComboItems rest on rest.ComboItemID = r.TypeID " & _
              "LEFT OUTER JOIN t_ComboItems resst on resst.ComboItemID = r.SubTypeID " & _
              "INNER JOIN t_ComboItems rl on rl.ComboItemID = r.ResLocationID " & _
              "INNER JOIN t_ComboItems st on st.ComboItemID = r.StatusID " & _
              "WHERE rl.ComboItem = 'KCP' and st.ComboItem = 'Booked' AND (pa.prospectid is null or pa.activeflag = 1) and r.CheckInDate = '" & dfCheckIn.Selected_Date & "') a " & _
              "ORDER BY Prospectname asc", cn, 0, 1)
        If rs.eof And rs.bof Then
            sAns = ("<font color=" & Chr(34) & "red" & Chr(34) & ">There are no Check-Ins today</font>")
        Else
            sAns = "<div id='printable'>"
            Do While Not rs.eof

                sResID = rs.fields("ReservationID").value

                rs2.open("SELECT DISTINCT(r.RoomNumber) FROM t_Room r INNER JOIN t_RoomAllocationMatrix a on a.RoomID = r.RoomID WHERE a.ReservationID = '" & sResID & "'", cn, 3, 3)

                sTable = "<table>"
                Do While Not rs2.eof

                    If Left(rs2.fields("RoomNumber").value, 2) = "1-" Then
                        sRoom = Right(rs2.fields("RoomNumber").value, 4) & " Petersburg Circle"
                    ElseIf Left(rs2.fields("RoomNumber").value, 2) = "2-" Then
                        sRoom = Right(rs2.fields("RoomNumber").value, 4) & " Marsh Tacky"
                    ElseIf Left(rs2.fields("RoomNumber").value, 2) = "3-" Then
                        sRoom = Right(rs2.fields("RoomNumber").value, 4) & " Pocotaligo Lane"
                    ElseIf Left(rs2.fields("RoomNumber").value, 2) = "4-" Then
                        sRoom = Right(rs2.fields("RoomNumber").value, 4) & " Shipyard Drive"
                    ElseIf Left(rs2.fields("RoomNumber").value, 2) = "5-" Then
                        sRoom = Right(rs2.fields("RoomNumber").value, 4) & " Ironhinge Road"
                    ElseIf Left(rs2.fields("RoomNumber").value, 2) = "6-" Then
                        sRoom = Right(rs2.fields("RoomNumber").value, 4) & " Charlotte Circle"
                    ElseIf Left(rs2.fields("RoomNumber").value, 2) = "7-" Then
                        sRoom = Right(rs2.fields("RoomNumber").value, 4) & " Duckblind Way"
                    ElseIf Left(rs2.fields("RoomNumber").value, 2) = "8-" Then
                        sRoom = Right(rs2.fields("RoomNumber").value, 4) & " Squirrel Landing"
                    ElseIf Left(rs2.fields("RoomNumber").value, 2) = "9-" Then
                        sRoom = Right(rs2.fields("RoomNumber").value, 4) & " Turtle Trace"
                    ElseIf Left(rs2.fields("RoomNumber").value, 2) = "10" Then
                        sRoom = Right(rs2.fields("RoomNumber").value, 4) & " Jasmine Crescent"
                    ElseIf Left(rs2.fields("RoomNumber").value, 2) = "11" Then
                        sRoom = Right(rs2.fields("RoomNumber").value, 4) & " Begonia Way"
                    ElseIf Left(rs2.fields("RoomNumber").value, 2) = "12" Then
                        sRoom = Right(rs2.fields("RoomNumber").value, 4) & " Aster Lane"
                    ElseIf Left(rs2.fields("RoomNumber").value, 2) = "13" Then
                        sRoom = Right(rs2.fields("RoomNumber").value, 4) & " Sunflower Court"
                    ElseIf Left(rs2.fields("RoomNumber").value, 2) = "14" Then
                        sRoom = Right(rs2.fields("RoomNumber").value, 4) & " Derwent Way"
                    ElseIf Left(rs2.fields("RoomNumber").value, 2) = "15" Then
                        sRoom = Right(rs2.fields("RoomNumber").value, 4) & " Dakota Drive"
                    ElseIf Left(rs2.Fields("RoomNumber").value, 2) = "16" Then
                        sRoom = Right(rs2.Fields("RoomNumber").value, 4) & " Elijah Way"
                    ElseIf Left(rs2.Fields("RoomNumber").value, 2) = "17" Then
                        sRoom = Right(rs2.Fields("RoomNumber").value, 4) & " Emmanus Court"
                    ElseIf Left(rs2.Fields("RoomNumber").value, 2) = "18" Then
                        sRoom = Right(rs2.Fields("RoomNumber").value, 4) & " Carousel Court"
                    Else
                        sRoom = "Street Unknown"
                    End If

                    sTable = sTable & "<tr>"
                    sTable = sTable & "<td><font size = '2'>" & sRoom & "</font></td>"
                    sTable = sTable & "</td>"
                    rs2.movenext()
                Loop
                sTable = sTable & "</table>"

                rs2.close()

                sAns &= "<center><img src='../../images/kcp_logo.bmp' width='342' height='41'></center>"
                sAns &= "<br>"
                sAns &= "<div style='float:left;'>"
                sAns &= "<table>"
                sAns &= "<tr>"
                sAns &= "<td>"
                sAns &= "<font size='2'>"
                sAns &= "<b>Reservation #:" & rs.fields("ReservationID").value & "<br></b>"
                sAns &= rs.fields("ProspectName").value & "<br>"
                sAns &= rs.fields("Address1").value & "<br>"
                sAns &= rs.fields("City").value & ",&nbsp;" & rs.fields("state").value & "&nbsp;" & rs.fields("PostalCode").value
                sAns &= "</font>"
                sAns &= "</td>"
                sAns &= "</tr>"
                sAns &= "</table>"
                sAns &= "</div>"
                sAns &= "<div style='float:right;'>"
                sAns &= "<table>"
                sAns &= "<tr>"
                sAns &= "<td colspan='2'><font size='2'><b>Res Type/Sub-Type:</b>&nbsp;" & rs.fields("ResType").value & "/" & rs.fields("ResSubType").value & "</font></td>"
                sAns &= "</tr>"
                sAns &= "<tr>"
                sAns &= "<td><font size='2'><b>Arrival Date:</b></font></td>"
                sAns &= "<td><font size='2'>" & rs.fields("CheckInDate").value & "</font></td>"
                sAns &= "</tr>"
                sAns &= "<tr>"
                sAns &= "<td><font size='2'><b>Departure Date:</b></font></td>"
                sAns &= "<td><font size='2'>" & rs.fields("CheckOutDate").value & "</font></td>"
                sAns &= "</tr>"
                sAns &= "<tr>"
                sAns &= "<td colspan='3'><font size='2'><b>Room(s):</b></font>" & sTable & "</td>"
                sAns &= "</tr>"
                sAns &= "</table>"
                sAns &= "</div>"

                sAns &= "<br><br><br><br>"
                sAns &= "<h2 align='center'><u>Visitor Registration Card</u></h2>"
                sAns &= "</center>"
                sAns &= "<table>"
                sAns &= "<tr>"
                sAns &= "<td>_________</td>"
                sAns &= "<td colspan='50'><b><font size='2'>I AGREE THAT MY LIABILITY FOR THIS BILL IS NOT WAIVED AND AGREE TO BE HELD PERSONALLY LIABLE IN THE EVENT THAT THE INDICATED PERSON, COMPANY OR ASSOCIATION FAILS TO PAY THE FULL AMOUNT OF THESE CHARGES</font></b><br><br></td>"
                sAns &= "</tr>"
                sAns &= "<tr>"
                sAns &= "<td>_________</td>"
                sAns &= "<td><b><font size='2'>I UNDERSTAND THAT NON-OWNER AND NON-EXCHANGE WEEK VISITORS ARE REQUIRED TO PAY TAXES ON THE ROOM FOR THEIR VISIT. SHOULD THE TAXES NOT BE PAID AT THE TIME OF INITIAL RESERVATION, THE TAX WILL BE DUE AT CHECK OUT. SHOULD I NOT PRESENT MYSELF AT CHECK OUT, I UNDERSTAND AND AGREE THAT THE CREDIT CARD PRESENTED AT CHECK IN WILL BE CHARGED FOR ALL AMOUNTS DUE.</font></b><br><br></td>"
                sAns &= "</tr>"
                sAns &= "<tr>"
                sAns &= "<td height='95'>_________</td>"
                sAns &= "<td height='95'><b><font size='2'>I UNDERSTAND THAT THE FOLLOWING PHONE CHARGES APPLY:<br>DIRECT DIALED LONG DISTANCE - A CONNECTION FEE OF $1.50 AND EACH ADDITIONAL MINUTE WILL BE "
                sAns &= "$0.25 CHARGE.<br>LOCAL CALLS - THERE WILL BE A $0.35 CHARGE (UNLIMITED TALK TIME)<br>INTERNATIONAL CALLS - A CONNECTION FEE OF $2.50 AND EACH ADDITIONAL MINUTE WILL BE "
                sAns &= "$0.50.<br>INFORMATIONAL CALLS - THERE WILL BE A $0.75 CHARGE.<br>CALLING CARD/CREDIT CARD/800 CALLS (800,888,877) - WILL BE NO CHARGE</font></b><br><br></td>"
                sAns &= "</tr>"

                sAns &= "<tr>"
                sAns &= "<td>_________</td>"
                sAns &= "<td><b><font size='2'>I UNDERSTAND THAT A LATE CHECK OUT FEE OF $50.00 WILL BE CHARGED TO MY ACCOUNT FOR EACH ADDITIONAL HOUR AFTER THE ESTABLISHED CHECK OUT TIME OF 10:00AM.</font></b><br><br></td>"
                sAns &= "</tr>"

                sAns &= "<tr>"
                sAns &= "<td>_________</td>"
                sAns &= "<td><b><font size='2'>I UNDERSTAND THAT PETS ARE NOT PERMITTED ON THE PROPERTY OF KING'S CREEK PLANTATIONS. VIOLATION OF THIS POLICY WILL BE A MINIMUM CHARGE OF $250.00. I WILL BE REQUIRED TO REMOVE THE PET FROM THE PROPERTY IMMEDIATELY OR I AGREE TO CHECK OUT OF MY UNIT.</font></b><br><br></td>"
                sAns &= "</tr>"
                sAns &= "<tr>"
                sAns &= "<td>_________</td>"
                sAns &= "<td><b><font size='2'>I UNDERSTAND THAT KING'S CREEK IS NOT RESPONSIBLE FOR ANY ARTICLES LEFT BEHIND DURING OUR STAY HERE AT THE RESORT.</font></b><br><br></td>"
                sAns &= "</tr>"
                sAns &= "<tr>"
                sAns &= "<td>_________</td>"
                sAns &= "<td><b><font size='2'>I UNDERSTAND THAT KING'S CREEK PLANTATION IS NOT RESPONSIBLE FOR INCLEMENT WEATHER, ACTS OF GOD, POWER OUTAGES, ETC.</font></b><br><br></td>"
                sAns &= "</tr>"

                sAns &= "<tr> "
                sAns &= "<td height='25'>_________</td>"
                sAns &= "<td height='25'><b><font size='2'>I UNDERSTAND ALL OF KING'S CREEK PLANTATION UNITS ARE NON-SMOKING. IF I AM FOUND IN VIOLATION OF THIS POLICY, A MINIMUM CHARGE OF $250.00 WILL BE ASSESSED TO MY ACCOUNT AND I AGREE TO CHECK OUT IMMEDIATELY.</font></b><br><br></td>"
                sAns &= "</tr>"

                sAns &= "<tr>"
                sAns &= "<td>_________</td>"
                sAns &= "<td><b><font size='2'>I UNDERSTAND CHARGES WILL APPLY FOR ITEMS MISSING OR DAMAGED AT THE END OF MY STAY.</font></b></td>"
                sAns &= "</tr>"

                sAns &= "<tr>"
                sAns &= "<td>_________</td>"
                sAns &= "<td><b><font size='2'>KING'S CREEK PLANTATION PROVIDES FREE ACCESS TO AN UNSECURED WIRELESS NETWORK IN EACH UNIT. THIS FREE SERVICE IS AN OPEN NETWORK PROVIDED FOR YOUR CONVENIENCE AND ITS USE IS AT YOUR OWN RISK. THE SERVICE IS PROVIDED ON AN AS AVAILABLE BASIS WITHOUT WARRANTIES OF ANY KIND. NO ADVICE OR INFORMATION GIVEN BY KING'S CREEK PLANTATION, PROVIDERS, AFFILIATES, OR CONTRACTORS OF THE SERVICE OR THEIR RESPECTIVE EMPLOYEES SHALL CREATE SUCH A WARRANTY.</font></b><br><br></td>"
                sAns &= "</tr>"

                sAns &= "<tr>"
                sAns &= "<td>________</td>"
                sAns &= "<td><b><font size='2'>IN THE EVENT I SECURE TEE TIMES THROUGH THE CONCIERGE FOR WILLIAMSBURG NATIONAL GOLF COURSE, I AM AWARE OF THE 24 HOUR CANCELLATION POLICY AND THAT I WILL BE CHARGED A NO SHOW FEE IF TEE TIME IS NOT CANCELLED WITHIN THE ALLOTTED TIME.</font></b></td>"
                sAns &= "</tr>"

                sAns &= "<tr>"
                sAns &= "<td></td>"
                sAns &= "<td>"
                sAns &= "<table>"
                sAns &= "<tr>"
                sAns &= "<td><b><font size='2'>Smoke Damage - $500 (minimum)</font></b></td>"
                sAns &= "<td><b><font size='2'>Carpet Damage - $250 (minimum)</font></b></td>"
                sAns &= "<td><b><font size='2'>Paint Damage - $250 (minimum)</font></b></td>"
                sAns &= "</tr>"
                sAns &= "<tr>"
                sAns &= "<td><b><font size='2'>Furniture Damage - $250 (minimum)</font></b></td>"
                sAns &= "<td><b><font size='2'>Damaged Appliances - $250 (minimum)</font></b></td>"
                sAns &= "</tr>"
                sAns &= "</table>"
                sAns &= "</td>"
                sAns &= "</tr>"
                sAns &= "<tr>"
                sAns &= "<td></td>"
                sAns &= "<td><b><font size='2'>Prices may increase due to extent of damages (to be determined by management) plus an additional 25% service charge.</font></b></td>"
                sAns &= "</tr>"
                sAns &= "</table>"
                sAns &= "<table height='68' style='page-break-after:always'>"
                sAns &= "<tr>"
                sAns &= "<td colspan = '2'><font size = '2'>By signing below, I hereby authorize King’s Creek Plantation, L.L.C., its affiliates, vendors, subsidiaries and successors to contact me by auto-dialed telemarketing calls, SMS messages, texts and\or e-mails to my phone number(s) and email address(es) regarding their travel products and services. I understand such consent is not condition of any purchase.</font></td>"
                sAns &= "</tr>"
                sAns &= "<tr>"
                sAns &= "<td colspan='2' height='31'><font size = '2'>Email Address</font> _________________________________________________________</td>"
                sAns &= "</tr>"
                sAns &= "<tr>"
                sAns &= "<td colspan='2' ><font size = '2'>Signature </font>____________________________________________________________</td>"
                sAns &= "</tr>"
                sAns &= "</table>"
                rs.movenext()
            Loop
            sAns &= "</div>"
        End If
        Lit.Text = sAns
        rs.close()
        cn.close()

        rs2 = Nothing
        rs = Nothing
        cn = Nothing

    End Sub
End Class
