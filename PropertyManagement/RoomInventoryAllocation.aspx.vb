
Partial Class PropertyManagement_RoomInventoryAllocation
    Inherits System.Web.UI.Page

    Protected Sub btnAllocate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAllocate.Click
        If lstRooms.Items.Count > 0 And dfSDate.Selected_Date <> "" And dfEDate.Selected_Date <> "" Then
            If CheckSecurity("Rooms", "AllocateNew", , , Session("UserDBID")) Then
                Dim iAllocations As Integer = 0
                For Each item As ListItem In lstRooms.Items
                    For i = 0 To DateDiff(DateInterval.Day, CDate(dfSDate.Selected_Date), CDate(dfEDate.Selected_Date))
                        Dim oRM As New clsRoomAllocationMatrix
                        If Not (oRM.Allocation_Exists(CDate(dfSDate.Selected_Date).AddDays(i), item.Value)) Then
                            oRM.RoomID = item.Value
                            oRM.DateAllocated = CDate(dfSDate.Selected_Date).AddDays(i)
                            oRM.UserID = Session("UserDBID")
                            oRM.Save()
                            iAllocations += 1
                        End If
                        oRM = Nothing
                        lit1.text = "Allocations: " & iAllocations
                    Next
                Next
            Else
                lit1.text = "Access Denied"
            End If
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oRooms As New clsRooms
            ddRooms.DataSource = oRooms.List_Rooms()
            ddRooms.DataTextField = "RoomNumber"
            ddRooms.DataValueField = "RoomID"
            ddRooms.DataBind()
            oRooms = Nothing
        End If
    End Sub

    Protected Sub btnAddRooms_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddRooms.Click

        Dim All = New List(Of ListItem)

        For Each li As ListItem In ddRooms.Items
            All.Add(li)
        Next

        For Each li As ListItem In lstRooms.Items
            All.Add(li)
        Next

        If cbAddAll.Checked Then
            ddRooms.Items.Clear()
            lstRooms.Items.Clear()
            For Each li In All
                lstRooms.Items.Add(li)
            Next
            Dim sorted = lstRooms.Items.OfType(Of ListItem).OrderBy(Function(x) Convert.ToInt32(x.Text.Replace("-", "").Replace("A", "1").Replace("B", "2").Replace("C", "3"))).ToList()
            lstRooms.Items.Clear()
            For Each li In sorted
                lstRooms.Items.Add(li)
            Next
            cbRemoveAll.Checked = False
        Else
            Dim selectedItem = ddRooms.SelectedItem
            lstRooms.Items.Add(selectedItem)            
            ddRooms.Items.Remove(selectedItem)
            ddRooms.ClearSelection()
            lstRooms.ClearSelection()

            Dim sorted = lstRooms.Items.OfType(Of ListItem).OrderBy(Function(x) Convert.ToInt32(x.Text.Replace("-", "").Replace("A", "1").Replace("B", "2").Replace("C", "2"))).ToList()
            lstRooms.Items.Clear()
            For Each li In sorted
                lstRooms.Items.Add(li)
            Next
        End If
    End Sub

    Protected Sub btnRemoveRoom_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemoveRoom.Click

        Dim All = New List(Of ListItem)

        For Each li As ListItem In ddRooms.Items
            All.Add(li)
        Next
        For Each li As ListItem In lstRooms.Items
            All.Add(li)
        Next

        If cbRemoveAll.Checked Then
            ddRooms.Items.Clear()
            lstRooms.Items.Clear()

            For Each li In All
                ddRooms.Items.Add(li)
            Next
            Dim sorted = ddRooms.Items.OfType(Of ListItem).OrderBy(Function(x) Convert.ToInt32(x.Text.Replace("-", "").Replace("A", "1").Replace("B", "2").Replace("C", "3"))).ToList()
            ddRooms.Items.Clear()
            For Each li In sorted
                ddRooms.Items.Add(li)
            Next
            cbAddAll.Checked = False
        Else

            Dim selectedItem = lstRooms.SelectedItem
            ddRooms.Items.Add(selectedItem)
            lstRooms.Items.Remove(selectedItem)
            ddRooms.ClearSelection()
            lstRooms.ClearSelection()

            Dim sorted = ddRooms.Items.OfType(Of ListItem).OrderBy(Function(x) Convert.ToInt32(x.Text.Replace("-", "").Replace("A", "1").Replace("B", "2").Replace("C", "3"))).ToList()
            ddRooms.Items.Clear()
            For Each li In sorted
                ddRooms.Items.Add(li)
            Next
        End If

    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        lstRooms.Items.Clear()
        dfSDate.Selected_Date = ""
        dfEDate.Selected_Date = ""
    End Sub
End Class
