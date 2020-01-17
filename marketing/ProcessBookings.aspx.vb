Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.ComponentModel
Imports System.IO
Imports System.Reflection
Imports System.Threading



Partial Class marketing_ProcessBookings
    Inherits System.Web.UI.Page

    Protected gridviews() As GridView = New GridView() {}

    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        initialize_CheckBoxList()        
        Array.Resize(gridviews, cbl_resorts.Items.Count)
        For i = 0 To cbl_resorts.Items.Count - 1
            gridviews(i) = New GridView()
            Dim tf1 = New TemplateField(), tf2 = New TemplateField(), tf3 = New TemplateField()            
            tf1.HeaderText = String.Format("<strong>{0}</strong>", cbl_resorts.Items(i).Text.ToUpper())
            tf1.ItemTemplate = New RadioButtonListTemplate()
            tf2.ItemTemplate = New MyTemplateEmailButton()
            tf3.HeaderText = "Note".ToUpper()
            tf3.ItemTemplate = New MyTemplateNoteTextBox()

            With gridviews(i)
                .DataKeyNames = New String() {"RESERVATION ID"}
                .BorderColor = Drawing.ColorTranslator.FromHtml("#d80073")
                .BorderStyle = BorderStyle.Groove
                .Columns.Add(tf2)   '// submit button
                .Columns.Add(tf1)   '// radio buttons
                .Columns.Add(tf3)   '// note
                .ID = String.Format("gv{0}GridView", cbl_resorts.Items(i).Text.Replace(" ", ""))
                .ShowFooter = True
                .ShowHeader = True
                .CssClass = "layout-fix"

                AddHandler .RowDataBound, AddressOf gv_RowDataBound
                AddHandler .RowCommand, AddressOf gv_RowCommand
                AddHandler .RowCreated, AddressOf gv_RowCreated

                Dim htmlG As New HtmlGenericControl("div")
                htmlG.Controls.Add(New HtmlGenericControl("br"))
                htmlG.Controls.Add(gridviews(i))

                resultDiv.Controls.Add(htmlG)
            End With
        Next
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load        

        If IsPostBack = False Then
        End If
    End Sub

    Private Sub initialize_CheckBoxList()
        Dim cbl_List = New List(Of CheckBoxList)
        cbl_List.Add(cbl_resorts)
        cbl_List.Add(cbl_locations)
        cbl_List.Add(cbl_statuses)
        cbl_List.Add(cbl_bookings)
        With New clsComboItems()
            cbl_List.ElementAt(0).DataSource = .Load_ComboItems("ResortCompany")
            cbl_List.ElementAt(1).DataSource = .Load_ComboItems("ReservationLocation")
            cbl_List.ElementAt(2).DataSource = .Load_ComboItems("ReservationStatus")
            cbl_List.ElementAt(3).DataSource = .Load_ComboItems("BookingStatus")
        End With

        For Each e In cbl_List
            e.AppendDataBoundItems = True
            e.Items.Add(New ListItem("All", "-1"))
            If e.ID = "cbl_resorts" Then e.Items.Add(New ListItem("(Empty)", "0"))
            If e.ID = "cbl_bookings" Then e.Items.Add(New ListItem("(Empty)", "is null"))
            e.DataTextField = "comboItem"
            e.DataValueField = "comboItemID"
            e.DataBind()
        Next



    End Sub

    Private ReadOnly Property sql As String
        Get
            Dim locations = From l As ListItem In cbl_locations.Items.OfType(Of ListItem)() Where l.Selected = True And l.Text.ToLower().Equals("all") = False Select l.Value
            Dim resorts = From l As ListItem In cbl_resorts.Items.OfType(Of ListItem)() Where l.Selected = True And l.Text.ToLower().Equals("all") = False Select l.Value
            Dim statuses = From l As ListItem In cbl_statuses.Items.OfType(Of ListItem)() Where l.Selected = True And l.Text.ToLower().Equals("all") = False Select l.Value

            Dim bookings = From li In cbl_bookings.Items.OfType(Of ListItem).Where(Function(x) x.Selected And x.Text <> "(Empty)" And x.Text.ToLower().Equals("all") = False).Select(Function(x) x.Value)

            If locations.ToArray().Length = 0 Then locations = New String() {"0"}
            If statuses.ToArray().Length = 0 Then statuses = New String() {"0"}

            Dim appendedWhere = " "

            If bookings.Count() > 0 And cbl_bookings.Items.OfType(Of ListItem).Where(Function(x) x.Selected And x.Text = "(Empty)").Count = 1 Then
                appendedWhere = String.Format(" and (r.bookingStatusID in ({0}) or (r.BookingStatusID is null or r.BookingStatusID = 0))", String.Join(",", bookings.ToArray()))                
            End If

            If bookings.Count() > 0 And cbl_bookings.Items.OfType(Of ListItem).Where(Function(x) x.Selected And x.Text = "(Empty)").Count = 0 Then
                appendedWhere = String.Format(" and (r.bookingStatusID in ({0}))", String.Join(",", bookings.ToArray()))
            End If

            If bookings.Count() = 0 And cbl_bookings.Items.OfType(Of ListItem).Where(Function(x) x.Selected And x.Text = "(Empty)").Count = 1 Then
                appendedWhere += " and (r.BookingStatusID is null or r.BookingStatusID = 0)"
            End If

            Dim resortWhereClause = IIf(resorts.Count() = 0, " and (r.resortcompanyid is null or resortcompanyid = 0)", String.Format(" and resortcompanyid in ({0}) ", String.Join(",", resorts.ToArray())))

            Dim start_date As DateTime, end_date As DateTime

            If DateTime.TryParse(dteEDate.Selected_Date, end_date) = False Then end_date = DateTime.MaxValue
            If DateTime.TryParse(dteSDate.Selected_Date, start_date) = False Then start_date = DateTime.MaxValue

            Dim sqlText = String.Format( _
                "select r.ReservationID [RESERVATION ID], " & _
                "bs.ComboItem [BOOKED STATUS], " & _
                "CAST(r.bookingStatusDate as date) [STATUS DATE], " & _
                "coalesce((select comboitem from t_ComboItems where ComboItemID = (select top 1 roomtypeid from t_Accommodation where ReservationID = r.ReservationID order by AccommodationID desc)), 'Double') [ROOM TYPE], " & _
                "cast(r.CheckInDate as date) [ARRIVAL DATE], " & _
                "cast(r.CheckOutDate as date) [DEPARTURE DATE], " & _
                "ts.comboItem [STATUS], " & _
                "r.NumberAdults [#ADULTS], " & _
                "r.NumberChildren [#CHILDREN], " & _
                "P.FirstName [FIRST NAME], " & _
                "p.LastName [LAST NAME], " & _
                "(select top 1 v.UFValue from t_UFields f inner join t_UF_Tables t on f.UFTableID = t.UFTableID inner join t_UF_Value v on v.UFID = f.UFID where t.UFTable = 'prospect' and f.UFName = 'age' and v.keyValue=p.prospectid) [AGE], " & _
                "ms.ComboItem [MARITAL STATUS], " & _
                "p.SpouseFirstName + ' ' + " & _
                "p.SpouseLastName [SPOUSE NAME], " & _
                "(select top 1 v.UFValue from t_UFields f inner join t_UF_Tables t on f.UFTableID = t.UFTableID inner join t_UF_Value v on v.UFID = f.UFID where t.UFTable = 'prospect' and f.UFName = 'age spouse/cohab' and v.keyValue=p.prospectid) [AGE OF SPOUSE], " & _
                "(select top 1 address1 from t_ProspectAddress where ProspectID = p.ProspectID and ActiveFlag = 1 order by AddressID desc) + ', ' + (select top 1 city from t_ProspectAddress where ProspectID = p.ProspectID and ActiveFlag = 1 order by AddressID desc) + ' ' + (select comboItem from t_comboitems where comboitemid = (select top 1 StateID from t_ProspectAddress where ProspectID = p.ProspectID and ActiveFlag = 1 order by AddressID desc)) [ADDRESS], " & _
                "(select top 1 PostalCode from t_ProspectAddress where ProspectID = p.ProspectID and ActiveFlag = 1 order by AddressID desc) [ZIP], (select top 1 Number from t_ProspectPhone where ProspectID = p.ProspectID and Active = 1 order by PhoneID desc) [PHONE NUMBER], " & _
                "(select top 1 email from t_ProspectEmail where ProspectID = p.ProspectID and IsActive = 1 and IsPrimary = 1 order by emailid desc) [EMAIL ADDRESS], " & _
                "(case when (select top 1 v.UFValue from t_UFields f inner join t_UF_Tables t on f.UFTableID = t.UFTableID inner join t_UF_Value v on v.UFID = f.UFID where t.UFTable = 'reservation' and f.UFName = 'Westgate PP Number' and v.KeyValue = r.ReservationID) is null then 'Travel Club Mini Vac: 0703107' end) [COMMENTS], " & _
                 "p.Income [INCOME], " & _
                "(case when (select top 1 v.UFValue from t_UFields f inner join t_UF_Tables t on f.UFTableID = t.UFTableID inner join t_UF_Value v on v.UFID = f.UFID where t.UFTable = 'reservation' and f.UFName = 'Major Credit Card' and v.KeyValue = r.ReservationID) = 'true' then '***Y***' else '***N***' end) [CREDIT CARD], " & _
                "'' [GIFTS], ''" & _
                "[SPECIAL REQUESTS], " & _
                "t.TOURID [TOUR ID], " & _
                "r.PackageIssuedID [PACKAGE ID], " & _
                "r.statusid [STATUS ID], " & _
                "coalesce(r.ResortCompanyID, 0) [RESORTCOMPANYID], " & _
                "r.BookingStatusID [BOOKING STATUS ID], " & _
                "r.ResLocationID [LOCATION ID], " & _
                "(select package from t_Package where PackageID = r.PackageIssuedID) [PACKAGE NAME], " & _
                "(select top 1 v.UFValue from t_UFields f inner join t_UF_Tables t on f.UFTableID = t.UFTableID inner join t_UF_Value v on v.UFID = f.UFID where t.UFTable = 'reservation' and f.UFName = 'Westgate PP Number' and v.KeyValue = r.ReservationID) [PP#], " & _
                "(select top 1 note from t_note where keyfield = 'RESERVATIONBOOKINGID' and keyvalue = r.reservationid order by noteid desc) [NOTE], " & _
                "P.prospectid [PROSPECT ID], " & _
                "(select AccomName from t_accom where accomid in (select top 1 accomid from t_accommodation where ReservationID = r.reservationID order by AccommodationID desc)) [Hotel], r.reservationNumber " & _
                "from t_reservations r " & _
                "inner join t_Prospect p on r.ProspectID = p.ProspectID " & _
                "left join t_ComboItems ms on ms.ComboItemID = p.MaritalStatusID " & _
                "left join t_ComboItems bs on bs.ComboItemID = r.bookingStatusID " & _
                "left join t_tour t on r.TourID = t.TourID " & _
                "left join t_comboitems ts on ts.ComboItemID = r.StatusID " & _
                "where CheckInDate between '{0}' and '{1}' " & _
                "and r.ResLocationID in ({2}) " & _
                "and r.StatusID in ({3}) " & _
                resortWhereClause & appendedWhere, start_date.ToShortDateString, end_date.ToShortDateString(), _
                String.Join(",", locations.ToArray()), _
                String.Join(",", statuses.ToArray()))
            Return sqlText
        End Get
    End Property

    Private Function GetTableData(sqlText As String) As DataTable
        Dim dt = New DataTable()
        Using cn = New SqlConnection(Resources.Resource.cns)
            Using ad = New SqlDataAdapter(sqlText, cn)
                Try
                    ad.Fill(dt)
                Catch ex As Exception
                    Response.Write(ex.Message)
                End Try
            End Using
        End Using
        Return dt
    End Function

    Private Function GetPremium(s As String) As String
        Dim sqlText = String.Format("Select p.PremiumName " & _
                            "from t_Reservations r inner join t_Tour t on r.tourid = t.TourID  " & _
                            "inner join t_PremiumIssued pi on pi.KeyValue = t.TourID " & _
                            "inner join t_Premium p on pi.PremiumID = p.PremiumID  " & _
                            "where pi.KeyField = 'tourid' and r.ReservationID = {0}", s)

        Dim dt = GetTableData(sqlText)
        Dim ar() As String = New String() {}

        If dt.Rows.Count > 0 Then
            Array.Resize(ar, dt.Rows.Count)
            For i = 0 To dt.Rows.Count - 1
                ar(i) = dt.Rows(i)(0)
            Next
        End If
        Return String.Join(",", ar)
    End Function

    Private Function GetSpecialNeeds(reservationID As Int32) As String

        Dim sqlText = String.Format( _
                        "select [special request] = case when c.ComboItem = 'Other' then sn.NeedText else c.comboItem end " & _
                        "from t_specialneeds sn inner join t_ComboItems c on sn.NeedID = c.ComboItemID  where sn.keyvalue = {0}", reservationID)

        Dim dt = GetTableData(sqlText)
        Dim ar() As String = New String() {}

        If dt.Rows.Count > 0 Then
            Array.Resize(ar, dt.Rows.Count)
            For i = 0 To dt.Rows.Count - 1
                If dt.Rows(i)(0).Equals(DBNull.Value) = False Then
                    ar(i) = dt.Rows(i)(0)
                End If
            Next
        End If
        Return String.Join(",", ar)
    End Function

    Private Function GetNote(reservationId As Int32) As String

        Dim sqlText = String.Format( _
            "select top 1 note from t_note where keyfield = 'RESERVATIONBOOKINGSTATUSID' and keyvalue = {0} order by noteid desc", reservationId)
        Dim dt = GetTableData(sqlText)

        If dt.Rows.Count = 1 Then
            Return dt.Rows(0)(0)
        Else
            Return String.Empty
        End If
    End Function

    Protected Sub gv_RowCreated(sender As Object, e As GridViewRowEventArgs)

        If e.Row.RowType = DataControlRowType.Footer Then
            Dim tc = New TableCell()
            tc.Controls.Add(New Button() With {.Text = "Email All", .CommandName = "Email All"})
            tc.Controls.Add(New Button() With {.Text = "Complete All", .CommandName = "Complete All"})
            e.Row.Cells.AddAt(1, tc)
        End If
    End Sub

    Protected Sub gv_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs)
        Dim gvr As GridViewRow = e.Row
        Dim gv As GridView = CType(gvr.NamingContainer, GridView)
        Dim t As TextBox = Nothing

        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim dt = CType(gv.DataSource, DataTable)
            dt.PrimaryKey = New DataColumn() {dt.Columns(0)}

            Dim dr As DataRow = dt.Rows.Find(gv.DataKeys(e.Row.RowIndex).Value)
            Dim status = dr("BOOKED STATUS").ToString()
            gvr.Cells(25).Text = GetPremium(dr.Field(Of Int32)("RESERVATION ID").ToString())
            gvr.Cells(26).Text = GetSpecialNeeds(dr.Field(Of Int32)("RESERVATION ID").ToString())

            t = CType(gvr.FindControl("note"), TextBox)
            t.Text = GetNote(dr.Field(Of Int32)("RESERVATION ID").ToString())

            gvr.Cells(30).Text = cbl_resorts.Items.OfType(Of ListItem).SingleOrDefault(Function(x) x.Value = gvr.Cells(30).Text).Text
            gvr.Cells(29).Text = cbl_statuses.Items.OfType(Of ListItem).SingleOrDefault(Function(x) x.Value = gvr.Cells(29).Text).Text
            gvr.Cells(32).Text = cbl_locations.Items.OfType(Of ListItem).SingleOrDefault(Function(x) x.Value = gvr.Cells(32).Text).Text

            gvr.Cells(3).Controls.Add(New HyperLink() With {.Text = dr.Field(Of Int32)("RESERVATION ID").ToString(), .Target = "_blank", .NavigateUrl = String.Format("../marketing/editreservation.aspx?reservationid={0}", dr.Field(Of Int32)("RESERVATION ID").ToString())})

            If String.IsNullOrEmpty(gvr.Cells(7).Text) = False Then
                gvr.Cells(7).Text = DateTime.Parse(gvr.Cells(7).Text).ToShortDateString()
            End If
            If String.IsNullOrEmpty(gvr.Cells(8).Text) = False Then
                gvr.Cells(8).Text = DateTime.Parse(gvr.Cells(8).Text).ToShortDateString()
            End If
            If String.IsNullOrEmpty(gvr.Cells(23).Text) = False Then
                Dim income As Decimal = 0
                If Decimal.TryParse(gvr.Cells(23).Text, income) Then
                    gvr.Cells(23).Text = Decimal.Round(income, 2, MidpointRounding.AwayFromZero)
                End If
            End If
        End If
    End Sub

    Protected Sub gv_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim gvr As GridViewRow = CType(CType(e.CommandSource, Button).NamingContainer, GridViewRow)
        Dim gv As GridView = CType(gvr.NamingContainer, GridView)
        Dim radioButtonList_rows = gv.Rows.OfType(Of GridViewRow).Where(Function(x) x.RowType = DataControlRowType.DataRow _
                                                                         And Not CType(x.FindControl("rbl"), RadioButtonList).SelectedItem Is Nothing)


        If CType(e.CommandSource, Button).CommandName = "Submit" Then
            Save_Data(CType(CType(e.CommandSource, Button).NamingContainer, GridViewRow), CType(gvr.NamingContainer, GridView))
        ElseIf CType(e.CommandSource, Button).CommandName = "Email All" And radioButtonList_rows.Count() > 0 And radioButtonList_rows.Where(Function(x) CType(x.FindControl("rbl"), RadioButtonList).SelectedItem.Text = "Email").Count() > 0 Then
            Save_Data(radioButtonList_rows.Where(Function(x) CType(x.FindControl("rbl"), RadioButtonList).SelectedItem.Text = "Email").ToList(), CType(gvr.NamingContainer, GridView))
        ElseIf CType(e.CommandSource, Button).CommandName = "Complete All" And radioButtonList_rows.Count() > 0 And radioButtonList_rows.Where(Function(x) CType(x.FindControl("rbl"), RadioButtonList).SelectedItem.Text = "Complete").Count() > 0 Then
            Save_Data(radioButtonList_rows.Where(Function(x) CType(x.FindControl("rbl"), RadioButtonList).SelectedItem.Text = "Complete").ToList(), CType(gvr.NamingContainer, GridView))
        End If
    End Sub

    Private Sub Save_Data(gvr As GridViewRow, gv As GridView)
        Dim reservation_id = gv.DataKeys(gvr.RowIndex).Value, sb = New StringBuilder(), rbl = CType(gvr.FindControl("rbl"), RadioButtonList)

        If Not rbl.SelectedItem Is Nothing Then
            Select Case rbl.SelectedItem.Text
                Case "In-Complete"
                    Dim note As TextBox = CType(gvr.FindControl("note"), TextBox)
                    sb.AppendFormat("update t_reservations set bookingStatusDate = getDate(), bookingStatusID = {1} where reservationID = {0};", _
                                        reservation_id, cbl_bookings.Items.OfType(Of ListItem).Single(Function(x) x.Text.Equals("In-complete")).Value)

                    If note.Text.Trim().Replace("'", "''") <> "" Then
                        sb.AppendFormat("insert into t_note (keyfield, keyvalue, note, datecreated, createdbyid) values ('RESERVATIONBOOKINGSTATUSID', {0}, '{1}', getdate(), {2});", _
                                       reservation_id, note.Text.Trim().Replace("'", "''"), Session("UserDBID"))
                    End If

                    gvr.Cells(4).Text = cbl_bookings.Items.OfType(Of ListItem).Single(Function(x) x.Text.Equals("In-complete")).Text
                    gvr.Cells(5).Text = DateTime.Now.ToShortDateString()
                    gvr.Cells(31).Text = cbl_bookings.Items.OfType(Of ListItem).Single(Function(x) x.Text.Equals("In-complete")).Value

                Case "Complete"
                    sb.AppendFormat("update t_reservations set bookingStatusID = {1}, bookingStatusDate = getDate() where reservationID = {0};", _
                                    reservation_id, cbl_bookings.Items.OfType(Of ListItem).Single(Function(x) x.Text.Equals("Completed")).Value)
                    gvr.Cells(4).Text = cbl_bookings.Items.OfType(Of ListItem).Single(Function(x) x.Text.Equals("Completed")).Text
                    gvr.Cells(5).Text = DateTime.Now.ToShortDateString()
                    gvr.Cells(31).Text = cbl_bookings.Items.OfType(Of ListItem).Single(Function(x) x.Text.Equals("Completed")).Value

                Case "Email"
                    Dim status_r = gvr.Cells(9).Text
                    Dim note = String.Empty

                    If status_r.ToLower().Equals("Pending".ToLower()) Then
                        note = "Booking request sent to resort, pending confirmation."
                    ElseIf status_r.ToLower().Equals("Reset".ToLower()) Then
                        note = "Change request sent to resort, pending confirmation."
                    ElseIf status_r.ToLower().Equals("Cancelled".ToLower()) Then
                        note = "Booking cancellation sent to resort, pending confirmation."
                    End If

                    sb.AppendFormat("insert into t_note (keyfield, keyvalue, note, datecreated, createdbyid) values ('RESERVATIONBOOKINGSTATUSID', '{0}', '{1}', getdate(), {2});", _
                                    reservation_id, note, Session("UserDBID"))

                    sb.AppendFormat("update t_reservations set bookingStatusID = {1}, bookingStatusDate = getDate() where reservationID = {0}", _
                                    reservation_id, cbl_bookings.Items.OfType(Of ListItem).Single(Function(x) x.Text.Equals("Request Sent")).Value)

                    gvr.Cells(4).Text = cbl_bookings.Items.OfType(Of ListItem).Single(Function(x) x.Text.Equals("Request Sent")).Text
                    gvr.Cells(5).Text = DateTime.Now.ToShortDateString()
                    gvr.Cells(31).Text = cbl_bookings.Items.OfType(Of ListItem).Single(Function(x) x.Text.Equals("Request Sent")).Value
            End Select
           
            Using cn = New SqlConnection(Resources.Resource.cns)
                Using cm = New SqlCommand(sb.ToString(), cn)
                    Try
                        cn.Open()
                        cm.ExecuteNonQuery()
                    Catch ex As Exception
                        Response.Write(ex.Message)
                    Finally
                        cn.Close()
                    End Try
                End Using
            End Using

            If Not rbl.SelectedItem Is Nothing Then
                Select Case rbl.SelectedItem.Text
                    Case "Email"
                        Email_Resort(gvr, gv)
                End Select
            End If
        End If
    End Sub

    Private Sub Save_Data(lvr As List(Of GridViewRow), gv As GridView)
        Dim reservation_id = 0, sb = New StringBuilder()
        For Each r As GridViewRow In lvr
            reservation_id = gv.DataKeys(r.RowIndex).Value
            Dim rbl = CType(r.FindControl("rbl"), RadioButtonList)
            Select Case rbl.SelectedItem.Text

                Case "Complete"
                    sb.AppendFormat("update t_reservations set bookingStatusID = {1}, bookingStatusDate = getDate() where reservationID = {0};", _
                                    reservation_id, cbl_bookings.Items.OfType(Of ListItem).Single(Function(x) x.Text.Equals("Completed")).Value)

                    r.Cells(4).Text = cbl_bookings.Items.OfType(Of ListItem).Single(Function(x) x.Text.Equals("Completed")).Text
                    r.Cells(5).Text = DateTime.Now.ToShortDateString()
                    r.Cells(31).Text = cbl_bookings.Items.OfType(Of ListItem).Single(Function(x) x.Text.Equals("Completed")).Value
                Case "Email"

                    Dim status_r = r.Cells(9).Text, note = String.Empty

                    If status_r.ToLower().Equals("Pending".ToLower()) Then
                        note = "Booking request sent to resort, pending confirmation."
                    ElseIf status_r.ToLower().Equals("Reset".ToLower()) Then
                        note = "Change request sent to resort, pending confirmation."
                    ElseIf status_r.ToLower().Equals("Cancelled".ToLower()) Then
                        note = "Booking cancellation sent to resort, pending confirmation."
                    End If

                    sb.AppendFormat("insert into t_note (keyfield, keyvalue, note, datecreated, createdbyid) values ('RESERVATIONBOOKINGSTATUSID', '{0}', '{1}', getdate(), {2});", _
                                    reservation_id, note, Session("UserDBID"))

                    sb.AppendFormat("update t_reservations set bookingStatusID = {1}, bookingStatusDate = getDate() where reservationID = {0}", _
                                    reservation_id, cbl_bookings.Items.OfType(Of ListItem).Single(Function(x) x.Text.Equals("Request Sent")).Value)

                    r.Cells(4).Text = cbl_bookings.Items.OfType(Of ListItem).Single(Function(x) x.Text.Equals("Request Sent")).Text
                    r.Cells(5).Text = DateTime.Now.ToShortDateString()
                    r.Cells(31).Text = cbl_bookings.Items.OfType(Of ListItem).Single(Function(x) x.Text.Equals("Request Sent")).Value
            End Select
        Next
        Using cn = New SqlConnection(Resources.Resource.cns)
            Using cm = New SqlCommand(sb.ToString(), cn)
                Try
                    cn.Open()
                    cm.ExecuteNonQuery()
                Catch ex As Exception
                    Response.Write(ex.Message)
                Finally
                    cn.Close()
                End Try
            End Using
        End Using

        For Each r As GridViewRow In lvr
            Dim rbl = CType(r.FindControl("rbl"), RadioButtonList)
            Select Case rbl.SelectedItem.Text
                Case "Email"
                    Email_Resort(r, gv)
            End Select
        Next
    End Sub

    ''' <summary>
    ''' Send email preconfigured to resorts
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Email_Resort(gvr As GridViewRow, gv As GridView)
        Dim reservation_id As String = 0, status_id As String = 0, resortcompany_id As String = 0
        With gvr
            reservation_id = .Cells(3).Text.Trim().ToLower()

            If .Cells(29).Text.Length > 0 And .Cells(30).Text.Length > 0 Then

                status_id = (From l As ListItem In cbl_statuses.Items.OfType(Of ListItem)() _
                                               Where l.Text.ToLower().Equals(.Cells(29).Text.ToLower())).Single().Value
                resortcompany_id = (From l As ListItem In cbl_resorts.Items.OfType(Of ListItem)() _
                                    Where l.Text.ToLower().Equals(.Cells(30).Text.Trim().ToLower())).Single().Value

                Dim dt = GetTableData(String.Format("select * from t_ResortEmailManager r inner join t_letters l on r.letterid = l.letterid " & _
                                       "where r.resortcompanyid = {0} and r.reservationstatusid = {1} and active = 1", resortcompany_id, status_id))

                If dt.Rows.Count = 0 Then Return

                For Each dr As DataRow In dt.Rows
                    Dim receipient As String = dr("EmailAddress").ToString()
                    Dim subject As String = dr("Subject").ToString()
                    Dim body As String = dr("LetterContent").ToString()

                    Dim o_letter As New clsLetters
                    Dim DATA_COLUMNS As New Dictionary(Of String, String)
                    Dim tbl = GetTableData(String.Format("select * from t_reservations r inner join t_prospect p on r.prospectid = p.prospectid where r.reservationid ={0}", reservation_id))
                    For i = 0 To tbl.Columns.Count - 1

                        If tbl.Columns(i).DataType.ToString() = "System.DateTime" Then
                            If tbl.Rows(0)(i).Equals(DBNull.Value) = False Then
                                DATA_COLUMNS.Add(tbl.Columns(i).ColumnName.ToLower(), String.Format("{0}", DateTime.Parse(tbl.Rows(0)(i).ToString()).ToShortDateString()))
                            Else
                                DATA_COLUMNS.Add(tbl.Columns(i).ColumnName.ToLower(), String.Format("{0}", tbl.Rows(0)(i).ToString()))
                            End If
                        Else
                            DATA_COLUMNS.Add(tbl.Columns(i).ColumnName.ToLower(), String.Format("{0}", tbl.Rows(0)(i).ToString()))
                        End If
                    Next

                    DATA_COLUMNS.Add("currentdate", DateTime.Now.ToShortDateString())

                    Dim arr = subject.Split(" ")
                    For i = 0 To arr.Length - 1
                        If arr(i).Trim().IndexOf("<") >= 0 And arr(i).Trim().IndexOf(">") > 1 Then
                            Dim substr = arr(i).Trim().Substring(arr(i).Trim().IndexOf("<") + 1, arr(i).Trim().IndexOf(">") - 1)

                            For Each kvp As KeyValuePair(Of String, String) In DATA_COLUMNS
                                If substr = kvp.Key Then
                                    If Array.IndexOf(New String() {"reslocationid", "statusid", "typeid", "subtypeid"}, kvp.Key) > -1 Then

                                        Using cn = New SqlConnection(Resources.Resource.cns)
                                            Using cm = New SqlCommand("select comboitem from t_comboitems where comboitemid = " & kvp.Value, cn)
                                                Try
                                                    cn.Open()
                                                    arr(i) = arr(i).Replace(String.Format("<{0}>", kvp.Key), cm.ExecuteScalar())
                                                Catch ex As Exception
                                                    Response.Write(ex.Message)
                                                Finally
                                                    cn.Close()
                                                End Try
                                            End Using
                                        End Using
                                    Else
                                        arr(i) = arr(i).Replace(String.Format("<{0}>", kvp.Key), kvp.Value)
                                    End If
                                End If
                            Next
                        End If
                    Next

                    Dim content = String.Format("{0}", o_letter.Get_Letter_Text(dr("letterid").ToString(), reservation_id))
                    Send_Mail(receipient, "confirmation@vrcvacations.com", String.Join(" ", arr), content, True)

                    Dim ud = New clsUploadedDocs()
                    ud.DateUploaded = DateTime.Now
                    ud.KeyField = "reservationid"
                    ud.KeyValue = reservation_id
                    ud.Name = "confirmation email"
                    ud.ContentText = content

                    ud.Path = ""
                    ud.Save()
                    ud = Nothing
                Next
            End If
        End With
    End Sub

    Protected Sub submit_Click(sender As Object, e As System.EventArgs) Handles submit.Click

        For i = 0 To gridviews.Length - 1
            gridviews(i).DataSource = Nothing
            gridviews(i).DataBind()
        Next
        submit.Attributes.Remove("columns")
        Dim dt = GetTableData(sql)

        If dt.Rows.Count > 0 Then
            submit.Attributes.Add("columns", String.Join(",", (From c As DataColumn In dt.Columns Select c.ColumnName).ToArray()))
            dt.PrimaryKey = New DataColumn() {dt.Columns(0)}

            For Each li As ListItem In cbl_resorts.Items.OfType(Of ListItem).OrderBy(Function(x) x.Text)
                Dim text = li.Text
                Dim value = li.Value
                Dim dv As DataView = New DataView(dt, String.Format("RESORTCOMPANYID = {0}", value), "", DataViewRowState.CurrentRows)
                Dim table_resort As DataTable = dv.ToTable()
                If table_resort.Rows.Count > 0 Then
                    With gridviews.Single(Function(x) x.ID = "gv" & text.Replace(" ", "") & "GridView")
                        .DataSource = table_resort
                        .DataBind()
                        .UseAccessibleHeader = True
                        .FooterRow.TableSection = TableRowSection.TableFooter
                        .HeaderRow.TableSection = TableRowSection.TableHeader
                    End With
                End If
            Next
        End If
    End Sub

    Protected Sub btEE_Click(sender As Object, e As System.EventArgs) Handles btEE.Click
        Dim dt = GetTableData(sql)
        If dt.Rows.Count > 0 Then

            Dim sb = New StringBuilder()
            sb.Append("<table>")

            sb.AppendFormat("<tr>")
            For Each col As DataColumn In dt.Columns
                sb.AppendFormat("<th>{0}</th>", col.ColumnName)
            Next
            sb.AppendFormat("</tr>")

            For Each dr As DataRow In dt.Rows
                sb.AppendFormat("<tr>")
                For Each col As DataColumn In dt.Columns
                    sb.AppendFormat("<td>{0}</td>", dr(col).ToString())
                Next
                sb.AppendFormat("</tr>")
            Next

            sb.Append("</table>")

            Response.Clear()
            Response.ContentType = "application/vnd.ms-excel"
            Response.AddHeader("Content-Disposition", String.Format("attachment; filename=ProcessBookings{0}_{1}.xls", DateTime.Now.ToLongTimeString(), DateTime.Now.Millisecond))
            Response.Write(sb.ToString())
            Response.End()
        End If
    End Sub

    Private Class RadioButtonListTemplate
        Implements ITemplate
        Public Sub InstantiateIn(container As System.Web.UI.Control) Implements System.Web.UI.ITemplate.InstantiateIn
            Dim rbl As New RadioButtonList()
            With rbl
                .Items.Add(New ListItem("Email", "EmailAll"))
                .Items.Add(New ListItem("Complete", "CompleteAll"))
                .Items.Add(New ListItem("In-Complete", "In-complete"))
                .RepeatDirection = RepeatDirection.Horizontal
                .AutoPostBack = False
                .ID = "rbl"
            End With
            container.Controls.Add(rbl)
        End Sub

        Public Sub BindData(sender As Object, e As EventArgs)
            Dim rbl As RadioButtonList = CType(sender, RadioButtonList)
            Dim gvr As GridViewRow = CType(rbl.NamingContainer, GridViewRow)
        End Sub
    End Class

    Private Class MyTemplateEmailButton
        Implements ITemplate
        Public Sub InstantiateIn(container As System.Web.UI.Control) Implements System.Web.UI.ITemplate.InstantiateIn
            Dim b As New Button
            b.Text = "Submit"
            b.CommandName = "Submit"
            container.Controls.Add(b)
        End Sub
    End Class

    Private Class MyTemplateNoteTextBox
        Implements ITemplate
        Public Sub InstantiateIn(container As System.Web.UI.Control) Implements System.Web.UI.ITemplate.InstantiateIn
            container.Controls.Add(New TextBox() With {.Text = String.Empty, .ID = "note"})
        End Sub
    End Class

End Class
