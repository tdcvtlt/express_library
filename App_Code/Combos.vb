Imports System.Data.Linq.Mapping

<Table(Name:="dbo.t_Combos")> _
Partial Public Class Combos
    Private _comboid As System.Nullable(Of Integer)
    Private _comboname As String
    Private _description As String

    <System.Data.Linq.Mapping.Column(IsPrimaryKey:=True, IsDbGenerated:=True)> _
Public Property ComboID As System.Nullable(Of Integer)
        Get
            Return _comboid
        End Get
        Set(ByVal value As System.Nullable(Of Integer))
            _comboid = value
        End Set
    End Property



    <System.Data.Linq.Mapping.Column()> _
    Public Property ComboName As String
        Get
            Return _comboname
        End Get
        Set(ByVal value As String)
            If (String.IsNullOrEmpty(value) = False) Then
                _comboname = value
            End If
        End Set
    End Property

    <System.Data.Linq.Mapping.Column()> _
    Public Property Description As String
        Get
            Return _description
        End Get
        Set(ByVal value As String)
            If (String.IsNullOrEmpty(value) = False) Then
                _description = value
            End If
        End Set
    End Property

End Class
