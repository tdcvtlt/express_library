Imports System.Linq
Imports System.ComponentModel
Imports System
Imports System.Data.Linq

Public Class ComboItem
    Private _comboitemid As System.Nullable(Of Integer)
    Private _comboid As System.Nullable(Of Integer)
    Private _comboitem As String
    Private _active As Boolean
    Private _description As String
    Private _locationid As System.Nullable(Of Integer)
    Private _comboname As String

    Public Property Active As Boolean
        Get
            Return _active
        End Get
        Set(ByVal value As Boolean)
            _active = value
        End Set
    End Property
    Public Property ComboID As Integer
        Get
            Return _comboid
        End Get
        Set(ByVal value As Integer)
            _comboid = value
        End Set
    End Property
    Public Property ComboItem() As String
        Get
            Return _comboitem
        End Get
        Set(ByVal value As String)
            _comboitem = value
        End Set
    End Property
    Public Property ComboItemID As Integer
        Get
            Return _comboitemid
        End Get
        Set(ByVal value As Integer)
            _comboitemid = value
        End Set
    End Property
    Public Property ComboName As String
        Get
            Return _comboname
        End Get
        Set(ByVal value As String)
            _comboname = value
        End Set
    End Property
    Public Property Description As String
        Get
            Return _description
        End Get
        Set(ByVal value As String)
            _description = value
        End Set
    End Property
    Public Property Locationid() As System.Nullable(Of Integer)
        Get
            Return _locationid
        End Get
        Set(ByVal value As System.Nullable(Of Integer))
            _locationid = value
        End Set
    End Property



End Class
