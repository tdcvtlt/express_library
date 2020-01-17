Imports System
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Web.Services
Imports System.Web.Script.Serialization

Partial Class marketing_EditRoom
    Inherits System.Web.UI.Page

    Dim oUnit As New clsUnit
    Dim oRoom As New clsRooms
    Dim oLockOut As New clsRooms
    Dim oCRMS As New clsRooms

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            MultiView1.ActiveViewIndex = 0
            Load_SIs()
            oRoom.RoomID = IIf(IsNumeric(Request("RoomID")), CInt(Request("RoomID")), 0)
            oRoom.Load()
            If Request("RoomID") = 0 Then
                ddUnits.DataSource = oUnit.List_Units()
                ddUnits.DataTextField = "Name"
                ddUnits.DataValueField = "UnitID"
                ddUnits.DataBind()
                If IsNumeric(Request("UnitID")) And Request("UnitID") <> "" Then ddUnits.SelectedValue = Request("UnitID")
            End If
            oUnit.UnitID = IIf(oRoom.UnitID > 0, oRoom.UnitID, IIf(IsNumeric(Request("UnitID")) And Request("UnitID") <> "", Request("UnitID"), 0))
            oUnit.Load()
            'oCRMS.CRMSID = oRoom.CRMSID
            'oCRMS.Load()
            'Set_Values()
            oLockOut.RoomID = oRoom.LockOutID
            oLockOut.Load()
            Set_Values()
        End If
    End Sub

    Private Sub Load_SIs()
        'siStatus.Connection_String = Resources.Resource.cns
        'siStatus.ComboItem = "RoomStatus"
        'siStatus.Label_Caption = "Room Status:"
        'siStatus.Load_Items()
        'siUStatus.Label_Caption = ""
        'siStatus.Load_Items()
        siType.Connection_String = Resources.Resource.cns
        siType.ComboItem = "RoomType"
        siType.Label_Caption = "" '"Room Type:"
        siType.Load_Items()
        siSubType.Connection_String = Resources.Resource.cns
        siSubType.ComboItem = "RoomSubType"
        siSubType.Label_Caption = "" ' "Room Sub Type:"
        siSubType.Load_Items()
        siMaintStatus.Connection_String = Resources.Resource.cns
        siMaintStatus.ComboItem = "RoomMaintenanceStatus "
        siMaintStatus.Label_Caption = "" '"Maint. Status:"
        siMaintStatus.Load_Items()
        siHKStatus.Connection_String = Resources.Resource.cns
        siHKStatus.ComboItem = "RoomHousekeepingStatus "
        siHKStatus.Label_Caption = "" '"HK. Status:"
        siHKStatus.Load_Items()
    End Sub

    Private Sub Get_UnitName()

    End Sub

    Private Sub Set_Values()
        txtRoomID.Text = oRoom.RoomID
        txtRoomNumber.Text = oRoom.RoomNumber
        If Request("RoomID") = 0 Then
        Else
            txtUnit.Text = oUnit.Name
        End If
        txtLockOut.Text = oLockOut.RoomNumber
        'txtStatusDate.Text = oRoom.StatusDate
        txtExtension.Text = oRoom.Phone
        txtMaxOcc.Text = oRoom.MaxOccupancy
        txtMaintStatusDate.Text = oRoom.MaintenanceStatusDate
        txtCRMSID.Text = oRoom.CRMSID & " " & oRoom.Err
        siType.Selected_ID = oRoom.TypeID
        siSubType.Selected_ID = oRoom.SubTypeID
        'siStatus.Selected_ID = oRoom.StatusID
        siMaintStatus.Selected_ID = oRoom.MaintenanceStatusID
        siHKStatus.Selected_ID = oRoom.HouseKeepingStatusID
        txtHKStatusDate.Text = oRoom.HouseKeepingStatusDate
    End Sub

    Protected Sub Rooms_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Rooms_Link.Click
        MultiView1.ActiveViewIndex = 0
    End Sub

    Protected Sub Events_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Events_Link.Click
        If txtRoomID.Text > 0 Then
            MultiView1.ActiveViewIndex = 5
            Events1.KeyField = "RoomID"
            Events1.KeyValue = txtRoomID.Text
            Events1.List()
        End If
    End Sub

    Protected Sub MaintRequest_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles MaintRequest_Link.Click
        If txtRoomID.Text > 0 Then
            Dim oReq As New clsRequest
            MultiView1.ActiveViewIndex = 1
            gvMaintRequests.DataSource = oReq.List_maint_Requests(txtRoomID.Text)
            gvMaintRequests.DataBind()
            oReq = Nothing
        End If
    End Sub

    Protected Sub Amenities_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Amenities_Link.Click
        If txtRoomID.Text > 0 Then
            MultiView1.ActiveViewIndex = 2
        End If
    End Sub

    Protected Sub UserFields_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles UserFields_Link.Click
        If txtRoomID.Text > 0 Then
            MultiView1.ActiveViewIndex = 3
            UF.KeyField = "Room"
            UF.KeyValue = CInt(txtRoomID.Text)
            UF.Load_List()
        End If
    End Sub

    Protected Sub Notes_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Notes_Link.Click
        If txtRoomID.Text > 0 Then
            MultiView1.ActiveViewIndex = 4
            Notes1.KeyValue = txtRoomID.Text
            Notes1.Display()
        End If
    End Sub

    Private Sub Save_Values()
        Dim oComboItems As New clsComboItems
        Dim bProceed As Boolean = True
        Dim sErr As String = ""

        If txtRoomID.Text = 0 Then
            If Not (CheckSecurity("Rooms", "Edit", , , Session("UserDBID"))) Then
                bProceed = False
                sErr = "You Do Not Have Permission To Edit Rooms."
            Else
                oRoom.RoomID = 0
                oRoom.UserID = Session("UserDBID")
                oRoom.Load()
                'oRoom.StatusID = siStatus.Selected_ID
                oRoom.UnitID = ddUnits.SelectedValue
                If siMaintStatus.Selected_ID <> oRoom.MaintenanceStatusID Then oRoom.MaintenanceStatusDate = Date.Now
                oRoom.MaintenanceStatusID = siMaintStatus.Selected_ID
                If oRoom.HouseKeepingStatusID <> siHKStatus.Selected_ID Then oRoom.HouseKeepingStatusDate = Date.Now
                oRoom.HouseKeepingStatusID = siHKStatus.Selected_ID
                oRoom.RoomNumber = txtRoomNumber.Text
                oRoom.TypeID = siType.Selected_ID
                oRoom.SubTypeID = siSubType.Selected_ID
                oRoom.Phone = txtExtension.Text
                oRoom.MaxOccupancy = txtMaxOcc.Text
                oRoom.Save()
                Response.Redirect("EditRoom.aspx?RoomID=" & oRoom.RoomID)
            End If
        Else
            Dim ihGuest As Boolean = False
            If Not (CheckSecurity("Rooms", "Edit", , , Session("UserDBID"))) And Not (CheckSecurity("Rooms", "EditStatus", , , Session("UserDBID"))) And Not (CheckSecurity("Rooms", "EditMaintStatus", , , Session("UserDBID"))) Then
                bProceed = False
                sErr = "You Do Not Have Permission To Edit Rooms."
            ElseIf CheckSecurity("Rooms", "Edit", , , Session("userDBID")) Then

                oRoom.RoomID = txtRoomID.Text
                oRoom.UserID = Session("UserDBID")
                oRoom.Load()
                'oRoom.StatusID = siStatus.Selected_ID
                If siMaintStatus.Selected_ID <> oRoom.MaintenanceStatusID Then oRoom.MaintenanceStatusDate = Date.Now
                oRoom.MaintenanceStatusID = siMaintStatus.Selected_ID
                If oRoom.HouseKeepingStatusID <> siHKStatus.Selected_ID Then oRoom.HouseKeepingStatusDate = Date.Now
                If oComboItems.Lookup_ComboItem(oRoom.HouseKeepingStatusID) <> "Cleaned" And oComboItems.Lookup_ComboItem(siHKStatus.Selected_ID) = "Cleaned" And oRoom.Check_InHouse_Res(oRoom.RoomID) Then
                    ihGuest = True
                End If
                oRoom.HouseKeepingStatusID = siHKStatus.Selected_ID
                oRoom.RoomNumber = txtRoomNumber.Text
                oRoom.TypeID = siType.Selected_ID
                oRoom.SubTypeID = siSubType.Selected_ID
                oRoom.Phone = txtExtension.Text
                oRoom.MaxOccupancy = txtMaxOcc.Text
                oRoom.Save()
                If ihGuest Then
                    oRoom.HouseKeepingStatusID = oComboItems.Lookup_ID("RoomHousekeepingStatus", "Dirty")
                    oRoom.HouseKeepingStatusDate = Date.Now
                    oRoom.Save()
                End If
                Set_Values()
            ElseIf CheckSecurity("Rooms", "EditmaintStatus", , , Session("UserDBID")) And CheckSecurity("Rooms", "EditStatus", , , Session("UserDBID")) Then
                oRoom.RoomID = txtRoomID.Text
                oRoom.UserID = Session("UserDBID")
                oRoom.Load()
                'oRoom.StatusID = siStatus.Selected_ID
                oRoom.MaintenanceStatusID = siMaintStatus.Selected_ID
                If oComboItems.Lookup_ComboItem(oRoom.HouseKeepingStatusID) <> "Cleaned" And oComboItems.Lookup_ComboItem(siHKStatus.Selected_ID) = "Cleaned" And oRoom.Check_InHouse_Res(oRoom.RoomID) Then
                    ihGuest = True
                End If
                oRoom.HouseKeepingStatusID = siHKStatus.Selected_ID
                oRoom.Save()
                If ihGuest Then
                    oRoom.HouseKeepingStatusID = oComboItems.Lookup_ID("RoomHousekeepingStatus", "Dirty")
                    oRoom.HouseKeepingStatusDate = Date.Now
                    oRoom.Save()
                End If
                Set_Values()
            ElseIf CheckSecurity("Rooms", "EditStatus", , , Session("UserDBID")) Then
                oRoom.RoomID = txtRoomID.Text
                oRoom.UserID = Session("UserDBID")
                oRoom.Load()
                'oRoom.StatusID = siStatus.Selected_ID
                If oComboItems.Lookup_ComboItem(oRoom.HouseKeepingStatusID) <> "Cleaned" And oComboItems.Lookup_ComboItem(siHKStatus.Selected_ID) = "Cleaned" And oRoom.Check_InHouse_Res(oRoom.RoomID) Then
                    ihGuest = True
                End If
                oRoom.HouseKeepingStatusID = siHKStatus.Selected_ID
                oRoom.Save()
                If ihGuest Then
                    oRoom.HouseKeepingStatusID = oComboItems.Lookup_ID("RoomHousekeepingStatus", "Dirty")
                    oRoom.HouseKeepingStatusDate = Date.Now
                    oRoom.Save()
                End If
                Set_Values()
            ElseIf CheckSecurity("Rooms", "EditMaintStatus", , , Session("userDBID")) Then
                oRoom.RoomID = txtRoomID.Text
                oRoom.UserID = Session("UserDBID")
                oRoom.Load()
                oRoom.MaintenanceStatusID = siMaintStatus.Selected_ID
                If oComboItems.Lookup_ComboItem(oRoom.HouseKeepingStatusID) <> "Cleaned" And oComboItems.Lookup_ComboItem(siHKStatus.Selected_ID) = "Cleaned" And oRoom.Check_InHouse_Res(oRoom.RoomID) Then
                    ihGuest = True
                End If
                oRoom.HouseKeepingStatusID = siHKStatus.Selected_ID
                oRoom.Save()
                If ihGuest Then
                    oRoom.HouseKeepingStatusID = oComboItems.Lookup_ID("RoomHousekeepingStatus", "Dirty")
                    oRoom.HouseKeepingStatusDate = Date.Now
                    oRoom.Save()
                End If
                Set_Values()
            End If

        End If


    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        Save_Values()
    End Sub

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        If txtRoomID.Text = 0 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('Please Save room First');", True)
        Else
            If CheckSecurity("Rooms", "Edit", , , Session("UserDBID")) Then
                ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/marketing/addLockout.aspx?RoomID=" & txtRoomID.Text & "','win01',350,350);", True)
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('Access Denied');", True)
            End If
        End If
    End Sub

    Protected Sub gvMaintRequests_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvMaintRequests.SelectedIndexChanged
        Dim row As GridViewRow = gvMaintRequests.SelectedRow
        Response.Redirect(Request.ApplicationPath & "/Maintenance/EditRequest.aspx?RequestID=" & row.Cells(1).Text)
    End Sub

    Protected Sub PM_Link_Click(sender As Object, e As System.EventArgs) Handles PM_Link.Click
        MultiView1.SetActiveView(PMItems_View)
        refresh_btn_top_Click(Nothing, e)
    End Sub

    Protected Sub UploadedFilesLink_Click(sender As Object, e As System.EventArgs) Handles UploadedFiles_Link.Click
        MultiView1.SetActiveView(UploadedFiles)
        UploadedDocs.KeyField = "RoomID"
        UploadedDocs.KeyValue = txtRoomID.Text
        UploadedDocs.List()
    End Sub

    Protected Sub refresh_btn_top_Click(sender As Object, e As System.EventArgs) Handles refresh_btn_top.Click
        gvPM.DataSource = New clsPreventiveMaintenance.Item2ItemTracks().List("roomid", Request.QueryString("roomid"))
        gvPM.DataBind()
    End Sub

    Protected Sub OnButtonLinkClick(ByVal sender As Object, ByVal e As EventArgs)
        Dim lnk As LinkButton = CType(sender, LinkButton)
        Dim gvr As GridViewRow = lnk.Parent.Parent
        Dim gv As GridView = CType(gvr.NamingContainer, GridView)
        Dim t As TextBox = CType(gvr.FindControl("extraText"), TextBox)
        Dim script = String.Empty

        Dim pmitemid = lnk.CommandArgument.Substring(lnk.CommandArgument.IndexOf("-") + 1)
        Dim item2trackid = lnk.CommandArgument.Substring(0, lnk.CommandArgument.IndexOf("-"))
        lblStatus.Text = item2trackid + "~" + pmitemid
        lblStatus.Visible = False

        Select Case lnk.CommandName
            Case "Remove"
                script = String.Format("modal.mwindow.open('{0}/maintenance/pmtasks.aspx?mvView=vwPMItemChangeRemove&action={1}&pmitemid={2}','win01',690,400);", Request.ApplicationPath, lnk.CommandName, pmitemid)
            Case "Change"

                script = String.Format("modal.mwindow.open('{0}/maintenance/pmtasks.aspx?mvView=vwPMItemChangeRemove&action={1}&pmitemid={2}','win01',690,400);", Request.ApplicationPath, lnk.CommandName, pmitemid)
            Case "Add"
                script = String.Format("modal.mwindow.open('{0}/maintenance/pmtasks.aspx?mvView={1}&action={2}&pmitem2trackid={3}&KeyField=RoomID&KeyValue={4}','win01',690,600);", _
                                       Request.ApplicationPath, "mvPMItem2Track", lnk.CommandName, 0, Request.QueryString("roomid"))
        End Select

        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), DateTime.Now.ToLongTimeString(), script, True)
    End Sub

    Protected Sub add_btn_Click(sender As Object, e As System.EventArgs) Handles add_btn.Click
        Dim script = String.Format("modal.mwindow.open('{0}/maintenance/pmtasks.aspx?mvView={1}&action={2}&pmitem2trackid={3}&KeyField=RoomID&KeyValue={4}','win01',690,600);",
                                       Request.ApplicationPath, "mvPMItem2Track", "Add", 0, Request.QueryString("roomid"))
        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), DateTime.Now.ToLongTimeString(), script, True)
    End Sub
    Protected Sub QuickChecks_Link_Click(sender As Object, e As EventArgs) Handles QuickChecks_Link.Click
        MultiView1.SetActiveView(QuickChecks)
        gvQuickChecks.DataSource = (New clsQuickCheckHist).List_By_Room(txtRoomID.Text)
        gvQuickChecks.DataBind()
    End Sub

    Private Sub gvQuickChecks_DataBound(sender As Object, e As GridViewRowEventArgs) Handles gvQuickChecks.RowDataBound
        If e.Row.Cells.Count > 2 Then
            e.Row.Cells(1).Visible = False
        End If
    End Sub

    Private Sub gvQuickChecks_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvQuickChecks.SelectedIndexChanged
        If CType(sender, GridView).SelectedIndex > -1 Then
            Response.Redirect(Request.ApplicationPath & "/maintenance/editquickcheckhist.aspx?id=" & CType(sender, GridView).SelectedRow.Cells(1).Text)
        End If
    End Sub
    Protected Sub Refurbs_Link_Click(sender As Object, e As EventArgs) Handles Refurbs_Link.Click
        MultiView1.SetActiveView(Refurbs)
        gvRefurbs.DataSource = (New clsRefurbHist).List_By_Room(txtRoomID.Text)
        gvRefurbs.DataBind()
    End Sub

    Private Sub gvRefurbs_DataBound(sender As Object, e As GridViewRowEventArgs) Handles gvRefurbs.RowDataBound
        If e.Row.Cells.Count > 2 Then
            e.Row.Cells(1).Visible = False
        End If
    End Sub

    Private Sub gvRefurbs_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvRefurbs.SelectedIndexChanged
        If CType(sender, GridView).SelectedIndex > -1 Then
            Response.Redirect(Request.ApplicationPath & "/maintenance/editRefurbhist.aspx?id=" & CType(sender, GridView).SelectedRow.Cells(1).Text)
        End If
    End Sub
End Class
