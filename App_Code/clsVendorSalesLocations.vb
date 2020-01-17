Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsVendorSalesLocations
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _VendorID As Integer = 0
    Dim _Location As String = ""
    Dim _DateCreated As String = ""
    Dim _Active As Boolean = False
    Dim _DateDeActivated As String = ""
    Dim _CRMSID As Integer = 0
    Dim _VRCCost As Decimal
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_VendorSalesLocations where SalesLocationID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_VendorSalesLocations where SalesLocationID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_VendorSalesLocations")
            If ds.Tables("t_VendorSalesLocations").Rows.Count > 0 Then
                dr = ds.Tables("t_VendorSalesLocations").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("VendorID") Is System.DBNull.Value) Then _VendorID = dr("VendorID")
        If Not (dr("Location") Is System.DBNull.Value) Then _Location = dr("Location")
        If Not (dr("DateCreated") Is System.DBNull.Value) Then _DateCreated = dr("DateCreated")
        If Not (dr("Active") Is System.DBNull.Value) Then _Active = dr("Active")
        If Not (dr("DateDeActivated") Is System.DBNull.Value) Then _DateDeActivated = dr("DateDeActivated")
        If Not (dr("CRMSID") Is System.DBNull.Value) Then _CRMSID = dr("CRMSID")
        If Not (dr("VRCCost") Is System.DBNull.Value) Then _VRCCost = dr("VRCCost")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_VendorSalesLocations where SalesLocationID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_VendorSalesLocations")
            If ds.Tables("t_VendorSalesLocations").Rows.Count > 0 Then
                dr = ds.Tables("t_VendorSalesLocations").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_VendorSalesLocationsInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@VendorID", SqlDbType.Int, 0, "VendorID")
                da.InsertCommand.Parameters.Add("@Location", SqlDbType.VarChar, 0, "Location")
                da.InsertCommand.Parameters.Add("@DateCreated", SqlDbType.DateTime, 0, "DateCreated")
                da.InsertCommand.Parameters.Add("@Active", SqlDbType.Bit, 0, "Active")
                da.InsertCommand.Parameters.Add("@DateDeActivated", SqlDbType.DateTime, 0, "DateDeActivated")
                da.InsertCommand.Parameters.Add("@CRMSID", SqlDbType.Int, 0, "CRMSID")
                da.InsertCommand.Parameters.Add("@VRCCost", SqlDbType.SmallMoney, 0, "VRCCost")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@SalesLocationID", SqlDbType.Int, 0, "SalesLocationID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_VendorSalesLocations").NewRow
            End If
            Update_Field("VendorID", _VendorID, dr)
            Update_Field("Location", _Location, dr)
            Update_Field("DateCreated", _DateCreated, dr)
            Update_Field("Active", _Active, dr)
            Update_Field("DateDeActivated", _DateDeActivated, dr)
            Update_Field("CRMSID", _CRMSID, dr)
            Update_Field("VRCCost", _VRCCost, dr)
            If ds.Tables("t_VendorSalesLocations").Rows.Count < 1 Then ds.Tables("t_VendorSalesLocations").Rows.Add(dr)
            da.Update(ds, "t_VendorSalesLocations")
            _ID = ds.Tables("t_VendorSalesLocations").Rows(0).Item("SalesLocationID")
            Return True
        Catch ex As Exception
            _Err = ex.ToString
            Return False
        End Try
    End Function

    Private Sub Update_Field(ByVal sField As String, ByVal sValue As String, ByRef drow As DataRow)
        Dim oEvents As New clsEvents
        If IIf(Not (drow(sField) Is System.DBNull.Value), drow(sField), "") <> sValue Then
            oEvents.EventType = "Change"
            oEvents.FieldName = sField
            oEvents.OldValue = IIf(Not (drow(sField) Is System.DBNull.Value), drow(sField), "")
            oEvents.NewValue = sValue
            oEvents.DateCreated = Date.Now
            oEvents.CreatedByID = _UserID
            oEvents.KeyField = "SalesLocationID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
        oEvents = Nothing
    End Sub

    Public Function List_Tradeshow_Vendors() As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            ds.SelectCommand = String.Format("select distinct v.VendorID, v.Vendor " _
                & "from t_VendorSalesLocations vsl inner join t_Vendor v on v.VendorID = vsl.VendorID " _
                & "inner join t_Vendor2Package v2p on v2p.VendorID = v.VendorID " _
                & "inner join t_Package p on p.PackageID = v2p.PackageID " _
                & "inner join t_ComboItems pt on pt.ComboItemID = p.TypeID " _
                & "where pt.ComboItem = 'Tradeshow' " _
                & "and p.Active = 1 and v.Active = 1 and vsl.Active = 1 order by v.Vendor")
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function

    Public Function List_Sales_Locations_By_Vendor(vendor_id As Int32) As SqlDataSource
        Dim ds As New SqlDataSource
        ds.ConnectionString = Resources.Resource.cns
        Try
            ds.SelectCommand = String.Format("select * from t_VendorSalesLocations where VendorID in ({0}) and Active = 1 order by Location;", vendor_id)
        Catch ex As Exception
            _Err = ex.Message
        End Try
        Return ds
    End Function



    Protected Overrides Sub Finalize()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Public Property VendorID() As Integer
        Get
            Return _VendorID
        End Get
        Set(ByVal value As Integer)
            _VendorID = value
        End Set
    End Property

    Public Property Location() As String
        Get
            Return _Location
        End Get
        Set(ByVal value As String)
            _Location = value
        End Set
    End Property

    Public Property DateCreated() As String
        Get
            Return _DateCreated
        End Get
        Set(ByVal value As String)
            _DateCreated = value
        End Set
    End Property

    Public Property Active() As Boolean
        Get
            Return _Active
        End Get
        Set(ByVal value As Boolean)
            _Active = value
        End Set
    End Property

    Public Property DateDeActivated() As String
        Get
            Return _DateDeActivated
        End Get
        Set(ByVal value As String)
            _DateDeActivated = value
        End Set
    End Property

    Public Property CRMSID() As Integer
        Get
            Return _CRMSID
        End Get
        Set(ByVal value As Integer)
            _CRMSID = value
        End Set
    End Property

    Public Property VRCCost() As Decimal
        Get
            Return _VRCCost
        End Get
        Set(ByVal value As Decimal)
            _VRCCost = value
        End Set
    End Property

    Public Property SalesLocationID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
