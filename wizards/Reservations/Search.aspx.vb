Imports System.Data
Imports System.Reflection
Imports System.Data.SqlClient
Imports System.Web.Services
Imports System.Collections.Generic
Imports System.Web.Script.Serialization
Imports System.Web.Script.Services

Imports clsReservationWizard


Partial Class wizard_Reservations_Search
    Inherits System.Web.UI.Page

    Private wiz As New Wizard
    Private package_base As New Base_Package
    Private accom_name As String = "KCP"
    Private Structure des_Package
        Public parent_package_id As Int32
        Public child_package_id As Int32
        Public parent_guid As String
        Public child_guid As String
        Public price_max As Decimal
        Public price_min As Decimal
    End Structure

#Region "Event Handlers"

    Private Sub wizard_Reservations_Search_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim ph = CType(Me.Master.Master.FindControl("ContentPlaceHolder1"), ContentPlaceHolder)
        Dim wiz_data = CType(ph.FindControl("LB_WIZ_DATA"), Label)

        If Session("wizData" + Session("wizGuid")) <> Nothing Then
            wiz = New JavaScriptSerializer().Deserialize(Of Wizard)(Session("wizData" + Session("wizGuid")))
        Else
            wiz = New JavaScriptSerializer().Deserialize(Of Wizard)(wiz_data.Text)
        End If
        Session("wizData" + wiz.GUID_TIMESTAMP) = New JavaScriptSerializer().Serialize(wiz)
        Session("wizGuid") = wiz.GUID_TIMESTAMP

        dtT.Attributes.Add("readonly", "readonly")
        datePicker1.Attributes.Add("readonly", "readonly")
        spErr.InnerHtml = ""

        Try
            Dim date_parts = dtT.Text.Split("/")
            If date_parts.Length = 3 Then
                Dim year = date_parts(2).ToCharArray().Where(Function(x) Char.IsNumber(x) = True).ToArray()
                Dim month = date_parts(0).ToCharArray().Where(Function(x) Char.IsNumber(x) = True).ToArray()
                Dim day = date_parts(1).ToCharArray().Where(Function(x) Char.IsNumber(x) = True).ToArray()

                wiz.Search_CheckIn_Date = DateTime.Parse(datePicker1.Text)
                wiz.Search_CheckOut_date = New DateTime(Integer.Parse(year), Integer.Parse(month), Integer.Parse(day))

            Else
                If String.IsNullOrEmpty(datePicker1.Text) = False And String.IsNullOrEmpty(dtT.Text.Trim()) = False Then

                    wiz.Search_CheckIn_Date = DateTime.Parse(datePicker1.Text)
                    wiz.Search_CheckOut_date = DateTime.Parse(DateTime.Parse(dtT.Text.Trim()))
                End If
            End If
        Catch ex As Exception
            spErr.InnerHtml = "Error parsing Check-Out date, please re-select Check-In"
            Return
        End Try

        Dim hide_controls As Action =
            Sub()
                rResort.Visible = False
                rHotel.Visible = False
                cbAuto.Visible = False
                lbAutoRoomSelect.Visible = False
                gvToursAvail.Visible = False
                ddUnit.Visible = False
                ddPackage.Visible = False
                lbMin.Visible = False
                lbMax.Visible = False
                lbPurchase.Visible = False
                lbUnit.Visible = False
                lbPackage.Visible = False
                txMin.Visible = False
                txMax.Visible = False
                txP.Visible = False
                btNext.Enabled = False
                If wiz.Scenario = EnumScenario.One Then
                    ddUnit.Visible = True
                    ddPackage.Visible = True
                End If
            End Sub
        hide_controls()

        If IsPostBack = False Then
            Dim dd_nights = 0
            Select Case wiz.Scenario
                Case EnumScenario.One
                    If Not Session("Search" + wiz.GUID_TIMESTAMP) Is Nothing Then
                        gvPackages.DataSource = CType(Session("Search" + wiz.GUID_TIMESTAMP), DataTable)
                        gvPackages.DataBind()
                        gvPackages.Enabled = False
                    End If

                    ddStayDays.ClearSelection()
                    ddStayDays.Items.Clear()
                    ddStayDays.DataSource = Enumerable.Range(1, (15))
                    ddStayDays.DataBind()
                    ddStayDays.SelectedIndex = 2

                    btNext.Enabled = True

                    txMin.Visible = True
                    txMax.Visible = True
                    txP.Visible = True
                    lbMin.Visible = True
                    lbMax.Visible = True
                    lbPurchase.Visible = True

                    btNext.Enabled = IIf(wiz.Packages.Count() = 0, False, True)

                Case EnumScenario.Two

                    accom_name = package_base.Get_Accom_Name(wiz.Packages(0).PackageID)

                    If accom_name = "KCP" Then
                        hf_default_package_id.Value = wiz.Packages(0).PackageID

                        ddStayDays.Attributes.Add("scenario", "2")

                        If Not Session("Search" + wiz.GUID_TIMESTAMP) Is Nothing Then

                            gridview1.DataKeyNames = New String() {"PACKAGEID"}
                            gridview1.DataSource = CType(Session("Search" + wiz.GUID_TIMESTAMP), DataTable)
                            gridview1.DataBind()

                            btNext.Enabled = True
                            dd_nights = package_base.Get_Promo_Nights(wiz.Packages(0).PackageID)
                            ddStayDays.ClearSelection()
                            ddStayDays.Items.Clear()
                            ddStayDays.DataSource = Enumerable.Range(dd_nights, (10 + dd_nights - 1))
                            ddStayDays.DataBind()

                            If wiz.Search_CheckIn_Date.HasValue And wiz.Search_CheckOut_date.HasValue Then
                                dtT.Text = wiz.Search_CheckOut_date.Value.ToShortDateString()
                                ddStayDays.Items.FindByText(wiz.Search_CheckOut_date.Value.Subtract(wiz.Search_CheckIn_Date).Days).Selected = True
                            End If

                        Else
                            ddStayDays.ClearSelection()
                            ddStayDays.Items.Clear()
                        End If
                    Else
                        hf_default_package_id.Value = wiz.Packages(0).PackageID
                        ddStayDays.Attributes.Add("scenario", "2")
                        ddStayDays.Enabled = True

                        If Not Session("Search" + wiz.GUID_TIMESTAMP) Is Nothing Then

                            gridview_hotel.DataKeyNames = New String() {"PACKAGEID"}
                            gridview_hotel.DataSource = CType(Session("Search" + wiz.GUID_TIMESTAMP), DataTable)
                            gridview_hotel.DataBind()

                            btNext.Enabled = True
                            dd_nights = package_base.Get_Promo_Nights(wiz.Packages(0).PackageID)
                            ddStayDays.ClearSelection()
                            ddStayDays.Items.Clear()
                            Dim counter() As Int16 = {2, 3, 4}
                            ddStayDays.DataSource = counter.ToArray()
                            ddStayDays.DataBind()

                            If wiz.Search_CheckIn_Date.HasValue And wiz.Search_CheckOut_date.HasValue Then
                                dtT.Text = wiz.Search_CheckOut_date.Value.ToShortDateString()
                                ddStayDays.Items.FindByText(wiz.Search_CheckOut_date.Value.Subtract(wiz.Search_CheckIn_Date).Days).Selected = True
                            End If

                        Else
                            ddStayDays.ClearSelection()
                            ddStayDays.Items.Clear()
                        End If

                    End If


                Case EnumScenario.Three

                    If wiz.Packages.Count = 0 Then
                        spErr.InnerHtml = "Package can't be found probably the reservation was not booked through the wizard."
                    Else
                        accom_name = package_base.Get_Accom_Name(wiz.Packages(0).PackageID)

                        If accom_name = "KCP" Then
                            hf_default_package_id.Value = wiz.Packages(0).PackageID
                            ddStayDays.Attributes.Add("scenario", "3")
                            ddStayDays.Enabled = True

                            If Not Session("Search" + wiz.GUID_TIMESTAMP) Is Nothing Then
                                ll1.Text = CType(Session("Search" + wiz.GUID_TIMESTAMP), String)
                                txP.Attributes.Add("readonly", "readonly")
                                btNext.Enabled = True
                                ddStayDays.ClearSelection()
                                ddStayDays.Items.Clear()

                                Dim li = New List(Of Int32)
                                Dim package_type = package_base.Get_Package_Type(hf_default_package_id.Value)
                                Dim promo_nights = package_base.Get_Promo_Nights(0)
                                Dim min_nights = package_base.Get_Package_MinNights(0)
                                dd_nights = IIf(package_type = "Rental".ToLower(), min_nights, promo_nights)
                                For i = 15 To dd_nights Step -1
                                    li.Add(i)
                                Next
                                ddStayDays.DataSource = li.OrderBy(Function(x) x).ToArray()
                                ddStayDays.DataBind()

                                If wiz.Search_CheckIn_Date.HasValue Then
                                    datePicker1.Text = wiz.Search_CheckIn_Date.Value.ToShortDateString()
                                    dtT.Text = wiz.Search_CheckOut_date.Value.ToShortDateString()
                                    ddStayDays.Items.FindByText(wiz.Search_CheckOut_date.Value.Subtract(wiz.Search_CheckIn_Date.Value).Days).Selected = True
                                End If
                            Else
                                ddStayDays.ClearSelection()
                                ddStayDays.Items.Clear()
                                txP.Attributes.Remove("readonly")
                            End If
                            ddStayDays.Attributes.Add("readonly", "readonly")

                        Else

                            hf_default_package_id.Value = wiz.Packages(0).PackageID
                            ddStayDays.Attributes.Add("scenario", "3")
                            ddStayDays.Enabled = True

                            If Not Session("Search" + wiz.GUID_TIMESTAMP) Is Nothing Then

                                gridview_hotel.DataKeyNames = New String() {"PACKAGEID"}
                                gridview_hotel.DataSource = CType(Session("Search" + wiz.GUID_TIMESTAMP), DataTable)
                                gridview_hotel.DataBind()

                                btNext.Enabled = True
                                dd_nights = package_base.Get_Promo_Nights(wiz.Packages(0).PackageID)
                                ddStayDays.ClearSelection()
                                ddStayDays.Items.Clear()
                                Dim counter() As Int16 = {2, 3, 4}
                                ddStayDays.DataSource = counter.ToArray()
                                ddStayDays.DataBind()

                                If wiz.Search_CheckIn_Date.HasValue And wiz.Search_CheckOut_date.HasValue Then
                                    dtT.Text = wiz.Search_CheckOut_date.Value.ToShortDateString()
                                    ddStayDays.Items.FindByText(wiz.Search_CheckOut_date.Value.Subtract(wiz.Search_CheckIn_Date).Days).Selected = True
                                End If
                            Else
                                ddStayDays.ClearSelection()
                                ddStayDays.Items.Clear()
                            End If
                        End If
                    End If
            End Select
        Else
            If gridview1.Rows.Count = 0 Then
                gridview1.EmptyDataText = ""
                gridview1.DataBind()
            End If
        End If
    End Sub
    Protected Sub ddStayDays_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddStayDays.SelectedIndexChanged
        If dtF.Text.Length > 0 And dtT.Text.Length > 0 Then
            dtT.Text = DateTime.Parse(dtF.Text).AddDays(ddStayDays.SelectedItem.Text)
        End If
    End Sub
    Protected Sub btSearch_Click(sender As Object, e As System.EventArgs) Handles btSearch.Click

        txP.Attributes.Remove("readonly")
        If String.IsNullOrEmpty(datePicker1.Text.Trim()) Then Return
        If String.IsNullOrEmpty(dtT.Text.Trim()) Then Return

        Dim tour_location_id = 0
        Dim campaign_id = 0
        Dim checkin_date As DateTime
        Dim nights_stay = 0
        Dim package_id = 0
        Dim accom_name = String.Empty
        Dim extra_night = False

        If wiz.Scenario = EnumScenario.Two Or wiz.Scenario = EnumScenario.Three Then
            tour_location_id = wiz.Tour.TourLocationID
            campaign_id = wiz.Tour.CampaignID
            checkin_date = DateTime.Parse(wiz.Search_CheckIn_Date.Value)
            nights_stay = DateTime.Parse(wiz.Search_CheckOut_date.Value).Subtract(DateTime.Parse(wiz.Search_CheckIn_Date.Value)).Days
            package_id = package_base.Get_Package_PackageID(wiz.Reservation.ReservationID)
        End If

        Select Case wiz.Scenario
            Case EnumScenario.One
                gvPackages.DataSource = Nothing
                gvPackages.DataBind()
                gvPackages.Enabled = True
                gvToursAvail.DataSource = Nothing
                gvToursAvail.DataBind()
                btNext.Enabled = False
                txMax.Text = "0.00"
                txMin.Text = "0.00"
                txP.Text = "0.00"
                spErr.InnerHtml = ""
                Session("Search" + wiz.GUID_TIMESTAMP) = Nothing
                Dim arr_p() As String = {}
                Dim arr_u() As String = {}

                If ddPackage.SelectedItem.Text.Equals("All") Then
                    For Each li In ddPackage.Items.OfType(Of ListItem).Where(Function(x) x.Value <> "All")
                        Array.Resize(arr_p, arr_p.Length + 1)
                        arr_p(arr_p.Length - 1) = li.Value
                    Next
                Else
                    Array.Resize(arr_p, 1)
                    arr_p(0) = ddPackage.SelectedItem.Value
                End If
                If ddUnit.SelectedItem.Text.Equals("All") Then
                    For Each li In ddUnit.Items.OfType(Of ListItem).Where(Function(x) x.Value <> "All")
                        Array.Resize(arr_u, arr_u.Length + 1)
                        arr_u(arr_u.Length - 1) = li.Value
                    Next
                Else
                    Array.Resize(arr_u, 1)
                    arr_u(0) = ddUnit.SelectedItem.Value
                End If
                Try
                    Dim IFind As IFind = Nothing
                    If Request.RawUrl.ToLower().IndexOf("kcpcrms".ToLower()) < 1 Then
                        IFind = New VrcPackagesAvailable(arr_p, arr_u)
                    Else
                        Dim vendorID = CType(Session("ActiveVendorID"), Int32)
                        With New clsVendor()
                            .VendorID = vendorID
                            .Load()
                            If .Vendor.ToLower() = "Grand Incentives".ToLower() Then
                                IFind = New GrandIncentivesPackagesAvailable(arr_p, arr_u)

                            ElseIf .Vendor.ToLower() = "Complete Call Solutions".ToLower() Then
                                IFind = New CompleteCallSolutions(arr_p, arr_u)
                            Else
                                Return
                            End If
                        End With
                    End If

                    Dim dt = IFind.Search(wiz.Search_CheckIn_Date, ddStayDays.SelectedItem.Text)
                    gvPackages.DataSource = dt
                    gvPackages.DataBind()
                    gvPackages.Enabled = True

                    Session("Search" + wiz.GUID_TIMESTAMP) = dt

                    txMin.Visible = True
                    txMax.Visible = True
                    txP.Visible = True
                    lbMin.Visible = True
                    lbMax.Visible = True
                    lbPurchase.Visible = True

                    wiz.Packages.Clear()
                    wiz.Packages_Price = 0

                Catch ex As Exception_Packages_Available_None
                    spErr.InnerHtml = ex.Message
                    spErr.Visible = True
                Catch ex As Exception
                    spErr.InnerHtml = ex.Message
                    spErr.Visible = True
                End Try


            Case EnumScenario.Two

                accom_name = package_base.Get_Accom_Name(wiz.Packages(0).PackageID)

                If accom_name = "KCP" Then
                    Dim promo_nights = package_base.Get_Promo_Nights(package_id)

                    gridview1.Enabled = True

                    ddStayDays.DataSource = Enumerable.Range(promo_nights, 15 - promo_nights + 1)
                    ddStayDays.DataBind()
                    ddStayDays.Items.FindByText(nights_stay).Selected = True
                    Try
                        Dim s2 = New Book_Tour_Or_Tradeshow_Package
                        s2.Refresh(gridview1, campaign_id, tour_location_id, checkin_date, nights_stay, package_id)
                        Session("Search" + wiz.GUID_TIMESTAMP) = CType(gridview1.DataSource, DataTable)

                    Catch ex As Exception_Tour_Waves_Not_Available
                        gridview1.EmptyDataText = String.Format("<span style=color:red>There are no tours available between {0} and {1} in <b>{2}</b></span>",
                                    wiz.Search_CheckIn_Date.Value.ToShortDateString(),
                                    wiz.Search_CheckOut_date.Value.ToShortDateString(),
                                    New clsComboItems().Lookup_ComboItem(wiz.Tour.TourLocationID))
                        gridview1.DataBind()
                    Catch ex As ArgumentException
                        spErr.InnerHtml = ex.Message
                    Catch ex As Exception_Allocation_Room_Search_Not_Exist
                        gridview1.EmptyDataText = String.Format("<span style=color:red>There are no packages available between {0} and {1}</span>",
                                                                wiz.Search_CheckIn_Date.Value.ToShortDateString(),
                                                                wiz.Search_CheckOut_date.Value.ToShortDateString())
                        gridview1.DataBind()
                    Catch ex As Exception
                        spErr.InnerHtml = ex.Message
                    End Try

                Else
                    Dim promo_nights = package_base.Get_Promo_Nights(package_id)

                    gridview_hotel.Enabled = True

                    Dim counter() As Int16 = {2, 3, 4}
                    ddStayDays.DataSource = counter.ToArray()
                    ddStayDays.DataBind()
                    ddStayDays.Items.FindByText(nights_stay).Selected = True
                    ddStayDays.Enabled = True

                    Try
                        Dim s2 = New Book_Hotel_Reservation
                        s2.Refresh(gridview_hotel, campaign_id, tour_location_id, checkin_date, nights_stay, package_id)

                        Session("Search" + wiz.GUID_TIMESTAMP) = CType(gridview_hotel.DataSource, DataTable)

                    Catch ex As Exception_Packages_Available_None
                        gridview_hotel.EmptyDataText = String.Format("<span style=color:red><b>{0}</b></span>", ex.Message)
                        gridview_hotel.DataBind()
                    Catch ex As Exception
                        spErr.InnerHtml = ex.Message
                    End Try
                End If

            Case EnumScenario.Three

                accom_name = package_base.Get_Accom_Name(wiz.Packages(0).PackageID)
                If accom_name = "KCP" Then
                    Dim dd_nights = 0
                    If package_base.Get_Package_Type(package_id) = "Rental" Then
                        dd_nights = package_base.Get_Package_MinNights(package_id)
                    Else
                        dd_nights = package_base.Get_Promo_Nights(package_id) + IIf(package_base.Get_Package_Extra_Night_Allowed(package_id, checkin_date.ToShortDateString()), 1, 0)
                    End If

                    Dim li = New List(Of Int32)

                    gridview1.Enabled = False
                    gridview2.Enabled = True
                    gridview2.EmptyDataText = String.Empty
                    gridview2.ForeColor = Drawing.Color.Red
                    gridview2.DataBind()
                    txP.Text = "0"
                    For i = 15 To dd_nights Step -1
                        li.Add(i)
                    Next
                    ddStayDays.ClearSelection()
                    ddStayDays.Items.Clear()
                    ddStayDays.DataSource = li.OrderBy(Function(x) x).ToArray()
                    ddStayDays.DataBind()
                    ddStayDays.Items.FindByText(wiz.Search_CheckOut_date.Value.Subtract(wiz.Search_CheckIn_Date.Value).Days).Selected = True

                    Session("Search" + wiz.GUID_TIMESTAMP) = Nothing
                    Dim s2 = New RebookResortReservation

                    Try
                        ll1.Text = s2.Refresh(gridview2, wiz.Tour.TourID, campaign_id, tour_location_id, checkin_date, nights_stay, package_id, wiz.Packages)
                        If ll1.Text.Length > 0 Then
                            Session("Search" + wiz.GUID_TIMESTAMP) = ll1.Text
                            txMin.Visible = True
                            txMax.Visible = True
                            txP.Visible = True
                            lbMin.Visible = True
                            lbMax.Visible = True
                            lbPurchase.Visible = True
                        Else
                            spErr.InnerHtml = String.Format("Vendor {0} has no packages offered available between {1} and {2}", package_base.Get_Package_Vendor_By_VendorID(package_id), checkin_date.ToShortDateString(), checkin_date.AddDays(nights_stay).ToShortDateString())
                            spErr.Visible = True
                        End If

                        gridview2.ForeColor = Drawing.Color.Black
                    Catch ex As Exception
                        If TypeOf ex Is Exception_Tour_Waves_Not_Available Or TypeOf ex Is Exception_Allocation_Room_Search_Not_Exist Then
                            gridview2.EmptyDataText = ex.Message
                            gridview2.ForeColor = Drawing.Color.Red
                            gridview2.DataBind()

                        ElseIf TypeOf ex Is Exception_Package_Is_Not_Found_In_PackageIssued2Package_Table Then
                            gridview2.EmptyDataText = ex.Message
                            gridview2.ForeColor = Drawing.Color.Red
                            gridview2.DataBind()
                        ElseIf TypeOf ex Is Exception Then

                            gridview2.EmptyDataText = ex.Message
                            gridview2.ForeColor = Drawing.Color.Red
                            gridview2.DataBind()

                        Else
                            spErr.InnerHtml = ex.Message
                            gridview2.DataSource = Nothing
                            gridview2.DataBind()
                        End If

                    End Try
                Else
                    Dim promo_nights = package_base.Get_Promo_Nights(package_id)

                    gridview_hotel.Enabled = True

                    Dim counter() As Int16 = {2, 3, 4}
                    ddStayDays.DataSource = counter.ToArray()
                    ddStayDays.DataBind()
                    ddStayDays.Items.FindByText(nights_stay).Selected = True
                    ddStayDays.Enabled = True

                    Try
                        Dim s2 = New Book_Hotel_Reservation
                        s2.Refresh(gridview_hotel, campaign_id, tour_location_id, checkin_date, nights_stay, package_id)

                        Session("Search" + wiz.GUID_TIMESTAMP) = CType(gridview_hotel.DataSource, DataTable)

                    Catch ex As Exception_Packages_Available_None
                        gridview_hotel.EmptyDataText = String.Format("<span style=color:red><b>{0}</b></span>", ex.Message)
                        gridview_hotel.DataBind()
                    Catch ex As Exception
                        spErr.InnerHtml = ex.Message
                    End Try

                End If


        End Select
        Session("wizData" + wiz.GUID_TIMESTAMP) = New JavaScriptSerializer().Serialize(wiz)
        btNext.Enabled = True
    End Sub
    Protected Sub btNext_Click(sender As Object, e As System.EventArgs) Handles btNext.Click

        If wiz.Scenario = EnumScenario.One Then
            If gvPackages.Enabled Then
                wiz.Packages.Clear()
                wiz.Packages_Price = txP.Text

                Dim packages_temp = New List(Of Wizard_Package)

                For Each gvr As GridViewRow In gvPackages.Rows
                    If gvr.RowType = DataControlRowType.DataRow Then
                        Dim cb = CType(gvr.Cells(0).FindControl("cbS"), CheckBox)
                        Dim package_id = cb.Attributes("PackageID")

                        If cb.Checked Then
                            If Convert.ToBoolean(IIf(cb.Attributes("rental") = "0", False, True)) = True Then

                                For i = 0 To Convert.ToInt16(CType(gvr.Cells(4).FindControl("ddQ"), DropDownList).SelectedItem.Text) - 1
                                    packages_temp.Add(New Wizard_Package With {.PackageID = package_id, .GUID = Guid.NewGuid().ToString()})
                                Next
                            Else
                                packages_temp.Add(New Wizard_Package With {.PackageID = package_id, .GUID = Guid.NewGuid().ToString()})
                            End If
                        End If
                    End If
                Next

                For Each p_temp As Wizard_Package In packages_temp
                    If package_base.Get_Package_Type(p_temp.PackageID) = "Tour Promotion" Then
                        wiz.Packages.Add(New Wizard_Package With {.PackageID = p_temp.PackageID, .GUID = p_temp.GUID})
                    End If
                Next
                For Each p_temp As Wizard_Package In packages_temp
                    If package_base.Get_Package_Type(p_temp.PackageID) = "Owner Getaway" Then
                        wiz.Packages.Add(New Wizard_Package With {.PackageID = p_temp.PackageID, .GUID = p_temp.GUID})
                    End If
                Next
                For Each p_temp As Wizard_Package In packages_temp
                    If package_base.Get_Package_Type(p_temp.PackageID) = "Rental" Then
                        wiz.Packages.Add(New Wizard_Package With {.PackageID = p_temp.PackageID, .GUID = p_temp.GUID})
                    End If
                Next
            End If

        ElseIf wiz.Scenario = EnumScenario.Two Then

            If accom_name = "KCP" Then
                Dim package_id = gridview1.SelectedDataKey.Value
                wiz.Packages(0).Package = New Wizard_Package With {.PackageID = package_id, .GUID = Guid.NewGuid.ToString()}
            End If

        ElseIf wiz.Scenario = EnumScenario.Three Then

            wiz.Reservation.CheckInDate = wiz.Search_CheckIn_Date.Value
            wiz.Reservation.CheckOutDate = wiz.Search_CheckOut_date.Value

            If txP.Visible Then
                wiz.Packages_Price = txP.Text
            End If

            If accom_name = "KCP" Then
                'Modified 04/25/2019
                'If hf_default_package_id.Value.Length > 0 And wiz.Packages(0).Package Is Nothing Then
                If hf_default_package_id.Value.Length > 0 Then
                    Dim arr = New JavaScriptSerializer().Deserialize(Of List(Of des_Package))(hf_list_packages.Value)
                    For Each el In arr
                        Dim child_package_id = el.child_package_id
                        Dim parent_guid = el.parent_guid
                        Dim child_guid = el.child_guid

                        For Each p In wiz.Packages
                            If p.GUID = parent_guid Then
                                p.Package = New Wizard_Package With {.PackageID = child_package_id, .GUID = Guid.NewGuid.ToString()}
                            End If
                        Next
                    Next
                End If
            End If


        End If

        Session("wizData" + wiz.GUID_TIMESTAMP) = New JavaScriptSerializer().Serialize(wiz)
        Dim bt = CType(sender, Button)
        bt.Attributes.Add("nav", 1)
        Navigate(bt)

    End Sub
    Protected Sub btPrevious_Click(sender As Object, e As System.EventArgs) Handles btPrevious.Click
        Dim bt = CType(sender, Button)
        bt.Attributes.Add("nav", 0)
        Navigate(CType(sender, Button))
    End Sub

    Class Night
        Public Counter As Int16
        Public Selected As Boolean
    End Class

    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Shared Function GetNights(chkin As String, package_id As String) As String
        Dim l = New List(Of Night)
        Dim checkin_date = DateTime.Parse(chkin)

        Dim package_base = New Base_Package
        Dim promo_nights = package_base.Get_Promo_Nights(package_id)

        If package_base.Get_Accom_Name(package_id) = "KCP" Then
            For i = promo_nights To 15
                l.Add(New Night With {.Counter = i, .Selected = False})
            Next
            If package_base.Get_Package_Extra_Night_Allowed(package_id, checkin_date) Then
                For Each n In l
                    If n.Counter = promo_nights + 1 Then
                        n.Selected = True
                    End If
                Next
            End If
        Else
            For i = 2 To 4
                l.Add(New Night With {.Counter = i, .Selected = False})
            Next
        End If
        Return New JavaScriptSerializer().Serialize(l.ToArray())
    End Function

    <WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Shared Function ufn_ToursAvailability(package_id As Int32, tour_date As DateTime, nights_stay As Int32) As String
        Dim st = New List(Of WaveDetail)
        Dim campaign_id As Int32, tour_location_id As Int32, package_tour_id = 0
        With New clsPackageTour
            .Load(package_id, New clsPackageReservation().Find_Res_ID(package_id))
            package_tour_id = .PackageTourID
            .Load()
            campaign_id = .CampaignID
            tour_location_id = .TourLocationID
        End With
        Dim dt = (New Package_Xtra).Get_Tours_Available(DateTime.Parse(tour_date), nights_stay, campaign_id, tour_location_id)

        For Each gr In dt.AsEnumerable().GroupBy(Function(x) x.Field(Of DateTime)("TOURDATE"))
            st.Add(New WaveDetail With {.TourDate = gr.Key.ToShortDateString(), .Counts = gr.Count()})
        Next
        Return New JavaScriptSerializer().Serialize(st.ToArray())
    End Function

    Protected Sub gvPackages_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvPackages.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim cb = CType(e.Row.Cells(0).FindControl("cbS"), CheckBox)
            Dim dd = CType(e.Row.Cells(3).FindControl("ddQ"), DropDownList)
            Dim mn = CType(e.Row.Cells(1).FindControl("MinRate"), TextBox)
            Dim mx = CType(e.Row.Cells(2).FindControl("MaxRate"), TextBox)
            Dim ex = CType(e.Row.Cells(4).FindControl("ddExtra"), DropDownList)

            cb.Attributes.Add("cbAttr", "cb")

            cb.Text = "&nbsp;" & e.Row.DataItem("Description").ToString().Trim()
            dd.DataSource = Nothing
            dd.DataBind()

            If Convert.ToInt16(e.Row.DataItem("RoomsAvail")) >= 1 Then
                dd.DataSource = Enumerable.Range(1, Convert.ToInt16(e.Row.DataItem("RoomsAvail")))
                dd.DataBind()
            End If

            mn.Text = e.Row.DataItem("MinRate")
            mx.Text = e.Row.DataItem("MaxRate")
            ex.SelectedIndex = 0

            cb.Attributes.Add("PackageID", e.Row.DataItem("PackageID"))
            cb.Attributes.Add("MinRate", mn.Text)
            cb.Attributes.Add("MaxRate", mx.Text)
            dd.Enabled = False

            If Convert.ToBoolean(e.Row.DataItem("TourRequired").ToString()) = True Then
                cb.Attributes.Add("data-campaign-id", 0)
                cb.Attributes.Add("data-tourlocation-id", 0)
                cb.Attributes.Add("tour-required", 1)
                cb.Attributes.Add("rental", 0)
                cb.Attributes.Add("owner-getaway", 0)
                ex.SelectedIndex = 1
            ElseIf e.Row.DataItem("PackageType").ToString() = "Owner Getaway" Then
                cb.Attributes.Add("owner-getaway", 1)
                cb.Attributes.Add("tour-required", 0)
                cb.Attributes.Add("rental", 0)
                ex.SelectedIndex = 2
            ElseIf e.Row.DataItem("PackageType").ToString() = "Rental" Then
                cb.Attributes.Add("rental", 1)
                cb.Attributes.Add("tour-required", 0)
                cb.Attributes.Add("owner-getaway", 0)
                ex.SelectedIndex = 3
                dd.Enabled = True
            End If
        End If
    End Sub

    Protected Sub gridview1_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gridview1.SelectedIndexChanged

        ''Scenario 2    
        Dim new_package_id = gridview1.SelectedDataKey.Value
        wiz.Reservation.CheckInDate = wiz.Search_CheckIn_Date
        wiz.Reservation.CheckOutDate = wiz.Search_CheckOut_date
        wiz.Packages_Price = gridview1.SelectedRow.Cells(10).Text.Replace("$", "")
        wiz.Packages.First().Package = New Wizard_Package With {.PackageID = new_package_id, .GUID = Guid.NewGuid().ToString()}
        wiz.Reservation.TypeID = package_base.Get_Package_Reservation_Type_ID(new_package_id)
        Session("wizData" + wiz.GUID_TIMESTAMP) = New JavaScriptSerializer().Serialize(wiz)
        btNext_Click(btNext, EventArgs.Empty)
    End Sub
    Protected Sub gridview1_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gridview1.RowDataBound
        ''Scenario 2
        If e.Row.RowType = DataControlRowType.DataRow Then

            e.Row.Cells(9).Text = Decimal.Parse(e.Row.Cells(9).Text).ToString("c")
            e.Row.Cells(9).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(2).Visible = False
            e.Row.Cells(3).Visible = False
        ElseIf e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(2).Visible = False
            e.Row.Cells(3).Visible = False
            e.Row.Cells(1).Text = "Package ID".ToUpper()
            e.Row.Cells(4).Text = "Type".ToUpper()
            e.Row.Cells(5).Text = "Package".ToUpper()
            e.Row.Cells(6).Text = "Bedrooms".ToUpper()
            e.Row.Cells(7).Text = "Unit Type".ToUpper()
            e.Row.Cells(8).Text = "Reservation's Type".ToUpper()
            e.Row.Cells(9).Text = "Invoice Amount".ToUpper()
        End If
    End Sub
#End Region

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
    Protected Sub gridview_hotel_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gridview_hotel.SelectedIndexChanged
        wiz.Packages(0).Package = New Wizard_Package With {.PackageID = gridview_hotel.SelectedDataKey.Value, .GUID = Guid.NewGuid.ToString()}
        wiz.Packages_Price = gridview_hotel.SelectedRow.Cells(4).Text.Replace("$", "")
        accom_name = gridview_hotel.SelectedRow.Cells(3).Text
        wiz.Reservation.CheckInDate = wiz.Search_CheckIn_Date
        wiz.Reservation.CheckOutDate = wiz.Search_CheckOut_date
        wiz.Accom_Name = accom_name
        Session("wizData" + wiz.GUID_TIMESTAMP) = New JavaScriptSerializer().Serialize(wiz)
        btNext_Click(btNext, EventArgs.Empty)
    End Sub
    Protected Sub gridview_hotel_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gridview_hotel.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(4).Text = Decimal.Parse(e.Row.Cells(4).Text).ToString("c")
            e.Row.Cells(4).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(5).Visible = False
            e.Row.Cells(6).Visible = False
        ElseIf e.Row.RowType = DataControlRowType.Header Then
            e.Row.Cells(1).Text = "Package ID".ToUpper()
            e.Row.Cells(2).Text = "Package Description".ToUpper()
            e.Row.Cells(3).Text = "Hotel Accommodation".ToUpper()
            e.Row.Cells(4).Text = "ON-HOA".ToUpper()
            e.Row.Cells(5).Visible = False
            e.Row.Cells(6).Visible = False
        End If
    End Sub
End Class
Class Book_Tour_Or_Tradeshow_Package

    Dim package_tour_package As New TourPackage_Package
    Dim package_tradeshow As New Tradeshow_Package
    Dim invoice_tour_package As New TourPackage_Invoice
    Dim invoice_tradeshow As New Tradeshow_Invoice
    Private package_base As New Base_Package
    Private package_x As New Package_Xtra
    Private cns As String = Resources.Resource.cns

    Public Sub Refresh(gv As GridView, campaign_id As Int32, tour_location_id As Int32, checkin_date As DateTime, nights_stay As Int32, package_id As Int32)

        Dim dt1 = New DataTable(), dt2 = New DataTable(), dt3 = New DataTable()

        Try
            package_x.Get_Tours_Available(checkin_date, nights_stay, campaign_id, tour_location_id)

        Catch ex As ArgumentException
            Throw ex
        Catch ex As Exception_Tour_Waves_Not_Available
            Throw ex
        Catch ex As Exception
            Throw ex
        End Try

        Using cn = New SqlConnection(cns)
            Using ad = New SqlDataAdapter()

                Dim bed_room = package_base.Get_Package_Bedroom(package_id)
                If bed_room.Length > 0 Then

                    Dim sql = String.Format("select p.PackageID, p.UnitTypeID, pr.TypeID ,(select ComboItem from t_ComboItems where ComboItemID = p.TypeID) [Type], Description [Package Description], Bedrooms, (select ComboItem from t_ComboItems where ComboItemID = p.UnitTypeID) [Unit Type], (select ComboItem from t_ComboItems where ComboItemID = pr.TypeID) [Reservation's Type], 0 [Invoice], 0 [ON-HOA] " _
                       & "from t_Vendor2Package v2p inner join t_Package p on v2p.PackageID = p.PackageID " _
                       & "inner join t_PackageReservation pr on pr.PackageID = p.PackageID " _
                       & "where v2p.VendorID={0} " _
                       & "and pr.PromoNights={1} " _
                       & "and LEFT(p.Bedrooms, 1)={2} ",
                       package_base.Get_Package_VendorID(package_id),
                       package_base.Get_Promo_Nights(package_id),
                       bed_room.Substring(0, 1))

                    Try
                        ad.SelectCommand = New SqlCommand(sql, cn)
                        cn.Open()
                        ad.Fill(dt2)
                        dt3 = dt2.Clone()

                        For Each dr As DataRow In dt2.Rows

                            bed_room = dr.Item("Bedrooms").ToString()
                            Dim unit_type_id As Int32 = dr.Item("UnitTypeID").ToString()
                            Dim reservation_type_id As Int32 = dr.Item("TypeID").ToString()
                            Dim up_dwn = String.Empty
                            Dim row_package_id = dr.Item("PackageID").ToString()

                            If bed_room.IndexOf("-") > 1 Then
                                If bed_room.IndexOf("DWN") > 2 Then
                                    up_dwn = "DWN"
                                Else
                                    up_dwn = "UP"
                                End If
                            End If

                            bed_room = bed_room.Substring(0, 1)

                            Try

                                Dim l = package_x.Get_Rooms_Avalailable(bed_room, unit_type_id, reservation_type_id, checkin_date, nights_stay, up_dwn)
                                Dim package_type = package_base.Get_Package_Type(row_package_id)

                                If package_type = "Tradeshow" Then

                                    invoice_tradeshow = package_tradeshow.Calculate_OnHOA(nights_stay, checkin_date.ToShortDateString(), row_package_id)
                                    dr("ON-HOA") = invoice_tradeshow.Additional_Amount + invoice_tradeshow.Base_Amount

                                ElseIf package_type = "Tour Package" Then

                                    invoice_tour_package = package_tour_package.Calculate_OnHOA(nights_stay, checkin_date.ToShortDateString(), row_package_id)
                                    dr("ON-HOA") = invoice_tour_package.Additional_Amount + invoice_tour_package.Base_Amount
                                End If

                                If l.Count > 0 Then
                                    dt3.ImportRow(dr)
                                End If
                            Catch ex As Exception_Allocation_Room_Search_Not_Exist
                            End Try
                        Next

                        If dt3.Rows.Count = 0 Then
                            Throw New Exception_Allocation_Room_Search_Not_Exist(checkin_date, nights_stay)
                        Else
                            gv.DataKeyNames = New String() {"PACKAGEID"}
                            gv.DataSource = dt3
                            gv.DataBind()
                        End If
                    Catch ex As ArgumentException
                        Throw ex
                    Catch ex As Exception
                        Throw ex
                    Finally
                        cn.Close()
                    End Try
                End If
            End Using
        End Using
    End Sub
End Class
Class RebookResortReservation

    Dim package_tour_package As New TourPackage_Package
    Dim package_tradeshow As New Tradeshow_Package
    Dim package_tour_promotion As New TourPromotion_Package
    Dim package_rental As New Rental_Package
    Dim package_owner_getaway As New OwnerGetaway_Package

    Dim invoice_tour_package As New TourPackage_Invoice
    Dim invoice_tradeshow As New Tradeshow_Invoice
    Dim invoice_tour_promotion As New TourPromotion_Invoice
    Dim invoice_rental As New Rental_Invoice
    Dim invoice_owner_getaway As New OwnerGetaway_Invoice

    Private package_base As New Base_Package
    Private package_x As New Package_Xtra

    Private cns As String = Resources.Resource.cns

    Public Function Refresh(gv As GridView, tour_id As Int32, campaign_id As Int32, tour_location_id As Int32, checkin_date As DateTime, nights_stay As Int32, package_id As Int32, packages As List(Of Wizard_Package), Optional host_name As String = "") As String
        Dim sb = New StringBuilder()
        Dim vendor_id = package_base.Get_Package_VendorID(package_id)
        Dim promo_nights = package_base.Get_Promo_Nights(package_id)
        Dim bed_room = String.Empty, sq = String.Empty
        Dim room_unit_type_id As Integer = 0, reservation_type_id As Integer = 0, data_any = False, loop_counter = 0

        Dim getSQL As Func(Of Int32, Int32, Boolean, DataTable) =
            Function(param_package_id As Int32, param_vendor_id As Int32, has_tour As Boolean)
                param_vendor_id = package_base.Get_Package_VendorID(param_package_id)

                If has_tour Then
                    sq = String.Format("Select p.PackageID, p.Package, p.Description [Package Description], p.Bedrooms, pr.PackageReservationID, pr.TypeID  [Reservation Type ID],(select Comboitem from t_ComboItems where ComboItemID = p.TypeID) [Package Reservation Type], " _
                            & "p.UnitTypeID,(select Comboitem from t_ComboItems where ComboItemID = p.UnitTypeID) [Package Unit Type],vp.VendorID," _
                            & "(select Comboitem from t_ComboItems where ComboItemID = pr.TypeID) [Reservation Type], " _
                            & "(select Vendor from t_Vendor where VendorID = vp.VendorID) [Vendor],pr.PromoNights, pr.AllowExtraNight, pt.CampaignID, " _
                            & "(select Name from t_Campaign c where c.CampaignID = pt.CampaignID) [Package Tour Campaign],pt.TourLocationID," _
                            & "(select ci.ComboItem from t_ComboItems ci where ci.ComboItemID = pt.TourLocationID) [Package Tour Location], " _
                            & "0 [Invoice] " _
                            & "from t_Package p inner join t_PackageReservation pr on p.PackageID = pr.PackageID " _
                            & "inner join t_Vendor2Package vp on vp.PackageID = p.PackageID " _
                            & "inner join t_PackageTour pt on pt.PackageReservationID = pr.PackageReservationID " _
                            & "inner join (Select p1.PackageID, p1.Bedrooms, pr1.PromoNights, pv.VendorID " _
                            & "from t_Package p1 inner join t_PackageReservation pr1 on p1.PackageID = pr1.PackageID " _
                            & "inner join t_Vendor2Package pv on p1.PackageID = pv.PackageID where p1.PackageID = {0}) x " _
                            & "on pr.PromoNights = x.PromoNights and vp.VendorID = x.VendorID and Left(p.Bedrooms, 1) = Left(x.Bedrooms, 1) " _
                            & "and CONVERT(datetime, CONVERT(varchar(10), p.EndDate, 101)) >= CONVERT(datetime, CONVERT(varchar(10), GETDATE(), 101)) and vp.VendorID = {1}", param_package_id, param_vendor_id)
                Else    ''neither tradeshow nor tour package

                    If host_name = "KCPCRMS" Then

                        sq = String.Format("Select p.PackageID, p.Package, p.Description [Package Description], p.Bedrooms, pr.PackageReservationID, pr.TypeID [Reservation Type ID],(select Comboitem from t_ComboItems where ComboItemID = p.TypeID) [Package Reservation Type], " _
                                & "p.UnitTypeID,(select Comboitem from t_ComboItems where ComboItemID = p.UnitTypeID) [Package Unit Type],vp.VendorID," _
                                & "(select Comboitem from t_ComboItems where ComboItemID = pr.TypeID) [Reservation Type], " _
                                & "(select Vendor from t_Vendor where VendorID = vp.VendorID) [Vendor], pr.PromoNights, pr.AllowExtraNight, " _
                                & "0 [Invoice] " _
                                & "from t_Package p inner join t_PackageReservation pr on p.PackageID = pr.PackageID " _
                                & "inner join t_Vendor2Package vp on vp.PackageID = p.PackageID " _
                                & "inner join (Select p1.PackageID, p1.Bedrooms, pr1.PromoNights, p1.TypeID " _
                                & "from t_Package p1 inner join t_PackageReservation pr1 on p1.PackageID = pr1.PackageID where p1.PackageID = {0}) x " _
                                & "on pr.PromoNights = x.PromoNights and Left(p.Bedrooms, 1) = Left(x.Bedrooms, 1) and x.TypeID = p.TypeID " _
                                & "Where vp.VendorID = {1} " _
                                & "and CONVERT(datetime, CONVERT(varchar(10), p.EndDate, 101)) >= CONVERT(datetime, CONVERT(varchar(10), GETDATE(), 101))", param_package_id, param_vendor_id)

                    ElseIf host_name = "CRMSNET" Or String.IsNullOrEmpty(host_name) Then

                        If package_base.Get_Package_Type(param_package_id) = "Owner Getaway" Then

                            sq = String.Format("Select p.PackageID, p.Package, p.Description [Package Description], p.Bedrooms, pr.PackageReservationID, pr.TypeID [Reservation Type ID],(select Comboitem from t_ComboItems where ComboItemID = p.TypeID) [Package Reservation Type], " _
                               & "p.UnitTypeID,(select Comboitem from t_ComboItems where ComboItemID = p.UnitTypeID) [Package Unit Type],vp.VendorID," _
                               & "(select Comboitem from t_ComboItems where ComboItemID = pr.TypeID) [Reservation Type], " _
                               & "(select Vendor from t_Vendor where VendorID = vp.VendorID) [Vendor], pr.PromoNights, pr.AllowExtraNight, " _
                               & "0 [Invoice] " _
                               & "from t_Package p inner join t_PackageReservation pr on p.PackageID = pr.PackageID " _
                               & "inner join t_Vendor2Package vp on vp.PackageID = p.PackageID " _
                               & "inner join (Select p1.PackageID, p1.Bedrooms, pr1.PromoNights, p1.TypeID from t_Package p1 " _
                               & "inner join t_PackageReservation pr1 on p1.PackageID = pr1.PackageID where p1.PackageID = {0}) x " _
                               & "on Left(p.Bedrooms, 1) = Left(x.Bedrooms, 1) and x.TypeID = p.TypeID " _
                               & "and CONVERT(datetime, CONVERT(varchar(10), p.EndDate, 101)) >= CONVERT(datetime, CONVERT(varchar(10), GETDATE(), 101)) and vp.VendorID = {1} where pr.PromoNights <= {2}", param_package_id, param_vendor_id, nights_stay + 1)
                        Else

                            sq = String.Format("Select p.PackageID, p.Package, p.Description [Package Description], p.Bedrooms, pr.PackageReservationID, pr.TypeID [Reservation Type ID],(select Comboitem from t_ComboItems where ComboItemID = p.TypeID) [Package Reservation Type], " _
                               & "p.UnitTypeID,(select Comboitem from t_ComboItems where ComboItemID = p.UnitTypeID) [Package Unit Type], VendorID = 0," _
                               & "(select Comboitem from t_ComboItems where ComboItemID = pr.TypeID) [Reservation Type], " _
                               & "(select Vendor from t_Vendor where VendorID = 0) [Vendor], pr.PromoNights, pr.AllowExtraNight, " _
                               & "0 [Invoice], 0 [TourLocationID], 0 [CampaignID] " _
                               & "from t_Package p inner join t_PackageReservation pr on p.PackageID = pr.PackageID " _
                               & "inner join (Select p1.PackageID, p1.Bedrooms, pr1.PromoNights, p1.TypeID from t_Package p1 " _
                               & "inner join t_PackageReservation pr1 on p1.PackageID = pr1.PackageID where p1.PackageID = {0}) x " _
                               & "on pr.PromoNights = x.PromoNights and Left(p.Bedrooms, 1) = Left(x.Bedrooms, 1) and x.TypeID = p.TypeID " _
                               & "and CONVERT(datetime, CONVERT(varchar(10), p.EndDate, 101)) >= CONVERT(datetime, CONVERT(varchar(10), GETDATE(), 101)) ", param_package_id, param_vendor_id)
                        End If
                    End If
                End If

                Dim dt = New DataTable()
                Using cn = New SqlConnection(cns)
                    Using cm = New SqlCommand(sq, cn)
                        Try
                            cn.Open()
                            dt.Load(cm.ExecuteReader())
                        Catch ex As Exception
                            cn.Close()
                            Throw ex
                        Finally
                            cn.Close()
                        End Try
                    End Using
                End Using
                Return dt
            End Function

        Dim currentVendorID = 74
        If Not HttpContext.Current.Session("ActiveVendorID") Is Nothing Then
            currentVendorID = CType(HttpContext.Current.Session("ActiveVendorID"), Int32)
        End If


        For Each p In packages

            Dim has_tour = IIf(package_base.Get_Package_Type(p.PackageID) = "Tour Package" _
                           Or package_base.Get_Package_Type(p.PackageID) = "Tour Promotion" _
                           Or package_base.Get_Package_Type(p.PackageID) = "Tradeshow", True, False)

            Dim tbl = getSQL(p.PackageID, currentVendorID, has_tour)
            loop_counter += 1

            sb.AppendLine("<table class=table table-striped><thead>")
            sb.AppendLine("<tr><th></th><th>PACKAGE ID</th><th>DESCRITPION</th><th>BED ROOM</th><th>UNIT TYPE</th><th>PACKAGE TYPE</th><th>RESERVATION TYPE</th><th>MIN RATE</th><th>MAX RATE</th></tr>")
            sb.AppendLine("</thead>")
            sb.AppendLine("<tbody>")
            sb.AppendFormat("<tr><td colspan=8><strong style='color:darkblue;'>{0}/{1} -  {2} ({3})</strong></td></tr>", loop_counter, packages.Count(), package_base.Get_Package_Description(p.PackageID).ToUpper(), package_base.Get_Package_Type(p.PackageID).ToUpper())

            For Each dr As DataRow In tbl.Rows
                bed_room = dr("Bedrooms").ToString()
                package_id = dr("PackageID").ToString()
                tour_location_id = dr("TourLocationID").ToString
                campaign_id = dr("CampaignID").ToString()
                reservation_type_id = dr("Reservation Type ID").ToString()
                room_unit_type_id = dr("UnitTypeID").ToString()
                Dim up_dwn As String = String.Empty
                Dim package_end_date = package_base.Get_Package_EndDate(package_id)

                If package_end_date.HasValue Then

                    If package_end_date.Value >= Convert.ToDateTime(checkin_date.AddDays(nights_stay)) Then

                        If bed_room.IndexOf("-") > 1 Then
                            If bed_room.IndexOf("DWN") > 2 Then
                                up_dwn = "DWN"
                            Else
                                up_dwn = "UP"
                            End If
                        End If

                        bed_room = bed_room.Substring(0, 1)

                        Try
                            If has_tour Then
                                package_x.Get_Tours_Available(checkin_date, nights_stay, campaign_id, tour_location_id)
                            End If

                            Dim ra = package_x.Get_Rooms_Avalailable(bed_room.Substring(0, 1), room_unit_type_id, reservation_type_id, checkin_date, nights_stay, up_dwn)

                            Dim rate_min = 0D
                            Dim rate_max = 0D

                            If dr("Package Reservation Type").ToString() = "Tradeshow" Then

                                invoice_tradeshow = package_tradeshow.Calculate_OnHOA(nights_stay, checkin_date.ToShortDateString(), package_id)
                                rate_min += invoice_tradeshow.Additional_Amount + invoice_tradeshow.Base_Amount
                                rate_max += invoice_tradeshow.Additional_Amount + invoice_tradeshow.Base_Amount

                            ElseIf dr("Package Reservation Type").ToString() = "Tour Package" Then

                                invoice_tour_package = package_tour_package.Calculate_OnHOA(nights_stay, checkin_date.ToShortDateString(), package_id)
                                rate_min += invoice_tour_package.Additional_Amount + invoice_tour_package.Base_Amount
                                rate_max += invoice_tour_package.Additional_Amount + invoice_tour_package.Base_Amount

                            ElseIf dr("Package Reservation Type").ToString() = "Tour Promotion" Then

                                invoice_tour_promotion = package_tour_promotion.Calculate_OnHOA(nights_stay, checkin_date.ToShortDateString(), package_id)
                                rate_min += invoice_tour_promotion.Base_Amount + invoice_tour_promotion.Additional_Amount
                                rate_max += invoice_tour_promotion.Base_Amount + invoice_tour_promotion.Additional_Amount

                            ElseIf dr("Package Reservation Type").ToString() = "Owner Getaway" Then

                                invoice_owner_getaway = package_owner_getaway.Calculate_OnHOA(nights_stay, checkin_date.ToShortDateString(), package_id)
                                rate_min += invoice_owner_getaway.Base_Amount + invoice_owner_getaway.Additional_Amount
                                rate_max += invoice_owner_getaway.Base_Amount + invoice_owner_getaway.Additional_Amount
                            End If

                            If dr("Package Reservation Type").ToString() = "Rental" Then

                                invoice_rental = package_rental.Calculate_OnHOA(nights_stay, checkin_date.ToShortDateString(), package_id)

                                rate_min += invoice_rental.Min_Amount
                                rate_max += invoice_rental.Base_Amount

                            End If

                            sb.AppendLine("<tr>")
                            sb.AppendFormat("<td><input type=radio name={0} parent-package-id={1} child-package-id={2} parent-guid={3} child-guid={4} price-min={5} price-max={6} class=exchange /></td>",
                                    p.GUID, p.PackageID, package_id, p.GUID, Guid.NewGuid.ToString(), rate_min.ToString(), rate_max.ToString())
                            sb.AppendFormat("<td>{0}</td>", package_id)
                            sb.AppendFormat("<td>{0}</td>", dr("Package Description").ToString())
                            sb.AppendFormat("<td>{0}</td>", dr("Bedrooms").ToString())
                            sb.AppendFormat("<td>{0}</td>", dr("Package Unit Type").ToString())
                            sb.AppendFormat("<td>{0}</td>", dr("Package Reservation Type").ToString())
                            sb.AppendFormat("<td>{0}</td>", dr("Reservation Type").ToString())
                            sb.AppendFormat("<td class=text-right>{0}</td>", rate_min.ToString("n2"))
                            sb.AppendFormat("<td class=text-right>{0}</td>", rate_max.ToString("n2"))
                            sb.AppendLine("</tr>")

                            data_any = True

                        Catch ex As Exception_Tour_Waves_Not_Available
                            'ignore this error

                        Catch ex As Exception_Allocation_Room_Search_Not_Exist
                            'ignore this type of error here

                        Catch ex As Exception
                            Throw ex
                        End Try
                    End If
                End If
            Next
            sb.AppendLine("</thead></table>")
        Next
        If data_any = False Then
            sb.Clear()
        End If
        Return sb.ToString()
    End Function
End Class
Class Book_Hotel_Reservation
    Dim package_tour_package As New TourPackage_Package
    Dim invoice_tour_package As New TourPackage_Invoice
    Private package_base As New Base_Package
    Private package_x As New Package_Xtra

    Private cns As String = Resources.Resource.cns

    Public Sub Refresh(gv As GridView, campaign_id As Int32, tour_location_id As Int32, checkin_date As DateTime, nights_stay As Int32, package_id As Int32)

        Dim dt1 = New DataTable(), dt2 = New DataTable(), dt3 = New DataTable()

        Using cn = New SqlConnection(cns)
            Using ad = New SqlDataAdapter()
                Dim sql = String.Format("select p.PackageID, p.Description [Package Description], acc.AccomName, 0 [Invoice], pt.TourLocationID, pt.CampaignID " _
                            & "from t_Vendor2Package v2p inner join t_Package p on v2p.PackageID = p.PackageID " _
                            & "inner join t_PackageReservation pr on pr.PackageID = p.PackageID " _
                            & "inner join t_Accom acc on acc.AccomID = p.AccomID inner join t_PackageTour pt on pt.PackageReservationID = pr.PackageReservationID " _
                            & "where v2p.VendorID={0} and pr.PromoNights={1} and acc.AccomName in ('Best Western Historic', 'Wyndham Garden')",
                            package_base.Get_Package_VendorID(package_id),
                            package_base.Get_Promo_Nights(package_id))

                Try

                    ad.SelectCommand = New SqlCommand(sql, cn)
                    cn.Open()
                    ad.Fill(dt2)
                    dt3 = dt2.Clone()
                    For Each dr As DataRow In dt2.Rows

                        Dim p_id = Int32.Parse(dr("PackageID").ToString())
                        campaign_id = Int32.Parse(dr("CampaignID").ToString())
                        tour_location_id = Int32.Parse(dr("TourLocationID").ToString())

                        Try
                            package_x.Get_Tours_Available(checkin_date, nights_stay, campaign_id, tour_location_id)
                            invoice_tour_package = package_tour_package.Calculate_OnHOA_Hotel(nights_stay, checkin_date, p_id)
                            package_tour_package.Calculate_Chargeback_Hotel(p_id, invoice_tour_package)
                            dr("Invoice") = invoice_tour_package.Base_Amount

                            dt3.ImportRow(dr)
                        Catch ex As Exception_Tour_Waves_Not_Available

                        End Try
                    Next
                Catch ex As Exception
                Finally
                    cn.Close()
                End Try
            End Using
        End Using

        If dt3.Rows.Count = 0 Then
            Throw New Exception_Packages_Available_None(checkin_date, nights_stay)
        Else
            gv.DataKeyNames = New String() {"PACKAGEID"}
            gv.DataSource = dt3
            gv.DataBind()
        End If
    End Sub
End Class
Interface IFilter
    Function Filter(w As Wizard, checkinDate As Nullable(Of DateTime), nights As Short, byPackageType As String(), byUnitType As String()) As DataTable
End Interface
Interface IFind
    Function Search(checkin_date As DateTime, nights_stay As Int32) As DataTable
End Interface
Class VrcPackagesAvailable
    Implements IFind

    Private byPackageTypes As String()
    Private byUnitTypes As String()
    Private package_base As New Base_Package

    Public Sub New(packageTypes As String(), unitTypes As String())
        Me.byPackageTypes = packageTypes
        Me.byUnitTypes = unitTypes
    End Sub
    Public Function Search(checkin_date As Date, nights_stay As Int32) As DataTable Implements IFind.Search
        Dim ds = New clsReservationWizard().Available_Packages(checkin_date.ToShortDateString(), nights_stay, 1, "crms.kingscreekplantation.com")
        Try
            Dim dt = ds.Tables(1).Rows.OfType(Of DataRow).Where(Function(x) x("PackageType").ToString() <> "Tradeshow" And x("PackageType").ToString() <> "Tour Package").CopyToDataTable()
            dt.Columns.Add("UnitType")
            For Each dr As DataRow In dt.Rows
                dr("UnitType") = New clsComboItems().Lookup_ComboItem(package_base.Get_Package_Unit_Type_ID(dr("PackageID").ToString()))
            Next

            Return dt.Rows.OfType(Of DataRow).Where(Function(x) _
                                            Array.IndexOf(byPackageTypes, x.Item("PackageType").ToString()) >= 0 _
                                            And Array.IndexOf(byUnitTypes, x.Item("UnitType").ToString()) >= 0).OrderBy(Function(x) x.Item("PackageType")).ThenBy(Function(x) x.Item("Description")).CopyToDataTable()
        Catch ex As Exception
            If ex.Message = "The source contains no DataRows." Then
                Throw New Exception_Packages_Available_None(checkin_date, nights_stay)
            Else
                Return New DataTable
            End If
        End Try
    End Function
End Class
Class GrandIncentivesPackagesAvailable
    Implements IFind

    Private byPackageTypes As String()
    Private byUnitTypes As String()
    Private package_base As New Base_Package

    Public Sub New(packageTypes As String(), unitTypes As String())
        Me.byPackageTypes = packageTypes
        Me.byUnitTypes = unitTypes
    End Sub
    Public Function Search(checkin_date As Date, nights_stay As Int32) As DataTable Implements IFind.Search
        Dim ds = New clsReservationWizard().Available_Packages(checkin_date.ToShortDateString(), nights_stay, 1, "crms.kingscreekplantation.com")
        Try
            Dim dt = ds.Tables(0).Rows.OfType(Of DataRow).Where(Function(x) x("PackageType").ToString() <> "Tradeshow" And x("PackageType").ToString() <> "Tour Package").CopyToDataTable()
            dt.Columns.Add("UnitType")
            For Each dr As DataRow In dt.Rows
                dr("UnitType") = New clsComboItems().Lookup_ComboItem(package_base.Get_Package_Unit_Type_ID(dr("PackageID").ToString()))
            Next

            Return dt.Rows.OfType(Of DataRow).Where(Function(x) (x.Item("Description").ToString().ToLower().IndexOf("ORBlast".ToLower()) > 0 _
                                            Or x.Item("PackageType").ToString().ToLower().IndexOf("Tour Promotion".ToLower()) >= 0) And
                                            Array.IndexOf(byPackageTypes, x.Item("PackageType").ToString()) >= 0 _
                                            And Array.IndexOf(byUnitTypes, x.Item("UnitType").ToString()) >= 0).OrderBy(Function(x) x.Item("PackageType")).ThenBy(Function(x) x.Item("Description")).CopyToDataTable()
        Catch ex As Exception
            If ex.Message = "The source contains no DataRows." Then
                Throw New Exception_Packages_Available_None(checkin_date, nights_stay)
            Else
                Return New DataTable
            End If
        End Try
    End Function
End Class
Class CompleteCallSolutions
    Implements IFind

    Private byPackageTypes As String()
    Private byUnitTypes As String()
    Private package_base As New Base_Package

    Public Sub New(packageTypes As String(), unitTypes As String())
        Me.byPackageTypes = packageTypes
        Me.byUnitTypes = unitTypes
    End Sub
    Public Function Search(checkin_date As Date, nights_stay As Int32) As DataTable Implements IFind.Search
        Dim ds = New clsReservationWizard().Available_Packages(checkin_date.ToShortDateString(), nights_stay, 1, "crms.kingscreekplantation.com")
        Try
            Dim dt = ds.Tables(0).Rows.OfType(Of DataRow).Where(Function(x) x("PackageType").ToString() <> "Tradeshow" And x("PackageType").ToString() <> "Tour Package").CopyToDataTable()
            dt.Columns.Add("UnitType")
            For Each dr As DataRow In dt.Rows
                dr("UnitType") = New clsComboItems().Lookup_ComboItem(package_base.Get_Package_Unit_Type_ID(dr("PackageID").ToString()))
            Next            

            Return dt.Rows.OfType(Of DataRow).Where(Function(x) _
                                           Array.IndexOf(byPackageTypes, x.Item("PackageType").ToString()) >= 0 _
                                           And Array.IndexOf(byUnitTypes, x.Item("UnitType").ToString()) >= 0).OrderBy(Function(x) x.Item("PackageType")).ThenBy(Function(x) x.Item("Description")).CopyToDataTable()

        Catch ex As Exception
            If ex.Message = "The source contains no DataRows." Then
                Throw New Exception_Packages_Available_None(checkin_date, nights_stay)
            Else
                Return New DataTable
            End If
        End Try
    End Function
End Class
Public Structure WaveDetail
    Public TourDate As String
    Public Counts As Int16
End Structure
