
Partial Class PropertyManagement_Projects_AddRoom
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            ddRooms.DataSource = (New clsRooms).List_Rooms
            ddRooms.DataValueField = "RoomID"
            ddRooms.DataTextField = "RoomNumber"
            ddRooms.DataBind()
        End If
    End Sub

    Protected Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Dim items() As Integer = ddRooms.GetSelectedIndices
        If items.Length > 0 Then
            For i = 0 To items.Length - 1
                If ddRooms.Items(i).Value > 0 Then
                    Dim opr As New clsProject2Room
                    opr.ProjectRoomID = 0
                    opr.Load()
                    opr.RoomID = ddRooms.Items(i).Value
                    opr.ProjectID = Request("ID")
                    opr.Save()
                    opr = Nothing
                End If
            Next
            ClientScript.RegisterClientScriptBlock(Me.GetType(), "Refresh", "window.opener.Refresh_Rooms();", True)
            Close()
        End If

    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Close()
    End Sub

    Private Sub Close()
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "Close", "window.close();", True)
    End Sub
End Class
