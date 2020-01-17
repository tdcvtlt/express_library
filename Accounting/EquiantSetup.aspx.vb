
Partial Class Accounting_EquiantSetup
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Fill_GridViews()
        End If
    End Sub

    Private Sub Fill_GridViews()
        gvMapped.DataSource = (New clsEquiantCodeMapping).List
        gvMapped.DataBind()

        gvUnmapped.DataSource = (New clsEquiantCodeMapping).List_UnMapped
        gvUnmapped.DataBind()
    End Sub

    Protected Sub gvUnmapped_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvUnmapped.SelectedIndexChanged
        Response.Redirect("editEquiantMapping.aspx?code=" & gvUnmapped.SelectedRow.Cells(1).Text & "&sourcecode=" & gvUnmapped.SelectedRow.Cells(2).Text & "&Category=" & gvUnmapped.SelectedRow.Cells(3).Text & "&PosNeg=" & If(gvUnmapped.SelectedRow.Cells(4).Text = "Positive", 0, 1))
    End Sub

    Private Sub gvUnmapped_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvUnmapped.RowDataBound
        If e.Row.Cells(4).Text = "0" Then e.Row.Cells(4).Text = "Positive" Else e.Row.Cells(4).Text = "Negative"
    End Sub
    Protected Sub gvMapped_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvMapped.SelectedIndexChanged
        Response.Redirect("editEquiantMapping.aspx?id=" & gvMapped.SelectedRow.Cells(1).Text)
    End Sub

    Private Sub gvMapped_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvMapped.RowDataBound
        'Action = 1 (Payment)
        'Action = 2 (Reversal/Credit)
        'Action = 3 (Create Invoice)
        'Action = 4 (Dump/Ignore)
        Select Case e.Row.Cells(5).Text
            Case "1"
                e.Row.Cells(5).Text = "Payment"
            Case "2"
                e.Row.Cells(5).Text = "Reversal"
            Case "3"
                e.Row.Cells(5).Text = "Invoice"
            Case "4"
                e.Row.Cells(5).Text = "Ignore"
        End Select

        e.Row.Cells(6).Text = If(e.Row.Cells(6).Text = "1", "Negative", "Positive")

        If IsNumeric(e.Row.Cells(7).Text) Then
            Dim oFT As New clsFinancialTransactionCodes
            oFT.FinTransID = e.Row.Cells(7).Text
            oFT.Load()
            e.Row.Cells(7).Text = (New clsComboItems).Lookup_ComboItem(oFT.TransCodeID)
            oFT = Nothing
        End If

        If IsNumeric(e.Row.Cells(8).Text) Then e.Row.Cells(8).Text = (New clsComboItems).Lookup_ComboItem(e.Row.Cells(8).Text)

    End Sub
End Class
