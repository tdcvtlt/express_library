
Partial Class Maintenance_EditRefurb
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Fill()
            If Request("id") <> "0" And Request("id") <> "" Then
                lbRefurbID.Text = Request("ID")
                Dim oQC As New clsRefurb
                oQC.RefurbID = Request("id")
                oQC.Load()
                lbRefurbID.Text = oQC.RefurbID
                txtName.Text = oQC.Name
                txtDescription.Text = oQC.Description
                siRoomType.Selected_ID = oQC.RoomTypeID
                siUnitType.Selected_ID = oQC.UnitTypeID
                siUnitStyle.Selected_ID = oQC.UnitStyleID
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
        siAreas.ComboItem = "RefurbAreas"
        siAreas.Label_Caption = ""
        siAreas.Load_Items()
        Load_Items()
    End Sub

    Private Sub Load_Items()
        ddItems.DataSource = (New clsRefurbItem).List
        ddItems.DataTextField = "ItemAreaDescription"
        ddItems.DataValueField = "RefurbItemID"
        ddItems.DataBind()
    End Sub

    Private Sub Load_GridView()
        gvItems.DataSource = (New clsRefurb2Item).List_Refurb_Items(lbRefurbID.Text)
        gvItems.DataBind()
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim oQC As New clsRefurb
        With oQC
            .RefurbID = lbRefurbID.Text
            .Load()
            .Name = txtName.Text
            .Description = txtDescription.Text
            .RoomTypeID = siRoomType.Selected_ID
            .UnitTypeID = siUnitType.Selected_ID
            .UnitStyleID = siUnitStyle.Selected_ID
            .Save()
            lbRefurbID.Text = .RefurbID
        End With
        oQC = Nothing
    End Sub
    Protected Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Response.Redirect("Refurbs.aspx")
    End Sub
    Protected Sub btnAddExisting_Click(sender As Object, e As EventArgs) Handles btnAddExisting.Click
        If ddItems.Items.Count > 0 Then
            If lbRefurbID.Text <> "0" And ddItems.SelectedValue > 0 Then
                Dim oItem As New clsRefurb2Item
                oItem.RefurbID = lbRefurbID.Text
                oItem.RefurbItemID = ddItems.SelectedValue
                oItem.Save()
                oItem = Nothing
                Load_GridView()
            End If
        End If
    End Sub
    Protected Sub btnAddNewItem_Click(sender As Object, e As EventArgs) Handles btnAddNewItem.Click
        If lbRefurbID.Text <> "0" And siAreas.Selected_ID > 0 And txtItemDescription.Text <> "" Then
            Dim oItem As New clsRefurbItem
            With oItem
                .Active = True
                .AreaID = siAreas.Selected_ID
                .Description = txtItemDescription.Text
                .Save()
            End With
            Dim oMap As New clsRefurb2Item
            With oMap
                .RefurbID = lbRefurbID.Text
                .RefurbItemID = oItem.RefurbItemID
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
            With New clsRefurb2Item
                .Remove_Item(row.Cells(1).Text, lbRefurbID.Text)
            End With
            Load_GridView()
        End If
    End Sub
End Class
