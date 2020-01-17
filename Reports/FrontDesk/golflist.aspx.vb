
Partial Class Reports_FrontDesk_golflist
    Inherits System.Web.UI.Page

    Protected Sub btnRun_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRun.Click
        Dim cn As Object
        Dim rs As Object
        Dim sAns As String = ""

        cn = Server.CreateObject("ADODB.Connection")
        rs = Server.CreateObject("ADODB.Recordset")

        cn.Open(Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)


        rs.Open("Select a.CheckInDate, a.CheckOUtDate, b.FirstName, b.LastName from t_Reservations a inner join t_Prospect b on a.prospectid = b.prospectid where a.statusid in (Select comboitemid from t_ComboItems i inner join t_Combos c on c.comboid = i.comboid where comboname = 'ReservationStatus' and comboitem = 'Booked') and a.reslocationid in (select comboitemid from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid where comboname = 'ReservationLocation' and comboitem = 'KCP') and a.typeID  in (Select comboitemid from t_CombOItems i inner join t_Combos c on c.comboid = i.comboid where comboname = 'reservationtype' and comboitem in ('Owner','Rental','Marketing','Points','Developer')) and a.CheckInDate between '" & SDate.Selected_Date & "' and '" & EDate.Selected_Date & "' order by lastname asc", cn, 3, 3)
        If rs.EOF And rs.BOF Then
            sAns = "No Reservations in this Date Range."
        Else
            sANs = "<table>"
            Do While Not rs.EOF
                sAns = sAns & "<tr>"
                sAns = sAns & "<td>" & rs.Fields("LastName").value & ", " & rs.Fields("FirstName").value & "</td>"
                sAns = sAns & "<td>" & rs.Fields("CheckInDate").value & "</td>"
                sAns = sAns & "<td>" & rs.Fields("CheckOUtDate").value & "</td>"
                sAns = sAns & "</tr>"
                rs.MoveNExt()
            Loop
            sAns = sANs & "</table>"
        End If
        rs.Close()

        Lit.Text = "<div id='printable'>" & sAns & "</div>"



        cn.Close()
        rs = Nothing
        cn = Nothing
    End Sub
End Class
