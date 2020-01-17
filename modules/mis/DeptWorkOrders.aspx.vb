
Partial Class mis_DeptWorkOrders
    Inherits System.Web.UI.Page

    Protected Sub Submitted_Link_Click(sender As Object, e As System.EventArgs) Handles Submitted_Link.Click
        Dim oWorkOrder As New clsWorkOrder
        gvActiveWO.DataSource = oWorkOrder.get_Dept_WorkOrders(Session("UserDBID"), 1)
        gvActiveWO.DataBind()
        oWorkOrder = Nothing
        MultiView1.ActiveViewIndex = 0
    End Sub

    Protected Sub MgrApproval_Link_Click(sender As Object, e As System.EventArgs) Handles MgrApproval_Link.Click
        Dim oWorkOrder As New clsWorkOrder
        gvPendingWO.DataSource = oWorkOrder.get_Dept_WorkOrders(Session("UserDBID"), 0)
        gvPendingWO.DataBind()
        oWorkOrder = Nothing
        MultiView1.ActiveViewIndex = 1
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oWorkOrder As New clsWorkOrder
            gvActiveWO.DataSource = oWorkOrder.get_Dept_WorkOrders(Session("UserDBID"), 1)
            gvActiveWO.DataBind()
            oWorkOrder = Nothing
            MultiView1.ActiveViewIndex = 0
        End If
    End Sub

    Protected Sub gvActiveWO_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvActiveWO.SelectedIndexChanged
        Dim row As GridViewRow = gvActiveWO.SelectedRow
        Response.Redirect("editWorkOrder.aspx?WorkOrderID=" & row.Cells(1).Text)
    End Sub

    Protected Sub gvPendingWO_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles gvPendingWO.SelectedIndexChanged
        Dim row As GridViewRow = gvPendingWO.SelectedRow
        Response.Redirect("editWorkOrder.aspx?WorkOrderID=" & row.Cells(1).Text)
    End Sub
End Class
