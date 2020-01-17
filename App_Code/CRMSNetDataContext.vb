Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Linq
Imports System.Data.SqlClient
Imports System.Data.Linq

Public Class CRMSNetDataContext

    Private Shared _context As DataContext = New DataContext(Resources.Resource.cns)

    Public Shared ReadOnly Property Context() As DataContext
        Get
            Return _context
        End Get
    End Property


End Class
