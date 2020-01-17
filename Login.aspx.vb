
Imports System.Data.SqlClient


Partial Class Login
    Inherits System.Web.UI.Page



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then
            
        End If
    End Sub

    Private Function Auth(ByVal sUser As String, ByVal sPass As String, ByRef sErr As String) As Boolean
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("Select * from t_Personnel where UserName = '" & sUser & "' and Password = '" & sPass & "' and active = 1", cn)
        Dim dr As SqlDataReader
        Try
            cn.Open()
            dr = cm.ExecuteReader
            If dr.HasRows Then
                dr.Read()
                Session.Item("UserName") = dr("UserName")
                Session.Item("UserID") = dr("PersonnelID")
                Session.Item("FirstName") = dr("FirstName")
                Session.Item("LastName") = dr("LastName")
                Return True
            Else
                Return False
            End If
            cn.Close()
        Catch ex As Exception
            sErr = ex.ToString
            Return False
        Finally
            If cn.State <> Data.ConnectionState.Closed Then cn.Close()
            cn = Nothing
            cm = Nothing
            dr = Nothing
        End Try

    End Function

    Protected Sub Submit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Submit.Click
        Dim sErr As String = ""
        If Auth(UserName.Text, Password.Value, sErr) Then
            Response.Redirect(referrer.Value)
        Else
            Response.Write(sErr) 'Me.Page.Items("sError").innerHTML = sErr
        End If
    End Sub


    
End Class
