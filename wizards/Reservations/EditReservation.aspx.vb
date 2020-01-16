Imports System.Reflection
Imports System.Data
Imports System.Linq


Imports clsReservationWizard
Imports System.Web.Script.Serialization

Partial Class wizard_Reservations_Reervation
    Inherits System.Web.UI.Page

    Private package_base As New Base_Package
    Private wiz As New Wizard

#Region "Event Handlers"
    Private Sub wizard_Reservations_Reervation_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim ph = CType(Me.Master.Master.FindControl("ContentPlaceHolder1"), ContentPlaceHolder)
        Dim wiz_data = CType(ph.FindControl("LB_WIZ_DATA"), Label)

        If Session("wizData" + Session("wizGuid")) <> Nothing Then
            wiz = New JavaScriptSerializer().Deserialize(Of Wizard)(Session("wizData" + Session("wizGuid")))
        Else
            wiz = New JavaScriptSerializer().Deserialize(Of Wizard)(wiz_data.Text)
        End If
        Session("wizData" + wiz.GUID_TIMESTAMP) = New JavaScriptSerializer().Serialize(wiz)
        Session("wizGuid") = wiz.GUID_TIMESTAMP

        Dim b = CType(SyncDateField1.FindControl("btnSelect"), Button)
        Dim tc = CType(SyncDateField1.FindControl("txtDate"), TextBox)

        b.Enabled = False

        If wiz.Scenario = EnumScenario.One Or wiz.Scenario = EnumScenario.Ten Then

            b.Enabled = False
            b.Visible = False
            tc.CssClass = "form-control"
            ddNights.Enabled = False
            ddNights.Attributes("disabled") = "disabled"

            If wiz.Reservation.CheckInDate.Length = 0 Then
                Dim package_id = wiz.Packages(0).PackageID
                With New clsPackageReservation
                    .PackageReservationID = .Find_Res_ID(package_id)
                    .Load()
                    wiz.Reservation.ResLocationID = .LocationID
                    wiz.Reservation.ResortCompanyID = .ResortCompanyID
                    wiz.Reservation.SourceID = .SourceID
                    wiz.Reservation.TypeID = .TypeID
                    wiz.Reservation.StatusID = .StatusID
                    wiz.Reservation.NumberAdults = 2
                    wiz.Reservation.NumberChildren = 0
                    wiz.Reservation.LockInventory = False
                    wiz.Reservation.DateBooked = DateTime.Now.ToShortDateString()
                    wiz.Reservation.CheckInDate = wiz.Search_CheckIn_Date
                    wiz.Reservation.CheckOutDate = wiz.Search_CheckOut_date
                End With
            End If

        ElseIf wiz.Scenario = EnumScenario.Three Or wiz.Scenario = EnumScenario.Two Then

            b.Enabled = False
            b.Visible = False
            tc.CssClass = "form-control"
            ddNights.Enabled = False
            ddNights.Attributes("disabled") = "disabled"
        End If


        ddLocation.Enabled = False

        lbErr.Text = ""

        If IsPostBack = False Then


            ddNights.DataSource = Enumerable.Range(1, 21)
            ddNights.DataBind()

            ddAdults.DataSource = Enumerable.Range(1, 20)
            ddAdults.DataBind()

            ddChildren.DataSource = Enumerable.Range(0, 20)
            ddChildren.DataBind()

            ddSource.DataSource = New clsComboItems().Load_ComboItems("ReservationSource")
            ddSource.Items.Add(New ListItem("(Empty)", ""))
            ddSource.AppendDataBoundItems = True
            ddSource.DataTextField = "ComboItem"
            ddSource.DataValueField = "ComboItemID"
            ddSource.DataBind()

            ddLocation.DataSource = New clsComboItems().Load_ComboItems("ReservationLocation")
            ddLocation.Items.Add(New ListItem("(Empty)", ""))
            ddLocation.AppendDataBoundItems = True
            ddLocation.DataTextField = "ComboItem"
            ddLocation.DataValueField = "ComboItemID"
            ddLocation.DataBind()

            Dim ds = New clsComboItems().Load_ComboItems("ReservationStatus")
            Dim dv As DataView = CType(ds.Select(DataSourceSelectArguments.Empty), DataView)
            Dim dt = dv.ToTable()

            If wiz.Scenario = EnumScenario.One Then
                ddStatus.DataSource = dt.AsEnumerable().Where(Function(x) x.Field(Of String)("ComboItem") = "OpenEnded" Or x.Field(Of String)("ComboItem") = "Pending Payment").CopyToDataTable
            ElseIf wiz.Scenario = EnumScenario.Two Then
                ddStatus.DataSource = dt.AsEnumerable().Where(Function(x) x.Field(Of String)("ComboItem") = "Booked" Or x.Field(Of String)("ComboItem") = "OpenEnded" Or x.Field(Of String)("ComboItem") = "Pending Payment").CopyToDataTable
            Else
                ddStatus.DataSource = dt.AsEnumerable().Where(Function(x) x.Field(Of String)("ComboItem") = "Booked" Or x.Field(Of String)("ComboItem") = "Pending Payment" Or x.Field(Of String)("ComboItem") = "Reset").CopyToDataTable
            End If

            ddStatus.Items.Add(New ListItem("(Empty)", ""))
            ddStatus.AppendDataBoundItems = True
            ddStatus.DataTextField = "ComboItem"
            ddStatus.DataValueField = "ComboItemID"
            ddStatus.DataBind()

            ddType.DataSource = New clsComboItems().Load_ComboItems("ReservationType")
            ddType.Items.Add(New ListItem("(Empty)", ""))
            ddType.AppendDataBoundItems = True
            ddType.DataTextField = "ComboItem"
            ddType.DataValueField = "ComboItemID"
            ddType.DataBind()

            ddSubType.DataSource = New clsComboItems().Load_ComboItems("ReservationSubType")
            ddSubType.Items.Add(New ListItem("(Empty)", ""))
            ddSubType.AppendDataBoundItems = True
            ddSubType.DataTextField = "ComboItem"
            ddSubType.DataValueField = "ComboItemID"
            ddSubType.DataBind()

            ddResortCompany.DataSource = New clsComboItems().Load_ComboItems("ResortCompany")
            ddResortCompany.Items.Add(New ListItem("(Empty)", ""))
            ddResortCompany.AppendDataBoundItems = True
            ddResortCompany.DataTextField = "ComboItem"
            ddResortCompany.DataValueField = "ComboItemID"
            ddResortCompany.DataBind()

            Form_Load()
        End If
        txDateBooked.Text = DateTime.Now.ToShortDateString()
        Session("wizData" + wiz.GUID_TIMESTAMP) = New JavaScriptSerializer().Serialize(wiz)
    End Sub
    Protected Sub Unnamed1_Click1(sender As Object, e As EventArgs)
        Calendar1.Visible = Not (Calendar1.Visible)
        If IsDate(txCheckIn.Text) Then
            Calendar1.SelectedDate = CDate(txCheckIn.Text)
        Else
            Calendar1.SelectedDate = Date.Today
        End If
        txCheckIn.Text = Calendar1.SelectedDate
    End Sub
    Protected Sub ddNights_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddNights.SelectedIndexChanged
        If SyncDateField1.Selected_Date <> "" Then
            txCheckOut.Text = CDate(SyncDateField1.Selected_Date).AddDays(ddNights.SelectedValue).ToShortDateString
        End If
    End Sub
    Protected Sub Calendar1_SelectionChanged(sender As Object, e As EventArgs) Handles Calendar1.SelectionChanged
        txCheckIn.Text = Calendar1.SelectedDate.ToShortDateString
        txCheckOut.Text = CDate(txCheckIn.Text).AddDays(ddNights.SelectedValue).ToShortDateString
        Calendar1.Visible = False
    End Sub
    Protected Sub SyncDateField1_Date_Updated() Handles SyncDateField1.Date_Updated
        txCheckIn.Text = SyncDateField1.Selected_Date
        If txCheckIn.Text = "" Then
            txCheckOut.Text = ""
        Else
            txCheckOut.Text = CDate(txCheckIn.Text).AddDays(ddNights.SelectedValue).ToShortDateString
        End If

        Calendar1.Visible = False
    End Sub
    Protected Sub btPrevious_Click(sender As Object, e As EventArgs) Handles btPrevious.Click
        Dim bt = CType(sender, Button)
        bt.Attributes.Add("nav", 0)
        Navigate(CType(sender, Button))
    End Sub

    Protected Sub btNext_Click(sender As Object, e As EventArgs) Handles btNext.Click
        Form_Save()

        Session("wizData" + wiz.GUID_TIMESTAMP) = New JavaScriptSerializer().Serialize(wiz)

        Dim bt = CType(sender, Button)
        bt.Attributes.Add("nav", 1)
        Navigate(bt)
    End Sub
#End Region

#Region "Sub/Functions"
    Private Sub Form_Load()

        ddNights.ClearSelection()
        ddAdults.ClearSelection()
        ddChildren.ClearSelection()

        ddResortCompany.ClearSelection()
        ddSubType.ClearSelection()
        ddType.ClearSelection()
        ddStatus.ClearSelection()
        ddLocation.ClearSelection()
        ddSource.ClearSelection()

        cbLockedInventory.Checked = False

        txReservationID.Text = String.Empty
        txReservationNumber.Text = String.Empty
        txStatusDate.Text = String.Empty
        txDateBooked.Text = String.Empty

        With wiz.Reservation
            txReservationID.Text = .ReservationID
            cbLockedInventory.Checked = .LockInventory

            txReservationNumber.Text = .ReservationNumber

            If .StatusDate.Length > 0 Then
                txStatusDate.Text = DateTime.Parse(.StatusDate).ToShortDateString()
            End If

            If .CheckInDate.Length > 0 And .CheckOutDate.Length > 0 Then
                txCheckIn.Text = DateTime.Parse(.CheckInDate).ToShortDateString()

                If DateTime.Parse(.CheckOutDate).Subtract(DateTime.Parse(.CheckInDate)).Days > 0 Then
                    ddNights.Items.FindByText(DateTime.Parse(.CheckOutDate).Subtract(
                                              DateTime.Parse(.CheckInDate)).Days).Selected = True

                    SyncDateField1.Selected_Date = DateTime.Parse(.CheckInDate).ToShortDateString()
                End If
            End If


            If .DateBooked.Length > 0 Then
                txDateBooked.Text = DateTime.Parse(.DateBooked).ToShortDateString()
            End If


            If .NumberAdults > 0 Then
                ddAdults.Items.FindByText(.NumberAdults).Selected = True
            End If

            ddChildren.Items.FindByText(.NumberChildren).Selected = True

            If .ResortCompanyID > 0 Then
                ddResortCompany.Items.FindByValue(.ResortCompanyID).Selected = True
            End If

            If Not ddType.Items.FindByValue(.TypeID) Is Nothing Then
                ddType.Items.FindByValue(.TypeID).Selected = True
            End If

            If .SubTypeID > 0 Then
                ddSubType.Items.FindByValue(.SubTypeID).Selected = True
            End If

            If Not ddStatus.Items.FindByValue(.StatusID) Is Nothing Then
                ddStatus.Items.FindByValue(.StatusID).Selected = True
            End If

            If .ResLocationID > 0 Then

                If Not ddLocation.Items.FindByValue(.ResLocationID) Is Nothing Then
                    ddLocation.Items.FindByValue(.ResLocationID).Selected = True
                End If

            End If

            If .SourceID > 0 Then

                If Not ddSource.Items.FindByValue(.SourceID) Is Nothing Then
                    ddSource.Items.FindByValue(.SourceID).Selected = True
                End If
            End If

        End With


        txStatusDate.Text = DateTime.Now.ToShortDateString()
        txDateBooked.Text = DateTime.Now.ToShortDateString()

        Select Case wiz.Scenario
            Case EnumScenario.One

                ddStatus.ClearSelection()
                ddStatus.Items.FindByText("OpenEnded").Selected = True

            Case Else
        End Select


    End Sub
    Private Sub Form_Save()

        With wiz.Reservation

            .ReservationNumber = txReservationNumber.Text.ToUpper().Trim()
            '.Accom.ConfirmationNumber = .ReservationNumber.ToUpper().Trim()


            .ResLocationID = ddLocation.SelectedItem.Value
            .CheckInDate = Convert.ToDateTime(txCheckIn.Text).ToShortDateString()
            .CheckOutDate = Convert.ToDateTime(txCheckOut.Text).ToShortDateString()

            .LockInventory = cbLockedInventory.Checked

            If String.IsNullOrEmpty(ddResortCompany.SelectedItem.Value) = False Then
                .ResortCompanyID = ddResortCompany.SelectedItem.Value
            Else
                .ResortCompanyID = 0
            End If

            .NumberAdults = ddAdults.SelectedItem.Value
            .NumberChildren = ddChildren.SelectedItem.Value

            If ddType.SelectedItem.Value.Length > 0 Then
                .TypeID = ddType.SelectedItem.Value
            Else
                .TypeID = 0
            End If

            If String.IsNullOrEmpty(ddSource.SelectedItem.Value) = False Then
                .SourceID = ddSource.SelectedItem.Value
            Else
                .SourceID = 0
            End If

            If String.IsNullOrEmpty(ddSubType.SelectedItem.Value) = False Then
                .SubTypeID = ddSubType.SelectedItem.Value
            Else
                .SubTypeID = 0
            End If


            If wiz.Scenario = EnumScenario.One Then
                .StatusID = ddStatus.Items.FindByText("OpenEnded").Value
            End If


            .StatusDate = DateTime.Now

            If ddStatus.SelectedItem.Text.ToLower() = "booked" Then
                .DateBooked = DateTime.Now
            End If

        End With

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

End Class
