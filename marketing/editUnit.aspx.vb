Imports System
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient

Partial Class marketing_editUnit
    Inherits System.Web.UI.Page

    Dim oUnit As New clsUnit
    Dim oRoom As New clsRooms

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            If CheckSecurity("Units", "View", , , Session("UserDBID")) Then
                MultiView1.ActiveViewIndex = 0
                Load_SIs()
                oUnit.UnitID = IIf(IsNumeric(Request("UnitID")), CInt(Request("UnitID")), 0)
                oUnit.Load()
                oRoom.Load()
                Set_Values()
            Else
                MultiView1.ActiveViewIndex = -1
                txtUnitID.Text = -1
            End If
        End If
    End Sub

    Private Sub Load_SIs()
        siUStatus.Connection_String = Resources.Resource.cns
        siUStatus.ComboItem = "UnitStatus"
        siUStatus.Label_Caption = ""
        siUStatus.Load_Items()
        'siUStatus.Label_Caption = ""
        siUStatus.Load_Items()
        siUType.Connection_String = Resources.Resource.cns
        siUType.ComboItem = "UnitType"
        siUType.Label_Caption = ""
        siUType.Load_Items()
        siUSubType.Connection_String = Resources.Resource.cns
        siUSubType.ComboItem = "UnitSubType"
        siUSubType.Label_Caption = ""
        siUSubType.Load_Items()
        siUStyle.Connection_String = Resources.Resource.cns
        siUStyle.ComboItem = "UnitStyle"
        siUStyle.Label_Caption = ""
        siUStyle.Load_Items()
        siState.Connection_String = Resources.Resource.cns
        sistate.Comboitem = "State"
        siState.Label_Caption = ""
        siState.Load_Items()
    End Sub

    Private Sub Set_Values()
        txtRoomID.Text = oRoom.RoomID
        txtUnitId.text = oUnit.UnitID
        txtUname.Text = oUnit.Name
        txtUAddress.Text = oUnit.Address1
        txtUCity.Text = oUnit.City
        txtUZip.Text = oUnit.Zip
        siUType.Selected_ID = oUnit.TypeID
        siUSubType.Selected_ID = oUnit.SubTypeID
        siUStatus.Selected_ID = oUnit.StatusID
        siState.Selected_ID = oUnit.StateID
        siUStyle.Selected_ID = oUnit.StyleID

    End Sub

    Protected Sub Units_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Units_Link.Click
        If txtUnitID.Text > 0 Then
            MultiView1.ActiveViewIndex = 0
        End If
    End Sub

    Protected Sub Events_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Events_Link.Click
        If txtUnitID.Text > 0 Then
            MultiView1.ActiveViewIndex = 4
            Events1.KeyField = "UnitID"
            Events1.KeyValue = txtUnitID.Text
            Events1.List()
        End If
    End Sub

    Protected Sub SalesInventory_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SalesInventory_Link.Click
        If txtUnitID.Text > 0 Then
            MultiView1.ActiveViewIndex = 1
            Dim oInventory As New clsSalesInventory
            gvSalesInventory.datasource = oInventory.List_Inventory(txtUnitID.Text)
            gvSalesInventory.databind()
            oInventory = Nothing
        End If
    End Sub

    Protected Sub Rooms_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Rooms_Link.Click
        If txtUnitID.Text > 0 Then
            MultiView1.ActiveViewIndex = 2
            Dim ds As New sqldataSource
            ds.ConnectionString = Resources.Resource.cns
            ds.SelectCommand = "Select RoomID, RoomNumber from t_room where unitid =" & txtUnitID.Text
            gvRooms.datasource = ds
            gvRooms.databind()
        End If
    End Sub

    Protected Sub UnitTies_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UnitTies_Link.Click
        If txtUnitID.Text > 0 Then
            Dim oUnitTie As New clsUnit2Unit
            gvUnitTies.DataSource = oUnitTie.List_UnitTies(txtUnitID.Text)
            Dim sKeys(0) As String
            sKeys(0) = "Unit2UnitID"
            gvUnitTies.DataKeyNames = sKeys
            gvUnitTies.DataBind()
            MultiView1.ActiveViewIndex = 3
        End If
    End Sub

    Private Sub Save_Values()
        With oUnit
            .UnitID = txtUnitId.text
            .Name = txtUname.Text
            .Address1 = txtUAddress.Text
            .City = txtUCity.Text
            .Zip = txtUZip.Text
            .TypeID = siUType.Selected_ID
            .SubTypeID = siUSubType.Selected_ID
            .StatusID = siUStatus.Selected_ID
            .StateID = siState.Selected_ID
            .StyleID = siUStyle.Selected_ID()
            .BudgetedPrice = CDbl(txtBudgetedPrice.Text)
            .Save()
            If txtUnitID.Text = 0 Then
                Response.Redirect("editUnit.aspx?UnitID=" & oUnit.UnitID)
            Else
                Set_Values()
            End If
        End With
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        If txtUnitID.Text = 0 Then
            If CheckSecurity("Units", "Add", , , Session("UserDBID")) Then
                Save_Values()
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('You Do Not Have Persmission To Add Units.');", True)
            End If
        Else
            If CheckSecurity("Units", "Edit", , , Session("UserDBID")) Then
                Save_Values()
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('You Do Not Have Persmission To Edit Units.');", True)
            End If
        End If
    End Sub

    Protected Sub gvRooms_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvRooms.SelectedIndexChanged
        Dim row As gridviewrow = gvRooms.selectedRow
        Response.Redirect("editRoom.aspx?roomid=" & row.cells(1).Text)
    End Sub

    Protected Sub gvSalesInventory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvSalesInventory.SelectedIndexChanged
        Dim row As gridviewrow = gvSalesInventory.selectedRow
        Response.Redirect("editSalesInventory.aspx?salesinventoryid=" & row.cells(1).Text)
    End Sub

    Protected Sub gvUnitTies_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvUnitTies.RowCommand
        Dim ID As Integer
        ID = Convert.ToInt32(gvUnitTies.DataKeys(Convert.ToInt32(e.CommandArgument)).Value)
        If e.CommandName.CompareTo("Remove") = 0 Then
            Dim oUnitTie As New clsUnit2Unit
            oUnitTie.Remove_Tie(ID)
            gvUnitTies.DataSource = oUnitTie.List_UnitTies(txtUnitID.Text)
            Dim sKeys(0) As String
            sKeys(0) = "Unit2UnitID"
            gvUnitTies.DataKeyNames = sKeys
            gvUnitTies.DataBind()
        End If
    End Sub

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/marketing/addUnitTie.aspx?UnitID=" & txtUnitID.Text & "','win01',350,350);", True)
    End Sub

    Protected Sub Unnamed1_Click1(sender As Object, e As System.EventArgs)
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/marketing/addSalesInventory.aspx?UnitID=" & txtUnitID.Text & "','win01',350,350);", True)
    End Sub

    Protected Sub btnAddRoom_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddRoom.Click
        Response.Redirect("EditRoom.aspx?roomid=0&UnitID=" & txtUnitID.Text)
    End Sub
End Class
