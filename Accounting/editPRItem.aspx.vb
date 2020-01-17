
Partial Class Accounting_editPRItem
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            For i = 1 To 999
                ddQty.items.Add(i)
            Next
            Dim opreqItem As New clsPurchaseRequestItems
            opreqItem.Item2RequestID = Request("ID")
            opreqItem.Load()
            txtItem.text = opreqItem.ItemNumber
            ddQty.SelectedValue = opreqItem.Qty
            txtAmount.Text = opreqItem.Amount
            txtLocation.Text = opreqItem.Location
            txtPurpose.Text = opreqItem.Purpose
            opreqItem = Nothing
        End If
    End Sub

    Protected Sub Unnamed1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim bProceed As Boolean = True
        Dim sErr As String = ""
        '****Security Check****'

        If bProceed = True Then
            Dim opRequest As New clsPurchaseRequest
            Dim opRequestItem As New clsPurchaseRequestItems
            Dim qtyRequested As Integer = 0
            Dim qtyOrdered As Integer = 0
            Dim bSave As Boolean = True
            opRequestItem.Item2RequestID = Request("ID")
            opRequestItem.Load()
            qtyOrdered = opRequest.Get_Ordered_Count_PR(opRequestItem.ItemNumber, opRequestItem.PurchaseRequestID, opRequestItem.Item2RequestID)
            qtyRequested = opRequest.Get_Requested_Count_PR(opRequestItem.ItemNumber, opRequestItem.PurchaseRequestID)
            If qtyRequested = 0 Or (qtyOrdered - qtyRequested) > 0 Then
                bSave = True
            Else
                If ddQty.SelectedValue < (qtyRequested - qtyOrdered) Then
                    bSave = False
                    sErr = "The Quantity Ordered Must Be At Least " & (qtyRequested - qtyOrdered) & " To Fulfill Maintenance Request Needs."
                Else
                    bSave = True
                End If
            End If
            If bSave Then
                opRequestItem.Qty = ddQty.SelectedValue
                opRequestItem.Amount = txtAmount.Text
                opRequestItem.Location = txtLocation.Text
                opRequestItem.Purpose = txtPurpose.Text
                opRequestItem.Save()
                ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "window.opener.Refresh_Parts('pReq');window.close();", True)
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('" & sErr & "');", True)
            End If
            opRequest = Nothing
            opRequestItem = Nothing
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('" & sErr & "');", True)
        End If
    End Sub
End Class
