Imports System
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient

Partial Class marketing_Rooms
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        If CheckSecurity("Rooms", "List", , , Session("UserDBID")) Then
            Select Case LCase(ddFilter.Text)
                Case "room number"
                    If filter.text <> "" Then
                        Run_Query("select RoomID, RoomNumber from t_Room where roomnumber like '%" & filter.text & "%'")
                    Else
                        Run_Query("select RoomID, RoomNumber from t_Room")
                    End If
                Case "extension"
                    If filter.text <> "" Then
                        Run_Query("select RoomID, RoomNumber from t_Room where Phone like '%" & filter.text & "%'")
                    Else
                        Run_Query("select RoomID, RoomNumber from t_Room")
                    End If
                Case Else
                    lblErr.text = ddfilter.text
            End Select
        Else
            lblErr.Text = "ACCESS DENIED"
        End If
    End Sub

    Sub Run_Query(ByVal sSQL As String)
        Dim cn As New SqlConnection(Resources.Resource.cns)
        Dim cm As New SqlCommand("", cn)
        Dim dr As SqlDataReader

        Try
            cn.Open()
            cm.CommandText = sSQL
            dr = cm.ExecuteReader
            gridview1.datasource = dr
            Dim ka(0) As String
            ka(0) = "roomid"
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'If IsPostBack Then
        'Button1_Click(sender, e)
        'End If
        ddFilter_SelectedIndexChanged(sender, e)
    End Sub

    Protected Sub btnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Response.Redirect("editRoom.aspx?roomid=0")
    End Sub

    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged
        Response.Redirect("editRoom.aspx?roomid=" & gridview1.selectedValue)
    End Sub

    Protected Sub ddFilter_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddFilter.SelectedIndexChanged
        Label1.Text = "Enter a " & ddFilter.SelectedItem.Text
    End Sub
End Class
