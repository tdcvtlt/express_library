
Partial Class Add_Ins_VendorCheckPayout
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            ddInvoices.Items.Add("Expedia Billing")
            ddInvoices.Items.Add("Leisure Link Billing")
            ddInvoices.Items.Add("Orbitz Billing")
            ddInvoices.Items.Add("Plan With Tan")
        End If
    End Sub

    Protected Sub Button1_Click(sender As Object, e As System.EventArgs) Handles Button1.Click
        If dteStartDate.Selected_date.ToString = "" Or dteEndDate.Selected_Date.ToString = "" Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('Please Select a Start And End Date.');", True)
        ElseIf txtCheckNum.Text = "" Then
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('Please Enter a Check Number.');", True)
        Else
            Dim oFinancials As New clsFinancials
            gvInvoices.DataSource = oFinancials.Get_Vendor_Check_Payout_Invoices(dteStartDate.Selected_Date, dteEndDate.Selected_Date, ddInvoices.SelectedValue)
            gvInvoices.DataBind()
            gvInvoices.Visible = True
            oFinancials = Nothing
            btnSubmit.Visible = True
            lblErr.Text = ""
        End If
    End Sub

    Protected Sub Unnamed1_Click(sender As Object, e As System.EventArgs)
        Dim bProceed As Boolean = True
        Dim sErr As String = ""
        If txtCheckNum.Text = "" Then
            bProceed = False
            sErr = "Please Enter a Check Number."
        End If
        If bProceed Then
            For Each row As GridViewRow In gvInvoices.Rows
                Dim cb As CheckBox = row.FindControl("cb")
                If cb.Checked Then
                    Dim tb As TextBox = row.FindControl("txtAmt")
                    If tb.Text = "" Or Not (IsNumeric(tb.Text)) Then
                        bProceed = False
                        sErr = "Please Enter a Valid Check Amount."
                        Exit For
                    ElseIf CDbl(tb.Text) > CDbl(row.Cells(5).Text) Then
                        bProceed = False
                        sErr = "Amount Can Not Be Greater Than Balance."
                        Exit For
                    End If
                End If
            Next

            If bProceed Then
                Dim oCombo As New clsComboItems
                Dim oPayment As New clsPayments
                Dim oPmt2Invoice As New clsPayment2Invoice
                Dim pmtID As Integer = 0
                For Each row As GridViewRow In gvInvoices.Rows
                    Dim cb As CheckBox = row.FindControl("cb")
                    If cb.Checked Then
                        Dim tb As TextBox = row.FindControl("txtAmt")
                        oPayment.PaymentID = 0
                        oPayment.Load()
                        oPayment.Amount = tb.Text
                        oPayment.MethodID = oCombo.Lookup_ID("PaymentMethod", "Check")
                        oPayment.Reference = "Check-" & txtCheckNum.Text
                        oPayment.Description = "Check Payment"
                        oPayment.UserID = Session("UserDBID")
                        oPayment.TransDate = System.DateTime.Now.ToShortDateString
                        oPayment.PosNeg = True
                        oPayment.Save()
                        pmtID = oPayment.PaymentID
                        oPmt2Invoice.Inv2PayID = 0
                        oPmt2Invoice.Load()
                        oPmt2Invoice.InvoiceID = row.Cells(1).Text
                        oPmt2Invoice.PaymentID = pmtID
                        oPmt2Invoice.Amount = tb.Text
                        oPmt2Invoice.PosNeg = True
                        oPmt2Invoice.Save()
                    End If
                Next
                oCombo = Nothing
                oPayment = Nothing
                oPmt2Invoice = Nothing
                gvInvoices.Visible = False
                lblErr.Text = "Checks Processed."
                btnSubmit.Visible = False
            Else
                ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('" & sErr & "');", True)
            End If
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "AjaxCall", "alert('" & sErr & "');", True)
        End If
    End Sub

    Protected Sub gvInvoices_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.Cells(0).Text <> "No Invoices" Then
            If e.Row.RowIndex > -1 Then
                e.Row.Cells(3).Text = CDate(e.Row.Cells(3).Text & "").ToShortDateString
                e.Row.Cells(4).Text = CDate(e.Row.Cells(4).Text & "").ToShortDateString
            End If
            e.Row.Cells(1).Visible = False
        End If
    End Sub

End Class
