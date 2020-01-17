Imports System.Data.SqlClient

Partial Class setup_PackageFinancial
    Inherits System.Web.UI.Page




    Public Property PackageID As Integer
        Get
            Return IIf(String.IsNullOrEmpty(HiddenFieldPackageID.Value), 0, Integer.Parse(HiddenFieldPackageID.Value))
        End Get
        Set(ByVal value As Integer)
            HiddenFieldPackageID.Value = value
        End Set
    End Property

    Public Property PackageTourID As Integer
        Get
            Return IIf(String.IsNullOrEmpty(HiddenFieldPackageTourID.Value), 0, Integer.Parse(HiddenFieldPackageTourID.Value))
        End Get
        Set(ByVal value As Integer)
            HiddenFieldPackageTourID.Value = value
        End Set
    End Property

    Public Property PackageReservationID As Integer
        Get
            Return IIf(String.IsNullOrEmpty(HiddenFieldPackageReservationID.Value), 0, Integer.Parse(HiddenFieldPackageReservationID.Value))
        End Get
        Set(ByVal value As Integer)
            HiddenFieldPackageReservationID.Value = value
        End Set
    End Property

    Public Property PackageFinTransCodeID As Integer
        Get
            Return IIf(String.IsNullOrEmpty(HiddenFieldPackageFinTransCodeID.Value), 0, Integer.Parse(HiddenFieldPackageFinTransCodeID.Value))
        End Get
        Set(ByVal value As Integer)
            HiddenFieldPackageFinTransCodeID.Value = value
        End Set
    End Property

    Public Property PackageReservationFinTransCodeID As Integer
        Get
            Return IIf(String.IsNullOrEmpty(HiddenFieldPackageReservationFinTransCodeID.Value), 0, Integer.Parse(HiddenFieldPackageReservationFinTransCodeID.Value))
        End Get
        Set(ByVal value As Integer)
            HiddenFieldPackageReservationFinTransCodeID.Value = value
        End Set
    End Property

    Public Property PackageTourFinTransCodeID As Integer
        Get
            Return IIf(String.IsNullOrEmpty(HiddenFieldPackageTourFinTransCodeID.Value), 0, Integer.Parse(HiddenFieldPackageTourFinTransCodeID.Value))
        End Get
        Set(ByVal value As Integer)
            HiddenFieldPackageTourFinTransCodeID.Value = value
        End Set
    End Property

    Public Property Back As String
        Get
            Return HiddenFieldBack.Value
        End Get
        Set(ByVal value As String)
            HiddenFieldBack.Value = value
        End Set
    End Property

    Public Property Type As String
        Get
            Return HiddenFieldType.Value
        End Get
        Set(ByVal value As String)
            HiddenFieldType.Value = value
        End Set
    End Property

    Public Property PaymentType As String
        Get
            Return HiddenFieldPaymentType.Value
        End Get
        Set(ByVal value As String)
            HiddenFieldPaymentType.Value = value
        End Set
    End Property




    '
    '================================================================================================================================================
    '
 


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load



        If Me.IsPostBack = False Then

            Me.PackageID = Request.QueryString("PackageID")
            Me.Type = Request.QueryString("Type")

           
            If Not Request.UrlReferrer Is Nothing Then
                Me.Back = Request.UrlReferrer.PathAndQuery
            End If


            If String.IsNullOrEmpty(Me.Back) Then
                Me.Back = "/CRMSNET"
            End If


            MultiViewPackageFinancialCode.ActiveViewIndex = 0


            If Me.PackageID > 0 Then

                Me.HyperLinkParentLink.NavigateUrl = Me.Back
                Me.HyperLinkParentLink.Text = "Back"

                If Me.Type.Equals("PackageFinTran") Then

                    Preload_Combo("PACKAGETRANS")

                    Me.PackageFinTransCodeID = Request.QueryString("PackageFinTransCodeID")

                    Dim package = New clsPackageFinTransCode()
                    Load_Values(package)

                    Me.PaymentType = "PackagePayment"


                ElseIf Me.Type.Equals("ReservationFinTran") Then

                    Me.PackageReservationID = Request.QueryString("PackageReservationID")
                    Me.PackageReservationFinTransCodeID = Request.QueryString("PackageReservationFinTransCodeID")

                    Preload_Combo("RESERVATIONTRANS")

                    Dim package = New clsPackageReservationFinTransCode()
                    Load_Values(package)

                    Me.PaymentType = "PackageReservationPayment"

                ElseIf Me.Type.Equals("TourFinTran") Then

                    Me.PackageTourID = Request.QueryString("PackageTourID")
                    Me.PackageTourFinTransCodeID = Request.QueryString("PackageTourFinTransCodeID")

                    Preload_Combo("TOURTRANS")

                    Dim package = New clsPackageTourFinTransCode()
                    Load_Values(package)

                    Me.PaymentType = "PackageTourPayment"
                End If


            End If
        End If

    End Sub

    Protected Sub LinkButtonPayment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButtonPayment.Click        

        Dim sql = String.Empty

        If Me.PaymentType.Equals("PackagePayment") Then

            sql = String.Format( _
                        "select A.PackagePaymentID, PackageID, Amount, PaymentMethodID, " & _
                        "(Select ComboItem from t_ComboItems Where ComboItemID = A.PaymentMethodID) [Method] from T_PackagePayment A " & _
                        "where PackageID = {0}", Me.PackageID)

            Run_Query(sql, GridViewPayment, New String() {"PackagePaymentID"})

            Me.MultiViewPackageFinancialCode.ActiveViewIndex = 1

        ElseIf Me.PaymentType.Equals("PackageTourPayment") Then
            If Request("PackageTourFinTransCodeID") > 0 Then
                sql = String.Format( _
                    "select A.PackageTourPaymentID, PackageID, Amount, PackageTourFinTransID, PaymentMethodID, " & _
                    "(Select ComboItem from t_ComboItems Where ComboItemID = A.PaymentMethodID) [Method] from T_PackageTourPayment A " & _
                    "where PackageID = {0} and PackageTourFinTransID = {1}", Me.PackageID, Me.PackageTourFinTransCodeID)

                Run_Query(sql, GridViewViewTourPayment, New String() {"PackagePaymentID"})

                Me.MultiViewPackageFinancialCode.ActiveViewIndex = 2
            End If
        ElseIf Me.PaymentType.Equals("PackageReservationPayment") Then
            If Request("PackageReservationFinTransCodeID") > 0 Then

                sql = String.Format( _
                            "select A.PackageReservationPaymentID, PackageReservationFinTransID, PackageID, Amount, PaymentMethodID, " & _
                            "(Select ComboItem from t_ComboItems Where ComboItemID = A.PaymentMethodID) [Method] from T_PackageReservationPayment A " & _
                            "where PackageID = {0} and PackageReservationFinTransID = {1}", Me.PackageID, Me.PackageReservationFinTransCodeID)

                Run_Query(sql, GridViewViewReservationPayment, New String() {"PackageReservationPaymentID"})
                Me.MultiViewPackageFinancialCode.ActiveViewIndex = 3
            End If
        End If


    End Sub

    Protected Sub LinkButtonFinancial_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButtonFinancial.Click
        MultiViewPackageFinancialCode.ActiveViewIndex = 0
    End Sub

    Protected Sub ButtonSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSave.Click

        If Me.Type.Equals("PackageFinTran") Then

            Dim package = New clsPackageFinTransCode()
            Write_Values(package)

        ElseIf Me.Type.Equals("ReservationFinTran") Then

            Dim package = New clsPackageReservationFinTransCode()
            Write_Values(package)

        ElseIf Me.Type.Equals("TourFinTran") Then

            Dim package = New clsPackageTourFinTransCode()
            Write_Values(package)
        End If

    End Sub








    '------------------------------------------------------------------------------------------------------------------------------------------------
    '------------------------------------------------------------------------------------------------------------------------------------------------
    '




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
            "select a.FinTransID, a.TransCodeID, (select ComboItem from t_ComboItems " & _
            "where ComboItemID = a.TransCodeID) [Financial] from t_fintranscodes a " & _
            "order by Financial "

        Dim cm As SqlCommand
        cm = GetSQLData(sql)

        DropDownListFinTransCodeID.DataSource = cm.ExecuteReader(Data.CommandBehavior.CloseConnection)
        DropDownListFinTransCodeID.DataTextField = "Financial"
        DropDownListFinTransCodeID.DataValueField = "FinTransID"
        DropDownListFinTransCodeID.DataBind()
    End Sub


    Private Sub Preload_Combo(ByVal transType As String)

        DropDownListFinTransCodeID.Items.Clear()

        Dim sql = String.Format( _
            "SELECT (SELECT COMBOITEM FROM T_COMBOITEMS A WHERE " & _
            "A.COMBOITEMID = C.TRANSCODEID) [CODENAME], C.FINTRANSID [CODEID], * FROM T_COMBOS A " & _
            "INNER JOIN T_COMBOITEMS B " & _
            "ON A.COMBOID = B.COMBOID " & _
            "INNER JOIN T_FINTRANSCODES C " & _
            "ON C.TRANSTYPEID = B.COMBOITEMID " & _
            "WHERE A.COMBONAME = 'TransCodeType' and B.COMBOITEM = '{0}' " & _
            "ORDER BY [CODENAME]", transType)

        Dim cm = GetSQLData(sql)
        DropDownListFinTransCodeID.DataSource = cm.ExecuteReader(Data.CommandBehavior.CloseConnection)
        DropDownListFinTransCodeID.DataTextField = "CODENAME"
        DropDownListFinTransCodeID.DataValueField = "CODEID"
        DropDownListFinTransCodeID.DataBind()
    End Sub


    'Package
    Private Sub Load_Values(ByVal package As clsPackageFinTransCode)


        If Me.PackageFinTransCodeID > 0 Then

            package.PackageFinTransCodeID = Me.PackageFinTransCodeID
            package.Load()

            Show_Values(package.FinTransCodeID, _
                        package.UseFormula, _
                        package.Formula, _
                        package.Amount)
        End If
    End Sub


    'Reservation
    Private Sub Load_Values(ByVal package As clsPackageReservationFinTransCode)

        If Me.PackageReservationFinTransCodeID > 0 Then

            package.PackageReservationFinTransCodeID = Me.PackageReservationFinTransCodeID
            package.Load()

            Show_Values(package.FinTransID, _
                        package.UseFormula, _
                        package.Formula, _
                        package.Amount)
        End If
    End Sub

    'Tour
    Private Sub Load_Values(ByVal package As clsPackageTourFinTransCode)

        If Me.PackageTourFinTransCodeID > 0 Then

            package.PackageTourFinTransCodeID = Me.PackageTourFinTransCodeID
            package.Load()

            Show_Values(package.FinTransID, _
                         package.UseFormula, _
                         package.Formula, _
                         package.Amount)
        End If
    End Sub

    Private Sub Show_Values(ByVal finTransCodeId As Integer, _
                            ByVal useFormula As Boolean, _
                            ByVal formula As String, _
                            ByVal amount As Decimal)


        Dim code = New clsFinancialTransactionCodes()
        code.FinTransID = finTransCodeId
        code.Load()

        DropDownListFinTransCodeID.SelectedValue = code.FinTransID
        CheckBoxUseFormula.Checked = useFormula
        TextBoxFormula.Text = formula
        TextBoxAmount.Text = amount
    End Sub


    'Package
    Private Sub Write_Values(ByVal package As clsPackageFinTransCode)

        If Me.PackageFinTransCodeID > 0 Then
            package.PackageFinTransCodeID = Me.PackageFinTransCodeID
            package.Load()
        Else

            package.PackageID = Me.PackageID
        End If

        package.FinTransCodeID = DropDownListFinTransCodeID.SelectedValue
        package.UseFormula = CheckBoxUseFormula.Checked
        package.Formula = TextBoxFormula.Text.Trim()

        Dim amt As Decimal = 0

        If String.IsNullOrEmpty(TextBoxAmount.Text) Then
            package.Amount = 0
        Else

            If Decimal.TryParse(TextBoxAmount.Text, amt) Then
                package.Amount = amt
            End If
        End If

        package.Save()
    End Sub

    'Reservation
    Private Sub Write_Values(ByVal package As clsPackageReservationFinTransCode)

        If Me.PackageReservationFinTransCodeID > 0 Then

            package.PackageReservationFinTransCodeID = Me.PackageReservationFinTransCodeID
            package.Load()
        Else
            package.PackageID = Me.PackageID
            package.PackageReservationID = Me.PackageReservationID

        End If

        package.FinTransID = DropDownListFinTransCodeID.SelectedValue
        package.UseFormula = CheckBoxUseFormula.Checked
        package.Formula = TextBoxFormula.Text.Trim()
        Dim amt As Decimal = 0

        If String.IsNullOrEmpty(TextBoxAmount.Text) Then
            package.Amount = 0
        Else

            If Decimal.TryParse(TextBoxAmount.Text, amt) Then
                package.Amount = amt
            End If
        End If

        package.Save()
    End Sub

    'Tour
    Private Sub Write_Values(ByVal package As clsPackageTourFinTransCode)

        If Me.PackageTourFinTransCodeID > 0 Then

            package.PackageTourFinTransCodeID = Me.PackageTourFinTransCodeID
            package.Load()
        Else
            package.PackageID = Me.PackageID
            package.PackageTourID = Me.PackageTourID
        End If

        package.FinTransID = DropDownListFinTransCodeID.SelectedValue
        package.UseFormula = CheckBoxUseFormula.Checked
        package.Formula = TextBoxFormula.Text.Trim()

        Dim amt As Decimal = 0

        If String.IsNullOrEmpty(TextBoxAmount.Text) Then
            package.Amount = 0
        Else

            If Decimal.TryParse(TextBoxAmount.Text, amt) Then
                package.Amount = amt
            End If
        End If

        package.Save()
    End Sub


End Class
