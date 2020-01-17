
Partial Class Accounting_ApprovalBreakout
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            If Request("Field") = "CCTrans" Then
                Dim oCCApply As New clsCCTransApplyTo
                gvTransactions.DataSource = oCCApply.List_ApplyTo_Items(Request("ID"))
                gvTransactions.DataBind()
                oCCApply = Nothing
            ElseIf Request("Field") = "RefundTrans" Then
                Dim oRefApply As New clsRefundRequestApplyTo
                gvtransactions.DataSource = oRefApply.List_ApplyTo_Items(Request("ID"))
                gvTransactions.DataBind()
                oRefApply = Nothing
            End If

        End If
    End Sub

    Protected Sub gvTransactions_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Transactions" Then
            If e.Row.RowIndex > -1 Then
                e.Row.Cells(2).Text = CDate(e.Row.Cells(2).Text).ToShortDateString
            End If
        End If
    End Sub
End Class
