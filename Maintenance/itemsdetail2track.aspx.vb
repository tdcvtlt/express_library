Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.ComponentModel
Imports System.IO
Imports System.Reflection
Imports System.Threading
Imports System.Web.Services
Imports System.Web.Script.Serialization

Partial Class Maintenance_itemsdetail2track
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        Dim id_maint = Request.QueryString("PreventiveMaintenanceID")
        If id_maint.Length = 0 Then Return

        data_refresh()

    End Sub


    Private Sub data_refresh()
        Dim sb = New StringBuilder()

        Using cn = New SqlConnection(Resources.Resource.cns)
            Using ad = New SqlDataAdapter("select r.roomnumber, u.name, r.roomid from t_room r inner join t_unit u on r.unitid = u.unitid order by roomnumber", cn)

                Dim dt = New DataTable()
                ad.Fill(dt)

                sb.AppendFormat("<table id=summary-list>")
                sb.AppendFormat("<thead><tr><th>Room Number</th><th>Unit</th></tr></thead>")
                sb.AppendFormat("<tbody>")

                For Each dr As DataRow In dt.Rows

                    sb.AppendFormat("<tr><td roomnumber={2}>{0}</td><td>{1}</td></tr>", dr("roomnumber").ToString(), dr("name").ToString(), dr("roomid").ToString())

                Next

                sb.AppendFormat("</tbody>")
                sb.AppendFormat("</table>")
                lit0.Text = sb.ToString()

            End Using


        End Using

    End Sub

    <WebMethod()> _
    Public Shared Function search(roomid As Int32) As String
        Dim sb = New StringBuilder()

        Using cn = New SqlConnection(Resources.Resource.cns)
            Using ad = New SqlDataAdapter("", cn)


            End Using

        End Using







        Return sb.ToString()
    End Function


    Private Class item

    End Class

End Class
