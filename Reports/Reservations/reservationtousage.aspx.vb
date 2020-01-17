
Partial Class Reports_Reservations_reservationtousage
    Inherits System.Web.UI.Page

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        Dim cn As Object
        Dim rs As Object
        Dim rs2 As Object
        Dim edate As String = Me.dteEDate.Selected_Date
        Dim sdate As String = Me.dteSDate.Selected_Date
        Dim sAns As String = ""
        Dim sql As String = ""
        cn = Server.CreateObject("ADODB.Connection")
        rs = Server.CreateObject("ADODB.Recordset")
        rs2 = Server.CreateObject("ADODB.Recordset")

        Server.ScriptTimeout = 10000
        cn.open(Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
        cn.CommandTimeout = 0

        sql = "select distinct r.reservationid as Reservation, r.checkindate as InDate, rt.comboitem as [Res Type], ut.comboitem as [Usage Type], rs.ComboItem as Source " & _
              "from t_Reservations r " & _
              "	inner join t_Roomallocationmatrix m on m.reservationid = r.reservationid " & _
              "	inner join t_Comboitems rt on rt.comboitemid = r.typeid " & _
              "	inner join t_Comboitems mt on mt.comboitemid = m.typeid " & _
              "	inner join t_usage u on u.usageid = m.usageid " & _
              "	inner join t_Comboitems ut on ut.comboitemid = u.typeid " & _
              " left outer join t_ComboItems rs on r.SourceID = rs.ComboItemID " & _
              "where ut.comboitem <> rt.comboitem " & _
              "	and r.checkindate between '" & sdate & "' and '" & edate & "' " & _
              "	order by r.checkindate	 "
        rs.open(sql, cn, 0, 1)
        If rs.eof And rs.bof Then
            sAns = "No Records"
        Else
            sAns = "<table>"
            sAns = sAns & "<tr>"
            For i = 0 To rs.fields.count - 1
                sAns = sAns & "<th>" & rs.fields(i).name & "</th>"
            Next
            sAns = sAns & "</tr>"
            Do While Not rs.eof
                sAns = sAns & "<tr>"
                For i = 0 To rs.fields.count - 1
                    sAns = sAns & "<td>"
                    If i = 1 Then
                        sAns = sAns & CDate(rs.fields(i).value)
                    Else
                        sAns = sAns & rs.fields(i).value
                    End If
                    sAns = sAns & "</td>"
                Next
                sAns = sAns & "</tr>"
                rs.movenext()
            Loop
            sAns = sAns & "</table>"
        End If
        rs.close()
        cn.close()
        cn = Nothing
        rs = Nothing
        litReport.Text = sAns
    End Sub
End Class
