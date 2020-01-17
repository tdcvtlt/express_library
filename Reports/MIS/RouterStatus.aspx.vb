
Partial Class Reports_MIS_RouterStatus
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Get_Report()
        End If
    End Sub

    Protected Sub Get_Report()
        Dim cn As Object
        Dim rs As Object
        Dim sql As String = ""

        cn = Server.CreateObject("ADODB.Connection")
        rs = Server.CreateObject("ADODB.Recordset")

        cn.open(Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)

        cn.commandtimeout = 0
        litReport.Text &= "<table>"
        litReport.Text &= "<tr><th><u>RoomNumber</u></th><th><u>IPAddress</u></th><th><u>Status</u></th><th><u>Reservation When Missing</u></th><th><u>Date Missing</u></th><th><u>Last Checked</u></th></tr>"
        sql = "Select a.*, b.RoomNumber, case when u.UFValue is null then 0 else u.UFValue end as Installed from t_RouterStatus a inner join t_Room b on a.roomid = b.roomid left outer join (select * from t_UF_Value where UFID = 399) u on u.keyvalue = b.roomid order by charindex('-', b.roomnumber), RoomNumber asc"

        rs.open(sql, cn, 0, 1)

        If rs.EOF And rs.BOF Then
            litReport.Text &= "<tr><td colspan = '6'>No Routers in System</td></tr>"
        Else
            Do While Not rs.EOF
                litReport.Text &= "<tr>"
                litReport.Text &= "<td>" & rs.Fields("RoomNumber").value & "</td>"
                litReport.Text &= "<td><a href='http://" & Trim(rs.Fields("IPAddress").value & "") & ":8080' target = '_blank'>" & rs.Fields("IPAddress").value & "</a></td>"
                If rs.fields("Installed").value Then
                    litReport.Text &= "<td><font color = 'green'>Maintained by Cox Communications</font></td>"
                Else
                    If rs.Fields("Status").value Then
                        litReport.Text &= "<td><font color = 'green'>UP</font</td>"
                    Else
                        litReport.Text &= "<td><font color = 'red'>DOWN</font></td>"
                    End If
                End If
        If rs.Fields("MissingResID").value = 0 Then
            litReport.Text &= "<td>N/A</td>"

            If rs.Fields("DateMissing").value.Equals(DBNull.Value) Then
                litReport.Text &= "<td>N/A</td>"
            Else
                litReport.Text &= "<td>" & rs.Fields("DateMissing").value & "</td>"
            End If
        Else
            litReport.Text &= "<td><a href = '../editReservation.asp?reservationid=" & rs.Fields("MissingResID").value & "'>" & rs.Fields("MissingResID").value & "</a></td>"
            litReport.Text &= "<td>" & rs.Fields("DateMissing").value & "</td>"
        End If
        litReport.Text &= "<td>" & rs.Fields("LastChecked").value & "</td>"
        litReport.Text &= "</tr>"
        rs.MoveNext()
            Loop
        End If
        litReport.Text &= "</table>"

        cn.close()
        rs = Nothing
        cn = Nothing

    End Sub

End Class
