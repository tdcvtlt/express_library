
Partial Class Reports_Reservations_InventoryControl
    Inherits System.Web.UI.Page

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        Dim cn As Object
        Dim rs As Object
        Dim rsO As Object
        Dim edate As String = Me.dteEDate.Selected_Date
        Dim sdate As String = Me.dteSDate.Selected_Date
        Dim mType As String = Me.siResType.Selected_ID
        Dim sql As String = ""
        Dim sAns As String = ""
        Dim lastroom As String = ""
        Dim lastUsage As Integer = 0
        Dim lastIndate As Date
        Dim nights As Integer = 0
        Dim lastCategory As String = ""
        Dim lastusageid As Integer = 0
        Dim lastSize As String = ""

        cn = Server.CreateObject("ADODB.Connection")
        rs = Server.CreateObject("ADODB.Recordset")
        cn.open(Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
        cn.CommandTimeout = 0
        sql = "select u.usageid,m.*,r.roomnumber + ' ' + rst.comboitem as Roomnumber,rt.comboitem as Size,c.ComboItem as Category from t_Usage u  " & _
              "inner join t_Comboitems ut on ut.comboitemid = u.typeid " & _
              "inner join t_roomallocationmatrix m on m.usageid = u.usageid " & _
              "inner join t_Room r on r.roomid = m.roomid " & _
              "left outer join t_ComboItems c on u.categoryid = c.comboitemid " & _
              "left outer join t_Comboitems rst on rst.comboitemid = r.subtypeid " & _
              "inner join t_Comboitems rt on rt.comboitemid = r.typeid " & _
              "where ut.comboitemid in ('" & mType & "') " & _
              "and u.indate between '" & sdate & "' and '" & edate & "' " & _
              "and (RESERVATIONID IS NULL OR reservationid = 0) order by charindex('-',roomnumber), roomnumber,u.usageid,m.dateallocated"
        rs.open(sql, cn, 0, 1)
        If rs.eof And rs.bof Then
            sAns = "No Records"
        Else
            lastroom = ""
            lastUsage = 0
            lastIndate = System.DateTime.Now.ToShortDateString
            lastsize = ""
            nights = 0
            rsO = Server.CreateObject("ADODB.Recordset")
            rsO.fields.append("RoomNumber", 200, 255)
            rsO.fields.append("RoomSize", 200, 255)
            rsO.fields.append("Nights", 200, 255)
            rsO.fields.append("InDate", 7)
            rsO.fields.append("OutDate", 7)
            rsO.fields.append("Category", 200, 255)
            rsO.fields.append("Usageid", 200, 255)
            rsO.open()
            sAns = "<table>"
            sAns = sAns & "<tr><th>Room Number</th><th>Room Size</th><th># Nights</th><th>In Date</th><th>Out Date</th><th>Category</th></tr>"
            Do While Not rs.eof
                If lastroom <> rs.fields("RoomNumber").value & "" Or lastUsage <> rs.fields("UsageID").value Or lastIndate.AddDays(nights) <> rs.fields("dateallocated").value Or nights = 7 Then
                    If lastroom <> "" Then
                        rsO.addnew()
                        rsO.fields("RoomNumber").value = lastroom
                        rsO.fields("RoomSize").value = lastsize
                        rsO.fields("Nights").value = nights
                        rsO.fields("Indate").value = lastIndate
                        rsO.fields("OutDate").value = lastIndate.AddDays(nights)
                        rsO.fields("Category").value = lastCategory
                        rsO.fields("UsageID").value = lastusageid
                        rsO.update()
                    End If
                    lastroom = rs.fields("Roomnumber").value & ""
                    lastUsage = rs.fields("UsageID").value
                    lastIndate = rs.fields("dateallocated").value
                    lastsize = rs.fields("Size").value & ""
                    lastCategory = rs.FIelds("Category").value & ""
                    lastusageid = rs.Fields("UsageID").value & ""
                    nights = 0
                End If
                nights = nights + 1
                rs.movenext()
            Loop
            rsO.addnew()
            rsO.fields("RoomNumber").value = lastroom
            rsO.fields("RoomSize").value = lastsize
            rsO.fields("Nights").value = nights
            rsO.fields("Indate").value = lastIndate
            rsO.fields("OutDate").value = lastIndate.AddDays(nights)
            rsO.fields("Category").value = lastCategory
            rsO.fields("UsageID").value = lastusageid
            rsO.update()
            rsO.sort = "Indate "
            If rsO.eof And rsO.bof Then
                sAns = sAns & "No RECORDS"
            Else
                rsO.movefirst()
                Do While Not rsO.eof
                    If rsO.fields("Nights").value > 1 Then
                        sAns = sAns & "<tr><td>" & rsO.fields("RoomNumber").value & "</td><td>" & rsO.fields("Roomsize").value & "</td><td>" & rsO.fields("Nights").value & "</td><td align=right>" & rsO.fields("indate").value & "</td><td align = right>" & rsO.fields("OutDate").value & "</td><td align = 'center'>" & rsO.fields("Category").value & "</td></tr>"
                    End If
                    rsO.movenext()
                Loop
            End If
            rsO.close()
            rsO = Nothing
        End If
        sAns = sAns & "</table>"
        rs.close()
        cn.close()
        rs = Nothing
        cn = Nothing
        litReport.Text = sAns
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            siResType.Connection_String = Resources.Resource.cns
            siResType.ComboItem = "ReservationType"
            siResType.Label_Caption = ""
            siResType.Load_Items()
        End If
    End Sub
End Class
