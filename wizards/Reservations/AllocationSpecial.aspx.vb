Imports System.Data
Imports System.Reflection
Imports System.Data.SqlClient
Imports System.Runtime.CompilerServices
Imports System.Linq

Imports clsReservationWizard
Imports System.Web.Script.Serialization

Partial Class wizard_Reservations_AllocationSpecial
    Inherits System.Web.UI.Page

    Private package_base As New Base_Package
    Private package_x As New Package_Xtra
    Private wiz As New Wizard

    Private iGrid As IGridRefreshSpecial

#Region "Event Handlers"

    Private Sub wizard_Reservations_Allocation_Load(sender As Object, e As EventArgs) Handles Me.Load

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
        lbBuyAdditional.Visible = False
        lbErr.Visible = True

        gridview1.Visible = False
        gridview3.Visible = False

        If wiz.Scenario = EnumScenario.One Then
            Dim sc = New Scenario_A
            iGrid = sc
            AddHandler gridview3.RowDataBound, AddressOf sc.GridView_RowDataBound
            If IsPostBack = False Then
                iGrid.Refresh_Rooms(gridview3, wiz.Packages)
            End If

            gridview3.Visible = True
        ElseIf wiz.Scenario = EnumScenario.Two Or wiz.Scenario = EnumScenario.Three Then

            Dim sc = New Scenario_B
            iGrid = sc
            AddHandler gridview1.RowDataBound, AddressOf sc.GridView_RowDataBound
            If IsPostBack = False Then
                iGrid.Refresh_Rooms(gridview1, wiz.Packages)
            End If
            gridview1.Visible = True
        ElseIf wiz.Scenario = EnumScenario.Ten Then
            Dim sc = New Scenario_A
            iGrid = sc
            AddHandler gridview3.RowDataBound, AddressOf sc.GridView_RowDataBound
            If IsPostBack = False Then
                iGrid.Refresh_Rooms(gridview3, wiz.Packages)
            End If

            gridview3.Visible = True
        End If

    End Sub
    Protected Sub gridview1_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridview1.RowCommand
        ' Scenario two and three
        Dim gvr = CType(CType(e.CommandSource, Button).NamingContainer, GridViewRow)
        Dim button = CType(e.CommandSource, Button)
        Dim nights_stay = DateTime.Parse(wiz.Reservation.CheckOutDate).Subtract(DateTime.Parse(wiz.Reservation.CheckInDate)).Days
        Dim select_package_guid = button.Attributes("GUID")
        Dim package_id = wiz.Packages.Single(Function(x) x.Package.GUID = select_package_guid).Package.PackageID
        If button.ID = "btSelect" Then
            If wiz.Scenario = EnumScenario.Two Or wiz.Scenario = EnumScenario.Three Then
                Dim o = CType(iGrid, Scenario_B)
                If button.Text <> "Remove" Then
                    Try
                        Dim dt = o.Search_Available_Rooms(select_package_guid, wiz.Packages, wiz.Reservation.CheckInDate, nights_stay)
                        wiz.Inventories_Available.Clear()
                        wiz.Inventories_Available = Package_Xtra.Inventories_Available

                        Session("wizData" + wiz.GUID_TIMESTAMP) = New JavaScriptSerializer().Serialize(wiz)
                        Session("wizInventories" + wiz.GUID_TIMESTAMP) = New JavaScriptSerializer().Serialize(wiz.Inventories_Available)

                        gridview2.DataSource = dt.Rows.OfType(Of DataRow).OrderBy(Function(x) x.Field(Of String)("UnitType")).CopyToDataTable()
                        gridview2.DataBind()
                        gridview2.Attributes("GUID") = button.Attributes("GUID")
                        gridview2.Attributes("Mode") = "Select"

                        multiview1.SetActiveView(view2)
                    Catch ex As Exception
                        lbErr.Text = ex.Message
                        lbErr.ForeColor = Drawing.Color.Red
                    End Try
                Else
                    o.Remove_Room(select_package_guid, wiz.Packages)
                    Session("wizData" + wiz.GUID_TIMESTAMP) = New JavaScriptSerializer().Serialize(wiz)
                    Session("wizPackages_Ghost" + wiz.GUID_TIMESTAMP) = New JavaScriptSerializer().Serialize(wiz.Packages)
                    iGrid.Refresh_Rooms(gridview1, wiz.Packages)
                End If
            End If
        End If
    End Sub
    Protected Sub gridview2_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridview2.RowCommand
        Dim row = CType(CType(e.CommandSource, Button).NamingContainer, GridViewRow)
        Dim cmd = e.CommandName
        Dim btn = CType(e.CommandSource, Button)

        Dim p_GUID = gridview2.Attributes("GUID")
        Dim r_GUID = btn.Attributes("GUID")

        Package_Xtra.Inventories_Available = wiz.Inventories_Available

        If TypeOf iGrid Is Scenario_A Then

            Dim o = CType(iGrid, Scenario_A)

            iGrid.Add_Room(p_GUID, r_GUID, wiz.Packages)
            iGrid.Refresh_Rooms(gridview3, wiz.Packages)
            If gridview2.Attributes("Mode") = "Select" Then

                o.Add_Room(p_GUID, r_GUID, wiz.Packages)
                Session("wizPackages_Ghost" + wiz.GUID_TIMESTAMP) = New JavaScriptSerializer().Serialize(wiz.Packages)
                iGrid.Refresh_Rooms(gridview1, wiz.Packages)
            End If
        End If

        If TypeOf iGrid Is Scenario_B Then

            Dim o = CType(iGrid, Scenario_B)
            If gridview2.Attributes("Mode") = "Select" Then

                o.Add_Room(p_GUID, r_GUID, wiz.Packages)
                Session("wizPackages_Ghost" + wiz.GUID_TIMESTAMP) = New JavaScriptSerializer().Serialize(wiz.Packages)
                iGrid.Refresh_Rooms(gridview1, wiz.Packages)
            End If
        End If

        Session("wizData" + wiz.GUID_TIMESTAMP) = New JavaScriptSerializer().Serialize(wiz)
        multiview1.SetActiveView(view1)
        Package_Xtra.Inventories_Available.Clear()
    End Sub
    Protected Sub gridview2_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gridview2.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim btSelect = CType(e.Row.FindControl("btRoomSelect"), Button)
            Dim dataItem = CType(e.Row.DataItem, DataRowView)
            btSelect.Attributes("GUID") = dataItem("GUID").ToString()
        End If
    End Sub
    Protected Sub gridview3_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridview3.RowCommand
        Dim row = CType(CType(e.CommandSource, Button).NamingContainer, GridViewRow)
        Dim cmd = e.CommandName
        Dim button = CType(e.CommandSource, Button)
        Dim nights_stay = DateTime.Parse(wiz.Reservation.CheckOutDate).Subtract(DateTime.Parse(wiz.Reservation.CheckInDate)).Days
        Dim select_package_guid = button.Attributes("GUID")
        Dim package_id = wiz.Packages.Single(Function(x) x.GUID = select_package_guid).PackageID

        If TypeOf iGrid Is Scenario_A Then
            If button.Text <> "Remove" Then
                Try

                    Dim o = CType(iGrid, Scenario_A)
                    Dim dt = o.Search_Available_Rooms(select_package_guid, wiz.Packages, wiz.Reservation.CheckInDate, nights_stay)
                    wiz.Inventories_Available.Clear()
                    Dim l_w_r As New List(Of List(Of Wizard_Room))
                    For Each w_r As List(Of Wizard_Room) In Package_Xtra.Inventories_Available
                        Dim found = False
                        For Each p As Wizard_Package In wiz.Packages
                            If p.RoomID_List.Count > 0 Then
                                If p.RoomID_List(0).Room_ID = w_r.First().Room_ID Then
                                    found = True
                                End If
                            End If
                        Next
                        If found = False Then
                            l_w_r.Add(w_r)
                        End If
                    Next
                    wiz.Inventories_Available = l_w_r
                    Session("wizData" + wiz.GUID_TIMESTAMP) = New JavaScriptSerializer().Serialize(wiz)
                    Session("wizInventories" + wiz.GUID_TIMESTAMP) = New JavaScriptSerializer().Serialize(wiz.Inventories_Available)

                    gridview2.DataSource = dt.Rows.OfType(Of DataRow).OrderBy(Function(x) x.Field(Of String)("UnitType")).CopyToDataTable()
                    gridview2.DataBind()
                    gridview2.Attributes("GUID") = button.Attributes("GUID")
                    gridview2.Attributes("Mode") = "Select"

                    multiview1.SetActiveView(view2)

                Catch ex As Exception
                    lbErr.Text = ex.Message
                    lbErr.ForeColor = Drawing.Color.Red
                End Try

            Else
                iGrid.Remove_Room(select_package_guid, wiz.Packages)
                Session("wizData" + wiz.GUID_TIMESTAMP) = New JavaScriptSerializer().Serialize(wiz)
                Session("wizPackages_Ghost" + wiz.GUID_TIMESTAMP) = New JavaScriptSerializer().Serialize(wiz.Packages)
                iGrid.Refresh_Rooms(gridview3, wiz.Packages)
            End If
        End If

    End Sub
    Protected Sub gridview4_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridview4.RowCommand
        Dim row = CType(CType(e.CommandSource, Button).NamingContainer, GridViewRow)
        Dim cmd = e.CommandName
        Dim btn = CType(e.CommandSource, Button)

        If wiz.Scenario = EnumScenario.Two Then

            Dim o = CType(iGrid, Scenario_B)

        ElseIf wiz.Scenario = EnumScenario.Four Then

            Dim o = CType(iGrid, ScenarioFour)

            If btn.Text <> "Remove" Then

            Else

            End If

        End If

        Session("wizData" + wiz.GUID_TIMESTAMP) = New JavaScriptSerializer().Serialize(wiz)

    End Sub
    Protected Sub gridview4_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gridview4.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim dataItem = CType(e.Row.DataItem, DataRowView)
            Dim btSelect = CType(e.Row.FindControl("btSelect"), Button)
            Dim lbPackage = CType(e.Row.FindControl("lbPackage"), Label)
            Dim lbUnit = CType(e.Row.FindControl("lbUnit"), Label)
            Dim lbRoomNumber = CType(e.Row.FindControl("lbRoomNumber"), Label)
            Dim lbBedRoom = CType(e.Row.FindControl("lbBedRoom"), Label)
            Dim lbStyle = CType(e.Row.FindControl("lbStyle"), Label)

            lbPackage.Text = dataItem("Package").ToString()
            lbUnit.Text = dataItem("UnitType").ToString()
            lbRoomNumber.Text = dataItem("RoomNumber").ToString()
            lbBedRoom.Text = dataItem("Beds").ToString()

            lbStyle.Text = dataItem("Style").ToString()

            btSelect.Attributes("GUID") = dataItem("GUID")

            If lbRoomNumber.Text.Length > 0 Then btSelect.Text = "Remove"

        End If
    End Sub
    Protected Sub btPrevious_Click(sender As Object, e As EventArgs) Handles btPrevious.Click
        Dim bt = CType(sender, Button)
        bt.Attributes.Add("nav", 0)
        Navigate(CType(sender, Button))
    End Sub
    Protected Sub btNext_Click(sender As Object, e As EventArgs) Handles btNext.Click

        If iGrid.Ensure_Rooms_Allocated(wiz.Packages) = False Then
            lbErr.Visible = True
            lbErr.Text = String.Format("Please ensure all rooms have been selected.")
            Return
        End If

        Dim save_throws_exception = False

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

        If save_throws_exception = False Then
            Dim bt = CType(sender, Button)
            bt.Attributes.Add("nav", 1)
            Navigate(bt)
        End If
    End Sub
    Protected Sub btCancel_Click(sender As Object, e As EventArgs) Handles btCancel.Click
        multiview1.SetActiveView(view1)
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

End Class

Interface IGridRefreshSpecial
    Sub Refresh_Rooms(gv As GridView, packages As List(Of Wizard_Package))
    Function Search_Available_Rooms(package_guid As String, packages As List(Of Wizard_Package), checkin_date As DateTime, nights_stay As Int32) As DataTable
    Sub Remove_Room(p_guid As String, packages As List(Of Wizard_Package))
    Sub Add_Room(p_guid As String, r_guid As String, packages As List(Of Wizard_Package))
    Function Ensure_Rooms_Allocated(packages As List(Of Wizard_Package)) As Boolean
    Sub GridView_RowDataBound(sender As Object, e As GridViewRowEventArgs)
End Interface

Class Scenario_A
    Implements IGridRefreshSpecial

    Private package_x As New Package_Xtra
    Private package_base As New Base_Package

    Public Function Search_Available_Rooms(package_guid As String, packages As List(Of Wizard_Package), checkin_date As DateTime, nights_stay As Int32) As DataTable Implements IGridRefreshSpecial.Search_Available_Rooms
        Dim dt = New DataTable()
        With dt
            .Columns.Add("GUID")
            .Columns.Add("RoomNumber")
            .Columns.Add("Beds")
            .Columns.Add("UnitType")
            .Columns.Add("UnitStyle")
        End With

        Dim package_id = packages.Single(Function(x) x.GUID = package_guid).PackageID
        Dim bed_room = package_base.Get_Package_Bedroom(package_id)
        Dim reservation_type_id = package_base.Get_Package_Reservation_Type_ID(package_id)
        Dim room_unit_type_id = package_base.Get_Package_Unit_Type_ID(package_id)
        Dim up_dwn = String.Empty

        Try
            If bed_room.ToLower().IndexOf("BD-UP".ToLower()) >= 0 Or bed_room.ToLower().IndexOf("BD-DWN".ToLower()) >= 0 Then
                If bed_room.IndexOf("DWN") > 2 Then
                    up_dwn = "DWN"
                Else
                    up_dwn = "UP"
                End If
            End If

            Dim ra = package_x.Get_Rooms_Avalailable(bed_room.Substring(0, 1), room_unit_type_id, reservation_type_id, checkin_date, nights_stay, up_dwn)

            For Each w_r_l As List(Of Wizard_Room) In ra
                Dim found = False

                For Each p As Wizard_Package In packages
                    If p.RoomID_List.Count > 0 Then
                        If p.RoomID_List(0).Room_ID = w_r_l.First().Room_ID Then
                            found = True
                        End If
                    End If
                Next

                If found = False Then
                    Dim new_row = dt.NewRow()
                    Dim room_numbers As New List(Of String)
                    Dim bd = 0
                    Dim unit_type_stye = w_r_l.First().Room_Sub_Type_1a

                    For Each w_r As Wizard_Room In w_r_l
                        room_numbers.Add(w_r.Room_Number)
                        bd += w_r.Room_Type.Substring(0, 1)
                    Next

                    new_row("RoomNumber") = String.Join("<br/>", room_numbers)
                    If New clsComboItems().Lookup_ComboItem(room_unit_type_id) = "Estates" And w_r_l.Count = 1 Then
                        new_row("Beds") = w_r_l(0).Room_Type
                    Else
                        new_row("Beds") = bd.ToString() + "BD"
                    End If

                    new_row("UnitType") = New clsComboItems().Lookup_ComboItem(room_unit_type_id)
                    new_row("UnitStyle") = unit_type_stye
                    new_row("GUID") = w_r_l.First().Wizard_Room_GUID

                    dt.Rows.Add(new_row)
                End If
            Next

        Catch ex As Exception_Allocation_Room_Search_Not_Exist
            Throw ex
        Catch ex As Exception
            Throw ex
        End Try
        Return dt
    End Function
    Public Sub Add_Room(p_guid As String, r_guid As String, packages As List(Of Wizard_Package)) Implements IGridRefreshSpecial.Add_Room
        Dim selected_package = packages.Single(Function(x) x.GUID = p_guid)
        For Each w_r_l As List(Of Wizard_Room) In Package_Xtra.Inventories_Available
            If w_r_l.First().Wizard_Room_GUID = r_guid Then
                selected_package.RoomID_List = w_r_l
            End If
        Next
    End Sub
    Public Sub Remove_Room(p_guid As String, packages As List(Of Wizard_Package)) Implements IGridRefreshSpecial.Remove_Room
        packages.Single(Function(x) x.GUID = p_guid).RoomID_List.Clear()
    End Sub
    Public Function Ensure_Rooms_Allocated(packages As List(Of Wizard_Package)) As Boolean Implements IGridRefreshSpecial.Ensure_Rooms_Allocated
        Dim tf = True
        For Each p As Wizard_Package In packages
            If p.RoomID_List.Count = 0 Then
                tf = False
                Exit For
            End If
        Next
        Return tf
    End Function

    'gridview3  
    Public Sub Refresh_Rooms(gv As GridView, packages As List(Of Wizard_Package)) Implements IGridRefreshSpecial.Refresh_Rooms
        Dim dt = New DataTable()
        With dt
            .Columns.Add("Unit")
            .Columns.Add("Room")
            .Columns.Add("Bed")
            .Columns.Add("Style")
            .Columns.Add("Package_Description")
            .Columns.Add("GUID")
        End With

        Dim package_id = 0

        Dim room_numbers As New List(Of String)
        Dim bd = 0
        Dim unit_type_stye = String.Empty
        Dim room_unit_type_id = 0

        For Each w_p As Wizard_Package In packages
            package_id = w_p.PackageID
            Dim w_r_l = w_p.RoomID_List
            Dim new_row = dt.NewRow()
            room_numbers.Clear()
            room_unit_type_id = package_base.Get_Package_Unit_Type_ID(package_id)
            bd = 0

            new_row("Package_Description") = package_base.Get_Package_Description(package_id)

            For Each w_r As Wizard_Room In w_r_l

                room_numbers.Add(w_r.Room_Number)
                bd += w_r.Room_Type.Substring(0, 1)
            Next

            If w_r_l.Count > 0 Then
                unit_type_stye = w_r_l.First().Room_Sub_Type_1a

                new_row("Unit") = New clsComboItems().Lookup_ComboItem(room_unit_type_id)
                new_row("Room") = String.Join("<br/>", room_numbers)
                If New clsComboItems().Lookup_ComboItem(room_unit_type_id) = "Estates" And w_r_l.Count = 1 Then
                    new_row("Bed") = w_r_l(0).Room_Type
                Else
                    new_row("Bed") = bd.ToString() + "BD"
                End If
                new_row("Style") = unit_type_stye
            End If
            new_row("GUID") = w_p.GUID
            dt.Rows.Add(new_row)
        Next

        gv.DataSource = dt
        gv.DataBind()
    End Sub
    Public Sub GridView_RowDataBound(sender As Object, e As GridViewRowEventArgs) Implements IGridRefreshSpecial.GridView_RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dataItem = CType(e.Row.DataItem, DataRowView)

            Dim lbUnit = CType(e.Row.FindControl("lbUnit"), Label)
            Dim lbRoomNumber = CType(e.Row.FindControl("lbRoomNumber"), Label)
            Dim lbBedRoom = CType(e.Row.FindControl("lbBedRoom"), Label)
            Dim lbStyle = CType(e.Row.FindControl("lbStyle"), Label)
            Dim btSelect = CType(e.Row.FindControl("btSelect"), Button)
            Dim lbPackage = CType(e.Row.FindControl("lbPackage"), Label)

            lbUnit.Text = dataItem("Unit").ToString()
            lbRoomNumber.Text = dataItem("Room").ToString()
            lbBedRoom.Text = dataItem("Bed").ToString()
            lbStyle.Text = dataItem("Style").ToString()
            lbPackage.Text = dataItem("Package_Description").ToString()

            If dataItem("GUID").Equals(DBNull.Value) = False Then
                btSelect.Attributes("GUID") = dataItem("GUID")
            End If
            If lbRoomNumber.Text.Length > 0 Then btSelect.Text = "Remove"
        End If
    End Sub
End Class
Class Scenario_B
    Implements IGridRefreshSpecial

    Private package_x As New Package_Xtra
    Private package_base As New Base_Package

    Public Sub Add_Room(p_guid As String, r_guid As String, packages As List(Of Wizard_Package)) Implements IGridRefreshSpecial.Add_Room

        Dim selected_package = packages.Where(Function(x) x.Package.GUID = p_guid).Single().Package
        For Each w_r_l As List(Of Wizard_Room) In Package_Xtra.Inventories_Available
            If w_r_l.First().Wizard_Room_GUID = r_guid Then
                selected_package.RoomID_List = w_r_l
            End If
        Next
    End Sub
    Public Function Search_Available_Rooms(package_guid As String, packages As List(Of Wizard_Package), checkin_date As DateTime, nights_stay As Int32) As DataTable Implements IGridRefreshSpecial.Search_Available_Rooms
        Dim dt = New DataTable()
        With dt
            .Columns.Add("GUID")
            .Columns.Add("RoomNumber")
            .Columns.Add("Beds")
            .Columns.Add("UnitType")
            .Columns.Add("UnitStyle")
        End With

        Dim package_id = packages.Where(Function(x) x.Package.GUID = package_guid).Single().Package.PackageID
        Dim bed_room = package_base.Get_Package_Bedroom(package_id)
        Dim reservation_type_id = package_base.Get_Package_Reservation_Type_ID(package_id)
        Dim room_unit_type_id = package_base.Get_Package_Unit_Type_ID(package_id)
        Dim up_dwn = String.Empty

        Try
            If bed_room.ToLower().IndexOf("BD-UP".ToLower()) >= 0 Or bed_room.ToLower().IndexOf("BD-DWN".ToLower()) >= 0 Then
                If bed_room.IndexOf("DWN") > 2 Then
                    up_dwn = "DWN"
                Else
                    up_dwn = "UP"
                End If
            End If

            Dim ra = package_x.Get_Rooms_Avalailable(bed_room.Substring(0, 1), room_unit_type_id, reservation_type_id, checkin_date, nights_stay, up_dwn)

            For Each w_r_l As List(Of Wizard_Room) In ra

                Dim found = False

                For Each p As Wizard_Package In packages
                    If p.Package.RoomID_List.Count > 0 Then
                        If p.Package.RoomID_List(0).Room_ID = w_r_l.First().Room_ID Then
                            found = True
                        End If
                    End If
                Next

                If found = False Then
                    Dim new_row = dt.NewRow()
                    Dim room_numbers As New List(Of String)
                    Dim bd = 0
                    Dim unit_type_stye = w_r_l.First().Room_Sub_Type_1a

                    For Each w_r As Wizard_Room In w_r_l
                        room_numbers.Add(w_r.Room_Number)
                        bd += w_r.Room_Type.Substring(0, 1)
                    Next

                    new_row("RoomNumber") = String.Join("<br/>", room_numbers)
                    If New clsComboItems().Lookup_ComboItem(room_unit_type_id) = "Estates" And w_r_l.Count = 1 Then
                        new_row("Beds") = w_r_l(0).Room_Type
                    Else
                        new_row("Beds") = bd.ToString() + "BD"
                    End If

                    new_row("UnitType") = New clsComboItems().Lookup_ComboItem(room_unit_type_id)
                    new_row("UnitStyle") = unit_type_stye
                    new_row("GUID") = w_r_l.First().Wizard_Room_GUID

                    dt.Rows.Add(new_row)
                End If
            Next

        Catch ex As Exception_Allocation_Room_Search_Not_Exist
            Throw ex
        Catch ex As Exception
            Throw ex
        End Try
        Return dt
    End Function
    Public Function Ensure_Rooms_Allocated(packages As List(Of Wizard_Package)) As Boolean Implements IGridRefreshSpecial.Ensure_Rooms_Allocated
        Dim tf = True
        For Each p As Wizard_Package In packages
            If p.Package.RoomID_List.Count = 0 Then
                tf = False
                Exit For
            End If
        Next
        Return tf
    End Function
    Public Sub Remove_Room(p_guid As String, packages As List(Of Wizard_Package)) Implements IGridRefreshSpecial.Remove_Room
        packages.Single(Function(x) x.Package.GUID = p_guid).Package.RoomID_List.Clear()
    End Sub
    Public Sub Refresh_Rooms(gv As GridView, packages As List(Of Wizard_Package)) Implements IGridRefreshSpecial.Refresh_Rooms
        Dim dt = New DataTable()
        With dt
            .Columns.Add("Unit")
            .Columns.Add("Room")
            .Columns.Add("Bed")
            .Columns.Add("Style")
            .Columns.Add("Package_Description")
            .Columns.Add("GUID")
        End With

        Dim package_id = 0

        Dim room_numbers As New List(Of String)
        Dim bd = 0
        Dim unit_type_stye = String.Empty
        Dim room_unit_type_id = 0

        For Each w_p As Wizard_Package In packages
            package_id = w_p.Package.PackageID
            Dim w_r_l = w_p.Package.RoomID_List
            Dim new_row = dt.NewRow()
            room_numbers.Clear()
            room_unit_type_id = package_base.Get_Package_Unit_Type_ID(package_id)
            bd = 0
            new_row("Package_Description") = package_base.Get_Package_Description(package_id)
            Dim show_package_desc_only = False

            For Each w_r As Wizard_Room In w_r_l

                room_numbers.Add(w_r.Room_Number)
                If String.IsNullOrEmpty(w_r.Room_Type) Then
                    show_package_desc_only = True
                Else
                    bd += w_r.Room_Type.Substring(0, 1)
                End If

            Next

            If show_package_desc_only = False Then
                If w_r_l.Count > 0 Then
                    unit_type_stye = w_r_l.First().Room_Sub_Type_1a

                    new_row("Unit") = New clsComboItems().Lookup_ComboItem(room_unit_type_id)
                    new_row("Room") = String.Join("<br/>", room_numbers)
                    If New clsComboItems().Lookup_ComboItem(room_unit_type_id) = "Estates" And w_r_l.Count = 1 Then
                        new_row("Bed") = w_r_l(0).Room_Type
                    Else
                        new_row("Bed") = bd.ToString() + "BD"
                    End If
                    new_row("Style") = unit_type_stye
                End If
            End If

            new_row("GUID") = w_p.Package.GUID
            dt.Rows.Add(new_row)
        Next

        gv.DataSource = dt
        gv.DataBind()
    End Sub
    Public Sub GridView_RowDataBound(sender As Object, e As GridViewRowEventArgs) Implements IGridRefreshSpecial.GridView_RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dataItem = CType(e.Row.DataItem, DataRowView)
            Dim lbUnit = CType(e.Row.FindControl("lbUnit"), Label)
            Dim lbRoomNumber = CType(e.Row.FindControl("lbRoomNumber"), Label)
            Dim lbBedRoom = CType(e.Row.FindControl("lbBedRoom"), Label)
            Dim lbStyle = CType(e.Row.FindControl("lbStyle"), Label)
            Dim lbPackage = CType(e.Row.FindControl("lbPackage"), Label)
            Dim btSelect = CType(e.Row.FindControl("btSelect"), Button)

            lbUnit.Text = dataItem("Unit").ToString()
            lbRoomNumber.Text = dataItem("Room").ToString()
            lbBedRoom.Text = dataItem("Bed").ToString()
            lbStyle.Text = dataItem("Style").ToString()
            lbPackage.Text = dataItem("Package_Description").ToString()

            If dataItem("GUID").Equals(DBNull.Value) = False Then
                btSelect.Attributes("GUID") = dataItem("GUID")
            End If
            If lbRoomNumber.Text.Length > 0 Then btSelect.Text = "Remove"
        End If
    End Sub
End Class

Class ScenarioFour

    Private wiz As clsReservationWizard.Wizard

    Public Sub New(w As clsReservationWizard.Wizard)
        Me.wiz = w
    End Sub
End Class
Module E

    <Extension()>
    Public Sub Disable_Select_Buttons(gv As GridView)

        If gv.ID <> "gridview1" Then Return

        Dim r = From gvr As GridViewRow In gv.Rows
                Where gvr.RowType = DataControlRowType.DataRow
                Select CType(gvr.Cells(5).FindControl("btSelect"), Button)

        Array.ForEach(r.ToArray(), Sub(e) e.Enabled = False)
    End Sub

    <Extension()>
    Public Sub Enable_Upgrade_Button(gv As GridView)
        If gv.ID <> "gridview1" Then Return

        Dim r = From gvr As GridViewRow In gv.Rows
                Where gvr.RowType = DataControlRowType.DataRow
                Select CType(gvr.Cells(0).FindControl("btUpgrade"), Button)

        Array.ForEach(r.ToArray(), Sub(e) e.Enabled = True)
    End Sub

    <Extension()>
    Public Sub Rename_Upgrade_Button_To_Select(gv As GridView)
        If gv.ID <> "gridview1" Then Return

        Dim r = From gvr As GridViewRow In gv.Rows
                Where gvr.RowType = DataControlRowType.DataRow AndAlso CType(gvr.Cells(0).FindControl("btUpgrade"), Button).Text = "Upgrade"
                Select CType(gvr.Cells(0).FindControl("btUpgrade"), Button)

        Array.ForEach(r.ToArray(), Sub(e) e.Text = "Select")
    End Sub

    <Extension()>
    Public Sub Rename_Select_Button_To_Remove(gv As GridView)

        If gv.ID <> "gridview1" Then Return

        Dim r = From gvr As GridViewRow In gv.Rows
                Where gvr.RowType = DataControlRowType.DataRow AndAlso CType(gvr.Cells(1).FindControl("lbUpDnUnit"), Label).Text.Length > 0
                Select CType(gvr.Cells(0).FindControl("btUpgrade"), Button)


        Array.ForEach(r.ToArray(), Sub(e) e.Text = "Remove")

    End Sub

    <Extension()>
    Public Sub Rename_Remove_Button_To_Select(gv As GridView)

        If gv.ID <> "gridview1" Then Return

        Dim r = From gvr As GridViewRow In gv.Rows
                Where gvr.RowType = DataControlRowType.DataRow AndAlso CType(gvr.Cells(1).FindControl("lbUpDnUnit"), Label).Text.Length = 0
                Select CType(gvr.Cells(0).FindControl("btUpgrade"), Button)


        Array.ForEach(r.ToArray(), Sub(e) e.Text = "Select")
    End Sub

End Module



