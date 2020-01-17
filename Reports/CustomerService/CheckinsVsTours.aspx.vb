Imports System
Imports System.Data
Imports System.Data.SqlClient


Partial Class Reports_CustomerService_CheckinsVsTours
    Inherits System.Web.UI.Page

    Private CRMSNET_CONNECTION_STRING As String = Resources.Resource.cns
    Private DATE_CHECK_IN As String
    Private DATE_CHECK_OUT As String

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load


        If IsPostBack = False Then

            Using cnn As New SqlConnection(CRMSNET_CONNECTION_STRING)
                Using ada As New SqlDataAdapter()

                    Dim dt As New DataTable
                    Try

                        ada.SelectCommand = New SqlCommand("select comboItem from t_comboItems where active = 1 and comboid = 272 order by comboItem", cnn)
                        ada.Fill(dt)
                        cbl1.DataSource = dt
                        cbl1.DataTextField = "comboItem"
                        cbl1.DataValueField = "comboItem"
                        cbl1.DataBind()

                        dt.Clear()

                        ada.SelectCommand = New SqlCommand("select comboItem from t_comboItems where active = 1 and comboid = 319 order by comboItem", cnn)
                        ada.Fill(dt)
                        cbl2.DataSource = dt
                        cbl2.DataTextField = "comboItem"
                        cbl2.DataValueField = "comboItem"
                        cbl2.DataBind()

                        dt.Clear()

                    Catch ex As Exception

                        Response.Write(ex.Message)
                    End Try
                End Using

            End Using

        End If

        DATE_CHECK_IN = dteSDate.Selected_Date
        DATE_CHECK_OUT = dteEDate.Selected_Date

    End Sub

    Protected Sub btn_Submit_Click(sender As Object, e As System.EventArgs) Handles btn_Submit.Click


        Dim t_sel = cbl1.Items.OfType(Of ListItem).Where(Function(x) x.Selected = True)
        Dim r_sel = cbl2.Items.OfType(Of ListItem).Where(Function(x) x.Selected = True)

        If t_sel.Count() > 0 And r_sel.Count() > 0 And _
            String.IsNullOrEmpty(DATE_CHECK_IN) = False And _
            String.IsNullOrEmpty(DATE_CHECK_OUT) = False Then

            Dim sq = String.Format( _
                "select (select comboitem from t_ComboItems where ComboItemID = (select typeid from t_Reservations where ReservationID = t.ReservationID)) [reservation_type], " & _
                "(select comboitem from t_ComboItems where ComboItemID = t.StatusID) [tour_status], " & _
                "coalesce((select comboitem from t_ComboItems where ComboItemID = (select c.TypeID from t_Campaign c inner join t_Tour on t.CampaignID = c.CampaignID where TourID = t.TourID)), '')[campaign_type], " & _
                "(select c.Name from t_Campaign c where c.CampaignID = t.CampaignID) [campaign], * " & _
                "from t_Tour t  where t.StatusID in (select ComboItemID from t_ComboItems where ComboID = 272 and Active = 1 and ComboItem in ( {3})) " & _
                "and t.ReservationID in ( " & _
                "select r.ReservationID from  t_Reservations r where r.CheckInDate between '{0}' and '{1}' " & _
                "and r.TypeID in (select ComboItemID from t_ComboItems where ComboID = 319 and Active = 1 and ComboItem in ({2})) " & _
                "and r.StatusID in (select ComboItemID from t_ComboItems where ComboID = 321 and Active = 1 and ComboItem in ( 'Completed', 'in-house')) " & _
                "and r.ResLocationID in (select ComboItemID from t_ComboItems where ComboID = 318 and Active = 1 and ComboItem in ( 'KCP'))) " & _
                "order by reservation_type, tour_status, campaign_type, campaign", _
                DATE_CHECK_IN, DATE_CHECK_OUT, _
                String.Join(",", r_sel.OfType(Of ListItem).Select(Function(x) String.Format("'{0}'", x.Text)).ToArray()), _
                String.Join(",", t_sel.OfType(Of ListItem).Select(Function(x) String.Format("'{0}'", x.Text)).ToArray()))


            Using cnn = New SqlConnection(CRMSNET_CONNECTION_STRING)
                Using ada = New SqlDataAdapter(sq, cnn)

                    ada.SelectCommand.CommandTimeout = 0

                    Dim t As New DataTable()

                    Try
                        ada.Fill(t)
                        Dim sb = New StringBuilder()

                        If t.Rows.Count = 0 Then

                            lit.Text = String.Format("<strong>{0}.</strong>", "No data")
                            Return
                        Else


                            sb.AppendFormat("<table border=0 cellpadding=10 cellspacing=15 style=border-collapse:collapse;>")
                            sb.AppendFormat("<tr><td style=width:200px;><strong style=color:red>Reservation Type</strong></td><td>TOTAL</td><td>NON OPC-OS</td><td>OPC-OS</td>")

                            'Header row
                            For Each td In t_sel.OfType(Of ListItem).Select(Function(x) x.Text).OrderBy(Function(x) x)

                                sb.AppendFormat("<td style=text-align:right><strong>{0}</strong></td>", td.ToUpper())

                            Next
                            sb.AppendFormat("</tr>")



                            'Loop through reservation type
                            'Details row
                            For Each tr In r_sel.OfType(Of ListItem).Select(Function(x) x.Text)

                                Dim reservation_type = tr

                                'sum of OPC-OS                               
                                Dim opcos_sum1 = From dr As DataRow In t.Rows.OfType(Of DataRow)() _
                                                 Where dr.Field(Of String)("reservation_type") = reservation_type _
                                                And dr.Field(Of String)("campaign_type").ToUpper() = "OPC-OS" _
                                                Select dr



                                'sum of NON OPC-OS
                                Dim opcos_sum2 = From dr As DataRow In t.Rows.OfType(Of DataRow)() _
                                                 Where dr.Field(Of String)("reservation_type") = reservation_type _
                                                 And (String.IsNullOrEmpty(dr.Field(Of String)("campaign_type")) = False _
                                                And dr.Field(Of String)("campaign_type").ToUpper().Equals("OPC-OS") = False) _
                                                  Select dr


                                sq = String.Format("select COUNT(r.ReservationID) from t_Reservations r where r.CheckInDate between '{0}' and '{1}' and " & _
                                    "r.TypeID in (select ComboItemID from t_ComboItems where ComboID = 319 and Active = 1 and ComboItem in ('{2}'))  " & _
                                    "and r.StatusID in (select ComboItemID from t_ComboItems where ComboID = 321 and Active = 1 and ComboItem in ( 'Completed', 'in-house')) " & _
                                    "and r.ResLocationID in (select ComboItemID from t_ComboItems where ComboID = 318 and Active = 1 and ComboItem in ( 'KCP'))", DATE_CHECK_IN, DATE_CHECK_OUT, tr)


                                ada.SelectCommand = New SqlCommand(sq, cnn)
                                cnn.Open()
                                Dim reservations_total = DirectCast(ada.SelectCommand.ExecuteScalar(), Integer)
                                cnn.Close()


                                sb.AppendFormat("<tr>")

                                sb.AppendFormat("<td style=width:200px;><strong style=color:blue;>{0}</strong></td>", tr.ToUpper())

                                sb.AppendFormat("<td style=text-align:right>{0}</td><td style=text-align:right>{1}</td><td style=text-align:right>{2}</td>", reservations_total, opcos_sum2.Count(), opcos_sum1.Count())


                                For Each td In t_sel.OfType(Of ListItem).Select(Function(x) x.Text)

                                    Dim tour_status = td.ToUpper()

                                    Dim status_sum = From dr As DataRow In t.Rows.OfType(Of DataRow)() _
                                                        Where dr.Field(Of String)("tour_status").ToUpper() = tour_status _
                                                        And String.IsNullOrEmpty(dr.Field(Of String)("tour_status")) = False _
                                                        And dr.Field(Of String)("reservation_type") = reservation_type _
                                                        Select dr

                                    sb.AppendFormat("<td style=text-align:right><strong>{0}</strong></td>", status_sum.Count())
                                Next

                                sb.AppendFormat("</tr>")
                            Next

                            sb.AppendFormat("</table>")
                        End If

                        lit.Text = sb.ToString()

                    Catch ex As Exception
                        lit.Text = String.Format("<strong>{0}.</strong>", ex.Message)
                    End Try

                End Using

            End Using
        Else
            lit.Text = String.Format("<strong>Criteria selections are not sufficient.</strong>")

        End If
    End Sub

End Class
