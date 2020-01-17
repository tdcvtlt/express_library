
Partial Class Maintenance_installPart
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim reqID As Integer = 0
            If Request("transType") = "move" Then
                Table1.Rows(0).Cells(0).Text = "Qty Moved:"
                Table1.Rows(1).Cells(0).Text = "Moved By:"
                table1.Rows(2).Visible = True
                Dim opartMove As New clsRequestPartMoves
                opartMove.RequestPartMoveID = Request("ID")
                opartMove.Load()
                reqID = opartMove.RequestID
                For i = 1 To opartMove.Qty
                    ddQty.Items.Add(i)
                Next
                ddQty.SelectedValue = opartMove.Qty
                opartMove = Nothing
            Else
                Dim opart As New clsRequestParts
                opart.Part2RequestID = Request("ID")
                opart.Load()
                reqID = opart.RequestID
                For i = 1 To opart.Qty
                    ddQty.Items.Add(i)
                Next
                ddQty.SelectedValue = opart.Qty
                opart = Nothing
            End If
            Dim oRequest As New clsRequest
            oRequest.RequestID = reqID
            oRequest.Load()
            If oRequest.CategoryID = "MSTRCORP" Then
                ddTechnician.DataSource = oRequest.List_Maint_Reps("MC", oRequest.AssignedToID)
            Else
                ddTechnician.DataSource = oRequest.List_Maint_Reps("KCP", oRequest.AssignedToID)
            End If
            ddTechnician.DataValueField = "PersonnelID"
            ddTechnician.DataTextField = "Personnel"
            ddTechnician.DataBind()
            ddTechnician.SelectedValue = oRequest.AssignedToID
            oRequest = Nothing
        End If
    End Sub

    Protected Sub Unnamed1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim oCombo As New clsComboItems
        If Request("transType") = "move" Then
            Dim opartMove As New clsRequestPartMoves
            opartMove.RequestPartMoveID = Request("ID")
            opartMove.Load()
            opartMove.UserID = Session("UserDBID")
            opartMove.Qty = ddQty.SelectedValue
            opartMove.MovedBy = ddTechnician.SelectedValue
            opartMove.DateMoved = dteMovedDate.Selected_Date
            opartMove.StatusID = oCombo.Lookup_ID("PartMoveStatus", "Moved")
            opartMove.Save()
            opartMove = Nothing
        Else
            Dim opart As New clsRequestParts
            opart.Part2RequestID = Request("ID")
            opart.Load()
            opart.UserID = Session("UserDBID")
            '***** if Qty Installed is less than qty on work order a new entry needs to be inserted in the t_RequestParts table
            '***** with the qty being the remainder to be installed and the 
            '***** status needs to be on hand and the prid must match the original entry
            '***** The original partrequest record qty field needs to be updated to reflect the qty actually installed
            '***** and date/worker installed by
            If opart.Qty > ddQty.SelectedValue Then
                Dim oPart2 As New clsRequestParts
                oPart2.Part2RequestID = 0
                oPart2.Load()
                oPart2.ItemNumber = opart.ItemNumber
                oPart2.Qty = opart.Qty - ddQty.SelectedValue
                oPart2.StatusID = opart.StatusID
                oPart2.PRID = opart.PRID
                oPart2.RequestID = opart.RequestID
                oPart2.Save()
                oPart2 = Nothing
                opart.Qty = ddQty.SelectedValue
            End If
            opart.InstalledById = ddTechnician.SelectedValue
            opart.StatusID = oCombo.Lookup_ID("PartStatus", "Installed")
            opart.DateInstalled = System.DateTime.Now
            opart.Save()
            opart = Nothing
        End If
        oCombo = Nothing
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.opener.Refresh_Parts('" & Request("transType") & "');window.close();", True)
    End Sub
End Class
