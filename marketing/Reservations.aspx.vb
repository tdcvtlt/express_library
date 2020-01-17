Imports System.Data.Sql
Imports System.Data.SqlClient
Partial Class marketing_Reservations
    Inherits System.Web.UI.Page

    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvReservations.SelectedIndexChanged
        Dim row As gridviewrow = gvReservations.SelectedRow
        Response.Redirect("editReservation.aspx?reservationid=" & row.Cells(1).text)
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        If CheckSecurity("Reservations", "List", , , Session("UserDBID")) Then
            Dim res As New clsReservations
            gvReservations.DataSource = res.Search(txtFilter.text, ddFilter.selectedvalue)
            gvReservations.DataBind()
            res = Nothing
        Else
            lblErr.Text = "ACESS DENIED"
        End If
    End Sub

    Protected Sub gvReservations_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then
            If e.Row.RowIndex > -1 Then
                If Trim(e.Row.Cells(3).Text & "") <> "" Then
                    Dim sDate() As String = e.Row.Cells(3).Text.Split(" ")
                    e.Row.Cells(3).Text = sDate(0) 'e.Row.Cells(3).Text & " HI" ' CDate(Trim(e.Row.Cells(3).Text)).ToShortDateString
                End If
                If Trim(e.Row.Cells(4).Text & "") <> "" Then
                    Dim sDate() As String = e.Row.Cells(4).Text.Split(" ")
                    e.Row.Cells(4).Text = sDate(0) 'e.Row.Cells(3).Text & " HI" ' CDate(Trim(e.Row.Cells(3).Text)).ToShortDateString
                End If
            End If
        End If
    End Sub
End Class
