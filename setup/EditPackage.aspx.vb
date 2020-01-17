Imports System.Data
Imports System.Data.SqlClient

Partial Class setup_EditPackage
    Inherits System.Web.UI.Page
 
    Private ActiveViewIndex() As String = {"MultiViewPackagePackage", "MultiViewPackageReservation", "MultiViewPackageFinancial", "MultiViewPackagePersonnel"}
    Private __PACKAGE As clsPackage


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Me.PackageID = DirectCast(Request.QueryString("PackageID"), String)

        __PACKAGE = New clsPackage()
        If Me.IsPostBack = False Then

            Dim i As Integer = 0
            For i = 1 To 10
                DropDownListNights.Items.Add(New ListItem(i, i))
            Next i

            siRoomType.Connection_String = Resources.Resource.cns
            siRoomType.Label_Caption = ""
            siRoomType.ComboItem = "AccomRoomType"
            siRoomType.Load_Items()
            siUnitType.Connection_String = Resources.Resource.cns
            siUnitType.Label_Caption = ""
            siUnitType.ComboItem = "UnitType"
            siUnitType.Load_Items()
            siType.Connection_String = Resources.Resource.cns
            siType.Label_Caption = ""
            siType.ComboItem = "PackageType"
            siType.Load_Items()
            siSubType.Connection_String = Resources.Resource.cns
            siSubType.Label_Caption = ""
            siSubType.ComboItem = "PackageSubType"
            siSubType.Load_Items()
            Dim oAccom As New clsAccom
            DropDownListAccommodation.Items.Add(New ListItem("", 0))
            DropDownListAccommodation.AppendDataBoundItems = True

            'DropDownListAccommodation.DataSource = oAccom.Get_Accommodations

            Dim ds = oAccom.Get_Accommodations()
            Dim dv = CType(ds.Select(DataSourceSelectArguments.Empty), DataView)

            DropDownListAccommodation.DataSource = dv.ToTable().AsEnumerable().OrderBy(Function(x) x("AccomName").ToString()).CopyToDataTable()            
            DropDownListAccommodation.DataValueField = "AccomID"
            DropDownListAccommodation.DataTextField = "AccomName"
            DropDownListAccommodation.DataBind()
            oAccom = Nothing

            If PackageID <> Nothing Then

                __PACKAGE.PackageID = PackageID
                __PACKAGE.Load()

                Me.LabelPackage.Text = __PACKAGE.Package
                Load_Values(__PACKAGE)

                HiddenFieldPackagePK.Value = __PACKAGE.PackageID
                HiddenFieldPackageName.Value = __PACKAGE.Package

                HiddenFieldPackageName.Value = __PACKAGE.Package.ToUpper()
            Else
                HiddenFieldPackagePK.Value = Request("PackageID")
            End If




        End If
        'Ensure 1st tab selected when arrived at page
        '
        If IsPostBack = False Then
            MultiViewPackage.ActiveViewIndex = 0
        End If

    End Sub



    Private Sub Load_Values(ByVal _PACKAGE As clsPackage)

        TextBoxPackage.Text = _PACKAGE.Package.Trim()
        TextBoxDescription.Text = _PACKAGE.Description.Trim()
        TextBoxExpirationPeriod.Text = _PACKAGE.ExpirationPeriod.ToString()

        TextBoxDefaultCost.Text = Decimal.Round(_PACKAGE.DefaultCost, 2, MidpointRounding.AwayFromZero)
        TextBoxCost.Text = Decimal.Round(_PACKAGE.Cost, 2, MidpointRounding.AwayFromZero)

        TextBoxMininumCharge.Text = Decimal.Round(_PACKAGE.MinimumCharge, 2, MidpointRounding.AwayFromZero)
        TextBoxMaximumCharge.Text = Decimal.Round(_PACKAGE.MaximumCharge, 2, MidpointRounding.AwayFromZero)
        txtInvAmt.Text = Decimal.Round(_PACKAGE.DefaultInvoiceAmt, 2, MidpointRounding.AwayFromZero)
        txtMaxPremAmt.Text = Decimal.Round(_PACKAGE.MaxPremiumAmount, 2, MidpointRounding.AwayFromZero)
        CheckBoxActive.Checked = IIf(_PACKAGE.Active = True, True, False)
        CheckBoxPromptCost.Checked = _PACKAGE.PromptCost
        CheckBoxOptionalLocation.Checked = IIf(_PACKAGE.OptionalLocation = True, True, False)
        DropDownListAccommodation.SelectedValue = _PACKAGE.AccomID
        siUnitType.Selected_ID = _PACKAGE.UnitTypeID
        siRoomType.Selected_ID = _PACKAGE.AccomRoomTypeID
        DropDownListBedRooms.SelectedValue = _PACKAGE.Bedrooms
        DropDownListNights.SelectedValue = _PACKAGE.MinNights
        dteStartDate.Selected_Date = __PACKAGE.StartDate
        dteEndDate.Selected_Date = __PACKAGE.EndDate
        siType.Selected_ID = __PACKAGE.TypeID
        siSubType.Selected_ID = __PACKAGE.SubTypeID
    End Sub

    Private Function Write_Values() As clsPackage

        If IsDBNull(__PACKAGE) Then
            __PACKAGE = New clsPackage()
        End If

        If String.IsNullOrEmpty(HiddenFieldPackagePK.Value) = False Then
            __PACKAGE.PackageID = HiddenFieldPackagePK.Value
        End If

        __PACKAGE.UserID = Session("UserDBID")

        __PACKAGE.Load()

        __PACKAGE.Package = TextBoxPackage.Text.Trim()
        __PACKAGE.Description = TextBoxDescription.Text.Trim()
        __PACKAGE.ExpirationPeriod = TextBoxExpirationPeriod.Text.Trim()


        Dim cost As Decimal
        Dim default_cost As Decimal
        Dim minimum_charge As Decimal
        Dim maximum_charge As Decimal
        Dim inv_amt As Decimal
        Dim prem_amt As Decimal
        If Decimal.TryParse(TextBoxCost.Text.Trim(), cost) Then
            __PACKAGE.Cost = cost
        End If


        If Decimal.TryParse(TextBoxDefaultCost.Text.Trim(), default_cost) Then
            __PACKAGE.DefaultCost = default_cost
        End If


        If Decimal.TryParse(TextBoxMininumCharge.Text.Trim(), minimum_charge) Then
            __PACKAGE.MinimumCharge = minimum_charge
        End If


        If Decimal.TryParse(TextBoxMaximumCharge.Text.Trim(), maximum_charge) Then
            __PACKAGE.MaximumCharge = maximum_charge
        End If

        If Decimal.TryParse(txtInvAmt.Text.Trim(), inv_amt) Then
            __PACKAGE.DefaultInvoiceAmt = inv_amt
        End If

        If Decimal.TryParse(txtMaxPremAmt.Text.Trim(), prem_amt) Then
            __PACKAGE.MaxPremiumAmount = prem_amt
        End If

        __PACKAGE.Active = CheckBoxActive.Checked
        __PACKAGE.OptionalLocation = CheckBoxOptionalLocation.Checked
        __PACKAGE.PromptCost = CheckBoxPromptCost.Checked
        __PACKAGE.StartDate = dteStartDate.Selected_Date
        __PACKAGE.EndDate = dteEndDate.Selected_Date
        __PACKAGE.AccomID = DropDownListAccommodation.SelectedValue
        __PACKAGE.MinNights = DropDownListNights.SelectedValue
        If siUnitType.Selected_ID > 0 Then
            __PACKAGE.UnitTypeID = siUnitType.Selected_ID
            __PACKAGE.Bedrooms = DropDownListBedRooms.SelectedValue
            __PACKAGE.AccomRoomTypeID = 0
        Else
            __PACKAGE.AccomRoomTypeID = siRoomType.Selected_ID
            __PACKAGE.Bedrooms = ""
            __PACKAGE.UnitTypeID = 0
        End If
        __PACKAGE.SubTypeID = siSubType.Selected_ID
        __PACKAGE.TypeID = siType.Selected_ID
        Return __PACKAGE
    End Function

    Protected Sub LinkPackageFinancial_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkPackageFinancial.Click



        If Integer.Parse(Me.PackageID) > 0 Then
            Dim sql As String = _
                String.Format( _
                    "select PackageFinTransCodeID, C.PackageID, FinTransCodeID, A.Amount, UseFormula, Formula, B.Description,  " & _
                    "(select ComboItem from t_comboitems where comboitemid = B.TransCodeID) [Transaction Name], " & _
                    "(select ComboItem from t_comboitems where comboitemid = B.TransTypeID) [Transaction Type], " & _
                    "C.Package " & _
                    "from t_packageFinTransCode a " & _
                    "inner join t_FinTransCodes B " & _
                    "on a.FinTransCodeID = b.FinTransID " & _
                    "inner join t_Package C On C.PackageID = A.PackageID " & _
                    "where A.PackageID = {0}", _
                    Me.PackageID)

            Run_Query(sql, GridViewPackageFinancial, New String() {"PackageFinTransCodeID"})
            MultiViewPackage.ActiveViewIndex = Array.IndexOf(ActiveViewIndex, ActiveViewIndex(2))
        End If
    End Sub


    Protected Sub LinkPackageReservation_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkPackageReservation.Click

        If Me.PackageID <> 0 Then


            Dim sql As String = "SELECT d.Package, cast(round(d.Cost, 2) as numeric(36,2)) [Price], A.PackageReservationID [Reservation#], A.PackageID, B.COMBOITEM Location, C.COMBOITEM Status FROM T_PACKAGERESERVATION A LEFT OUTER JOIN T_COMBOITEMS B " & _
                "ON A.LOCATIONID = B.COMBOITEMID " & _
                "LEFT OUTER JOIN T_COMBOITEMS C ON A.STATUSID = C.COMBOITEMID " & _
                "inner join t_package d on a.packageid = d.packageid WHERE A.PackageID = " & Me.PackageID

            Run_Query(sql, GridViewPackageReservation, New String() {"Reservation#"})
            MultiViewPackage.ActiveViewIndex = Array.IndexOf(ActiveViewIndex, ActiveViewIndex(1))
        End If


    End Sub

    Protected Sub GridViewPackageReservation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridViewPackageReservation.SelectedIndexChanged
        Response.Redirect(String.Format("EditPackageReservation.aspx?PackageReservationId={0}&Package={1}&PackageID={2}&ReservationName={3}", _
                                        Me.GridViewPackageReservation.SelectedValue, Me.PackageName, _
                                        Me.PackageID, _
                                        GridViewPackageReservation.SelectedRow.Cells(5).Text))
    End Sub







    Protected Sub LinkPackagePersonnel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkPackagePersonnel.Click




        If Me.PackageID <> 0 Then

            Dim sql As String = _
                String.Format( _
                "select d.Package, b.FirstName + ' ' + b.LastName [Personnel], c.ComboItem [Title], a.PackageID, PackagePersonnelID, b.PersonnelID from t_packagepersonnel a " & _
                "inner join t_personnel b " & _
                "on a.PersonnelId = b.PersonnelId " & _
                "left outer join t_comboitems c on c.ComboItemId = a.TitleId " & _
                "inner join t_package d on a.PackageId = d.PackageId " & _
                "where a.PackageId = {0} Order By Personnel", Me.PackageID)


            Run_Query(sql, GridViewPackagePersonnel, New String() {"PackagePersonnelID"})
            MultiViewPackage.ActiveViewIndex = Array.IndexOf(ActiveViewIndex, ActiveViewIndex(3))
        End If

    End Sub


    Protected Sub GridViewPackagePersonnel_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridViewPackagePersonnel.SelectedIndexChanged

    End Sub






    Protected Sub LinkButtonPackage_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButtonPackage.Click
        If Me.PackageID > 0 Then
            MultiViewPackage.ActiveViewIndex = Array.IndexOf(ActiveViewIndex, ActiveViewIndex(0))
        End If
    End Sub


    Protected Sub LinkButtonPackageReservationAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButtonPackageReservationAdd.Click

        Response.Redirect( _
            String.Format( _
                "EditPackageReservation.aspx?PackageID={0}&Package={1}", _
                Me.PackageID, Me.PackageName))
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


    Protected Sub ButtonSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSubmit.Click

        If String.IsNullOrEmpty(HiddenFieldPackagePK.Value) = False Then
            __PACKAGE.PackageID = HiddenFieldPackagePK.Value
        End If
        Write_Values()
        Dim success = __PACKAGE.Save()
        'If HiddenFieldPackagePK.Value = 0 Then Response.Redirect("EditPackage.aspx?PackageID=" & __PACKAGE.PackageID)
        Response.Write(__PACKAGE.Err)
        'Response.Redirect("EditPackage.aspx?PackageID=" & __PACKAGE.PackageID)
    End Sub




    Public Property PackagePersonnelID As String
        Get
            Return HiddenFieldPackagePersonnelID.Value
        End Get
        Set(ByVal value As String)
            HiddenFieldPackagePersonnelID.Value = value
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


    Public Property PackageName As String
        Get
            Return HiddenFieldPackageName.Value
        End Get
        Set(ByVal value As String)
            HiddenFieldPackageName.Value = value
        End Set
    End Property

  
 
    Protected Sub LinkButtonPackageFinancialCreate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButtonPackageFinancialCreate.Click

        Response.Redirect(String.Format( _
                          "EditPackageFinTransCode.aspx?PackageID={0}&PkgFinTransID={1}&Type={3}&Path={2}", _
                          Me.PackageID, _
                          0, _
                          Request.Url.PathAndQuery, _
                          "PackageFinTran"))
    End Sub

    Protected Sub LinkPackageWebSources_Click(sender As Object, e As System.EventArgs) Handles LinkPackageWebSources.Click
        If Me.PackageID > 0 Then
            Dim oPkg2WebSource As New clsPackage2WebSource
            gvWebSource.DataSource = oPkg2WebSource.Get_Pkg_WebSources(Me.PackageID)
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvWebSource.DataKeyNames = sKeys
            gvWebSource.DataBind()
            MultiViewPackage.ActiveViewIndex = 4
            oPkg2WebSource = Nothing
        End If
    End Sub

    Protected Sub gvWebSource_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvWebSource.SelectedIndexChanged
        Dim row As GridViewRow = gvWebSource.SelectedRow
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/setup/EditWebSource.aspx?Package2WebSourceID=" & row.Cells(1).Text & "','win01',350,350);", True)

    End Sub

    Protected Sub GridViewPackageFinancial_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles GridViewPackageFinancial.SelectedIndexChanged

    End Sub

    Protected Sub LinkPackageLetters_Click(sender As Object, e As System.EventArgs) Handles LinkPackageLetters.Click
        If Me.PackageID > 0 Then
            Dim oPkg2Letter As New clsPackage2Letter
            gvLetters.DataSource = oPkg2Letter.Get_Package_Letters(Me.PackageID)
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvLetters.DataKeyNames = sKeys
            gvLetters.DataBind()
            MultiViewPackage.ActiveViewIndex = 5
            oPkg2Letter = Nothing
        End If
    End Sub

    Protected Sub gvLetters_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvLetters.SelectedIndexChanged
        Dim row As GridViewRow = gvLetters.SelectedRow
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/setup/EditPkg2Letter.aspx?ID=" & row.Cells(1).Text & "','win01',350,350);", True)
    End Sub

    Protected Sub LinkPackageAddCost_Click(sender As Object, e As System.EventArgs) Handles LinkPackageAddCost.Click
        If Me.PackageID > 0 Then
            Dim oPkgAddCost As New clsPackageAdditionalCost
            gvAddCost.DataSource = oPkgAddCost.List_Add_Costs(Me.PackageID)
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvAddCost.DataKeyNames = sKeys
            gvAddCost.DataBind()
            MultiViewPackage.ActiveViewIndex = 6
            oPkgAddCost = Nothing
        End If
    End Sub

    Protected Sub gvAddCost_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvAddCost.SelectedIndexChanged
        Dim row As GridViewRow = gvAddCost.SelectedRow
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/setup/EditpackageAddCost.aspx?ID=" & row.Cells(1).Text & "','win01',350,350);", True)
    End Sub

    Protected Sub gvAddCost_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then
            If e.Row.RowIndex > -1 Then
                e.Row.Cells(2).Text = CStr(CDate(e.Row.Cells(2).Text).ToShortDateString)
                e.Row.Cells(3).Text = CStr(CDate(e.Row.Cells(3).Text).ToShortDateString)
                e.Row.Cells(4).Text = FormatCurrency(e.Row.Cells(4).Text, 2)
            End If
            e.Row.Cells(1).Visible = False
        End If
    End Sub

    Protected Sub package_copy_btn_Click(sender As Object, e As System.EventArgs) Handles package_copy_btn.Click        

        Dim id_package_url As String = Request("packageid")   
        Dim name_package As String = package_copy_txt.Text.Trim()

        Dim id_package_new = -1
        Dim id_reservation_new = -1
        Dim id_tour_new = -1
        Dim id_financial_new = -1

        Dim id_reservation_old = -1
        Dim id_tour_old = -1
        Dim id_financial_old = -1

        Using cn_package = New SqlConnection(Resources.Resource.cns)
            Using ad_package = New SqlDataAdapter(String.Format("select * from t_package where packageid = {0}", id_package_url), cn_package)

                Dim dt_package = New DataTable()
                ad_package.Fill(dt_package)

                If dt_package.Rows.Count = 1 Then

                    Dim package_newRow As DataRow = dt_package.NewRow()
                    For i = 0 To dt_package.Columns.Count - 1
                        package_newRow(i) = dt_package.Rows(0)(i)
                    Next

                    Dim package_column_arr(dt_package.Columns.Count - 2) As String
                    Dim package_values_arr(dt_package.Columns.Count - 2) As String

                    For i = 1 To dt_package.Columns.Count - 1
                        package_column_arr(i - 1) = String.Format("{0}", dt_package.Columns(i).ColumnName)

                        If i = 1 Then
                            package_values_arr(i - 1) = String.Format("'{0}'", name_package)
                        Else
                            package_values_arr(i - 1) = String.Format("'{0}'", package_newRow(i).ToString())
                        End If
                    Next

                    Using cm_package = New SqlCommand(String.Format("insert into t_package ({0}) values ({1});select scope_identity();", _
                                                                    String.Join(",", package_column_arr), _
                                                                    String.Join(",", package_values_arr)), cn_package)

                        If cn_package.State = ConnectionState.Closed Then cn_package.Open()
                        id_package_new = cm_package.ExecuteScalar()

                    End Using


                    Using cn_reservation = New SqlConnection(Resources.Resource.cns)
                        Using ad_reservation = New SqlDataAdapter(String.Format("select * from t_packagereservation where packageid = {0}", id_package_url), cn_reservation)

                            Dim dt_reservation = New DataTable()
                            ad_reservation.Fill(dt_reservation)

                            For Each dr_reservation As DataRow In dt_reservation.Rows

                                Dim reservation_newRow As DataRow = dt_reservation.NewRow()
                                id_reservation_old = dr_reservation(0).ToString()

                                For i = 0 To dt_reservation.Columns.Count - 1
                                    reservation_newRow(i) = dr_reservation(i)
                                Next

                                Dim reservation_column_arr(dt_reservation.Columns.Count - 2) As String
                                Dim reservation_values_arr(dt_reservation.Columns.Count - 2) As String

                                For i = 1 To dt_reservation.Columns.Count - 1
                                    reservation_column_arr(i - 1) = String.Format("{0}", dt_reservation.Columns(i).ColumnName)

                                    If i = 1 Then
                                        reservation_values_arr(i - 1) = String.Format("'{0}'", id_package_new)
                                    Else
                                        reservation_values_arr(i - 1) = String.Format("'{0}'", reservation_newRow(i).ToString())
                                    End If
                                Next

                                Using cm_reservation = New SqlCommand(String.Format("insert into t_packagereservation ({0}) values ({1});select scope_identity();", _
                                                                  String.Join(",", reservation_column_arr), _
                                                                  String.Join(",", reservation_values_arr)), cn_reservation)

                                    If cn_reservation.State = ConnectionState.Closed Then cn_reservation.Open()
                                    id_reservation_new = cm_reservation.ExecuteScalar()
                                End Using


                                ' reservation tours
                                '
                                '
                                Using cn_tour = New SqlConnection(Resources.Resource.cns)
                                    Using ad_tour = New SqlDataAdapter(String.Format("select * from t_packagetour where packageid = {0} and packagereservationid = {1}", id_package_url, id_reservation_old), cn_tour)

                                        Dim dt_tour = New DataTable()
                                        ad_tour.Fill(dt_tour)

                                        For Each dr_tour As DataRow In dt_tour.Rows

                                            Dim tour_newRow As DataRow = dt_tour.NewRow()
                                            id_tour_old = dr_tour(0).ToString()

                                            For i = 0 To dt_tour.Columns.Count - 1
                                                tour_newRow(i) = dr_tour(i)
                                            Next

                                            Dim tour_column_arr(dt_tour.Columns.Count - 2) As String
                                            Dim tour_values_arr(dt_tour.Columns.Count - 2) As String

                                            For i = 1 To dt_tour.Columns.Count - 1
                                                tour_column_arr(i - 1) = String.Format("{0}", dt_tour.Columns(i).ColumnName)

                                                If i = 1 Then
                                                    tour_values_arr(i - 1) = String.Format("'{0}'", id_package_new)
                                                ElseIf i = 2 Then
                                                    tour_values_arr(i - 1) = String.Format("'{0}'", id_reservation_new)
                                                Else
                                                    tour_values_arr(i - 1) = String.Format("'{0}'", tour_newRow(i).ToString())
                                                End If
                                            Next

                                            Using cm_reservation = New SqlCommand(String.Format("insert into t_packagetour ({0}) values ({1});select scope_identity();", _
                                                                 String.Join(",", tour_column_arr), _
                                                                 String.Join(",", tour_values_arr)), cn_tour)

                                                If cn_tour.State = ConnectionState.Closed Then cn_tour.Open()
                                                id_tour_new = cm_reservation.ExecuteScalar()
                                            End Using


                                            'tour premiums
                                            '
                                            Using cn_premium = New SqlConnection(Resources.Resource.cns)
                                                Using ad_premium = New SqlDataAdapter(String.Format("select * from t_packagetourpremium where packagetourid = {0} and packageid = {1}", id_tour_old, id_package_url), cn_premium)

                                                    Dim dt_premium = New DataTable()

                                                    ad_premium.Fill(dt_premium)

                                                    For Each dr_premium In dt_premium.Rows

                                                        Dim premium_newRow = dt_premium.NewRow()

                                                        For i = 0 To dt_premium.Columns.Count - 1
                                                            premium_newRow(i) = dr_premium(i)
                                                        Next

                                                        Dim premium_column_arr(dt_premium.Columns.Count - 2) As String
                                                        Dim premium_values_arr(dt_premium.Columns.Count - 2) As String

                                                        For i = 1 To dt_premium.Columns.Count - 1
                                                            premium_column_arr(i - 1) = String.Format("{0}", dt_premium.Columns(i).ColumnName)

                                                            If i = 1 Then
                                                                premium_values_arr(i - 1) = String.Format("'{0}'", id_tour_new)
                                                            ElseIf i = 2 Then
                                                                premium_values_arr(i - 1) = String.Format("'{0}'", id_package_new)
                                                            Else
                                                                premium_values_arr(i - 1) = String.Format("'{0}'", premium_newRow(i).ToString())
                                                            End If
                                                        Next

                                                        Using cm_premium = New SqlCommand(String.Format("insert into t_PackageTourPremium ({0}) values ({1});select scope_identity();", _
                                                                                          String.Join(",", premium_column_arr), _
                                                                                          String.Join(",", premium_values_arr)), cn_premium)

                                                            If cn_premium.State = ConnectionState.Closed Then cn_premium.Open()
                                                            cm_premium.ExecuteScalar()
                                                        End Using
                                                    Next
                                                End Using

                                            End Using


                                            'tour personnel
                                            Using cn_personnel = New SqlConnection(Resources.Resource.cns)
                                                Using ad_personnel = New SqlDataAdapter(String.Format("select * from t_PackageTourPersonnel where packageid = {0} and packagetourid = {1}", id_package_url, id_tour_old), cn_personnel)

                                                    Dim dt_personnel = New DataTable()
                                                    ad_personnel.Fill(dt_personnel)

                                                    For Each dr_personnel In dt_personnel.Rows

                                                        Dim personnel_newRow = dt_personnel.NewRow()

                                                        For i = 0 To dt_personnel.Columns.Count - 1
                                                            personnel_newRow(i) = dr_personnel(i)
                                                        Next

                                                        Dim personnel_column_arr(dt_personnel.Columns.Count - 2) As String
                                                        Dim personnel_values_arr(dt_personnel.Columns.Count - 2) As String

                                                        For i = 1 To dt_personnel.Columns.Count - 1
                                                            personnel_column_arr(i - 1) = String.Format("{0}", dt_personnel.Columns(i).ColumnName)

                                                            If i = 1 Then
                                                                personnel_values_arr(i - 1) = String.Format("'{0}'", id_package_new)
                                                            ElseIf i = 2 Then
                                                                personnel_values_arr(i - 1) = String.Format("'{0}'", id_tour_new)
                                                            Else
                                                                personnel_values_arr(i - 1) = String.Format("'{0}'", personnel_newRow(i).ToString())
                                                            End If
                                                        Next

                                                        Using cm_personnel = New SqlCommand(String.Format("insert into t_PackageTourPersonnel ({0}) values ({1});select scope_identity();", _
                                                                                        String.Join(",", personnel_column_arr), _
                                                                                        String.Join(",", personnel_values_arr)), cn_personnel)

                                                            If cn_personnel.State = ConnectionState.Closed Then cn_personnel.Open()
                                                            cm_personnel.ExecuteScalar()
                                                        End Using

                                                    Next

                                                End Using
                                            End Using

                                            ' end of tour personnel
                                            '


                                            'tour financial
                                            Using cn_financial = New SqlConnection(Resources.Resource.cns)
                                                Using ad_financial = New SqlDataAdapter(String.Format("select * from t_PackageTourFinTransCode where packageid = {0} and packagetourid = {1}", id_package_url, id_tour_old), cn_financial)

                                                    Dim dt_financial = New DataTable()
                                                    ad_financial.Fill(dt_financial)

                                                    For Each dr_financial In dt_financial.Rows

                                                        Dim financial_newRow = dt_financial.NewRow()
                                                        id_financial_old = dr_financial(0).ToString()

                                                        For i = 0 To dt_financial.Columns.Count - 1
                                                            financial_newRow(i) = dr_financial(i)
                                                        Next

                                                        Dim financial_column_arr(dt_financial.Columns.Count - 2) As String
                                                        Dim financial_values_arr(dt_financial.Columns.Count - 2) As String


                                                        For i = 1 To dt_financial.Columns.Count - 1
                                                            financial_column_arr(i - 1) = String.Format("{0}", dt_financial.Columns(i).ColumnName)

                                                            If i = 1 Then
                                                                financial_values_arr(i - 1) = String.Format("'{0}'", id_package_new)
                                                            ElseIf i = 2 Then
                                                                financial_values_arr(i - 1) = String.Format("'{0}'", id_tour_new)
                                                            Else
                                                                financial_values_arr(i - 1) = String.Format("'{0}'", financial_newRow(i).ToString())
                                                            End If
                                                        Next

                                                        Using cm_financial = New SqlCommand(String.Format("insert into t_PackageReservationFinTransCode ({0}) values ({1});select scope_identity();", _
                                                                                        String.Join(",", financial_column_arr), _
                                                                                        String.Join(",", financial_values_arr)), cn_financial)

                                                            If cn_financial.State = ConnectionState.Closed Then cn_financial.Open()
                                                            id_financial_new = cm_financial.ExecuteScalar()
                                                        End Using


                                                        'payment
                                                        '
                                                        Using cn_payment = New SqlConnection(Resources.Resource.cns)
                                                            Using ad_payment = New SqlDataAdapter(String.Format("select * from t_PackageTourPayment where packageid = {0} and packagetourfintransid = {1}", id_package_url, id_financial_old), cn_payment)

                                                                Dim dt_payment = New DataTable()
                                                                ad_payment.Fill(dt_payment)

                                                                For Each dr_payment In dt_payment.Rows

                                                                    Dim payment_newRow = dt_payment.NewRow()

                                                                    For i = 0 To dr_payment.Columns.Count - 1
                                                                        payment_newRow(i) = dr_payment(i)
                                                                    Next

                                                                    Dim payment_column_arr(dt_payment.Columns.Count - 2) As String
                                                                    Dim payment_values_arr(dt_payment.Columns.Count - 2) As String

                                                                    For i = 1 To dt_payment.Columns.Count - 1
                                                                        payment_column_arr(i - 1) = String.Format("{0}", dt_payment.Columns(i).ColumnName)

                                                                        If i = 1 Then
                                                                            payment_values_arr(i - 1) = String.Format("'{0}'", id_package_new)
                                                                        ElseIf i = 2 Then
                                                                            payment_values_arr(i - 1) = String.Format("'{0}'", id_financial_new)
                                                                        Else
                                                                            payment_values_arr(i - 1) = String.Format("'{0}'", payment_newRow(i).ToString())
                                                                        End If
                                                                    Next

                                                                    Using cm_payment = New SqlCommand(String.Format("insert into t_PackageTourPayment ({0}) values ({1});select scope_identity();", _
                                                                                                      String.Join(",", payment_column_arr), _
                                                                                                      String.Join(",", payment_values_arr)), cn_payment)

                                                                        If cn_payment.State = ConnectionState.Closed Then cn_payment.Open()
                                                                        cm_payment.ExecuteScalar()
                                                                    End Using

                                                                Next

                                                            End Using

                                                        End Using

                                                    Next

                                                End Using
                                            End Using
                                            '
                                            'end of tour financial


                                        Next

                                    End Using
                                End Using


                                'reservation personnel
                                Using cn_personnel = New SqlConnection(Resources.Resource.cns)
                                    Using ad_personnel = New SqlDataAdapter(String.Format("select * from t_PackageReservationPersonnel where packageid = {0} and packagereservationid = {1}", id_package_url, id_reservation_old), cn_personnel)

                                        Dim dt_personnel = New DataTable()
                                        ad_personnel.Fill(dt_personnel)

                                        For Each dr_personnel In dt_personnel.Rows

                                            Dim personnel_newRow = dt_personnel.NewRow()

                                            For i = 0 To dt_personnel.Columns.Count - 1
                                                personnel_newRow(i) = dr_personnel(i)
                                            Next

                                            Dim personnel_column_arr(dt_personnel.Columns.Count - 2) As String
                                            Dim personnel_values_arr(dt_personnel.Columns.Count - 2) As String

                                            For i = 1 To dt_personnel.Columns.Count - 1
                                                personnel_column_arr(i - 1) = String.Format("{0}", dt_personnel.Columns(i).ColumnName)

                                                If i = 1 Then
                                                    personnel_values_arr(i - 1) = String.Format("'{0}'", id_package_new)
                                                ElseIf i = 2 Then
                                                    personnel_values_arr(i - 1) = String.Format("'{0}'", id_reservation_new)
                                                Else
                                                    personnel_values_arr(i - 1) = String.Format("'{0}'", personnel_newRow(i).ToString())
                                                End If
                                            Next

                                            Using cm_personnel = New SqlCommand(String.Format("insert into t_PackageReservationPersonnel ({0}) values ({1});select scope_identity();", _
                                                                            String.Join(",", personnel_column_arr), _
                                                                            String.Join(",", personnel_values_arr)), cn_personnel)

                                                If cn_personnel.State = ConnectionState.Closed Then cn_personnel.Open()
                                                cm_personnel.ExecuteScalar()
                                            End Using

                                        Next

                                    End Using
                                End Using

                                ' end of reservation personnel
                                '


                                'reservation financial
                                Using cn_financial = New SqlConnection(Resources.Resource.cns)
                                    Using ad_financial = New SqlDataAdapter(String.Format("select * from t_PackageReservationFinTransCode where packageid = {0} and packagereservationid = {1}", id_package_url, id_reservation_old), cn_financial)

                                        Dim dt_financial = New DataTable()
                                        ad_financial.Fill(dt_financial)

                                        For Each dr_financial In dt_financial.Rows

                                            Dim financial_newRow = dt_financial.NewRow()                                            
                                            id_financial_old = dr_financial(0).ToString()

                                            For i = 0 To dt_financial.Columns.Count - 1
                                                financial_newRow(i) = dr_financial(i)
                                            Next

                                            Dim financial_column_arr(dt_financial.Columns.Count - 2) As String
                                            Dim financial_values_arr(dt_financial.Columns.Count - 2) As String


                                            For i = 1 To dt_financial.Columns.Count - 1
                                                financial_column_arr(i - 1) = String.Format("{0}", dt_financial.Columns(i).ColumnName)

                                                If i = 1 Then
                                                    financial_values_arr(i - 1) = String.Format("'{0}'", id_package_new)
                                                ElseIf i = 2 Then
                                                    financial_values_arr(i - 1) = String.Format("'{0}'", id_reservation_new)
                                                Else
                                                    financial_values_arr(i - 1) = String.Format("'{0}'", financial_newRow(i).ToString())
                                                End If
                                            Next

                                            Using cm_financial = New SqlCommand(String.Format("insert into t_PackageReservationFinTransCode ({0}) values ({1});select scope_identity();", _
                                                                            String.Join(",", financial_column_arr), _
                                                                            String.Join(",", financial_values_arr)), cn_financial)

                                                If cn_financial.State = ConnectionState.Closed Then cn_financial.Open()
                                                id_financial_new = cm_financial.ExecuteScalar()
                                            End Using


                                            'payment
                                            '
                                            Using cn_payment = New SqlConnection(Resources.Resource.cns)
                                                Using ad_payment = New SqlDataAdapter(String.Format("select * from t_PackageReservationPayment where packageid = {0} and packagereservationfintransid = {1}", id_package_url, id_financial_old), cn_payment)

                                                    Dim dt_payment = New DataTable()
                                                    ad_payment.Fill(dt_payment)

                                                    For Each dr_payment In dt_payment.Rows

                                                        Dim payment_newRow = dt_payment.NewRow()

                                                        For i = 0 To dt_payment.Columns.Count - 1
                                                            payment_newRow(i) = dr_payment(i)
                                                        Next

                                                        Dim payment_column_arr(dt_payment.Columns.Count - 2) As String
                                                        Dim payment_values_arr(dt_payment.Columns.Count - 2) As String

                                                        For i = 1 To dt_payment.Columns.Count - 1
                                                            payment_column_arr(i - 1) = String.Format("{0}", dt_payment.Columns(i).ColumnName)

                                                            If i = 1 Then
                                                                payment_values_arr(i - 1) = String.Format("'{0}'", id_package_new)
                                                            ElseIf i = 2 Then
                                                                payment_values_arr(i - 1) = String.Format("'{0}'", id_financial_new)
                                                            Else
                                                                payment_values_arr(i - 1) = String.Format("'{0}'", payment_newRow(i).ToString())
                                                            End If
                                                        Next

                                                        Using cm_payment = New SqlCommand(String.Format("insert into t_PackageReservationPayment ({0}) values ({1});select scope_identity();", _
                                                                                          String.Join(",", payment_column_arr), _
                                                                                          String.Join(",", payment_values_arr)), cn_payment)

                                                            If cn_payment.State = ConnectionState.Closed Then cn_payment.Open()
                                                            cm_payment.ExecuteScalar()
                                                        End Using

                                                    Next

                                                End Using

                                            End Using

                                        Next

                                    End Using
                                End Using
                                '
                                'end of reservatiion financial



                            Next


                        End Using
                    End Using




                    'package personnel
                    Using cn_personnel = New SqlConnection(Resources.Resource.cns)
                        Using ad_personnel = New SqlDataAdapter(String.Format("select * from t_packagepersonnel where packageid = {0}", id_package_url), cn_package)

                            Dim dt_personnel = New DataTable()
                            ad_personnel.Fill(dt_personnel)

                            For Each dr_personnel In dt_personnel.Rows

                                Dim personnel_newRow = dt_personnel.NewRow()

                                For i = 0 To dt_personnel.Columns.Count - 1
                                    personnel_newRow(i) = dr_personnel(i)
                                Next

                                Dim personnel_column_arr(dt_personnel.Columns.Count - 2) As String
                                Dim personnel_values_arr(dt_personnel.Columns.Count - 2) As String

                                For i = 1 To dt_personnel.Columns.Count - 1
                                    personnel_column_arr(i - 1) = String.Format("{0}", dt_personnel.Columns(i).ColumnName)

                                    If i = 1 Then
                                        personnel_values_arr(i - 1) = String.Format("'{0}'", id_package_new)
                                    Else
                                        personnel_values_arr(i - 1) = String.Format("'{0}'", personnel_newRow(i).ToString())
                                    End If
                                Next

                                Using cm_personnel = New SqlCommand(String.Format("insert into t_PackagePersonnel ({0}) values ({1});select scope_identity();", _
                                                                String.Join(",", personnel_column_arr), _
                                                                String.Join(",", personnel_values_arr)), cn_personnel)

                                    If cn_personnel.State = ConnectionState.Closed Then cn_personnel.Open()
                                    cm_personnel.ExecuteScalar()
                                End Using

                            Next

                        End Using
                    End Using

                    ' end of personnel
                    '

                    'package financial
                    Using cn_financial = New SqlConnection(Resources.Resource.cns)
                        Using ad_financial = New SqlDataAdapter(String.Format("select * from t_packagefintranscode where packageid = {0}", id_package_url), cn_financial)

                            Dim dt_financial = New DataTable()
                            ad_financial.Fill(dt_financial)

                            For Each dr_financial In dt_financial.Rows

                                Dim financial_newRow = dt_financial.NewRow()
                                id_financial_old = dr_financial(0).ToString()

                                For i = 0 To dt_financial.Columns.Count - 1
                                    financial_newRow(i) = dr_financial(i)
                                Next

                                Dim financial_column_arr(dt_financial.Columns.Count - 2) As String
                                Dim financial_values_arr(dt_financial.Columns.Count - 2) As String


                                For i = 1 To dt_financial.Columns.Count - 1
                                    financial_column_arr(i - 1) = String.Format("{0}", dt_financial.Columns(i).ColumnName)

                                    If i = 1 Then
                                        financial_values_arr(i - 1) = String.Format("'{0}'", id_package_new)
                                    Else
                                        financial_values_arr(i - 1) = String.Format("'{0}'", financial_newRow(i).ToString())
                                    End If
                                Next

                                Using cm_financial = New SqlCommand(String.Format("insert into t_packagefintranscode ({0}) values ({1});select scope_identity();", _
                                                                String.Join(",", financial_column_arr), _
                                                                String.Join(",", financial_values_arr)), cn_financial)

                                    If cn_financial.State = ConnectionState.Closed Then cn_financial.Open()
                                    id_financial_new = cm_financial.ExecuteScalar()
                                End Using


                                'payment
                                '
                                Using cn_payment = New SqlConnection(Resources.Resource.cns)
                                    Using ad_payment = New SqlDataAdapter(String.Format("select * from t_packagepayment where packageid = {0} and packagefintransid = {1}", id_package_url, id_financial_old), cn_payment)

                                        Dim dt_payment = New DataTable()
                                        ad_payment.Fill(dt_payment)

                                        For Each dr_payment In dt_payment.Rows

                                            Dim payment_newRow = dt_payment.NewRow()

                                            For i = 0 To dt_payment.Columns.Count - 1
                                                payment_newRow(i) = dr_payment(i)
                                            Next

                                            Dim payment_column_arr(dt_payment.Columns.Count - 2) As String
                                            Dim payment_values_arr(dt_payment.Columns.Count - 2) As String

                                            For i = 1 To dt_payment.Columns.Count - 1
                                                payment_column_arr(i - 1) = String.Format("{0}", dt_payment.Columns(i).ColumnName)

                                                If i = 1 Then
                                                    payment_values_arr(i - 1) = String.Format("'{0}'", id_package_new)
                                                ElseIf i = 7 Then
                                                    payment_values_arr(i - 1) = id_financial_new
                                                Else
                                                    payment_values_arr(i - 1) = String.Format("'{0}'", payment_newRow(i).ToString())
                                                End If
                                            Next

                                            Using cm_payment = New SqlCommand(String.Format("insert into t_PackagePayment ({0}) values ({1});select scope_identity();", _
                                                                              String.Join(",", payment_column_arr), _
                                                                              String.Join(",", payment_values_arr)), cn_payment)

                                                If cn_payment.State = ConnectionState.Closed Then cn_payment.Open()
                                                cm_payment.ExecuteScalar()
                                            End Using

                                        Next

                                    End Using

                                End Using

                            Next

                        End Using
                    End Using
                    '
                    'end of package financial









                    'package web sources
                    Using cn_websource = New SqlConnection(Resources.Resource.cns)
                        Using ad_websource = New SqlDataAdapter(String.Format("select * from t_Package2WebSource where packageid = {0}", id_package_url), cn_websource)

                            Dim dt_websource = New DataTable()
                            ad_websource.Fill(dt_websource)

                            For Each dr_websource In dt_websource.Rows

                                Dim websource_newRow = dt_websource.NewRow()

                                For i = 0 To dt_websource.Columns.Count - 1
                                    websource_newRow(i) = dr_websource(i)
                                Next

                                Dim websource_column_arr(dt_websource.Columns.Count - 2) As String
                                Dim websource_values_arr(dt_websource.Columns.Count - 2) As String

                                For i = 1 To dt_websource.Columns.Count - 1
                                    websource_column_arr(i - 1) = String.Format("{0}", dt_websource.Columns(i).ColumnName)

                                    If i = 1 Then
                                        websource_values_arr(i - 1) = String.Format("'{0}'", id_package_new)
                                    Else
                                        websource_values_arr(i - 1) = String.Format("'{0}'", websource_newRow(i).ToString())
                                    End If
                                Next

                                Using cm_websource = New SqlCommand(String.Format("insert into t_Package2WebSource ({0}) values ({1});select scope_identity();", _
                                                                String.Join(",", websource_column_arr), _
                                                                String.Join(",", websource_values_arr)), cn_websource)

                                    If cn_websource.State = ConnectionState.Closed Then cn_websource.Open()
                                    cm_websource.ExecuteScalar()
                                End Using

                            Next

                        End Using
                    End Using

                    ' end of web sources

                    'package conf. letters
                    Using cn_package2letter = New SqlConnection(Resources.Resource.cns)
                        Using ad_package2letter = New SqlDataAdapter(String.Format("select * from t_package2letter where packageid = {0}", id_package_url), cn_package2letter)

                            Dim dt_package2letter = New DataTable()
                            ad_package2letter.Fill(dt_package2letter)

                            For Each dr_package2letter In dt_package2letter.Rows

                                Dim package2letter_newRow = dt_package2letter.NewRow()

                                For i = 0 To dt_package2letter.Columns.Count - 1
                                    package2letter_newRow(i) = dr_package2letter(i)
                                Next

                                Dim package2letter_column_arr(dt_package2letter.Columns.Count - 2) As String
                                Dim package2letter_values_arr(dt_package2letter.Columns.Count - 2) As String

                                For i = 1 To dt_package2letter.Columns.Count - 1
                                    package2letter_column_arr(i - 1) = String.Format("{0}", dt_package2letter.Columns(i).ColumnName)

                                    If i = 1 Then
                                        package2letter_values_arr(i - 1) = String.Format("'{0}'", id_package_new)
                                    Else
                                        package2letter_values_arr(i - 1) = String.Format("'{0}'", package2letter_newRow(i).ToString())
                                    End If
                                Next

                                Using cm_package2letter = New SqlCommand(String.Format("insert into t_package2letter ({0}) values ({1});select scope_identity();", _
                                                                String.Join(",", package2letter_column_arr), _
                                                                String.Join(",", package2letter_values_arr)), cn_package2letter)

                                    If cn_package2letter.State = ConnectionState.Closed Then cn_package2letter.Open()
                                    cm_package2letter.ExecuteScalar()
                                End Using

                            Next

                        End Using
                    End Using

                    ' end of conf. letters


                    'package add. costs
                    Using cn_additionalcost = New SqlConnection(Resources.Resource.cns)
                        Using ad_additionalcost = New SqlDataAdapter(String.Format("select * from t_packageadditionalcost where packageid = {0}", id_package_url), cn_package)

                            Dim dt_additionalcost = New DataTable()
                            ad_additionalcost.Fill(dt_additionalcost)

                            For Each dr_additionalcost In dt_additionalcost.Rows

                                Dim additionalcost_newRow = dt_additionalcost.NewRow()

                                For i = 0 To dt_additionalcost.Columns.Count - 1
                                    additionalcost_newRow(i) = dr_additionalcost(i)
                                Next

                                Dim additionalcost_column_arr(dt_additionalcost.Columns.Count - 2) As String
                                Dim additionalcost_values_arr(dt_additionalcost.Columns.Count - 2) As String

                                For i = 1 To dt_additionalcost.Columns.Count - 1
                                    additionalcost_column_arr(i - 1) = String.Format("{0}", dt_additionalcost.Columns(i).ColumnName)

                                    If i = 1 Then
                                        additionalcost_values_arr(i - 1) = String.Format("'{0}'", id_package_new)
                                    Else
                                        additionalcost_values_arr(i - 1) = String.Format("'{0}'", additionalcost_newRow(i).ToString())
                                    End If
                                Next

                                Using cm_additionalcost = New SqlCommand(String.Format("insert into t_packageadditionalcost ({0}) values ({1});select scope_identity();", _
                                                                String.Join(",", additionalcost_column_arr), _
                                                                String.Join(",", additionalcost_values_arr)), cn_additionalcost)

                                    If cn_additionalcost.State = ConnectionState.Closed Then cn_additionalcost.Open()
                                    cm_additionalcost.ExecuteScalar()
                                End Using

                            Next

                        End Using
                    End Using

                    ' end add. costs
                    '                    '


                End If
            End Using
        End Using

    End Sub



    Protected Sub LinkVendors_Click(sender As Object, e As System.EventArgs) Handles LinkVendors.Click
        If Me.PackageID > 0 Then
            Dim oPkgVendor As New clsVendor2Package
            gvVendors.DataSource = oPkgVendor.List_Vendors_By_Package(Me.PackageID)
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvVendors.DataKeyNames = sKeys
            gvVendors.DataBind()
            MultiViewPackage.ActiveViewIndex = 7
            oPkgVendor = Nothing
        End If
    End Sub

    Protected Sub gvVendors_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvVendors.SelectedIndexChanged
        Dim row As GridViewRow = gvVendors.SelectedRow
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/setup/Editpackage2Vendor.aspx?ID=" & row.Cells(1).Text & "','win01',350,350);", True)

    End Sub
End Class
