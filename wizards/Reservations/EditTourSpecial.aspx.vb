Imports System.Data
Imports System.Reflection
Imports System.Web.Script.Serialization
Imports clsReservationWizard

Partial Class wizard_Reservations_TourSpecial
    Inherits System.Web.UI.Page

    Private package_base As New Base_Package
    Private package_x As New Package_Xtra
    Private wiz As New Wizard

#Region "Event Handlers"

    Protected Sub btPrevious_Click(sender As Object, e As System.EventArgs) Handles btPrevious.Click
        Dim bt = CType(sender, Button)
        bt.Attributes.Add("nav", 0)
        Navigate(CType(sender, Button))
    End Sub
    Protected Sub btCancel_Click(sender As Object, e As EventArgs) Handles btCancel.Click
        multiview1.SetActiveView(view1)
    End Sub
    Protected Sub btSubmit_Click(sender As Object, e As EventArgs) Handles btSubmit.Click
        Form_Save()
        multiview1.SetActiveView(view1)
        Form_Load()
    End Sub
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        Dim ph = CType(Me.Master.Master.FindControl("ContentPlaceHolder1"), ContentPlaceHolder)
        Dim wiz_data = CType(ph.FindControl("LB_WIZ_DATA"), Label)

        If Session("wizData" + Session("wizGuid")) <> Nothing Then
            wiz = New JavaScriptSerializer().Deserialize(Of Wizard)(Session("wizData" + Session("wizGuid")))
        Else
            wiz = New JavaScriptSerializer().Deserialize(Of Wizard)(wiz_data.Text)
        End If
        Session("wizData" + wiz.GUID_TIMESTAMP) = New JavaScriptSerializer().Serialize(wiz)
        Session("wizGuid") = wiz.GUID_TIMESTAMP


        lbPriceOverLimit.Text = ""
        lbErr.Text = ""

        If IsPostBack = False Then
            Form_Load()
            Session("wizData" + wiz.GUID_TIMESTAMP) = New JavaScriptSerializer().Serialize(wiz)
        End If

        If Not Session("LB_ERR_MASTERPAGE") Is Nothing Then
            Dim lb_err_mp = CType(Me.Master.Master.FindControl("ContentPlaceHolder1").FindControl("LB_ERR_MASTERPAGE"), Label)
            lbErr.Text = CType(Session("LB_ERR_MASTERPAGE"), String)
            Session("LB_ERR_MASTERPAGE") = Nothing
        End If

    End Sub


    Protected Sub gridview1_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gridview1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dataItem = CType(e.Row.DataItem, DataRowView)

            Dim td = Convert.ToDateTime(dataItem("TourDate").ToString())
            Dim dow = String.Empty
            If td.DayOfWeek = DayOfWeek.Sunday Then
                dow = "Sun"
            ElseIf td.DayOfWeek = DayOfWeek.Monday Then
                dow = "Mon"
            ElseIf td.DayOfWeek = DayOfWeek.Tuesday Then
                dow = "Tue"
            ElseIf td.DayOfWeek = DayOfWeek.Wednesday Then
                dow = "Wed"
            ElseIf td.DayOfWeek = DayOfWeek.Thursday Then
                dow = "Thu"
            ElseIf td.DayOfWeek = DayOfWeek.Friday Then
                dow = "Fri"
            ElseIf td.DayOfWeek = DayOfWeek.Saturday Then
                dow = "Sat"
            End If
            e.Row.Cells(1).Text = String.Format("{0:MM/dd/yy} ({1})", td, dow)

            e.Row.Attributes.Add("data-tour-date", td.ToShortDateString())
            e.Row.Attributes.Add("onClick", "passTourDate(this);return false;")

            Dim tt = dataItem("TOURTIME").ToString()
            If tt.IndexOf("PM") > 0 Then
                tt = tt.Substring(0, tt.IndexOf(" "))
                tt = 12 + Integer.Parse(Left(tt.Replace(":", ""), 1)) & Right(tt.Replace(":", ""), 2)
            Else
                tt = tt.Substring(0, tt.IndexOf(" "))
                tt = tt.Replace(":", "")
            End If

            e.Row.Attributes.Add("data-tour-time", New clsComboItems().Lookup_ID("TourTime", tt))
        End If
    End Sub
    Protected Sub ddlQty_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim gvr = CType(CType(sender, DropDownList).NamingContainer, GridViewRow)
        Dim dd = CType(sender, DropDownList)

        Dim p = wiz.Tour.Premiums.Where(Function(x) x.PremiumID = dd.Attributes("PREMIUMID")).First()
        p.QtyAssigned = dd.SelectedItem.Text

        Session("wizData" + wiz.GUID_TIMESTAMP) = New JavaScriptSerializer().Serialize(wiz)
    End Sub
    Protected Sub gridview2_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gridview2.RowDataBound
        Dim drView = CType(e.Row.DataItem, DataRowView)

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim rowView As DataRowView = CType(e.Row.DataItem, DataRowView)

            Dim ddl = CType(e.Row.FindControl("ddlQty"), DropDownList)
            Dim txt = CType(e.Row.FindControl("txCostEA"), TextBox)
            Dim ID = CType(e.Row.FindControl("PremiumIssuedID"), Label)
            Dim lb = CType(e.Row.FindControl("lbPremiumName"), Label)

            ID.Text = rowView("PremiumID").ToString()

            With New clsPremium
                .PremiumID = ID.Text
                .Load()
                lb.Text = .PremiumName.ToUpper()
            End With

            ddl.SelectedValue = rowView("QtyAssigned").ToString()
            txt.Text = String.Format("{0:C}", Convert.ToDecimal(rowView("CostEA").ToString()))

            ddl.Attributes("PREMIUMID") = rowView("PremiumID").ToString()

            If wiz.Scenario = EnumScenario.Two Or wiz.Scenario = EnumScenario.Three Then
                Dim isReq = Convert.ToBoolean(e.Row.DataItem("IsRequired").ToString())
                e.Row.Enabled = IIf(isReq, False, True)
            End If
        End If
    End Sub
    Protected Sub gridview2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gridview2.SelectedIndexChanged

        Dim save_throws_exception = False

        Try
            Form_Save()
        Catch ex As Exception

            lbErr.Text = ex.Message
            save_throws_exception = True
        End Try

        If save_throws_exception = False Then

            multiview1.SetActiveView(view2)
            gridview2.Attributes("PremiumIssuedID") = gridview2.DataKeys(gridview2.SelectedRow.RowIndex).Value
            Form_Load()
        End If
    End Sub
    Protected Sub ddPremiums_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddPremiums.SelectedIndexChanged
        Dim pID = ddPremiums.SelectedItem.Value
        Dim p = wiz.Tour.Premiums.Where(Function(x) x.PremiumIssuedID = gridview2.Attributes("PremiumIssuedID")).Single()

        With New clsPremium
            .PremiumID = pID
            .Load()
            txAmount.Text = .Cost
        End With
    End Sub
#End Region

#Region "Subs/Functions"

    Private Sub Form_Load()

        If multiview1.GetActiveView().Equals(view1) Then
            If IsPostBack = False Then

                With ddTourTime
                    .DataSource = Listings.TourTime()
                    .DataTextField = "ComboItem"
                    .DataValueField = "ComboItemID"
                    .DataBind()
                End With
                With ddStatus
                    .DataSource = Listings.TourStatus()
                    .DataTextField = "ComboItem"
                    .DataValueField = "ComboItemID"
                    .DataBind()
                End With
                With ddLocation
                    .DataSource = Listings.TourLocation()
                    .DataTextField = "ComboItem"
                    .DataValueField = "ComboItemID"
                    .DataBind()
                End With
                With ddType
                    .DataSource = Listings.TourType()
                    .DataTextField = "ComboItem"
                    .DataValueField = "ComboItemID"
                    .DataBind()
                End With
                With ddSubType
                    .DataSource = Listings.TourSubType()
                    .DataTextField = "ComboItem"
                    .DataValueField = "ComboItemID"
                    .DataBind()
                End With
                With ddSource
                    .DataSource = Listings.TourSource()
                    .DataTextField = "ComboItem"
                    .DataValueField = "ComboItemID"
                    .DataBind()
                End With
                With ddCampaign
                    .DataSource = Listings.ActiveCampaigns()
                    .DataTextField = "Name"
                    .DataValueField = "CampaignID"
                    .DataBind()
                End With
            End If

            ddCampaign.ClearSelection()
            ddSource.ClearSelection()
            ddSubType.ClearSelection()
            ddType.ClearSelection()
            ddStatus.ClearSelection()
            ddLocation.ClearSelection()
            ddTourTime.ClearSelection()

            Try

                Dim package_id = wiz.Packages(0).PackageID
                Dim package_tour_id = 0

                If wiz.Scenario = EnumScenario.Three Or wiz.Scenario = EnumScenario.Two Then
                    package_id = wiz.Packages.First().Package.PackageID
                End If

                If wiz.Scenario = EnumScenario.One Then
                    If wiz.Tour.TourID < 0 And wiz.Tour.BookingDate.Length = 0 Then

                        With New clsPackageTour
                            .Load(package_id, New clsPackageReservation().Find_Res_ID(package_id))
                            package_tour_id = .PackageTourID
                            .Load()

                            wiz.Tour.TourLocationID = .TourLocationID
                            wiz.Tour.CampaignID = .CampaignID
                            wiz.Tour.TypeID = .TourTypeID
                            wiz.Tour.SubTypeID = .TourSubTypeID
                            wiz.Tour.SourceID = .TourSourceID
                            wiz.Tour.StatusID = .TourStatusID

                            wiz.Tour.StatusDate = DateTime.Now
                            wiz.Tour.BookingDate = DateTime.Now.ToShortDateString()
                        End With

                        Dim ds = New clsPackageTourPremium().List_Package_Tour_Premiums(package_tour_id)
                        For Each dr As DataRow In ds.Tables("Premiums").Rows
                            With New clsPackageTourPremium
                                .PackageTourPremiumID = dr("PackageTourPremiumID").ToString()
                                .Load()
                                Dim p As New PremiumIssued()
                                p.PremiumIssuedID = -(wiz.Tour.Premiums.Count() + 1)
                                p.PremiumID = .PremiumID
                                p.CostEA = .CostEA
                                p.QtyAssigned = .QtyAssigned

                                wiz.Tour.Premiums.Add(p)
                            End With
                        Next

                        With New clsPackage
                            .PackageID = package_id
                            .Load()
                            wiz.Tour.Premiums_Max_Cost = .MaxPremiumAmount
                        End With
                    End If
                ElseIf wiz.Scenario = EnumScenario.Ten Then
                    If wiz.Tour.TourID < 0 And wiz.Tour.Premiums.Count = 0 Then

                        With New clsPackageTour
                            .Load(package_id, New clsPackageReservation().Find_Res_ID(package_id))
                            package_tour_id = .PackageTourID
                            .Load()

                            wiz.Tour.TourLocationID = .TourLocationID
                            wiz.Tour.CampaignID = .CampaignID
                            wiz.Tour.TypeID = .TourTypeID
                            wiz.Tour.SubTypeID = .TourSubTypeID
                            wiz.Tour.SourceID = .TourSourceID
                            wiz.Tour.StatusID = .TourStatusID

                            wiz.Tour.StatusDate = DateTime.Now
                            wiz.Tour.BookingDate = DateTime.Now.ToShortDateString()
                        End With

                        Dim ds = New clsPackageTourPremium().List_Package_Tour_Premiums(package_tour_id)
                        For Each dr As DataRow In ds.Tables("Premiums").Rows
                            With New clsPackageTourPremium
                                .PackageTourPremiumID = dr("PackageTourPremiumID").ToString()
                                .Load()
                                Dim p As New PremiumIssued()
                                p.PremiumIssuedID = -(wiz.Tour.Premiums.Count() + 1)
                                p.PremiumID = .PremiumID
                                p.CostEA = .CostEA
                                p.QtyAssigned = .QtyAssigned
                                p.StatusID = .PremiumStatusID
                                wiz.Tour.Premiums.Add(p)
                            End With
                        Next

                        With New clsPackage
                            .PackageID = package_id
                            .Load()
                            wiz.Tour.Premiums_Max_Cost = .MaxPremiumAmount
                        End With
                    End If
                Else

                    If IsPostBack = False Then
                        wiz.Tour.Premiums.Clear()

                        Dim ds = New clsPremiumIssued().Get_Assigned_Premiums(wiz.Tour.TourID)
                        Dim dv = CType(ds.Select(DataSourceSelectArguments.Empty), DataView)

                        For Each dr In dv.ToTable().Rows

                            Dim p = New PremiumIssued()
                            p.PremiumIssuedID = dr("PremiumIssuedID").ToString()
                            p.Load()
                            wiz.Tour.Premiums.Add(p)
                        Next

                        With New clsPackage
                            .PackageID = package_id
                            .Load()
                            wiz.Tour.Premiums_Max_Cost = .MaxPremiumAmount
                        End With
                    End If

                End If

                Dim campaign_id = package_base.Get_Package_Campaign_ID(package_id)
                Dim tour_location_id = package_base.Get_Package_Tour_Location_ID(package_id)

                gridview1.DataSource = package_x.Get_Tours_Available(wiz.Search_CheckIn_Date,
                                                                     campaign_id, tour_location_id)

                gridview1.DataBind()
                btNext.Enabled = IIf(gridview1.Rows.Count = 0, False, True)

                Dim dt = New DataTable
                With dt.Columns
                    .Add("PremiumIssuedID", GetType(Int32))
                    .Add("PremiumID", GetType(Int32))
                    .Add("Premium", GetType(String))
                    .Add("StatusID", GetType(Int32))
                    .Add("Status", GetType(String))
                    .Add("QtyAssigned", GetType(Int16))
                    .Add("CostEA", GetType(Decimal))
                    .Add("IsRequired", GetType(Boolean))
                End With

                With wiz
                    For Each pr In .Tour.Premiums.Where(Function(x) New clsComboItems().Lookup_ComboItem(x.StatusID) <> "Do Not Issue")
                        Dim r = dt.NewRow
                        r("PremiumIssuedID") = pr.PremiumIssuedID
                        r("PremiumID") = pr.PremiumID
                        r("Premium") = pr.PremiumName
                        r("QtyAssigned") = pr.QtyAssigned
                        r("CostEA") = pr.CostEA
                        r("Status") = pr.StatusID

                        If wiz.Scenario = EnumScenario.Ten Then
                            r("IsRequired") = Not pr.Optional(package_tour_id, package_id, pr.PremiumID)
                        Else
                            r("IsRequired") = False
                        End If
                        dt.Rows.Add(r)
                    Next
                End With

                With wiz.Tour
                    txTourID.Text = .TourID

                    txBookingDate.Text = DateTime.Now.ToShortDateString()
                    .BookingDate = DateTime.Now

                    If .TourDate.Length > 0 Then
                        txTourDate.Text = DateTime.Parse(.TourDate).ToShortDateString()
                    End If

                    If .CampaignID > 0 Then
                        If Not ddCampaign.Items.FindByValue(.CampaignID) Is Nothing Then
                            ddCampaign.Items.FindByValue(.CampaignID).Selected = True
                        End If
                    End If

                    If .SourceID > 0 Then
                        If Not ddSource.Items.FindByValue(.SourceID) Is Nothing Then
                            ddSource.Items.FindByValue(.SourceID).Selected = True
                        End If
                    End If

                    If Not ddSubType.Items.FindByValue(.SubTypeID) Is Nothing Then
                        ddSubType.Items.FindByValue(.SubTypeID).Selected = True
                    End If

                    If .TypeID > 0 Then
                        ddType.Items.FindByValue(.TypeID).Selected = True
                    End If

                    If Not ddStatus.Items.FindByValue(.StatusID) Is Nothing Then
                        ddStatus.Items.FindByValue(.StatusID).Selected = True
                    End If

                    If .TourLocationID > 0 Then
                        If Not ddLocation.Items.FindByValue(.TourLocationID) Is Nothing Then
                            ddLocation.Items.FindByValue(.TourLocationID).Selected = True
                        End If
                    End If

                    If .TourTime > 0 Then
                        If Not ddTourTime.Items.FindByValue(.TourTime) Is Nothing Then
                            ddTourTime.Items.FindByValue(.TourTime).Selected = True
                        End If
                    End If
                End With
                gridview2.DataSource = dt
                gridview2.DataBind()

            Catch ex As Exception
                lbErr.Text = ex.Message
                btNext.Enabled = False
            End Try

        ElseIf multiview1.GetActiveView().Equals(view2) Then

            txAmount.Text = ""
            txCbAmount.Text = ""
            txCert.Text = ""


            ddPremiums.Items.Clear()
            ddStatus_P.Items.Clear()

            With ddPremiums
                .DataSource = Listings.ActivePremiums()
                .DataTextField = "PremiumName"
                .DataValueField = "PremiumID"
                .DataBind()
            End With

            With ddStatus_P
                .DataSource = Listings.PremiumStatus()
                .DataTextField = "ComboItem"
                .DataValueField = "ComboItemID"
                .DataBind()
            End With

            With ddQuantity
                .DataSource = Enumerable.Range(0, 10)
                .DataBind()
            End With

            ddPremiums.ClearSelection()
            ddStatus_P.ClearSelection()

            Dim pi = (From p In wiz.Tour.Premiums Where p.PremiumIssuedID = gridview2.Attributes("PremiumIssuedID")).Single()
            With pi
                If Not ddPremiums.Items.FindByValue(.PremiumID) Is Nothing Then
                    ddPremiums.Items.FindByValue(.PremiumID).Selected = True
                End If
                If Not ddStatus_P.Items.FindByValue(.StatusID) Is Nothing Then
                    ddStatus_P.Items.FindByValue(.StatusID).Selected = True
                End If

                txAmount.Text = .CostEA
                txCert.Text = .CertificateNumber
                ddQuantity.Items.FindByValue(.QtyAssigned).Selected = True
            End With
        End If
    End Sub

    Private Sub Form_Save()

        Dim campaign_id = 0
        Dim tour_location_id = 0

        If multiview1.GetActiveView().Equals(view1) Then
            With wiz.Tour

                .BookingDate = DateTime.Now.ToShortDateString()
                .StatusID = New clsComboItems().Lookup_ID("TourStatus", "Booked")
                .TypeID = ddType.SelectedItem.Value
                .SourceID = ddSource.SelectedItem.Value
                .TourLocationID = ddLocation.SelectedItem.Value
                .CampaignID = ddCampaign.SelectedItem.Value
                .BookingDate = DateTime.Now.ToShortDateString()

                campaign_id = .CampaignID
                tour_location_id = .TourLocationID

                If txTourDate.Text.Length = 0 Then
                    .TourDate = DateTime.Now.ToShortDateString()
                Else
                    .TourDate = txTourDate.Text.Trim()
                End If
                If ddSubType.SelectedItem.Value.Length > 0 Then
                    .SubTypeID = ddSubType.SelectedItem.Value
                End If
                If ddTourTime.SelectedItem.Value.Length > 0 Then
                    .TourTime = ddTourTime.SelectedItem.Value
                End If
                Try

                    Dim tours_avail_dt = package_x.Get_Tours_Available(wiz.Search_CheckIn_Date, campaign_id, tour_location_id)
                    Dim is_match = False

                    For Each dr As DataRow In tours_avail_dt.Rows
                        Dim tour_day = Convert.ToDateTime(dr("TourDate").ToString())
                        Dim tour_time = dr("TOURTIME").ToString()

                        If tour_time.IndexOf("PM") > 0 Then
                            tour_time = tour_time.Substring(0, tour_time.IndexOf(" "))
                            tour_time = 12 + Integer.Parse(Left(tour_time.Replace(":", ""), 1)) & Right(tour_time.Replace(":", ""), 2)
                        Else
                            tour_time = tour_time.Substring(0, tour_time.IndexOf(" "))
                            tour_time = tour_time.Replace(":", "")
                        End If

                        Dim tour_time_cb_id = New clsComboItems().Lookup_ID("TourTime", tour_time)

                        If tour_day = .TourDate And .TourTime = tour_time_cb_id Then
                            is_match = True
                        End If
                    Next
                    If is_match = False Then
                        Throw New Exception(String.Format("Tour date {0} and tour time {1} does not match any on the <b>Tours Availability</b> list", .TourDate, ddTourTime.SelectedItem.Text))
                    End If
                Catch ex As Exception
                    If TypeOf ex Is ArgumentNullException Or TypeOf ex Is ArgumentException Or TypeOf ex Is Exception_Tour_Waves_Not_Available Then
                        Throw New Exception(ex.Message)
                    Else
                        Throw New Exception(ex.Message)
                    End If
                End Try

            End With

        ElseIf multiview1.GetActiveView().Equals(view2) Then
            Try
                Dim premiumIssuedID = gridview2.Attributes("PremiumIssuedID")

                With (From p In wiz.Tour.Premiums Where p.PremiumIssuedID = premiumIssuedID Select p).Single()
                    .PremiumID = ddPremiums.SelectedItem.Value
                    .CertificateNumber = txCert.Text.Trim()
                    .QtyAssigned = ddQuantity.SelectedItem.Value
                    .CostEA = txAmount.Text
                    If String.IsNullOrEmpty(ddStatus_P.SelectedItem.Value) = False Then
                        .StatusID = ddStatus_P.SelectedItem.Value
                    End If
                End With
            Catch ex As Exception
                Throw ex
            End Try
        End If
        Session("wizData" + wiz.GUID_TIMESTAMP) = New JavaScriptSerializer().Serialize(wiz)
    End Sub

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

    Private Class Listings

        Public Shared Function PremiumStatus() As DataTable
            Dim ds = New clsComboItems().Load_ComboItems("PREMIUMSTATUS")
            Dim dv = CType(ds.Select(DataSourceSelectArguments.Empty), DataView)
            Return dv.ToTable()
        End Function

        Public Shared Function ActivePremiums() As DataTable
            Dim ds = New clsPremium().List_Active()
            Dim dv = CType(ds.Select(DataSourceSelectArguments.Empty), DataView)
            Return dv.ToTable()
        End Function

        Public Shared Function ActiveCampaigns() As DataTable
            Dim ds = New clsCampaign().List_Lookup()
            Dim dv = CType(ds.Select(DataSourceSelectArguments.Empty), DataView)
            Return dv.ToTable()
        End Function

        Public Shared Function TourTime() As DataTable
            Dim ds = New clsComboItems().Load_ComboItems("TOURTIME")
            Dim dv = CType(ds.Select(DataSourceSelectArguments.Empty), DataView)
            Return dv.ToTable()
        End Function

        Public Shared Function TourStatus() As DataTable
            Dim ds = New clsComboItems().Load_ComboItems("TOURSTATUS")
            Dim dv = CType(ds.Select(DataSourceSelectArguments.Empty), DataView)
            Return dv.ToTable()
        End Function

        Public Shared Function TourLocation() As DataTable
            Dim ds = New clsComboItems().Load_ComboItems("TOURLOCATION")
            Dim dv = CType(ds.Select(DataSourceSelectArguments.Empty), DataView)
            Return dv.ToTable()
        End Function

        Public Shared Function TourType() As DataTable
            Dim ds = New clsComboItems().Load_ComboItems("TOURTYPE")
            Dim dv = CType(ds.Select(DataSourceSelectArguments.Empty), DataView)
            Return dv.ToTable()
        End Function

        Public Shared Function TourSubType() As DataTable
            Dim ds = New clsComboItems().Load_ComboItems("TOURSUBTYPE")
            Dim dv = CType(ds.Select(DataSourceSelectArguments.Empty), DataView)
            Return dv.ToTable()
        End Function

        Public Shared Function TourSource() As DataTable
            Dim ds = New clsComboItems().Load_ComboItems("TOURSOURCE")
            Dim dv = CType(ds.Select(DataSourceSelectArguments.Empty), DataView)
            Return dv.ToTable()
        End Function
    End Class


    Protected Sub btNext_Click(sender As Object, e As System.EventArgs) Handles btNext.Click

        'regardless the scenario
        With wiz.Tour
            If .Premiums_Max_Cost < .Premiums.Where(Function(x) x.PremiumIssuedStatus = "Not Issued").Sum(Function(x) x.QtyAssigned * x.CostEA) And .Premiums_Max_Cost > 0 Then
                lbPriceOverLimit.Text = String.Format("<div class='alert alert-warning alert-dismissable'><button class='close' data-dismiss='alert'>&times;</button>The sum of all premiums can not exceed {0:C2}</div>", .Premiums_Max_Cost)
                Return
            End If
        End With
        Dim save_throws_exception = False


        Try
            Form_Save()
            Try
                Dim rooms_id As New List(Of Int32)
                For Each w_p As Wizard_Package In wiz.Packages
                    For Each r_w As Wizard_Room In w_p.RoomID_List
                        rooms_id.Add(r_w.Room_ID)
                    Next
                Next
                save_throws_exception = Not package_x.Ensure_Rooms_Still_Available(
                    rooms_id, wiz.Reservation.CheckInDate, DateTime.Parse(wiz.Reservation.CheckOutDate).Subtract(DateTime.Parse(wiz.Reservation.CheckInDate)).Days)

            Catch ex As Exception_Allocated_Rooms_No_Longer_Available
                save_throws_exception = True
                lbErr.Text = ex.Message
            End Try
        Catch ex As Exception
            lbErr.Text = ex.Message
            save_throws_exception = True
        End Try

        If save_throws_exception = False Then
            Dim bt = CType(sender, Button)
            bt.Attributes.Add("nav", 1)
            Navigate(CType(sender, Button))
        End If
    End Sub
End Class






