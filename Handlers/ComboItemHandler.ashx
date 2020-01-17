<%@ WebHandler Language="VB" Class="ComboItemHandler" %>

Imports System
Imports System.Web
Imports System.Data
Imports System.Data.SqlClient
Imports System.Linq
Imports System.Web.Script.Serialization

Public Class ComboItemHandler : Implements IHttpHandler
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        
        Dim comboitem As String = DirectCast(context.Request("comboitem"), String)
                
        context.Response.ContentType = "text/plain"
                
        Dim list As IList(Of IdName) = New List(Of IdName)
        
        Dim sql As String = "Select distinct UV.UFVALUE from t_UF_VALUE UV where UFID = 331 AND LEN(UFVALUE) > 0 ORDER BY UFVALUE"
       
        Using cnn As New SqlConnection(Resources.Resource.cns)
            Using cmd As New SqlCommand(sql, cnn)
                
                cnn.Open()
                Dim rdr As SqlDataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                
                If rdr.HasRows Then
                    Do While rdr.Read()
                        list.Add(New IdName With {.ID = 0, .Name = rdr.Item(0)})
                    Loop                    
                End If
            End Using
        End Using
        
        Dim js As New JavaScriptSerializer()
        
        context.Response.Write(js.Serialize(list))
    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class