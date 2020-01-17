Imports System.Data.SqlClient
Imports System.Data



Partial Class setup_PackagePersonnel
    Inherits System.Web.UI.Page

    Private __PERSONNEL As Object

    Private commission_perc As Decimal = 0
    Private fixed_amt As Decimal = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim sql = String.Empty

        If IsPostBack = False Then

            Preload_Combo()

            '
            'Each one requires a package ID
            HiddenFieldPackageID.Value = DirectCast(Request.QueryString("PackageID"), String)
            HiddenFieldType.Value = Request.QueryString("Type")

            If String.IsNullOrEmpty(Me.PackageID) = False Then
                
                If Me.Type.Equals("PackageReservationPersonnel") Then

                    HiddenFieldPackageID.Value = DirectCast(Request.QueryString("PackageID"), String)
                    HiddenFieldPackageReservationPersonnelID.Value = Request.QueryString("PackageReservationPersonnelID")
                    HiddenFieldPersonnelID.Value = Request.QueryString("PersonnelID")
                    HiddenFieldPackageReservationID.Value = DirectCast(Request.QueryString("PackageReservationID"), String)

                    Load_Values(Integer.Parse(Me.PackageID), _
                                Integer.Parse(Me.PackageReservationID), _
                                Integer.Parse(Me.PackageReservationPersonnelID))


                ElseIf Me.Type.Equals("PackagePersonnel") Then

                    HiddenFieldPackageID.Value = DirectCast(Request.QueryString("PackageID"), String)
                    HiddenFieldPackagePersonnelID.Value = DirectCast(Request.QueryString("PackagePersonnelID"), String)
                    HiddenFieldPersonnelID.Value = Request.QueryString("PersonnelID")


                    Load_Values(Integer.Parse(Me.PackageID), _
                                Integer.Parse(Me.PackagePersonnelID))


                ElseIf Me.Type.Equals("PackageTourPersonnel") Then

                    HiddenFieldPackageID.Value = DirectCast(Request.QueryString("PackageID"), String)
                    HiddenFieldPackageTourID.Value = Request.QueryString("PackageTourID")
                    HiddenFieldPersonnelID.Value = Request.QueryString("PersonnelID")
                    HiddenFieldPackageTourPersonnelID.Value = Request.QueryString("PackageTourPersonnelID")

                    If 1 = 2 Then
                        Response.Write(String.Format("1. {0} <br/> 2. {1} <br/> 3. {2} <br/> 4. {3}", _
                                                     Me.PackageID, _
                                                     Me.PackageTourID, _
                                                     Me.PersonnelID, _
                                                     Me.PackageTourPersonnelID.ToString()))
                    End If


                    Load_Values(Integer.Parse(Me.PackageID), _
                                Integer.Parse(Me.PackageTourID), _
                                Integer.Parse(Me.PersonnelID), _
                                Me.PackageTourPersonnelID)

                End If
            End If
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


    Protected Sub ButtonSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonSubmit.Click


        If String.IsNullOrEmpty(Me.PackageID) = False Then

            If Me.Type.Equals("PackagePersonnel") Then

                Write_Values(Integer.Parse(Me.PackageID), _
                             Integer.Parse(Me.PackagePersonnelID))

                ClientScript.RegisterClientScriptBlock(Me.GetType(), "", _
                        "window.opener.refreshPersonnel(22); window.close();", _
                        True)

            ElseIf Me.Type.Equals("PackageReservationPersonnel") Then


                Write_Values(Integer.Parse(Me.PackageID), _
                             Integer.Parse(Me.PackageReservationID), _
                             Integer.Parse(Me.PackageReservationPersonnelID), _
                             False)

                ClientScript.RegisterClientScriptBlock(Me.GetType(), "247", _
                                        "window.opener.refreshPersonnel(); window.close();", _
                                        True)


            ElseIf Me.Type.Equals("PackageTourPersonnel") Then

                Write_Values(Integer.Parse(Me.PackageID), _
                             Integer.Parse(Me.PackageTourID), _
                             Me.PackageTourPersonnelID)

                ClientScript.RegisterClientScriptBlock(Me.GetType(), "2479", _
                            "window.opener.refreshPersonnel(); window.close();", _
                            True)
            End If
        End If

    End Sub


    Private Sub Preload_Combo()

        Dim sql = _
                        "select B.ComboItemID, ComboItem [Title] from t_combos a inner join t_comboitems b " & _
                        "on a.comboid = b.ComboId " & _
                        "where a.ComboName = 'PersonnelTitle'  " & _
                        "order by a.ComboName DESC"

        Dim cm As SqlCommand
        cm = GetSQLData(sql)

        DropDownListPersonnelTitle.DataSource = cm.ExecuteReader(Data.CommandBehavior.CloseConnection)
        DropDownListPersonnelTitle.DataTextField = "Title"
        DropDownListPersonnelTitle.DataValueField = "ComboItemID"

        DropDownListPersonnelTitle.DataBind()



        sql = _
           "select FirstName, LastName, LastName + ', ' + FirstName [SALES STAFF], PersonnelID from t_personnel order by LastName"

        cm = GetSQLData(sql)

        If IsDBNull(cm) = False Then

            DropDownListPersonnel.DataSource = cm.ExecuteReader(Data.CommandBehavior.CloseConnection)

            DropDownListPersonnel.DataTextField = "SALES STAFF"
            DropDownListPersonnel.DataValueField = "PersonnelID"

            DropDownListPersonnel.DataBind()

        End If

    End Sub




    'Package Personnel
    Private Sub Load_Values(ByVal packageId As Integer, _
                            ByVal packagePersonnelId As Integer)

        Dim personnel = New clsPackagePersonnel()

        If packagePersonnelId > 0 Then
            personnel.ID = packagePersonnelId
            personnel.Load()

            Show_Values(personnel.PersonnelID, _
                        personnel.TitleID, _
                        personnel.CommissionPercentage, _
                        personnel.FixedAmount)
        End If

    End Sub


    'Package Tour Personnel
    Private Sub Load_Values(ByVal packageId As Integer, _
                            ByVal packageTourId As Integer, _
                            ByVal personnelId As Integer, _
                            ByVal packageTourPersonnelId As Integer)

        Dim personnel = New clsPackageTourPersonnel()
        If packageTourPersonnelId > 0 Then
            personnel.PackageTourPersonnelID = packageTourPersonnelId
            personnel.Load()

            Show_Values(personnelId, _
                        personnel.TitleID, _
                        personnel.CommissionPercentage, _
                        personnel.FixedAmount)
        End If

    End Sub

    'Package Reservation Personnel
    Private Sub Load_Values(ByVal packageId As Integer, _
                            ByVal packageReservationId As Integer, _
                            ByVal packageReservationPersonnelId As Integer)

        Dim personnel = New clsPackageReservationPersonnel()

        If packageReservationPersonnelId > 0 Then

            personnel.PackageReservationPersonnelID = packageReservationPersonnelId
            personnel.Load()

            Show_Values(personnel.PersonnelID, _
                        personnel.TitleID, _
                        personnel.CommissionPercentage, _
                        personnel.FixedAmount)

        End If
    End Sub


    Private Sub Show_Values(ByVal personnelId As Integer, _
                            ByVal titleId As Integer, _
                            ByVal percentage_amt As Decimal, _
                            ByVal fixed_amt As Decimal)

        DropDownListPersonnel.SelectedValue = personnelId
        DropDownListPersonnelTitle.SelectedValue = titleId
        TextBoxCommission.Text = percentage_amt
        TextBoxAmount.Text = fixed_amt

    End Sub



 

    'Package Tour Personnel
    Private Sub Write_Values(ByVal packageId As Integer, _
                             ByVal packageTourId As Integer, _
                             ByVal packageTourPersonnelId As Integer)


        ' Package Reservation Peronnel
        Dim personnel = New clsPackageTourPersonnel()

        'Primary key
        If packageTourPersonnelId > 0 Then

            personnel.PackageTourPersonnelID = packageTourPersonnelId
            personnel.Load()
        Else

            personnel.PackageID = packageId
            personnel.PackageTourID = packageTourId

        End If

        personnel.PersonnelID = DropDownListPersonnel.SelectedValue
        personnel.TitleID = DropDownListPersonnelTitle.SelectedValue

        If String.IsNullOrEmpty(TextBoxCommission.Text) Then

            personnel.CommissionPercentage = 0
        Else

            If Decimal.TryParse(TextBoxCommission.Text, commission_perc) Then
                personnel.CommissionPercentage = commission_perc
            End If

        End If


        If String.IsNullOrEmpty(TextBoxAmount.Text) Then
            personnel.FixedAmount = 0

        Else
            If Decimal.TryParse(TextBoxAmount.Text, fixed_amt) Then
                personnel.FixedAmount = fixed_amt
            End If

        End If

        personnel.Save()
    End Sub



    'Package Personnel
    Private Sub Write_Values(ByVal packageId As Integer, _
                              ByVal packagePersonnelId As Integer)

        Dim personnel = New clsPackagePersonnel()

        If packagePersonnelId > 0 Then

            personnel.ID = packagePersonnelId
            personnel.Load()

        Else

            personnel.PackageID = packageId

        End If

        personnel.PersonnelID = DropDownListPersonnel.SelectedValue
        personnel.TitleID = DropDownListPersonnelTitle.SelectedValue


        If String.IsNullOrEmpty(TextBoxCommission.Text) Then

            personnel.CommissionPercentage = 0
        Else

            If Decimal.TryParse(TextBoxCommission.Text, commission_perc) Then
                personnel.CommissionPercentage = commission_perc
            End If

        End If


        If String.IsNullOrEmpty(TextBoxAmount.Text) Then
            personnel.FixedAmount = 0

        Else
            If Decimal.TryParse(TextBoxAmount.Text, fixed_amt) Then
                personnel.FixedAmount = fixed_amt
            End If

        End If


        personnel.Save()
    End Sub



    'Package Reservation Personnel
    Private Sub Write_Values(ByVal packageId As Integer, _
                             ByVal packageReservationId As Integer, _
                             ByVal packageReservationPersonnelId As Integer, _
                             ByVal diff As Boolean)

        Dim personnel = New clsPackageReservationPersonnel()

        If packageReservationPersonnelId > 0 Then

            personnel.PackageReservationPersonnelID = packageReservationPersonnelId
            personnel.Load()

        Else

            personnel.PackageReservationID = packageReservationId
            personnel.PackageID = packageId
        End If

        personnel.PersonnelID = Int32.Parse(DropDownListPersonnel.SelectedValue)
        personnel.TitleID = Int32.Parse(DropDownListPersonnelTitle.SelectedValue)

        If String.IsNullOrEmpty(TextBoxCommission.Text) Then

            personnel.CommissionPercentage = 0
        Else

            If Decimal.TryParse(TextBoxCommission.Text, commission_perc) Then
                personnel.CommissionPercentage = commission_perc
            End If

        End If


        If String.IsNullOrEmpty(TextBoxAmount.Text) Then
            personnel.FixedAmount = 0

        Else
            If Decimal.TryParse(TextBoxAmount.Text, fixed_amt) Then
                personnel.FixedAmount = fixed_amt
            End If

        End If

        personnel.Save()
    End Sub




    Public Property PackageReservationPersonnelID As String
        Get
            Return HiddenFieldPackageReservationPersonnelID.Value
        End Get
        Set(ByVal value As String)
            HiddenFieldPackageReservationPersonnelID.Value = value
        End Set
    End Property

    Public Property PackageReservationID As String
        Get
            Return HiddenFieldPackageReservationID.Value
        End Get
        Set(ByVal value As String)
            HiddenFieldPackageReservationID.Value = value
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


    Public Property PersonnelID As String
        Get
            Return HiddenFieldPersonnelID.Value
        End Get
        Set(ByVal value As String)
            HiddenFieldPersonnelID.Value = value
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

    Public Property PackagePersonnelID As String
        Get
            Return HiddenFieldPackagePersonnelID.Value
        End Get
        Set(ByVal value As String)
            HiddenFieldPackagePersonnelID.Value = value
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
End Class
