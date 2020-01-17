Imports System
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient

Partial Class Maintenance_EditRequest
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            MultiView1.ActiveViewIndex = 0
            Load_SIs()
            Load_DDLs()
            Set_Values()
            If Request("RequestID") = 0 Then
                Set_Defaults()
            End If

        End If
    End Sub

    Protected Sub Set_Defaults()
        txtDate.Text = Date.Today
        siStatus.Select_Item("Not Started")
        siIssue.Select_Item("Service")
    End Sub

    Protected Sub Set_Values()
        Dim oRequest As New clsRequest
        oRequest.RequestID = Request("RequestID")
        oRequest.Load()
        txtRequestID.Text = oRequest.RequestID
        txtDate.Text = oRequest.EntryDate
        siLocation.Selected_ID = oRequest.RequestAreaID
        siIssue.Selected_ID = oRequest.IssuedID
        ddRoom.SelectedValue = oRequest.RoomID
        ddAssignedTo.SelectedValue = oRequest.AssignedToID
        ddCategory.SelectedValue = Trim(oRequest.CategoryID)
        siStatus.Selected_ID = oRequest.StatusID
        txtSDate.Selected_Date = oRequest.StartDate
        txtEDate.Selected_Date = oRequest.EndDate
        txtSTime.Text = oRequest.StartTime
        txtETime.Text = oRequest.EndTime
        txtSubject.Text = oRequest.Subject
        txtDescription.Text = oRequest.Description
        ddPriority.SelectedValue = oRequest.Priority
        ddKF.SelectedValue = oRequest.KeyField
        ddKV.SelectedValue = oRequest.KeyValue
        oRequest = Nothing
    End Sub

    Protected Sub Load_SIs()
        'Grab all the comboitems and load them into the form fields
        Dim oRequest As New clsRequest
        oRequest.RequestID = Request("RequestID")
        oRequest.Load()
        siStatus.Connection_String = Resources.Resource.cns
        siStatus.ComboItem = "WorkOrderStatus"
        siStatus.Label_Caption = ""
        If Request("RequestID") <> "0" Then
            siStatus.Selected_ID = oRequest.StatusID
        End If
        siStatus.Load_Items()

        siIssue.Connection_String = Resources.Resource.cns
        siIssue.ComboItem = "MaintenanceRequestIssue"
        siIssue.Label_Caption = ""
        siIssue.Load_Items()

        siLocation.Connection_String = Resources.Resource.cns
        siLocation.Comboitem = "WorkOrderCategory" '"RequestArea"
        siLocation.Label_Caption = ""
        If Request("RequestID") <> "0" Then
            siLocation.Selected_ID = oRequest.RequestAreaID
        End If
        siLocation.Load_Items()

        oRequest = Nothing

    End Sub
    Protected Sub Request_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Request_Link.Click
        MultiView1.ActiveViewIndex = 0
    End Sub

    Protected Sub Events_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Events_Link.Click
        If txtRequestID.Text > 0 Then
            ucEvents.KeyField = "RequestID"
            ucEvents.KeyValue = txtRequestID.Text
            ucEvents.List()
            MultiView1.ActiveViewIndex = 5
        End If
    End Sub
    Protected Sub Notes_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Notes_Link.Click
        If txtRequestID.Text > 0 Then
            MultiView1.ActiveViewIndex = 4
            Notes1.KeyValue = txtRequestID.Text
            Notes1.Display()
        End If
    End Sub

    Protected Sub Load_DDLs()
        Dim oRoom As New clsRooms
        'ddRoom.Items.Add(New ListItem("", "0"))
        'ddRoom.DataSource = oRoom.List_Rooms()
        'ddRoom.DataValueField = "RoomID"
        'ddRoom.DataTextField = "RoomNumber"
        'ddRoom.AppendDataBoundItems = True
        'ddRoom.DataBind()
        ddKF.Items.Add(New ListItem("Room", "RoomID"))
        ddKF.Items.Add(New ListItem("Other", "PMBuildingID"))
        Dim oRequest As New clsRequest
        If Request("RequestID") = "0" Then
            ddAssignedTo.DataSource = oRequest.List_Maint_Reps("All", 0)
            ddKF.SelectedValue = "RoomID"
            lblKF.Text = "Room"
            ddKV.DataSource = oRoom.List_Rooms()
            ddKV.DataTextField = "RoomNumber"
            ddKV.DataValueField = "RoomID"
            ddKV.DataBind()
        Else
            oRequest.RequestID = Request("RequestID")
            oRequest.Load()
            If oRequest.CategoryID = "MSTRCORP" Then
                ddAssignedTo.dataSource = oRequest.List_Maint_Reps("MC", oRequest.AssignedToID)
            Else
                ddAssignedTo.dataSource = oRequest.List_Maint_Reps("KCP", oRequest.AssignedToID)
            End If
            If oRequest.KeyField = "RoomID" Then
                ddKF.SelectedValue = "RoomID"
                ddKV.DataSource = oRoom.List_Rooms()
                ddKV.DataTextField = "RoomNumber"
                ddKV.DataValueField = "RoomID"
                ddKV.DataBind()
                lblKF.Text = "Room:"
            Else
                Dim oPMB As New clsPMBuilding
                ddKF.SelectedValue = "PMBuildingID"
                ddKV.DataSource = oPMB.Get_Buildings()
                ddKV.DataValueField = "ID"
                ddKV.DataTextField = "Name"
                ddKV.DataBind()
                oPMB = Nothing
                lblKF.Text = "Area:"
            End If
        End If
        ddAssignedTo.DataValueField = "PersonnelID"
        ddAssignedTo.DataTextField = "Personnel"
        ddAssignedTo.DataBind()
        ddCategory.DataSource = oRequest.List_Categories(0)
        ddCategory.DataValueField = "Category"
        ddCategory.DataTextField = "Category"
        ddCategory.DataBind()
        oRequest = Nothing
        oRoom = Nothing


    End Sub

    Protected Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/Maintenance/PrintWorkOrder.aspx?RequestID=" & txtRequestID.Text & "','win01',690,450);", True)
    End Sub

    Protected Sub Part_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Part_Link.Click
        If txtRequestID.Text > 0 Then
            Dim oParts As New clsRequestParts
            gvParts.DataSource = oParts.List_Parts(txtRequestID.Text)
            Dim sKeys(0) As String
            sKeys(0) = "Part2RequestID"
            gvParts.DatakeyNames = sKeys
            gvParts.DataBind()
            MultiView1.ActiveViewIndex = 1
        End If
    End Sub

    Protected Sub gvParts_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then
            If e.Row.RowIndex > -1 Then
                If e.Row.Cells(5).Text = "0" Then
                    e.Row.Cells(5).Text = ""
                Else
                    e.Row.Cells(5).Text = "<a href = '../Accounting/EditPurchaseRequest.aspx?ID=" & e.Row.Cells(5).Text & "'>" & e.Row.Cells(5).Text & "</a>"
                End If
                If e.Row.Cells(6).Text = "TRUE" Then
                    e.Row.Cells(7).Visible = True
                Else
                    e.Row.Cells(7).Visible = False
                End If
                If e.Row.Cells(8).Text = "TRUE" Then
                    e.Row.Cells(9).Visible = True
                Else
                    e.Row.Cells(9).Visible = False
                End If
            End If
            'e.Row.Cells(0).Visible = False
            e.Row.Cells(6).Visible = False
            e.Row.Cells(8).Visible = False
        End If
    End Sub

    Protected Sub PartMove_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles PartMove_Link.Click
        If txtRequestID.text > 0 Then
            Dim oParts As New clsRequestPartMoves
            gvPartsMoved.DataSource = oParts.List_Parts(txtRequestID.Text)
            Dim sKeys(0) As String
            sKeys(0) = "RequestPartMoveID"
            gvPartsMoved.DatakeyNames = sKeys
            gvPartsMoved.DataBind()
            MultiView1.ActiveViewIndex = 2
        End If
    End Sub

    Protected Sub gvPartsMoved_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then
            If e.Row.RowIndex > -1 Then
                If e.Row.Cells(6).Text = "TRUE" Then
                    e.Row.Cells(6).Visible = True
                End If
                If e.Row.Cells(7).Text = "TRUE" Then
                    e.Row.Cells(7).Visible = True
                End If

                If e.Row.Cells(6).Text = "TRUE" Then
                    e.Row.Cells(7).Visible = True
                Else
                    e.Row.Cells(7).Visible = False
                End If
                If e.Row.Cells(8).Text = "TRUE" Then
                    e.Row.Cells(9).Visible = True
                Else
                    e.Row.Cells(9).Visible = False
                End If
            End If
            e.Row.Cells(0).Visible = False
            e.Row.Cells(6).Visible = False
            e.Row.Cells(8).Visible = False
        End If
    End Sub
    Protected Sub gvPartsMoved_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvPartsMoved.RowCommand
        Dim partID As Integer
        Dim oCombo As New clsComboItems
        Dim opartMove As New clsRequestPartMoves
        partID = Convert.ToInt32(gvPartsMoved.DataKeys(Convert.ToInt32(e.CommandArgument)).Value)
        If e.CommandName.CompareTo("Remove") = 0 Then
            opartMove.RequestPartMoveID = partID
            opartMove.Load()
            opartMove.StatusID = oCombo.Lookup_ID("PartMoveStatus", "Cancelled")
            opartMove.Save()
            gvPartsMoved.DataSource = opartMove.List_Parts(txtRequestID.Text)
            Dim sKeys(0) As String
            sKeys(0) = "RequestPartMoveID"
            gvPartsMoved.DatakeyNames = sKeys
            gvPartsMoved.DataBind()
        ElseIf e.CommandName.CompareTo("Install") = 0 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/Maintenance/installPart.aspx?ID=" & partID & "&transType=move','win01',350,350);", True)
        End If
        oCombo = Nothing
        opartMove = Nothing
    End Sub


    Protected Sub gvPartsLoaned_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Records" Then
            If e.Row.RowIndex > -1 Then
                If e.Row.Cells(8).Text = "TRUE" Then
                    e.Row.Cells(9).Visible = True
                Else
                    e.Row.Cells(9).Visible = False
                End If
            End If
            e.Row.Cells(0).Visible = False
            e.Row.Cells(8).Visible = False
        End If
    End Sub

    Protected Sub gvPartsLoaned_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvPartsLoaned.RowCommand
        Dim partID As Integer
        Dim oCombo As New clsComboItems
        Dim opartLoan As New clsRequestPartLoan
        partID = Convert.ToInt32(gvPartsLoaned.DataKeys(Convert.ToInt32(e.CommandArgument)).Value)
        If e.CommandName.CompareTo("Return") = 0 Then
            opartLoan.RequestPartLoanID = partID
            opartLoan.Load()
            opartLoan.StatusID = oCombo.Lookup_ID("PartLoanStatus", "Returned")
            opartLoan.DatePickedUp = System.DateTime.Now.ToShortDateString
            opartLoan.Save()
            gvPartsLoaned.DataSource = opartLoan.List_Parts(txtRequestID.Text)
            Dim sKeys(0) As String
            sKeys(0) = "RequestPartLoanID"
            gvPartsLoaned.DatakeyNames = sKeys
            gvPartsLoaned.DataBind()
        End If
        oCombo = Nothing
        opartLoan = Nothing
    End Sub
    Protected Sub PartLoan_Link_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles PartLoan_Link.Click
        If txtRequestID.text > 0 Then
            Dim oParts As New clsRequestPartLoan
            gvPartsLoaned.DataSource = oParts.List_Parts(txtRequestID.Text)
            Dim sKeys(0) As String
            sKeys(0) = "RequestPartLoanID"
            gvPartsLoaned.DatakeyNames = sKeys
            gvPartsLoaned.DataBind()
            MultiView1.ActiveViewIndex = 3
        End If
    End Sub

    Protected Sub gvParts_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvParts.RowCommand
        Dim partID As Integer
        Dim oCombo As New clsComboItems
        partID = Convert.ToInt32(gvParts.DataKeys(Convert.ToInt32(e.CommandArgument)).Value)
        If e.CommandName.CompareTo("RemovePart") = 0 Then
            Dim oRequestPart As New clsRequestParts
            If oRequestPart.Remove_Part(partID) Then
                oRequestPart.Part2RequestID = partID
                oRequestPart.Load()
                oRequestPart.PRID = 0
                oRequestPart.StatusID = oCombo.Lookup_ID("PartStatus", "Cancelled")
                If oRequestPart.Save() Then
                    gvParts.DataSource = oRequestPart.List_Parts(txtRequestID.Text)
                    Dim sKeys(0) As String
                    sKeys(0) = "Part2RequestID"
                    gvParts.DatakeyNames = sKeys
                    gvParts.DataBind()
                Else
                    lblErr.Text = oRequestPart.Err
                End If
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('ERROR');", True)
            End If
        ElseIf e.CommandName.CompareTo("Install") = 0 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/Maintenance/installPart.aspx?ID=" & partID & "&transType=addPart','win01',350,350);", True)
        End If
    End Sub

    Protected Sub Unnamed1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/Maintenance/partfilter.aspx?RequestID=" & txtRequestID.Text & "&transType=addPart','win01',690,450);", True)
    End Sub

    Protected Sub Unnamed2_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/Maintenance/partfilter.aspx?RequestID=" & txtRequestID.Text & "&transType=move','win01',690,450);", True)
    End Sub

    Protected Sub Unnamed3_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/Maintenance/partfilter.aspx?RequestID=" & txtRequestID.Text & "&transType=loan','win01',690,450);", True)
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        Dim oReq As New clsRequest
        oReq.UserID = Session("UserDBID")
        Dim oCombo As New clsComboItems
        Dim sErr As String = ""
        Dim bProceed As Boolean = True
        If siLocation.Selected_ID < 1 Then
            bProceed = False
            sErr = "Please Select a Problem Location. \n"
        ElseIf siStatus.Selected_ID < 1 Then
            bProceed = False
            sErr = "Please Update the Status of Request. \n"
        ElseIf oCombo.Lookup_ComboItem(siStatus.Selected_ID) = "Complete" Then
            'If CStr(txtSDate.Selected_Date) = "" Then
            '    bProceed = False
            '    sErr = "Please Select a Start Date. \n"
            'End If
            'If CStr(txtEDate.Selected_Date) = "" Then
            '    bProceed = False
            '    sErr += "Please Select An End Date. \n"
            'End If
        End If

        If bProceed Then
            If Request("RequestID") = 0 Then
                oReq.RequestID = 0
                oReq.Load()
                oReq.EnteredByID = Session("UserDBID")
                oReq.EntryDate = System.DateTime.Now
                oReq.RequestAreaID = siLocation.Selected_ID
                'oReq.RoomID = ddRoom.SelectedValue
                oReq.AssignedToID = ddAssignedTo.SelectedValue
                oReq.StatusID = siStatus.Selected_ID
                oReq.CategoryID = ddCategory.SelectedValue
                oReq.IssuedID = siIssue.Selected_ID
                oReq.StartDate = txtSDate.Selected_Date
                oReq.EndDate = txtEDate.Selected_Date
                oReq.Subject = txtSubject.Text
                oReq.Description = txtDescription.Text
                oReq.StartTime = txtSTime.Text
                oReq.EndTime = txtETime.Text
                oReq.Priority = ddPriority.SelectedValue
                oReq.KeyField = ddKF.SelectedValue
                oReq.KeyValue = ddKV.SelectedValue
                oReq.Save()
                Response.Redirect("editRequest.aspx?RequestID=" & oReq.RequestID)
            Else
                Dim oReqParts As New clsRequestParts
                Dim oReqLoan As New clsRequestPartLoan
                Dim oReqMove As New clsRequestPartMoves
                oReq.RequestID = txtRequestID.Text
                oReq.Load()
                oReq.UserID = Session("UserDBID")
                If (oCombo.Lookup_ComboItem(siStatus.Selected_ID) = "Complete" Or oCombo.Lookup_ComboItem(siStatus.Selected_ID) = "Cancelled") Then
                    If oReqParts.Get_ReqAssigned_Parts(oReq.RequestID) > 0 Then
                        bProceed = False
                        sErr = "You have Pending Parts that must be Removed or Installed Before Completing Work Order."
                    ElseIf oReqMove.Get_Requested_Count(oReq.RequestID) > 0 Then
                        bProceed = False
                        sErr = "You Have Pending Part Moves that need to be moved or cancelled before Changing Status."
                    ElseIf oReqLoan.Get_ReqLoaned_Count(oReq.RequestID) > 0 Then
                        bProceed = False
                        sErr = "You Have Parts On Loan that must be returned before Changing Status."
                    End If
                End If
                If bProceed Then
                    oReq.RequestAreaID = siLocation.Selected_ID
                    oReq.KeyField = ddKF.SelectedValue
                    oReq.KeyValue = ddKV.SelectedValue
                    oReq.AssignedToID = ddAssignedTo.SelectedValue
                    If oCombo.Lookup_ComboItem(oReq.StatusID) = "Not Started" And (oCombo.Lookup_ComboItem(Request("statusid")) <> "Cancelled" And oCombo.Lookup_ComboItem(siStatus.Selected_ID) <> "Not Started") Then
                        oReq.StartDate = System.DateTime.Now.ToShortDateString
                        oReq.StartTime = System.DateTime.Now.ToShortTimeString
                    End If
                    If oCombo.Lookup_ComboItem(siStatus.Selected_ID) = "Complete" And oCombo.Lookup_ComboItem(oReq.StatusID) <> "Complete" Then
                        oReq.EndDate = System.DateTime.Now.ToShortDateString
                        oReq.EndTime = System.DateTime.Now.ToShortTimeString
                    End If
                    If oCombo.Lookup_ComboItem(siStatus.Selected_ID) = "Cancelled" Then
                        If oReqParts.Get_ReqInstalled_Parts(oReq.RequestID) > 0 Then
                            Send_Mail("afirth@kingscreekplantation.com", "do_not_reply@kingscreekplantation.com", "Maintenance Work Order Cancelled", "Work Order " & oReq.RequestID & " was cancelled with parts installed.")
                        End If
                    End If
                    oReq.StatusID = siStatus.Selected_ID
                    oReq.CategoryID = ddCategory.SelectedValue
                    oReq.IssuedID = siIssue.Selected_ID
                    'oReq.StartDate = txtSDate.Selected_Date
                    'oReq.EndDate = txtEDate.Selected_Date
                    oReq.Subject = txtSubject.Text
                    oReq.Description = txtDescription.Text
                    'oReq.StartTime = txtSTime.Text
                    'oReq.EndTime = txtETime.Text
                    oReq.Priority = ddPriority.SelectedValue
                    oReq.Save()
                    Response.Redirect("editRequest.aspx?RequestID=" & oReq.RequestID)
                Else
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('" & sErr & "');", True)
                End If
                oReqParts = Nothing
                oReqMove = Nothing
                oReqLoan = Nothing
            End If
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('" & sErr & "');", True)
        End If
        oReq = Nothing
        oCombo = Nothing
    End Sub

    Protected Sub ddKF_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddKF.SelectedIndexChanged
        If ddKF.SelectedValue = "RoomID" Then
            lblKF.Text = "Room:"
            Dim oRm As New clsRooms
            ddKV.DataSource = oRm.List_Rooms()
            ddKV.DataTextField = "RoomNumber"
            ddKV.DataValueField = "RoomID"
            ddKV.DataBind()
            oRm = Nothing
        Else
            lblKF.Text = "Area:"
            Dim oPMB As New clsPMBuilding
            ddKV.DataSource = oPMB.Get_Buildings
            ddKV.DataTextField = "Name"
            ddKV.DataValueField = "ID"
            ddKV.DataBind()
            oPMB = Nothing
        End If
    End Sub
End Class
