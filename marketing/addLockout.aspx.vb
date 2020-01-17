
Partial Class marketing_addLockout
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oRoom As New clsRooms
            gvLockout.DataSource = oRoom.List_Lockouts(Request("RoomID"))
            gvLockout.DataBind()
            oRoom = Nothing
        End If
    End Sub

    Protected Sub gvLockout_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvLockout.SelectedIndexChanged
        Dim row As GridViewRow = gvLockout.SelectedRow
        Dim oRoom As New clsRooms
        oRoom.RoomID = Request("RoomID")
        oRoom.Load()
        oRoom.LockOutID = row.Cells(1).Text
        oRoom.Save()
        oRoom = Nothing
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.opener.Refresh_Lockout('" & row.Cells(2).text & "');window.close();", True)
    End Sub
End Class
