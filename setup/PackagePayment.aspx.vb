Imports System.Data.SqlClient

Partial Class setup_PackagePayment
    Inherits System.Web.UI.Page


#Region "Application Properties"

    Public Property PackageID As Integer
        Get
            Return IIf(String.IsNullOrEmpty(HiddenFieldPackageID.Value), 0, Integer.Parse(HiddenFieldPackageID.Value))
        End Get
        Set(ByVal value As Integer)
            HiddenFieldPackageID.Value = value
        End Set
    End Property


    Public Property PackageTourPaymentID As Integer
        Get
            Return IIf(String.IsNullOrEmpty(HiddenFieldPackageTourPaymentID.Value), 0, Integer.Parse(HiddenFieldPackageTourPaymentID.Value))
        End Get
        Set(ByVal value As Integer)
            HiddenFieldPackageTourPaymentID.Value = value
        End Set
    End Property

    Public Property PackagePaymentID As Integer
        Get
            Return IIf(String.IsNullOrEmpty(HiddenFieldPackagePaymentID.Value), 0, Integer.Parse(HiddenFieldPackagePaymentID.Value))
        End Get
        Set(ByVal value As Integer)
            HiddenFieldPackagePaymentID.Value = value
        End Set
    End Property


    Public Property PackageReservationPaymentID As Integer
        Get
            Return IIf(String.IsNullOrEmpty(HiddenFieldPackageReservationPaymentID.Value), 0, Integer.Parse(HiddenFieldPackageReservationPaymentID.Value))
        End Get
        Set(ByVal value As Integer)
            HiddenFieldPackageReservationPaymentID.Value = value
        End Set
    End Property


    Public Property PackageTourFinTransID As Integer
        Get
            Return IIf(String.IsNullOrEmpty(HiddenFieldPackageTourFinTransID.Value), 0, Integer.Parse(HiddenFieldPackageTourFinTransID.Value))
        End Get
        Set(ByVal value As Integer)
            HiddenFieldPackageTourFinTransID.Value = value
        End Set
    End Property


    Public Property PackageReservationFinTransID As Integer
        Get
            Return IIf(String.IsNullOrEmpty(HiddenFieldPackageReservationFinTransID.Value), 0, Integer.Parse(HiddenFieldPackageReservationFinTransID.Value))
        End Get
        Set(ByVal value As Integer)
            HiddenFieldPackageReservationFinTransID.Value = value
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

    Public Property Back As String
        Get
            Return HiddenFieldBack.Value
        End Get
        Set(ByVal value As String)
            HiddenFieldBack.Value = value
        End Set
    End Property

#End Region
#Region "Page Events"


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Me.IsPostBack = False Then

            Preload_Combo()

            Me.PackageID = Request.QueryString("PackageID")
            Me.PackageTourPaymentID = Request.QueryString("PackageTourPaymentID")
            Me.PackagePaymentID = Request.QueryString("PackagePaymentID")
            Me.PackageReservationPaymentID = Request.QueryString("PackageReservationPaymentID")
            Me.PackageTourFinTransID = Request.QueryString("PackageTourFinTransCodeID")
            Me.PackageReservationFinTransID = Request.QueryString("PackageReservationFinTransCodeID")
            Response.Write(Me.PackageTourFinTransID.ToString())
            Me.Type = Request.QueryString("Type")
            If Me.PackageID > 0 Then

                If Me.Type.Equals("PackageTourPayment") Then

                    Load_Values(New clsPackageTourPayment())

                ElseIf Me.Type.Equals("PackageReservationPayment") Then

                    Load_Values(New clsPackageReservationPayment())

                ElseIf Me.Type.Equals("PackagePayment") Then

                    Load_Values(New clsPackagePayment())
                End If


            End If
        End If


    End Sub


    Protected Sub ButtonSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSubmit.Click

        If Me.PackageID > 0 Then

            If Me.Type.Equals("PackageTourPayment") Then

                Write_Values(New clsPackageTourPayment())

                ClientScript.RegisterClientScriptBlock(Me.GetType(), Guid.NewGuid.ToString(), _
                    "window.opener.refreshPayment(); window.close();", _
                    True)
            ElseIf Me.Type.Equals("PackageReservationPayment") Then

                Write_Values(New clsPackageReservationPayment())

                ClientScript.RegisterClientScriptBlock(Me.GetType(), Guid.NewGuid.ToString(), _
                    "window.opener.refreshPayment(); window.close();", _
                    True)
            ElseIf Me.Type.Equals("PackagePayment") Then

                Write_Values(New clsPackagePayment())

                ClientScript.RegisterClientScriptBlock(Me.GetType(), Guid.NewGuid.ToString(), _
                    "window.opener.refreshPayment(); window.close();", _
                    True)
            End If
        End If
    End Sub



#End Region
#Region "Custom Code"




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

        Dim sql As String = _
                  "select ComboItemID, ComboItem [Payment Method] from t_comboitems where ComboID = 343 and Active = 1 " & _
                  "order by ComboItem"

        DropDownListPaymentMethod.DataSource = GetSQLData(sql).ExecuteReader(Data.CommandBehavior.CloseConnection)
        DropDownListPaymentMethod.DataTextField = "Payment Method"
        DropDownListPaymentMethod.DataValueField = "ComboItemID"
        DropDownListPaymentMethod.DataBind()
    End Sub

    Private Sub Load_Values(ByVal payment As clsPackageTourPayment)

        If Me.PackageTourPaymentID > 0 Then
            payment.PackageTourPaymentID = Me.PackageTourPaymentID
            payment.Load()

            Show_Values( _
                payment.PaymentMethodID.ToString(), _
                payment.Amount, _
                payment.Adjustment, _
                payment.PosNeg)
        End If
  
    End Sub

    Private Sub Load_Values(ByVal payment As clsPackageReservationPayment)

        If Me.PackageReservationPaymentID > 0 Then
            payment.PackageReservationPaymentID = Me.PackageReservationPaymentID
            payment.Load()

            Show_Values( _
                payment.PaymentMethodID.ToString(), _
                payment.Amount, _
                payment.Adjustment, _
                payment.PosNeg)
        End If
    End Sub

    Private Sub Load_Values(ByVal payment As clsPackagePayment)

        If Me.PackagePaymentID > 0 Then
            payment.PackagePaymentID = Me.PackagePaymentID
            payment.Load()

            Show_Values( _
                payment.PaymentMethodID.ToString(), _
                payment.Amount, _
                payment.Adjustment, _
                payment.PosNeg)
        End If
    End Sub


    Private Sub Show_Values(ByVal selected_method As String, _
                            ByVal amount As Decimal, _
                            ByVal adj As Boolean, _
                            ByVal posneg As Boolean)

        DropDownListPaymentMethod.SelectedValue = selected_method
        TextBoxFixedAmount.Text = amount
        CheckBoxAdjustment.Checked = adj
        If posneg Then
            RadioButtonPositive.Checked = True
        Else
            RadioButtonNegative.Checked = True
        End If


    End Sub

    Private Sub Write_Values(ByVal payment As clsPackageTourPayment)

        If Me.PackageTourPaymentID > 0 Then
            payment.PackageTourPaymentID = Me.PackageTourPaymentID
            payment.Load()
        Else
            payment.PackageTourFinTransID = Me.PackageTourFinTransID
            payment.PackageID = Me.PackageID
        End If

        payment.PaymentMethodID = DropDownListPaymentMethod.SelectedValue

        Dim amount As Decimal = 0

        If String.IsNullOrEmpty(TextBoxFixedAmount.Text) = False Then
            If Decimal.TryParse(TextBoxFixedAmount.Text, amount) Then
                payment.Amount = amount
            End If
        Else
            payment.Amount = 0
        End If

        payment.Adjustment = CheckBoxAdjustment.Checked
        payment.PosNeg = IIf(RadioButtonNegative.Checked Or RadioButtonPositive.Checked, True, False)        

        payment.Save()
        Response.Write(payment.Err)
    End Sub

    Private Sub Write_Values(ByVal payment As clsPackageReservationPayment)

        If Me.PackageReservationPaymentID > 0 Then
            payment.PackageReservationPaymentID = Me.PackageReservationPaymentID
            payment.Load()
        Else
            payment.PackageReservationFinTransID = Me.PackageReservationFinTransID
            payment.PackageID = Me.PackageID
        End If

        payment.PaymentMethodID = DropDownListPaymentMethod.SelectedValue

        Dim amount As Decimal = 0

        If String.IsNullOrEmpty(TextBoxFixedAmount.Text) = False Then
            If Decimal.TryParse(TextBoxFixedAmount.Text, amount) Then
                payment.Amount = amount
            End If
        Else
            payment.Amount = 0
        End If

        payment.Adjustment = CheckBoxAdjustment.Checked
        payment.PosNeg = IIf(RadioButtonNegative.Checked Or RadioButtonPositive.Checked, True, False)



        payment.Save()
    End Sub

    Private Sub Write_Values(ByVal payment As clsPackagePayment)

        If Me.PackagePaymentID > 0 Then
            payment.PackagePaymentID = Me.PackagePaymentID
            payment.Load()        
        End If

        payment.PaymentMethodID = DropDownListPaymentMethod.SelectedValue
        payment.PackageID = Me.PackageID

        Dim amount As Decimal = 0

        If String.IsNullOrEmpty(TextBoxFixedAmount.Text) = False Then
            If Decimal.TryParse(TextBoxFixedAmount.Text, amount) Then
                payment.Amount = amount
            End If
        Else
            payment.Amount = 0
        End If

        payment.Adjustment = CheckBoxAdjustment.Checked
        payment.PosNeg = IIf(RadioButtonNegative.Checked Or RadioButtonPositive.Checked, True, False)

        payment.Save()
    End Sub

#End Region



  
  
End Class
