Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Partial Class Reports_Tours_FutureToursWithPremiumsIssued
    Inherits System.Web.UI.Page

    'Dim con As String = "data source=RS-SQL-01;initial catalog=CRMSNet;user=asp;password=aspnet;persist security info=False;packet size=4096; "
    Dim con As String = Resources.Resource.cns

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Me.IsPostBack = False Then
            Using cn = New SqlConnection(con)
                Using ad = New SqlDataAdapter("select b.COMBOITEM, b.COMBOITEMID from t_combos a inner join t_comboitems b on a.comboid = b.comboid where comboname = 'tourlocation' and (b.comboitem in ('kcp', 'richmond') or b.active = 1) order by b.comboitem", cn)
                    Dim dt = New DataTable()
                    Try
                        ad.Fill(dt)
                        ddLocation.DataSource = dt
                        ddLocation.DataTextField = "COMBOITEM"
                        ddLocation.DataValueField = "COMBOITEMID"
                        ddLocation.DataBind()

                        dt.Clear()
                        dt = Nothing
                        dt = New DataTable()

                        ad.SelectCommand = New SqlCommand("SELECT CampaignID, Name FROM T_CAMPAIGN where active = 1 ORDER BY Name", cn)
                        ad.Fill(dt)
                        ddCampaign.Items.Add(New ListItem("All", "1"))
                        ddCampaign.DataSource = dt
                        ddCampaign.DataTextField = "Name"
                        ddCampaign.DataValueField = "CampaignID"
                        ddCampaign.AppendDataBoundItems = True
                        ddCampaign.DataBind()


                        lbF.DataSource = dt
                        lbF.AppendDataBoundItems = True
                        lbF.DataTextField = "Name"
                        lbF.DataValueField = "CampaignID"

                        lbF.DataBind()


                    Catch ex As Exception
                        Response.Write(ex.Message)
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using
        End If
    End Sub

    Protected Sub btn_Submit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Submit.Click
        Dim sb As New StringBuilder()
        Dim l_tourIDs As New List(Of String)

        Using cn As New SqlConnection(con)
            Using ada As New SqlDataAdapter(getTours, cn)
                Try
                    ada.SelectCommand.CommandTimeout = 0

                    Dim tb_tours As New DataTable()
                    Dim tb_premiums As New DataTable()
                    ada.Fill(tb_tours)
                    ada.SelectCommand = New SqlCommand(getPremiums, cn)
                    ada.Fill(tb_premiums)

                    If tb_tours.Rows.Count > 0 Then

                        sb.Append("<table border=1 style=border-collapse:collapse;>")
                        Dim col_names() As String = {"Campaign", "Booked By", "Tour ID", "Prospect", "Extension", "Tour Date", "Tour Time", "Is Owner", "Comments"}

                        sb.Append("<tr>")
                        For Each s As String In col_names
                            sb.AppendFormat("<td style='font-weight:bold;'>{0}</td>", s.ToUpper())
                        Next
                        sb.Append("</tr>")


                        For Each g As IGrouping(Of String, DataRow) In tb_tours.AsEnumerable().GroupBy(Function(x) x.Item("tourid").ToString())

                            Dim ext As IEnumerable(Of String) = g.Where(Function(x) String.IsNullOrEmpty(x.Item("extension").ToString()) = False).Select(Function(x) x.Item("Extension").ToString())

                            Dim tourtime As String = g.First().Item("tourtime").ToString()

                            If String.IsNullOrEmpty(tourtime) = False Then
                                If Integer.Parse(tourtime) >= 1200 And Integer.Parse(tourtime) < 1300 Then
                                    tourtime = g.First().Item("tourtime").ToString() & " PM"
                                ElseIf Integer.Parse(tourtime) > 1259 Then
                                    tourtime = (Integer.Parse(tourtime) - 1200).ToString() & " PM"
                                Else
                                    tourtime = tourtime & " AM"
                                End If
                            End If

                            sb.Append("<tr>")
                            sb.AppendFormat("<td>{0}</td>", g.First().Item("Name").ToString())
                            sb.AppendFormat("<td>{0}</td>", g.First().Item("booked").ToString())
                            sb.AppendFormat("<td>{0}</td>", g.First().Item("tourid").ToString())
                            sb.AppendFormat("<td>{0}, {1}</td>", g.First().Item("LastName").ToString(), g.First().Item("FirstName").ToString())

                            sb.AppendFormat("<td>{0}</td>", String.Join(", ", ext.ToArray()))
                            sb.AppendFormat("<td>{0:d}</td>", DateTime.Parse(g.First().Item("tourdate").ToString()))
                            sb.AppendFormat("<td>{0}</td>", tourtime)

                            If g.First().Item("isowner").ToString().ToLower().Equals("y") Then
                                sb.AppendFormat("<td>{0}</td>", g.First().Item("isowner"))
                            Else
                                sb.AppendFormat("<td>{0}</td>", "")
                            End If


                            Dim sButton As String = String.Empty

                            sButton = "<input type='button' value = 'Add Comment' onclick="
                            sButton &= Chr(34) & "javascript:modal.mwindow.open('" & Request.ApplicationPath & "/general/editnote.aspx?noteid=0&KeyField=Comments&keyvalue=" & g.Key & "&linkid=','win01',350,350);" & Chr(34) & " />"
                            sButton &= "<input type='button' value = 'View Comments' onclick="
                            sButton &= Chr(34) & "javascript:modal.mwindow.open('" & Request.ApplicationPath & "/general/viewcomments.aspx?keyfield=Comments&keyvalue=" & g.Key & "','win01',350,350);" & Chr(34) & " />"

                            sButton &= "<input type='button' value='Print Tour Slip' onclick='print_tour_slip(" & g.First().Item("tourid").ToString() & ")' />"


                            l_tourIDs.Add(g.First().Item("tourid").ToString())


                            sb.AppendFormat("<td>{0}</td>", sButton)
                            sb.Append("</tr>")


                            Dim p() As DataRow = tb_premiums.Select(String.Format("KeyValue = {0}", g.Key))
                            For Each pi As DataRow In p
                                sb.Append("<tr>")
                                sb.Append("<td>&nbsp;</td>")
                                sb.Append("<td colspan=7>")
                                sb.Append("<table>")

                                sb.Append("<tr>")
                                sb.AppendFormat("<td><strong>{0:d}</strong></td>", DateTime.Parse(pi.Item("DateIssued").ToString()))
                                sb.AppendFormat("<td style='color:blue;'><strong>{0}</strong></td>", pi.Item("Personnel").ToString())
                                sb.AppendFormat("<td style='font-style:italics;'><strong>{0}</strong></td>", pi.Item("PremiumName").ToString())
                                sb.AppendFormat("<td style='color:red;'><strong>{0}</strong></td>", pi.Item("QtyIssued").ToString())
                                sb.AppendFormat("<td style='color:red;'><strong>{0:N2}</strong></td>", Decimal.Parse(pi.Item("CostEA").ToString()))

                                sb.Append("</tr>")
                                sb.Append("</table>")
                                sb.Append("</td>")
                                sb.Append("</tr>")
                            Next
                        Next

                        sb.Append("</table>")

                        hfTourIDs.Value = String.Join(",", l_tourIDs.ToArray())

                    Else
                        'sb.AppendFormat("<h2>Resort {0} with campaign {1} on {2} and beyond has no tours scheduled.</h2>", _
                        '                ddLocation.SelectedItem.Text, _
                        '                ddCampaign.SelectedItem.Text, _
                        '                DateTime.Parse(IIf(String.IsNullOrEmpty(sDate.Selected_Date), DateTime.Now, sDate.Selected_Date)).ToString("d"))

                        sb.Append("<h2>No data available</h2>")
                    End If

                Catch ex As Exception
                    Response.Write(ex.Message)
                Finally
                    cn.Close()
                End Try
            End Using
        End Using
        lit_report.Text = sb.ToString()
    End Sub

    Protected Sub btn_Export_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Export.Click
        Response.Clear()
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", String.Format("attachment; filename=FutureToursWithPremiumsIssued{0}_{1}.xls", DateTime.Now.ToLongTimeString(), DateTime.Now.Millisecond))
        Response.Write(lit_report.Text)
        Response.End()
    End Sub

    Private ReadOnly Property getTours As String
        Get
            Dim ddCampaign_sel() As String
            If ddCampaign.SelectedItem.Text.Trim().Contains("All") = True Then
                ddCampaign_sel = ddCampaign.Items.OfType(Of ListItem).Where(Function(x) x.Text.Equals("All") = False).Select(Function(x) x.Value).ToArray()
            Else
                ddCampaign_sel = ddCampaign.Items.OfType(Of ListItem).Where(Function(x) x.Value.Equals(ddCampaign.SelectedValue)).Select(Function(x) x.Value).ToArray()
            End If

            ddCampaign_sel = lbT.Items.OfType(Of ListItem).Select(Function(x) x.Value).ToArray()

            Dim sqlEx = IIf(cbOwnersOnly.Checked, String.Format("and IsOwner='y' "), "")

            Dim sql = String.Format( _
                "Select distinct a.*, ISNULL(room.phone, '') as Extension, b.Name, b.CampaignID, n.NoteID, " & _
                "(select FirstName from t_event e left join t_personnel p on e.createdbyid = p.personnelid where e.keyfield = 'tourid' and type = 'create' and e.keyvalue = a.tourid) + ' ' + " & _
                "(select LastName from t_event e left join t_personnel p on e.createdbyid = p.personnelid where e.keyfield = 'tourid' and type = 'create' and e.keyvalue = a.tourid) 'Booked' " & _
                "from ( " & _
                "Select t.tourid, t.ReservationID, t.statusid, " & _
                "coalesce(cast((select comboitem from t_comboitems where comboitemid = t.tourtime) as varchar(50)), 0) as TourTime,  " & _
                "t.tourdate, p.FirstName, coalesce(p.lastname, '')[LastName], t.CampaignID, " & _
                "case when (select COUNT(*) from v_contractinventory c where c.ProspectID = p.ProspectID) > 0 then 'Y' else 'N' end IsOwner " & _
                "from t_Tour t  " & _
                "left outer join t_Prospect p on t.prospectid = p.prospectid where tourdate between '{0}' and '{1}' and tourLocationid = {2} and t.statusid = '16995'  " & _
                ") a  " & _
                "inner join t_Campaign b on a.CampaignID = b.CampaignID " & _
                "left outer join t_reservations r on r.reservationid = a.reservationid " & _
                "left outer join t_roomallocationmatrix x on x.reservationid = r.reservationid " & _
                "left outer join t_room room on room.roomid = x.roomid " & _
                "left outer join t_note n on n.keyfield = 'comments' and n.keyvalue = a.tourid " & _
                "where b.CampaignID in ({3}) {4} order by a.TourDate, a.TourTime, a.lastname, a.firstname", _
                sDate.Selected_Date, _
                eDate.Selected_Date, _
                ddLocation.SelectedValue, _
                String.Join(",", ddCampaign_sel), sqlEx)

            Return sql
        End Get
    End Property

    Private ReadOnly Property getPremiums As String
        Get
            Dim ddCampaign_sel() As String
            If ddCampaign.SelectedItem.Text.Trim().Contains("All") = True Then
                ddCampaign_sel = ddCampaign.Items.OfType(Of ListItem).Where(Function(x) x.Text.Equals("All") = False).Select(Function(x) x.Value).ToArray()
            Else
                ddCampaign_sel = ddCampaign.Items.OfType(Of ListItem).Where(Function(x) x.Value.Equals(ddCampaign.SelectedValue)).Select(Function(x) x.Value).ToArray()
            End If

            If lbT.Items.Count = 0 Then Return ""

            ddCampaign_sel = lbT.Items.OfType(Of ListItem).Select(Function(x) x.Value).ToArray()


            Return String.Format( _
                "select (select comboItem from t_ComboItems where comboItemid = pi.statusid) 'status', " & _
                "(select firstname + ' ' + lastname from t_personnel where personnelid = issuedbyid) 'Personnel', * " & _
                "from t_premiumissued pi inner join t_premium p on pi.premiumid = p.premiumid " & _
                "where(pi.statusid = 16809)  " & _
                "and pi.keyfield = 'tourid' and pi.keyvalue in ( " & _
                "select distinct t.tourid " & _
                "from t_Tour t " & _
                "left outer join t_Prospect p on t.prospectid = p.prospectid " & _
                "inner join t_Campaign b on t.CampaignID = b.CampaignID  " & _
                "left outer join t_reservations r on r.reservationid = t.reservationid " & _
                "left outer join t_roomallocationmatrix x on x.reservationid = r.reservationid " & _
                "left outer join t_room room on room.roomid = x.roomid " & _
                "where b.CampaignID in ({0}) " & _
                "and tourdate between '{1}' and '{2}' and tourLocationid = {3} and t.statusid = '16995')", _
                String.Join(",", ddCampaign_sel), _
                 sDate.Selected_Date, _
                 eDate.Selected_Date, _
                ddLocation.SelectedValue)
        End Get
    End Property

    

    Protected Sub btPrintSlip_Click(sender As Object, e As System.EventArgs) Handles btPrintSlip.Click

        Using cn = New SqlConnection(Resources.Resource.cns)
            Using cm = New SqlCommand("sp_OPCosTourSlip", cn)

                Try

                    Dim dt = New DataTable()
                    cm.CommandType = CommandType.StoredProcedure
                    cm.Parameters.Add("@tourID", SqlDbType.Int).Value = hfTourID.Value

                    cn.Open()

                    dt.Load(cm.ExecuteReader())                    

                    If dt.Rows.Count = 1 Then
                        Dim cr = New CrystalDecisions.CrystalReports.Engine.ReportDocument()
                        cr.Load(Server.MapPath("~/Reports/OPCOS_TourSlip.rpt"))
                        cr.SetDataSource(dt)

                        cr.Subreports(0).DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
                        cr.Subreports(0).DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)

                        cr.Subreports(1).DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
                        cr.Subreports(1).DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)

                        cm.CommandText = String.Format("select REPLACE(p.LastName, ' ', '') + '_' + p.FirstName [FullName] from t_Prospect p inner join t_Tour t on t.ProspectID = p.ProspectID where t.TourID={0}", hfTourID.Value)
                        cm.CommandType = CommandType.Text
                        Dim full_name = CType(cm.ExecuteScalar(), String)


                        cr.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, True, String.Format("OPCOSTourSlip-({0})", full_name))

                    End If


                Catch ex As Exception
                    Response.Write(ex.Message)
                Finally
                    cn.Close()
                End Try
            End Using
        End Using
    End Sub

    Protected Sub btPrintSlips_Click(sender As Object, e As System.EventArgs) Handles btPrintSlips.Click

        'If String.IsNullOrEmpty(ddCampaign.SelectedValue) Or String.IsNullOrEmpty(ddLocation.SelectedValue) Or sDate.Selected_Date.Length = 0 Or eDate.Selected_Date.Length = 0 Then Return        
        If lbT.Items.Count = 0 Then Return

        Dim Report As New ReportDocument
        Dim sReport As String = Server.MapPath("~/Reports/OPCOS_TourSlips.rpt")

        Report.Load(sReport)

        Report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
        Report.DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)


        Report.Subreports(0).DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
        Report.Subreports(0).DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)

        Report.Subreports(1).DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
        Report.Subreports(1).DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)

        Report.SetParameterValue("campaignID", String.Join(",", lbT.Items.OfType(Of ListItem).Select(Function(x) String.Format("'{0}'", x.Value)).ToArray()))
        Report.SetParameterValue("sd", sDate.Selected_Date)
        Report.SetParameterValue("ed", eDate.Selected_Date)
        Report.SetParameterValue("tourLocationID", ddLocation.SelectedValue)
        Report.SetParameterValue("isOwner", IIf(cbOwnersOnly.Checked = False, "", " and a.IsOwner = 'Y' "))
        Session.Add("report", Report)

        CrystalReportViewer1.ReportSource = Session("report")

        Report.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, True, String.Format("OPCOSTourSlips-({0})", "kcp"))

        Report.Close()
        Report.Clone()
        Report.Dispose()
        Report = Nothing
        GC.Collect()
        GC.WaitForPendingFinalizers()

    End Sub

    Protected Sub btSingleRight_Click(sender As Object, e As System.EventArgs) Handles btSingleRight.Click

        If lbF.Items.Count = 0 Then Return
        If lbF.SelectedItem Is Nothing Then Return

        lbT.Items.Add(lbF.SelectedItem)
        lbF.Items.RemoveAt(lbF.SelectedIndex)

        Dim lb1 = lbT.Items.OfType(Of ListItem).OrderBy(Function(x) x.Text).ToList()

        lbT.DataSource = lb1

        lbT.ClearSelection()
        lbF.ClearSelection()

    End Sub

    Protected Sub btSingleLeft_Click(sender As Object, e As System.EventArgs) Handles btSingleLeft.Click

        If lbT.Items.Count = 0 Then Return
        If lbT.SelectedItem Is Nothing Then Return

        lbF.Items.Add(lbT.SelectedItem)
        lbT.Items.RemoveAt(lbT.SelectedIndex)

        lbT.ClearSelection()
        lbF.ClearSelection()

        Dim l = lbF.Items.OfType(Of ListItem).OrderBy(Function(x) x.Text).ToList()
        lbF.Items.Clear()

        For Each li As ListItem In l
            lbF.Items.Add(li)
        Next
    End Sub

    Protected Sub btMultipleRight_Click(sender As Object, e As System.EventArgs) Handles btMultipleRight.Click
        If lbF.Items.Count = 0 Then Return

        Dim l = lbF.Items.OfType(Of ListItem).Concat(lbT.Items.OfType(Of ListItem).OrderBy(Function(x) x.Text).AsEnumerable()).OrderBy(Function(x) x.Text).ToList()
        lbF.Items.Clear()
        lbT.Items.Clear()

        For Each li As ListItem In l
            lbT.Items.Add(li)
        Next

        lbT.ClearSelection()
        lbF.ClearSelection()
    End Sub

    Protected Sub btMultipleLeft_Click(sender As Object, e As System.EventArgs) Handles btMultipleLeft.Click

        If lbT.Items.Count = 0 Then Return

        Dim l = lbT.Items.OfType(Of ListItem).Concat(lbF.Items.OfType(Of ListItem).OrderBy(Function(x) x.Text).AsEnumerable()).OrderBy(Function(x) x.Text).ToList()
        lbF.Items.Clear()
        lbT.Items.Clear()

        For Each li As ListItem In l
            lbF.Items.Add(li)
        Next

        lbT.ClearSelection()
        lbF.ClearSelection()


    End Sub
End Class
