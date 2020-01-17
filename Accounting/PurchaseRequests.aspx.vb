Imports System
Imports Microsoft.VisualBasic
Partial Class Accounting_PurchaseRequests
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            ddPrStatus.Items.Add(New listItem("ALL", "ALL"))
            ddPrStatus.Items.Add(New listItem("Pending", "0"))
            ddPrStatus.Items.Add(New listItem("Approved", "1"))
            ddPrStatus.Items.Add(New listItem("Denied", "-1"))
        End If

    End Sub

    Protected Sub Unnamed1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim pRequest As New clsPurchaseRequest
        If txtFilter.Text <> "" Then
            gvPReq.DataSource = pRequest.List(ddPRStatus.SelectedValue, txtFilter.Text)
        Else
            gvPReq.DataSource = pRequest.List(ddPRStatus.SelectedValue)
        End If
        Dim sKeys(0) As String
        sKeys(0) = "ID"
        gvPReq.DataKeyNames = sKeys
        gvPReq.DataBind()
        pRequest = Nothing
    End Sub

    Protected Sub gvPReq_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <= "No Records" Then
            If e.Row.RowIndex > -1 Then
                e.Row.Cells(2).Text = FormatCurrency(CDbl(e.Row.Cells(2).Text), 2)
                If e.Row.Cells(3).Text = "0" Then 
                    e.Row.Cells(3).Text = "Pending"
                ElseIf e.Row.Cells(3).Text = "1" Then
                    e.Row.Cells(3).Text = "Approved"
                Else
                    e.Row.Cells(3).Text = "Denied"
                End If
            End If
        End If
    End Sub

    Protected Sub gvPReq_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvPReq.SelectedIndexChanged
        Dim row As GridviewRow = gvPReq.SelectedRow
        Response.Redirect("editPurchaseRequest.aspx?ID=" & row.Cells(1).Text & "")
    End Sub

    Protected Sub Unnamed2_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("editPurchaseRequest.aspx?ID=0")
    End Sub
End Class
