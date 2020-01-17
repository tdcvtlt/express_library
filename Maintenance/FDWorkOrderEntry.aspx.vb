Imports System
Imports System.Data
Partial Class Maintenance_FDWorkOrderEntry
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oRoom As New clsRooms
            ddRooms.Items.Add(New ListItem("", "0"))
            ddRooms.DataSource = oRoom.List_Rooms()
            ddRooms.DataValueField = "RoomID"
            ddRooms.DataTextField = "RoomNumber"
            ddRooms.AppendDataBoundItems = True
            ddRooms.DataBind()
            oRoom = Nothing

            siProblemArea.Connection_String = Resources.Resource.cns
            siProblemArea.Comboitem = "RequestArea"
            siProblemArea.Label_Caption = ""
            siProblemArea.Load_Items()

            Dim dt As New DataTable
            dt.Columns.Add("Category")
            dt.Columns.Add("Issue")
            dt.Columns.Add("Description")

            Session("dt") = dt
            gvItems.DataSource = dt
            gvItems.DataBind()
        Else
            gvItems.DataSource = Session("dt")
            gvItems.DataBind()
        End If
    End Sub
    Protected Sub Unnamed1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "modal.mwindow.open('" & Request.ApplicationPath & "/Maintenance/FDWorkOrderItem.aspx','win01',350,350);", True)
    End Sub

    Protected Sub Unnamed1_Click1(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim oRequest As New clsRequest
        Dim oCombo As New clsComboItems
        Dim dt As New DataTable
        Dim drow As DataRow
        dt = Session("dt")
        Dim i As Integer
        If dt.Rows.Count = 0 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('Please Enter a Maintenance Issue.');", True)
        Else
            For i = 0 To dt.Rows.Count - 1
                drow = dt.Rows(i)
                oRequest.RequestID = 0
                oRequest.Load()
                oRequest.EntryDate = System.DateTime.Now
                oRequest.EnteredByID = Session("UserDBID")
                oRequest.StatusID = oCombo.Lookup_ID("WorkOrderStatus", "NotStarted")
                oRequest.RoomID = ddRooms.SelectedValue
                oRequest.RequestAreaID = siProblemArea.Selected_ID
                oRequest.CategoryID = drow(0)
                oRequest.StartTime = "12:00:AM"
                oRequest.EndTime = "12:00:AM"
                oRequest.IssuedID = oCombo.Lookup_ID("MaintenanceRequestIssue", drow(1))
                oRequest.Description = drow(2)
                oRequest.Save()
            Next
            Response.Redirect("FDWorkOrderEntry.aspx")
        End If
        oRequest = Nothing
        oCombo = Nothing
    End Sub


    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        gvItems.DataSource = Session("dt")
        gvItems.DataBind()
    End Sub

    Protected Sub gvItems_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvItems.RowDeleting
        Dim dt As New DataTable
        Dim dRow As DataRow
        dt = Session("dt")
        dRow = dt.Rows(e.RowIndex)
        dt.Rows.Remove(dRow)
        Session("dt") = dt
        gvItems.DataSource = Session("dt")
        gvItems.DataBind()
    End Sub
End Class
