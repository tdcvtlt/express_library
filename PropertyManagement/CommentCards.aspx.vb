Imports System
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient

Partial Class PropertyManagement_CommentCards
    Inherits System.Web.UI.Page


    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        If txtID.text <> "" Then
            Get_List("select * from t_CommentCards where cardid = '" & txtID.Text & "'")
        Else
            Get_List("select top 150 * from t_CommentCards")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then

        End If
    End Sub

    Private Sub Get_List(ByVal sSQL As String)
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("", cn)
        Dim dr As SqlDataReader

        Try
            cn.Open()
            cm.CommandText = sSQL
            dr = cm.ExecuteReader
            gridview1.datasource = dr
            Dim ka(0) As String
            ka(0) = "CardID"
            gridview1.datakeynames = ka
            gridview1.databind()
            cn.Close()
        Catch ex As Exception
            lblErr.text = ex.Message
        Finally
            If cn.State <> Data.ConnectionState.Closed Then
                cn = Nothing
                cm = Nothing
                dr = Nothing
            End If

        End Try
    End Sub

    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged
        Response.Redirect("editCommentCards.aspx?cardid=" & gridview1.selectedValue)
    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        Response.Redirect("editCommentCards.aspx?Cardid =0")
    End Sub
End Class
