Imports System.Data.SqlClient

Partial Class setup_EditPackageReservation
    Inherits System.Web.UI.Page

    Private __RESERVATION As clsPackageReservation
    Private sql As String = String.Empty

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If String.IsNullOrEmpty(Request.QueryString("PackageID")) = False Then
            Me.PackageID = Request.QueryString("PackageId")
        End If



        If IsPostBack = False Then

            Preload_Combo()
            siSourceID.Connection_String = Resources.Resource.cns
            siSourceID.ComboItem = "ReservationSource"
            siSourceID.Label_Caption = ""
            siSourceID.Load_Items()
            siCompany.Connection_String = Resources.Resource.cns
            siCompany.ComboItem = "ResortCompany"
            siCompany.Label_Caption = ""
            siCompany.Load_Items()
            siType.Connection_String = Resources.Resource.cns
            siType.Comboitem = "ReservationType"
            siType.Label_Caption = ""
            siType.Load_Items()
            MultiViewPackageReservation.ActiveViewIndex = 0

            HiddenfieldReservationName.Value = Request.QueryString("ReservationName")
            HiddenFieldPackageName.Value = DirectCast(Request.QueryString("Package"), String)

            Me.PackageReservationID = DirectCast(Request.QueryString("PackageReservationId"), String)

            Me.PackageReservationPersonnelID = Request.QueryString("PackageReservationPersonnelID")

            HiddenFieldPackageReservationID.Value = Me.PackageReservationID
            HiddenFieldPackageReservationPersonnelID.Value = Me.PackageReservationPersonnelID

            __RESERVATION = New clsPackageReservation()

            HyperLinkPackageID.NavigateUrl = "EditPackage.aspx?PackageID=" & Me.PackageID
            HyperLinkPackageID.Text = Me.PackageName
            LabelPackageName.Text = Me.PackageName

            If Me.PackageReservationID <> Nothing Then

                __RESERVATION.PackageReservationID = Me.PackageReservationID
                __RESERVATION.Load()

 
                LabelPackageName.Text = Me.PackageName

                Load_Values(Me.PackageReservationID)

            End If
        End If
    End Sub


    Protected Sub LinkButtonReservation_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButtonReservation.Click

        MultiViewPackageReservation.ActiveViewIndex = 0
        LabelPackageName.Text = DirectCast(Request.QueryString("Package"), String)
    End Sub

    Protected Sub LinkButtonPersonnel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButtonPersonnel.Click



        If String.IsNullOrEmpty(Me.PackageReservationID) = False Then

            If Me.PackageReservationID <> 0 Then

                sql = _
                    String.Format("select c.FirstName + ' ' + c.LastName [Personnel], d.ComboItem [Title],  a.PackageReservationPersonnelID [PackageReservationPersonnelID], " & _
                    "A.PackageID, A.PersonnelID from t_packagereservationpersonnel a  " & _
                    "inner join t_packagereservation b on a.packagereservationid = " & _
                    "b.packagereservationid " & _
                    "left join t_personnel c " & _
                    "on c.personnelid = a.personnelid " & _
                    "LEFT JOIN t_comboitems d on d.ComboItemId = a.TitleID " & _
                    "where a.PackageReservationId = {0} and a.PackageID = {1} ORDER BY Personnel", Me.PackageReservationID, Me.PackageID)

                Run_Query(sql, GridViewReservationPersonnel, New String() {"PackageReservationPersonnelID"})
                MultiViewPackageReservation.ActiveViewIndex = 2
            End If

        End If

    End Sub

    Protected Sub LinkButtonTour_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButtonTour.Click


        If String.IsNullOrEmpty(Me.PackageReservationID) = False Then
            If Me.PackageReservationID <> 0 Then

                sql = String.Format("select a.PackageTourID, a.PackageID, a.PackageReservationID, (select ComboItem from t_comboItems " & _
                        "where comboitemid = a.TourLocationID) [Location], " & _
                        "(select ComboItem from t_ComboItems Where ComboItemID = a.TourStatusID) [Status] from t_PackageTour a " & _
                        "where a.PackageReservationID = {0}", Me.PackageReservationID)

                Run_Query(sql, GridViewReservationTour, New String() {"PackageTourID"})
                MultiViewPackageReservation.ActiveViewIndex = 3
            End If

        End If
    End Sub

    Protected Sub LinkButtonFinancial_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButtonFinancial.Click


        If String.IsNullOrEmpty(Me.PackageReservationID) = False Then

            If Integer.Parse(Me.PackageReservationID) > 0 Then

                sql = String.Format( _
                    "select a.PackageReservationFinTransCodeID, a.PackageReservationID, a.PackageID, a.FinTransCodeID, A.Amount, UseFormula, Formula, " & _
                    "(select ComboItem from t_comboitems where comboitemid = B.TransCodeID) [Transaction Name], B.Description, " & _
                    "(select ComboItem from t_comboitems where comboitemid = B.TransTypeID) [Transaction Type] " & _
                    "from t_PackageReservationFinTransCode a " & _
                    "inner join t_FinTransCodes B " & _
                    "on a.FinTransCodeID = b.FinTransID " & _
                    "where a.packageid = {0} And a.PackageReservationID = {1}", _
                    Me.PackageID, _
                    Me.PackageReservationID)

                Run_Query(sql, GridViewReservationFinancial, New String() {"PackageReservationFinTransCodeID"})
                MultiViewPackageReservation.ActiveViewIndex = 1
            End If
        End If
    End Sub




    Sub Run_Query(ByVal sSQL As String, ByVal gv As GridView, ByVal keynames As String())
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand(sSQL, cn)
        Dim dr As SqlDataReader

        Try
            cn.Open()

            dr = cm.ExecuteReader(Data.CommandBehavior.CloseConnection)
            gv.DataSource = dr

            gv.DataKeyNames = keynames
            gv.DataBind()


        Catch ex As Exception

            'p_error.InnerText = ex.ToString()

        Finally
            If cn.State <> Data.ConnectionState.Closed Then cn.Close()
            cn = Nothing
            cm = Nothing
            dr = Nothing
        End Try
    End Sub

    Private Function GetComboItem(ByVal sql As String) As SqlCommand
        Dim cn As New SqlConnection(Resources.Resource.cns)

        Try
            cn.Open()
            Return New SqlCommand(sql, cn)
        Catch ex As Exception

        End Try

        Return Nothing
    End Function


    Private Sub Preload_Combo()
        Dim sql = _
                        "select ComboItemID, ComboItem [Reservation Status] from t_comboitems where ComboID = 321 and Active = 1 " & _
                        "order by ComboItem"

        DropDownListStatus.DataSource = GetComboItem(sql).ExecuteReader(Data.CommandBehavior.CloseConnection)
        DropDownListStatus.DataTextField = "Reservation Status"
        DropDownListStatus.DataValueField = "ComboItemID"

        DropDownListStatus.DataBind()

        If Request("PackageReservationId") = "" Then
            sql = _
                "select ComboItemID, ComboItem [Reservation Location] from t_comboitems where ComboID = 318 and Active = 1 " & _
                "order by ComboItem"
        Else
            sql = _
                "select ComboItemID, ComboItem [Reservation Location] from t_comboitems where ComboID = 318 and Active = 1 or comboitemid = (Select LocationID from t_packagereservation where packagereservationid = " & Request("PackageReservationId") & ") " & _
                "order by ComboItem"

        End If

        DropDownListLocation.DataSource = GetComboItem(sql).ExecuteReader(Data.CommandBehavior.CloseConnection)
        DropDownListLocation.DataTextField = "Reservation Location"
        DropDownListLocation.DataValueField = "ComboItemID"

        DropDownListLocation.DataBind()


        DropDownListPromoNights.DataSource = Linq.Enumerable.Range(0, 15)
        DropDownListPromoNights.DataBind()
    End Sub
  
    Private Sub Load_Values(ByVal package_reservation_id As String)

        If String.IsNullOrEmpty(package_reservation_id) = False Then

            __RESERVATION = New clsPackageReservation()
            __RESERVATION.PackageReservationID = package_reservation_id
            __RESERVATION.Load()

            DropDownListLocation.SelectedValue = __RESERVATION.LocationID
            DropDownListStatus.SelectedValue = __RESERVATION.StatusID
            DropDownListPromoNights.SelectedValue = __RESERVATION.PromoNights
            siSourceID.Selected_ID = __RESERVATION.SourceID
            siCompany.Selected_ID = __RESERVATION.ResortCompanyID
            siType.Selected_ID = __RESERVATION.TypeID
            TextBoxPromoRate.Text = Decimal.Round(__RESERVATION.PromoRate, 2, MidpointRounding.AwayFromZero)
            CheckBoxTourRequired.Checked = __RESERVATION.CreateTour
            cbExtraNight.Checked = __RESERVATION.AllowExtraNight
        End If

    End Sub

    Private Sub Write_Values()

        If String.IsNullOrEmpty(Me.PackageReservationID) = False Then

            __RESERVATION = New clsPackageReservation()
            __RESERVATION.PackageReservationID = Me.PackageReservationID
            __RESERVATION.Load()
            __RESERVATION.UserID = Session("UserDBID")
            __RESERVATION.PackageID = Me.PackageID
            __RESERVATION.LocationID = DropDownListLocation.SelectedValue
            __RESERVATION.StatusID = DropDownListStatus.SelectedValue
            __RESERVATION.PromoNights = DropDownListPromoNights.Text
            __RESERVATION.SourceID = siSourceID.Selected_ID
            __RESERVATION.ResortCompanyID = siCompany.Selected_ID
            __RESERVATION.TypeID = siType.Selected_ID
            __RESERVATION.AllowExtraNight = cbExtraNight.Checked
            Dim promo_rate As Decimal
            If Decimal.TryParse(TextBoxPromoRate.Text, promo_rate) Then
                __RESERVATION.PromoRate = promo_rate
            End If

            __RESERVATION.CreateTour = CheckBoxTourRequired.Checked
            __RESERVATION.Save()

            If String.IsNullOrEmpty(__RESERVATION.Err) = False Then
                Response.Write(__RESERVATION.Err)
            End If

        Else

            __RESERVATION = New clsPackageReservation()
            __RESERVATION.Load()
            __RESERVATION.UserID = Session("UserDBID")
            __RESERVATION.PackageID = Me.PackageID
            __RESERVATION.LocationID = DropDownListLocation.SelectedValue
            __RESERVATION.StatusID = DropDownListStatus.SelectedValue
            __RESERVATION.PromoNights = DropDownListPromoNights.Text
            __RESERVATION.CreateTour = CheckBoxTourRequired.Checked
            __RESERVATION.SourceID = siSourceID.Selected_ID
            __RESERVATION.ResortCompanyID = siCompany.Selected_ID
            __RESERVATION.TypeID = siType.Selected_ID
            __RESERVATION.AllowExtraNight = cbExtraNight.Checked
            Dim promo_rate As Decimal
            If Decimal.TryParse(TextBoxPromoRate.Text, promo_rate) Then
                __RESERVATION.PromoRate = promo_rate
            End If

            __RESERVATION.Save()
        End If
        Response.Redirect("EditPackageReservation.aspx?PackageReservationID=" & __RESERVATION.PackageReservationID & "&Package=" & Request("Package") & "&PackageID=" & Request("PackageID") & "&ReservationName=" & Request("ReservationName"))
    End Sub

    Protected Sub ButtonSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSubmit.Click

        Write_Values()        
    End Sub

    Protected Sub GridViewReservationFinancial_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridViewReservationFinancial.SelectedIndexChanged

    End Sub


    Protected Sub GridViewReservationTour_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridViewReservationTour.SelectedIndexChanged
        
        HiddenFieldPackageTourID.Value = GridViewReservationTour.SelectedDataKey.Value.ToString()
        Response.Redirect( _
            String.Format( _
                "EditPackageReservationTour.aspx?PackageID={0}&PackageName={1}&PackageReservationID={2}&Reservation={3}&PackageTourID={4}", _
                Me.PackageID, _
                Me.PackageName, _
                Me.PackageReservationID, _
                Me.ReservationName, _
                Me.PackageTourID))

    End Sub






    Public Property PackageReservationID As String
        Get
            Return HiddenFieldPackageReservationID.Value
        End Get
        Set(ByVal value As String)
            HiddenFieldPackageReservationID.Value = value
        End Set
    End Property



    Private _packageReservationFinTransCodeID As String
    Public Property PackageReservationFinTransCodeID As String
        Get
            Return _packageReservationFinTransCodeID
        End Get
        Set(ByVal value As String)
            _packageReservationFinTransCodeID = value
        End Set
    End Property


    Public Property PackageReservationPersonnelID As String
        Get
            Return HiddenFieldPackageReservationPersonnelID.Value
        End Get
        Set(ByVal value As String)
            HiddenFieldPackageReservationPersonnelID.Value = value
        End Set
    End Property


    Public Property PackageTourID As String
        Get
            Return HiddenFieldPackageTourID.Value
        End Get
        Set(ByVal value As String)
            HiddenFieldPackageTourID.Value = value
        End Set
    End Property





    Private _reservationID As String
    Public Property ReservationID As String
        Get
            Return _reservationID
        End Get
        Set(ByVal value As String)
            _reservationID = value
        End Set
    End Property


    Private _finTransID As String
    Public Property FinTransID As String
        Get
            Return _finTransID
        End Get
        Set(ByVal value As String)
            _finTransID = value
        End Set
    End Property



    Public Property PackageID As String
        Get
            Return HiddenFieldPackageID.Value
        End Get
        Set(ByVal value As String)
            HiddenFieldPackageID.Value = value
        End Set
    End Property

    Private _personnelID As String
    Public Property PersonnelID As String
        Get
            Return _personnelID
        End Get
        Set(ByVal value As String)
            _personnelID = value
        End Set
    End Property




    Public Property PackageName As String
        Get
            Return HiddenFieldPackageName.Value
        End Get
        Set(ByVal value As String)
            HiddenFieldPackageName.Value = value
        End Set
    End Property


    Public Property ReservationName As String
        Get
            Return HiddenfieldReservationName.Value
        End Get
        Set(ByVal value As String)
            HiddenfieldReservationName.Value = value
        End Set
    End Property



    Protected Sub LinkButtonTourAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButtonTourAdd.Click

        If Me.PackageReservationID <> 0 Then

            Response.Redirect( _
                String.Format( _
                    "EditPackageReservationTour.aspx?PackageID={0}&PackageName={1}&PackageReservationID={2}&Reservation={3}", _
                    Me.PackageID, _
                    Me.PackageName, _
                    Me.PackageReservationID, _
                    Me.ReservationName))
        End If

    End Sub

    Protected Sub LinkButtonReservationFinancialCreate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButtonReservationFinancialCreate.Click
        Response.Redirect(String.Format( _
                                  "EditPackageResFinTrans.aspx?PkgResFinTransID=0&PackageID={0}&PackageReservationID={1}&Path={3}&Type={2}", _
                                  Me.PackageID, _
                                  Me.PackageReservationID, _
                                  "ReservationFinTran", _
                                  ""))
    End Sub

    Protected Sub LinkButtonCheckInDays_Click(sender As Object, e As EventArgs) Handles LinkButtonCheckInDays.Click
        If String.IsNullOrEmpty(Me.PackageReservationID) = False Then

            If Integer.Parse(Me.PackageReservationID) > 0 Then
                Dim opkgRes As New clsPackageReservation
                opkgRes.PackageReservationID = Me.PackageReservationID
                opkgRes.Load()
                If opkgRes.AllowExtraNight Then
                    Dim opkgResNights As New clsPackageReservation2CheckInDay
                    gvDays.DataSource = opkgResNights.List_CheckIn_Days(Me.PackageReservationID)
                    Dim sKeys(0) As String
                    sKeys(0) = "ID"
                    gvDays.DataKeyNames = sKeys
                    gvDays.DataBind()
                    MultiViewPackageReservation.ActiveViewIndex = 4
                End If
                opkgRes = Nothing
            End If
        End If
    End Sub

    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/setup/PackageReservationCheckInDay.aspx?ID=" & Me.PackageReservationID & "','win01',350,350);", True)
    End Sub

    Protected Sub gvDays_RowDeleting(sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvDays.RowDeleting
        Dim opkgResNights As New clsPackageReservation2CheckInDay
        opkgResNights.Delete_Record(gvDays.Rows(e.RowIndex).Cells(1).Text)
        gvDays.DataSource = opkgResNights.List_CheckIn_Days(Me.PackageReservationID)
        Dim sKeys(0) As String
        sKeys(0) = "ID"
        gvDays.DataKeyNames = sKeys
        gvDays.DataBind()
        opkgResNights = Nothing
    End Sub
End Class
