Imports System.Data.Sql
Imports System.Data.SqlClient

Partial Class frontdesk_ResortOverview
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Set_Lookups()
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
        Dim sql As String = "Select r.roomid,count(distinct req.requestid) as Requests, r.roomnumber,rs.comboitem as status,rms.comboitem as maintstatus, rhs.comboitem as hkstatus, ut.comboitem as ut, ust.comboitem as ust from t_Room r  left outer join t_Comboitems rs on rs.comboitemid = r.statusid inner join t_Unit u on u.unitid = r.unitid inner join t_Comboitems ut on ut.comboitemid = u.typeid inner join t_comboitems ust on ust.comboitemid = u.subtypeid left outer join t_Comboitems rms on rms.comboitemid = r.maintenancestatusid left outer join t_Comboitems rhs on rhs.comboitemid = r.housekeepingstatusid left outer join (select * from t_Request where statusid not in (select comboitemid from t_Comboitems inner join t_Combos on t_Combos.comboid = t_Comboitems.comboid where comboname = 'workorderstatus' and comboitem in ('complete', 'completed','canceled','cancelled'))) req on req.roomid = r.roomid group by r.roomnumber,rs.comboitem,rms.comboitem , rhs.comboitem, ut.comboitem, ust.comboitem, r.roomid order by CharIndex('-',roomnumber), r.roomnumber asc, ut.comboitem"
        If siRoomType.Selected_ID > 0 And siRoomSubType.Selected_ID > 0 Then
            sql = "Select r.roomid,count(distinct req.requestid) as Requests, r.roomnumber,rs.comboitem as status,rms.comboitem as maintstatus, rhs.comboitem as hkstatus, ut.comboitem as ut, ust.comboitem as ust from t_Room r  left outer join t_Comboitems rs on rs.comboitemid = r.statusid inner join t_Unit u on u.unitid = r.unitid inner join t_Comboitems ut on ut.comboitemid = u.typeid inner join t_comboitems ust on ust.comboitemid = u.subtypeid left outer join t_Comboitems rms on rms.comboitemid = r.maintenancestatusid left outer join t_Comboitems rhs on rhs.comboitemid = r.housekeepingstatusid left outer join (select * from t_Request where statusid not in (select comboitemid from t_Comboitems inner join t_Combos on t_Combos.comboid = t_Comboitems.comboid where comboname = 'workorderstatus' and comboitem in ('complete', 'completed','canceled','cancelled'))) req on req.roomid = r.roomid where ust.comboitemid in (" & siRoomSubType.Selected_ID & ") and ut.comboitemid in (" & siRoomType.Selected_ID & ") group by r.roomnumber,rs.comboitem,rms.comboitem , rhs.comboitem, ut.comboitem, ust.comboitem, r.roomid order by CharIndex('-',roomnumber), r.roomnumber asc, ut.comboitem"
        ElseIf siRoomSubType.Selected_ID > 0 Then
            sql = "Select r.roomid,count(distinct req.requestid) as Requests, r.roomnumber,rs.comboitem as status,rms.comboitem as maintstatus, rhs.comboitem as hkstatus, ut.comboitem as ut, ust.comboitem as ust from t_Room r  left outer join t_Comboitems rs on rs.comboitemid = r.statusid inner join t_Unit u on u.unitid = r.unitid inner join t_Comboitems ut on ut.comboitemid = u.typeid inner join t_comboitems ust on ust.comboitemid = u.subtypeid left outer join t_Comboitems rms on rms.comboitemid = r.maintenancestatusid left outer join t_Comboitems rhs on rhs.comboitemid = r.housekeepingstatusid left outer join (select * from t_Request where statusid not in (select comboitemid from t_Comboitems inner join t_Combos on t_Combos.comboid = t_Comboitems.comboid where comboname = 'workorderstatus' and comboitem in ('complete', 'completed','canceled','cancelled'))) req on req.roomid = r.roomid where ust.comboitemid in (" & siRoomSubType.Selected_ID & ") group by r.roomnumber,rs.comboitem,rms.comboitem , rhs.comboitem, ut.comboitem, ust.comboitem, r.roomid order by CharIndex('-',roomnumber), r.roomnumber asc, ut.comboitem"
        ElseIf siRoomType.Selected_ID > 0 Then
            sql = "Select r.roomid,count(distinct req.requestid) as Requests, r.roomnumber,rs.comboitem as status,rms.comboitem as maintstatus, rhs.comboitem as hkstatus, ut.comboitem as ut, ust.comboitem as ust from t_Room r  left outer join t_Comboitems rs on rs.comboitemid = r.statusid inner join t_Unit u on u.unitid = r.unitid inner join t_Comboitems ut on ut.comboitemid = u.typeid inner join t_comboitems ust on ust.comboitemid = u.subtypeid left outer join t_Comboitems rms on rms.comboitemid = r.maintenancestatusid left outer join t_Comboitems rhs on rhs.comboitemid = r.housekeepingstatusid left outer join (select * from t_Request where statusid not in (select comboitemid from t_Comboitems inner join t_Combos on t_Combos.comboid = t_Comboitems.comboid where comboname = 'workorderstatus' and comboitem in ('complete', 'completed','canceled','cancelled'))) req on req.roomid = r.roomid where ut.comboitemid in (" & siRoomType.Selected_ID & ") group by r.roomnumber,rs.comboitem,rms.comboitem , rhs.comboitem, ut.comboitem, ust.comboitem, r.roomid order by CharIndex('-',roomnumber), r.roomnumber asc, ut.comboitem"
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
        lit1.Text &= ("<table>")
        If Not rs.HasRows Then

            lit1.Text &= ("<tr><td>NO RECORDS</td></tr>")
        Else
            Dim colcounter As Integer = 0

            Do While rs.Read
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
                'HouseKeeping Status Color
                Select Case Trim(rs("HKStatus") & "")
                    Case "Cleaned"
                        hksc = "#00FF00;"
                    Case "Occupied"
                        hksc = "#FFFF00;"
                    Case "Offline"
                        hksc = "#FF0000;"
                    Case "Dirty"
                        hksc = "#00FFFF;"
                    Case "Being Cleaned"
                        hksc = "#F0AAFA;"
                    Case "Needs Inspected"
                        hksc = "#AAFFAA;"
                    Case "Maintenance In"
                        hksc = "#999999;"
                    Case "Maintenance Out"
                        hksc = "#FF00FF;"
                    Case Else
                        hksc = "#FFFFFF;"
                End Select
                'Room Status
                Select Case Trim(rs("Status") & "")
                    Case "Cleaned"
                        rsc = "#00FF00;"
                    Case "Occupied"
                        rsc = "#FFFF00;"
                    Case "Offline"
                        rsc = "#FF0000;"
                    Case "Dirty"
                        rsc = "#00FFFF;"
                    Case "Being Cleaned"
                        rsc = "#F0AAFA;"
                    Case "Needs Inspected"
                        rsc = "#AAFFAA;"
                    Case "Maintenance In"
                        rsc = "#999999;"
                    Case "Maintenance Out"
                        rsc = "#FF00FF;"
                    Case Else
                        rsc = "#FFFFFF;"
                End Select
                'Maintenance Status
                Select Case Trim(rs("MaintStatus") & "")
                    Case "Cleaned"
                        msc = "#00FF00;"
                    Case "Occupied"
                        msc = "#FFFF00;"
                    Case "Offline"
                        msc = "#FF0000;"
                    Case "Dirty"
                        msc = "#00FFFF;"
                    Case "Being Cleaned"
                        msc = "#F0AAFA;"
                    Case "Needs Inspected"
                        msc = "#AAFFAA;"
                    Case "Maintenance In"
                        msc = "#999999;"
                    Case "Maintenance Out"
                        msc = "#FFFFFF;"
                    Case Else
                        msc = "#FFFFFF;"
                End Select


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
                'style='position:relative;background-color:"



            Loop
        End If
        lit1.Text &= ("</div>")
        rs.Close()
        cn.Close()
    End Sub

    Protected Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick

    End Sub
End Class
