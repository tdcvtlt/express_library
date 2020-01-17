Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsPurchaseRequestItemsReceived
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _PRItemID As Integer = 0
    Dim _Qty As Integer = 0
    Dim _ReceivedByID As Integer = 0
    Dim _DateReceived As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_PurchaseRequestItemsReceived where ID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_PurchaseRequestItemsReceived where ID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_PurchaseRequestItemsReceived")
            If ds.Tables("t_PurchaseRequestItemsReceived").Rows.Count > 0 Then
                dr = ds.Tables("t_PurchaseRequestItemsReceived").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("PRItemID") Is System.DBNull.Value) Then _PRItemID = dr("PRItemID")
        If Not (dr("Qty") Is System.DBNull.Value) Then _Qty = dr("Qty")
        If Not (dr("ReceivedByID") Is System.DBNull.Value) Then _ReceivedByID = dr("ReceivedByID")
        If Not (dr("DateReceived") Is System.DBNull.Value) Then _DateReceived = dr("DateReceived")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_PurchaseRequestItemsReceived where ID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_PurchaseRequestItemsReceived")
            If ds.Tables("t_PurchaseRequestItemsReceived").Rows.Count > 0 Then
                dr = ds.Tables("t_PurchaseRequestItemsReceived").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_PurchaseRequestItemsReceivedInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@PRItemID", SqlDbType.Int, 0, "PRItemID")
                da.InsertCommand.Parameters.Add("@Qty", SqlDbType.Int, 0, "Qty")
                da.InsertCommand.Parameters.Add("@ReceivedByID", SqlDbType.Int, 0, "ReceivedByID")
                da.InsertCommand.Parameters.Add("@DateReceived", SqlDbType.Date, 0, "DateReceived")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@ID", SqlDbType.Int, 0, "ID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_PurchaseRequestItemsReceived").NewRow
            End If
            Update_Field("PRItemID", _PRItemID, dr)
            Update_Field("Qty", _Qty, dr)
            Update_Field("ReceivedByID", _ReceivedByID, dr)
            Update_Field("DateReceived", _DateReceived, dr)
            If ds.Tables("t_PurchaseRequestItemsReceived").Rows.Count < 1 Then ds.Tables("t_PurchaseRequestItemsReceived").Rows.Add(dr)
            da.Update(ds, "t_PurchaseRequestItemsReceived")
            _ID = ds.Tables("t_PurchaseRequestItemsReceived").Rows(0).Item("ID")
            If cn.State <> ConnectionState.Closed Then cn.Close()
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
            oEvents.KeyField = "ID"
            oEvents.KeyValue = _ID
            oEvents.Create_Event()
            drow(sField) = sValue
            _Err = oEvents.Error_Message
        End If
        oEvents = Nothing
    End Sub

    Protected Overrides Sub Finalize()
        cn = Nothing
        cm = Nothing
        MyBase.Finalize()
    End Sub

    Public Property PRItemID() As Integer
        Get
            Return _PRItemID
        End Get
        Set(ByVal value As Integer)
            _PRItemID = value
        End Set
    End Property

    Public Property Qty() As Integer
        Get
            Return _Qty
        End Get
        Set(ByVal value As Integer)
            _Qty = value
        End Set
    End Property

    Public Property RecevedByID() As Integer
        Get
            Return _ReceivedByID
        End Get
        Set(ByVal value As Integer)
            _ReceivedByID = value
        End Set
    End Property

    Public Property DateReceived() As String
        Get
            Return _DateReceived
        End Get
        Set(ByVal value As String)
            _DateReceived = value
        End Set
    End Property

    Public Property ID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property

    Public Property UserID() As Integer
        Get
            Return _UserID
        End Get
        Set(value As Integer)
            _UserID = value
        End Set
    End Property

    Public Property Err() As String
        Get
            Return _Err
        End Get
        Set(value As String)
            _Err = value
        End Set
    End Property

End Class
