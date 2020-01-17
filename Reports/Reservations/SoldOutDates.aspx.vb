Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports System.Data.SqlClient
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports System.Web.Security
Imports System.Web
Imports System.Configuration
Imports System.Data
Imports System

Partial Class Reports_Reservations_SoldOutDates
    Inherits System.Web.UI.Page
    Dim Report As New ReportDocument
    Dim sReport As String = "reportfiles/CrystalReport.rpt"


    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If IsPostBack = True And Not Session("Report") Is Nothing Then CrystalReportViewer1.ReportSource = Session("Report")
    End Sub

    Protected Sub btnRun_Click(sender As Object, e As System.EventArgs) Handles btnRun.Click
        lit1.Text = ""
        If sdate.Selected_Date.Length > 0 And edate.Selected_Date.Length > 0 Then
            show_report()
        Else
            lit1.Text = String.Format("<br/><strong>Dates are missing.</strong>")
        End If
        Return

        If sdate.Selected_Date <> "" And edate.Selected_Date <> "" Then

            Report.Load(Server.MapPath(sReport))
            Report.FileName = Server.MapPath(sReport)
            Report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
            Report.DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)

            With Report
                .SetParameterValue("sdate", sdate.Selected_Date)
                .SetParameterValue("edate", edate.Selected_Date)
            End With

            Session.Add("Report", Report)
            CrystalReportViewer1.ReportSource = Session("Report")
        End If




    End Sub

    Private Sub show_report()
        Dim s_date As DateTime = Convert.ToDateTime(sdate.Selected_Date)
        Dim e_date As DateTime = Convert.ToDateTime(edate.Selected_Date)
        'Dim s_date As DateTime = Convert.ToDateTime("09/01/2014")
        'Dim e_date As DateTime = Convert.ToDateTime("12/31/2014")
        Dim m_avail As Integer = ddlMinAvail.SelectedItem.Text
        Dim n_days = Enumerable.Range(1, 31).ToArray()
        Dim r_types() As String = New String() {"rental", "marketing"}
        Dim u_types() As String = New String() {"cottage", "estates", "townes"}
        Dim i_resv_2_unit As New Dictionary(Of String, List(Of String))
        i_resv_2_unit.Add("cottage", New String() {"1", "2", "3"}.ToList())
        i_resv_2_unit.Add("estates", New String() {"1BD-DWN", "1BD-UP", "2", "3", "4"}.ToList())
        i_resv_2_unit.Add("townes", New String() {"2", "4"}.ToList())

        Dim dt = New DataTable()
        Using cn = New SqlConnection(Resources.Resource.cns)
            Using cm = New SqlCommand("sp_SoldOutDates", cn)
                cm.CommandTimeout = 0
                cm.CommandType = CommandType.StoredProcedure
                cm.Parameters.AddWithValue("@sdate", s_date)
                cm.Parameters.AddWithValue("@edate", e_date)
                Try
                    cn.Open()
                    Dim dr As SqlDataReader = cm.ExecuteReader()
                    Dim dt_o As New DataTable()
                    dt_o.Load(dr)                    
                    dt = dt_o.Select("reservationtype in ('rental', 'marketing')").CopyToDataTable()
                Catch ex As Exception
                    Response.Write(ex.Message)
                Finally
                    cn.Close()
                End Try
            End Using
        End Using

        Dim sb = New StringBuilder()
        sb.AppendFormat("<table border=1 style=border-collapse:collapse>")
        sb.AppendFormat("<thead><tr><th/><th/><th/><th/>")
        For Each d As Integer In n_days
            sb.AppendFormat("<th>{0}</th>", d)
        Next
        sb.AppendFormat("</tr></thead><tbody>")

        Dim list As New List(Of DateTime)
        Dim days = (e_date - s_date).Days        
        For i = 0 To days
            list.Add(s_date.AddDays(i))
        Next
        Dim prev_month As Integer = -1
        For Each e As Date In list                    

            If e.Month <> prev_month Then

                Dim date_current = DateTime.MinValue
                sb.AppendFormat("<tr><td><h2>{0}/{1}</h2></td>", e.Month, e.Year)

                For r = 0 To r_types.Count() - 1
                    If r > 0 Then
                        sb.AppendFormat("<tr><td/><td><h3>{0}</h3></td>", r_types(r))
                    Else
                        sb.AppendFormat("<td><h3>{0}</h3></td>", r_types(r))
                    End If

                    For u = 0 To u_types.Count() - 1
                        If u > 0 Then
                            sb.AppendFormat("<tr><td/><td/><td><h3>{0}</h3></td>", u_types(u))
                        Else
                            sb.AppendFormat("<td><h3>{0}</h3></td>", u_types(u))
                        End If

                        For bd = 0 To i_resv_2_unit(u_types(u)).Count - 1
                            Dim bedroom = i_resv_2_unit(u_types(u))(bd)
                            If bd > 0 Then
                                sb.AppendFormat("<tr><td/><td/><td/><td><h4>{0}</h4></td>", IIf(bedroom.Length = 1, bedroom & "BD", bedroom))
                            Else
                                sb.AppendFormat("<td><h4>{0}</h4></td>", IIf(bedroom.Length = 1, bedroom & "BD", bedroom))
                            End If

                            For Each d In n_days
                                Dim bed = bd
                                Dim r_type = r
                                Dim u_type = u

                                If d >= e.Day And d <= Date.DaysInMonth(e.Year, e.Month) Then
                                    date_current = New DateTime(e.Year, e.Month, d)
                                    Dim avail = dt.Rows.OfType(Of DataRow).Where(Function(x) x("reservationtype").ToString().ToLower() = r_types(r_type).ToLower() _
                                                 And x("unittype").ToString().ToLower() = u_types(u_type).ToLower() _
                                                 And x("bd").ToString().ToLower() = i_resv_2_unit(u_types(u_type))(bed).ToLower() _
                                                 And Convert.ToInt16(x("month").ToString()) = date_current.Month _
                                                 And Convert.ToInt16(x("day").ToString()) = date_current.Day _
                                                 And Convert.ToInt16(x("year").ToString()) = date_current.Year).FirstOrDefault()

                                    If Not avail Is Nothing Then
                                        If avail("available").ToString() > m_avail Then
                                            sb.AppendFormat("<td style=font-size:1.2em;font-weight:bold >{0}</td>", avail("available").ToString())
                                        Else
                                            sb.AppendFormat("<td style=font-size:1.2em;font-weight:bold>{0}</td>", "0")
                                        End If
                                    Else
                                        sb.AppendFormat("<td></td>")
                                    End If
                                Else
                                    sb.AppendFormat("<td></td>")
                                End If
                            Next
                        Next
                        If u > 0 Then sb.AppendFormat("</tr>")
                    Next
                    If r > 0 Then sb.AppendFormat("</tr>")
                Next
                sb.AppendFormat("</tr>")
            End If
            If e.Month <> prev_month Then prev_month = e.Month
        Next
        sb.AppendFormat("</tbody></table>")
        lit1.Text = sb.ToString()
    End Sub

    Protected Sub btnExcel_Click(sender As Object, e As System.EventArgs) Handles btnExcel.Click
        Response.Clear()
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment; filename=Sold-Out-Dates.xls")
        Response.Write(lit1.Text)
        Response.End()
    End Sub
End Class
