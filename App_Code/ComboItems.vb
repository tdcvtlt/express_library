Imports System.Data.Linq.Mapping

<Table(Name:="dbo.t_ComboItems")> _
Partial Public Class ComboItems
    Private _comboitemid As System.Nullable(Of Integer)
    Private _comboid As System.Nullable(Of Integer)
    Private _comboitem As String
    Private _active As Boolean
    Private _description As String
    Private _locationid As System.Nullable(Of Integer)


    <Column()> _
    Public Property ComboID As System.Nullable(Of Integer)
        Get
            Return _comboid
        End Get
        Set(ByVal value As System.Nullable(Of Integer))
            _comboid = value
        End Set
    End Property
    <Column(IsPrimaryKey:=True, IsDbGenerated:=True)> _
    Public Property ComboItemID As System.Nullable(Of Integer)
        Get
            Return _comboitemid
        End Get
        Set(ByVal value As System.Nullable(Of Integer))
            _comboitemid = value
        End Set
    End Property


    <Column()> _
    Public Property LocationId() As System.Nullable(Of Integer)
        Get
            Return _locationid
        End Get
        Set(ByVal value As System.Nullable(Of Integer))
            _locationid = value
        End Set
    End Property


    <Column()> _
    Public Property Active As Boolean
        Get
            Return _active
        End Get
        Set(ByVal value As Boolean)
            _active = value
        End Set
    End Property


    <Column()> _
    Public Property ComboItem As String
        Get
            Return _comboitem
        End Get
        Set(ByVal value As String)
            _comboitem = value
        End Set
    End Property


    <Column()> _
    Public Property Description() As String
        Get
            Return _description
        End Get
        Set(ByVal value As String)
            If String.IsNullOrEmpty(value) = False Then
                _description = value
            End If
        End Set
    End Property


End Class
