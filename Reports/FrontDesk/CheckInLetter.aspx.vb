
Partial Class Reports_FrontDesk_CheckInLetter
    Inherits System.Web.UI.Page

    Protected Sub btnRun_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRun.Click
        Dim cn As Object
        Dim rs As Object

        cn = Server.CreateObject("ADODB.connection")
        rs = Server.CreateObject("ADODB.recordset")

        cn.open(Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)

        Lit.Text = "<div id='printable'>"

        rs.open("SELECT p.FirstName + ' ' + p.LastName as GuestName FROM t_Prospect p INNER JOIN t_Reservations r on r.ProspectID = p.ProspectID INNER JOIN t_ComboItems c on c.ComboItemID = r.ResLocationID INNER JOIN t_ComboItems st on st.ComboItemID = r.StatusID WHERE c.ComboItem = 'KCP' and st.ComboItem = 'Booked' and r.CheckInDate = '" & dfCheckIn.Selected_Date & "' ORDER BY p.LastName", cn, 3, 3)
        If rs.eof And rs.bof Then
            Lit.Text &= ("There are no scheduled check-ins for the date chosen.")
        Else
            Do While Not rs.eof

                Lit.Text &= "<div id='letter' style='position:relatve;'>"
                Lit.Text &= "<br><br><br><br><br><br>"
                Lit.Text &= "<p>"
                Lit.Text &= "<font face = 'Monotype Corsiva'>"
                Lit.Text &= "<font size = '20'>"
                Lit.Text &= "Dear&nbsp;" & rs.fields("GuestName").value & ",<br><br>"
                Lit.Text &= "Welcome to King's Creek!<br><br>"
                Lit.Text &= "I wish you an enjoyable visit to King's Creek and the Historical Williamsburg area.<br><br>"
                Lit.Text &= "Please feel free to call upon our staff or me if there is anything we can do to ensure your comfort.<br><br><br/>"
                Lit.Text &= "Warm Regards,<br/><br/><div><img style=""height:125px;width:350px;"" src=""../../images/atyler.gif"" /></div>Angie Tyler<br/>Guest Relations Manager"
                Lit.Text &= "</font>"
                Lit.Text &= "</font>"
                Lit.Text &= "</p>"
                Lit.Text &= "</div><br style='page-break-after:always'>"

                rs.movenext()
            Loop
        End If
        rs.close()
        cn.close()

        Lit.Text &= "</div>"
        rs = Nothing
        cn = Nothing


    End Sub
End Class
