Imports System.Data.SqlClient
Imports System.Data


Partial Class setup_EditPackageReservationTour
    Inherits System.Web.UI.Page




    Public Property PackageID As Integer
        Get
            If String.IsNullOrEmpty(HiddenFieldPackageID.Value) Then
                Return 0
            Else
                Return Integer.Parse(HiddenFieldPackageID.Value)
            End If            
        End Get
        Set(ByVal value As Integer)
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

    Public Property PackageTourID As Integer
        Get

            If String.IsNullOrEmpty(HiddenFieldPackageTourID.Value) Then
                Return 0
            Else
                Return Integer.Parse(HiddenFieldPackageTourID.Value)
            End If            
        End Get
        Set(ByVal value As Integer)
            HiddenFieldPackageTourID.Value = value
        End Set
    End Property

    Public Property PackageReservationID As Integer
        Get
            If String.IsNullOrEmpty(HiddenFieldPackageReservationID.Value) Then
                Return 0
            Else
                Return Integer.Parse(HiddenFieldPackageReservationID.Value)
            End If            
        End Get
        Set(ByVal value As Integer)
            HiddenFieldPackageReservationID.Value = value
        End Set
    End Property

    Public Property Reservation As String
        Get
            Return HiddenFieldReservationName.Value
        End Get
        Set(ByVal value As String)
            HiddenFieldReservationName.Value = value
        End Set
    End Property

    Public Property PackageTourPersonnelID As Integer
        Get
            If String.IsNullOrEmpty(HiddenFieldPackageTourPersonnelID.Value) Then
                Return 0
            Else
                Return Integer.Parse(HiddenFieldPackageTourPersonnelID.Value)
            End If
        End Get
        Set(ByVal value As Integer)
            HiddenFieldPackageTourPersonnelID.Value = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        HiddenFieldPackageID.Value = Request.QueryString("PackageID")
        HiddenFieldPackageName.Value = Request.QueryString("PackageName")
        HiddenFieldPackageTourID.Value = Request.QueryString("PackageTourID")
        HiddenFieldPackageReservationID.Value = Request.QueryString("PackageReservationID")
        HiddenFieldReservationName.Value = Request.QueryString("Reservation")

        HiddenFieldPackageTourPersonnelID.Value = Request.QueryString("PackageTourPersonnelID")

        If String.IsNullOrEmpty(Me.Reservation) = False Then
            HyperLinkReservation.Text = Me.Reservation
            HyperLinkReservation.NavigateUrl = String.Format( _
                "EditPackageReservation.aspx?PackageReservationID={0}&Package={1}&PackageID={2}&ReservationName={3}", _
                Me.PackageReservationID, _
                Me.PackageName, _
                Me.PackageID, _
                Me.Reservation)

        End If

        If IsPostBack = False Then
            Preload_Combo()

            If Me.PackageTourID <> 0 Then
                Load_Values()
            End If

            MultiViewPackageReservationTour.ActiveViewIndex = 0
        End If

    End Sub



    Private Function GetSQLData(ByVal sql As String) As SqlCommand

        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand(sql, cn)

        Try

            cn.Open()

        Catch ex As Exception

        End Try

        Return cm
    End Function


    Private Sub Preload_Combo()

        Dim sql = _
                    "select CampaignID, Name from t_campaign where active = 1 order by Name"


        Dim cm As SqlCommand
        cm = GetSQLData(Sql)

        DropDownListCampaign.DataSource = cm.ExecuteReader(Data.CommandBehavior.CloseConnection)
        DropDownListCampaign.DataTextField = "Name"
        DropDownListCampaign.DataValueField = "CampaignID"

        DropDownListCampaign.DataBind()


        sql = _
           "select ComboItemID, ComboItem from t_combos a " & _
            "inner join t_comboitems b " & _
            "on a.comboid = b.comboid " & _
            "where comboname = 'tourlocation' order by ComboItem "

        cm = GetSQLData(sql)
        DropDownListLocation.DataSource = cm.ExecuteReader(Data.CommandBehavior.CloseConnection)
        DropDownListLocation.DataTextField = "ComboItem"
        DropDownListLocation.DataValueField = "ComboItemID"

        DropDownListLocation.DataBind()

        sql = _
           "select ComboItemID, ComboItem from t_combos a " & _
            "inner join t_comboitems b " & _
            "on a.comboid = b.comboid " & _
            "where comboname = 'tourtype' order by ComboItem "

        cm = GetSQLData(sql)
        DropDownListType.DataSource = cm.ExecuteReader(Data.CommandBehavior.CloseConnection)
        DropDownListType.DataTextField = "ComboItem"
        DropDownListType.DataValueField = "ComboItemID"

        DropDownListType.DataBind()

        sql = _
           "select ComboItemID, ComboItem from t_combos a " & _
            "inner join t_comboitems b " & _
            "on a.comboid = b.comboid " & _
            "where comboname = 'tourstatus' order by ComboItem "

        cm = GetSQLData(sql)
        DropDownListStatus.DataSource = cm.ExecuteReader(Data.CommandBehavior.CloseConnection)
        DropDownListStatus.DataTextField = "ComboItem"
        DropDownListStatus.DataValueField = "ComboItemID"
        DropDownListStatus.DataBind()

        sql = _
            "select ComboItemID, ComboItem from t_combos a " & _
            "inner join t_comboitems b " & _
            "on a.comboid = b.comboid " & _
            "where comboname = 'toursource' order by ComboItem "

        cm = GetSQLData(sql)
        DropDownListSource.DataSource = cm.ExecuteReader(Data.CommandBehavior.CloseConnection)
        DropDownListSource.DataTextField = "ComboItem"
        DropDownListSource.DataValueField = "ComboItemID"
        DropDownListSource.DataBind()




    End Sub

    Protected Sub ButtonSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSubmit.Click

        If Me.PackageReservationID <> 0 Then
            Write_Values()
        End If
    End Sub

    Private Sub Write_Values()

        If Me.PackageReservationID <> 0 Then

            Dim tour = New clsPackageTour()

            If Me.PackageTourID <> 0 Then

                tour.PackageTourID = Me.PackageTourID

            Else
                tour.PackageReservationID = Me.PackageReservationID
            End If

            tour.Load()
            tour.PackageID = Request("PackageID")
            tour.TourLocationID = DropDownListLocation.SelectedValue
            tour.CampaignID = DropDownListCampaign.SelectedValue
            tour.TourTypeID = DropDownListType.SelectedValue
            tour.TourStatusID = DropDownListStatus.SelectedValue
            tour.TourSourceID = DropDownListSource.SelectedValue

            tour.Save()
            Response.Redirect("EditpackageReservationTour.aspx?PackageID=" & Request("PackageID") & "&PackageName=" & Request("PackageName") & "&PackageReservationID=" & Request("PackageReservationID") & "&Reservation=" & Request("Reservation") & "&PackageTourID=" & tour.PackageTourID)
            Response.Write(tour.Err)
        End If
    End Sub

    Private Sub Load_Values()


        If Me.PackageTourID <> 0 Then
            Dim tour = New clsPackageTour()

            tour.PackageTourID = Me.PackageTourID
            tour.Load()

            DropDownListLocation.SelectedValue = tour.TourLocationID
            DropDownListCampaign.SelectedValue = tour.CampaignID
            DropDownListType.SelectedValue = tour.TourTypeID
            DropDownListStatus.SelectedValue = tour.TourStatusID
            DropDownListSource.SelectedValue = tour.TourSourceID

        End If
    End Sub




    Protected Sub LinkButtonPackageReservationTour_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButtonPackageReservationTour.Click
        MultiViewPackageReservationTour.ActiveViewIndex = 0
    End Sub

    Protected Sub LinkPackageReservationFinancial_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkPackageReservationFinancial.Click


        If Me.PackageID > 0 And Me.PackageTourID > 0 Then

            Dim sql = String.Format( _
                "select a.PackageTourFinTransCodeID, a.PackageTourID, a.PackageID, a.FinTransCodeID, A.Amount, UseFormula, Formula, " & _
                "(select ComboItem from t_comboitems where comboitemid = B.TransCodeID) [Transaction Name], B.Description, " & _
                "(select ComboItem from t_comboitems where comboitemid = B.TransTypeID) [Transaction Type] " & _
                "from t_PackageTourFinTransCode a " & _
                "inner join t_FinTransCodes B " & _
                "on a.FinTransCodeID = b.FinTransID " & _
                "where a.packageid = {0} And a.PackageTourID = {1}", _
                Me.PackageID, _
                Me.PackageTourID)

            Run_Query(sql, GridViewPackageTourFinancial, New String() {"PackageTourFinTransCodeID"})
            MultiViewPackageReservationTour.ActiveViewIndex = 1
        End If
    End Sub

    Protected Sub LinkPackageReservationPersonnel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkPackageReservationPersonnel.Click


        If Me.PackageTourID <> 0 Then

            Dim sql = String.Format( _
                "select PackageTourPersonnelID, PackageID, PackageTourID,  a.PersonnelID, C.ComboItem [Title], b.FirstName + ' ' + b.LastName [Personnel] from t_packagetourpersonnel a " & _
                "inner join t_personnel b on a.personnelid = b.personnelid left outer join " & _
                "t_ComboItems C on a.TitleID = C.ComboItemID " & _
                "where PackageID = {0} " & _
                "and PackageTourID = {1}", Me.PackageID, Me.PackageTourID)

            Run_Query(sql, GridViewPackagePersonnel, New String() {"PackageTourPersonnelID"})
            MultiViewPackageReservationTour.ActiveViewIndex = 2
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

    Protected Sub LinkButtonTourFinancial_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButtonTourFinancial.Click

        Response.Redirect(String.Format( _
                          "EditPackageTourFinTrans.aspx?PackageID={0}&PkgTourFinTransID={1}&Type={3}&Path={2}", _
                          Me.PackageID, _
                          0, _
                          Request.Url.PathAndQuery, _
                          "TourFinTran"))
    End Sub

    Protected Sub LinkPackageTourPremiums_Click(sender As Object, e As System.EventArgs) Handles LinkPackageTourPremiums.Click
        If Me.PackageTourID > 0 Then
            Dim oPkgTourPrem As New clsPackageTourPremium
            gvPackageTourPremiums.DataSource = oPkgTourPrem.Get_Package_Tour_Premiums(PackageTourID)
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvPackageTourPremiums.DataKeyNames = sKeys
            gvPackageTourPremiums.DataBind()
            oPkgTourPrem = Nothing
            MultiViewPackageReservationTour.ActiveViewIndex = 3
        End If
    End Sub

    Protected Sub gvPackageTourPremiums_RowDeleting(sender As Object, e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvPackageTourPremiums.RowDeleting

        Dim c_file = New clsPackageTourPremium()
        With c_file
            .PackageTourPremiumID = gvPackageTourPremiums.DataKeys(e.RowIndex).Value
	.UserID = Session("UserDBID")
            .Delete()
        End With
        LinkPackageTourPremiums_Click(Nothing, EventArgs.Empty)
    End Sub

    Protected Sub gvPackageTourPremiums_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvPackageTourPremiums.SelectedIndexChanged
        Dim row As GridViewRow = gvPackageTourPremiums.SelectedRow        

        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/setup/EditPackageTourPremium.aspx?PkgTourPremiumID=" & row.Cells(1).Text & "','win01',350,350);", True)
    End Sub

End Class
