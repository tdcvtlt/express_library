Imports System.Data
Imports System.Data.SqlClient
Imports System.Data.Linq
Imports System.Linq
Imports Microsoft.VisualBasic

Partial Class Reports_Tours_FutureTours
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            Using cn = New SqlConnection(Resources.Resource.cns)

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

                    Catch ex As Exception
                        Response.Write(ex.Message)
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using

            siTourStatus.Connection_String = Resources.Resource.cns
            siTourStatus.Label_Caption = ""
            siTourStatus.ComboItem = "TourStatus"
            siTourStatus.Load_Items()

        End If
    End Sub

    Protected Sub btRunReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btRunReport.Click
        If String.IsNullOrEmpty(ucStartDate.Selected_Date) Or _
           String.IsNullOrEmpty(ucEndDate.Selected_Date) Then Return

        Dim ddCampaign_sel() As String

        If ddCampaign.SelectedItem.Text.Trim().Contains("All") = True Then
            ddCampaign_sel = ddCampaign.Items.OfType(Of ListItem).Where(Function(x) x.Text.Equals("All") = False).Select(Function(x) x.Value).ToArray()
        Else
            ddCampaign_sel = ddCampaign.Items.OfType(Of ListItem).Where(Function(x) x.Value.Equals(ddCampaign.SelectedValue)).Select(Function(x) x.Value).ToArray()
        End If

        Dim sql = String.Format("Select distinct a.*, room.RoomNumber, CAST(r.CheckOutDate as varchar(11)) [Departure Date], (select ComboItem from t_ComboItems where ComboItemID = r.StatusID) [Reservation Status],ISNULL(room.phone, '') as Extension, ISNULL(b.Name, '')[Name], b.CampaignID " & _
                "from (Select t.tourid, t.ReservationID, t.statusid, coalesce(cast((select comboitem from t_comboitems where comboitemid = t.tourtime) as varchar(50)), 0) as TourTime, 	" & _
             "case when (select COUNT(*) from v_contractinventory c where c.ProspectID = t.ProspectID) > 0 then 'Y' else 'N' end IsOwner, " & _
             "case when convert(smallint,(select comboitem from t_comboitems where comboitemid = t.tourtime)) >= 1200 and CONVERT(smallint, t.tourtime) < 1300 then CAST(t.TourTime as varchar(max)) + ' PM' " & _
             "when convert(smallint, (select comboitem from t_comboitems where comboitemid = t.tourtime)) > 1259 then CAST(CONVERT(smallint, (select comboitem from t_comboitems where comboitemid = t.tourtime)) - 1200 as varchar(max)) + ' PM' " & _
             "else CAST((select comboitem from t_comboitems where comboitemid = t.tourtime) as varchar(max)) + ' AM' " & _
             "end as tourTimeFormatted, " & _
             "(Select top 1 p.firstname from t_Event e " & _
              "left outer join t_Personnel p on p.personnelid = e.createdbyid " & _
              "where e.keyfield = 'TourID' and e.KeyValue = t.TourID  and " & _
              "(e.fieldname='tourdate' or e.fieldname = 'wave') order by e.datecreated desc)  waveFName, " & _
             "(Select top 1 p.LastName from t_Event e left outer join t_Personnel p on p.personnelid = e.createdbyid " & _
              "where e.keyfield = 'TourID' and e.KeyValue = t.TourID  and (e.fieldname='tourdate' or e.fieldname = 'wave') order by e.datecreated desc)  waveLName, " & _
             "(Select top 1 e.DateCreated from t_Event e left outer join t_Personnel p on p.personnelid = e.createdbyid where e.keyfield = 'TourID' and e.KeyValue = t.TourID  and " & _
              "(e.fieldname='tourdate' or e.fieldname = 'wave') order by e.datecreated desc)  waveDateCreated, " & _
             "(Select top 1 p.firstname from t_Event e left outer join t_Personnel p on p.personnelid = e.createdbyid where e.keyfield = 'TourID' and e.KeyValue = t.TourID  and " & _
              "e.type = 'Create' and (e.subtype is null or e.subtype = '' or e.subtype='tour') order by e.datecreated desc)  createFName, " & _
              "(Select top 1 p.LastName from t_Event e left outer join t_Personnel p on p.personnelid = e.createdbyid where e.keyfield = 'TourID' and e.KeyValue = t.TourID  and " & _
              "e.type = 'Create' and (e.subtype is null or e.subtype = '' or e.subtype='tour') order by e.datecreated desc)  createLName, " & _
              "(Select top 1 e.DateCreated from t_Event e left outer join t_Personnel p on p.personnelid = e.createdbyid where e.keyfield = 'TourID' and e.KeyValue = t.TourID  and " & _
              "e.type = 'Create' and (e.subtype is null or e.subtype = '' or e.subtype='tour') order by e.datecreated desc)  createDateCreated, " & _
             "t.tourdate, p.FirstName, coalesce(p.lastname, '')[LastName], t.CampaignID from t_Tour t left outer join t_Prospect p on t.prospectid = p.prospectid " & _
            "where tourdate between '{0}' and '{1}' and tourLocationid = '{2}' and t.statusid = (select ComboItemID from t_ComboItems ci inner join t_Combos c on c.ComboID = ci.ComboID where c.ComboName = 'tourstatus' and ci.ComboItem = '{4}')) a inner join t_Campaign b on a.CampaignID = b.CampaignID " & _
            "left outer join t_reservations r on r.reservationid = a.reservationid left outer join t_roomallocationmatrix x on x.reservationid = " & _
            "r.reservationid left outer join t_room room on room.roomid = x.roomid where b.CampaignID in ({3}) order by a.tourdate, a.TourTime, a.LastName", ucStartDate.Selected_Date, ucEndDate.Selected_Date, ddLocation.SelectedValue, String.Join(",", ddCampaign_sel), New clsComboItems().Lookup_ComboItem(siTourStatus.Selected_ID))

        Using cn = New SqlConnection(Resources.Resource.cns)
            'cn.CommandTimeout = 0
            Using ad = New SqlDataAdapter(sql, cn)
                Try
                    Dim dt = New DataTable(), sb As New StringBuilder(), table As String = String.Empty
                    ad.Fill(dt)
                    Dim grpTourDate = dt.Rows.OfType(Of DataRow).GroupBy(Function(x) New DateTime(DateTime.Parse(x("tourdate").ToString()).Year, DateTime.Parse(x("tourdate").ToString()).Month, DateTime.Parse(x("tourdate").ToString()).Day)).OrderBy(Function(x) x.Key)

                    For Each g In grpTourDate
                        table = "<br/><h2>" & g.Key.ToShortDateString() & "</h2><br/><table style='border-collapse:collapse;' border='1px'><tr><th>Campaign</th><th>Tour ID</th><th>Guest</th><th>Departure Date</th><th>Extension</th><th>Tour Time</th><th>Tour Date/Wave Last Modified On</th><th>Room Number</th><th>Reservation Status</th><th>Gifts</th><th>Is Ower?</th><th>Remark</th><th>Comments</th></tr>"
                        sb.Append(table)
                        For Each dr As DataRow In g
                            Dim sButton = "<input type='button' value = 'Add Comment' onclick="
                            sButton &= Chr(34) & "javascript:modal.mwindow.open('" & Request.ApplicationPath & "/general/editnote.aspx?noteid=0&KeyField=Comments&keyvalue=" & dr.Item("TourID") & "&linkid=','win01',350,350);" & Chr(34) & " />"
                            sButton &= "<input type='button' value = 'View Comments' onclick="
                            sButton &= Chr(34) & "javascript:modal.mwindow.open('" & Request.ApplicationPath & "/general/viewcomments.aspx?keyfield=Comments&keyvalue=" & dr.Item("TourID") & "','win01',350,350);" & Chr(34) & " />"

                            sb.Append("<tr>")
                            sb.AppendFormat("<td>{0}</td>", dr("name"))
                            sb.AppendFormat("<td class=clickable>{0}</td>", dr("TourID").ToString())
                            sb.AppendFormat("<td>{0}, {1}</td>", dr.Item("LastName").ToString(), dr.Item("FirstName"))
                            sb.AppendFormat("<td>{0}</td>", dr.Item("Departure Date").ToString())
                            sb.AppendFormat("<td>{0}</td>", dr.Item("Extension").ToString())
                            sb.AppendFormat("<td>{0}</td>", dr.Item("tourTimeFormatted"))

                            Dim modified_by = String.Empty
                            Dim modified_on = DateTime.MaxValue

                            If DateTime.TryParse(dr("waveDateCreated").ToString(), modified_on) = False Then
                                modified_by = String.Format("{0}, {1}", dr("createLName"), dr("createFName"))
                                DateTime.TryParse(dr("createDateCreated").ToString(), modified_on)
                            Else
                                modified_by = String.Format("{0}, {1}", dr("waveLName"), dr("waveFName"))
                            End If

                            If dr.Item("TourDate").ToString().Length > 0 Then
                                sb.AppendFormat("<td>{0}</td>", Convert.ToDateTime(dr.Item("TourDate").ToString()).ToShortDateString())
                            End If



                            sb.AppendFormat("<td>{0}</td>", dr.Item("RoomNumber").ToString())
                            sb.AppendFormat("<td>{0}</td>", dr.Item("Reservation Status").ToString())

                            Using cm = New SqlCommand(String.Format("select p.PremiumName, pis.ComboItem [Status] from t_premium p inner join t_premiumissued pi on p.premiumid = pi.premiumid inner join t_ComboItems pis on pis.ComboItemID = pi.StatusID where pi.keyfield = 'tourid' and pi.KeyValue = {0}", dr("TourID").ToString()), cn)
                                If cn.State = ConnectionState.Closed Then cn.Open()

                                Dim dt2 = New DataTable
                                dt2.Load(cm.ExecuteReader())
                                Dim html = String.Empty
                                For Each dr2 As DataRow In dt2.Rows
                                    html += String.Format("{0} ({1})<br/>", dr2("PremiumName").ToString(), dr2("Status").ToString())
                                Next
                                If dt2.Rows.Count = 0 Then
                                    sb.AppendFormat("<td>N/A</td>")
                                Else
                                    sb.AppendFormat("<td>{0}</td>", html)
                                End If

                                cn.Close()
                            End Using


                            If dr("isOwner").ToString().ToLower().Equals("y") Then
                                sb.AppendFormat("<td style=text-align:center;><span style=font-weight:bold;color:red;>{0}</span></td>", dr("isOwner").ToString())
                            Else
                                sb.AppendFormat("<td>{0}</td>", "")
                            End If

                            sb.AppendFormat("<td>{0}</td>", String.Empty)
                            sb.AppendFormat("<td>{0}</td>", sButton)

                            sb.Append("</tr>")
                        Next

                        sb.Append("</table><br/>")
                    Next

                    Lit1.Text = sb.ToString()
                Catch ex As Exception
                    Response.Write(ex.Message)
                Finally
                    cn.Close()
                End Try
            End Using
        End Using
    End Sub

    Protected Sub btPrintable_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btPrintable.Click
        ClientScript.RegisterClientScriptBlock(Me.GetType, "Printable", "var mwin = window.open();mwin.document.write('" & Replace(Lit1.Text, "'", "\'") & "');", True)
    End Sub

    Protected Sub btExportToExcel_Click(sender As Object, e As System.EventArgs) Handles btExportToExcel.Click
        Response.Clear()
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", String.Format("attachment; filename=FutureToursReport_{0}_{1}.xls", DateTime.Now.ToLongTimeString(), DateTime.Now.Millisecond))
        Response.Write(Lit1.Text)
        Response.End()
    End Sub
End Class
