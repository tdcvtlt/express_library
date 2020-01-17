Imports System
Imports System.Linq
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO


Partial Class Reports_Contracts_SalesReportByClosingOfficer
    Inherits System.Web.UI.Page

    Private cnx As String = Resources.Resource.cns.ToString()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load



        If (Page.IsPostBack = False) Then



        End If

    End Sub





End Class
