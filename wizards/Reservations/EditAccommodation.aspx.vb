Imports System.Data.SqlClient
Imports System.Reflection
Imports System.Data
Imports System.Linq
Imports clsReservationWizard
Imports System.Web.Script.Serialization

Partial Class wizards_Reservations_EditAccommodation
    Inherits System.Web.UI.Page

    Private package_base As New Base_Package
    Private wiz As New Wizard

#Region "Event Handlers"

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

        If IsPostBack = False Then
            Form_Load()
        End If
    End Sub
    Protected Sub btPrevious_Click(sender As Object, e As System.EventArgs) Handles btPrevious.Click
        Dim bt = CType(sender, Button)
        bt.Attributes.Add("nav", 0)
        Navigate(CType(sender, Button))
    End Sub

    Protected Sub btNext_Click(sender As Object, e As System.EventArgs) Handles btNext.Click
        Form_Save()
        Dim bt = CType(sender, Button)
        bt.Attributes.Add("nav", 1)
        Navigate(bt)
    End Sub
    Private Sub Form_Load()

        If IsPostBack = False Then
            ddLocation.DataSource = ComboItem.ReservationLocations()
            ddLocation.Items.Add(New ListItem("(Empty)", ""))
            ddLocation.AppendDataBoundItems = True
            ddLocation.DataTextField = "ComboItem"
            ddLocation.DataValueField = "ComboItemID"
            ddLocation.DataBind()

            ddGuestType.DataSource = ComboItem.GuestTypes()
            ddGuestType.Items.Add(New ListItem("(Empty)", ""))
            ddGuestType.AppendDataBoundItems = True
            ddGuestType.DataTextField = "ComboItem"
            ddGuestType.DataValueField = "ComboItemID"
            ddGuestType.DataBind()

            ddRoomType.DataSource = ComboItem.RoomTypes()
            ddRoomType.Items.Add(New ListItem("(Empty)", ""))
            ddRoomType.AppendDataBoundItems = True
            ddRoomType.DataTextField = "ComboItem"
            ddRoomType.DataValueField = "ComboItemID"
            ddRoomType.DataBind()
        End If

        ddLocation.ClearSelection()
        ddAccom.ClearSelection()
        ddCheckInLocation.ClearSelection()
        ddGuestType.ClearSelection()
        ddRoomType.ClearSelection()

        Dim package_base = New Base_Package

        With wiz.Reservation

            Dim package_id = 0

            If wiz.Scenario = EnumScenario.Two Or wiz.Scenario = EnumScenario.Three Then
                package_id = wiz.Packages.First().Package.PackageID
            End If
            If .Accommodation.AccommodationID < 1 Then
                .Accommodation.AccommodationID = Helper.Int32_Randomized
            End If

            If .ResLocationID > 0 Then
                ddLocation.Items.FindByValue(.ResLocationID).Selected = True
            End If

            If .ReservationNumber.Length > 0 Then txConfirmationNumber.Text = .ReservationNumber.Trim()
            If .ReservationNumber.Length = 0 Then txConfirmationNumber.Text = .Accommodation.ConfirmationNumber.Trim()
            txID.Text = .Accommodation.AccommodationID
            txArrivalDate.Text = DateTime.Parse(.CheckInDate).ToString("D")
            txDepartureDate.Text = DateTime.Parse(.CheckOutDate).ToString("D")

            If .ReservationNumber.Length = 0 Then
                .ReservationNumber = .Accommodation.ConfirmationNumber.Trim()
            End If

            Dim dt = ComboItem.Accom(1007, 0)

            If dt.Rows.Count() > 0 Then
                ddAccom.DataSource = dt.AsEnumerable().Where(Function(x) x.Item("AccomName").ToString().Equals("Best Western") = False).Select(Function(x) x).CopyToDataTable()
                ddAccom.AppendDataBoundItems = True
                ddAccom.DataTextField = "AccomName"
                ddAccom.DataValueField = "AccomID"
                ddAccom.DataBind()
            End If

            With New clsPackage
                .PackageID = package_id
                .Load()
                wiz.Reservation.Accommodation.AccomID = .AccomID
            End With

            If Not ddAccom.Items.FindByText(package_base.Get_Accom_Name(wiz.Packages(0).Package.PackageID)) Is Nothing Then
                ddAccom.Items.FindByText(package_base.Get_Accom_Name(wiz.Packages(0).Package.PackageID)).Selected = True
            End If

            ddCheckInLocation.DataSource = ComboItem.CheckInLocations(0, 290)
            ddCheckInLocation.AppendDataBoundItems = True
            ddCheckInLocation.DataTextField = "Location"
            ddCheckInLocation.DataValueField = "ID"
            ddCheckInLocation.DataBind()

            If Not ddGuestType.Items.FindByValue(wiz.Reservation.Accommodation.GuestTypeID) Is Nothing Then
                ddGuestType.Items.FindByValue(wiz.Reservation.Accommodation.GuestTypeID).Selected = True
            Else
                ddGuestType.Items.FindByText("Non-Owner").Selected = True
            End If

            If Not ddRoomType.Items.FindByValue(wiz.Reservation.Accommodation.RoomTypeID) Is Nothing Then
                ddRoomType.Items.FindByValue(wiz.Reservation.Accommodation.RoomTypeID).Selected = True
            Else
                ddRoomType.Items.FindByText("Standard").Selected = True
            End If
        End With
    End Sub

    Private Sub Form_Save()

        With wiz.Reservation
            .ReservationNumber = txConfirmationNumber.Text.ToUpper().Trim()
            .Accommodation.ConfirmationNumber = .ReservationNumber
            .Accommodation.ReservationID = .ReservationID
            .Accommodation.PromoNights = package_base.Get_Promo_Nights(wiz.Packages(0).Package.PackageID)
            .Accommodation.AdditionalNights = 0

            .Accommodation.ArrivalDate = .CheckInDate
            .Accommodation.DepartureDate = .CheckOutDate
            .Accommodation.NumberAdults = .NumberAdults
            .Accommodation.NumberChildren = .NumberChildren

            If ddAccom.SelectedItem.Value.Length > 0 Then
                .Accommodation.AccomID = ddAccom.SelectedItem.Value
            End If

            If ddGuestType.SelectedItem.Value.Length > 0 Then
                .Accommodation.GuestTypeID = ddGuestType.SelectedItem.Value
            End If

            If ddCheckInLocation.SelectedItem.Value.Length > 0 Then
                .Accommodation.CheckInLocationID = ddCheckInLocation.SelectedItem.Value
            End If

            If ddRoomType.SelectedItem.Value.Length > 0 Then
                .Accommodation.RoomTypeID = ddRoomType.SelectedItem.Value
            End If

        End With

        Session("wizData" + wiz.GUID_TIMESTAMP) = New JavaScriptSerializer().Serialize(wiz)
    End Sub
#End Region

#Region "Sub/Function"

    Private Sub Navigate(bt As Button)
        Dim gt = Me.Master.GetType()
        Dim mi As MethodInfo = gt.GetMethod("Navigate")
        Dim parameters = mi.GetParameters()

        Dim dir = Convert.ToInt32(bt.Attributes("nav"))
        Dim parametersArray() As Object = New Object() {Request.PhysicalPath.Substring(Request.PhysicalPath.LastIndexOf("\") + 1).ToLower(), dir}
        Dim r = mi.Invoke(Me.Master, parametersArray)
        Response.Redirect(r.ToString())
    End Sub

    Class ComboItem
        Public Shared Function CheckInLocations(accomLocationID As Int32, Optional accomID As Int32 = 0) As DataTable
            Dim oAccLoc As New clsAccom2CheckInLocation
            Dim ds = oAccLoc.CheckIn_Locations_By_Accom(accomID, accomLocationID)
            Dim dv As DataView = CType(ds.Select(DataSourceSelectArguments.Empty), DataView)
            Return dv.ToTable()
        End Function

        Public Shared Function ReservationLocations() As DataTable
            Dim ds = New clsComboItems().Load_ComboItems("ReservationLocation")
            Dim dv As DataView = CType(ds.Select(DataSourceSelectArguments.Empty), DataView)
            Return dv.ToTable()
        End Function

        Public Shared Function Accom(accomLocationID As Int32, Optional accomID As Int32 = 0) As DataTable
            Dim oAcc As New clsAccom
            Dim ds = oAcc.Accoms_By_Location(accomLocationID, accomID)
            Dim dv As DataView = CType(ds.Select(DataSourceSelectArguments.Empty), DataView)
            Return dv.ToTable()
        End Function

        Public Shared Function GuestTypes() As DataTable
            Dim ds = New clsComboItems().Load_ComboItems("AccGuestType")
            Dim dv As DataView = CType(ds.Select(DataSourceSelectArguments.Empty), DataView)
            Dim dt = dv.ToTable()
            Return dv.ToTable()
        End Function

        Public Shared Function RoomTypes() As DataTable
            Dim ds = New clsComboItems().Load_ComboItems("AccomRoomType")
            Dim dv As DataView = CType(ds.Select(DataSourceSelectArguments.Empty), DataView)
            Dim dt = dv.ToTable()
            Return dt.AsEnumerable().Where(Function(x) x.Item("ComboItem").ToString().Equals("Standard") Or x.Item("ComboItem").ToString().Equals("Suite")).Select(Function(x) x).CopyToDataTable()
        End Function
    End Class

#End Region


End Class

