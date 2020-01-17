
Partial Class Reports_FrontDesk_maintenancerequestbystatus
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim cn As Object
        Dim rs As Object

        cn = Server.CreateObject("ADODB.Connection")
        rs = Server.CreateObject("ADODB.Recordset")

        cn.open(Resources.Resource.DATABASE, Resources.Resource.USERNAME, Resources.Resource.PASSWORD)
        lit.text = ""
        rs.open("Select s.comboitem as Status, count(distinct requestid) as [Count] from t_Request r left outer join t_Comboitems s on s.comboitemid= r.statusid group by s.comboitem order by s.comboitem", cn, 0, 1)
        lit.text &= ("<table>")
        'response.write "<tr><th colspan = 2>Requests by Status</th></tr>"
        lit.text &= ("<tr>")
        For i = 0 To rs.fields.count - 1
            lit.text &= ("<th>" & rs.fields(i).name & "</th>")
        Next
        lit.text &= ("</tr>")

        Do While Not rs.eof
            lit.text &= ("<tr>")
            For i = 0 To rs.fields.count - 1
                lit.text &= ("<td align = right>" & rs.fields(i).value & "</td>")
            Next
            lit.text &= ("</tr>")
            rs.movenext()
        Loop
        rs.close()
        lit.text &= ("</table>")


        
        cn.close()
        rs = Nothing
        cn = Nothing


    End Sub
End Class
