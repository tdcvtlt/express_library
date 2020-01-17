<%@ WebHandler Language="VB" Class="HandlerBankReport" %>

Imports System
Imports System.Web
Imports System.Data
Imports System.Data.SqlClient

Public Class HandlerBankReport : Implements IHttpHandler
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        
        Dim Status As String = context.Request("Status")
        Dim UnitSize As String = context.Request("UnitSize")
        Dim Exchange As String = context.Request("Exchange")
        Dim YearUsed As String = context.Request("YearUsed")
        Dim Season As String = context.Request("Season")
        Dim UnitType As String = context.Request("UnitType")
        Dim DepositYear As String = context.Request("DepositYear")
        Dim Usage As String = context.Request("Usage")
        
        
        Dim sql As String = String.Format("select * from t_bankedunits where statusid like '{0}%' and unitsize like '{1}" & _
                                   "%' and Exchangeid like '{2}%' and usageyear like '{3}" & _
                                   "%' and seasonid like '{4}%' and unittypeid like '{5}" & _
                                   "%' and deposityear like '{6}%' and frequencyid like '{7}%'", _
                                   Status, UnitSize, Exchange, YearUsed, Season, UnitType, DepositYear, Usage)
        
        
        Using cnn As New SqlConnection(Resources.Resource.cns)
            Using ada As New SqlDataAdapter(sql, cnn)
                
                Dim dt As New DataTable()
                ada.Fill(dt)
                
                context.Response.ContentType = "text/plain"
                context.Response.Write(dt.Rows.Count.ToString())
                
            End Using
        End Using        
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class