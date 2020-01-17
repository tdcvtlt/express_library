Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsEquiantCodeMapping
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _Code As String = ""
    Dim _SourceCode As String = ""
    Dim _Category As String = ""
    Dim _Action As Integer = 0
    Dim _PosNeg As Integer = 0
    Dim _InvoiceID As Integer = 0
    Dim _PaymentMethodID As Integer = 0
    Dim _CreateInvoice As Boolean = False
    Dim _KeyField As String = ""
    Dim _InvoiceType As String = ""
    Dim _Err As String = ""
    Dim _OneTime As Boolean = False
    Dim _TransID As Integer = 0
    Dim _Active As Boolean = False
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_EquiantCodeMapping where ID = " & _ID, cn)
    End Sub

    Public Function List() As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.SelectCommand = "Select * from t_EquiantCodeMapping Where Active = 1 order by Code, SourceCode, Category"
            ds.ConnectionString = Resources.Resource.cns
        Catch ex As Exception

        End Try
        Return ds
    End Function

    Public Function List_UnMapped() As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            ds.SelectCommand = "Select distinct t.Code, t.SourceCode, d.Category, case when d.amount > 0 then 0 else 1 end as PosNeg from t_EquiantBillingTransactions t inner join t_EquiantCategoryDescriptions d on d.transactionid = t.transactionid where d.amount <> 0 and t.Code + t.SourceCode + d.Category + case when d.amount>0 then '0' else '1' end  not in (Select Code + SourceCode + Category + cast(posneg as varchar(1)) from t_EquiantCodeMapping) order by t.Code, t.SourceCode, d.Category"
            ds.ConnectionString = Resources.Resource.cns
        Catch ex As Exception

        End Try
        Return ds
    End Function

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_EquiantCodeMapping where ID = " & _ID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_EquiantCodeMapping")
            If ds.Tables("t_EquiantCodeMapping").Rows.Count > 0 Then
                dr = ds.Tables("t_EquiantCodeMapping").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("Code") Is System.DBNull.Value) Then _Code = dr("Code")
        If Not (dr("SourceCode") Is System.DBNull.Value) Then _SourceCode = dr("SourceCode")
        If Not (dr("Category") Is System.DBNull.Value) Then _Category = dr("Category")
        If Not (dr("Action") Is System.DBNull.Value) Then _Action = dr("Action")
        If Not (dr("PosNeg") Is System.DBNull.Value) Then _PosNeg = dr("PosNeg")
        If Not (dr("InvoiceID") Is System.DBNull.Value) Then _InvoiceID = dr("InvoiceID")
        If Not (dr("PaymentMethodID") Is System.DBNull.Value) Then _PaymentMethodID = dr("PaymentMethodID")
        If Not (dr("CreateInvoice") Is System.DBNull.Value) Then _CreateInvoice = dr("CreateInvoice")
        If Not (dr("KeyField") Is System.DBNull.Value) Then _KeyField = dr("KeyField")
        If Not (dr("InvoiceType") Is System.DBNull.Value) Then _InvoiceType = dr("InvoiceType")
        If Not (dr("OneTime") Is System.DBNull.Value) Then _OneTime = dr("OneTime")
        If Not (dr("TransID") Is System.DBNull.Value) Then _TransID = dr("TransID")
        If Not (dr("Active") Is System.DBNull.Value) Then _Active = dr("Active")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_EquiantCodeMapping where ID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_EquiantCodeMapping")
            If ds.Tables("t_EquiantCodeMapping").Rows.Count > 0 Then
                dr = ds.Tables("t_EquiantCodeMapping").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_EquiantCodeMappingInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@Code", SqlDbType.varchar, 0, "Code")
                da.InsertCommand.Parameters.Add("@SourceCode", SqlDbType.varchar, 0, "SourceCode")
                da.InsertCommand.Parameters.Add("@Category", SqlDbType.varchar, 0, "Category")
                da.InsertCommand.Parameters.Add("@Action", SqlDbType.int, 0, "Action")
                da.InsertCommand.Parameters.Add("@PosNeg", SqlDbType.int, 0, "PosNeg")
                da.InsertCommand.Parameters.Add("@InvoiceID", SqlDbType.int, 0, "InvoiceID")
                da.InsertCommand.Parameters.Add("@PaymentMethodID", SqlDbType.int, 0, "PaymentMethodID")
                da.InsertCommand.Parameters.Add("@CreateInvoice", SqlDbType.bit, 0, "CreateInvoice")
                da.InsertCommand.Parameters.Add("@KeyField", SqlDbType.varchar, 0, "KeyField")
                da.InsertCommand.Parameters.Add("@InvoiceType", SqlDbType.VarChar, 0, "InvoiceType")
                da.InsertCommand.Parameters.Add("@OneTime", SqlDbType.Bit, 0, "OneTime")
                da.InsertCommand.Parameters.Add("@TransID", SqlDbType.Int, 0, "TransID")
                da.InsertCommand.Parameters.Add("@Active", SqlDbType.Bit, 0, "Active")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@ID", SqlDbType.Int, 0, "ID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_EquiantCodeMapping").NewRow
            End If
            Update_Field("Code", _Code, dr)
            Update_Field("SourceCode", _SourceCode, dr)
            Update_Field("Category", _Category, dr)
            Update_Field("Action", _Action, dr)
            Update_Field("PosNeg", _PosNeg, dr)
            Update_Field("InvoiceID", _InvoiceID, dr)
            Update_Field("PaymentMethodID", _PaymentMethodID, dr)
            Update_Field("CreateInvoice", _CreateInvoice, dr)
            Update_Field("KeyField", _KeyField, dr)
            Update_Field("InvoiceType", _InvoiceType, dr)
            Update_Field("OneTime", _OneTime, dr)
            Update_Field("TransID", _TransID, dr)
            Update_Field("Active", _Active, dr)
            If ds.Tables("t_EquiantCodeMapping").Rows.Count < 1 Then ds.Tables("t_EquiantCodeMapping").Rows.Add(dr)
            da.Update(ds, "t_EquiantCodeMapping")
            _ID = ds.Tables("t_EquiantCodeMapping").Rows(0).Item("ID")
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


    Public Function Get_payment_Methods(ByVal reqID As Integer) As SqlDataSource
        Dim ds As New SqlDataSource
        Try
            If reqID = 0 Then
                ds.SelectCommand = "Select p.ComboItemID, p.ComboItem from t_Comboitems p inner join t_Combos c on p.ComboID = c.ComboID where c.ComboName = 'PaymentMethod' and p.ComboItem in ('Adjustment','Equiant Payment','Meridian - Equiant','Waiver','Bad Debt','Referral') order by p.ComboItem"
            Else
                ds.SelectCommand = "Select p.ComboItemID, p.ComboItem from t_Comboitems p inner join t_Combos c on p.ComboID = c.ComboID where (c.ComboName = 'PaymentMethod' and p.ComboItem in ('Adjustment','Equiant Payment','Meridian - Equiant','Waiver','Bad Debt','Referral')) or p.ComboItemID in (Select PaymentMethodID from t_EquiantCodeMapping where ID = '" & reqID & "') order by p.ComboItem"
            End If
            'ds.SelectCommand = "Select Distinct(e.PaymentMethodID) as ComboItemID, p.ComboItem from t_EquiantCodeMapping e inner join t_Comboitems p on e.PaymentMethodID = p.ComboItemID order by p.ComboItem"
            ds.ConnectionString = Resources.Resource.cns
        Catch ex As Exception

        End Try
        Return ds
    End Function

    Public Property OneTime As Boolean
        Get
            Return _OneTime
        End Get
        Set(value As Boolean)
            _OneTime = value
        End Set
    End Property

    Public Property TransID As Integer
        Get
            Return _TransID
        End Get
        Set(value As Integer)
            _TransID = value
        End Set
    End Property

    Public Property Active As Boolean
        Get
            Return _Active
        End Get
        Set(value As Boolean)
            _Active = value
        End Set
    End Property

    Public Property Code() As String
        Get
            Return _Code
        End Get
        Set(ByVal value As String)
            _Code = value
        End Set
    End Property

    Public Property SourceCode() As String
        Get
            Return _SourceCode
        End Get
        Set(ByVal value As String)
            _SourceCode = value
        End Set
    End Property

    Public Property Category() As String
        Get
            Return _Category
        End Get
        Set(ByVal value As String)
            _Category = value
        End Set
    End Property

    Public Property Action() As Integer
        Get
            Return _Action
        End Get
        Set(ByVal value As Integer)
            _Action = value
        End Set
    End Property

    Public Property PosNeg() As Integer
        Get
            Return _PosNeg
        End Get
        Set(ByVal value As Integer)
            _PosNeg = value
        End Set
    End Property

    Public Property InvoiceID() As Integer
        Get
            Return _InvoiceID
        End Get
        Set(ByVal value As Integer)
            _InvoiceID = value
        End Set
    End Property

    Public Property PaymentMethodID() As Integer
        Get
            Return _PaymentMethodID
        End Get
        Set(ByVal value As Integer)
            _PaymentMethodID = value
        End Set
    End Property

    Public Property CreateInvoice() As Boolean
        Get
            Return _CreateInvoice
        End Get
        Set(ByVal value As Boolean)
            _CreateInvoice = value
        End Set
    End Property

    Public Property KeyField() As String
        Get
            Return _KeyField
        End Get
        Set(ByVal value As String)
            _KeyField = value
        End Set
    End Property

    Public Property InvoiceType() As String
        Get
            Return _InvoiceType
        End Get
        Set(ByVal value As String)
            _InvoiceType = value
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
End Class
