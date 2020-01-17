Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System

Public Class clsEquiant2CRMS
    Dim _UserID As Integer = 0
    Dim _ID As Integer = 0
    Dim _ContractID As Integer = 0
    Dim _EquiantBillingAccount As String = ""
    Dim _EquiantMortAccount As String = ""
    Dim _Err As String = ""
    Dim cn As SqlConnection
    Dim cm As SqlCommand
    Dim da As SqlDataAdapter
    Dim ds As DataSet
    Dim dr As DataRow

    Public Sub New()
        cn = New SqlConnection(Resources.Resource.cns)
        cm = New SqlCommand("Select * from t_Equiant2CRMS where Equiant2CRMSID = " & _ID, cn)
    End Sub

    Public Sub Load()
        Try
            cm.CommandText = "Select * from t_Equiant2CRMS where Equiant2CRMSID = " & _ID & " or ContractID=" & _ContractID
            da = New SqlDataAdapter(cm)
            ds = New DataSet
            da.Fill(ds, "t_Equiant2CRMS")
            If ds.Tables("t_Equiant2CRMS").Rows.Count > 0 Then
                dr = ds.Tables("t_Equiant2CRMS").Rows(0)
                Set_Values()
            End If
        Catch ex As Exception
            _Err = ex.ToString
        End Try
    End Sub

    Private Sub Set_Values()
        If Not (dr("Equiant2CRMSID") Is System.DBNull.Value) Then _ID = dr("Equiant2CRMSID")
        If Not (dr("ContractID") Is System.DBNull.Value) Then _ContractID = dr("ContractID")
        If Not (dr("EquiantBillingAccount") Is System.DBNull.Value) Then _EquiantBillingAccount = dr("EquiantBillingAccount")
        If Not (dr("EquiantMortAccount") Is System.DBNull.Value) Then _EquiantMortAccount = dr("EquiantMortAccount")
    End Sub

    Public Function Save() As Boolean
        Try
            If cn.State <> ConnectionState.Open Then cn.Open()
            cm.CommandText = "Select * from t_Equiant2CRMS where Equiant2CRMSID = " & _ID
            da = New SqlDataAdapter(cm)
            Dim sqlCMBuild As New SqlCommandBuilder(da)
            ds = New DataSet
            da.Fill(ds, "t_Equiant2CRMS")
            If ds.Tables("t_Equiant2CRMS").Rows.Count > 0 Then
                dr = ds.Tables("t_Equiant2CRMS").Rows(0)
            Else
                da.InsertCommand = New SqlCommand("dbo.sp_Equiant2CRMSInsert", cn)
                da.InsertCommand.CommandType = CommandType.StoredProcedure
                da.InsertCommand.Parameters.Add("@ContractID", SqlDbType.Int, 0, "ContractID")
                da.InsertCommand.Parameters.Add("@EquiantBillingAccount", SqlDbType.VarChar, 0, "EquiantBillingAccount")
                da.InsertCommand.Parameters.Add("@EquiantMortAccount", SqlDbType.VarChar, 0, "EquiantMortAccount")
                Dim parameter As SqlParameter = da.InsertCommand.Parameters.Add("@Equiant2CRMSID", SqlDbType.Int, 0, "Equiant2CRMSID")
                parameter.Direction = ParameterDirection.Output
                dr = ds.Tables("t_Equiant2CRMS").NewRow
            End If
            Update_Field("ContractID", _ContractID, dr)
            Update_Field("EquiantBillingAccount", _EquiantBillingAccount, dr)
            Update_Field("EquiantMortAccount", _EquiantMortAccount, dr)
            If ds.Tables("t_Equiant2CRMS").Rows.Count < 1 Then ds.Tables("t_Equiant2CRMS").Rows.Add(dr)
            da.Update(ds, "t_Equiant2CRMS")
            _ID = ds.Tables("t_Equiant2CRMS").Rows(0).Item("Equiant2CRMSID")
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
            oEvents.KeyField = "Equiant2CRMSID"
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

    Public Property ContractID() As Integer
        Get
            Return _ContractID
        End Get
        Set(ByVal value As Integer)
            _ContractID = value
        End Set
    End Property

    Public Property EquiantBillingAccount() As String
        Get
            Return _EquiantBillingAccount
        End Get
        Set(ByVal value As String)
            _EquiantBillingAccount = value
        End Set
    End Property

    Public Property EquiantMortAccount() As String
        Get
            Return _EquiantMortAccount
        End Get
        Set(ByVal value As String)
            _EquiantMortAccount = value
        End Set
    End Property

    Public Property Equiant2CRMSID() As Integer
        Get
            Return _ID
        End Get
        Set(ByVal value As Integer)
            _ID = value
        End Set
    End Property
End Class
