
Partial Class general_EditInvoiceAdjustment
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not (IsPostBack) Then
            Dim oInv As New clsInvoices
            oInv.InvoiceID = Request("InvoiceID")
            oInv.Load()

            If (Session("UserDBID") = oInv.UserID And DateDiff(DateInterval.Hour, CDate(oInv.TransDate), Date.Now) < 24 Or CheckSecurity("Payments", "EditAdjustments", , , Session("UserDBID"))) Then

                gvInvoices.DataSource = oInv.List_Invoices
                gvInvoices.DataBind()

                ddAdjustments.DataSource = oInv.List_Invoice_Adjustments()
                ddAdjustments.DataTextField = "Adjustment"
                ddAdjustments.DataValueField = "ID"
                ddAdjustments.DataBind()

                Dim tItem As ListItem = ddAdjustments.Items.FindByValue(oInv.PaymentMethodID)
                ddAdjustments.SelectedIndex = ddAdjustments.Items.IndexOf(tItem)
                For i = 0 To gvInvoices.Rows.Count - 1
                    If gvInvoices.Rows(i).Cells(1).Text = CStr(oInv.ApplyToID) Then
                        Dim c As CheckBox = gvInvoices.Rows(i).FindControl("cb")
                        c.Checked = True
                        Exit For
                    End If
                Next
                txtAmount.Text = oInv.Amount
                txtDescription.Text = oInv.Description
                rbNeg.Checked = oInv.PosNeg
                rbPos.Checked = Not (oInv.PosNeg)
                hfInvoiceID.Value = oInv.ApplyToID

            Else
                Button1.Enabled = False
                ClientScript.RegisterClientScriptBlock(Me.GetType, "Access", "alert('Access Denied');", True)
                ClientScript.RegisterClientScriptBlock(Me.GetType, "close", "window.close();", True)
            End If
            
            oInv = Nothing

        Else
            lblErr.Text = sender.GetType.ToString
        End If
    End Sub

    Protected Sub gvInvoices_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        e.Row.Cells(7).Visible = False

    End Sub

    Protected Sub Unnamed1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim amt_Selected As Decimal = 0
        Dim amt_Entered As String = ""
        amt_Entered = txtAmount.Text
        Dim strMA As String = "0"
        If Not (Verify_Trans()) Then
            'Return(False)
            Exit Sub
        End If
        If amt_Entered = "" Or Not (IsNumeric(amt_Entered)) Then
            lblErr.Text = "Please enter a valid amount"
            'Return False
            Exit Sub
        End If
        If hfInvoiceID.Value = "0" Then
            lblErr.Text = "Please Select An Invoice To Adjust."
            Exit Sub
        End If
        Dim paymentID As Integer = 0
        For i = 0 To gvInvoices.Rows.Count - 1
            If gvInvoices.Rows(i).Cells(1).Text = hfInvoiceID.Value Then 'Request.Form("rbGroup") Then
                paymentID = Create_Inv_Adjustment(ddAdjustments.SelectedItem.Text, amt_Entered, rbNeg.Checked, gvInvoices.Rows(i).Cells(1).Text)
            End If
        Next
        If paymentID = 0 Then
            lblErr.Text = "Please Select an Invoice To Adjust."
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType, "refresh", "window.opener.__doPostBack('" & Request("linkid") & "','');window.close();", True)
        End If
    End Sub

    Private Function Create_Inv_Adjustment(ByVal sMethod As String, ByVal sAmount As String, ByVal bPosNeg As Boolean, ByVal applyTo As Integer) As Integer
        Dim iRet As Integer = 0
        Dim oPay As New clsInvoices
        Dim oInvoice As New clsInvoices
        Dim oCombo As New clsComboItems
        Try
            oInvoice.InvoiceID = applyTo
            oInvoice.Load()
            oPay.InvoiceID = Request("InvoiceID")
            oPay.Load()
            oPay.PaymentMethodID = oCombo.Lookup_ID("InvoiceAdjustments", sMethod)
            oPay.Adjustment = True
            oPay.PosNeg = bPosNeg
            oPay.Amount = sAmount
            oPay.ProspectID = oInvoice.ProspectID 'IIf(IsNumeric(Request("ProspectID")), Request("Prospectid"), 0)
            oPay.KeyField = oInvoice.KeyField 'Request("KeyField")
            oPay.KeyValue = oInvoice.KeyValue 'Request("KeyValue")
            oPay.ApplyToID = applyTo
            oPay.UserID = oPay.UserID 'Session("UserDBID")
            oPay.TransDate = Date.Now
            oPay.Save()
            iRet = oPay.InvoiceID
        Catch ex As Exception
            lblErr.Text = ex.ToString
        End Try
        oInvoice = Nothing
        oPay = Nothing
        oCombo = Nothing
        Return iRet
    End Function

    Private Function Verify_Trans() As Boolean
        If Not (rbNeg.Checked Or rbPos.Checked) Then
            lblErr.Text = "Please select either Positive or Negative"
            Return False
        ElseIf Not (IsNumeric(txtAmount.Text)) Or txtAmount.Text = "" Then
            lblErr.Text = "Please enter an Amount"
            Return False
        ElseIf txtDescription.Text = "" Then
            lblErr.Text = "Please enter a description"
            Return False
        Else
            Return True
        End If
    End Function

    Private Function Verify_Amounts(ByRef strMA As String, ByRef amt_Selected As Decimal, ByVal amt_Entered As String, ByRef autoApprove As Boolean) As Boolean
        'Get_Amount selected and the ids
        For i = 0 To gvInvoices.Rows.Count - 1
            If CType(gvInvoices.Rows(i).FindControl("rb"), RadioButton).Checked Then
                If strMA = "0" Then
                    strMA = gvInvoices.Rows(i).Cells(2).Text
                    autoApprove = CBool(gvInvoices.Rows(i).Cells(7).Text)
                End If
                If strMA <> gvInvoices.Rows(i).Cells(2).Text Then
                    lblErr.Text = "Please select invoices on the same merchant account."
                    Return False
                    Exit Function
                End If
                amt_Selected += CDec(gvInvoices.Rows(i).Cells(6).Text)
            End If
        Next

        If amt_Selected <= 0 Then
            lblErr.Text = "Please select an Invoice with a balance greater than $0"
            Return False
        ElseIf amt_Selected < CDec(amt_Entered) Then
            lblErr.Text = "Please lower the amount to charge or select more invoices"
            Return False
        Else
            Return True
        End If
    End Function

    Protected Sub cb_CheckedChanged(sender As Object, e As System.EventArgs)
        ' Dim x As New RadioButton
        'lblErr.Text = e.GetType.ToString
        hfInvoiceID.Value = CType(sender.Parent.parent, GridViewRow).Cells(1).Text
        For Each i As Control In Me.Controls
            'lblErr.Text &= "<br />" & i.GetType.ToString
            If TypeOf i Is HtmlForm Then
                For Each x As Control In i.Controls
                    If TypeOf x Is GridView Then
                        For Each y As Control In x.Controls
                            For Each z As Control In y.Controls
                                For Each f As Control In z.Controls
                                    If f.HasControls Then
                                        For Each r As Control In f.Controls
                                            If TypeOf r Is RadioButton And r IsNot sender Then
                                                If r.ID = "cb" Then
                                                    CType(r, RadioButton).Checked = False

                                                End If
                                            ElseIf r Is sender Then
                                                lblErr.Text = CType(sender.Parent.parent, GridViewRow).Cells(1).Text
                                            End If
                                        Next
                                    End If
                                Next
                            Next
                        Next
                    End If
                Next
            End If
        Next
    End Sub
End Class
