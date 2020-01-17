
Partial Class PropertyManagement_RoomUsage
    Inherits System.Web.UI.Page



    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        Dim oRoom As New clsRooms
        gvRoomUsage.DataSource = oRoom.Room_Usage(ddRooms.SelectedValue, dteSDate.Selected_Date, dteEDate.Selected_Date)
        gvRoomUsage.DataBind()
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim oRoom As New clsRooms
            ddRooms.dataSource = oRoom.List_Rooms()
            ddRooms.DataTextField = "RoomNumber"
            ddRooms.DataValueField = "RoomID"
            ddRooms.DataBind()
            oRoom = Nothing
        End If
    End Sub

    Protected Sub gvRoomUsage_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then
            If e.Row.RowIndex > -1 Then
                e.Row.Cells(3).Text = CDate(e.Row.Cells(3).Text).ToShortDateString
                If Not (IsDBNull(e.Row.Cells(1).Text)) And e.Row.Cells(1).Text <> "0" Then
                    e.Row.Cells(1).Text = "<a href = '../marketing/editReservation.aspx?ReservationID=" & e.Row.Cells(1).Text & "'>" & e.Row.Cells(1).Text & "</a>"
                Else
                    e.Row.Cells(1).Text = ""
                End If
                If Not (IsDBNull(e.Row.Cells(2).Text)) And e.Row.Cells(2).Text <> "0" Then
                    e.Row.Cells(4).Text = "<a href = '../marketing/editContract.aspx?ContractID=" & e.Row.Cells(2).Text & "&UsageID=" & e.Row.Cells(5).Text & "'>" & e.Row.Cells(4).Text & "</a>"
                End If
            End If
            e.Row.Cells(2).Visible = False
            e.Row.Cells(5).Visible = False
        End If
    End Sub

End Class
