
Partial Class Maintenance_EditQuickCheck
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Fill()
            If Request("id") <> "0" And Request("id") <> "" Then
                lbQuickCheckID.Text = Request("ID")
                Dim oQC As New clsMaintQuickCheck
                oQC.QuickCheckID = Request("id")
                oQC.Load()
                lbQuickCheckID.Text = oQC.QuickCheckID
                txtName.Text = oQC.Name
                txtDescription.Text = oQC.Description
                siRoomType.Selected_ID = oQC.RoomTypeID
                siUnitType.Selected_ID = oQC.UnitTypeID
                siUnitStyle.Selected_ID = oQC.UnitStyleID
                ckActive.Checked = oQC.Active
                oQC = Nothing
                Load_GridView()
            End If
        End If
    End Sub

    Private Sub Fill()
        siRoomType.Connection_String = Resources.Resource.cns
        siRoomType.ComboItem = "RoomType"
        siRoomType.Label_Caption = ""
        siRoomType.Load_Items()
        siUnitType.Connection_String = Resources.Resource.cns
        siUnitType.ComboItem = "UnitType"
        siUnitType.Label_Caption = ""
        siUnitType.Load_Items()
        siUnitStyle.Connection_String = Resources.Resource.cns
        siUnitStyle.ComboItem = "UnitStyle"
        siUnitStyle.Label_Caption = ""
        siUnitStyle.Load_Items()
        siAreas.Connection_String = Resources.Resource.cns
        siAreas.ComboItem = "QuickCheckAreas"
        siAreas.Label_Caption = ""
        siAreas.Load_Items()
        Load_Items()
    End Sub

    Private Sub Load_Items()
        ddItems.DataSource = (New clsQuickCheckItem).List
        ddItems.DataTextField = "ItemAreaDescription"
        ddItems.DataValueField = "QuickCheckItemID"
        ddItems.DataBind()
    End Sub

    Private Sub Load_GridView()
        gvItems.DataSource = (New clsQuickCheck2Item).List_Quick_Check_Items(lbQuickCheckID.Text)
        gvItems.DataBind()
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim oQC As New clsMaintQuickCheck
        With oQC
            .QuickCheckID = lbQuickCheckID.Text
            .Load()
            .Name = txtName.Text
            .Description = txtDescription.Text
            .RoomTypeID = siRoomType.Selected_ID
            .UnitTypeID = siUnitType.Selected_ID
            .UnitStyleID = siUnitStyle.Selected_ID
            .Active = ckActive.Checked
            .Save()
            lbQuickCheckID.Text = .QuickCheckID
        End With
        oQC = Nothing
    End Sub
    Protected Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Response.Redirect("QuickChecks.aspx")
    End Sub
    Protected Sub btnAddExisting_Click(sender As Object, e As EventArgs) Handles btnAddExisting.Click
        If ddItems.Items.Count > 0 Then
            If lbQuickCheckID.Text <> "0" And ddItems.SelectedValue > 0 Then
                Dim oItem As New clsQuickCheck2Item
                oItem.QuickCheck2ItemID = 0
                oItem.Load()
                oItem.QuickCheckID = lbQuickCheckID.Text
                oItem.QuickCheckItemID = ddItems.SelectedValue
                oItem.Save()
                oItem = Nothing
                Load_GridView()
            End If
        End If
    End Sub
    Protected Sub btnAddNewItem_Click(sender As Object, e As EventArgs) Handles btnAddNewItem.Click
        If lbQuickCheckID.Text <> "0" And siAreas.Selected_ID > 0 And txtItemDescription.Text <> "" Then
            Dim oItem As New clsQuickCheckItem
            With oItem
                .Active = True
                .AreaID = siAreas.Selected_ID
                .Description = txtItemDescription.Text
                .Save()
            End With
            Dim oMap As New clsQuickCheck2Item
            With oMap
                .QuickCheckID = lbQuickCheckID.Text
                .QuickCheckItemID = oItem.QuickCheckItemID
                .Save()
            End With
            oItem = Nothing
            oMap = Nothing
            Load_Items()
            Load_GridView()
        End If
    End Sub

    Private Sub gvItems_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvItems.RowCommand
        If e.CommandName = "Remove" Then
            Dim row As GridViewRow = gvItems.Rows(Convert.ToInt32(e.CommandArgument))
            With New clsQuickCheck2Item
                .Remove_Item(row.Cells(1).Text, lbQuickCheckID.Text)
            End With
            Load_GridView()
        End If
    End Sub
End Class
