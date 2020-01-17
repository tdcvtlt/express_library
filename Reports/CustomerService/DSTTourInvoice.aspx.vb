
Partial Class Reports_CustomerService_DSTTourInvoice
    Inherits System.Web.UI.Page

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        Dim cn As Object
        Dim rs As Object
        cn = Server.CreateObject("ADODB.Connection")
        rs = Server.CreateObject("ADODB.Recordset")
        Dim edate As String = Me.dteEDate.Selected_Date
        Dim sdate As String = Me.dteSDate.Selected_Date
        Dim totalAmt As Double = 0
        Dim sAns As String = ""
        Server.ScriptTimeout = 10000
        cn.open(Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
        cn.CommandTimeout = 0

        sAns = "<H2>DST Tour Invoice " & sdate & " - " & edate & "</H2>"
        sAns = sAns & "<table>"
        sAns = sAns & "<tr>"
        sAns = sAns & "<th><u>Name</u></th>"
        sAns = sAns & "<th><u>TourID</u></th>"
        sAns = sAns & "<th><u>Campaign</u></th>"
        sAns = sAns & "<th><u>Tour Date</u></th>"
        sAns = sAns & "<th><u>Tour Status</u></th>"
        sAns = sAns & "<th><u>Marital Status</u></th>"
        sAns = sAns & "<th><u>Tour Fee</u></th>"
        sAns = sAns & "</tr>"
        rs.Open("SELECT a.TourID, e.Name, b.FirstName, b.LastName, a.TourDate, c.ComboItem AS TourStatus, d.ComboItem AS MaritalStatus FROM t_Tour a INNER JOIN t_Prospect b ON a.ProspectID = b.ProspectID LEFT OUTER JOIN t_ComboItems c ON a.StatusID = c.ComboItemID LEFT OUTER JOIN t_ComboItems d ON b.MaritalStatusID = d.ComboItemID LEFT OUTER JOIN t_Campaign e ON a.CampaignID = e.CampaignID WHERE     ((e.Name = 'DST') OR (e.Name = 'DSTY')) and a.subtypeid not in (Select c.comboitemid from t_comboItems c inner join t_Combos co on c.COmbOID = co.COmbOID where co.comboname = 'TourSubType' and c.comboitem = 'Exit') and TourDate between '" & sdate & "' and '" & edate & "' order by tourdate asc", cn, 3, 3)
        If rs.EOF And rs.BOF Then
            sAns = sAns & "<tr><td colspan = '8'>No Tours to Report in this Date Range.</td></tr>"
        Else
            Do While Not rs.EOF
                sAns = sAns & "<tr>"
                sAns = sAns & "<td>" & rs.Fields("Firstname").Value & " " & rs.Fields("Lastname").Value & "</td>"
                sAns = sAns & "<td>" & rs.Fields("TourID").Value & "</td>"
                sAns = sAns & "<td>" & rs.Fields("name").Value & "</td>"
                sAns = sAns & "<td>" & rs.Fields("TourDate").Value & "</td>"
                sAns = sAns & "<td>" & rs.Fields("TourStatus").Value & "</td>"
                sAns = sAns & "<td>" & rs.Fields("MaritalStatus").Value & "</td>"
                If rs.Fields("TourStatus").Value = "Showed" Or rs.Fields("TourStatus").Value = "No Tour - Overage" Then
                    If rs.Fields("MaritalStatus").Value & "" = "Married" Or rs.Fields("MaritalStatus").Value & "" = "Co-Habitant" Then
                        sAns = sAns & "<td>$175.00</td>"
                        totalAmt = totalAmt + 175
                    Else
                        sAns = sAns & "<td>$80.00</td>"
                        totalAmt = totalAmt + 80
                    End If
                Else
                    sAns = sAns & "<td>$0.00</td>"
                End If
                sAns = sAns & "</tr>"
                rs.moveNext()
            Loop
            sAns = sAns & "<tr><td colspan = '5'></td><td><B>TOTAL:</B></td><td><B>" & FormatCurrency(totalAmt) & "<B></td></tr>"
        End If
        rs.Close()
        sAns = sAns & "</table>"


        cn.Close()
        rs = Nothing
        cn = Nothing

        litReport.Text = sAns

    End Sub

    Protected Sub Button3_Click(sender As Object, e As System.EventArgs) Handles Button3.Click
        Response.Clear()
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment; filename=DSTTourInvoice.xls")
        Response.Write(litReport.Text)
        Response.End()

    End Sub
End Class
