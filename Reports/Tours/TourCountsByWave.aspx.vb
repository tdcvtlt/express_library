Imports Microsoft.VisualBasic

Partial Class Reports_Tours_TourCountsByWave
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then

        End If
    End Sub

    Protected Sub Report_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Report.Click
        Dim cn As Object
        Dim rs As Object
        Dim edate As String = Me.eDate.Selected_Date
        Dim sdate As String = Me.sDate.Selected_Date
        Dim sql As String = ""
        Dim sAns As String = ""
        Dim curdate As Date

        cn = Server.CreateObject("ADODB.Connection")
        rs = Server.CreateObject("ADODB.Recordset")

        cn.open(Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)

        cn.commandtimeout = 0
        curdate = CDate(sdate)


        sAns = "<table>"
        Do While curdate <= CDate(edate)
            sql = "Select Distinct(tt.comboitem) as TourTime, Count(*) as TimeCounts from t_Tour t left outer join t_Comboitems tt on tt.comboitemid = t.tourtime where tourdate = '" & CDate(curdate) & "' and subtypeid not in (Select comboitemid from t_ComboItems i inner join t_combos c on c.comboid = i.comboid where comboname = 'TourSubType' and comboitem = 'Exit' ) and tourLocationID in (Select comboitemid from t_ComboItems i inner join t_combos c on c.comboid = i.comboid where comboname = 'TourLocation' and comboitem = 'KCP' ) and statusid in (Select comboitemid from t_ComboItems i inner join t_combos c on c.comboid = i.comboid where comboname = 'TourStatus' and comboitem = 'Booked' ) and campaignid not in (Select campaignid from t_Campaign where name in ('KCM', 'MAL', 'RR-O', 'DTD', 'DYD', 'ODP','ODPH','OPC-OS','OPC-REF','4K','Prestige','PMI','DST-DYD','DM-DYD','OPC-OSWT','OPC-OSWC','OPC-OSG')) group by tourdate,tt.comboitem order by tt.comboitem asc"
            sAns = sAns & "<tr><td><b>" & curdate & "</b></td></tr>"
            rs.open(sql, cn, 0, 1)
            If rs.eof And rs.bof Then
                sAns = sAns & "<tr><td colspan = '4'>No Tours Booked for this Date.</td></tr>"
            Else
                Do While Not rs.eof
                    sAns = sAns & "<tr>"
                    sAns = sAns & "<td>" & rs.fields("TourTime").value & "</td>"
                    sAns = sAns & "<td>" & rs.fields("TimeCounts").value & "</td>"
                    sAns = sAns & "</tr>"
                    rs.MoveNext()
                Loop
            End If
            rs.close()
            curdate = curdate.AddDays(1)
        Loop
        sAns = sAns & "</table>"

        cn.close()
        rs = Nothing
        cn = Nothing
        'sAns = "What up Mutta"
        Me.litReport.Text = sAns & " " & curdate

    End Sub
End Class
