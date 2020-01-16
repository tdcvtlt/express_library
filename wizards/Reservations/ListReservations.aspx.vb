Imports System.Reflection
Imports System.Data
Imports System.Data.SqlClient

Imports clsReservationWizard
Imports System.Web.Script.Serialization

Partial Class wizard_Reservations_ListReservations
    Inherits System.Web.UI.Page

    Private package_base As New Base_Package
    Private wiz As New Wizard

#Region "Subs/Functions"


    Private Sub Navigate(bt As Button)
        Dim gt = Me.Master.GetType()
        Dim mi As MethodInfo = gt.GetMethod("Navigate")
        Dim parameters = mi.GetParameters()

        Dim dir = Convert.ToInt32(bt.Attributes("nav"))
        Dim parametersArray() As Object = New Object() {Request.PhysicalPath.Substring(Request.PhysicalPath.LastIndexOf("\") + 1).ToLower(), dir}
        Dim r = mi.Invoke(Me.Master, parametersArray)
        Response.Redirect(r.ToString())
    End Sub

#End Region

#Region "Event Handlers"

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim ph = CType(Me.Master.Master.FindControl("ContentPlaceHolder1"), ContentPlaceHolder)
        Dim wiz_data = CType(ph.FindControl("LB_WIZ_DATA"), Label)

        If Session("wizData" + Session("wizGuid")) <> Nothing Then
            wiz = New JavaScriptSerializer().Deserialize(Of Wizard)(Session("wizData" + Session("wizGuid")))
        Else
            wiz = New JavaScriptSerializer().Deserialize(Of Wizard)(wiz_data.Text)
        End If
        Session("wizData" + wiz.GUID_TIMESTAMP) = New JavaScriptSerializer().Serialize(wiz)
        Session("wizGuid") = wiz.GUID_TIMESTAMP

        lbErr.Text = String.Empty

        Dim loadSql As Func(Of String, DataTable) = _
            Function(sql As String)
                Dim dt = New DataTable()

                Using cn = New SqlConnection(Resources.Resource.cns)
                    Using ad = New SqlDataAdapter(sql, cn)
                        Try
                            ad.Fill(dt)
                        Catch ex As Exception
                            HttpContext.Current.Response.Write(String.Format("<b style=color:red;>{0}</b>", ex.Message))
                        End Try
                    End Using
                End Using
                Return dt
            End Function


        If Not IsPostBack Then

            If Not Session("ListReservations") Is Nothing Then
                With gvListReservations
                    .DataSource = CType(Session("ListReservations"), DataTable)
                    .DataBind()
                End With
            Else

                Dim dt = New DataTable
                Dim sql = String.Empty

                If wiz.Scenario = EnumScenario.Two Then

                    sql = String.Format("select distinct r.ReservationID [Reservation ID], rs.ComboItem [Status], g.Description [Package Description], convert(varchar, r.CheckInDate, 101) [Check-In],  " _
                            & "convert(varchar, r.CheckOutDate, 101) [Check-Out], p.LastName + ', ' + p.FirstName [Prospect], a.AccomName [Accommodation] " _
                            & "from t_Reservations r inner join t_Prospect p on r.ProspectID = p.ProspectID " _
                            & "left join t_Tour t on t.tourid = r.tourid " _
                            & "inner join t_ComboItems rs on r.StatusID = rs.ComboItemID " _
                            & "inner join t_PackageIssued pi on r.PackageIssuedID = pi.PackageIssuedID " _
                            & "inner join t_Package g on g.PackageID = pi.PackageID " _
                            & "inner join t_ComboItems pt on pt.ComboItemID = g.TypeID " _
                            & "inner join t_Accom a on a.AccomID = g.AccomID " _
                            & "where r.ProspectID = {0} and rs.ComboItem in ('OpenEnded') and pt.ComboItem in ('Tradeshow', 'Tour Package') " _
                            & "and r.CheckInDate is null " _
                            & "and r.CheckOutDate is null " _
                            & "and DATALENGTH(g.Bedrooms) > 0 " _
                            & "and a.AccomName in ('KCP') " _
                            & "union all " _
                            & "select distinct  r.ReservationID [Reservation ID], rs.ComboItem [Status], g.Description [Package Description], " _
                            & "convert(varchar, r.CheckInDate, 101) [Check-In],  convert(varchar, r.CheckOutDate, 101) [Check-Out], " _
                            & "p.LastName + ', ' + p.FirstName [Prospect], a.AccomName " _
                            & "from t_Reservations r " _
                            & "inner join t_Prospect p on r.ProspectID = p.ProspectID " _
                            & "inner join t_Tour t on t.ReservationID = r.ReservationID " _
                            & "inner join t_ComboItems rs on r.StatusID = rs.ComboItemID " _
                            & "inner join t_PackageIssued pi on r.PackageIssuedID = pi.PackageIssuedID " _
                            & "inner join t_Package g on g.PackageID = pi.PackageID " _
                            & "inner join t_ComboItems pt on pt.ComboItemID = g.TypeID " _
                            & "inner join t_Accom a on a.AccomID = g.AccomID  " _
                            & "where r.ProspectID = {0} and rs.ComboItem in ('OpenEnded') and pt.ComboItem in ('Tradeshow', 'Tour Package') and r.CheckInDate is null and r.CheckOutDate is null " _
                            & "and a.AccomName in ('Best Western Historic', 'Wyndham Garden') order by g.Description;", wiz.Prospect.Prospect_ID)

                ElseIf wiz.Scenario = EnumScenario.Three Then

                    sql = String.Format("select distinct r.ReservationID [Reservation ID], rs.ComboItem [Status], g.Description [Package Description], convert(varchar, r.CheckInDate, 101) [Check-In],  " _
                            & "convert(varchar, r.CheckOutDate, 101) [Check-Out], p.LastName + ', ' + p.FirstName [Prospect], a.AccomName [Accommodation Name] " _
                            & "from t_Reservations r inner join t_Prospect p on r.ProspectID = p.ProspectID " _
                            & "left join t_Tour t on t.tourid = r.tourid " _
                            & "inner join t_ComboItems rs on r.StatusID = rs.ComboItemID " _
                            & "inner join t_PackageIssued pi on r.PackageIssuedID = pi.PackageIssuedID " _
                            & "inner join t_Package g on g.PackageID = pi.PackageID " _
                            & "inner join t_ComboItems pt on pt.ComboItemID = g.TypeID " _
                            & "inner join t_Accom a on a.AccomID = g.AccomID " _
                            & "where r.ProspectID = {0} and rs.ComboItem in ('Booked', 'Pending Payment', 'Reset') " _
                            & "and r.CheckInDate is not null " _
                            & "and r.CheckOutDate is not null " _
                            & "and DATALENGTH(g.Bedrooms) > 0 and AccomName in ('KCP') " _
                            & "union all " _
                            & "select distinct  r.ReservationID [Reservation ID], rs.ComboItem [Status], g.Description [Package Description], " _
                            & "convert(varchar, r.CheckInDate, 101) [Check-In],  convert(varchar, r.CheckOutDate, 101) [Check-Out], " _
                            & "p.LastName + ', ' + p.FirstName [Prospect], a.AccomName " _
                            & "from t_Reservations r " _
                            & "inner join t_Accommodation n on n.ReservationID = r.ReservationID " _
                            & "inner join t_Prospect p on r.ProspectID = p.ProspectID " _
                            & "left join t_Tour t on t.ReservationID = r.ReservationID " _
                            & "inner join t_ComboItems rs on r.StatusID = rs.ComboItemID " _
                            & "inner join t_PackageIssued pi on r.PackageIssuedID = pi.PackageIssuedID " _
                            & "inner join t_Package g on g.PackageID = pi.PackageID " _
                            & "inner join t_ComboItems pt on pt.ComboItemID = g.TypeID " _
                            & "inner join t_Accom a on a.AccomID = g.AccomID  " _
                            & "inner join t_Accommodation acc on acc.ReservationID = r.ReservationID " _
                            & "where r.ProspectID = {0} and rs.ComboItem in ('Booked', 'Pending Payment', 'Reset') and r.CheckInDate is not null and r.CheckOutDate is not null " _
                            & "and a.AccomName in ('Best Western Historic', 'Wyndham Garden') order by g.Description;", wiz.Prospect.Prospect_ID)

                End If

                dt = loadSql(sql)

                With gvListReservations
                    If dt.Rows.Count = 0 Then
                        Dim first_name As String, last_name As String
                        With New clsProspect
                            .Prospect_ID = wiz.Prospect.Prospect_ID
                            .Load()
                            first_name = .First_Name.ToUpper
                            last_name = .Last_Name.ToUpper
                        End With
                        .EmptyDataText = String.Format("{0} {1} does not have a valid reservation.", first_name,
                                      last_name)
                        Session("List") = Nothing
                    Else
                        .DataSource = dt
                        Session("ListReservations") = dt
                    End If
                    .DataBind()
                End With
            End If
            btNext.Enabled = False
        Else
            If wiz.Prospect.Prospect_ID > 0 And wiz.Reservation.ReservationID > 0 Then
                btNext.Enabled = True
            Else
                btNext.Enabled = False
            End If
        End If
    End Sub
    Protected Sub btPrevious_Click(sender As Object, e As EventArgs) Handles btPrevious.Click
        Dim bt = CType(sender, Button)
        bt.Attributes.Add("nav", 0)
        Navigate(CType(sender, Button))
    End Sub
    Protected Sub btNext_Click(sender As Object, e As EventArgs) Handles btNext.Click
        Dim bt = CType(sender, Button)
        bt.Attributes.Add("nav", 1)
        Navigate(bt)
    End Sub

    Protected Sub gvListReservations_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvListReservations.SelectedIndexChanged

        Dim hasEx = False
        Try
            Dim reservation_id = Int32.Parse(gvListReservations.DataKeys(gvListReservations.SelectedRow.RowIndex).Value.ToString())
            wiz.Reservation.ReservationID = reservation_id
            wiz.Load_All()
            Session("wizData" + wiz.GUID_TIMESTAMP) = New JavaScriptSerializer().Serialize(wiz)

        Catch ex As Exception

            hasEx = True

            If TypeOf ex Is Exception_Package_Is_Not_Found_In_PackageIssued2Package_Table Then

                lbErr.Text = ex.Message
                lbErr.ForeColor = Drawing.Color.Red
                lbErr.CssClass = "alert alert-warning fade in"
            Else

                lbErr.ForeColor = Drawing.Color.BlueViolet
                lbErr.Text = ex.Message
            End If
        End Try

        If hasEx = False Then

            Session("Search2") = Nothing
            Session("Search3") = Nothing
            btNext_Click(CType(btNext, Object), EventArgs.Empty)
        End If
    End Sub

#End Region

End Class
