Imports Microsoft.VisualBasic
Partial Class Accounting_CCApprovals
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
        If CheckSecurity("Payments", "ViewPending" & oCCAcct.AccountName & "Approvals", , , Session("UserDBID")) Then
            Dim oCCTrans As New clsCCTrans
            gvTransactions.DataSource = oCCTrans.Get_Pending_Requests(ddAccounts.SelectedValue)
            Dim sKeys(0) As String
            sKeys(0) = "ID"
            gvTransactions.DataKeyNames = sKeys
            gvTransactions.DataBind()
            If CheckSecurity("Payments", "Approve" & oCCAcct.AccountName & "Charges", , , Session("UserDBID")) Then
                btnProcess.Visible = True
            End If
        Else
            lblErr.Text = "ACCESS DENIED"
        End If
        oCCAcct = Nothing
    End Sub

    Protected Sub gvTransactions_RowDataBound(sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Requests For This Account" Then
            If e.Row.RowIndex > -1 Then
                e.Row.Cells(3).Text = FormatCurrency(e.Row.Cells(3).Text, 2)
                e.Row.Cells(4).Text = CDate(e.Row.Cells(4).Text).ToShortDateString
            End If
        End If
    End Sub
    Protected Sub Unnamed2_Click(sender As Object, e As System.EventArgs) Handles btnProcess.Click
        'Error Checking
        Dim bproceed As Boolean = True
        Dim sErr As String = ""
        For Each row As GridviewRow In gvTransactions.Rows
            Dim cbApprove As CheckBox = row.FindControl("cbApprove")
            Dim cbDeny As CheckBox = row.FindControl("cbDeny")
            If cbDeny.Checked And cbApprove.Checked Then
                bproceed = False
                sErr = "You Can Not Approve and Deny a Refund."
            End If
        Next

        If bproceed Then
            Dim oCCTrans As New clsCCTrans
            Dim oCCtransApply As New clsCCTransApplyTo
            Dim oCombo As New clsComboItems
            For Each row As GridViewRow In gvTransactions.Rows
                Dim cbApprove As CheckBox = row.FindControl("cbApprove")
                Dim cbDeny As CheckBox = row.FindControl("cbDeny")
                If cbDeny.Checked Then
                    oCCTrans.CCTransID = row.Cells(0).Text
                    oCCTrans.Load()
                    oCCTrans.UserID = Session("UserDBID")
                    oCCTrans.Approved = -1
                    oCCTrans.ApprovedBy = Session("UserDBID")
                    oCCTrans.DateApproved = System.DateTime.Now
                    oCCTrans.Save()
                    Dim sAcct(1) As String
                    sAcct = row.Cells(2).Text.Split(":")
                    If UCase(sAcct(0)) = "PACKAGEISSUEDID" Then
                        Dim oPkgIss As New clsPackageIssued
                        oPkgIss.UserID = Session("UserDBID")
                        oPkgIss.CXL_Package(Trim(sAcct(1)))
                        oPkgIss.PackageIssuedID = Trim(sAcct(1))
                        oPkgIss.Load()
                        oPkgIss.StatusID = oCombo.Lookup_ID("PackageStatus", "Kicked")
                        oPkgIss.Save()
                        oPkgIss = Nothing
                    End If
                ElseIf cbApprove.Checked Then
                    oCCTrans.CCTransID = row.Cells(0).text
                    oCCTrans.Load()
                    oCCTrans.UserID = Session("UserDBID")
                    oCCtransApply.UserID = oCCTrans.CreatedByID 'Session("UserDBID")
                    If oCCTrans.Approved = 0 Then
                        If oCCtransApply.Create_Payment_Items(row.Cells(0).Text, oCCTrans.CreditCardID) Then
                            oCCTrans.Approved = 1
                            oCCTrans.ApprovedBy = Session("UserDBID")
                            oCCTrans.DateApproved = System.DateTime.Now
                            oCCTrans.Save()
                        Else
                            lblErr.Text = oCCtransApply.Err
                        End If
                    End If
                End If
            Next
            If lblErr.Text = "" Then
                Response.Redirect("CCApprovals.aspx")
            End If
            oCombo = Nothing
            oCCTrans = Nothing
            oCCtransApply = Nothing
        End If

    End Sub

    Protected Sub lbDetails_Click(sender As Object, e As System.EventArgs)
        ClientScript.RegisterClientScriptBlock(Me.GetType, "Alert", "modal.mwindow.open('" & Request.ApplicationPath & "/Accounting/ApprovalBreakout.aspx?Field=CCTrans&ID=" & CType(sender.Parent.parent, GridViewRow).Cells(0).Text() & "', 'win01',450,450);", True)
    End Sub

End Class
