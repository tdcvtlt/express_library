
Partial Class Accounting_RefundRequests
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oMerchant As New clsCCMerchantAccount
            ddAccounts.DataSource = oMerchant.List_Accounts()
            ddAccounts.DataTextField = "Description"
            ddAccounts.DataValueField = "AccountID"
            ddAccounts.DataBind()
            oMerchant = Nothing
        End If
    End Sub


    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        lblErr.Text = ""
        Dim oCCAcct As New clsCCMerchantAccount
        oCCAcct.AccountID = ddAccounts.SelectedValue
        oCCAcct.Load()
        If CheckSecurity("Payments", "ViewPending" & oCCAcct.AccountName & "Refunds", , , Session("UserDBID")) Then
            Dim oRefRequest As New clsRefundRequest
            gvRefundRequests.DataSource = oRefRequest.Get_Pending_Requests(ddAccounts.SelectedValue)
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvRefundRequests.DataKeyNames = sKeys
            gvRefundRequests.DataBind()
            If CheckSecurity("Payments", "Approve" & oCCAcct.AccountName & "Refunds", , , Session("UserDBID")) Then
                btnProcess.Visible = True
            End If
        Else
            lblErr.Text = "ACCESS DENIED"
        End If
        oCCAcct = Nothing
    End Sub

    Protected Sub lbDetails_Click(sender As Object, e As System.EventArgs)
        ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "modal.mwindow.open('" & Request.ApplicationPath & "/Accounting/ApprovalBreakout.aspx?Field=RefundTrans&ID=" & CType(sender.Parent.parent, GridViewRow).Cells(0).Text() & "', 'win01',450,450);", True)
    End Sub

    Protected Sub Unnamed2_Click(sender As Object, e As System.EventArgs)
        'Error Checking
        Dim bproceed As Boolean = True
        Dim sErr As String = ""
        For Each row As GridviewRow In gvRefundRequests.Rows
            Dim cbApprove As CheckBox = row.FindControl("cbApprove")
            Dim cbDeny As CheckBox = row.FindControl("cbDeny")
            If cbDeny.Checked And cbApprove.Checked Then
                bproceed = False
                sErr = "You Can Not Approve and Deny a Refund."
            End If
        Next

        If bproceed Then
            Dim orefRequest As New clsRefundRequest
            Dim oCCTrans As New clsCCTrans
            Dim oCCtransApply As New clsCCTransApplyTo
            Dim oCombo As New clsComboItems
            Dim ccTransID As Integer = 0
            Dim oMercAcct As New clsCCMerchantAccount
            For Each row As GridViewRow In gvRefundRequests.Rows
                Dim cbApprove As CheckBox = row.FindControl("cbApprove")
                Dim cbDeny As CheckBox = row.FindControl("cbDeny")
                If cbDeny.Checked Then
                    orefRequest.RefundRequestID = row.Cells(0).Text
                    orefRequest.Load()
                    orefRequest.Approved = -1
                    orefRequest.ApprovedByID = Session("UserDBID")
                    orefRequest.DateApproved = System.DateTime.Now
                    orefRequest.Save()
                ElseIf cbApprove.Checked Then
                    orefRequest.RefundRequestID = row.Cells(0).text
                    orefRequest.Load()
                    orefRequest.UserID = Session("UserDBID")
                    orefRequest.Approved = 1
                    orefRequest.ApprovedByID = Session("UserDBID")
                    orefRequest.DateApproved = System.DateTime.Now
                    orefRequest.Save()
                    oCCTrans.CCTransID = 0
                    oCCTrans.Load()
                    oCCTrans.TransTypeID = oCombo.Lookup_ID("CCTransType", "Refund")
                    oMercAcct.AccountID = orefRequest.AccountID
                    oMercAcct.Load()
                    If oMercAcct.AccountName = "~0009~" Then
                        oCCTrans.AccountID = oMercAcct.Lookup_By_AcctName("~0003~")
                    Else
                        oCCTrans.AccountID = orefRequest.AccountID
                    End If
                    oCCTrans.Amount = orefRequest.Amount
                    oCCTrans.CreditCardID = orefRequest.CreditCardID

                    Dim oCC As New clsCreditCard
                    oCC.CreditCardID = orefRequest.CreditCardID
                    oCC.Load()
                    oCCTrans.Token = oCC.token
                    oCC = Nothing

                    oCCTrans.CreatedByID = Session("UserDBID")
                    oCCTrans.DateCreated = System.DateTime.Now
                    oCCTrans.Imported = 0
                    oCCTrans.ClientIP = Request.ServerVariables("REMOTE_HOST")
                    oCCTrans.BatchID = 0
                    If oCCTrans.Save() Then
                        ccTransID = oCCTrans.CCTransID
                    Else
                        lblErr.Text = oCCTrans.Error_Message
                    End If
                    oCCtransApply.UserID = Session("UserDBID")
                    If oCCtransApply.Create_Refund_Items(ccTransID, row.Cells(0).Text) Then
                        oCCTrans.Approved = 1
                        oCCTrans.ApprovedBy = Session("UserDBID")
                        oCCTrans.DateApproved = System.DateTime.Now
                        oCCTrans.Save()
                        orefRequest.RefundRequestID = row.Cells(0).Text
                        orefRequest.UserID = Session("UserDBID")
                        orefRequest.CCTransID = ccTransID
                        orefRequest.Save()
                    Else
                        lblErr.Text = oCCtransApply.Err
                    End If
                    End If
            Next
            If lblErr.Text = "" Then
                Response.Redirect("RefundRequests.aspx")
            End If
            oCombo = Nothing
            orefRequest = Nothing
            oCCTrans = Nothing
            oCCtransApply = Nothing
        End If
    End Sub

    Protected Sub gvRefundRequests_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Requests For This Account." Then
            If e.Row.RowIndex > -1 Then
                e.Row.Cells(3).Text = FormatCurrency(e.Row.Cells(3).Text, 2)
            End If
        End If
    End Sub
End Class
