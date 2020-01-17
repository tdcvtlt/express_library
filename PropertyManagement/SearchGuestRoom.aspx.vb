Imports System
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Web

Partial Class PropertyManagement_SearchGuestRoom
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            If Not Session("RoomSet") Is Nothing Then
                gridview1.datasource = Session("RoomSet")
                gridview1.databind()
            End If
        End If
    End Sub

    Private Sub Get_List(ByRef sSQL As String, ByRef Param As String)
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand(sSQL, cn)
        Dim rs As SqlDataReader
        cn.Open()
        rs = cm.ExecuteReader
    End Sub

    'Protected Sub btnGuest_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGuest.Click

    'End Sub

    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged
        Dim row As GridViewRow = GridView1.SelectedRow
        'Page.RegisterStartupScript("alert", "<script language='javascript'>Get_Window('" & row.cells(3).Text & "');</script>")
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.opener.Refresh_Rpt('" & row.Cells(3).Text & "', '" & row.Cells(4).Text & "', '" & row.Cells(5).Text & "'," & row.Cells(1).Text & ", " & row.Cells(2).Text & ");window.close();", True)

    End Sub
End Class
