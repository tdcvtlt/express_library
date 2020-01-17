Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient

Partial Class mis_workorders
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Search()
    End Sub

    Private Sub Search()
        Dim oWorkOrder As New clsWorkOrder
        oWorkOrder.UserID = Session("UserDBID")
        gvWorkOrder.DataSource = oWorkOrder.List_WorkOrders(txtFilter.Text)
        gvWorkOrder.DataBind()
        oWorkOrder = Nothing
    End Sub

    Protected Sub gvWorkOrder_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvWorkOrder.SelectedIndexChanged
        Dim row As GridViewRow = gvWOrkOrder.SelectedRow
        Response.Redirect("editworkorder.aspx?workorderid=" & row.Cells(1).Text)
    End Sub

    Protected Sub gvWorkOrder_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Work Orders" Then
            If e.Row.RowIndex > -1 Then
                If e.Row.Cells(2).Text <> "&nbsp;" And e.Row.Cells(2).Text <> "" Then
                    e.Row.Cells(2).Text = CDate(e.Row.Cells(2).Text).ToShortDateString
                End If
                If e.Row.Cells(3).Text <> "&nbsp;" And e.Row.Cells(3).Text <> "" Then
                    e.Row.Cells(3).Text = CDate(e.Row.Cells(3).Text).ToShortDateString
                End If
            End If
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then Search()
    End Sub
End Class
