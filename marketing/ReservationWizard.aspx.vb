Imports System
Imports System.Data
Imports System.Data.SqlClient


Partial Class marketing_ReservationWizard
    Inherits System.Web.UI.Page

#Region "private fields"

    Private ROOMS() As String = {"1", "1BD-DWN", "1BD-UP", "2", "3", "4"}

    Private hiddenFields() As HiddenField = { _
        hfd_packageId, hfd_packageIssuedId, hfd_packageName, _
        hfd_prospectId, hfd_status, hfd_cost, _
        hfd_inventoryType, hfd_checkInDate, hfd_checkOutDate}


#End Region
#Region "private event handlers"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Me.IsPostBack = False Then
            ddl_room_size.DataSource = ROOMS
            ddl_room_size.DataBind()

            ddl_inventory_type.DataSource = New String() {"Marketing", "Rental"}


            ddl_additional_nights.DataSource = Enumerable.Range(1, 7)
            ddl_nights.DataSource = Enumerable.Range(2, 4)

            Dim dropDownLists() = {ddl_reservation_location, ddl_reservation_type, ddl_unit_type}
            Dim comboNames() = {"ReservationLocation", "ReservationType", "UnitType"}

            Using cnn As New SqlConnection(Resources.Resource.cns)

                For i = 0 To comboNames.Count() - 1

                    Dim sql = String.Format("Select 0 as ComboItemID, '(empty)' as Comboitem union Select i.ComboItemID, i.Comboitem " & _
                                            "from t_Comboitems i inner join t_Combos c on c.comboid = i.comboid where (i.active=1) and " & _
                                            "c.ComboName = '{0}' order by comboitem", comboNames(i))
                    Using ada = New SqlDataAdapter(sql, cnn)
                        Dim dt = New DataTable()

                        ada.Fill(dt)
                        dropDownLists(i).DataSource = dt
                        dropDownLists(i).DataTextField = "ComboItem"
                        dropDownLists(i).DataValueField = "ComboItemID"
                    End Using
                Next

                Me.DataBind()
            End Using

            Get_PackageIssued_ById("74252")
            'Get_PackageIssued_ById("82000")

            Response.Write(String.Format("<strong>{0}, {1} at {2} for ${3}</strong>", PackageName, Status, DateTime.Now.ToLongTimeString(), Cost))


            mtv_wizard.SetActiveView(view_1)
            btn_next_Click(Nothing, EventArgs.Empty)
            btn_previous_Click(Nothing, EventArgs.Empty)
            ddl_nights_SelectedIndexChanged(Nothing, EventArgs.Empty)


        End If
    End Sub

    Protected Sub btn_next_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_next.Click

        btn_next.Enabled = True
        btn_previous.Enabled = True

        If mtv_wizard.ActiveViewIndex < mtv_wizard.Views.Count - 1 Then
            mtv_wizard.ActiveViewIndex += 1
        End If

        If mtv_wizard.ActiveViewIndex = mtv_wizard.Views.Count - 1 Then
            btn_next.Enabled = False
        End If

        Dim vw = mtv_wizard.GetActiveView()
        If vw.Equals(view_2) Then
            Show_Inventories()
            lbl_view2_err.Text = String.Format("{0}, {1}", PackageName, Charge_Package_Rate())
        End If

    End Sub

    Protected Sub btn_previous_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_previous.Click

        btn_next.Enabled = True
        btn_previous.Enabled = True

        If mtv_wizard.ActiveViewIndex > 0 Then
            mtv_wizard.ActiveViewIndex -= 1
        End If

        If mtv_wizard.ActiveViewIndex = 0 Then
            btn_previous.Enabled = False
        End If
    End Sub

    Protected Sub ddl_nights_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_nights.SelectedIndexChanged        

        If String.IsNullOrEmpty(hfd_checkInDate.Value) = False And cbx_additional_nights.Checked = False Then
            lbl_reservation_checkout.Text = DateTime.Parse(hfd_checkInDate.Value).AddDays(Integer.Parse(ddl_nights.Text))

            hfd_checkOutDate.Value = lbl_reservation_checkout.Text

        ElseIf String.IsNullOrEmpty(hfd_checkInDate.Value) = False And cbx_additional_nights.Checked = True Then
            ddl_additional_nights_SelectedIndexChanged(Nothing, EventArgs.Empty)
        End If
    End Sub

    Protected Sub ddl_additional_nights_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_additional_nights.SelectedIndexChanged

        If String.IsNullOrEmpty(hfd_checkInDate.Value) = False And cbx_additional_nights.Checked = True Then
            lbl_reservation_checkout.Text = DateTime.Parse(hfd_checkInDate.Value).AddDays(Integer.Parse(ddl_nights.Text) + Integer.Parse(ddl_additional_nights.Text))
            hfd_checkOutDate.Value = lbl_reservation_checkout.Text
        End If

    End Sub

    Protected Sub cbx_additional_nights_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbx_additional_nights.CheckedChanged
        If cbx_additional_nights.Checked Then
            ddl_additional_nights_SelectedIndexChanged(Nothing, EventArgs.Empty)
        Else
            ddl_nights_SelectedIndexChanged(Nothing, EventArgs.Empty)
        End If
    End Sub

    Protected Sub dtf_reservation_checkin_Date_Updated() Handles dtf_reservation_checkin.Date_Updated
        hfd_checkInDate.Value = DateTime.Parse(dtf_reservation_checkin.Selected_Date).ToString("MM/dd/yyyy")
        cbx_additional_nights_CheckedChanged(Nothing, EventArgs.Empty)
    End Sub


    Protected Sub gvw_Inventories_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvw_Inventories.RowCommand

        lbl_view2_err.Text = gvw_Inventories.Rows(e.CommandArgument.ToString()).Cells(1).Text

        Charge_Package_Rate()
    End Sub



    Protected Sub gvw_Inventories_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvw_Inventories.RowCreated

        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim btn = DirectCast(e.Row.FindControl("btn_room_choose"), Button)
            btn.CommandArgument = e.Row.RowIndex.ToString()

        End If

    End Sub

#End Region
#Region "private functions"

    Private Sub Get_PackageIssued_ById(ByVal packageIssuedId As String)

        Using cnn = New SqlConnection(Resources.Resource.cns)
            Dim sql = String.Format("select (select comboitem from t_comboitems where comboitemid = pi.statusid) status, pi.Cost [PurchasedCost], * " & _
                                    "from t_packageissued pi inner join t_package p on pi.packageid = p.packageid where pi.packageissuedid = {0}", packageIssuedId)

            Using ada = New SqlDataAdapter(sql, cnn)
                Dim dt = New DataTable()

                ada.Fill(dt)

                If dt.Rows.Count = 0 Or dt.Rows.Count > 1 Then
                    For Each s In hiddenFields
                        s.Value = String.Empty
                    Next
                Else
                    hfd_packageId.Value = dt.Rows(0)("packageid")
                    hfd_packageIssuedId.Value = dt.Rows(0)("packageissuedid")
                    hfd_packageName.Value = dt.Rows(0)("package")
                    hfd_prospectId.Value = dt.Rows(0)("prospectid")
                    hfd_status.Value = dt.Rows(0)("status")
                    hfd_cost.Value = dt.Rows(0)("PurchasedCost")

                    lbl_programName.Text = PackageName
                End If
            End Using

        End Using

    End Sub

    Private Function IsInPeakedSeason(ByVal dateIn As DateTime) As Boolean

        Dim year = dateIn.Year

        If DateTime.Compare(dateIn, New DateTime(dateIn.Year, 3, 22)) >= 0 And DateTime.Compare(dateIn, New DateTime(dateIn.Year, 4, 7)) <= 0 Then
            Return True
        ElseIf DateTime.Compare(dateIn, New DateTime(dateIn.Year, 6, 10)) >= 0 And DateTime.Compare(dateIn, New DateTime(dateIn.Year, 9, 2)) <= 0 Then
            Return True
        ElseIf DateTime.Compare(dateIn, New DateTime(dateIn.Year, 11, 25)) >= 0 And DateTime.Compare(dateIn, New DateTime(dateIn.Year, 12, 1)) <= 0 Then
            Return True
        ElseIf DateTime.Compare(dateIn, New DateTime(dateIn.Year, 12, 19)) >= 0 And DateTime.Compare(dateIn, New DateTime(dateIn.Year, 12, 31)) <= 0 Then
            Return True
        ElseIf DateTime.Compare(dateIn, New DateTime(dateIn.Year + 1, 1, 1)) >= 0 And DateTime.Compare(dateIn, New DateTime(dateIn.Year + 1, 1, 2)) <= 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function IsInSMT(ByVal dateIn As DateTime) As Boolean

        If dateIn.DayOfWeek = System.DayOfWeek.Sunday Or dateIn.DayOfWeek = System.DayOfWeek.Monday Or dateIn.DayOfWeek = System.DayOfWeek.Tuesday Then
            Return True
        Else
            Return False
        End If
    End Function

   

    

    

    

  

    Private Function Show_Inventories() As String

        If String.IsNullOrEmpty(UnitType) Or String.IsNullOrEmpty(InventoryType) Then Return String.Empty

        For Each gvr As GridViewRow In gvw_Inventories.Rows
            gvw_Inventories.DeleteRow(gvr.RowIndex)
        Next

        Using cnn = New SqlConnection(Resources.Resource.cns)
            Dim sql = String.Format("select * from ufn_RoomsAvailable('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', 0) where available = 'available'", _
                                    ddl_room_size.Text, _
                                    ddl_unit_type.SelectedItem.Value, _
                                    hfd_checkInDate.Value, _
                                    hfd_checkOutDate.Value, _
                                    ddl_reservation_type.SelectedItem.Value, _
                                    ddl_unit_type.SelectedItem.Text, _
                                    ddl_reservation_type.SelectedItem.Text)


            Using ada = New SqlDataAdapter(sql, cnn)
                Dim dt = New DataTable()

                ada.Fill(dt)

                If dt.Rows.Count > 0 Then

                    lbl_view2_err.Text = dt.Rows(0)(0)

                    gvw_Inventories.DataSource = dt
                    gvw_Inventories.DataBind()


                Else
                    lbl_view2_err.Text = "No rows"
                End If
            End Using

        End Using

        Return ""
    End Function

    Private Function Charge_Package_Rate() As String

        Dim Czar3Nights() = {"CZAR-CW", "CZAR-FAB", "CZAR-FAB3", "CZAR-FB", "CZAR-FB3", "CZAR-FC", _
                          "CZAR-FC3", "CZAR-FM", "CZAR-FMB", "CZAR-FMB3", "CZAR-GCB", "CZAR-GCMB", _
                          "CZAR-GCMS", "CZAR-GCTN", "CZAR-GOLF", "CZAR-LV", "CZAR-LVBG", "CZAR-M", _
                          "CZAR-O", "CZAR-OCLC100", "CZAR-OCLC50", "CZAR-RQWB", "CZAR-RQWB3", "CZAR-RQWG", _
                          "CZAR-RQWG3", "CZAR-RQWLV", "CZAR-RQWLV3", "CZAR-RQWM", "CZAR-RQWM3", "CZAR-RQWMB", _
                          "CZAR-RQWMB3", "CZAR-RQWO", "CZAR-RQWO3", "CZAR-W", "CZAR-WB", "CZAR-WB3", _
                          "CZAR-WDH", "CZAR-WG", "CZAR-WG3", "CZAR-WLV", "CZAR-WLV3", "CZAR-WM", _
                          "CZAR-WM3", "CZAR-WMB", "CZAR-WMB3", "CZAR-WO", "CZAR-WO3"}

        Dim inHi_season = IsInPeakedSeason(CheckInDate)
        Dim in_SMT = IsInSMT(CheckInDate)
        Dim added_nights = cbx_additional_nights.Checked

        If Array.IndexOf(Czar3Nights, PackageName.ToUpper()) >= 0 Then

            If IsInPeakedSeason(CheckInDate) Then

                If IsInSMT(DateTime.Parse(CheckInDate)) Then

                    If cbx_additional_nights.Checked = True Then


                    Else
                        lbl_view3_err.Text = String.Format("Your rate ${0}", Decimal.Parse(Cost) + 149D)
                    End If

                Else
                    If cbx_additional_nights.Checked = True Then

                    Else

                    End If

                End If
            Else

                If cbx_additional_nights.Checked = True Then

                Else
                    lbl_view3_err.Text = String.Format("Your rate ${0:N2}", Cost)
                End If

            End If


        ElseIf Array.IndexOf(New String() {"MAL-TS", "G&P-TS", "PM-TS", "RECGOV-TS"}, PackageName.ToUpper()) >= 0 Then


            lbl_view3_err.Text = String.Format("Your overall rate ${0}", "784.44")

            'lbl_view3_err.Text = String.Format("Your overall rate ${0} {1}, {2}", InventoryType, Room, TotalNights)
        Else
            Return "not found"
        End If

        Return ""

    End Function


    


#End Region
#Region "private properties"
    Private ReadOnly Property PackageName As String
        Get
            Return IIf(String.IsNullOrEmpty(hfd_packageName.Value), String.Empty, hfd_packageName.Value)
        End Get
    End Property

    Private ReadOnly Property Cost As String
        Get
            If String.IsNullOrEmpty(hfd_cost.Value) Then
                Return "0.00"
            Else
                Return Decimal.Parse(hfd_cost.Value).ToString("N2")
            End If
        End Get
    End Property

    Private ReadOnly Property CheckInDate As String
        Get
            Return IIf(String.IsNullOrEmpty(hfd_checkInDate.Value), DateTime.MinValue.ToString(), hfd_checkInDate.Value)
        End Get
    End Property

    Private ReadOnly Property CheckOutDate As String
        Get
            Return IIf(String.IsNullOrEmpty(hfd_checkOutDate.Value), DateTime.MinValue.ToString(), hfd_checkOutDate.Value)
        End Get
    End Property

    Private ReadOnly Property Status As String
        Get
            Return hfd_status.Value
        End Get
    End Property

    Private ReadOnly Property InventoryType As String
        Get
            Return ddl_reservation_type.SelectedItem.Text
        End Get
    End Property

    Private ReadOnly Property IsInHighSeason As Boolean
        Get
            Return IsInPeakedSeason(CheckInDate)
        End Get
    End Property

    Private ReadOnly Property IsSMT As Boolean
        Get
            Return IsInSMT(CheckInDate)
        End Get
    End Property

    Private ReadOnly Property TotalNights As Integer
        Get
            Return DateDiff("d", CheckInDate, CheckOutDate)
        End Get
    End Property

    Private ReadOnly Property UnitType As String
        Get
            Return ddl_unit_type.SelectedItem.Text
        End Get
    End Property

    Private ReadOnly Property Room As String
        Get
            If ddl_room_size.SelectedItem.Text.Length = 1 Then
                Return String.Format("{0}BR {1}", ddl_room_size.SelectedItem.Text, UnitType)
            Else
                Return String.Format("{0} {1}", ddl_room_size.SelectedItem.Text, UnitType)
            End If
        End Get
    End Property





#End Region


#Region "private classes"


    Private Interface IRoomQuote

        Function Quote() As Decimal
    End Interface


    Private Class BaseRate
        Implements IRoomQuote

        Dim mal_normal_rates_nohooked As Integer(,) = {{84, 159, 239, -1, 115, 219, 320, -1}, {159, 299, -1, -1, 219, 419, -1, -1}, {109, 209, 309, 379, 149, 279, 409, 519}}
        Dim mal_holiday_rates_2ni As Integer(,) = {{84, 156, 0, -1, 95, 180, 0, -1}, {156, 0, -1, -1, 180, 0, -1, -1}, {99, 180, 0, 0, 113, 204, 0, 0}}
        Dim mal_holiday_rates_3ni4 As Integer(,) = {{73, 136, 0, -1, 83, 157, 0, -1}, {136, 0, -1, -1, 157, 0, -1, -1}, {85, 157, 0, 0, 99, 178, 0, 0}}

        Dim rate_owner_getaways As Integer(,) = {{79, 129, 213, -1, 99, 149, 264, -1}, {129, 288, -1, -1, 149, 368, -1, -1}, {99, 139, 248, 357, 109, 159, 308, 457}}
        Dim mal_entire_stay As Integer() = {111, 166, 0, -1, 166, 0, -1, -1, 122, 166, 0, 0}

        Dim _table As DataTable
        Dim _checkinDate As DateTime
        Dim _unit As String
        Dim _size As Integer
        Dim _tradeShow As String
        Dim _hooked As Boolean
        Dim _nights As Integer
        Dim _inventory As String

        ''' <summary>
        ''' Possible value is either marketing or rental
        ''' </summary>
        Protected Friend Property Inventory As String
            Get
                Return _inventory
            End Get
            Set(ByVal value As String)
                _inventory = value
            End Set
        End Property

        Protected Friend Property Nights As Integer
            Get
                Return _nights
            End Get
            Set(ByVal value As Integer)
                _nights = value
            End Set
        End Property

        ''' <summary>
        ''' Different rate when tour is hooked with a package
        ''' </summary>       
        Protected Friend Property Hooked As Boolean
            Get
                Return _hooked
            End Get
            Set(ByVal value As Boolean)
                _hooked = value
            End Set
        End Property

        Protected Friend Property CheckinDate As DateTime
            Get
                Return _checkinDate
            End Get
            Set(ByVal value As DateTime)
                _checkinDate = value
            End Set
        End Property

        Protected Friend Property Unit As String
            Get
                Return _unit
            End Get
            Set(ByVal value As String)
                _unit = value
            End Set
        End Property

        Protected Friend Property Size As Integer
            Get
                Return _size
            End Get
            Set(ByVal value As Integer)
                _size = value
            End Set
        End Property

        ''' <summary>
        ''' Possible values are 2 night promo, 3 or 4 night promo, or getaways
        ''' </summary>
        Protected Friend Property TradeShow As String
            Get
                Return _tradeShow
            End Get
            Set(ByVal value As String)
                _tradeShow = value
            End Set
        End Property


        Protected Friend ReadOnly Property HiSeason As Boolean
            Get
                If DateTime.Compare(CheckinDate, New DateTime(CheckinDate.Year, 3, 22)) >= 0 And DateTime.Compare(CheckinDate, New DateTime(CheckinDate.Year, 4, 7)) <= 0 Then
                    Return True
                ElseIf DateTime.Compare(CheckinDate, New DateTime(CheckinDate.Year, 6, 10)) >= 0 And DateTime.Compare(CheckinDate, New DateTime(CheckinDate.Year, 9, 2)) <= 0 Then
                    Return True
                ElseIf DateTime.Compare(CheckinDate, New DateTime(CheckinDate.Year, 11, 25)) >= 0 And DateTime.Compare(CheckinDate, New DateTime(CheckinDate.Year, 12, 1)) <= 0 Then
                    Return True
                ElseIf DateTime.Compare(CheckinDate, New DateTime(CheckinDate.Year, 12, 19)) >= 0 And DateTime.Compare(CheckinDate, New DateTime(CheckinDate.Year, 12, 31)) <= 0 Then
                    Return True
                ElseIf DateTime.Compare(CheckinDate, New DateTime(CheckinDate.Year + 1, 1, 1)) >= 0 And DateTime.Compare(CheckinDate, New DateTime(CheckinDate.Year + 1, 1, 2)) <= 0 Then
                    Return True
                Else
                    Return False
                End If
            End Get
        End Property

        Protected Friend ReadOnly Property IsSMT As Boolean
            Get
                Return IIf( _
                    CheckinDate.DayOfWeek = System.DayOfWeek.Sunday Or _
                    CheckinDate.DayOfWeek = System.DayOfWeek.Monday Or _
                    CheckinDate.DayOfWeek = System.DayOfWeek.Tuesday, True, False)
            End Get
        End Property


        Protected Friend ReadOnly Property NoHookedRate As Double
            Get
                If Hooked = False Then
                    Dim arr() = New String() {"Cottage", "Townes", "Estates"}

                    If HiSeason Then

                        If Unit.CompareTo("Townes") = 0 Then
                            If Size = 2 Then

                                Return mal_normal_rates_nohooked(Array.IndexOf(arr, Unit), 4)

                            ElseIf Size = 4 Then
                                Return mal_normal_rates_nohooked(Array.IndexOf(arr, Unit), 5)
                            End If
                        Else
                            Return mal_normal_rates_nohooked(Array.IndexOf(arr, Unit), 3 + Size)
                        End If

                    Else

                        If Unit.CompareTo("Townes") = 0 Then
                            If Size = 2 Then
                                Return mal_normal_rates_nohooked(Array.IndexOf(arr, Unit), 0)
                            ElseIf Size = 4 Then
                                Return mal_normal_rates_nohooked(Array.IndexOf(arr, Unit), 1)
                            End If
                        Else
                            Return mal_normal_rates_nohooked(Array.IndexOf(arr, Unit), Size - 1)
                        End If
                    End If
                Else
                    Return 0D
                End If

                Return 0D
            End Get
        End Property



        Protected Friend ReadOnly Property HookedRate As Double
            Get
                Dim span As TimeSpan = DateTime.Now.AddYears(1) - DateTime.Now
                Dim arr() = New String() {"Cottage", "Townes", "Estates"}

                If Hooked Then

                    For i = 0 To span.Days

                        Dim day = DateTime.Now.AddDays(i)

                        If DateTime.Compare(CheckinDate, New DateTime(day.Year, 6, 10)) >= 0 And DateTime.Compare(CheckinDate, New DateTime(day.Year, 9, 2)) <= 0 Then

                            If TradeShow.CompareTo("3 or 4 night promo") = 0 Then

                                If Unit = "Townes" Then
                                    If Size = 2 Then
                                        Return mal_holiday_rates_3ni4(Array.IndexOf(arr, Unit), 4)
                                    Else
                                        Return 0D
                                    End If
                                Else
                                    Return mal_holiday_rates_3ni4(Array.IndexOf(arr, Unit), Size + 3)
                                End If

                            ElseIf TradeShow.CompareTo("2 night promo") = 0 Then
                                If Unit = "Townes" Then
                                    If Size = 2 Then
                                        Return mal_holiday_rates_3ni4(Array.IndexOf(arr, Unit), 4)
                                    Else
                                        Return 0D
                                    End If
                                Else
                                    Return mal_holiday_rates_2ni(Array.IndexOf(arr, Unit), Size + 3)
                                End If
                            End If

                        Else
                            If New DateTime(CheckinDate.Year, CheckinDate.Month, CheckinDate.Day) = New DateTime(day.Year, day.Month, day.Day) Then

                                If TradeShow.CompareTo("3 or 4 night promo") = 0 Then

                                    If Unit = "Townes" Then
                                        If Size = 2 Then
                                            Return mal_holiday_rates_3ni4(Array.IndexOf(arr, Unit), 0)
                                        Else
                                            Return 0D
                                        End If
                                    Else
                                        Return mal_holiday_rates_3ni4(Array.IndexOf(arr, Unit), Size - 1)

                                    End If

                                ElseIf TradeShow.CompareTo("2 night promo") = 0 Then
                                    If Unit = "Townes" Then
                                        If Size = 2 Then
                                            Return mal_holiday_rates_3ni4(Array.IndexOf(arr, Unit), 4)
                                        Else
                                            Return 0D
                                        End If
                                    Else
                                        Return mal_holiday_rates_2ni(Array.IndexOf(arr, Unit), Size - 1)
                                    End If
                                End If
                            End If
                        End If
                    Next
                End If
                Return 0
            End Get
        End Property


        Private ReadOnly Property GetawaysRate As Double
            Get
                Dim arr() = New String() {"Cottage", "Townes", "Estates"}

                If HiSeason Then

                    If Unit.CompareTo("Townes") = 0 Then
                        If Size = 2 Then                            
                            Return rate_owner_getaways(Array.IndexOf(arr, Unit), 4)
                        ElseIf Size = 4 Then
                            Return rate_owner_getaways(Array.IndexOf(arr, Unit), 5)
                        End If
                    Else
                        Return rate_owner_getaways(Array.IndexOf(arr, Unit), 3 + Size)
                    End If
                
                Else
                    If Unit.CompareTo("Townes") = 0 Then
                        If Size = 2 Then
                            Return rate_owner_getaways(Array.IndexOf(arr, Unit), 0)
                        ElseIf Size = 4 Then
                            Return rate_owner_getaways(Array.IndexOf(arr, Unit), 1)
                        End If
                    Else
                        Return rate_owner_getaways(Array.IndexOf(arr, Unit), Size - 1)
                    End If
                End If

                Return 0
            End Get
        End Property

        Private ReadOnly Property MarketingRate As Double
            Get

                If Nights < 3 Or Nights > 4 Then Return 0D

                If Inventory.CompareTo("Marketing") = 0 Then

                    If Unit.CompareTo("Cottage") = 0 Then

                        If Size = 1 Then
                            Return mal_entire_stay(0)

                        ElseIf Size = 2 Then
                            Return mal_entire_stay(1)
                        Else
                            Return -1
                        End If

                    ElseIf Unit.CompareTo("Townes") = 0 Then

                        If Size = 2 Then
                            Return mal_entire_stay(4)
                        Else
                            Return -1
                        End If

                    ElseIf Unit.CompareTo("Estates") = 0 Then
                        If Size = 1 Then
                            Return mal_entire_stay(8)

                        ElseIf Size = 2 Then
                            Return mal_entire_stay(9)
                        Else
                            Return -1
                        End If
                    End If
                End If
                Return 0D
            End Get
        End Property

        ''' <summary>
        ''' Implemented interface IRoomQuote function
        ''' </summary>       
        Public Function Quote() As Decimal Implements IRoomQuote.Quote

            If Validate() Then
                If Inventory.CompareTo("Marketing") = 0 And String.IsNullOrEmpty(TradeShow) Then

                    Return MarketingRate

                ElseIf (Nights.CompareTo(3) = 0 Or Nights.CompareTo(4) = 0) And TradeShow.CompareTo("getaways") = 0 Then

                    Return GetawaysRate

                ElseIf Hooked And (TradeShow.CompareTo("3 or 4 night promo") = 0 Or TradeShow.CompareTo("2 night promo") = 0) Then

                    Return HookedRate

                ElseIf Hooked = False Then
                    Return NoHookedRate
                End If

            End If

            Return 0D
        End Function

        Protected Function Validate() As Boolean
            Return True
        End Function

        Public Sub New()

            _table = New DataTable

            _table.Columns.Add("RowNumber", GetType(Integer))
            _table.Columns.Add("Unit", GetType(String))
            _table.Columns.Add("Size", GetType(Integer))
            _table.Columns.Add("Minimum", GetType(Decimal))

            _table.PrimaryKey = New DataColumn() {_table.Columns(0)}

            Dim row = _table.NewRow()
            row("RowNumber") = 1
            row("Unit") = "Cottage"
            row("Size") = 1

            _table.Rows.Add(row)



        End Sub

    End Class


    Private Class OwnerGetAway
        Inherits BaseRate

        Private _additionalNights As Integer

        Protected Friend Property AdditionalNights As Integer
            Get
                Return _additionalNights
            End Get
            Set(ByVal value As Integer)
                _additionalNights = value
            End Set
        End Property

        Public Sub New()

            TradeShow = "getaways"
            Inventory = ""

        End Sub

               
       
        ''' <summary>
        ''' Overloads the base Quote fuction
        ''' </summary> 
        ''' 
        Public Function RateQuote() As Decimal

            If Validate() = False Then Return 0

            Dim q = 0D
            Dim tmp_checkIn As DateTime = CheckinDate
            Dim tmp_TradeShow = TradeShow

            If Nights.CompareTo(3) = 0 Or Nights.CompareTo(4) = 0 Then
                q = MyBase.Quote() * Nights
            End If

            If AdditionalNights > 0 And Nights = 4 Then

                CheckinDate = CheckinDate.AddDays(Nights)
                Hooked = False
                TradeShow = String.Empty

                q = (MyBase.Quote() * AdditionalNights) + q
            End If

            MyBase.CheckinDate = tmp_checkIn
            TradeShow = tmp_TradeShow

            Return q
        End Function

        Private Overloads Function Validate() As Boolean
            If (Unit = "Townes" And (Size = 3 Or Size = 1)) Or (Unit = "Cottage" And (Size = 3 Or Size = 4)) _
              Or (Unit = "Estates" And (Size = 3 Or Size = 4)) Then
                Return False
            End If

            Return True
        End Function

    End Class


    Private Class OwnerGetAwayUpgradeUnit
        Inherits OwnerGetAway

        Public Sub New()
            MyBase.New()
        End Sub

        Private Overloads Function Validate() As Boolean
            If Unit = "Townes" And (Size = 1 Or Size = 2 Or Size = 3) Or (Unit = "Cottage" And Size = 4) Then
                Return False
            End If

            If (Unit = "Cottage" And (Size = 1 Or Size = 2)) Or (Unit = "Estates" And (Size = 1 Or Size = 2)) Then
                Return False
            End If
            Return True
        End Function
        

        Public Overloads Function RateQuote() As Decimal

            If Validate() = False Then Return 0D

            Dim ret = IIf(Nights.CompareTo(3) = 0 Or Nights.CompareTo(4) = 0, MyBase.Quote() * Nights, 0)

            If AdditionalNights > 0 And Nights = 4 Then

                Dim tmp_TradeShow = TradeShow

                CheckinDate = CheckinDate.AddDays(Nights)
                Hooked = False
                TradeShow = String.Empty

                ret += MyBase.Quote() * AdditionalNights

                CheckinDate = CheckinDate.AddDays(-Nights)
                TradeShow = tmp_TradeShow
            End If

            Return ret
        End Function
    End Class

    Private Class MalBaseRate
        Inherits BaseRate

        Private _additionalNights As Integer

        Protected ReadOnly Property TaxRate As Decimal
            Get
                If Nights = 2 Then
                    Return 2.55D
                ElseIf Nights = 3 Then
                    Return 2.46D
                ElseIf Nights = 4 Then
                    Return 2.36D
                Else
                    Return 0
                End If
            End Get
        End Property

        Public Sub New()
            TradeShow = String.Empty
            Hooked = True
        End Sub

        Protected Friend Property AdditionalNights As Integer
            Get
                Return _additionalNights
            End Get
            Set(ByVal value As Integer)
                _additionalNights = value
            End Set
        End Property

        Public Overloads Function RateQuote() As Decimal

            Dim ret = 0D

            If MyBase.Inventory.CompareTo("Marketing") = 0 Then

                ret = MyBase.Quote()

            ElseIf MyBase.Inventory.CompareTo("Rental") = 0 And (TradeShow.Equals("3 or 4 night promo") Or TradeShow.Equals("2 night promo")) Then

                If Nights = 2 Then

                ElseIf Nights = 3 Or Nights = 4 Then
                    ret = MyBase.Quote()
                End If
            End If

            If AdditionalNights > 0 And Nights = 4 And ret > 0 Then
                Dim tmp_inventory = Inventory

                Hooked = False
                Inventory = String.Empty

                CheckinDate = CheckinDate.AddDays(AdditionalNights)
                ret += MyBase.Quote() * AdditionalNights

                CheckinDate = CheckinDate.AddDays(-AdditionalNights)

                Inventory = tmp_inventory
            End If

            Return ret
        End Function
    End Class


    


    
#End Region


    Protected Sub btn_GET_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_GET.Click


        'Usage
        'Owner Getaways
        Dim getaWay = New OwnerGetAway()
        getaWay.Nights = ddl_nights.SelectedItem.Text
        getaWay.CheckinDate = dtf_reservation_checkin.Selected_Date
        getaWay.Unit = ddl_unit_type.SelectedItem.Text
        getaWay.Size = ddl_room_size.SelectedItem.Text.Substring(0, 1)

        If cbx_additional_nights.Checked And getaWay.Nights = 4 Then
            getaWay.AdditionalNights = ddl_additional_nights.SelectedItem.Text
        End If


        lbl_view1_err.Text = String.Empty


        If ddl_reservation_type.SelectedItem.Text.Equals("Owner") Then
            lbl_view1_err.Text = String.Format("<br><strong>{0}</strong>", ddl_reservation_type.SelectedItem.Text)
            Dim unit = ddl_unit_type.SelectedItem.Text
            Dim size = Integer.Parse(ddl_room_size.SelectedItem.Text.Substring(0, 1))

            If (unit = "Cottage" And size = 3) Or (unit = "Estates" And (size = 3 Or size = 4)) Or (unit = "Townes" And size = 4) Then

                Dim getAwayUp = New OwnerGetAwayUpgradeUnit()
                getAwayUp.Nights = ddl_nights.SelectedItem.Text
                getAwayUp.CheckinDate = dtf_reservation_checkin.Selected_Date
                getAwayUp.Unit = ddl_unit_type.SelectedItem.Text
                getAwayUp.Size = Integer.Parse(ddl_room_size.SelectedItem.Text.Substring(0, 1))

                If cbx_additional_nights.Checked And getAwayUp.Nights = 4 Then
                    getAwayUp.AdditionalNights = ddl_additional_nights.SelectedItem.Text
                End If

                lbl_view1_err.Text = String.Format("<br/><strong>Quote ${0} Check-in: {1:MM/dd/yyyy} - Packpage: <italic>{2}</italic></strong>", getAwayUp.RateQuote(), getAwayUp.CheckinDate.ToShortDateString(), getAwayUp.TradeShow)
            Else
                lbl_view1_err.Text = String.Format("<br/><strong>Quote ${0} Check-in: {1:MM/dd/yyyy}</strong>", getaWay.RateQuote(), getaWay.CheckinDate.ToString())
            End If

        ElseIf ddl_reservation_type.SelectedItem.Text.Trim().Equals("Rental") Or ddl_reservation_type.SelectedItem.Text.Trim().Equals("Marketing") Then

            Try
                Dim mal = New MalBaseRate()
                mal.Nights = ddl_nights.SelectedItem.Text
                mal.CheckinDate = dtf_reservation_checkin.Selected_Date
                mal.Unit = ddl_unit_type.SelectedItem.Text
                mal.Size = ddl_room_size.SelectedItem.Text.Substring(0, 1)

                mal.Inventory = ddl_reservation_type.SelectedItem.Text.Trim()

                If cbx_additional_nights.Checked And mal.Nights = 4 Then
                    mal.AdditionalNights = ddl_additional_nights.SelectedItem.Text
                End If

                mal.TradeShow = "3 or 4 night promo"

                lbl_view1_err.Text = String.Format("<br/><strong>Marketing Inventory Quote ${0} Check-in: {1}</strong>", mal.RateQuote(), mal.CheckinDate.ToShortDateString())

            Catch ex As Exception
                lbl_view1_err.Text = String.Format("<br/><strong>Error: {0}</strong>", ex.Message)
            End Try


        Else

            lbl_view1_err.Text = String.Format("<br><strong>{0}</strong>", ddl_reservation_type.SelectedItem.Text)
        End If

      

    End Sub
End Class
