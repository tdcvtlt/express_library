Imports System.Data.Sql
Imports System.Data.SqlClient

Partial Class setup_QueueScripts
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then
            btnSearch_Click(sender, e)
        End If
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If txtScript.Text & "" <> "" Then
            Run_Query("Select * from t_Scripts where ScriptID = " & txtScript.Text)
        Else
            Run_Query("Select * from t_Scripts")
        End If
    End Sub

    Public Sub Run_Query(ByVal sSQL As String)
        Dim cn As New SqlConnection("data source=SQL-01;initial catalog=CallQueue;user=asp;password=aspnet;persist security info=False;packet size=4096;")
        Dim cm As New SqlCommand("", cn)
        Dim dr As SqlDataReader

        Try
            cn.Open()
            cm.CommandText = sSQL
            dr = cm.ExecuteReader

            If dr.HasRows Then
                GridView1.DataSource = dr
                Dim ka(0) As String
                ka(0) = "scriptid"
                GridView1.DataKeyNames = ka
                GridView1.DataBind()
            Else
                Response.Write("No Rows")
            End If
            cn.Close()
        Catch ex As Exception
            Response.Write(ex.Message)
        Finally
            If cn.State <> Data.ConnectionState.Closed Then cn.Close()
            cn = Nothing
            cm = Nothing
            dr = Nothing
        End Try
    End Sub

    Protected Sub btnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Response.Redirect("editScripts.aspx?scriptid=0")
    End Sub
End Class
