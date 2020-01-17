Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.Services
Imports System.Web.Script.Serialization
Imports ClosedXML.Excel

Partial Class marketing_reservationconfirmationcall
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then
            cbl_resort.DataSource = New clsComboItems().Load_ComboItems("resortcompany")
            cbl_resort.AppendDataBoundItems = True
            cbl_resort.Items.Add(New ListItem("All", "-1"))
            cbl_resort.DataTextField = "comboitem"
            cbl_resort.DataValueField = "comboitemid"
            cbl_resort.DataBind()

            cbl_type.DataSource = New clsComboItems().Load_ComboItems("reservationtype")
            cbl_type.AppendDataBoundItems = True
            cbl_type.Items.Add(New ListItem("All", "-1"))
            cbl_type.DataTextField = "comboitem"
            cbl_type.DataValueField = "comboitemid"
            cbl_type.DataBind()

            cbl_status.DataSource = New clsComboItems().Load_ComboItems("reservationstatus")
            cbl_status.AppendDataBoundItems = True
            cbl_status.Items.Add(New ListItem("All", "-1"))
            cbl_status.DataTextField = "comboitem"
            cbl_status.DataValueField = "comboitemid"
            cbl_status.DataBind()
        End If

        lit_report.Text = ""
        lbError.Text = ""


        If CType(Session("User"), User).PersonnelID > 0 Then
            btn_report_run.Attributes.Add("UserDBID", CType(Session("User"), User).PersonnelID)
        Else
            btn_report_run.Attributes.Add("UserDBID", Session.Item("UserDBID"))
        End If
        
    End Sub

    Protected Sub btn_report_run_Click(sender As Object, e As System.EventArgs) Handles btn_report_run.Click
        Dim sb = New StringBuilder()
        Dim date_checkin = dteSDate.Selected_Date
        Dim date_checkou = dteEDate.Selected_Date



        If date_checkin.ToString().Length = 0 Or date_checkou.ToString().Length = 0 Then Return

        Dim resorts = (From r As ListItem In cbl_resort.Items.OfType(Of ListItem).Where(Function(x) x.Selected And x.Value <> "-1")).ToList()
        Dim statuses = (From r As ListItem In cbl_status.Items.OfType(Of ListItem).Where(Function(x) x.Selected And x.Value <> "-1")).ToList()
        Dim types = (From r As ListItem In cbl_type.Items.OfType(Of ListItem).Where(Function(x) x.Selected And x.Value <> "-1")).ToList()

        Dim where = String.Empty        
        Dim days As Int16 = DateTime.Parse(date_checkou).Subtract(DateTime.Parse(date_checkin)).Days

        If resorts.Count() = 0 Then
            lbError.Text = "Please select at least a resort company."
            Return
        End If

        Dim Kcp = (From li As ListItem In resorts.Where(Function(x) x.Text = "KCP")).SingleOrDefault()
            
        sb.Append("<table id=""report-table"">")

        Dim sqlText = String.Format( _
            "select p.FirstName + ' ' + p.LastName [prospect-name], " & _
            "(select COUNT(*) from t_Tour t inner join t_ComboItems ci on ci.ComboItemID = t.TourLocationID inner join t_ComboItems ts on ts.ComboItemID = t.StatusID  " & _
            "inner join t_ComboItems tss on tss.ComboItemID = t.SubTypeID where ts.ComboItem = 'showed' and tss.ComboItem not in ('exit') and ci.ComboItem in ('kcp', 'richmond', 'woodbridge') and t.ProspectID = p.ProspectID) [tourlocation-count], " & _
            "r.reservationid [reservation-id], " & _
            "rc.comboitem [resort-company], " & _
            "convert(varchar(10), r.checkindate, 101) [date-checkin], " & _
            "DATEDIFF(d, r.CheckInDate, r.CheckOutDate) [nights], " & _
            "rs.ComboItem [reservation-status], " & _
            "rl.ComboItem [reservation-location], " & _
            "ts.ComboItem [tour-status], " & _
            "s.comboitem [reservation-source], " & _
            "convert(varchar(10), t.TourDate, 101) [tour-date], " & _
            "tw.ComboItem [tour-wave], " & _
            "(select top 1 b.accomName from t_accommodation a inner join t_accom b on a.accomid = b.accomid  " & _
            "where a.reservationid = r.reservationid order by b.accomid desc) [accommodation], " & _
            "(select top 1 note from t_note where KeyField = 'reservationconfirmationstatusid' and keyvalue = r.ReservationID order by noteid desc) [note], " & _
            "(select top 1 DateCreated from t_note where KeyField = 'reservationconfirmationstatusid' and keyvalue = r.ReservationID order by noteid desc) [last-attempt] " & _
            "from t_Reservations r inner join t_Prospect p  " & _
            "on r.ProspectID = p.ProspectID " & _
            "left join t_Tour t on r.TourID = t.TourID " & _
            "left join t_ComboItems rs on rs.ComboItemID = r.StatusID  " & _
            "left join t_ComboItems rl on rl.ComboItemID = r.ResLocationID " & _
            "left join t_ComboItems ts on ts.ComboItemID = t.StatusID " & _
            "left join t_ComboItems tw on tw.ComboItemID = t.TourTime " & _
            "left join t_ComboItems rc on rc.ComboItemID = r.ResortCompanyID " & _
            "left join t_comboitems cs on cs.comboitemid = r.confirmationstatusid " & _
            "left join t_comboitems s on s.comboitemid = r.sourceid " & _
            "left join t_PackageIssued pi on pi.PackageIssuedID = r.PackageIssuedID " & _
            "left join t_Package pa on pa.PackageID = pi.PackageID "  & _
			"left join t_comboitems pt on pt.comboitemid = pa.typeid ")

        Dim absolutePath = Request.Url.AbsolutePath, sql = String.Empty
        If absolutePath.ToLower().IndexOf("crmsnet") > 0 Then

            If Not Kcp Is Nothing And resorts.Count = 1 Then
                sql = sqlText + sqlResortKcpWhere()
            ElseIf Kcp Is Nothing Then
                sql = sqlText + sqlResortNonKcpWhere()
            Else
                sql = sqlText + sqlResortNonKcpWhere()
                sql += " union all " + sqlText + sqlResortKcpWhere()
            End If

        ElseIf absolutePath.ToLower().IndexOf("kcpcrms") > 0 Then

                If Not Kcp Is Nothing Then
                sql = sqlText + sqlResortKcpWhere()
            Else
                lbError.Text = "Please select KCP as resort company."
            End If
        End If

        If Session("UserDBID") = 8227  Then
            Response.Write(sql)
        End If


        Using cn = New SqlConnection(Resources.Resource.cns)
            Using ad = New SqlDataAdapter(sql, cn)
                Dim dt = New DataTable()

                Try
                    ad.Fill(dt)

                    If dt.Rows.Count > 0 Then

                        sb.AppendFormat("<thead><tr>")
                        sb.AppendFormat("<th><strong>reservation</strong></th>")
                        sb.AppendFormat("<th><strong>prospect</strong></th>")
                        sb.AppendFormat("<th><strong>resort <br/>company</strong></th>")
                        sb.AppendFormat("<th><strong>reservation<br/>location</strong></th>")
                        sb.AppendFormat("<th><strong>date check-in</strong></th>")
                        sb.AppendFormat("<th><strong>nights</strong></th>")
                        sb.AppendFormat("<th><strong>accommodation</strong></th>")
                        sb.AppendFormat("<th><strong>reservation <br/>status</strong></th>")
                        sb.AppendFormat("<th><strong>source</strong></th>")
                        sb.AppendFormat("<th><strong>KCP/R/W <br/>Counts</strong></th>")
                        sb.AppendFormat("<th><strong>tour <br/>status</strong></th>")
                        sb.AppendFormat("<th><strong>tour <br/>date</strong></th>")
                        sb.AppendFormat("<th><strong>tour <br/>wave</strong></th>")

                        sb.AppendFormat("</tr></thead>")
                    End If


                    Dim row_count = 1

                    For Each dr As DataRow In dt.Rows

                        Dim id_reservation As Int32 = dr("reservation-id").ToString()

                        sb.Append("<tr class=data-row>")
                        sb.Append("<td class=clickable>" & dr("reservation-id").ToString() & "</td>")
                        sb.Append("<td>" & dr("prospect-name").ToString() & "</td>")
                        sb.Append("<td>" & dr("resort-company").ToString() & "</td>")
                        sb.Append("<td>" & dr("reservation-location").ToString() & "</td>")
                        sb.Append("<td>" & dr("date-checkin").ToString() & "</td>")
                        sb.Append("<td>" & dr("nights").ToString() & "</td>")
                        sb.Append("<td>" & dr("accommodation").ToString() & "</td>")
                        sb.Append("<td>" & dr("reservation-status").ToString() & "</td>")
                        sb.Append("<td>" & dr("reservation-source").ToString() & "</td>")
                        sb.Append("<td style=text-align:center>" & dr("tourlocation-count").ToString() & "</td>")
                        sb.Append("<td>" & dr("tour-status").ToString() & "</td>")
                        sb.Append("<td>" & dr("tour-date").ToString() & "</td>")
                        sb.Append("<td>" & dr("tour-wave").ToString() & "</td>")

                        sb.Append("</tr>")

                        sb.Append("<tr><td colspan=9>")

                        sb.Append("<div><table>")
                        sb.Append("<tr class=action-row>")
                        sb.Append("<td align=center><strong>confirmed</strong></td><td align=center><strong>confirmed by Email</strong></td><td align=center><strong>left v/m</strong></td><td align=center><strong>n/a</strong></td><td align=center><strong>remove</strong></td><td align=left><strong>last attempt</strong></td><td align=left><strong>additional notes</strong></td><td>&nbsp;</td>")
                        sb.Append("</tr>")
                        sb.Append("<tr class=action-row>")


                        sb.AppendFormat("<td align=center style={1}><input type=radio name=group-check-{0} id={0}-cm /></td>", id_reservation.ToString(), "width:100px;")
                        sb.AppendFormat("<td align=center style={1}><input type=radio name=group-check-{0} id={0}-em /></td>", id_reservation.ToString(), "width:100px;")                        
                        sb.AppendFormat("<td align=center style={1}><input type=radio name=group-check-{0} id={0}-vm /></td>", id_reservation.ToString(), "width:100px;")
                        sb.AppendFormat("<td align=center style={1}><input type=radio name=group-check-{0} id={0}-na /></td>", id_reservation.ToString(), "width:100px;")
                        sb.AppendFormat("<td align=center style={1}><input type=radio name=group-check-{0} id={0}-rm /></td>", id_reservation.ToString(), "width:100px;")
                        sb.AppendFormat("<td align=left style={0}><label><strong>{1}</strong></label></td>", "width:100px;", dr("last-attempt").ToString())
                        sb.AppendFormat("<td align=left style={0} colspan=2><input type=text value='{1}' size=40 /></td>", "width:100px;", dr("note").ToString())

                        sb.Append("</tr>")

                        sb.Append("</table></div>")
                        sb.Append("</td><td class=pad-background></td><td class=pad-background></td><td class=pad-background></td></tr>")

                        row_count += 1
                    Next

                Catch ex As Exception
                    Response.Write(ex.Message)
                End Try

            End Using

        End Using

        sb.Append("</table>")

        lit_report.Text = sb.ToString()

    End Sub

    Protected Sub btn_report_refresh_Click(sender As Object, e As System.EventArgs) Handles btn_report_refresh.Click
        btn_report_run_Click(Nothing, e)
    End Sub

    <WebMethod()> _
    Public Shared Function update(id_r As Int32, param As String, note As String, UserDBID As String) As Int16
        Dim rec_affected = -1

        Dim sqlText = String.Format("select * from t_reservations where reservationid = {0}", id_r)

        Using cn = New SqlConnection(Resources.Resource.cns)
            Using ad = New SqlDataAdapter(sqlText, cn)
                Dim sb = New SqlCommandBuilder(ad)
                Dim dt = New DataTable()

                Try
                    ad.Fill(dt)

                    If param = "cm" Then
                        note = "Reservation has been confirmed"
                        sqlText = "select comboitemid from t_comboitems ci inner join t_combos c on ci.comboid = c.comboid where c.comboname = 'ReservationConfirmationStatus' and ci.comboitem = 'confirmed'"
                    ElseIf param = "rm" Then
                        sqlText = "select comboitemid from t_comboitems ci inner join t_combos c on ci.comboid = c.comboid where c.comboname = 'ReservationConfirmationStatus' and ci.comboitem = 'removed'"
                    ElseIf param = "na" Then
                        sqlText = "select comboitemid from t_comboitems ci inner join t_combos c on ci.comboid = c.comboid where c.comboname = 'ReservationConfirmationStatus' and ci.comboitem = 'n/a'"
                    ElseIf param = "vm" Then
                        note = "Left V/M to confirm reservation"
                        sqlText = "select comboitemid from t_comboitems ci inner join t_combos c on ci.comboid = c.comboid where c.comboname = 'ReservationConfirmationStatus' and ci.comboitem = 'left vm'"
                    ElseIf param = "em" Then
                        note = "Guest confirmed stay thru email"
                        sqlText = "select comboitemid from t_comboitems ci inner join t_combos c on ci.comboid = c.comboid where c.comboname = 'ReservationConfirmationStatus' and ci.comboitem = 'Confirmed by Email'"                        
                    End If

                    Using cm = New SqlCommand(sqlText, cn)
                        If cn.State = ConnectionState.Closed Then cn.Open()

                        dt.Rows(0).SetField(Of Int32)("confirmationstatusid", CType(cm.ExecuteScalar(), Int32))

                        cn.Close()

                        rec_affected = ad.Update(dt)
                        If String.IsNullOrEmpty(UserDBID) Or UserDBID = "undefined" Then UserDBID = 8227

                        ad.SelectCommand.CommandText = String.Format( _
                            "insert into t_note (keyfield, keyvalue, note, datecreated, createdbyid) " & _
                            "values ('reservationconfirmationstatusid', {0}, '{1}', getdate(), {2})", id_r, note, userDBid)

                        ad.SelectCommand.Connection.Open()
                        rec_affected += ad.SelectCommand.ExecuteNonQuery()
                        ad.SelectCommand.Connection.Close()

                    End Using

                Catch ex As Exception
                    Return ex.Message
                End Try
            End Using
        End Using

        Return rec_affected
    End Function

    Private Function sqlResortKcpWhere() As String

        Dim date_checkin = dteSDate.Selected_Date, date_checkou = dteEDate.Selected_Date, days As Int16 = DateTime.Parse(date_checkou).Subtract(DateTime.Parse(date_checkin)).Days
        Dim resorts = (From r As ListItem In cbl_resort.Items.OfType(Of ListItem).Where(Function(x) x.Selected And x.Value <> "-1")).ToList()
        Dim statuses = (From r As ListItem In cbl_status.Items.OfType(Of ListItem).Where(Function(x) x.Selected And x.Value <> "-1")).ToList()
        Dim types = (From r As ListItem In cbl_type.Items.OfType(Of ListItem).Where(Function(x) x.Selected And x.Value <> "-1")).ToList()

        Dim Kcp = (From li As ListItem In resorts.Where(Function(x) x.Text = "KCP")).SingleOrDefault()

        Dim sql = String.Empty

        Dim typeWhere = "", statusWhere = "", resortWhere = ""

        If statuses.Count > 0 Then
            statusWhere = String.Format("and r.statusid in ({0})", String.Join(",", statuses.Select(Function(x) x.Value).ToArray()))
        Else
            statusWhere = "and r.statusid = 0"
        End If

        If types.Count > 0 Then
            typeWhere = String.Format("and r.typeid in ({0})", String.Join(",", types.Select(Function(x) x.Value).ToArray()))
        Else
            typeWhere = "and r.typeid = 0"
        End If

        If resorts.Count > 0 Then
            resortWhere = String.Format("r.resortcompanyid in ({0})", String.Join(",", resorts.Where(Function(x) x.Text = "KCP").Select(Function(x) x.Value).ToArray()))
        End If


        Dim absolutePath = Request.Url.AbsolutePath
        If absolutePath.ToLower().IndexOf("crmsnet") > 0 Then
            If resorts.Count() > 0 Then
                sql = String.Format("where {0} {1} {2} and checkindate between '{3}' and dateadd(d, {4}, '{3}') and (pt.comboitem not in ('Tradeshow', 'Tour Package') or pa.typeid is null) and (cs.comboitem not in ('confirmed', 'removed', 'Confirmed by Email') or r.ConfirmationStatusID = 0) ", resortWhere, statusWhere, typeWhere, date_checkin, days)
            End If

        ElseIf absolutePath.ToLower().IndexOf("kcpcrms") > 0 Then

            sql = String.Format("where r.resortcompanyid in ({0}) {1} {2} and checkindate between '{3}' and dateadd(d, {4}, '{3}') and (cs.comboitem not in ('confirmed', 'removed', 'Confirmed by Email') or r.ConfirmationStatusID = 0 ) ", Kcp.Value, statusWhere, typeWhere, date_checkin, days)

            sql += "and (pt.comboitem in ('Tradeshow', 'Tour Package') or pa.typeid is null) and pi.PackageIssuedID is not null " 
                          
            Return sql
        End If
        Return sql
    End Function

    Private Function sqlResortNonKcpWhere() As String

        Dim date_checkin = dteSDate.Selected_Date, date_checkou = dteEDate.Selected_Date, days As Int16 = DateTime.Parse(date_checkou).Subtract(DateTime.Parse(date_checkin)).Days
        Dim resorts = (From r As ListItem In cbl_resort.Items.OfType(Of ListItem).Where(Function(x) x.Selected And x.Value <> "-1")).ToList()
        Dim statuses = (From r As ListItem In cbl_status.Items.OfType(Of ListItem).Where(Function(x) x.Selected And x.Value <> "-1")).ToList()
        Dim types = (From r As ListItem In cbl_type.Items.OfType(Of ListItem).Where(Function(x) x.Selected And x.Value <> "-1")).ToList()

        Dim Kcp = (From li As ListItem In resorts.Where(Function(x) x.Text = "KCP")).SingleOrDefault()

        Dim absolutePath = Request.Url.AbsolutePath, sql = String.Empty
        If absolutePath.ToLower().IndexOf("crmsnet") > 0 Then

            If Not Kcp Is Nothing Then
                resorts.Remove(Kcp)
            End If

            Dim typeWhere = "", statusWhere = "", resortWhere = ""

            If statuses.Count > 0 Then
                statusWhere = String.Format("and r.statusid in ({0})", String.Join(",", statuses.Select(Function(x) x.Value).ToArray()))
            Else
                statusWhere = "and r.statusid = 0"
            End If

            If types.Count > 0 Then
                typeWhere = String.Format("and r.typeid in ({0})", String.Join(",", types.Select(Function(x) x.Value).ToArray()))
            Else
                typeWhere = "and r.typeid = 0"
            End If

            If resorts.Count > 0 Then
                resortWhere = String.Format("r.resortcompanyid in ({0})", String.Join(",", resorts.Select(Function(x) x.Value).ToArray()))
            End If

            sql = String.Format("where {0} {1} {2} and checkindate between '{3}' and dateadd(d, {4}, '{3}') and (cs.comboitem not in ('confirmed', 'removed', 'Confirmed by Email') or r.ConfirmationStatusID = 0) ", _
                    resortWhere, statusWhere, typeWhere, date_checkin, days)

        End If

        Return sql
    End Function
    
End Class
