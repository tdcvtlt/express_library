Imports Microsoft.VisualBasic
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports System.Data.SqlClient
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports System.Web.Security
Imports System.Web
Imports System.Configuration
Imports System.Data
Imports System
Imports System.Collections.Generic
Imports System.Linq

Public Class Report : Implements ICrystal
    Private __Session As HttpContext
    Private server As HttpServerUtility

    Private _reportDocument As ReportDocument
    Private _reportPath As String
    Private _serverName As String
    Private _database As String
    Private _user As String
    Private _password As String
    Private _parameters As IList(Of KeyValuePair(Of String, String))
    Private _httpCurrent As HttpContext




    Public Sub New()

        _database = Resources.Resource.DATABASE
        _serverName = Resources.Resource.SERVER
        _user = Resources.Resource.USERNAME
        _password = Resources.Resource.PASSWORD
        _httpCurrent = HttpContext.Current
    End Sub


#Region "Interface Implementations"
    Public Function DoReport() As System.Web.HttpContext Implements ICrystal.DoReport

        _reportDocument = New ReportDocument()

        _reportDocument.FileName = _reportPath
        _reportDocument.DataSourceConnections(0).SetConnection(_serverName, _database, False)
        _reportDocument.DataSourceConnections(0).SetLogon(_user, _password)

        For Each Pair As KeyValuePair(Of String, String) In Parameters
            _reportDocument.SetParameterValue(Pair.Key, Pair.Value)
        Next

        _reportDocument.Load(_reportPath)
        _httpCurrent.Session("Crystal") = _reportDocument

        Return HttpCurrent
    End Function
#End Region




#Region "Public Methods & Functions"
    Public Sub Dispose()
        HttpContext.Current.Session("Crystal") = Nothing
    End Sub

#End Region

#Region "Properties"


    Public Property HttpCurrent() As HttpContext
        Get
            Return _httpCurrent
        End Get
        Set(ByVal value As HttpContext)
            _httpCurrent = value
        End Set
    End Property


    Public WriteOnly Property Path As String
        Set(ByVal value As String)
            _reportPath = value
        End Set
    End Property

    Public ReadOnly Property Document As ReportDocument
        Get
            Return _reportDocument
        End Get
    End Property

    Public WriteOnly Property ServerName As String
        Set(ByVal value As String)
            _serverName = value
        End Set
    End Property

    Public WriteOnly Property Database As String
        Set(ByVal value As String)
            _database = value
        End Set
    End Property

    Public WriteOnly Property Password() As String
        Set(ByVal value As String)
            _password = value
        End Set
    End Property

    Public WriteOnly Property User() As String
        Set(ByVal value As String)
            _user = value
        End Set
    End Property


    Public Property Parameters() As IList(Of KeyValuePair(Of String, String))
        Get
            Return _parameters
        End Get
        Set(ByVal value As IList(Of KeyValuePair(Of String, String)))
            _parameters = value
        End Set
    End Property
#End Region

End Class
