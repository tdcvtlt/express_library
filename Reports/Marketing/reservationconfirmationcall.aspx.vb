Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.Services
Imports System.Web.Script.Serialization


Partial Class marketing_reservationconfirmationcall
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then
            btn_report_run.Attributes.Add("UserDBID", Session("UserDBID"))

            Using cn = New SqlConnection(Resources.Resource.cns)
                Dim sqlText = String.Format( _
                    "select * from t_ComboItems ci inner join t_Combos c on ci.ComboID = c.ComboID where c.ComboName	= 'resortcompany' and ci.Active=1 order by ci.comboitem;" & _
                    "select * from t_ComboItems ci inner join t_Combos c on ci.ComboID = c.ComboID where c.ComboName	= 'reservationtype' and ci.Active=1 order by ci.comboitem;" & _
                    "select * from t_ComboItems ci inner join t_Combos c on ci.ComboID = c.ComboID where c.ComboName	= 'reservationstatus' and ci.Active=1 order by ci.comboitem")

                Using ad = New SqlDataAdapter(sqlText, cn)
                    Try

                        Dim ds = New DataSet()
                        ad.Fill(ds)

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

                    Catch ex As Exception
                        Response.Write(ex.Message)
                    End Try
                End Using

            End Using
        End If
        
    End Sub

    Protected Sub btn_report_run_Click(sender As Object, e As System.EventArgs) Handles btn_report_run.Click
        Dim sb = New StringBuilder()
        Dim date_checkin = dteSDate.Selected_Date
        Dim date_checkou = dteEDate.Selected_Date

        If date_checkin.ToString().Length = 0 Or date_checkou.ToString().Length = 0 Then Return

        Dim resorts = From r As ListItem In cbl_resort.Items.OfType(Of ListItem).Where(Function(x) x.Selected And x.Value <> "-1")
        Dim statuses = From r As ListItem In cbl_status.Items.OfType(Of ListItem).Where(Function(x) x.Selected And x.Value <> "-1")
        Dim types = From r As ListItem In cbl_type.Items.OfType(Of ListItem).Where(Function(x) x.Selected And x.Value <> "-1")

        Dim where = String.Empty        
        Dim days As Int16 = DateTime.Parse(date_checkou).Subtract(DateTime.Parse(date_checkin)).Days

        If resorts.Count() = 0 And statuses.Count() = 0 And types.Count() = 0 Then
            where = String.Format("where (r.resortcompanyid is null or r.resortcompanyid = 0) and (r.statusid = 0 or r.statusid is null) and (r.typeid = 0 or r.typeid is null) and checkindate between '{0}' and dateadd(d, {1}, '{0}') ", date_checkin, days)

        ElseIf resorts.Count() > 0 And statuses.Count() > 0 And types.Count() > 0 Then

            where = String.Format("where r.statusid in ({0}) and r.resortcompanyid in ({1}) and r.typeid in ({2}) and checkindate between '{3}' and dateadd(d, {4}, '{3}') ", _
                         String.Join(",", statuses.Select(Function(x) x.Value).ToArray()), _
                         String.Join(",", resorts.Select(Function(x) x.Value).ToArray()), _
                         String.Join(",", types.Select(Function(x) x.Value).ToArray()), _
                         date_checkin, days)

        ElseIf resorts.Count() > 0 And statuses.Count() = 0 And types.Count() = 0 Then

            where = String.Format("where r.resortcompanyid in ({0}) and (r.statusid = 0 or r.statusid is null) and (r.typeid = 0 or r.typeid is null) and checkindate between '{1}' and dateadd(d, {2}, '{1}') ", _
                           String.Join(",", resorts.Select(Function(x) x.Value).ToArray()), date_checkin, days)


        ElseIf resorts.Count() > 0 And statuses.Count() > 0 And types.Count() = 0 Then

            where = String.Format("where r.resortcompanyid in ({0}) and r.statusid in ({1}) and (r.typeid = 0 or r.typeid is null) and checkindate between '{2}' and dateadd(d, {3}, '{2}') ", _
                    String.Join(",", resorts.Select(Function(x) x.Value).ToArray()), _
                    String.Join(",", statuses.Select(Function(x) x.Value).ToArray()), date_checkin, days)


        ElseIf resorts.Count() = 0 And statuses.Count() > 0 And types.Count() = 0 Then
            where = String.Format("where (r.resortcompanyid is null or r.resortcompanyid = 0) and r.statusid in ({0}) and (r.typeid = 0 or r.typeid is null) and checkindate between '{1}' and dateadd(d, {2}, '{1}') ", _
                           String.Join(",", statuses.Select(Function(x) x.Value).ToArray()), date_checkin, days)


        ElseIf resorts.Count() = 0 And statuses.Count() > 0 And types.Count() > 0 Then

            where = String.Format("where (r.resortcompanyid is null or r.resortcompanyid = 0) and r.statusid in ({0}) and r.typeid in ({1}) and checkindate between '{2}' and dateadd(d, {3}, '{2}') ", _
                                  String.Join(",", statuses.Select(Function(x) x.Value).ToArray()), _
                                  String.Join(",", types.Select(Function(x) x.Value).ToArray()), date_checkin, days)

        ElseIf resorts.Count() > 0 And statuses.Count() = 0 And types.Count() > 0 Then

            where = String.Format("where r.resortcompanyid in ({0}) and (r.statusid = 0 or r.statusid is null) and r.typeid in ({1}) and checkindate between '{2}' and dateadd(d, {3}, '{2}') ", _
                                  String.Join(",", resorts.Select(Function(x) x.Value).ToArray()), _
                                  String.Join(",", types.Select(Function(x) x.Value).ToArray()), date_checkin, days)

        ElseIf resorts.Count() = 0 And statuses.Count() = 0 And types.Count() > 0 Then

            where = String.Format("where (r.resortcompanyid is null or r.resortcompanyid = 0) and (r.statusid = 0 or r.statusid is null) and r.typeid in ({0}) and checkindate between '{1}' and dateadd(d, {2}, '{1}') ", _
                             String.Join(",", types.Select(Function(x) x.Value).ToArray()), date_checkin, days)
        End If

	Dim absolutePath = Request.Url.AbsolutePath
        If absolutePath.ToLower().IndexOf("crmsnet") > 0 Then
            where += "and (cs.comboitem not in ('confirmed', 'removed') or cs.comboitem is null) and ((pa.TypeID NOT IN (select ci.ComboItemID from t_ComboItems ci inner join t_Combos c " & _
                            "on ci.ComboID = c.ComboID where c.ComboName = 'PackageType' and " & _
                            "ci.ComboItem in ('Tradeshow', 'Tour Package'))) or r.PackageIssuedID = 0) "
        ElseIf absolutePath.ToLower().IndexOf("kcpcrms") > 0 Then
            where += "and pa.TypeID in (select ci.ComboItemID from t_ComboItems ci inner join t_Combos c " & _
                            "on ci.ComboID = c.ComboID where c.ComboName = 'PackageType' and " & _
                            "ci.ComboItem in ('Tradeshow', 'Tour Package')) "

        End If
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
            "left join t_PackageIssued pi on r.PackageIssuedID = pi.PackageIssuedID " & _
            "left join t_Package pa on pa.PackageID = pi.PackageID " & _
            where & _
            "and (cs.comboitem not in ('confirmed', 'removed') or cs.comboitem is null ) " & _
            "order by r.ReservationID desc")

        If Session("UserDBID") = 8227 Then
            Response.Write(sqlText)
 
	
        End If


        Using cn = New SqlConnection(Resources.Resource.cns)
            Using ad = New SqlDataAdapter(sqlText, cn)
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
                        sb.Append("<td align=center><strong>confirmed</strong></td><td align=center><strong>left v/m</strong></td><td align=center><strong>n/a</strong></td><td align=center><strong>remove</strong></td><td align=left><strong>last attempt</strong></td><td align=left><strong>additional notes</strong></td><td>&nbsp;</td>")
                        sb.Append("</tr>")
                        sb.Append("<tr class=action-row>")


                        sb.AppendFormat("<td align=center style={1}><input type=radio name=group-check-{0} id={0}-cm /></td>", id_reservation.ToString(), "width:100px;")
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

        For i = 0 To 100
           
        Next
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
                    End If

                    Using cm = New SqlCommand(sqlText, cn)
                        If cn.State = ConnectionState.Closed Then cn.Open()

                        dt.Rows(0).SetField(Of Int32)("confirmationstatusid", CType(cm.ExecuteScalar(), Int32))

                        cn.Close()

                        rec_affected = ad.Update(dt)

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

    
End Class
