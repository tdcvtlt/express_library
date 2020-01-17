Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Linq
Imports System.Data.SqlClient
Imports System.Data.Linq
Imports CrystalDecisions.CrystalReports.Engine


Namespace kcp


    Public Class CrystalReportRepository


        Public Function Bind(ByRef report As ReportDocument, _
                             ByVal path As String, _
                             ByVal parameter As IDictionary(Of String, String)) As ReportDocument


            Try

                report.DataSourceConnections(0).SetConnection(Resources.Resource.SERVER, Resources.Resource.DATABASE, False)
                report.DataSourceConnections(0).SetLogon(Resources.Resource.USERNAME, Resources.Resource.PASSWORD)

                report.Load(path)
                report.FileName = HttpContext.Current.Server.MapPath(path)

                If parameter.Count > 0 Then

                    'For Each(KeyValuePair(Of
                End If



            Catch ex As Exception

            End Try

            Return report
        End Function
    End Class

End Namespace