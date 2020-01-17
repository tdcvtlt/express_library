
Partial Class PropertyManagement_BulkRoom2Rep
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oRequest As New clsRequest
            ddTech.DataSource = oRequest.List_Maint_Reps("KCP", oRequest.AssignedToID)
            ddTech.DataValueField = "PersonnelID"
            ddTech.DataTextField = "Personnel"
            ddTech.DataBind()
            ddRemoveTech.DataSource = oRequest.List_Maint_Reps("KCP", oRequest.AssignedToID)
            ddRemoveTech.DataValueField = "PersonnelID"
            ddRemoveTech.DataTextField = "Personnel"
            ddRemoveTech.DataBind()
            oRequest = Nothing
            Dim oRoom As New clsRooms
            lbRooms.DataSource = oRoom.List_Rooms()
            lbRooms.DataTextField = "RoomNumber"
            lbRooms.DataValueField = "RoomID"
            lbRooms.DataBind()
            oRoom = Nothing
            Dim oRoom2Maint As New clsRoom2MaintTech
            gvTechRooms.DataSource = oRoom2Maint.List_Assigned_Rooms(ddRemoveTech.SelectedValue)
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvTechRooms.DataKeyNames = sKeys
            gvTechRooms.DataBind()
            oRoom2Maint = Nothing
            MultiView1.ActiveViewIndex = 0
        End If
    End Sub
    Protected Sub btnAllRooms_Click(sender As Object, e As EventArgs) Handles btnAllRooms.Click
        For i = lbRooms.Items.Count - 1 To 0 Step -1
            lbRoomsAdded.ClearSelection()
            lbRoomsAdded.Items.Add(lbRooms.Items(i))
            lbRooms.Items.Remove(lbRooms.Items(i))
            lbRoomsAdded.SelectedIndex = 0
        Next
    End Sub
    Protected Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If lbRooms.SelectedIndex >= 0 Then
            Dim sel() As Integer
            sel = lbRooms.GetSelectedIndices
            lbRoomsAdded.ClearSelection()
            For i = 0 To sel.Count - 1
                lbRoomsAdded.Items.Add(lbRooms.Items(sel(i)))
            Next i
            Dim j As Integer = 0
            For i = 0 To sel.Count - 1
                lbRooms.Items.Remove(lbRooms.Items(sel(i) - j))
                j = j + 1
            Next
        End If
    End Sub
    Protected Sub btnRemoveAll_Click(sender As Object, e As EventArgs) Handles btnRemoveAll.Click
        For i = lbRoomsAdded.Items.Count - 1 To 0 Step -1
            lbRooms.ClearSelection()
            lbRooms.Items.Add(lbRoomsAdded.Items(i))
            lbRoomsAdded.Items.Remove(lbRoomsAdded.Items(i))
        Next
    End Sub
    Protected Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
        If lbRoomsAdded.SelectedIndex >= 0 Then
            lbRooms.ClearSelection()
            lbRooms.Items.Add(lbRoomsAdded.SelectedItem)
            lbRoomsAdded.Items.Remove(lbRoomsAdded.SelectedItem)
        End If
    End Sub
    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        If CheckSecurity("Rooms", "AssignTech",,, Session("UserDBID")) Then
            If lbRoomsAdded.Items.Count = 0 Then
            Else
                Dim oRoom2Maint As New clsRoom2MaintTech
                For i = 0 To lbRoomsAdded.Items.Count - 1
                    If Not (oRoom2Maint.chk_Exist(ddTech.SelectedValue, lbRoomsAdded.Items(i).Value)) Then
                        oRoom2Maint.ID = 0
                        oRoom2Maint.Load()
                        oRoom2Maint.UserID = Session("UserDBID")
                        oRoom2Maint.RoomID = lbRoomsAdded.Items(i).Value
                        oRoom2Maint.RepID = ddTech.SelectedValue
                        oRoom2Maint.ExpirationDate = dfExpirationDate.Selected_Date
                        oRoom2Maint.Save()
                    End If
                Next i
                oRoom2Maint = Nothing
            End If
            Response.Redirect("BulkRoom2Rep.aspx")
        Else
            litError.Text = "ACCESS DENIED"
        End If
    End Sub
    Protected Sub Add_Link_Click(sender As Object, e As EventArgs) Handles Add_Link.Click
        MultiView1.ActiveViewIndex = 0
    End Sub
    Protected Sub Remove_Link_Click(sender As Object, e As EventArgs) Handles Remove_Link.Click
        MultiView1.ActiveViewIndex = 1
    End Sub
    Protected Sub ddRemoveTech_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddRemoveTech.SelectedIndexChanged
        Dim oRoom2Maint As New clsRoom2MaintTech
        gvTechRooms.DataSource = oRoom2Maint.List_Assigned_Rooms(ddRemoveTech.SelectedValue)
        Dim sKeys(0) As String
        sKeys(0) = "ID"
        gvTechRooms.DataKeyNames = sKeys
        gvTechRooms.DataBind()
        oRoom2Maint = Nothing
    End Sub
    Protected Sub btnRemoveRoom_Click(sender As Object, e As EventArgs) Handles btnRemoveRoom.Click
        If CheckSecurity("Rooms", "AssignTech",,, Session("UserDBID")) Then
            For Each row As GridViewRow In gvTechRooms.Rows
                Dim cb As CheckBox = row.FindControl("RoomSelect")
                If cb.Checked Then
                    Dim oRoom2Tech As New clsRoom2MaintTech
                    oRoom2Tech.Delete_Tie(row.Cells(1).Text)
                    oRoom2Tech = Nothing
                End If
            Next
            Dim oRoom2Maint As New clsRoom2MaintTech
            gvTechRooms.DataSource = oRoom2Maint.List_Assigned_Rooms(ddRemoveTech.SelectedValue)
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvTechRooms.DataKeyNames = sKeys
            gvTechRooms.DataBind()
            oRoom2Maint = Nothing
        Else
            litError.Text = "ACCESS DENIED"
        End If
    End Sub
    Protected Sub gvTechRooms_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then
            'If e.Row.RowIndex > -1 Then
            e.Row.Cells(1).Visible = False
            'End If
        End If
    End Sub
End Class
